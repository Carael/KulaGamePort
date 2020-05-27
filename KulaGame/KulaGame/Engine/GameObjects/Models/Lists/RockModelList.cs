using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Inferfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models.Lists
{
    public class RockModelList : BaseModelList, IModelList
    {
        public RockModelList(Model model)
            : base(model)
        {
        }

        public List<RockModel> GetModels()
        {
            List<RockModel> rockModels = new List<RockModel>();
            foreach (BaseModel baseModel in _models.ToList())
            {
                rockModels.Add((RockModel)baseModel);
            }

            return rockModels;
        }

        public void AddModel(Vector3 position)
        {
            _models.Add(new RockModel(_model, position, new Vector3(0, 0, 0)));
        }

        public void AddModel(Vector3 position, Vector3 initialRotation)
        {
            _models.Add(new RockModel(_model, position, initialRotation));
        }
    }
}
