using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Phosphaze.Core.SceneElements
{
    class Visualizer//OSCILLISCOPE
    {
        private Vector2[] points = new Vector2[30];//array of points
        private Wave song;
        private Texture2D blank;
        public Visualizer(Wave song)
        {
            this.song = song;
            blank = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });//blank texture
            for (int i = 0; i < 30; i++)
            {
                points[i] = new Vector2(850, 645);//fill points with default values
            }
        }

        public void DrawLine(Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)//Draws a Line(math stuffs)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);
            Globals.spriteBatch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        //NOTICE: THIS CLASS HAS YET TO BE MADE ACCESSIBLE TO SCENES OTHER THAN SONG SELECT

        public void Update()
        {
            if (song != null)
            {
                double y = song.getStereoObject(song.getLastNextResult()).getAvg();//get average magnitude of current stereo object
                if (Math.Abs(y) < 10000) y = y / 1200;//scale small values to be be smaller
                else y = y / 900;//scale larger values slightly less
                Vector2[] temppoints = new Vector2[30];
                for (int i = 0; i < 29; i++)
                {
                    points[i].X -= 13;//move the points 13 units left
                    temppoints[i + 1] = points[i];
                }
                points = temppoints;
                points[0] = new Vector2(850, 645 + (int)y);
            }
        }
        public void Draw()
        {
            Globals.spriteBatch.Draw(blank, new Rectangle(240, 615, 800, 60), new Color(130,90,90, 254));
            for (int i = 1; i < 30; i++)
            {
                Vector2 point1 = points[i];
                Vector2 point2 = points[i - 1];
                if (i < 9)//more recent points drawn thicker
                {
                    this.DrawLine(blank, 3, Color.White, point1, new Vector2(point1.X - 6, 645));
                    this.DrawLine(blank, 3, Color.White, new Vector2(point1.X, -1 * point1.Y + 1290), new Vector2(point1.X - 6, 645));
                    this.DrawLine(blank, 3,Color.White, new Vector2(point2.X + 6, 645), point2);
                    this.DrawLine(blank, 3, Color.White, new Vector2(point2.X + 6, 645), new Vector2(point2.X, -1 * point2.Y + 1290));
                }
                else if (i < 20)//medium thickness
                {
                    this.DrawLine(blank, 2, Color.White, point1, new Vector2(point1.X - 6, 645));
                    this.DrawLine(blank, 2, Color.White, new Vector2(point1.X, -1 * point1.Y + 1290), new Vector2(point1.X - 6, 645));
                    this.DrawLine(blank, 2, Color.White, new Vector2(point2.X + 6, 645), point2);
                    this.DrawLine(blank, 2, Color.White, new Vector2(point2.X + 6, 645), new Vector2(point2.X, -1 * point2.Y + 1290));
                }
                else//thin
                {
                    this.DrawLine(blank, 1, Color.White, point1, new Vector2(point1.X - 6, 645));
                    this.DrawLine(blank, 1, Color.White, new Vector2(point1.X, -1 * point1.Y + 1290), new Vector2(point1.X - 6, 645));
                    this.DrawLine(blank, 1, Color.White, new Vector2(point2.X + 6, 645), point2);
                    this.DrawLine(blank, 1, Color.White, new Vector2(point2.X + 6, 645), new Vector2(point2.X, -1 * point2.Y + 1290));
                }
                //this.DrawLine(blank, 3, Color.White, new Vector2(point1.X, -1 * point1.Y+1290), point1);
            }
            this.DrawLine(blank, 1, Color.White, new Vector2(1000, 645), new Vector2(857, 645));//draw lines to sides to fit box
            this.DrawLine(blank, 1, Color.White, new Vector2(467, 645), new Vector2(250, 645));
        }
    }


    class Visualizer2//SPECTRUM
    {
        private Vector2[] points = new Vector2[30];
        private Wave song;
        private Texture2D blank;
        public Visualizer2(Wave song)
        {
            this.song = song;
            blank = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            for (int i = 0; i < 30; i++)
            {
                points[i] = new Vector2(900, 640);
            }
        }


        public void Update()
        {
            if (song != null)
            {
                memblock m = song.getLastNextResult();//current memblock
                FFTransform FFT = new FFTransform(song.getStereoObject(m), 5);//creates FFT using 5 samples per second
                DFTransform.DFTChannelResult Results;
                if (m.nBytes > 0)//if memblock is not empty
                {
                    do//has another memblock 
                    {
                        Results = FFT.next().stereo;
                        int x = (int)Results.freq;
                        Console.WriteLine(x);
                        if (x < 1 || x > 20000) break;
                        int i = (int)Math.Log(x, 1.3);
                        double y = Math.Abs(Results.dbmag);
                        points[i] = new Vector2(800 + 10 * i, 640 - (int)(y));
                    }
                    while (FFT.hasNext());
                }
            }
        }


        public void Draw()
        {
            foreach (Vector2 point in points)
            {
                Globals.spriteBatch.Draw(blank, new Rectangle((int)point.X, (int)point.Y, 5, 640 - (int)point.Y), Color.White);
            }
        }
    }
}