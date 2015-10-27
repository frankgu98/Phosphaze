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

// AUTHOR: Michael Ala

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core;
using Phosphaze.Core.Collision;
using Phosphaze.Core.Utils;
using Phosphaze.Core.SceneElements;
using Phosphaze.Core.Dml;
using Phosphaze.Core.Dml.Builtins;
using Phosphaze.Core.Dml.Parse;

namespace Phosphaze
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SceneManager sceneManager;

        public Main()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();



            // Set screen defaults.
            graphics.PreferredBackBufferWidth = Options.Resolutions.X;
            graphics.PreferredBackBufferHeight = Options.Resolutions.Y;
            graphics.ApplyChanges();

            // Set global GraphicsDeviceManager and ContentManager.
            Globals.graphics = graphics;
            Globals.content = Content;

            Globals.randGen = new System.Random();

            Globals.game = this;

            Globals.pathToCurrentMap = null;
            Globals.songPaused = false;

            IsMouseVisible = true;
            sceneManager = new SceneManager();

            sceneManager.RegisterScene("Background", new StarsScene());
            sceneManager.RegisterScene("Main Menu", new MainMenuScene());
            sceneManager.RegisterScene("Song Select", new SongScene());
            sceneManager.RegisterScene("Game Options", new OptionsScene());
            sceneManager.RegisterScene("Exit", new ExitScene());
            sceneManager.RegisterScene("Level", new LevelScene());

            var userInfo = new UserInfoLoader();
            if (userInfo.hasInfo)
            {
                userInfo.Load();
                sceneManager.AddCurrentScene("Background");
                sceneManager.AddCurrentScene("Main Menu");
                sceneManager.FocusOn("Main Menu");
            }
            else
            {
                sceneManager.RegisterScene("Init", new InitialScene());
                sceneManager.AddCurrentScene("Init");
                sceneManager.FocusOn("Init");
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            /*
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();*/

            base.Update(gameTime);

            // Set previous input states to the current input states.
            Globals.previousMouseState = Globals.currentMouseState;
            Globals.previousKeyboardState = Globals.currentKeyboardState;

            // Get new input states.
            Globals.currentMouseState = Mouse.GetState();
            Globals.currentKeyboardState = Keyboard.GetState();

            Globals.gameTime = gameTime;
            Globals.deltaTime = gameTime.ElapsedGameTime.Milliseconds;

            sceneManager.Update();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            GraphicsDevice.Clear(Color.Black);
            sceneManager.Draw();

        }

        public void ExitGame()
        {
            new UserInfoLoader().Write();
            Exit();
        }
    }
}
