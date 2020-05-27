using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.Utils
{
    class TimeBar
    {
        #region Declarations

        private Texture2D texture;
        private Vector2 position;
        private int width;
        private int height;
        private Color color;
        private float percent = 1.0f;
        private float startingTime;
        private float timer;
        private float timeLeft = 0.0f;
        private float elapsed = 0.0f;
        private bool isActive = false;
        #endregion

        #region Properties

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /*public float StartingTime
        {
            get { return startingTime; }
            set
            {
                startingTime = value;
                SetTimer();
            }
        }*/

        #endregion

        #region Constructor

        public TimeBar(int width, Color color, Vector2 position, ContentManager content)
        {
            this.width = width;
            this.height = 10;
            this.position = position;
            this.color = color;
            this.percent = 1.0f;
            LoadContent(content);
        }

        #endregion

        #region LoadContent

        private void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"Textures\pixel");
        }

        #endregion

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            if (isActive)
            {
                if (timeLeft < 100)
                {
                    elapsed += (float) gameTime.ElapsedGameTime.TotalSeconds;
                    timeLeft = elapsed * timer;
                    percent = (100.0f - timeLeft) / 100;
                }
                else
                {
                    isActive = false;
                }
            }
            else
            {
                elapsed = 0.0f;
                timeLeft = 0.0f;
                percent = 1.0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2((float)1, 0);
            Rectangle barRect = new Rectangle((int)position.X, (int)position.Y,
                                              (int)(width * percent), height);
            if (isActive)
            {
                spriteBatch.Draw(texture, barRect, null, this.color, 0, origin, SpriteEffects.None, 0);
            }
        }

        #endregion

        #region Helper Methods

        public void Start(float seconds)
        {
            isActive = true;
            startingTime = seconds;
            SetTimer();
        }

        private void SetTimer()
        {
            timer = 100.0f / startingTime;
            elapsed = 0.0f;
            timeLeft = 0.0f;
            percent = 1.0f;
        }

        #endregion
    }
}
