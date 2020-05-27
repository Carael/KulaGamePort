using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class CrystalModel : BaseModel
    {
        private int _angle = 0;

        public bool Active { get { return _active; } set { _active = value; } }

        private bool _active = true;

        private Matrix _localWorld;

        public CrystalModel(Model model, Vector3 position, Vector3 initialRotation)
            : base(model, position, initialRotation)
        {
            _localWorld = Matrix.Identity * Matrix.CreateTranslation(position);
        }


        public override void Update(GameTime gameTime)
        {
            if (_active)
            {
                _angle += 5;
                base.Update(gameTime);
            }

            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(1.5f, 1.5f, 1.5f)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(1.5f, 1.5f, 1.5f)) * _localWorld)));
        }


        public override void Draw(GameTime gameTime, Inferfaces.ICamera camera)
        {
            if (_active)
            {
                Matrix[] transforms = new Matrix[_model.Bones.Count];
                _model.CopyAbsoluteBoneTransformsTo(transforms);


                foreach (ModelMesh mesh in _model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {

                        effect.LightingEnabled = true;
                        var position = GetWorldMatrix().Translation;
                        var world = GetWorldMatrix();
                        world *= Matrix.CreateTranslation(-position);
                        world *= Matrix.CreateScale(3);
                        world *= Matrix.CreateFromAxisAngle(_world.Right, MathHelper.ToRadians(-90));
                        world *= Matrix.CreateFromAxisAngle(_world.Up, MathHelper.ToRadians(_angle));
                        world *= Matrix.CreateTranslation(position);


                        effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
                        effect.SpecularPower = 1.0f;
                        effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.9f);
                        effect.LightingEnabled = true;
                        effect.View = camera.View;
                        effect.Projection = camera.Projection;
                        effect.World = world;//*Matrix.CreateScale(0.8f);
                    }
                    mesh.Draw();
                }
            }

        }
    }
}
