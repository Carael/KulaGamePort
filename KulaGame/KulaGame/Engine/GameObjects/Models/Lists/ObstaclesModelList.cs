using KulaGame.Engine.Inferfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models.Lists
{
    public class ObstaclesModelList : BaseModelList, IModelList
    {
        public ObstaclesModelList(Model model) : base(model)
        {
        }

        public void AddModel(Vector3 position)
        {
            _models.Add(new ObstacleModel(_model, position, new Vector3(0, 0, 0)));
        }

        public void AddModel(Vector3 position, Vector3 initialRotation)
        {
            _models.Add(new ObstacleModel(_model, position, initialRotation));
        }
    }
}
