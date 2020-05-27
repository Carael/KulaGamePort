//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using KulaGame.Engine.Utils;
using KulaGame.ScreenManagement.ScreenManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace KulaGame.ScreenManagement.Screens
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class EndGameScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        private SpriteFont headerFont;
        private SpriteFont textFont;
        
        private Texture2D aboutTexture;
        private Texture2D pandaTexture;
        private Texture2D dymekTexture;
        private Texture2D mainmenuTexture;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public EndGameScreen()
        {
            EnabledGestures = GestureType.Tap;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            headerFont = content.Load<SpriteFont>("ScreenManager/Font/MenuHeader");
            textFont = content.Load<SpriteFont>("ScreenManager/Font/MenuDetail");

            aboutTexture = content.Load<Texture2D>(@"ScreenManager\settingsBoard");
            pandaTexture = content.Load<Texture2D>(@"Textures\panda");
            dymekTexture = content.Load<Texture2D>(@"Textures\dymek");
            mainmenuTexture = content.Load<Texture2D>(@"Textures\mainmenu");
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            // we cancel the current menu screen if the user presses the back button
            PlayerIndex player;
            if (input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out player))
            {
                OnCancel(player);
            }

            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (gesture.Position.X > 350 && gesture.Position.X < 650 && gesture.Position.Y > 290 &&
                        gesture.Position.Y < 450)
                    {
                        OnCancel(player);
                    }
                }
            }
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            if (Common.ReadMusicSettings() == MusicSettings.StopZune || Microsoft.Xna.Framework.Media.MediaPlayer.GameHasControl)
            {
                MediaPlayer.Stop();
            }
            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new MainMenuScreen(), playerIndex);
        }

        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {

        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            Vector2 origin = new Vector2(aboutTexture.Width / 2, aboutTexture.Height / 2);
            Color color = Color.White;
            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float) Math.Sin(time * 5) + 1;
            float textScale = 1.5f + pulsate * 0.09f;
            Color textColor = Color.Black;
            float scaling = 1.2f + pulsate * 0.09f;

            string txt = "That's unfortunately the end of game.";
            string txt1 = "Thank You for playing Kula Game!.";
            string txt2 = "You've helped the Kulas to solve all continents problems.";
            string txt3 = "But the game is not over yet! Check out the Kula Game website ";
            string txt4 = "www.kulagame.pl to get information about current achievement to ";
            string txt5 = "unlock and gain acces to full game highscores and stats!";
            string txt6 = "You can check also the Kula Game High Score Facebook App: ";
            string txt7 = "http://apps.facebook.com/kulagame/ ";
            string txt8 = "to check Your friends score and compete with them!";

            spriteBatch.Begin();
            spriteBatch.Draw(aboutTexture, new Vector2((fullscreen.Width / 2), 240), null, color, 0.0f, origin, new Vector2(1.1f, 1.15f),
                             SpriteEffects.None, 0.0f);
            spriteBatch.Draw(pandaTexture, new Vector2(90, 330), null, color, 0, Vector2.Zero, 1.1f, SpriteEffects.None,
                             0);
            spriteBatch.Draw(dymekTexture, new Vector2((fullscreen.Width / 2) - 10, 180), null, color, 0,
                             new Vector2(dymekTexture.Width / 2, dymekTexture.Height / 2), new Vector2(1.8f, 1.8f),
                             SpriteEffects.None, 0);
            spriteBatch.Draw(mainmenuTexture, new Vector2(500, 370), null, color, 0,
                             new Vector2(mainmenuTexture.Width / 2, mainmenuTexture.Height / 2), scaling, SpriteEffects.None, 0);
            spriteBatch.DrawString(headerFont, txt, new Vector2((fullscreen.Width / 2), 35), Color.Black, 0, new Vector2(headerFont.MeasureString(txt).X / 2, 0), 1.3f, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, txt1, new Vector2((fullscreen.Width / 2), 85), Color.Black, 0, new Vector2(textFont.MeasureString(txt1).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, txt2, new Vector2((fullscreen.Width / 2), 105), Color.Black, 0, new Vector2(textFont.MeasureString(txt2).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, txt3, new Vector2((fullscreen.Width / 2), 125), Color.Black, 0, new Vector2(textFont.MeasureString(txt3).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, txt4, new Vector2((fullscreen.Width / 2), 145), Color.Black, 0, new Vector2(textFont.MeasureString(txt4).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, txt5, new Vector2((fullscreen.Width / 2), 165), Color.Black, 0, new Vector2(textFont.MeasureString(txt5).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, txt6, new Vector2((fullscreen.Width / 2), 185), Color.Black, 0, new Vector2(textFont.MeasureString(txt6).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, txt7, new Vector2((fullscreen.Width / 2), 205), Color.Black, 0, new Vector2(textFont.MeasureString(txt7).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(textFont, txt8, new Vector2((fullscreen.Width / 2), 225), Color.Black, 0, new Vector2(textFont.MeasureString(txt8).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            
            spriteBatch.End();
        }


        #endregion
    }
}
