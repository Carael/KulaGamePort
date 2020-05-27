using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Inferfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models.Lists
{
    public class SkyboardModelList : BaseModelList, IModelList
    {
        public SkyboardModelList(Model model) : base(model)
        {
        }

        public List<SkyboardModel> GetModels()
        {
            List<SkyboardModel> skyboardModels = new List<SkyboardModel>();
            foreach (BaseModel baseModel in _models.ToList())
            {
                skyboardModels.Add((SkyboardModel)baseModel);
            }

            return skyboardModels;
        }

        public void AddModel(Vector3 position)
        {
            _models.Add(new SkyboardModel(_model, position, new Vector3(0, 0, 0)));
        }

        public void AddModel(Vector3 position, Vector3 initialRotation)
        {
            _models.Add(new SkyboardModel(_model, position, initialRotation));
        }
    }
}
