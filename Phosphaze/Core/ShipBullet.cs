using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core.Dml;
using Phosphaze.Core.Dml.Builtins;
using Phosphaze.Core.Dml.Parse;

namespace Phosphaze.Core
{
    public class ShipBullet
    {
        public Vector2 position;
        const float speed=10;
        Texture2D texture;
        

        public ShipBullet(Vector2 p)
        {
            position = p;
            texture = Globals.content.Load<Texture2D>("shipbullet");
        }
        public void Update()
        {
            position.Y -= speed;
        }
        public void Draw()
        {
            Globals.spriteBatch.Draw(texture, new Vector2(position.X-texture.Width/2,position.Y-texture.Height/2), Color.White);
        }
    }
}
