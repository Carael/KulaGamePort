using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.Utils
{
    class FpsCounter : DrawableGameComponent
    {
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;
        private Vector2 _position;

        public FpsCounter(Game game, Vector2 position)
            : base(game)
        {
            _spriteBatch = new SpriteBatch(game.GraphicsDevice);
            _spriteFont = game.Content.Load<SpriteFont>("Fonts/StandardFont");
            _position = position;
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_spriteFont, fps, new Vector2(1+ _position.X, 1+ _position.Y), Color.Black);
            _spriteBatch.DrawString(_spriteFont, fps, new Vector2(_position.X, _position.Y), Color.White);

            _spriteBatch.End();




            base.Draw(gameTime);
        }
    }
}
