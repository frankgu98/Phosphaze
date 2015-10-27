using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core.Collision;
using Phosphaze.Core.Utils;
using Phosphaze.Core.SceneElements;

namespace Phosphaze.Core
{
    class SongScene : Scene
    {
        //Ship player = new Ship(400, 400, 200, 200);
        private HashSet<SongButton> Songs;
        private SongButton active;
        private SongButton clicked = null;
        private SceneSlider songbar;
        private Point pressedpoint;
        private SpriteFont font;
        private Texture2D BottomBar;
        private Texture2D bg=null;
        private Texture2D fade;
        private double Width;
        private double dragspeed=0;

        SceneButton Play, Back;
        SceneButton[] scenebuttons;
        private Visualizer Visual;

        public override void Initialize()
        {//Calling Assets
            fade = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);//blank texture
            fade.SetData<Color>(new Color[] { Color.Black });
            font = Globals.content.Load<SpriteFont>("smallfont");
            Play = new SceneButton("Play", "start_button", new Vector2(600, 600), new Rectangle(1070,630,200,60));
            Back = new SceneButton("Back", "back_button", new Vector2(100, 600), new Rectangle(10,630,200,60));
            scenebuttons = new SceneButton[2] {Play,Back};
            BottomBar = Globals.content.Load<Texture2D>("BottomBar");
            Songs = new HashSet<SongButton>();
            int i = 0;
            int x, y, w, h;
            //makes song buttons out of folders 
            foreach (String folder in Directory.GetDirectories(Options.SongFolder))
            {
                w=Options.Resolutions.X*1/5;
                h=(Options.Resolutions.Y*9/10-30)*12/45;
                x=Options.Resolutions.X/20+(w+Options.Resolutions.X/30)*(i/3);
                y=Options.Resolutions.Y/20+(h+Options.Resolutions.Y/45)*(i%3);
                Rectangle rect = new Rectangle(x, y, w, h);
                Songs.Add(new SongButton(rect,folder));
                i++;
                Width = (x + w + (double)Options.Resolutions.X/5) / (double)Options.Resolutions.X;
            }
            songbar = new SceneSlider(new Rectangle(1, Options.Resolutions.Y*4/5, Options.Resolutions.X-2, 20), Width);
            songbar.scrollable = true;
        }


        public override void Update()
        {
            if (active != null)
            {
                Visual.Update();
                if (Globals.nextBlock.p >= Globals.wave.get_data_p() + Globals.wave.get_data_size())
                {
                    Globals.wave.reset();
                }
            }
            if (focused)
            {
                //SCROLL UPDATE
                songbar.Update();
                int Offset = songbar.ScaledMove();

                //BUTTON UPDATE
                foreach (SongButton song in Songs)
                {
                    song.Move(new Vector2(-Offset, 0));
                    song.Update();
                }


                Point pos = Globals.currentMouseState.Position;
                foreach (SongButton s in Songs)
                {
                    //Checks for mouse hovering over songbuttons
                    if (s.Collide(pos))//check collision
                    {
                        if (Globals.currentMouseState.LeftButton == ButtonState.Released
                            && clicked == s//THIS METHOD MAKES SURE YOU CANNOT RESELECT THE SAME SONG   
                            && Math.Pow((pressedpoint - Globals.currentMouseState.Position).X, 2) + Math.Pow((pressedpoint - Globals.currentMouseState.Position).Y, 2) < 10000)
                        {
                            if (active != s)
                            {
                                active = s;
                                bg = s.texture;
                                if (Globals.wave!=null) Globals.wave.reset();//reset previous song
                                Globals.wave = active.music;//new song
                                Globals.waveAudio = new Audio(Globals.wave, 0);
                                Visual = new Visualizer(Globals.wave);
                            }
                        }
                        if (Globals.currentMouseState.LeftButton == ButtonState.Pressed
                            && Globals.previousMouseState.LeftButton == ButtonState.Released)
                        {
                            clicked = s;
                            pressedpoint = Globals.currentMouseState.Position;
                        }
                        if (Globals.currentMouseState.LeftButton == ButtonState.Released) clicked = null;

                        s.Glow();//applies a glow effect
                    }
                }
                //DRAGGING SCREEN     move the screen with mouse/touchscreen
                if (Globals.currentMouseState.LeftButton == ButtonState.Pressed
                    && 0 < Globals.currentMouseState.Position.Y
                    && Globals.currentMouseState.Position.Y < Options.Resolutions.Y * 4 / 5
                    && !songbar.pressed)
                {
                    if (Globals.currentMouseState.Position.X - Globals.previousMouseState.Position.X > 250)//added to avoid bug when click, release,click
                    {
                        dragspeed = 1;
                    }
                    else if (Globals.currentMouseState.Position.X - Globals.previousMouseState.Position.X < -250)
                    {
                        dragspeed = -1;
                    }
                    else
                    {
                        dragspeed = (Globals.currentMouseState.Position.X - Globals.previousMouseState.Position.X) / 4;

                    }
                }
                if (Globals.currentMouseState.LeftButton == ButtonState.Released)
                {
                    if (dragspeed > 0)
                    {
                        dragspeed -= .4;
                        if (dragspeed < .3)
                        {
                            dragspeed = 0;
                        }
                    }
                    else if (dragspeed < 0)
                    {
                        dragspeed += .4;
                        if (dragspeed > -.3)
                        {
                            dragspeed = 0;
                        }
                    }
                }
                songbar.bar.X -= (int)dragspeed;
                songbar.Update();

                //Button update-check for collisions
                if (Globals.previousMouseState.LeftButton == ButtonState.Pressed && Globals.currentMouseState.LeftButton == ButtonState.Released)
                {
                    if (Play.Collide(pos))
                    {
                        //this.manager.RegisterScene("Play", new LevelScene());
                        this.manager.AlmostCurrentScenes.Add("Background");
                        this.manager.AlmostCurrentScenes.Add("Level");
                        Globals.pathToCurrentMap = active.dmlfile;
                        Globals.songname = active.title;
                        this.manager.transitioningScenes = true;
                        this.manager.FocusOn("Level");
                    }
                    if (Back.Collide(pos))
                    {
                        this.manager.AlmostCurrentScenes.Add("Background");
                        this.manager.AlmostCurrentScenes.Add("Main Menu");
                        this.manager.FocusOn("Main Menu");
                        this.manager.transitioningScenes = true;
                    }
                }
            }
           
        }

        public override void Draw()
        {
            Globals.spriteBatch.Draw(fade,new Rectangle(0,0,Options.Resolutions.X,Options.Resolutions.Y), Color.Black);
            if (bg != null)
            {
                Globals.spriteBatch.Draw(bg, new Rectangle(0,0,Options.Resolutions.X,Options.Resolutions.Y), new Color(Color.White, 250));
            }
            if (active != null) Globals.spriteBatch.DrawString(font, "Currently Playing   " + active.title, new Vector2((int)(Options.Resolutions.X / 2), 0), Color.White);
            songbar.Draw();//scrollbar bar for songs
            foreach (SongButton song in Songs)
            {
                song.Draw();
            }

            if (Visual != null)//Visualizer
            {
                Visual.Draw();
            }
            Globals.spriteBatch.Draw(BottomBar, new Rectangle(0, Options.Resolutions.Y * 17 / 20, Options.Resolutions.X, Options.Resolutions.Y * 3 / 20), Color.White);

            foreach (SceneButton button in scenebuttons)
            {
                button.Draw();
            }
        }
    }
}
