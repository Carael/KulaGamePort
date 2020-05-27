//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using KulaGame.Engine.Utils;
using KulaGame.ScreenManagement.ScreenManager;
//using Microsoft.Phone.Tasks;
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
    class AboutScreen : MenuScreen
    {
        #region Fields

        ContentManager content;
        private SpriteFont headerFont;
        private SpriteFont textFont;
        private Texture2D aboutTexture;
        private string text;
        private Vector2 position = Vector2.Zero;
        private Vector2 origin = Vector2.Zero;

        private Texture2D movieTexture;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public AboutScreen() 
            : base("About")
        {
            TouchPanel.EnabledGestures = GestureType.Tap;
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        private Vector2 StringLenght
        {
            get { return textFont.MeasureString(text); }
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
            movieTexture = content.Load<Texture2D>(@"Textures\movie");
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

            //foreach (GestureSample gesture in input.Gestures)
            //{
            //    if (gesture.GestureType == GestureType.Tap)
            //    {
            //        if (gesture.Position.X > 510 && gesture.Position.X < 750 && gesture.Position.Y > 357 &&
            //            gesture.Position.Y < 463)
            //        {
            //            PlayMovie();
            //        }
            //    }
            //}
        }
        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen(), playerIndex);
        }

        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public void UpdateMenuBoardLocation()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            position = new Vector2(0f, 240);
            origin = new Vector2(aboutTexture.Width / 2, aboutTexture.Height / 2);
            position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2;

            if (ScreenState == ScreenState.TransitionOn)
                position.X -= transitionOffset * 256;
            else
                position.X += transitionOffset * 512;
        }
        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            UpdateMenuBoardLocation();
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            Color color = Color.White * TransitionAlpha;
            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 2) + 1;
            float scale = 1.2f + pulsate * 0.02f;
            Color textColor = Color.Black * TransitionAlpha;
            float offSetX = 55.0f;
            float scaling = 1.2f + pulsate * 0.09f;

            
            spriteBatch.Begin();
            spriteBatch.Draw(aboutTexture, position, null, color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(movieTexture, new Vector2(630, 410), null, color, 0, new Vector2(movieTexture.Width / 2, movieTexture.Height / 2), scaling, SpriteEffects.None, 0);
            spriteBatch.DrawString(headerFont, text = "Kula Game by Carael's Team", new Vector2(fullscreen.Width / 2 - StringLenght.X / 2, 50), textColor);
            spriteBatch.DrawString(textFont, text = "version 1.0", new Vector2(fullscreen.Width / 2 - StringLenght.X / 2, 80), textColor);
            spriteBatch.DrawString(textFont, "Thanks for playing our game. We hope You enjoy it.",
                       new Vector2(offSetX, 100), textColor);
            spriteBatch.DrawString(textFont, "If You have any problems with the game or cool ideas write at our e-mail address: support@carael.net.", new Vector2(offSetX, 130), textColor);

            spriteBatch.DrawString(textFont, "Check out the game website: http://www.kulagame.pl for news, high scores and active achievements.", new Vector2(offSetX, 160), textColor);
            spriteBatch.DrawString(textFont, "Credits:", new Vector2(offSetX, 190), textColor);
            spriteBatch.DrawString(textFont, "Mariusz Matysek - programmer", new Vector2(offSetX, 220), textColor);
            spriteBatch.DrawString(textFont, "Micha³ Markiewicz - programmer", new Vector2(offSetX, 250), textColor);
            spriteBatch.DrawString(textFont, "Marcin Lipiñski - programmer", new Vector2(offSetX, 280), textColor);
            spriteBatch.DrawString(textFont, "Katarzyna Markiewicz - 2D&3D artist", new Vector2(offSetX, 310), textColor);
            spriteBatch.DrawString(textFont, "Bart³omiej Grech - 3D artist", new Vector2(offSetX, 340), textColor);
            spriteBatch.DrawString(textFont, "In order to go back to the menu press the 'back' button.", new Vector2(offSetX, 430), textColor);


            spriteBatch.End();
        }


        #endregion

        #region Helper Methods

        //private void PlayMovie()
        //{
        //    try
        //    {
        //        Common.SaveIntroPlayed();
        //        MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();
        //        mediaPlayerLauncher.Media = new Uri("Content/video.wmv", UriKind.Relative);
        //        mediaPlayerLauncher.Location = MediaLocationType.Install;
        //        mediaPlayerLauncher.Controls = MediaPlaybackControls.Skip | MediaPlaybackControls.Stop;
        //        mediaPlayerLauncher.Show();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        #endregion
    }
}
