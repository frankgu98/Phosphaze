using Microsoft.Xna.Framework;
using Phosphaze.Core.Dml.Parse;

namespace Phosphaze.Core.Dml
{
    public class DmlBulletFactory
    {

        /// <summary>
        /// The name of this bullet factory/class as seen in the Dml code.
        /// </summary>
        public readonly string Name;

    	// The bullet Init block.
 		public CodeBlock Init { get; private set; }

        // The bullet Update block.
 		public CodeBlock Update { get; private set; }

 		public DmlBulletFactory(string name, CodeBlock init, CodeBlock update)
 		{
            Name = name;
 			Init = init;
 			Update = update;
 		}

 		public DmlObject Instantiate(Vector2 Position, DmlSystem system) 
 		{
            DmlBullet bullet = new DmlBullet(Position, this);
            DmlObject bulletObject = new DmlObject(DmlType.Bullet, bullet);
            bullet.SetObject(bulletObject);

            // Execute the initialziation code.
            Init.Execute(bulletObject, system);

            return bulletObject;
 		}

    }
}
