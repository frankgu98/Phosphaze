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

// AUTHOR: Michael Ala&Frank Gu

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace Phosphaze.Core
{
    /// <summary>
    /// For overlapping scenes.
    /// </summary>
    public sealed class SceneManager
    {

        // Mapping of scene names to scene objects.
        Dictionary<string, Scene> RegisteredScenes;

        // The names of the scenes currently on screen, add scenes here if it's a small scene change 
        public List<String> CurrentScenes{get; private set;}

        //names of scenes that will be used after a transition fadeaway, add scenes here if it's a large, usually multi-scene change 
        public List<String> AlmostCurrentScenes{ get; set; }


        //if the current scenes have been changed
        public bool changedCurrentScenes{get;set;}

        //if there's going to be a full transitioning of scenes not just adding a small menu or something
        //not really necessary, but easier to understand
        public bool transitioningScenes { get; set; }

        //if there is one scene fading out to another, fadeAmount>=1 happens when it's time to change scenes
        public int fadeAmount;

        //black box used to fade between scenes
        public static Texture2D fadeTexture;



        /// <summary>
        /// Construct a new SceneManager
        /// </summary>
        public SceneManager()
        {
            RegisteredScenes = new Dictionary<string, Scene>();
            CurrentScenes = new List<string>();
            AlmostCurrentScenes = new List<string>();
            
            changedCurrentScenes=false;
            fadeTexture=Globals.content.Load<Texture2D>("black");
        }

        /// <summary>
        /// Register a scene to this SceneManager.
        /// </summary>
        /// <param name="name">The name of the scene by which to reference it.</param>
        /// <param name="scene">The scene object.</param>
        public void RegisterScene(string scenename, Scene scene)
        {
            scene.SetManager(this);
            // Scene specific initialization.
           
            RegisteredScenes[scenename] = scene;
        }

        /// <summary>
        /// Remove a registered scene.
        /// </summary>
        /// <param name="scenename">The name to revoke.</param>
        public void RevokeRegisteredScene(string scenename)
        {
            RegisteredScenes.Remove(scenename);
            if (CurrentScenes.Contains(scenename)) 
            { 
                CurrentScenes.Remove(scenename); 
            }
        }

        public void TryRevokeScene(string scenename)
        {
            try
            {
                RevokeRegisteredScene(scenename);
            }
            catch { }
        }

        
        public Scene GetScene(string scenename)
        {
            return RegisteredScenes[scenename];
        }


        /// <summary>
        /// Update the current scenes
        /// </summary>
        /// <param name></param>
        public void Update()
        {
            //TODO:lock buttons if transitioning
            //if you started fading, continue fading
            if (fadeAmount > 0)
            {
                fadeAmount += 5;//adjust how fast you fade
                //if you faded the full amount, switch out your current scenes
                if (fadeAmount > 255)
                {
                    fadeAmount = 0;
                    CurrentScenes.Clear();
                    foreach (string scenename in AlmostCurrentScenes)
                    {
                        AddCurrentScene(scenename);
                    }
                    AlmostCurrentScenes.Clear();

                }
            }
            
            foreach (String scenename in CurrentScenes)
            {
                RegisteredScenes[scenename].Update();

                //a full fadeaway/scene change
                if(transitioningScenes){
                    transitioningScenes = false;
                    fadeAmount = 1;
                    break;
                }
                //a small scene change (ex. semi-transparent menu being removed)
                if (changedCurrentScenes)
                {
                    changedCurrentScenes = false;
                    break;
                }
                
            }
            

            if (Globals.waveAudio != null && Globals.wave != null && !Globals.songPaused)
            {
                Globals.nextBlock = Globals.wave.next(Globals.waveAudio.framesAvailable());
                if (Globals.nextBlock.p >= Globals.wave.get_data_p() + Globals.wave.get_data_size())
                {
                    
                    Globals.wave.reset();
                }
            }

            if (Globals.waveAudio != null && !Globals.songPaused)
            {
                Globals.waveAudio.setChannelVolume(0, Options.Volumes.GlobalVolume * Options.Volumes.MusicVolume);
                Globals.waveAudio.setChannelVolume(1, Options.Volumes.GlobalVolume * Options.Volumes.MusicVolume);
                Globals.waveAudio.fillBuffer();

            }
        }

        /// <summary>
        /// Draws the current scenes
        /// </summary>
        public void Draw()
        {
            Globals.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            foreach (String scenename in CurrentScenes)
            {
                //if there's more than 1 current scene, it draws a semi-opaque black rectangle between scenes to separate them
                if (CurrentScenes.Count > 1)
                {
                    Globals.spriteBatch.Draw(fadeTexture, Vector2.Zero, new Color(0, 0, 0, Options.BackgroundDim));
                }
                RegisteredScenes[scenename].Draw();
            }
            //draws a black rectangle of varying opaqueness over everything if fading has started
            if (fadeAmount>0)
            {
                Globals.spriteBatch.Draw(fadeTexture, Vector2.Zero, new Color(0, 0, 0, fadeAmount));
            }
            Globals.spriteBatch.End();
        }

        public void AddCurrentScene(string scenename)
        {
            CurrentScenes.Add(scenename);
            RegisteredScenes[scenename].Initialize();
        }

        //focuses the scenemanager onto a specific scene (the one you can interact with)
        public void FocusOn(string scenename)
        {
            foreach (string s in CurrentScenes)
            {
                RegisteredScenes[s].focused = false;
            }
            RegisteredScenes[scenename].focused = true;
        
        }
    }
}