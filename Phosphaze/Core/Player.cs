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
    public class Player
    {
        public int points { get; set; }//how many points the player has earned
        public int bulletsHit { get; set; }//how many bullets the player hit on the "boss"
        public int bulletsTaken { get; set; }//how many times the player was hit by a bullet (while not immune)
        public int immuneTime { get; set; }

        Texture2D texture;
        public Vector2 position;//centre of ship
        double radius;//radius of ship's collision circle

        const float speed=12;
        //texture name, starting position, radisu of ship's hit circle, starting number of points for the ship
        public Player(string tname, Vector2 pos, double r, int startingPoints)
        {
            texture = Globals.content.Load<Texture2D>(tname); ;
            position = pos;
            radius = r;
            points = startingPoints;
        }

        public Player(string tname, Point pos, double r, int startingPoints)
        {
            texture = Globals.content.Load<Texture2D>(tname); ;
            position = new Vector2(pos.X,pos.Y);
            radius = r;
            points = startingPoints;
        }

        public void Update()
        {
            MoveTo(Globals.currentMouseState.Position);
            points = this.bulletsHit * 100 - this.bulletsTaken * 300 + Globals.wave.get_data_size();
            immuneTime--;
        }

        public void Draw()
        {
            Globals.spriteBatch.Draw(texture, new Vector2(position.X - texture.Width/2, position.Y - texture.Height/2), Color.White);
        }

        public bool BulletCollide(DmlBullet bullet)
        {
            Vector2 bpos=bullet.Position;
            return Math.Pow(bpos.X - position.X, 2) + Math.Pow(bpos.Y - position.Y, 2) < Math.Pow(bullet.bulletRadius + radius, 2);
        }

        //moves the ship as close to a given point as it can while taking speed into account
        public void MoveTo(Point p)//takes in a point since MouseState.Position returns a Point, not a Vector2
        {
            double ang = Math.Atan2(p.Y - position.Y, p.X - position.X);//Atan2 takes care of every case on the unit circle

            //moves it in the direction of the chosen position
            
            float dx1 = p.X - position.X;
            float dx2 = speed * (float)Math.Cos(ang);
            float dy1 = p.Y - position.Y;
            float dy2 = speed * (float)Math.Sin(ang);

            //makes sure it doesn't overshoot the position
            float dx = Math.Abs(dx1) < Math.Abs(dx2) ? dx1 : dx2;
            float dy = Math.Abs(dy1) < Math.Abs(dy2) ? dy1 : dy2;
            position += new Vector2(dx,dy);
            //makes sure it stays onscreen
            position.X = Math.Max(0, position.X);
            position.X = Math.Min(position.X,Options.Resolutions.X);
            position.Y = Math.Max(0, position.Y);
            position.Y = Math.Min(position.Y, Options.Resolutions.Y);
        }

        public ShipBullet Shoot()
        {
            return new ShipBullet(position);
        }
    }

}
