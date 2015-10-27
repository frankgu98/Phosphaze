//Frank Gu

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Phosphaze.Core.SceneElements
{
    public class SceneStar
    {

        public float speed { get; set; }
        public float ang { get; set; }

        //it's slower without saving these values in memory
        float cos, sin;
        float scale;
        int leeway;

        Vector2 pos;

        Texture2D texture;

        //makes a SceneStar with random starting position and direction
        public SceneStar()
        {
            texture = Globals.content.Load<Texture2D>("StarsContent/star3");

            pos = new Vector2(Globals.randGen.Next(0, Options.Resolutions.X),
                              Globals.randGen.Next(0, Options.Resolutions.Y));

            ang = ToRad(Globals.randGen.Next(-3, 3));
            float random = 0.4f * ((float)Globals.randGen.NextDouble() + .4f);
            cos = (float)Math.Cos(ang) * random;
            sin = (float)Math.Sin(ang) * random;
            speed = random;

            scale = (random) / 0.56f * .65f;
            leeway = 20;
        }


        public void Initialize()
        {

        }

        /// <summary>
        /// Updates this SceneStar
        /// </summary>
        public void Update()
        {
            //makes sure the star stays onscreen no matter which side it leaves by (since -1%2 returns -1)
            //also gives a 5 pixel buffer for stars to go completely offscreen before appearing on the opposite side
            pos.X = pos.X + speed * cos;
            pos.X = -leeway<pos.X ? pos.X : Options.Resolutions.X + pos.X;
            pos.X = pos.X < Options.Resolutions.X + leeway ? pos.X : -leeway;
            pos.Y = -leeway < pos.Y ? pos.Y : Options.Resolutions.Y + pos.Y;
            pos.Y = pos.Y < Options.Resolutions.Y + leeway ? pos.Y : -leeway;
        }

        /// <summary>
        /// Draws this SceneStar
        /// </summary>
        public void Draw()
        {
            Globals.spriteBatch.Draw(texture, pos, null, Color.White, 60.167f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        private float ToRad(float deg)
        {
            return (float)(Math.PI * deg / 180);
        }
    }
}

