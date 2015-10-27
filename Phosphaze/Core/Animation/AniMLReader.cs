#region License

// Copyright (c) 2015 FCDM
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation the 
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is furnished 
// to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

// AUTHOR: Michael Ala

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core;
using Phosphaze.Core.Collision;
using Phosphaze.Core.SceneElements;

namespace Phosphaze.Core.Animation
{
    /// <summary>
    /// Read an AniML file.
    /// </summary>
    public static class AniMLReader
    {

        /// <summary>
        /// Parse a given AniML file into an Animation object.
        /// </summary>
        /// <param name="file_name"></param>
        /// <returns></returns>
        public static Animation ParseAniMLFile(string file_name)
        {
            XmlDocument document = new XmlDocument();
            document.Load(file_name);

            XmlElement root = document.DocumentElement;

            // Get the spritesheet.
            XmlElement spritesheet_element = root.SelectSingleNode("spritesheet") as XmlElement;
            string spritesheet_name = spritesheet_element.Value;
            Texture2D spritesheet = Globals.content.Load<Texture2D>(spritesheet_name);

            // Advance to the cells.
            Rectangle[] cells = HandleCellDivision(root, spritesheet);

            var animation_states = GetAnimationStates(root);

            XmlElement default_state_element = root.SelectSingleNode("defaultState") as XmlElement;
            string default_state = default_state_element.Value;
            if (!animation_states.ContainsKey(default_state))
                throw new AniMLException("The given ``defaultState`` matches no defined animation states.");

            return new Animation(spritesheet, cells, animation_states, default_state);
        }

        /// <summary>
        /// Divide the spritesheet into cells.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="spritesheet"></param>
        /// <returns></returns>
        private static Rectangle[] HandleCellDivision(XmlElement root, Texture2D spritesheet)
        {
            int cell_width, cell_height;
            if (!Int32.TryParse((root.SelectSingleNode("width") as XmlElement).Value, out cell_width) ||
                !Int32.TryParse((root.SelectSingleNode("height") as XmlElement).Value, out cell_height))
                throw new AniMLException("``width`` and ``height`` elements must both be defined.");

            Rectangle[] cells = new Rectangle[(int)((spritesheet.Width % cell_width) * (spritesheet.Height % cell_height))];

            int crnt_x = 0, crnt_y = 0;
            int i = 0;

            // Partition the spritesheet up into a grid.
            while (crnt_y <= spritesheet.Height)
            {
                while (crnt_x <= spritesheet.Width)
                {
                    cells[i] = new Rectangle(crnt_x, crnt_y, cell_width, cell_height);
                    crnt_x += cell_width;                 
                }
                crnt_x = 0;
                crnt_y += cell_height;
            }
            
            return cells;
        }

        /// <summary>
        /// Get all the animation states defined in the AniML file.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private static Dictionary<string, AnimationState> GetAnimationStates(XmlElement root)
        {
            var states = new Dictionary<string, AnimationState>();
            XmlNodeList state_elements = root.GetElementsByTagName("animationState");
            foreach (XmlNode node in state_elements)
            {
                XmlElement current_element = node as XmlElement;
                AnimationState next_state = ParseAnimationState(current_element);
                states.Add(current_element.GetAttribute("name"), next_state);
            }
            return states;
        }

        /// <summary>
        /// Parse a single animation state.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private static AnimationState ParseAnimationState(XmlElement state)
        {
            // Get the default frame interval (0 by default).
            XmlElement frame_interval_element = state.SelectSingleNode("defaultInterval") as XmlElement;
            int default_frame_interval = 0;
            // Check if it's non-null, then try to parse it to an integer and set default_frame_interval to the result. Throw
            // an AniMLException if parsing failed.
            if (frame_interval_element != null && !Int32.TryParse(frame_interval_element.Value, out default_frame_interval))
                throw new AniMLException("``defaultInterval`` not an integer.");

            XmlNodeList nodes = state.ChildNodes;

            List<int> cells = new List<int>();
            List<int> waits = new List<int>();

            bool expecting_wait = false;

            foreach (XmlNode node in nodes)
            {
                // Figure out what to do based on the current node.
                switch (node.Name)
                {
                    case "cell":
                        // ``cell`` indicates a cell index to add to the list of cells.
                        // It also weakly implies that the next element should be a ``wait``
                        // element. If not, it just uses the default interval.
                        if (expecting_wait)
                            waits.Add(default_frame_interval);
                        expecting_wait = true;
                        cells.Add(Int32.Parse(node.InnerText));
                        break;
                    case "wait":
                        // ``wait`` indicates a wait time (in frames, as in FPS) after a
                        // cell element.
                        if (!expecting_wait)
                            // Compound adjacent wait times.
                            waits[waits.Count] += Int32.Parse(node.InnerText);
                        else
                        {
                            expecting_wait = false;
                            waits.Add(Int32.Parse(node.InnerText));
                        }
                        break;
                    case "cellRange":
                        // ``cellRange`` indicates to add all the cell indicies in a given range.
                        XmlElement element = node as XmlElement;
                        if (!element.IsEmpty)
                            throw new AniMLException("``cellRange` elements must be empty.");

                        // The starting cell index.
                        int start = Int32.Parse(element.GetAttribute("start"));

                        // The final cell index.
                        int stop = Int32.Parse(element.GetAttribute("end"));

                        // The stepsize by which the cell indices increase.
                        int step = element.HasAttribute("step") ? Int32.Parse(element.GetAttribute("step")) : 1;
                        if (step <= 0)
                            throw new AniMLException("Invalid step size.");

                        // The wait time to add after every cell.
                        int wait = element.HasAttribute("wait") ? 
                                   Int32.Parse(element.GetAttribute("wait")) : 
                                   default_frame_interval;

                        for (int i = start; i < stop; i += step)
                        {
                            cells.Add(i);
                            waits.Add(wait);
                        }

                        expecting_wait = false;
                        break;
                    case "defaultInterval":
                        // Already parsed, just skip.
                        break;
                    default:
                        throw new AniMLException("Unknown AniML element " + node.Name + ".");
                }
            }

            // Get the repeat type.
            AnimationState.RepeatTypes repeat_type;
            XmlElement repeat_type_node = state.SelectSingleNode("repeatType") as XmlElement;
            if (repeat_type_node == null)
                repeat_type = AnimationState.RepeatTypes.REPEAT;
            switch (repeat_type_node.Value)
            {
                case "REPEAT":
                    repeat_type = AnimationState.RepeatTypes.REPEAT;
                    break;
                case "REVERSE":
                    repeat_type = AnimationState.RepeatTypes.REVERSE;
                    break;
                case "ONCE":
                    repeat_type = AnimationState.RepeatTypes.ONCE;
                    break;
                default:
                    throw new AniMLException("``repeatType`` element must contain either REPEAT, REVERSE, or ONCE.");
            }   

            return new AnimationState(cells.ToArray(), waits.ToArray(), repeat_type);
        }
    }
}
