using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class SkyboardModel:BaseModel
    {

        private Matrix _localWorld;

        public SkyboardModel(Model model, Vector3 position, Vector3 initialRotation) : base(model, position, initialRotation)
        {
            _localWorld = Matrix.Identity * Matrix.CreateTranslation(position);

            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(0.1f, 0.1f, 0.1f)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(0.1f, 0.1f, 0.1f)) * _localWorld)));
        }

        public SkyboardModel(Model model, Vector3 position, Vector3 initialRotation, Vector3 originPointOffset) : base(model, position, initialRotation, originPointOffset)
        {
            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(0.1f, 0.1f, 0.1f)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(0.1f, 0.1f, 0.1f)) * _localWorld)));
        }

        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, Inferfaces.ICamera camera)
        {
            Matrix[] transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
                    effect.SpecularPower = 1.0f;
                    effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.9f);
                    effect.LightingEnabled = true;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = mesh.ParentBone.Transform * GetWorldMatrix();//*Matrix.CreateScale(0.8f);
                }
                mesh.Draw();
            }
        }
    }
}
