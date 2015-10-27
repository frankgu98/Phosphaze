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
    /// A class that represents an Animation.
    /// </summary>
    public class Animation
    {

        // The spritesheet from which to draw.
        public Texture2D Spritesheet { get; private set; }

        // The different cells of the spritesheet.
        public Rectangle[] Cells { get; private set; }

        // The mapping of state name to animation state.
        public Dictionary<string, AnimationState> States { get; private set; }

        // The current state name.
        private string current_state;

        /// <summary>
        /// Construct a new Animation given the name of an AniML file.
        /// </summary>
        /// <param name="filename"></param>
        public Animation(string filename)
        {
            Animation parsed = AniMLReader.ParseAniMLFile(filename);
            _initialize(parsed.Spritesheet, parsed.Cells, parsed.States, parsed.GetCurrentState());
        }

        /// <summary>
        /// Construct a new Animation given the explicit spritesheet, animation states, and default state.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="cells"></param>
        /// <param name="states"></param>
        /// <param name="default_state"></param>
        public Animation(
            Texture2D texture, Rectangle[] cells, 
            Dictionary<string, AnimationState> states, 
            string default_state)
        {
            _initialize(texture, cells, states, default_state);
        }

        /// <summary>
        /// Shared internal initialization.
        /// </summary>
        private void _initialize(
            Texture2D texture, Rectangle[] cells, 
            Dictionary<string, AnimationState> states, 
            string default_state)
        {
            this.Spritesheet = texture;
            this.States = states;
            current_state = default_state;
        }

        /// <summary>
        /// Return the name of the current animation state.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentState()
        {
            return current_state;
        }

        /// <summary>
        /// Sets the current animation state to the one with the given name.
        /// </summary>
        /// <param name="name"></param>
        public void SetCurrentState(string name)
        {
            if (!States.ContainsKey(name))
                throw new AniMLException("\"" + name + "\" is not a valid animation state.");
            current_state = name;
        }

        /// <summary>
        /// Render the animation to the given target.
        /// </summary>
        /// <param name="target"></param>
        public void Draw(RenderTarget2D target)
        {
            // NOT YET IMPLEMENTED
        }

    }
}
