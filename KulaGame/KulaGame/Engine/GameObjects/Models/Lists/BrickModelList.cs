using System.Collections.Generic;
using System.Linq;
using KulaGame.Engine.Inferfaces;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models.Lists
{
    public class BrickModelList : BaseModelList, IModelList
    {
        public BrickModelList(Model model) : base(model)
        {
        }

        public List<BrickModel> GetModels()
        {
            List<BrickModel> brickModels = new List<BrickModel>();
            foreach (BaseModel baseModel in _models.ToList())
            {
                brickModels.Add((BrickModel)baseModel);
            }

            return brickModels;
        }

        public void AddModel(Vector3 position)
        {
            _models.Add(new BrickModel(_model, position, new Vector3(0, 0, 0), BrickType.Normal));
        }

        public void AddModel(Vector3 position, Vector3 initialRotation)
        {
            _models.Add(new BrickModel(_model, position, initialRotation, BrickType.Normal));
        }

        public void AddRubberBrickModel(BrickModel rubberBrickModel)
        {
            _models.Add(rubberBrickModel);
        }
    }
}
