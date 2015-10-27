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

// AUTHOR: Frank Gu & Michael Ala

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

//input scenemanager into scenebutton, change scenes directly from scenebutton?
namespace Phosphaze.Core
{
    public class MainMenuScene : Scene
    {

        // Buttons
        //MIGHT PUT THESE IN A DICTIONARY TO IMMEDIATELY ACCESS WHEN DETECTING COLLISION00
        SceneButton startButton, optionsButton, aboutButton, exitButton, MainButton;
        // Phosphaze Logo
        SceneTexture logo;
        // FCDM copyright
        SceneText copyright;

        // Array of SceneButtons.
        SceneButton[] sceneButtons;

        Visualizer2 spectrum;
        private bool popout = false;
        private float rotation=0;

        int? selected = null; // By default select nothing.

        #region Constants

        // Figure these out later.
        private const int startButtonX = 0;
        private const int startButtonY = 0;
        private const int optionsButtonX = 200;
        private const int optionsButtonY = 0;
        private const int aboutButtonX = 400;
        private const int aboutButtonY = 0;
        private const int exitButtonX = 600;
        private const int exitButtonY = 0;

        private const int logoX = 0;
        private const int logoY = 0;

        private const int copyrightX = 0;
        private const int copyrightY = 0;

        #endregion

        public MainMenuScene()
        {
        }


        /// <summary>
        /// Perform any scene specific initialization.
        /// </summary>
        public override void Initialize()
        {
            spectrum = new Visualizer2(Globals.wave);
            focused = true;//first interactable scene so it's guarenteed to be focused
            // NOTE: Awaiting assets.

            Vector2 centre = new Vector2(Options.Resolutions.X / 2, Options.Resolutions.Y / 2);
            // Initialize the buttons. If the buttons are sectors, the smaller angle must be first
            MainButton = new SceneButton("Main", "MainMenuContent/MainButton", new Vector2(340, 220),new Rectangle(340,210,300,300));
            startButton = new SceneButton("Exit", "MainMenuContent/ExitButton", new Vector2(340, 220), new Rectangle(290, 160, 200, 200));
            optionsButton = new SceneButton("Play", "MainMenuContent/PlayButton", new Vector2(480, 220), new Rectangle(490, 160, 200, 200));
            aboutButton = new SceneButton("Options", "MainMenuContent/OptionsButton", new Vector2(340, 360), new Rectangle(290, 360, 200, 200));
            exitButton = new SceneButton("About", "MainMenuContent/EditButton", new Vector2(480, 360), new Rectangle(490, 360, 200, 200));
            
            // Initialize extra components.
            /*
            logo = new SceneTexture(@"MainMenuContent/Logo");
            copyright = new SceneText("Copyright (c) 2015 FCDM", @"MainMenuContent/CopyrightFont");
             */


            sceneButtons = new SceneButton[5] {
                startButton,
                optionsButton,
                aboutButton,
                exitButton,
                MainButton
            };

            //Console.WriteLine(System.IO.Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Update this scene.
        /// </summary>
        public override void Update()
        {
            spectrum.Update();
            base.Update();
            rotation += .003f;
            if (focused)
            {
                
                foreach (SceneButton button in sceneButtons)
                {
                    if (button.Collide(Globals.currentMouseState.Position))
                    {
                        //highlight
                        if (Globals.currentMouseState.LeftButton == ButtonState.Pressed &&
                        Globals.previousMouseState.LeftButton == ButtonState.Released)
                        {
                            if (popout)
                            {
                                if (button.name.Equals("Play"))
                                {
                                    manager.AlmostCurrentScenes.Add("Song Select");
                                    manager.FocusOn("Song Select");
                                    manager.transitioningScenes = true;
                                }
                                else if (button.name.Equals("Options"))
                                {
                                    manager.AlmostCurrentScenes.Add("Background");
                                    manager.AlmostCurrentScenes.Add("Game Options");//this makes a new songOptions everytime aka resets everything to 50
                                    manager.FocusOn("Game Options");
                                    manager.transitioningScenes = true;
                                }
                                else if (button.name.Equals("Exit"))
                                {
                                    Globals.game.Exit();
                                }
                                else if (button.name.Equals("Main"))
                                {
                                    popout = false;
                                }
                            }
                            else
                            {
                                if (button.name.Equals("Main"))
                                {
                                    popout = true;
                                }
                            }
                        }
                    }
                }
            }
            

            //foreach (SceneButton button in ) 
            /*
            if (selected.HasValue)
            {
                sceneButtons[selected.Value].selected = true;
            }
            
            // Determine which button is now selected.
            selected = InputUtils.GetMenuSelection(
                selected, sceneButtons, 
                Options.Keybindings.DOWN, 
                Options.Keybindings.UP);*/

        }

        /// <summary>
        /// draws scene
        /// </summary>
        public override void Draw()
        {
            MainButton.Draw(rotation);
            if (popout)
            {
            foreach (SceneButton button in sceneButtons)
            {
                if (button.name != "Main")
                    {
                        button.Draw();
                    }       
                }
            }
            spectrum.Draw();
        }
        
    }
}
