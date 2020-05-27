using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Inferfaces;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Models.Lists
{
    public class TeleportModelList : BaseModelList, IModelList
    {

        private Model _redModel;
        private Model _blueModel;

        public TeleportModelList(Model redModel, Model blueModel)
            : base(redModel)
        {
            _redModel = redModel;
            _blueModel = blueModel;
        }

        public List<TeleportModel> GetModels()
        {
            List<TeleportModel> teleportModel = new List<TeleportModel>();
            foreach (BaseModel baseModel in _models.ToList())
            {
                teleportModel.Add((TeleportModel)baseModel);
            }

            return teleportModel;
        }

        public void AddModel(Vector3 position)
        {
            throw new NotImplementedException();
        }

        public void AddModel(Vector3 position, Vector3 initialRotation)
        {
            throw new NotImplementedException();
        }

        public void AddModel(Vector3 position, Vector3 destinationPosition, TeleportModelType teleportModelType)
        {
            if (teleportModelType == TeleportModelType.Red)
            {
                _models.Add(new TeleportModel(_redModel, position, new Vector3(0, 0, 0), destinationPosition, new Vector3(0, 0, 0)));
            }
            if (teleportModelType == TeleportModelType.Blue)
            {
                _models.Add(new TeleportModel(_blueModel, position, new Vector3(0, 0, 0), destinationPosition, new Vector3(0, 0, 0)));
            }

        }

        public void AddModel(Vector3 position, Vector3 initialRotation, Vector3 destinationPosition, Vector3 destinationRotation, TeleportModelType teleportModelType)
        {
            if (teleportModelType == TeleportModelType.Red)
            {
                _models.Add(new TeleportModel(_redModel, position, initialRotation, destinationPosition, destinationRotation));
            }
            if (teleportModelType == TeleportModelType.Blue)
            {
                _models.Add(new TeleportModel(_blueModel, position, initialRotation, destinationPosition, destinationRotation));
            }

        }
    }
}
