using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class FlagModel:BaseModel
    {
        private int _angle = 0;
        private int _bounce = 0;
        private bool _bounceUpDirection = true;
        private int _timeElapsed;

        public bool Unlocked { get { return _unlocked; } set { _unlocked = value; } }

        private bool _unlocked = false;
        private Matrix _localWorld;


        public FlagModel(Model model, Vector3 position, Vector3 initialRotation): base(model, position, initialRotation)
        {
            _localWorld = Matrix.Identity * Matrix.CreateTranslation(position);
        }

        public override void Update(GameTime gameTime)
        {
            _angle += 5;
            _timeElapsed += gameTime.ElapsedGameTime.Milliseconds;

            if (_timeElapsed > 100)
            {
                if (_bounce == 5)
                {
                    _bounceUpDirection = false;
                }
                if (_bounce == 0)
                {
                    _bounceUpDirection = true;
                }

                if (_bounceUpDirection)
                {
                    _bounce += 1;
                }
                else
                {
                    _bounce -= 1;
                }

                _timeElapsed = 0;
            }


            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(1f, 1f, 1f)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(1f, 1f, 1f)) * _localWorld)));
   
        }

        public override void Draw(GameTime gameTime, Inferfaces.ICamera camera)
        {
                Matrix[] transforms = new Matrix[_model.Bones.Count];
                _model.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in _model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        var world = GetWorldMatrix();
                        
                        var position = GetWorldMatrix().Translation;
                        position.Y += _bounce;


                        world *= Matrix.CreateTranslation(-position);
                        world *= Matrix.CreateScale(2);
                        world *= Matrix.CreateFromAxisAngle(_world.Up, MathHelper.ToRadians(_angle));
                        
                        world *= Matrix.CreateTranslation(position);

                        if (_unlocked)
                        {
                            effect.Alpha = 1.0f;
                        }
                        else
                        {
                            effect.Alpha = 0.3f;
                        }
                        effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
                        effect.SpecularPower = 1.0f;
                        effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.9f);
                        effect.LightingEnabled = true;
                        effect.View = camera.View;
                        effect.Projection = camera.Projection;
                        effect.World = mesh.ParentBone.Transform*world;
                    }
                    mesh.Draw();
                }
        }
    }
}
