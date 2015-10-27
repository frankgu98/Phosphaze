//Frank Gu

using Phosphaze.Core.SceneElements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phosphaze.Core
{
    public class StarsScene : Scene
    {

        LinkedList<int> recentEnergyLevels;

        float speed;

        SceneStar[] stars;

        public StarsScene()
        {
            stars = new SceneStar[300];
            recentEnergyLevels = new LinkedList<int>();

            //if buffer duration is 250000hns, and we keep 20 items in list, we're looking at the last .375 seconds when getting averages
            //fills the lists with 0s to begin
            for (int i = 0; i < 20; i++)
            {
                updateList(0);
            }

            
            
        }

        public override void Initialize()
        {
            
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i]=new SceneStar();
            }
        }

        public override void Update()
        {
            base.Update();
            if (Globals.wave != null && Globals.nextBlock != null)
            {
                updateList(Math.Abs(Globals.wave.getStereoObject(Globals.nextBlock).getAvg()));
                //using the change in energy to get a nice looking speed for the stars, also adjust for how loudly the song is currently playing
                speed = (float)Math.Pow(Options.Volumes.MusicVolume * Options.Volumes.GlobalVolume * recentEnergyLevels.Average() / 1000, 1.5);
                speed = speed < 0 ? .5f : speed;

                foreach (SceneStar star in stars)
                {
                    star.speed = speed;
                    star.Update();
                }
            }
            

            
            
        }

        public override void Draw()
        {
            //Globals.spriteBatch.Begin();
            foreach (SceneStar star in stars)
            {
                star.Draw();
            }
            //Globals.spriteBatch.End();
        }

        // updates recentEnergyLevels and keeps it at a constant length
        private void updateList(int newLevel)
        {
            recentEnergyLevels.AddFirst(newLevel);
            if (recentEnergyLevels.Count > 20)
            {
                recentEnergyLevels.RemoveLast();
            }

        }

    }
}



