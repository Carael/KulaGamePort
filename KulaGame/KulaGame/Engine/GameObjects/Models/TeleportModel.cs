using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class TeleportModel:BaseModel
    {
        public Vector3 DestinationPosition
        {
            get { return _destinationPosition; }
        }
        public Vector3 DestinationRotation
        {
            get { return _destinationRotation; }

        }
            private Vector3 _destinationPosition;
        private Vector3 _destinationRotation;

        private Matrix _localWorld;

        public TeleportModel(Model model, Vector3 position, Vector3 initialRotation, Vector3 destinationPosition, Vector3 destinationRotation) : base(model, position, initialRotation)
        {
            _destinationPosition = destinationPosition;
            _destinationRotation = destinationRotation;
            if (initialRotation.X == 180)
            {
                position = new Vector3(position.X, position.Y + 9, position.Z);
            }
            else if (initialRotation.Z == 180)
            {
                position = new Vector3(position.X, position.Y + 9, position.Z);
            }
            else
            {
                position = new Vector3(position.X, position.Y - 9, position.Z);
            }



            if (initialRotation.X == 90)
            {
                position = new Vector3(position.X, position.Y + 9, position.Z - 9);
            }
            if (initialRotation.X == -90)
            {
                position = new Vector3(position.X, position.Y + 9, position.Z + 9);
            }
            if (initialRotation.X == 270)
            {
                position = new Vector3(position.X, position.Y + 9, position.Z + 9);
            }

            if (initialRotation.Z == 90)
            {
                position = new Vector3(position.X +9, position.Y + 9, position.Z);
            }
            if (initialRotation.Z == -90)
            {
                position = new Vector3(position.X -9, position.Y + 9, position.Z);
            } 
            if (initialRotation.Z == 270)
            {
                position = new Vector3(position.X - 9, position.Y + 9, position.Z);
            }


            _volume = (BoundingBox)_model.Tag;
            _world = Matrix.Identity * Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(initialRotation.Y), MathHelper.ToRadians(initialRotation.X), MathHelper.ToRadians(initialRotation.Z)) * Matrix.CreateTranslation(position);
            _localWorld = Matrix.Identity * Matrix.CreateTranslation(position);

            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(2, 2, 2)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(2, 2, 2)) * _localWorld)));


        }


        public override void Update(GameTime gameTime)
        {
            
        }




        public override void Draw(GameTime gameTime, Inferfaces.ICamera camera)
        {
                Matrix[] transforms = new Matrix[_model.Bones.Count];
                _model.CopyAbsoluteBoneTransformsTo(transforms);

                var world = GetWorldMatrix();
                var position = world.Translation;
                world *= Matrix.CreateTranslation(-position);
                world *= Matrix.CreateScale(4);
                world *= Matrix.CreateTranslation(position);



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
                        effect.World = mesh.ParentBone.Transform * world;//*Matrix.CreateScale(0.8f);
                    }
                    mesh.Draw();
                }
        }

    }
}
