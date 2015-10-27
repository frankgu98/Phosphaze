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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Phosphaze.Core
{
    /// <summary>
    /// The base interface for scene objects.
    /// </summary>
    public abstract class Scene
    {
        public Texture2D texture = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
        // The SceneManager that oversees this scene.
        protected SceneManager manager;

        protected double LocalTime { get; set; }

        //scenemanager needs to use this so it's public
        public bool focused { get; set; }

        /// <summary>
        /// Assign a SceneManager to this scene.
        /// </summary>
        /// <param name="manager">The SceneManager.</param>
        public void SetManager(SceneManager manager)
        {
            this.manager = manager;
        }



        /// <summary>
        /// Perform any scene specific initialization.
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// Update this scene.
        /// </summary>
        /// <param name="game">The global game time.</param>
        /// 
        //scenes are going to need base.update
        public virtual void Update()
        {
            LocalTime += Globals.deltaTime;
        }

        //draws scenes
        public abstract void Draw();


        /// <summary>
        /// Returns true once at the given time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool At(double time)
        {
            return From(time, time + Globals.deltaTime);
        }

        /// <summary>
        /// Returns true when the local time is in the given interval.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool From(double start, double end)
        {
            return start <= LocalTime && LocalTime <= end;
        }


        /// <summary>
        /// Returns true when the local time is before the given time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Before(double time)
        {
            return LocalTime < time;
        }

        /// <summary>
        /// Returns true when the local time is after the given time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool After(double time)
        {
            return LocalTime > time;
        }

        /// <summary>
        /// Returns true once at given intervals in time.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public bool AtIntervals(double interval, double start = 0, double end = Double.PositiveInfinity)
        {
            double modded = LocalTime % interval;
            return 0 <= modded && modded < Globals.deltaTime &&
                   start <= LocalTime && LocalTime <= end;
        }



        public bool DuringIntervals(double interval, double start = 0, double end = Double.PositiveInfinity)
        {
            return ((LocalTime - start) % (2 * interval)) < interval && start <= LocalTime && LocalTime <= end;
        }
    }
}
