using System.Collections.Generic;
using System.Linq;
using KulaGame.Engine.Inferfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models.Lists
{
    public class CoinsModelList : BaseModelList, IModelList
    {
        public CoinsModelList(Model model) : base(model)
        {
        }

        public List<CoinModel> GetModels()
        {
            List<CoinModel> coinModels = new List<CoinModel>();
            foreach (BaseModel baseModel in _models.ToList())
            {
                coinModels.Add((CoinModel)baseModel);
            }

            return coinModels;
        }

        public void AddModel(Vector3 position)
        {
            _models.Add(new CoinModel(_model, position, new Vector3(0, 0, 0)));
        }

        public void AddModel(Vector3 position, Vector3 initialRotation)
        {
            _models.Add(new CoinModel(_model, position, initialRotation));
        }
    }
}
