using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace KulaGame.Engine.Utils
{
    public class Pad
    {
        #region Declarations

        public bool IsVisible = true;
        // Dpad variables
        private int dpadTimer = 0;
        private Texture2D dpadTexture;
        private Vector2 dpadPosition = Vector2.Zero;
        private Vector2 dpadDefaultPosition = Vector2.Zero;
        private Vector2 dpadOrigin = Vector2.Zero;
        public Color DpadColor = Color.CornflowerBlue;
        public Rectangle DpadRect;
        public Rectangle UpRect;
        public Rectangle DownRect;
        public Rectangle LeftRect;
        public Rectangle RightRect;
        // JumpPad variables
        private int jumpTimer = 0;
        private Texture2D jumpTexture;
        private Vector2 jumpPosition = Vector2.Zero;
        private Vector2 jumpDefaultPosition = Vector2.Zero;
        private Vector2 jumpOrigin = Vector2.Zero;
        public Color JumpColor = Color.CornflowerBlue;
        public  Rectangle JumpRect;
        private AccelInput accelerometer;
        #endregion

        #region Initialization

        public Pad(ContentManager content, Vector2 DPadPosition, Vector2 JumpPadPosition, AccelInput accelerometer)
        {
            this.accelerometer = accelerometer;
            //Dpad
            dpadTexture = content.Load<Texture2D>(@"Control\xboxControllerDPad");
            dpadPosition = DPadPosition;
            dpadDefaultPosition = dpadPosition;
            dpadOrigin = new Vector2(dpadTexture.Width / 2, dpadTexture.Height / 2);
            DpadRect = new Rectangle((int)(dpadPosition.X - dpadOrigin.X), (int)(dpadPosition.Y - dpadOrigin.Y), (int)(dpadPosition.X + dpadOrigin.X), (int)(dpadPosition.Y + dpadOrigin.Y));
            UpRect = new Rectangle((int)(dpadPosition.X - 45), (int)(dpadPosition.Y - (dpadOrigin.Y + 15)), (int)(dpadPosition.X + 45), (int)(dpadPosition.Y - 30));
            DownRect = new Rectangle((int)(dpadPosition.X - 45), (int)(dpadPosition.Y + 30), (int)(dpadPosition.X + 45), (int)(dpadPosition.Y + (dpadOrigin.Y + 15)));
            LeftRect = new Rectangle((int)(dpadPosition.X - (dpadOrigin.X + 15)), (int)(dpadPosition.Y - 45), (int)(dpadPosition.X - 30), (int)(dpadPosition.Y + 45));
            RightRect = new Rectangle((int)(dpadPosition.X + 30), (int)(dpadPosition.Y - 45), (int)(dpadPosition.X + (dpadOrigin.X + 15)), (int)(dpadPosition.Y + 45));
            // Jump
            jumpTexture = content.Load<Texture2D>(@"Control\xboxControllerButtonA");
            jumpPosition = JumpPadPosition;
            jumpDefaultPosition = jumpPosition;
            jumpOrigin = new Vector2(jumpTexture.Width / 2, jumpTexture.Height / 2);
            JumpRect = new Rectangle((int)(jumpPosition.X - jumpOrigin.X), (int)(jumpPosition.Y - jumpOrigin.Y), (int)(jumpPosition.X + jumpOrigin.X), (int)(jumpPosition.Y + jumpOrigin.Y));
        }

        #endregion

        #region Update and Draw
        
        public void Update(GameTime gameTime)
        {
            if (IsVisible)
            {
                if (accelerometer.IsTurnOn)
                {
                    IsVisible = false;
                    return;
                }
                if (dpadPosition.X != dpadDefaultPosition.X)
                {
                    if (dpadPosition.X < dpadDefaultPosition.X)
                    {
                        dpadPosition.X += 25;
                    }
                    else
                    {
                        dpadPosition.X = dpadDefaultPosition.X;
                    }
                }

                if (jumpPosition.X != jumpDefaultPosition.X)
                {
                    if (jumpPosition.X > jumpDefaultPosition.X)
                    {
                        jumpPosition.X -= 25;
                    }
                    else
                    {
                        jumpPosition.X = jumpDefaultPosition.X;
                    }
                }

                if (DpadColor == Color.Gray)
                {
                    dpadTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (dpadTimer > 100)
                    {
                        DpadColor = Color.CornflowerBlue;
                        dpadTimer = 0;
                    }
                }
                else if (JumpColor == Color.Green)
                {
                    jumpTimer += gameTime.ElapsedGameTime.Milliseconds;
                    if (jumpTimer > 100)
                    {
                        JumpColor = Color.CornflowerBlue;
                        jumpTimer = 0;
                    }
                }
            }
            else
            {
                if (dpadPosition.X <= -100)
                {
                    dpadPosition.X = -100;
                }
                else
                {
                    dpadPosition.X -= 25;
                }

                if (jumpPosition.X >= 870)
                {
                    jumpPosition.X = 870;
                }
                else
                {
                    jumpPosition.X += 25;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /*if (isVisible)
            {
                spriteBatch.Draw(dpadTexture, dpadPosition, null, new Color(dpadColor.R, dpadColor.G, dpadColor.B, 128), 0, dpadOrigin, 1.0f, SpriteEffects.None, 0);
                spriteBatch.Draw(jumpTexture, jumpPosition, null, new Color(jumpColor.R, jumpColor.G, jumpColor.B, 128), 0, jumpOrigin, 1.0f, SpriteEffects.None, 0); 
            }*/
            if (dpadPosition.X >= -100)
            {
                spriteBatch.Draw(dpadTexture, dpadPosition, null, new Color(DpadColor.R, DpadColor.G, DpadColor.B, 128f), 0, dpadOrigin, 1.1f, SpriteEffects.None, 0);
            }
            if (jumpPosition.X <= 870)
            {
                spriteBatch.Draw(jumpTexture, jumpPosition, null, new Color(JumpColor.R, JumpColor.G, JumpColor.B, 128f), 0, jumpOrigin, 1.0f, SpriteEffects.None, 0); 
            }
        }

        #endregion

        private bool ReadAccelerometerStatus()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader reader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\accelerometer.txt", FileMode.OpenOrCreate, myStore));
            string valueS = reader.ReadLine();
            reader.Close();
            int value = 1;
            try
            {
                value = int.Parse(valueS);

            }
            catch (Exception)
            {
                value = 1;
            }

            return value == 1 ? true : false;
        }
    }
}
