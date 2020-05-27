using KulaGame.Engine.Inferfaces;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class BrickModel:BaseModel
    {

        public BrickType BrickType { get; private set; }

        public BrickModel(Model model, Vector3 position, Vector3 initialRotation, Utils.BrickType brickType)
            : base(model, position, initialRotation)
        {
            BrickType = brickType;
            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(10, 10, 10)) * GetWorldMatrix())), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(10, 10, 10)) * GetWorldMatrix())));
        }

        public override void Update(GameTime gameTime)
        {
                    //base.Update(gameTime);
        }

        public void DrawTransparency(GameTime gameTime, ICamera camera, bool transparent)
        {

            Matrix[] transforms = new Matrix[_model.Bones.Count];
            _model.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in _model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();


                    //effect.Alpha = .5f;
                    //effect.DiffuseColor = new Vector3(0.1f, 0.9f, 0.1f);
                    //effect.SpecularColor = new Vector3(0.25f, 0.25f, 0.25f);
                    //effect.SpecularPower = 1.0f;
                    //effect.AmbientLightColor = new Vector3(0.75f, 0.75f, 0.75f);

                    //effect.DirectionalLight0.Enabled = true;
                    //effect.DirectionalLight0.DiffuseColor = Vector3.One;
                    //effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1.0f, -1.0f, -1.0f));
                    //effect.DirectionalLight0.SpecularColor = Vector3.One;

                    //effect.LightingEnabled = true;


                    //effect.EnableDefaultLighting();
                    if (transparent)
                    {
                        effect.Alpha = 0.5f;
                    }
                    effect.View = camera.View;
                    effect.Projection = camera.Projection;
                    effect.World = mesh.ParentBone.Transform * GetWorldMatrix();
                }
                mesh.Draw();
            }
        }
    }
}
