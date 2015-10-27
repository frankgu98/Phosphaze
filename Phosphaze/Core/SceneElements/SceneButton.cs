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

// AUTHOR: Frank Gu

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core;
using Phosphaze.Core.Collision;
using Phosphaze.Core.Utils;
using Phosphaze.Core.SceneElements;

namespace Phosphaze.Core.SceneElements
{
    public class SceneButton
    {

        //alternate sprite and drawpos

        //name of button
        public string name { get; set; }

        // The collider for this button.
        public ICollidable CollisionMask { get; private set; }

        //if the button can be selected (false means selectable)
        public bool locked { get; private set; }

        // Whether or not the button is selected.
        public bool selected { get; set; }


        // The button texture to render.
        public Texture2D texture { get; protected set; } // Protected in case we want to make

        Vector2 drawPos;

        public SceneButton() { }

        public SceneButton(string n,string texture_name, Vector2 topleft)
        {
            // Constructor edited by: Michael Ala
            name = n;
            locked = false;
            selected = false;

            // Initialize RectCollider
            texture = Globals.content.Load<Texture2D>(texture_name);
            CollisionMask = new RectCollider(topleft.X, topleft.Y, texture.Width, texture.Height);
        }

        public SceneButton(string n, string texture_name, Vector2 topleft, Rectangle rect)
        {
            name = n;
            locked = false;
            selected = false;

            texture = Globals.content.Load<Texture2D>(texture_name);
            CollisionMask = new RectCollider(rect);
        }


        //shifts the button to centre on a point
        public void CentreOnPoint(float x, float y)
        {
            Rectangle bb = CollisionMask.GetBoundingBox();
            Vector2 delta = new Vector2();
            
            delta.X = x - bb.Width / 2;
            delta.Y = y - bb.Height / 2;
            CollisionMask.Translate(delta);
        }

        public void CentreOnPoint(Vector2 vec)
        {
            CentreOnPoint(vec.X, vec.Y);
        }
        
        //draws the buttons
        public void Draw()
        {
            Rectangle box = CollisionMask.GetBoundingBox();

            Globals.spriteBatch.Draw(texture, box, Color.White);


        }

        public void Draw(Rectangle size)
        {

            Globals.spriteBatch.Draw(texture, size, Color.White);


        }
        
        public void Draw(float rotation){
            
            Rectangle box = CollisionMask.GetBoundingBox();
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            box.X += box.Width / 2;
            box.Y += box.Height / 2;
            Globals.spriteBatch.Draw(texture, box, null, Color.White, rotation, origin, SpriteEffects.None, 0);
            
        }

        public void Update()
        {

        }

        public bool Collide(Vector2 pos)
        {

            return CollisionChecker.Collision(CollisionMask, new ParticleCollider(pos))&&!locked;
        
        }

        public bool Collide(Point pos)
        {
            
            return CollisionChecker.Collision(CollisionMask, new ParticleCollider(pos))&&!locked;
           
        }
    }
}
