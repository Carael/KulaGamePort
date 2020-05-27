using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Inferfaces;
using Microsoft.Xna.Framework;

namespace KulaGame.Engine.GameObjects.Camera
{
    class BasicCamera:ICamera
    {
        private Game _game;
        public BasicCamera(Game game)
        {
            FacingDirection = 0;
            _game = game;
            View = Matrix.CreateLookAt(CameraPosition,
                                       Vector3.Zero, Up);
            Projection=Matrix.CreatePerspectiveFieldOfView(
                    MathHelper.ToRadians(45.0f), _game.GraphicsDevice.Viewport.AspectRatio, 1f, 1000000);
            CameraPosition=new Vector3(0.0f, 100.0f, 200.0f);
            Up = Vector3.Up;
        }

        public Matrix View { get; private set; }

        public Matrix Projection { get; set; }

        public Vector3 CameraPosition { get; private set; }

        public Vector3 Up { get; set; }

        public float FacingDirection { get; private set; }

        public void UpdateCamera(Vector3 objectPosition, Matrix rotation, Vector3 up)
        {
            throw new NotImplementedException();
        }
    }
}
