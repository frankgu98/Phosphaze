using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core.Dml;
using Phosphaze.Core.Dml.Builtins;
using Phosphaze.Core.Dml.Parse;

namespace Phosphaze.Core
{
    //level where the player plays
    public class LevelScene : Scene
    {
        SpriteFont font;
        DmlSystem system;
        Player player;
        List<ShipBullet> shipBullets;
        public LevelScene()
        {
        }

        public override void Initialize()
        {
            font = Globals.content.Load<SpriteFont>("smallfont");
            Mouse.SetPosition(Options.Resolutions.X / 2, Options.Resolutions.Y / 2);
           

            //song was paused before reaching here
            Globals.songPaused = false;
            //resets the wave so the next memblock is the first one
            Globals.wave.reset();
            //gets the Audio ready to play
            Globals.waveAudio = new Audio(Globals.wave, 250000);
            player = new Player("ship", new Vector2(Options.Resolutions.X / 2, Options.Resolutions.Y / 2), 8.5, 0);
            shipBullets = new List<ShipBullet>();
            DmlFileReader fileReader = new DmlFileReader(Globals.pathToCurrentMap);
            fileReader.Parse();

            system = (DmlSystem)fileReader.GetResult();
            system.Begin();
        }

        public override void Update()
        {

            base.Update();

            //if this scene is the focused scene and if it's after 500ms from the initialization of this scene (500ms to align it with the song)
            if(focused&&After(500))
            {
                system.Update();
                player.Update();
                if (AtIntervals(100))//player shoots every 100ms
                {
                    shipBullets.Add(player.Shoot());
                }
                //updates each ship bullet, and adds points if it hit the offscreen enemy
                ShipBullet shipBullet;
                for (int i = shipBullets.Count - 1; i >= 0; i--)
                {
                    shipBullet = shipBullets[i];
                    shipBullet.Update();
                    if (shipBullet.position.Y < -10)
                    {
                        if (Options.Resolutions.X / 3 < shipBullet.position.X && shipBullet.position.X < 2 * Options.Resolutions.X / 3)
                        {
                            player.bulletsHit++;
                        }
                        shipBullets.Remove(shipBullet);
                    }
                }
                //checks if the player collides with te dml bullets
                if (player.immuneTime < 0)
                {
                    foreach (DmlBullet bullet in system.Bullets)
                    {
                        if (player.BulletCollide(bullet))
                        {
                            player.bulletsTaken++;
                            player.immuneTime = 100;
                            break;
                        }
                    }
                }
                //checks if the song ended
                if (Globals.nextBlock.p + 1 >= Globals.wave.get_data_p() + Globals.wave.get_data_size())
                {
                    Globals.songPaused = true;
                    //if the song ended and al;l the bullets have disappeared, it brings up the end scene
                    if (system.Bullets.Count <= 2)//2 generator bullets always stay on, so the count is 2 when there are no other bullets
                    {
                        manager.RegisterScene("End", new EndScene(player.bulletsHit, player.bulletsTaken));
                        manager.AddCurrentScene("End");
                        manager.FocusOn("End");
                        manager.changedCurrentScenes = true;
                        Globals.songPaused = false;

                    }

                }
            }
        }

        //draws everything in the level
        public override void Draw()
        {
            system.Draw();
            player.Draw();
            foreach (ShipBullet shipBullet in shipBullets)
            {
                shipBullet.Draw();
            }
            Globals.spriteBatch.DrawString(font, "Currently Playing   " + Globals.songname, new Vector2((int)(Options.Resolutions.X / 2), 0), Color.White);
            Globals.spriteBatch.DrawString(font, "Points    "+player.points, new Vector2((int)(Options.Resolutions.X / 2), 700), Color.White);
        }
    }
}


