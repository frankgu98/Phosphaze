using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core;
using Phosphaze.Core.Collision;
namespace Phosphaze.Core
{
    public class SceneSlider
    {
        private double scale;
        public Rectangle back;
        public Rectangle bar;//public so you can start the bar wherever you want

        private Texture2D texture;

        private int lastOffset;
        private int Scrolls = 0;
        private int distScrolled = 0;
        public bool scrollable = false;

        public string name{get;set;}
        public bool pressed;
        public ICollidable CollisionMask;
        public SceneSlider(string n, Rectangle rect, double scale, double startPercentage)//scale = back rect/moving rect
        {
            name = n;
            back = rect;
            bar = new Rectangle(back.X, back.Y + 1, back.Width / (int)scale - 2, back.Height - 2);
            bar.X += (int)((back.Width - bar.Width) * startPercentage);//puts the bar in a spot reflecting the startpercentage
            lastOffset = 0;
            
            if (scale < 1) { scale = 1; }
            this.scale = scale;

            texture = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.White });
            
            pressed = false;
            CollisionMask = new RectCollider(bar);
        }

        public SceneSlider(Rectangle rect, double scale)//scale=size of moving surface/screen
        {
            back = rect;
            bar = new Rectangle(back.X + Options.Resolutions.X / 480, back.Y + Options.Resolutions.Y / 380, (int)(back.Width / scale - Options.Resolutions.X / 240), back.Height - Options.Resolutions.Y / 190);
            if (scale < 1) { scale = 1; }
            this.scale = scale;
            texture = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData<Color>(new Color[] { Color.White });
        }

        public void Update()
        {
            if (CollisionChecker.Collision(new RectCollider(bar),
            new ParticleCollider(Globals.currentMouseState.Position)) &&
            Globals.currentMouseState.LeftButton == ButtonState.Pressed &&
            Globals.previousMouseState.LeftButton == ButtonState.Released)
            {
                pressed = true;
            }
            if (pressed)
            {
                if (Globals.currentMouseState.LeftButton == ButtonState.Released)
                {
                    pressed = false;
                }
                int pos = Globals.currentMouseState.Position.X;
                bar.X = pos - bar.Width / 2;
            }
            int curScrolls;
            if (scrollable)//uses mouse wheel to move screen
            {
                curScrolls = Globals.currentMouseState.ScrollWheelValue;
                if ((curScrolls - Scrolls) / 5 > 0)
                {
                    distScrolled += (int)(700 / scale);
                    Scrolls += 10;
                }
                else if ((curScrolls - Scrolls) / 3 < 0)
                {
                    distScrolled -= (int)(700 / scale);
                    Scrolls -= 10;
                }
                else
                {
                    Scrolls = curScrolls;
                }
                Scrolls = curScrolls;
            }
            if (distScrolled < 10 && distScrolled > -10) distScrolled = 0;
            if (distScrolled > 0)
            {
                bar.X += 10;
                distScrolled -= 10;
            }
            else if (distScrolled < 0)
            {
                bar.X -= 10;
                distScrolled += 10;
            }
            if (bar.X + bar.Width > back.X + back.Width)
            {
                distScrolled = 0;
                bar.X = back.X + back.Width - bar.Width;
            }//doesn't go out of bounds
            if (bar.X < back.X)
            {
                distScrolled = 0;
                bar.X = back.X;
            }
        }
        public void Draw()
        {
            Globals.spriteBatch.Draw(texture, back, new Color(111, 111, 111, 20));
            if (pressed) 
            { 
                Globals.spriteBatch.Draw(texture, bar, Color.White); 
            }
            else 
            {
                Globals.spriteBatch.Draw(texture, bar, new Color(50, 50, 50)); 
            }
        }
        public void Draw(Rectangle backsize,Rectangle barsize)
        {
            Globals.spriteBatch.Draw(texture, backsize, new Color(111, 111, 111, 20));
            if (pressed)
            {
                Globals.spriteBatch.Draw(texture, barsize, Color.White);
            }
            else
            {
                Globals.spriteBatch.Draw(texture, barsize, new Color(50, 50, 50));
            }
        }

        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            Globals.spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            Globals.spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            Globals.spriteBatch.Draw(texture, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            Globals.spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);
        }

        public int ScaledMove()//equivalent position for the screen used to create it(assuming start at 0,0)
        {
            int Offset = (bar.X - 1) * (int)scale - lastOffset;
            lastOffset = (bar.X - 1) * (int)scale;
            return Offset;
        }

        //how far the bar is into the larger rect
        public float GetPercentage()
        {
            return (bar.X - back.X) / (float)(back.Width - bar.Width);
        }
    }
}