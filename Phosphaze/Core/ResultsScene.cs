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

namespace Phosphaze.Core
{
    public class EndScene : Scene
    {
        SceneButton backButton;
        SceneButton[] sceneButtons;
        public EndScene()
        {
        }

        /// <summary>
        /// Perform any scene specific initialization.
        /// </summary>
        public override void Initialize()
        {
            backButton = new SceneButton("Back", "back_button", new Vector2(340, 120));

            sceneButtons = new SceneButton[1] { backButton };

        }

        /// <summary>
        /// Update this scene.
        /// </summary>
        public override void Update()
        {

            base.Update();
            if (focused)
            {
                //checks collision with each button
                foreach (SceneButton button in sceneButtons)
                {
                    if (button.Collide(Globals.currentMouseState.Position))
                    {
                        if (Globals.currentMouseState.LeftButton == ButtonState.Pressed &&
                        Globals.previousMouseState.LeftButton == ButtonState.Released)
                        {
                            if (button.name == "Back")
                            {
                                manager.CurrentScenes.Remove("End");
                                manager.AddCurrentScene("Song Select");
                                manager.FocusOn("Song Select");
                                manager.changedCurrentScenes = true;
                                Globals.wave.reset();
                                Globals.wave = null;
                                Globals.waveAudio = null;
                            }
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
            foreach (SceneButton button in sceneButtons)
            {
                button.Draw();
            }
        }



    }
}

