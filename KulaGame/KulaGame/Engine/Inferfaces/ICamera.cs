using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace KulaGame.Engine.Inferfaces
{
    public interface ICamera
    {
        Matrix View { get; }
        Matrix Projection { get; }
        Vector3 CameraPosition { get; }
        Vector3 Up { get; }
        void UpdateCamera(Vector3 vector, Matrix rotation, Vector3 up);
    }
}
