using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KulaGame.Engine.Inferfaces;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models.Lists
{
    public abstract class BaseModelList : IEnumerable<BaseModel>
    {
        protected List<BaseModel> _models;
        protected Model _model;

        public BaseModelList(Model model)
        {
            _models = new List<BaseModel>();
            _model = model;
        }

        public virtual List<BaseModel> GetModels()
        {

            return _models.ToList();
        }

        public IEnumerator<BaseModel> GetEnumerator()
        {
            return _models.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _models.GetEnumerator();
        }
    }
}
