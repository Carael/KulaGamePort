using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class ObstacleModel:BaseModel
    {
        private Matrix _localWorld;

        public ObstacleModel(Model model, Vector3 position, Vector3 initialRotation): base(model, position, initialRotation)
        {
            if (initialRotation.X == 180)
            {
                position = new Vector3(position.X, position.Y + 10, position.Z);
            }
            else if (initialRotation.Z == 180)
            {
                position = new Vector3(position.X, position.Y + 10, position.Z);
            }
            else
            {
                position = new Vector3(position.X, position.Y - 10, position.Z);
            }


            if (initialRotation.X == 90)
            {
                position = new Vector3(position.X, position.Y + 10, position.Z - 10);
            }
            if (initialRotation.X == -90)
            {
                position = new Vector3(position.X, position.Y + 10, position.Z + 10);
            }
            if (initialRotation.X == 270)
            {
                position = new Vector3(position.X, position.Y + 10, position.Z + 10);
            }

            if (initialRotation.Z == 90)
            {
                position = new Vector3(position.X - 10, position.Y + 10, position.Z);
            }
            if (initialRotation.Z == -90)
            {
                position = new Vector3(position.X + 10, position.Y + 10, position.Z);
            }
            if (initialRotation.Z == 270)
            {
                position = new Vector3(position.X + 10, position.Y + 10, position.Z);
            }
            
            _volume = (BoundingBox)_model.Tag;
            _world = Matrix.Identity * Matrix.CreateScale(2) * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(initialRotation.Y), MathHelper.ToRadians(initialRotation.X), MathHelper.ToRadians(initialRotation.Z)) * Matrix.CreateTranslation(position);
            _localWorld = Matrix.Identity  * Matrix.CreateTranslation(position);

            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(2, 2, 2)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(2, 2, 2)) * _localWorld)));
     

        }

        public override void Update(GameTime gameTime)
        {
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
