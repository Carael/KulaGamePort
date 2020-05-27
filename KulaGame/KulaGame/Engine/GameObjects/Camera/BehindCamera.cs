using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.Inferfaces;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;

namespace KulaGame.Engine.GameObjects.Camera
{
    public class BehindCamera : ICamera
    {
        public Matrix View { get; private set; }
        public Matrix Projection { get; private set; }
        public Vector3 CameraPosition { get; private set; }
        public Vector3 Up { get; set; }
        readonly Vector3 CameraPositionOffset = new Vector3(0, 20, 60);
        readonly Vector3 CameraTargetOffset = new Vector3(0, 15, 0);
        public Vector3 CameraZoom { get; set; }
        public Matrix CameraRotationMatrix { get; private set; }
        public Vector3 ThirdPersonReference { get; set; }
        private int _angle = 0;
        private bool _beginZoomIn = true;

        public BehindCamera(float aspectRatio, Vector3 objectPosition, Vector3 initialRotation)
        {
            CameraRotationMatrix = Matrix.Identity*
                                   Matrix.CreateFromYawPitchRoll(MathHelper.ToRadians(initialRotation.Y),
                                                                 MathHelper.ToRadians(initialRotation.X),
                                                                 MathHelper.ToRadians(initialRotation.Z));

            Up = CameraRotationMatrix.Up;
            CameraPosition = ThirdPersonReference + objectPosition;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), aspectRatio, 45, 1000000);
            View = Matrix.CreateLookAt(CameraPosition, objectPosition, Up);

        }
        public void UpdateCamera(Vector3 objectPosition , Matrix rotation, Vector3 up)
        {
            //update camera
            Up = up;
            CameraRotationMatrix = Matrix.Lerp(CameraRotationMatrix, rotation, 0.2f);
            Vector3 positionOffset = Vector3.Transform(CameraPositionOffset + CameraZoom,
                CameraRotationMatrix*Matrix.CreateFromAxisAngle(CameraRotationMatrix.Up, MathHelper.ToRadians(_angle)));
            Vector3 targetOffset = Vector3.Transform(CameraTargetOffset,
                CameraRotationMatrix);
            CameraPosition = objectPosition + positionOffset;
            Vector3 cameraTarget = objectPosition + targetOffset;
            View = Matrix.CreateLookAt(CameraPosition,cameraTarget,Up);
        }
        public void ZoomInTheCamera()
        {
            if (_beginZoomIn)
            {
                _beginZoomIn = false;
                CameraZoom=new Vector3(0,360,360);
                _angle = 0;
            }
            if (_angle != 360)
            {
                _angle += 3;
                CameraZoom-=new Vector3(0,3,3);
            }
        }
        public void ZoomOutTheCamera()
        {

            _angle += 2;
            if (CameraZoom.Y < 4000)
            {
                CameraZoom += new Vector3(0, 5, 5);
            }

        }
        public void ResetZoom()
        {
            _beginZoomIn = true;
            _angle = 0;
            CameraZoom=new Vector3(0,0,0);
        }
        public void ZoomInTheCamera_Pinch()
        {
            if (CameraZoom.Y>0 && CameraZoom.Z>0)
            {
                CameraZoom += new Vector3(0, -1, -1);
            }
            
        }
        public void ZoomOutTheCamera_Pinch()
        {
            if (CameraZoom.Y < 30 && CameraZoom.Z < 30)
            {
                CameraZoom += new Vector3(0, 1, 1);
            }
        }
    }
}
