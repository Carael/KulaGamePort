using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class PointModel:BaseModel
    {
        private int _angle = 0;

        public bool Active { get { return _active; } set { _active = value; } }

        private bool _active = true;

        public PointModel(Model model, Vector3 position) : base(model, position, Vector3.Zero)
        {
        }


        public override void Update(GameTime gameTime)
        {
            if (_active)
            {
                _angle += 5;
                base.Update(gameTime);
            }

            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(1.5f, 1.5f, 1.5f)) * GetWorldMatrix())), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(1.5f, 1.5f, 1.5f)) * GetWorldMatrix())));
      
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
                        //effect.EnableDefaultLighting();


                        //effect.Alpha = .5f;
                        ////effect.DiffuseColor = new Vector3(0.1f, 0.9f, 0.1f);
                        //effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
                        //effect.SpecularPower = 1.0f;
                        //effect.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

                        //effect.DirectionalLight0.Enabled = true;
                        //effect.DirectionalLight0.DiffuseColor = Vector3.One;
                        //effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
                        //effect.DirectionalLight0.SpecularColor = Vector3.One;

                        //effect.LightingEnabled = true;
                        var position = GetWorldMatrix().Translation;
                        var world = GetWorldMatrix();
                        world *= Matrix.CreateTranslation(-position);
                        world *= Matrix.CreateFromAxisAngle(_world.Up, MathHelper.ToRadians(_angle));
                        world *= Matrix.CreateTranslation(position);




                        effect.EnableDefaultLighting();
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
