using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core.Dml.Parse;
using System;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml
{
    public class DmlSystem
    {

        /// <summary>
        /// The maximum number of bullets allowed on screen at once before we 
        /// disallow the spawning of new bullets. Anything more than 5000 bullets
        /// lags.
        /// </summary>
        public static int BULLET_CAP = 5000;

        /// <summary>
        /// The global objects in this system.
        /// </summary>
    	public Dictionary<string, DmlObject> GlobalVars { get; private set; }

        /// <summary>
        /// The global code to execute before the system actually begins.
        /// </summary>
        public CodeBlock GlobalCode;

        /// <summary>
        /// The currently active bullets.
        /// </summary>
        public List<DmlBullet> Bullets { get; private set; }
        
        /// <summary>
        /// The list of bullets to add to the system in the next frame. We can't
        /// directly add to the Bullets list because that would change the size of
        /// the list during iteration.
        /// </summary>
        private List<DmlBullet> ToAdd = new List<DmlBullet>();

        /// <summary>
        /// The timeline of elements.
        /// </summary>
        public DmlTimeline Timeline { get; private set; }

        /// <summary>
        /// Whether or not the system has started running.
        /// </summary>
        private bool Begun = false;

        /// <summary>
        /// The system's global time (in milliseconds).
        /// </summary>
        public double GlobalTime {
            get {
                return Timeline.LocalTime;
            }
        }

    	public DmlSystem(CodeBlock globalCode, DmlTimeline timeline, Dictionary<string, DmlBulletFactory> bullets)
    	{
            GlobalCode = globalCode;
            Timeline = timeline;
    		GlobalVars = new Dictionary<string, DmlObject>();
            foreach (var bulletClass in bullets)
                GlobalVars[bulletClass.Key] = new DmlObject(DmlType.BulletClass, bulletClass.Value);
            Bullets = new List<DmlBullet>();
    	}

        /// <summary>
        /// Add a bullet to the system.
        /// </summary>
        /// <param name="bullet"></param>
        public void AddBullet(DmlBullet bullet)
        {
            if (Bullets.Count >= DmlSystem.BULLET_CAP)
                return;
            if (Begun)
                ToAdd.Add(bullet);
            else
                Bullets.Add(bullet);
        }

        /// <summary>
        /// Begin running the system.
        /// </summary>
        public void Begin() 
        {
            GlobalCode.Execute(null, this);
            Begun = true;
            Timeline.Begin();
        }

        /// <summary>
        /// Update the system and all active bullets contained in it.
        /// </summary>
        public void Update()
        {
            if (!Begun)
                return;
            // Update the bullets.
            foreach (DmlBullet bullet in Bullets)
                bullet.Update(this);

            // Add the newly spawned bullets.
            foreach (DmlBullet bullet in ToAdd)
            {
                if (Bullets.Count >= DmlSystem.BULLET_CAP)
                    break;
                Bullets.Add(bullet);
            }

            ToAdd.Clear();

            // Remove all the dead bullets.
            Bullets.RemoveAll(b => b.Dead);

            // Update the timeline.
            Timeline.Update(this);
        }

        /// <summary>
        /// Draw the system and all active bullets contained in it.
        /// </summary>
        public void Draw()
        {
            //Globals.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            foreach (DmlBullet bullet in Bullets)
                bullet.Draw();

            //Globals.spriteBatch.End();
        }
    }
}
