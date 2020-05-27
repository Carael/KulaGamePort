using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class SpoolModel : BaseModel
    {
        private int _angle = 0;
        private Matrix _localWorld;
        private Matrix _tempWorld = Matrix.Identity;
        public SpoolModel(Model model, Vector3 position, Vector3 initialRotation)
            : base(model, position, initialRotation)
        {
            _localWorld = Matrix.Identity * Matrix.CreateTranslation(position); _world = Matrix.Identity * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(initialRotation.Y), MathHelper.ToRadians(initialRotation.X), MathHelper.ToRadians(initialRotation.Z)) * Matrix.CreateTranslation(position);
        }
        public SpoolModel(Model model, Vector3 position, Vector3 initialRotation, Vector3 originPointOffset)
            : base(model, position, initialRotation, originPointOffset)
        {
            _localWorld = Matrix.Identity * Matrix.CreateTranslation(position);

        }
        public override void Update(GameTime gameTime)
        {
            _angle += 5;
            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(1, 1, 1)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(1, 1, 1)) * _localWorld)));
            _tempWorld = GetWorldMatrix();
            var position = _tempWorld.Translation;
            _tempWorld *= Matrix.CreateTranslation(-position);
            CameraRotationAxis upVector = Common.CameraRotationAxis(_world.Up);
            switch (upVector)
            {
                case CameraRotationAxis.X:
                    _tempWorld *= Matrix.CreateTranslation(new Vector3(10, 0, 0));
                    break;
                case CameraRotationAxis.Y:
                    _tempWorld *= Matrix.CreateTranslation(new Vector3(0, 10, 0));
                    break;
                case CameraRotationAxis.Z:
                    _tempWorld *= Matrix.CreateTranslation(new Vector3(0, 0, 10));
                    break;
            }
            _tempWorld *= Matrix.CreateFromAxisAngle(_world.Right, MathHelper.ToRadians(_angle));
            _tempWorld *= Matrix.CreateScale(2);
            _tempWorld *= Matrix.CreateTranslation(position);
            position = _tempWorld.Translation;
            var positionLocal = _localWorld.Translation;
            _localWorld *= Matrix.CreateTranslation(-positionLocal);
            _localWorld *= Matrix.CreateTranslation(position);
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
                    effect.World = mesh.ParentBone.Transform * _tempWorld;
                }
                mesh.Draw();
            }
        }
    }
}
