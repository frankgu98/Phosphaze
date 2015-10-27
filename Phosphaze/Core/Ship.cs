//Frank Gu
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Phosphaze.Core;

namespace Phosphaze.Core
{
    public class Ship
    {
        Vector3 cameraPosition = new Vector3(0.0f, 50.0f, 5000.0f);
        private Vector3 pos = new Vector3(400, 400, 0);
        private float width, height, rotation;
        Model myModel;
        float aspectRatio;

        public void LoadContent()
        {
            myModel = Globals.Content.Load<Model>("models\\ship2");
            aspectRatio = Globals.graphics.GraphicsDevice.Viewport.AspectRatio;
        }
        public Ship(float x, float y, float w, float h)
        {
            pos = new Vector3(x, y, 0);
            width = w;
            height = h;
            rotation = 0;
        }

        public void Update()
        {
        rotation += (float)Globals.gameTime.ElapsedGameTime.TotalMilliseconds *
            MathHelper.ToRadians(0.1f);
        }

        public void CenterOnPos(float x, float y)
        {
            pos.X = x - width / 2;
            pos.Y = y - height / 2;
        }

        public void Draw()
        {
            Matrix[] transforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * 
                        Matrix.CreateRotationY(rotation)
                        * Matrix.CreateTranslation(pos);
                    effect.View = Matrix.CreateLookAt(cameraPosition, 
                        Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45.0f), aspectRatio, 
                        1.0f, 10000.0f);
                }
                mesh.Draw();
            }
        }
    }
}
