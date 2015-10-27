//Frank Gu

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core;
using Phosphaze.Core.Collision;
using Phosphaze.Core.Utils;
using Phosphaze.Core.SceneElements;


namespace Phosphaze.Core
{
    public class OptionsScene:Scene
    {
        SceneButton bloomSelectButton,backButton;
        SceneSlider bgDimSlider, globalVolumeSlider, sfxVolumeSlider, musicVolumeSlider, MenuSlider;

        SceneButton[] sceneButtons;
        SceneSlider[] sceneSliders;

        SpriteFont font;
        Texture2D logo;
        public OptionsScene()
        {
        }

        /// <summary>
        /// Perform any scene specific initialization.
        /// </summary>
        public override void Initialize()
        {
            font = Globals.content.Load<SpriteFont>("NewSpriteFont");
            logo = Globals.content.Load<Texture2D>("models/Logo");
            bloomSelectButton = new SceneButton("Folder Select", "folder_button", new Vector2(1020, 620),new Rectangle(1020,620,260,110));
            backButton = new SceneButton("Back", "back_button", new Vector2(0,650), new Rectangle(0, 650, 200, 80));
            bgDimSlider = new SceneSlider("BG Dim", new Rectangle(340, 280, 300, 50), 5,Options.BackgroundDim);
            globalVolumeSlider = new SceneSlider("Global Volume", new Rectangle(340, 380, 300, 50), 5,Options.Volumes.GlobalVolume);//need to start at .5 since the actualy values start at .5
            sfxVolumeSlider = new SceneSlider("SFX Volume", new Rectangle(340, 480, 300, 50), 5,Options.Volumes.SoundFXVolume);
            musicVolumeSlider = new SceneSlider("Music Volume", new Rectangle(340, 580, 300, 50), 5,Options.Volumes.MusicVolume);

            sceneButtons = new SceneButton[2] { bloomSelectButton, backButton };
            sceneSliders = new SceneSlider[4] { bgDimSlider, globalVolumeSlider, sfxVolumeSlider, musicVolumeSlider };
        }

        /// <summary>
        /// Update this scene.
        /// </summary>
        public override void Update()
        {

            base.Update();
            Console.WriteLine(focused);
            if (focused)
            {
               
                //checks collision with each button
                foreach (SceneButton button in sceneButtons)
                {
                    if (button.Collide(Globals.currentMouseState.Position))
                    {
                        if (Globals.currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                        Globals.previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                        {
                            if (button.name == "Back")
                            {
                                manager.AlmostCurrentScenes.Add("Background");
                                manager.AlmostCurrentScenes.Add("Main Menu");
                                manager.FocusOn("Main Menu");
                                manager.transitioningScenes = true;
                            }
                        }
                        if (Globals.currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                        Globals.previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                        {
                            if (button.name == "Folder Select")
                            {
                                var dialog = new FolderBrowserDialog();
                                if (dialog.ShowDialog() == DialogResult.OK)
                                {
                                    Options.SongFolder = dialog.SelectedPath;
                                }
                            }
                        }
                    }
                }

                //checks collision with each slider
                foreach (SceneSlider slider in sceneSliders)
                {
                    //need to update after to keep bar in bounds of slider
                    slider.Update();
                    if (slider.pressed)
                    {
                    
                        if (slider.name == "BG Dim")
                        {

                            Options.BackgroundDim = 1 - slider.GetPercentage();
                        }
                        else if (slider.name == "Global Volume")
                        {
                            Options.Volumes.GlobalVolume = slider.GetPercentage();
                        }
                        else if (slider.name == "SoundFX Volume")
                        {

                            Options.Volumes.SoundFXVolume = slider.GetPercentage();
                        }
                        else if (slider.name == "Music Volume")
                        {
                            Options.Volumes.MusicVolume = slider.GetPercentage();
                        }
                        
                    }
                        
                }
            }
        }

        /// <summary>
        /// draws scene
        /// </summary>
        public override void Draw()
        {
            Texture2D blank = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1);
            blank.SetData(new[] { Color.White });
            Globals.spriteBatch.Draw(blank, new Rectangle(0, 0, Options.Resolutions.X, Options.Resolutions.Y), Color.Transparent);
            Globals.spriteBatch.Draw(blank, new Rectangle(10, 660, 170, 50), new Color(Color.White, 5));
            Globals.spriteBatch.Draw(blank, new Rectangle(0, 680, 520, 20), new Color(Color.White, 5));
            Globals.spriteBatch.Draw(blank, new Rectangle(0, 0, 20, 700), new Color(Color.White, 5));
            Globals.spriteBatch.Draw(blank, new Rectangle(700, 680, 800, 20), new Color(Color.White, 5));
            Globals.spriteBatch.Draw(blank, new Rectangle(1260, 0, 20, 700), new Color(Color.White, 5));
            Globals.spriteBatch.Draw(blank, new Rectangle(0, 0, 1280, 10), new Color(Color.White, 5));
            Globals.spriteBatch.Draw(logo, new Rectangle(750, 50, 500, 250), new Color(Color.White, 5));
            foreach (SceneButton button in sceneButtons)
            {
                button.Draw();
            }
            foreach (SceneSlider slider in sceneSliders)
            {
                //write text
                Globals.spriteBatch.DrawString(font, slider.name, new Vector2(slider.back.X + slider.back.Width+20, slider.back.Y), Color.White);
                Globals.spriteBatch.DrawString(font, ""+Math.Round(slider.GetPercentage(),2)*100, new Vector2(slider.back.X -100, slider.back.Y), Color.White);
                slider.Draw();
            }
        }

        public void Draw(Rectangle Window)
        {
            /*MenuSlider.*/
            int posX = (Window.X + Window.Width / 20);
            /*int Widt = (Window.Width*18/20-)*/
            foreach (SceneButton button in sceneButtons)
            {
                button.Draw();
            }
            foreach (SceneSlider slider in sceneSliders)
            {
                slider.Draw();
            }
            Globals.spriteBatch.End();
        }
    }



}
