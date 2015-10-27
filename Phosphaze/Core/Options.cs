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
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Phosphaze.Core.Utils;

namespace Phosphaze.Core
{
    /// <summary>
    /// The game options.
    /// </summary>
    public static class Options
    {

        public static string SongFolder;

        // The number of seconds delay between when the player
        // enters a level and when the bullets actually start
        // spawning.
        public static double SongOffset = 2.5;

        private static float _bgdim = 0.5f;

        // The background dim amount (ranges from 0 to 1).
        public static float BackgroundDim
        {
            get { return _bgdim; }
            set
            {
                if (0 <= value && value <= 1)
                {
                    _bgdim = value;
                }
            }
        }

        // Whether or not the keyboard is used for controls, or the mouse.
        // If true, then the keyboard controls the player, otherwise the
        // mouse controls the player. This does not override the dual
        // functionality of the keyboard and the mouse in menu scenes.
        public static bool KeyboardControls = true;


        /// <summary>
        /// The various volume sliders (clamped between 0 and 1 inclusive).
        /// </summary>
        public static class Volumes
        {

            private static float _gbvol = 0.5f;
            private static float _sfxvol = 0.5f;
            private static float _mscvol = 0.5f;

            // The global volume (from 0 to 1).
            public static float GlobalVolume
            {
                get { return _gbvol; }
                set
                {
                    if (0 <= value && value <= 1)
                    {
                        _gbvol = value;
                    }
                }
            }

            // The volume of the sound effects (from 0 to 1).
            public static float SoundFXVolume
            {
                get { return _sfxvol; }
                set
                {
                    if (0 <= value && value <= 1)
                    {
                        _sfxvol = value;
                    }
                }
            }

            // The volume of the music (from 0 to 1).
            public static float MusicVolume
            {
                get { return _mscvol; }
                set
                {
                    if (0 <= value && value <= 1)
                    {
                        _mscvol = value;
                    }
                }
            }
        }

        /// <summary>
        /// These are the keybindings for various commands. This allows
        /// users to set what keys they want to use, which helps if, for
        /// example, the user isn't using a QWERTY keyboard.
        /// </summary>
        public static class Keybindings
        {

            // The key that corresponds to the LEFT action. This action
            // moves the player and navigates the menus.
            public static Keys LEFT = Keys.Left;
            // Alternate LEFT key. This allows the user to use arrow keys
            // or WASD simultaneously. Both the ordinary and alternate keys
            // can be rebound in the Global Options menu.
            public static Keys LEFT_ALT = Keys.A;

            // The key that corresponds to the RIGHT action. This action
            // moves the player and navigates the menus.
            public static Keys RIGHT = Keys.Right;
            // Alternate RIGHT key.
            public static Keys RIGHT_ALT = Keys.D;

            // The key that corresponds to the UP action. This action
            // moves the player and navigates the menus.
            public static Keys UP = Keys.Up;
            // Alternate UP key.
            public static Keys UP_ALT = Keys.W;

            // The key that corresponds to the DOWN action. This action
            // moves the player and navigates the menus.
            public static Keys DOWN = Keys.Down;
            // Alternate DOWN key.
            public static Keys DOWN_ALT = Keys.S;

            // The key that corresponds to the SELECT action. This action
            // selects things in the menus.
            public static Keys SELECT = Keys.Enter;

            // The key that corresponds to the ESCAPE action. This action
            // exits scenes to previous ones.
            public static Keys ESCAPE = Keys.Escape;
        }

        /// <summary>
        /// Helper class in case we want to do something more complicated
        /// with screen resolutions.
        /// </summary>
        public static class Resolutions
        {
            // The index of the current dimensions we're using.
            static int current = 0;

            // The supported dimensions 
            public static readonly Vector2[] supported_dimensions = {
                                                                 new Vector2(1280f, 720f)
                                                             };

            public static int X { get { return (int)supported_dimensions[current].X; } }
            public static int Y { get { return (int)supported_dimensions[current].Y; } }
        }
    }
}
