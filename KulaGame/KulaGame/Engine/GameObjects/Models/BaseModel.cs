using System;
using KulaGame.Engine.GameObjects.Camera;
using KulaGame.Engine.Inferfaces;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public abstract class BaseModel:IModel
    {
        public BoundingBox BoundingBox
        {
            get { return _bounds; }
        }
        public Model Model
        {
            get { return _model; }
        }

        public BoundingBox Volume
        {
            get { return _volume; }
        }

        protected Model _model;
        protected Matrix _world;
        protected BoundingBox _volume;
        protected BoundingBox _bounds;

        public BaseModel(Model model, Vector3 position, Vector3 initialRotation)
        {
            _model = model;
            _world = Matrix.Identity;
            _volume = (BoundingBox)_model.Tag;
            _bounds = (BoundingBox)_model.Tag;
            _world = Matrix.Identity*Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(initialRotation.Y), MathHelper.ToRadians(initialRotation.X), MathHelper.ToRadians(initialRotation.Z))*Matrix.CreateTranslation(position);
        }

        public BaseModel(Model model, Vector3 position, Vector3 initialRotation, Vector3 originPointOffset)
        {
            _model = model;
            _world = Matrix.Identity;
            _volume = (BoundingBox)_model.Tag;
            _bounds = (BoundingBox)_model.Tag;
            _world = Matrix.Identity;
            _world *= Matrix.CreateTranslation(originPointOffset);
            _world *= Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(initialRotation.Y),
                                                    MathHelper.ToRadians(initialRotation.X),
                                                    MathHelper.ToRadians(initialRotation.Z));
            _world *= Matrix.CreateTranslation(position);
        }

        public virtual void Update(GameTime gameTime)
        {
            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, GetWorldMatrix())), Common.RoundVector((Vector3.Transform(_volume.Max, GetWorldMatrix()))));
        }

        public virtual Matrix GetWorldMatrix()
        {
            return _world;
        }

        public virtual void Draw(GameTime gameTime, ICamera camera)
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
                    effect.LightingEnabled = false;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = mesh.ParentBone.Transform*GetWorldMatrix();
                }
                mesh.Draw();
            }
        }

        public virtual void DrawTransparent(GameTime gameTime, BehindCamera camera)
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
                    effect.Alpha = 0.7f;
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = mesh.ParentBone.Transform * GetWorldMatrix();
                }
                mesh.Draw();
            }
        }
    }
}
