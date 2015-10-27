using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Phosphaze.Core
{
    /// <summary>
    /// A singleton of global properties accessible at any point in the game.
    /// </summary>
    public static class Globals
    {
        // Put all the properties below.

        //game object
        public static Main game { get; set; }

        public static Audio waveAudio { get; set; }
        public static Wave wave { get; set; }
        public static memblock nextBlock { get; set; }

        // The global GraphicsDeviceManager.
        public static GraphicsDeviceManager graphics = null;

        // The global SpriteBatch.
        public static SpriteBatch spriteBatch = null;

        // The global ContentManager
        public static ContentManager content = null;

        // The previous and current mouse states.
        public static MouseState previousMouseState;
        public static MouseState currentMouseState;

        // The previous and current keyboard states.
        public static KeyboardState previousKeyboardState;
        public static KeyboardState currentKeyboardState;

        // The current game time.
        public static GameTime gameTime;
        
        // The time since the last call to the game's update method.
        // This is short for GlobalProperties.Instance.gameTime.ElapsedGameTime.Milliseconds.
        public static double deltaTime;

        //Random object
        public static Random randGen = new Random();

        public static string pathToCurrentMap;

        public static bool songPaused;

        public static String songname;
    }
}
