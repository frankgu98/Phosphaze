using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core.SceneElements;
using Phosphaze.Core.Collision;
using Microsoft.Xna.Framework.Input;

namespace Phosphaze.Core
{
    class EndScene : Scene
    {
        SceneButton Back;
        SceneButton Retry;
        SpriteFont font,bigfont;
        Texture2D blank;
        int points, hits, dmg,tot,x1,x2;

        public override void Initialize()
        {
            blank = new Texture2D(Globals.graphics.GraphicsDevice, 1, 1);
            blank.SetData(new Color[] { Color.White });
            font = Globals.content.Load<SpriteFont>("NewSpriteFont");
            bigfont = Globals.content.Load<SpriteFont>("BiggerFont");
            Back = new SceneButton("Continue", "back_button", new Vector2(200, 200), new Rectangle(0, 645, 200, 75));
            points = Globals.wave.get_data_size()/10000 + hits * 100 - dmg * 300;
            tot = Globals.wave.get_data_size() / 10000 + hits * 100;
            x2 = 300 * dmg * 300 / tot;
            x1 = 300 * (tot - dmg * 300) / tot;
        }

        public EndScene(int hits, int dmg)
        {
            this.hits = hits;
            this.dmg = dmg;
        }

        public override void Update()
        {
            //check collision with buttons
            if (CollisionChecker.Collision(new RectCollider(new Rectangle(480, 200, x1, 30))
            , new ParticleCollider(Globals.currentMouseState.Position)))
            {
                this.Draw();
                Globals.spriteBatch.Begin();
                Globals.spriteBatch.Draw(blank, new Rectangle(300, 270, 250, 40), Color.Violet);
                Globals.spriteBatch.End();
            }
            if (Back.Collide(Globals.currentMouseState.Position)){
                if (Globals.currentMouseState.LeftButton == ButtonState.Pressed &&
Globals.previousMouseState.LeftButton == ButtonState.Released)
                {
                    manager.AlmostCurrentScenes.Add("Background");
                    manager.AlmostCurrentScenes.Add("Main Menu");
                    manager.FocusOn("Main Menu");
                    manager.transitioningScenes = true;
                }
            }
        }

        public override void Draw()
        {
            try 
            {
                Globals.spriteBatch.Begin();
            }
            catch { }
            //Globals.spriteBatch.Draw(blank, new Rectangle(200, 20, 880, 680), Color.Wheat);
            Globals.spriteBatch.DrawString(bigfont, "Results", new Vector2(520, 50), Color.White);
            Globals.spriteBatch.DrawString(font, String.Format("{0:0000000} P", points), new Vector2(510, 150), Color.White);
            Globals.spriteBatch.Draw(blank, new Rectangle(480, 200, x1, 30), Color.Green);
            Globals.spriteBatch.Draw(blank, new Rectangle(480+x1, 200, x2, 30), Color.Red);
            //Globals.spriteBatch.Draw(blank, new Rectangle(220, 250, 840, 200), Color.Turquoise);
            Globals.spriteBatch.DrawString(font, String.Format("Hits Dealt  {0:0000000}", hits), new Vector2(480, 270), Color.White);
            Globals.spriteBatch.DrawString(font, String.Format("Hits Taken {0:0000000}", dmg), new Vector2(480, 320), Color.White);
            Back.Draw();
        }

    }
    
}
