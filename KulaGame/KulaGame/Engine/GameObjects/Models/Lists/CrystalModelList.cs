using System.Collections.Generic;
using System.Linq;
using KulaGame.Engine.Inferfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models.Lists
{
    public class CrystalModelList : BaseModelList, IModelList
    {
        public CrystalModelList(Model model)
            : base(model)
        {
        }

        public List<CrystalModel> GetModels()
        {
            List<CrystalModel> crystalModels = new List<CrystalModel>();
            foreach (BaseModel baseModel in _models.ToList())
            {
                crystalModels.Add((CrystalModel)baseModel);
            }

            return crystalModels;
        }

        public void AddModel(Vector3 position)
        {
            _models.Add(new CrystalModel(_model, position, new Vector3(0, 0, 0)));
        }

        public void AddModel(Vector3 position, Vector3 initialRotation)
        {
            _models.Add(new CrystalModel(_model, position, initialRotation));
        }
    }
}
