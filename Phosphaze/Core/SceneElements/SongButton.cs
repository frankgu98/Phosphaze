using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Phosphaze.Core.Collision;

namespace Phosphaze.Core.SceneElements
{
    class SongButton
    {
        private String song;
        public Wave music{get;private set;}
        private Rectangle rect;
        public String title { get; set; }
        public Texture2D texture;
        public String dmlfile { get; set; }
        private SpriteFont font;
        private RectCollider CollisionMask;
        private bool glow = false;


        public SongButton(Rectangle rect, String folder)
        {
            String[] songfiles = Directory.GetFiles(folder, "*.wav", SearchOption.TopDirectoryOnly);
            String[] dmlfiles = Directory.GetFiles(folder, "*.dml", SearchOption.TopDirectoryOnly);

            if (dmlfiles.Count() > 0) 
                dmlfile = dmlfiles[0];

            String[] pngs = Directory.GetFiles(folder, "*.png", SearchOption.TopDirectoryOnly);//gets images from folder
            String[] jpgs = Directory.GetFiles(folder, "*.jpg", SearchOption.TopDirectoryOnly);
            String[] images = new String[pngs.Length + jpgs.Length];
            pngs.CopyTo(images, 0);
            jpgs.CopyTo(images, pngs.Length);
            if (images.Count() > 0) 
            {
                String image = images[0];
                texture = Texture2D.FromStream(Globals.graphics.GraphicsDevice, File.OpenRead(image));//reads images from stream(non content)
            }
            else
            {
                texture = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                texture.SetData<Color>(new Color[] { Color.White });
            }
            this.song = songfiles[0];
            if (songfiles.Length>0) this.music = new Wave(songfiles[0]);
            this.title = songfiles[0];
            this.rect = rect;
            CollisionMask = new RectCollider(rect);
        }

        public void Load()
        {
            font = Globals.content.Load<SpriteFont>("SpriteFont1");
        }

        public void Update()
        {
            CollisionMask = new RectCollider(rect);
        }

        public void Glow()
        {
            glow = true;
        }

        public void Draw()
        {
            if (glow) { Globals.spriteBatch.Draw(texture, new Rectangle(rect.X - 10, rect.Y - 10, rect.Width + 20, rect.Height + 20), new Color(255, 150, 255, 90)); }
            Globals.spriteBatch.Draw(texture, rect, new Color(200, 200, 200, 200));//should decide on button sizes(zoomed and unzoomed)
            glow = false;
            //Globals.spriteBatch.DrawString(font,title,new Vector2(rect.X+10,rect.Y+10),Color.White);
            //STILL NEED VISUALIZER FOR AUDIO
        }

        public Rectangle getRect()
        {
            return rect;
        }


        public bool Collide(Point pos)
        {
            return rect.Intersects(new Rectangle(pos.X, pos.Y,0,0));
        }


        public void Move(Vector2 deltapos)
        {
            rect.X += (int)deltapos.X;
            rect.Y += (int)deltapos.Y;
        }
    }
}
