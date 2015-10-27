//Frank Gu

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
    public class ExitScene:Scene
    {
        SceneButton cancelButton, exitButton;
        SceneButton[] sceneButtons;
        public ExitScene()
        {
        }

        /// <summary>
        /// Perform any scene specific initialization.
        /// </summary>
        public override void Initialize()
        {
            focused = false;
            cancelButton = new SceneButton("Cancel", "ExitContent/cancelbutton", new Vector2(340, 120));
            exitButton = new SceneButton("Exit", "ExitContent/exitbutton", new Vector2(340, 220));

            sceneButtons = new SceneButton[2] { cancelButton, exitButton };

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
                            if (button.name == "Cancel")
                            {
                                manager.CurrentScenes.Remove("Exit");
                                manager.FocusOn("Main Menu");
                                manager.changedCurrentScenes = true;
                            }
                            else if (button.name == "Exit")
                            {
                                Globals.game.Exit();
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
            Globals.spriteBatch.Begin();
            foreach (SceneButton button in sceneButtons)
            {
                button.Draw();
            }
            Globals.spriteBatch.End();
        }



    }
}
