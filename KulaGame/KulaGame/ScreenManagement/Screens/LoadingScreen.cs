//-----------------------------------------------------------------------------
// LoadingScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using KulaGame.ScreenManagement.ScreenManager;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.ScreenManagement.Screens
{
    class Dot
    {
        public Vector2 Position = Vector2.Zero;
        public Texture2D Texture;

        public Dot(Vector2 position, Texture2D texture)
        {
            Position = position;
            Texture = texture;
        }
    }
    /// <summary>
    /// The loading screen coordinates transitions between the menu system and the
    /// game itself. Normally one screen will transition off at the same time as
    /// the next screen is transitioning on, but for larger transitions that can
    /// take a longer time to load their data, we want the menu system to be entirely
    /// gone before we start loading the game. This is done as follows:
    /// 
    /// - Tell all the existing screens to transition off.
    /// - Activate a loading screen, which will transition on at the same time.
    /// - The loading screen watches the state of the previous screens.
    /// - When it sees they have finished transitioning off, it activates the real
    ///   next screen, which may take a long time to load its data. The loading
    ///   screen will be the only thing displayed while this load is taking place.
    /// </summary>
    class LoadingScreen : GameScreen
    {
        #region Fields

        bool loadingIsSlow;
        bool otherScreensAreGone;
        GameScreen[] screensToLoad;
        
        Texture2D backgroundTexture;
        ContentManager content;

        private Texture2D dotTexture;
        private List<Dot> dots = new List<Dot>();
        private double timer = 0.0;
        private SpriteFont font;
        private int offSet = 0;

        private Texture2D loadTexture;

        #endregion

        #region Initialization


        /// <summary>
        /// The constructor is private: loading screens should
        /// be activated via the static Load method instead.
        /// </summary>
        private LoadingScreen(ScreenManagement.ScreenManager.ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            // we don't serialize loading screens. if the user exits while the
            // game is at a loading screen, the game will resume at the screen
            // before the loading screen.
            IsSerializable = false;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);

            dots.Clear();
            timer = 0.0;
            offSet = 0;
        }


        /// <summary>
        /// Activates the loading screen.
        /// </summary>
        public static void Load(ScreenManagement.ScreenManager.ScreenManager screenManager, bool loadingIsSlow,
                                PlayerIndex? controllingPlayer,
                                params GameScreen[] screensToLoad)
        {
            // Tell all the current screens to transition off.
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // Create and activate the loading screen.
            LoadingScreen loadingScreen = new LoadingScreen(screenManager,
                                                            loadingIsSlow,
                                                            screensToLoad);

            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>("ScreenManager/kula_game");
            dotTexture = content.Load<Texture2D>(@"Images\Other\arbuz");
            loadTexture = content.Load<Texture2D>(@"Textures\loading");

            base.LoadContent();
        }

        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the loading screen.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // If all the previous screens have finished transitioning
            // off, it is time to actually perform the load.
            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen, ControllingPlayer);
                    }
                }

                // Once the load has finished, we use ResetElapsedTime to tell
                // the  game timing mechanism that we have just finished a very
                // long frame, and that it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }
        }


        /// <summary>
        /// Draws the loading screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // If we are the only active screen, that means all the previous screens
            // must have finished transitioning off. We check for this in the Draw
            // method, rather than in Update, because it isn't enough just for the
            // screens to be gone: in order for the transition to look good we must
            // have actually drawn a frame without them before we perform the load.
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            // The gameplay screen takes a while to load, so we display a loading
            // message while that is going on, but the menus load very quickly, and
            // it would look silly if we flashed this up for just a fraction of a
            // second while returning from the game to the menus. This parameter
            // tells us how long the loading is going to take, so we know whether
            // to bother drawing the message.
            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                const string message = "Loading";

                // Center the text in the viewport.
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = viewportSize / 2;
                Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
                Color color = Color.White * TransitionAlpha;

                timer += gameTime.ElapsedGameTime.TotalSeconds;
                
                if (timer > 0.15)
                {
                    dots.Add(new Dot(new Vector2(480 + (offSet * 90), viewport.Height / 2), dotTexture));
                    offSet++;
                    timer = 0.0;
                }
                if (offSet > 6)
                {
                    dots.Clear();
                    offSet = 0;
                    timer = 0.0;
                }

                // Draw the text.
                spriteBatch.Begin();
                spriteBatch.Draw(backgroundTexture, fullscreen, Color.White);
                spriteBatch.Draw(loadTexture, new Vector2(310, viewport.Height / 2), null, color, 0,
                                 new Vector2(loadTexture.Width / 2, loadTexture.Height / 2), 1.2f, SpriteEffects.None, 0);
                foreach (Dot s in dots)
                {
                    spriteBatch.Draw(s.Texture, s.Position, null, Color.White * TransitionAlpha, 0,
                                     new Vector2(s.Texture.Width / 2, s.Texture.Height / 2), 0.5f, SpriteEffects.None, 0);
                }
               // spriteBatch.DrawString(font, message, new Vector2(textPosition.X - 40, textPosition.Y), color, 0.0f, new Vector2(font.MeasureString(message).X / 2, font.MeasureString(message).Y / 2), 1.0f, SpriteEffects.None, 0);
                spriteBatch.End();
            }
        }


        #endregion
    }
}
