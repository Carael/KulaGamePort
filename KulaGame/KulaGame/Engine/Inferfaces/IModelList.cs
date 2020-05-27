using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.GameObjects.Models;
using Microsoft.Xna.Framework;

namespace KulaGame.Engine.Inferfaces
{
    public interface IModelList
    {

        List<BaseModel> GetModels();
        void AddModel(Vector3 position);
        void AddModel(Vector3 position, Vector3 initialRotation);
    }
}
