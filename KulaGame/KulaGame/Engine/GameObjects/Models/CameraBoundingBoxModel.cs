using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class CameraBoundingBoxModel:BaseModel
    {
        private Matrix _localWorld;

        private const float ScaleFactor=20;

        public CameraBoundingBoxModel(Model model)
            : base(model, Vector3.Zero, Vector3.Zero)
        {
            _localWorld = Matrix.Identity;
            _world = Matrix.Identity;
        }

        public void Update(GameTime gameTime, Vector3 position)
        {

            _world = _localWorld * Matrix.CreateTranslation(position);
            _bounds = new BoundingBox(Common.RoundVector(Vector3.Transform(_volume.Min, Matrix.CreateScale(new Vector3(ScaleFactor, ScaleFactor, ScaleFactor)) * _world)), Common.RoundVector(Vector3.Transform(_volume.Max, Matrix.CreateScale(new Vector3(ScaleFactor, ScaleFactor, ScaleFactor)) * _world)));
        }

        public override void Update(GameTime gameTime)
        {
        }


        public override void Draw(GameTime gameTime, Inferfaces.ICamera camera)
        {
            
        }
    }
}
