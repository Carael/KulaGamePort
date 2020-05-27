using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
//using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

namespace KulaGame.Engine.Utils
{
    public class AccelInput
    {
        #region Declarations

        public enum Direction
        {
            None,
            Forward,
            Back,
            Left,
            Right,
            Jump
        };

        public Direction direction = Direction.None;

        public bool IsTurnOn;
        public bool stateChanged = false;
        //private Accelerometer accelerometer;
        private float x, y, z;
        private Vector3 velocity = new Vector3(0, 0, 0);
        private float offSet = 0.2f;
        public Direction prevDirection;

        #endregion

        #region Initialize

        public AccelInput()
        {
            IsTurnOn = ReadAccelerometerStatus();
            //accelerometer = new Accelerometer();
            //accelerometer.ReadingChanged += AccelerometerReadingChanged;
            //accelerometer.Start();
        }

        #endregion

        #region Checking if Accelereometer is on

        private bool ReadAccelerometerStatus()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader reader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\accelerometer.txt", FileMode.OpenOrCreate, myStore));
            string valueS = reader.ReadLine();
            reader.Close();
            int value = 0;
            try
            {
                value = int.Parse(valueS);

            }
            catch (Exception)
            {
                value = 0;
            }

            return value == 1 ? true : false;
        }

        private void WriteAccelerometerValue()
        {
            if (IsTurnOn)
            {
                IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
                StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\accelerometer.txt", FileMode.OpenOrCreate, myStore));
                writer.Write(1);
                writer.Close();
            }
            else
            {
                IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
                StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\accelerometer.txt", FileMode.OpenOrCreate, myStore));
                writer.Write(0);
                writer.Close();
            }
        }
        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            if (IsTurnOn)
            {
                velocity = new Vector3(x, y, z);


                if (Math.Abs(x) < 0.25f && y < 0.16f && y > -0.4f)//Math.Abs(y) < 0.2f)
                {
                    direction = Direction.None;
                }

                prevDirection = direction;

                if (y >= 0.16f)
                {
                    direction = Direction.Forward;
                }
                if (y <= -0.4f && prevDirection == Direction.None)
                {
                    direction = Direction.Back;
                }

                if (x >= 0.25f && prevDirection == Direction.None)
                {
                    direction = Direction.Left;
                }
                if (x <= -0.25f && prevDirection == Direction.None)
                {
                    direction = Direction.Right;
                }

                if (z > 0.01f)//&& prevDirection == Direction.None)
                {
                    direction = Direction.Jump;
                }
            }

            if (stateChanged)
            {
                WriteAccelerometerValue();
                stateChanged = false;
            }
        }

        #endregion

        #region Private Methods

        //private void AccelerometerReadingChanged(object sender, AccelerometerReadingEventArgs e)
        //{
        //    if (IsTurnOn)
        //    {
        //        x = (float)e.Y;
        //        y = (float)e.X + offSet;
        //        z = (float)e.Z;
        //    }
        //}

        #endregion
    }
}
