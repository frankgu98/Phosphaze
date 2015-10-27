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

namespace Phosphaze.Core.Utils
{
    public static class InputUtils
    {

        /// <summary>
        /// Return the currently selected menu button based on the previous
        /// menu parameters.
        /// </summary>
        /// <param name="current_selected">The index of the currently selected button.</param>
        /// <param name="buttons">The array of buttons.</param>
        /// <param name="increment">The key that increments the selection.</param>
        /// <param name="decrement">The key that decrements the selection.</param>
        /// <param name="wrap">
        /// Determines what happens when the selection becomes < 0 or greater than the
        /// length of the array. If true, then the selection is modded by the length of
        /// the array. If false it is clamped.
        /// </param>
        /// <returns></returns>
        public static int? GetMenuSelection(
            int? current_selected, SceneButton[] buttons, 
            Keys increment, Keys decrement, 
            bool wrap = true, int default_selection = 0)
        {
            // Local aliases for previous and current mouse and keyboard states.
            MouseState pms = Globals.previousMouseState;
            MouseState cms = Globals.currentMouseState;

            KeyboardState pks = Globals.previousKeyboardState;
            KeyboardState cks = Globals.currentKeyboardState;

            // If the mouse states are valid, and the positions are different (implying
            // the mouse has moved).
            if (pms != null && cms.Position != pms.Position)
            {
                ParticleCollider mouse_pos = new ParticleCollider(cms.Position);
                // Find the first button that collides with the mouse.
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (CollisionChecker.Collision(buttons[i].CollisionMask, mouse_pos))
                        return i;
                }
            }
            // Else if the keyboard states are valid.
            else if (pks != null)
            {
                // If the increment key was pressed once.
                if (
                    cks.IsKeyDown(increment) &&
                    !pks.IsKeyDown(increment))
                {  
                    // Return the default value if current_selected == null.
                    if (!current_selected.HasValue)
                        return default_selection;
                    int new_selection = current_selected.Value - 1;
                    // Clamp or wrap the selection.
                    if (wrap)
                        return new_selection % buttons.Length;
                    return wrap ? new_selection % buttons.Length
                                : (int)MathHelper.Clamp(new_selection, 0, buttons.Length);
                }
                // If the decrement key was pressed once.
                else if (
                    cks.IsKeyDown(decrement) &&
                    !pks.IsKeyDown(decrement))
                {
                    // Return the default value if current_selected == null.
                    if (!current_selected.HasValue)
                        return default_selection;
                    int new_selection = current_selected.Value - 1;
                    // Clamp or wrap the selection.
                    if (wrap)
                        return new_selection % buttons.Length;
                    return wrap ? new_selection % buttons.Length 
                                : (int)MathHelper.Clamp(new_selection, 0, buttons.Length);
                }
            }
            // Return the current_selected value or null.
            return current_selected ?? null;
        }

    }
}
