using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Inferfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models.Lists
{
    public class SpoolModelList : BaseModelList, IModelList
    {
        public SpoolModelList(Model model)
            : base(model)
        {
        }

        public List<SpoolModel> GetModels()
        {
            List<SpoolModel> spoolModels = new List<SpoolModel>();
            foreach (BaseModel baseModel in _models.ToList())
            {
                spoolModels.Add((SpoolModel)baseModel);
            }

            return spoolModels;
        }

        public void AddModel(Vector3 position)
        {
            _models.Add(new SpoolModel(_model, position, new Vector3(0, 0, 0)));
        }

        public void AddModel(Vector3 position, Vector3 initialRotation)
        {
            _models.Add(new SpoolModel(_model, position, initialRotation));
        }
    }
}
