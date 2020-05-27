using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.Utils
{
    class Clock
    {
        #region Declarations

        private Texture2D ClockTexture;
        private Texture2D ShieldTexture;
        private Texture2D PointerTexture;

        private float startingTime = 0.0f;
        private float timeLeft = 0.0f;
        private float elapsed = 0.0f;
        private float timer;
        private List<float> points = new List<float>();

        private Color shieldColor = Color.White;

        private float endTimer = 0.0f;
        private bool warning = false;

        public SoundManager soundManager;

        public bool isLossSoundOn = false;

        #endregion

        public float StartingTime
        {
            get { return startingTime; }
            set
            {
                startingTime = value;
                SetTimer();
            }
        }

        private void SetTimer()
        {
            timer = 360.0f / StartingTime;
            timeLeft = 0.0f;
            elapsed = 0.0f;
            warning = false;
            points.Clear();
        }

        public Clock(Texture2D clockTexture, Texture2D shieldTextutre, Texture2D pointerTexture)
        {
            isLossSoundOn = true;
            ClockTexture = clockTexture;
            ShieldTexture = shieldTextutre;
            PointerTexture = pointerTexture;
        }

        public void Update(GameTime gameTime)
        {
            if (timeLeft < 360)
            {
                elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                timeLeft = elapsed * timer;

                if (startingTime - 6 <= elapsed)
                {
                    warning = true;
                    endTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;
                    if (endTimer >= 1)
                    {
                        soundManager.ZykniecieZegara();
                        endTimer = 0.0f;
                    }
                 }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 clockPosition, float clockSize)
        {
            float shieldSize = clockSize * 1.3f;
            if (!warning)
            {
                spriteBatch.Draw(ShieldTexture, clockPosition, null, Color.White, 0, new Vector2(ShieldTexture.Width / 2, ShieldTexture.Height / 2), shieldSize, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.Draw(ShieldTexture, clockPosition, null, Color.Red , 0, new Vector2(ShieldTexture.Width / 2, ShieldTexture.Height / 2), shieldSize, SpriteEffects.None, 0);
            }
            
            float pointerSize = clockSize * 2.0f;
            points.Add(MathHelper.ToRadians(180 + timeLeft));
            foreach (float f in points)
            {
                spriteBatch.Draw(PointerTexture, clockPosition, null, Color.Gold, f,
                             new Vector2(PointerTexture.Width / 2, 0), pointerSize, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(ClockTexture, clockPosition, null, Color.White, 0, new Vector2(ClockTexture.Width / 2, ClockTexture.Height / 2), clockSize, SpriteEffects.None, 0);
            pointerSize = clockSize * 1.80f;
            spriteBatch.Draw(PointerTexture, clockPosition, null, Color.Black, MathHelper.ToRadians(180 + timeLeft),
                             new Vector2(PointerTexture.Width / 2, 0), pointerSize, SpriteEffects.None, 0);
        }
    }
}
