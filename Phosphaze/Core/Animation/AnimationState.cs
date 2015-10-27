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
    /// A single animation state representing a sequence of cell indices
    /// and wait times.
    /// </summary>
    public class AnimationState
    {
        /// <summary>
        /// Valid RepeatTypes usable by an AnimationState instance.
        /// </summary>
        public enum RepeatTypes
        {
            REPEAT,
            REVERSE,
            ONCE
        }

        // The current cell index we're on.
        public int currentIndex { get; private set; }

        // The amount of time passed since the last time currentIndex changed.
        private int current_wait = 0;

        // The number of cells total.
        private int num_cells = 0;

        // The direction of increment (used if repeat_type == RepeatTypes.REVERSE).
        private int direction = 1;

        // The cell indices.
        private int[] cell_indices;

        // The wait times.
        private int[] wait_times;

        // Are we done?
        private bool done = false;

        // Determines what happens when the currentIndex reaches the end of the list of cell indices.
        private RepeatTypes repeat_type;

        /// <summary>
        /// Create a new AnimationState.
        /// </summary>
        /// <param name="cell_indices"></param>
        /// <param name="wait_times"></param>
        /// <param name="repeat_type"></param>
        public AnimationState(int[] cell_indices, int[] wait_times, RepeatTypes repeat_type)
        {
            // currentIndex always starts at 0.
            currentIndex = 0;
            // If we have more cells than wait times, fill in the remaining wait times
            // with zeros.
            if (cell_indices.Length > wait_times.Length)
            {
                int[] new_cells = new int[cell_indices.Length];
                wait_times.CopyTo(cell_indices, 0);
                cell_indices = new_cells;
            }
            this.cell_indices = cell_indices;
            this.wait_times = wait_times;
            this.repeat_type = repeat_type;
            // INTERNAL OPTIMIZATION: Store the number of cells.
            // This is probably a miniscule optimization.
            num_cells = cell_indices.Length;
        }

        /// <summary>
        /// Update this animation state. This will cause it to progress internally.
        /// </summary>
        public void Update()
        {
            // Do nothing if done (used by RepeatTypes.ONCE).
            if (done)
                return;
            current_wait++;
            // We've waited long enough, change the currentIndex!.
            if (current_wait > wait_times[currentIndex])
            {
                currentIndex += direction;
                current_wait = 0;
                // If the currentIndex is at the end/beginning of the list of cells, apply
                // the proper action depending on the given RepeatType.
                if (currentIndex == num_cells || currentIndex == -1)
                {
                    switch (repeat_type)
                    {
                        case RepeatTypes.ONCE:
                            currentIndex -= 1;
                            done = true;
                            break;
                        case RepeatTypes.REVERSE:
                            currentIndex -= direction;
                            direction *= -1;
                            break;
                        case RepeatTypes.REPEAT:
                            currentIndex = 0;
                            break;
                    }
                }
            }
        }

    }
}
