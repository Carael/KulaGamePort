using System;
using System.Collections.Generic;
using KulaGame.Engine.Inferfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models.Lists
{
    public class FruitModelList : BaseModelList, IModelList
    {
        public FruitModelList(Model model) : base(model)
        {

        }



        public void AddModel(Vector3 position)
        {
            _models.Add(new FruitModel(_model, position, new Vector3(0, 0, 0), 1));
        }

        public void AddModel(Vector3 position, Vector3 initialRotation)
        {
            _models.Add(new FruitModel(_model, position, initialRotation, 1));
        }


        public void AddModel(Vector3 position, Vector3 initialRotation, float scaleFactor)
        {
            Vector3 fruitOffset = new Vector3(0,0,0);
            _models.Add(new FruitModel(_model, position, initialRotation, scaleFactor));
        }
    }
}
