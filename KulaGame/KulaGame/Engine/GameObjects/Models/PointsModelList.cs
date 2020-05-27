using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.GameObjects.Models.Lists;
using KulaGame.Engine.Inferfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models
{
    public class PointsModelList : BaseModelList, IModelList
    {
        public PointsModelList(Model model) : base(model)
        {
        }

        public void AddModel(Vector3 position)
        {
            _models.Add(new PointModel(_model, position));
        }

        public void AddModel(Vector3 position, Vector3 initialRotation)
        {
            throw new NotImplementedException();
        }
    }
}
