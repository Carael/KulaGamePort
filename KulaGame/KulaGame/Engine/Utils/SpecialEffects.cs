using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace KulaGame.Engine.Utils
{
    public class SpecialEffects
    {
        #region Declarations

        public enum Effect
        {
            None,
            Puff,
            Boing,
            Doing,
            Click,
            Zzzzz,
            Dada
        }

        public Effect effect = Effect.None;
        private Vector2 Position = new Vector2(400, 240);
        private Texture2D puffTexture;
        private Texture2D boingTexture;
        private Texture2D doingTexture;
        private Texture2D clickTexture;
        private Texture2D zzzzTexture;
        private Texture2D dadaTexture;
        private float elapsed = 0.0f;
        private bool IsOn = false;
        private float timer = 0.0f;
        private Vector2 Scale = new Vector2(0.5f, 0.5f);

        #endregion

        #region Constructor

        public SpecialEffects(ContentManager content)
        {
            LoadContent(content);
        }

        #endregion

        #region ContentLoad

        private void LoadContent(ContentManager content)
        {
            puffTexture = content.Load<Texture2D>(@"Effects\puff");
            boingTexture = content.Load<Texture2D>(@"Effects\Boing");
            doingTexture = content.Load<Texture2D>(@"Effects\Doing");
            clickTexture = content.Load<Texture2D>(@"Effects\Click");
            zzzzTexture = content.Load<Texture2D>(@"Effects\Zzzz");
            dadaTexture = content.Load<Texture2D>(@"Effects\Dada");
        }

        #endregion

        #region Helper Methods

        public void StartEffect(Effect thisEffect, Vector2 thisPosition, float thisTimer, float thisScale)
        {
            effect = thisEffect;
            Position = thisPosition;
            timer = thisTimer;
            Scale = new Vector2(thisScale, thisScale);
            IsOn = true;
        }

        #endregion

        #region Update and Draw

        public void Update(GameTime gameTime)
        {
            if (IsOn)
            {
                elapsed += (float) gameTime.ElapsedGameTime.TotalSeconds;

                if (elapsed > timer)
                {
                    effect = Effect.None;
                    elapsed = 0.0f;
                    IsOn = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            float shaking = (float)(Math.Sin(elapsed * 18) / 4);
            float pulsate = (float)Math.Sin(elapsed * 9) + 1;
            Vector2 scale = new Vector2(Scale.X + pulsate * 0.06f, Scale.Y + pulsate * 0.06f);
            spriteBatch.Begin();

            switch (effect)
            {
                case Effect.None:
                    break;
                case Effect.Puff:
                    spriteBatch.Draw(puffTexture, Position, null, Color.White, shaking,
                                     new Vector2(puffTexture.Width / 2, puffTexture.Height / 2), scale, SpriteEffects.None, 0);
                    break;
                case Effect.Boing:
                    spriteBatch.Draw(boingTexture, Position, null, Color.White, 0,
                                     new Vector2(boingTexture.Width / 2, boingTexture.Height / 2), scale, SpriteEffects.None, 0);
                    break;
                case Effect.Doing:
                    spriteBatch.Draw(doingTexture, Position, null, Color.White, 0,
                                     new Vector2(doingTexture.Width / 2, doingTexture.Height / 2), scale, SpriteEffects.None, 0);
                    break;
                case Effect.Click:
                    spriteBatch.Draw(clickTexture, Position, null, Color.White, 0,
                                     new Vector2(clickTexture.Width / 2, clickTexture.Height / 2), scale, SpriteEffects.None, 0);
                    break;
                case Effect.Zzzzz:
                    spriteBatch.Draw(zzzzTexture, Position, null, Color.White, 0,
                                     new Vector2(zzzzTexture.Width / 2, zzzzTexture.Height / 2), scale, SpriteEffects.None, 0);
                    break;
                case Effect.Dada:
                    spriteBatch.Draw(dadaTexture, Position, null, Color.White, 0,
                                     new Vector2(dadaTexture.Width / 2, dadaTexture.Height / 2), scale, SpriteEffects.None, 0);
                    break;
            }

            spriteBatch.End();
        }

        #endregion
    }
}
