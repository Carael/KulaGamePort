using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class RockModel:BaseModel
    {

        private Matrix _localWorld;

        private bool _goinDown = false;
        private readonly int _upLimit = 40;
        private readonly int _downLimit =0;
        private int _currentOffset = 0;


        public RockModel(Model model, Vector3 position, Vector3 initialRotation) : base(model, position, initialRotation)
        {
            _localWorld = Matrix.Identity *Matrix.CreateScale(2)* Matrix.CreateTranslation(position);
        }

        public RockModel(Model model, Vector3 position, Vector3 initialRotation, Vector3 originPointOffset) : base(model, position, initialRotation, originPointOffset)
        {
            _localWorld = Matrix.Identity * Matrix.CreateScale(2) * Matrix.CreateTranslation(position);
        }

        public override void Update(GameTime gameTime)
        {
            if (_currentOffset == _upLimit)
            {
                _goinDown = true;
            }
            else if (_currentOffset == _downLimit)
            {
                _goinDown = false;
            }





            CameraRotationAxis upVector = Common.CameraRotationAxis(_world.Up);
            var position = _world.Translation;
            switch (upVector)
            {
                case CameraRotationAxis.X:
                    if (position.X > 0)
                    {

                        //w dó³
                        if (_goinDown)
                        {
                            _currentOffset -= 1;
                            _localWorld *= Matrix.CreateTranslation(-1, 0, 0);
                            _world *= Matrix.CreateTranslation(-1,0, 0);
                        }
                        //w górê
                        else
                        {
                            _currentOffset += 1;
                            _localWorld *= Matrix.CreateTranslation(1,0, 0);
                            _world *= Matrix.CreateTranslation(1,0, 0);
                        }

                    }
                    else
                    {
                        //w dó³
                        if (_goinDown)
                        {
                            _currentOffset -= 1;
                            _localWorld *= Matrix.CreateTranslation(1, 0, 0);
                            _world *= Matrix.CreateTranslation(1, 0, 0);
                        }
                        //w górê
                        else
                        {
                            _currentOffset += 1;
                            _localWorld *= Matrix.CreateTranslation(-1, 0, 0);
                            _world *= Matrix.CreateTranslation(-1, 0, 0);
                        }
                    }

                    break;
                case CameraRotationAxis.Y:
                    if (position.Y > 0)
                    {
                        //w dó³
                        if (_goinDown)
                        {
                            _currentOffset -= 1;
                            _localWorld *= Matrix.CreateTranslation(0,-1, 0);
                            _world *= Matrix.CreateTranslation(0,-1, 0);
                        }
                        //w górê
                        else
                        {
                            _currentOffset += 1;
                            _localWorld *= Matrix.CreateTranslation(0,1, 0);
                            _world *= Matrix.CreateTranslation(0,1, 0);
                        }
                    }
                    else
                    {
                        //w dó³
                        if (_goinDown)
                        {
                            _currentOffset -= 1;
                            _localWorld *= Matrix.CreateTranslation(0, 1, 0);
                            _world *= Matrix.CreateTranslation(0, 1, 0);
                        }
                        //w górê
                        else
                        {
                            _currentOffset += 1;
                            _localWorld *= Matrix.CreateTranslation(0, -1, 0);
                            _world *= Matrix.CreateTranslation(0, -1, 0);
                        }
                    }

                    break;
                case CameraRotationAxis.Z:

                    if (position.Z > 0)
                    {
                        //w dó³
                        if (_goinDown)
                        {
                            _currentOffset -= 1;
                            _localWorld *= Matrix.CreateTranslation(0, 0,-1);
                            _world *= Matrix.CreateTranslation(0, 0,-1);
                        }
                        //w górê
                        else
                        {
                            _currentOffset += 1;
                            _localWorld *= Matrix.CreateTranslation(0, 0,1);
                            _world *= Matrix.CreateTranslation(0, 0,1);
                        }
                    }
                    else
                    {
                        //w dó³
                        if (_goinDown)
                        {
                            _currentOffset -= 1;
                            _localWorld *= Matrix.CreateTranslation(0, 0, 1);
                            _world *= Matrix.CreateTranslation(0, 0, 1);
                        }
                        //w górê
                        else
                        {
                            _currentOffset += 1;
                            _localWorld *= Matrix.CreateTranslation(0, 0, -1);
                            _world *= Matrix.CreateTranslation(0, 0, -1);
                        }
                    }
                    break;
            }



            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(2, 2, 2)) * _localWorld)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(2, 2, 2)) * _localWorld)));
        
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
