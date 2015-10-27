using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core.Dml.Parse;
using Phosphaze.Core.Utils;
using System.Collections.Generic;

namespace Phosphaze.Core.Dml 
{

	public class DmlBullet 
	{
        public double bulletRadius = 7.5;

        /// <summary>
        /// The original position of this bullet.
        /// </summary>
        public Vector2 Origin { get; private set; }

        /// <summary>
        /// The position of this bullet relative to its origin.
        /// </summary>
        private Vector2 position = Vector2.Zero;

        public Vector2 RelativePosition { get { return position; } set { position = value; } }

        /// <summary>
        /// The actual position of the bullet relative to the world.
        /// </summary>
        public Vector2 Position { get { return Origin + position; } set { position = value - Origin; } }

        /// <summary>
        /// The direction in which this bullet is moving (down by default).
        /// </summary>
        public Vector2 Direction = new Vector2(0, 1);

        /// <summary>
        /// The speed at which this bullet is moving (0 by default).
        /// </summary>
        public double Speed = 0;

        /// <summary>
        /// The colour to tint this bullet's glow (colourless by default).
        /// </summary>
        public Color Colour = Color.White;
        // CANADA WILL PREVAIL

        /// <summary>
        /// Whether or not this bullet is dead.
        /// </summary>
        public bool Dead = false;

		/// <summary>
		/// The factory this bullet was created by.
		/// </summary>
		DmlBulletFactory factory;

        /// <summary>
        /// The local time of the bullet in milliseconds.
        /// </summary>
        public double LocalTime = 0;

        /// <summary>
        /// A list of all child bullets. If a BulletA instance spawns a BulletB
        /// instance, the BulletB instance becomes a child of the BulletA instance.
        /// </summary>
        public List<DmlBullet> Children = new List<DmlBullet>();

        /// <summary>
        /// For efficiency reasons, we store a reference in each bullet to it's
        /// associated DmlObject representation. This prevents us from having
        /// to cast DmlObject.Value to DmlBullet everytime we want to do something
        /// involving the DmlBullet.
        /// </summary>
        private DmlObject bulletObj;

        /// <summary>
        /// The radius of each bullet, used to test for collisions between the bullets and
        /// the player.
        /// </summary>
        public const double BulletRadius = 7.5;

        public Texture2D Sprite = null;

        public List<Component> Components = new List<Component>();

		public DmlBullet(Vector2 position, DmlBulletFactory factory)
		{
            Origin = position;
			this.factory = factory;
		}

        public void SetObject(DmlObject obj)
        {
            bulletObj = obj;
        }

        public void SetSprite(string sprite)
        {
            Sprite = Globals.content.Load<Texture2D>(sprite);
        }

        public void Update(DmlSystem system)
        {
            factory.Update.Execute(bulletObj, system);
            LocalTime += Globals.deltaTime;
            position += Direction * (float)Speed;

            foreach (var component in Components)
                component.Update(this, system);

            Components.RemoveAll(c => c.Dead);
        }

        public void Draw()
        {
            if (Sprite != null)
                DrawUtils.DrawCentered(Sprite, Position, Color.White);
        }

	}

}