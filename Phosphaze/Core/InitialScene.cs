using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core.SceneElements;
using Phosphaze.Core.Utils;

namespace Phosphaze.Core
{
    
    public class InitialScene : Scene
    {
        /// <summary>
        /// The currently chosen path name.
        /// </summary>
        string chosenPath;

        /// <summary>
        /// The position of the chosen path name.
        /// </summary>
        Vector2 chosenPathPosition = new Vector2(640, 400);

        /// <summary>
        /// The main font used to draw the strings in ``messages``.
        /// </summary>
        SpriteFont mainFont;

        /// <summary>
        /// The font used to draw the selected song folder path.
        /// </summary>
        SpriteFont pathFont;

        /// <summary>
        /// The main background of the scene.
        /// </summary>
        Texture2D background;

        /// <summary>
        /// The button that allows the user to choose their song folder.
        /// </summary>
        SceneButton ChooseFolderButton;

        /// <summary>
        /// The button that allows the user to accept their chosen song folder.
        /// </summary>
        FadingSceneButton AcceptButton;

        /// <summary>
        /// The button that allows the user to deny their chosen song folder.
        /// </summary>
        FadingSceneButton DenyButton;

        /// <summary>
        /// The possible states this scene can take.
        /// </summary>
        enum SceneStates { BeforeFolderChosen, AfterFolderChosen }

        /// <summary>
        /// The current state of the scene.
        /// </summary>
        SceneStates currentState = SceneStates.BeforeFolderChosen;

        double backgroundOffset = -300;

        static Vector2[] buttonPositions = {
            new Vector2(0, 0), // Corresponds to ChooseFolderButton (doesn't matter)
            new Vector2(260, 500), // Corresponds to AcceptButton
            new Vector2(1020, 500)  // Corresponds to DenyButton
            };

        static Rectangle[] buttonRects = {
            new Rectangle(640 - 387/2, 160, 387, 92), // Corresponds to ChooseFolderButton
            new Rectangle(0, 0, 387, 92), // Corresponds to AcceptButton
            new Rectangle(0, 0, 387, 92)  // Corresponds to DenyButton
            };

        /// <summary>
        /// The messages to draw onscreen.
        /// </summary>
        string[] messages = {
            "Hey friend!", 
            "You appear to be a new user.",
            "Please choose a song folder!",
            "This will be where Phosphaze loads", 
            "songs from and stores maps to.",
            "Make this the song folder?",
            "(You can change this later if you want)"
            };

        /// <summary>
        /// The positions of the respective messages.
        /// </summary>
        static Vector2[] textPositions = {
            new Vector2(640, 50),
            new Vector2(640, 90),
            new Vector2(640, 130),
            new Vector2(640, 285),
            new Vector2(640, 325),
            new Vector2(640, 600),
            new Vector2(640, 640)
            };

        /// <summary>
        /// The colour to draw the text.
        /// </summary>
        static Color textColour = Color.White;
        
        /// <summary>
        /// The colour to draw the path.
        /// </summary>
        static Color pathColour = Color.White;

        public override void Initialize()
        {
            mainFont = Globals.content.Load<SpriteFont>("Fonts/InitSceneFont");
            pathFont = Globals.content.Load<SpriteFont>("Fonts/PathFont");
            background = Globals.content.Load<Texture2D>("Backgrounds/InitSceneBG");
            
            // Load the buttons
            ChooseFolderButton = new SceneButton("Button", "InitSceneContent/ChooseFolderButton", buttonPositions[0], buttonRects[0]);
            AcceptButton = new FadingSceneButton(
                "InitSceneContent/AcceptButton", "InitSceneContent/AcceptButtonHover", "InitSceneContent/AcceptButtonClicked", 
                buttonPositions[1], buttonRects[1], 800d, 1);
            DenyButton = new FadingSceneButton(
                "InitSceneContent/DenyButton", "InitSceneContent/DenyButtonHover", "InitSceneContent/DenyButtonClicked",
                buttonPositions[2], buttonRects[2], 800d, 1);
            
        }

        /// <summary>
        /// Update the elements of the scene that respond to input from the user.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Update the buttons.
            ChooseFolderButton.Update();
            AcceptButton.Update();
            DenyButton.Update();

            // Scroll the background back and forth.
            backgroundOffset += 0.2*Math.Sin(LocalTime/5000);

            // If we've already selected a folder (i.e. the user pressed okay in the dialog box)
            if (currentState == SceneStates.AfterFolderChosen)
            {
                // If the user denied the path they selected.
                if (DenyButton.IsClicked())
                {
                    chosenPath = null;

                    currentState = SceneStates.BeforeFolderChosen;
                    AcceptButton.FadeOut();
                    DenyButton.FadeOut();
                }
                // If the user accepted the path they selected.
                else if (AcceptButton.IsClicked())
                {
                    Options.SongFolder = chosenPath;
                    new UserInfoLoader().Write();

                    AcceptButton.FadeOut();
                    DenyButton.FadeOut();

                    // Transition to the main menu.
                    manager.AlmostCurrentScenes.Add("Background");
                    manager.AlmostCurrentScenes.Add("Main Menu");
                    manager.FocusOn("Main Menu");
                    manager.transitioningScenes = true;

                    // Organize the songs folder.
                    SongFolderOrganizer.Organize();
                }
            }

            // If the user presses the Choose Folder button and they haven't already chosen a folder.
            if (currentState == SceneStates.BeforeFolderChosen && ChooseFolderButton.Collide(Globals.currentMouseState.Position) && 
                Globals.currentMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && // Freakin name collisions
                Globals.previousMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
            {
                var dialog = new FolderBrowserDialog();
                dialog.Description = "Choose a Song Folder for Phosphaze";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    chosenPath = dialog.SelectedPath;

                    currentState = SceneStates.AfterFolderChosen;
                    AcceptButton.FadeIn();
                    DenyButton.FadeIn();
                }
            }
        }

        /// <summary>
        /// Draw the scene.
        /// </summary>
        public override void Draw()
        {
            // Draw the background.
            Globals.spriteBatch.Draw(background, new Vector2((float)backgroundOffset, -300f), Color.White);

            // Draw the messages that always display.
            for (int i = 0; i < 5; i++)
                Globals.spriteBatch.DrawStringCentered(mainFont, messages[i], textPositions[i], textColour);
            if (currentState == SceneStates.AfterFolderChosen)
            {
                // Draw the messages that only display when you've chosen a path.
                Globals.spriteBatch.DrawStringCentered(mainFont, messages[5], textPositions[5], textColour);
                Globals.spriteBatch.DrawStringCentered(mainFont, messages[6], textPositions[6], textColour);
                Globals.spriteBatch.DrawStringCentered(pathFont, chosenPath, chosenPathPosition, pathColour);
            }

            // Draw the buttons.
            ChooseFolderButton.Draw();
            AcceptButton.Draw();
            DenyButton.Draw();
        }

    }
}
