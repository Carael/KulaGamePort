using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class SkyBox : BaseModel
    {
        public SkyBox(Model model, Vector3 position, Vector3 initialRotation)
            : base(model, position, initialRotation)
        {
            _model = model;
            _world = Matrix.CreateScale(100)*Matrix.CreateTranslation(position);
        }
        public override void Draw(GameTime gameTime, Inferfaces.ICamera camera)
        {
            Matrix[] transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = mesh.ParentBone.Transform * GetWorldMatrix();
                }
                mesh.Draw();
            }
        }
    }
}
