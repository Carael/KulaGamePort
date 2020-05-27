//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.IO;
using System.IO.IsolatedStorage;
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
    class DeleteOnlineSettingsScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        private SpriteFont headerFont;
        private SpriteFont statusFont;
        private SpriteFont textFont;
        private Texture2D aboutTexture;

        private Texture2D kulkaTexture;
        private Texture2D dymekTexture;
        private Texture2D yesTexture;
        private Texture2D noTexture;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public DeleteOnlineSettingsScreen()
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
            statusFont = content.Load<SpriteFont>("Fonts/StatusFont");
            aboutTexture = content.Load<Texture2D>(@"ScreenManager\settingsBoard");

            kulkaTexture = content.Load<Texture2D>(@"Textures\kulka");
            dymekTexture = content.Load<Texture2D>(@"Textures\dymek");

            yesTexture = content.Load<Texture2D>(@"Textures\yes");
            noTexture = content.Load<Texture2D>(@"Textures\no");
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

                    //yes
                    if (gesture.Position.X > 275 && gesture.Position.X < 445 && gesture.Position.Y > 290 &&
                            gesture.Position.Y < 390)
                    {
                        Common.RemoveOnlineCredentials();
                        try
                        {
                            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
                            StreamWriter save = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\save.txt", FileMode.OpenOrCreate, myStore));
                            save.WriteLine(1);
                            save.WriteLine(1);
                            save.Close();

                            for (int i = 1; i <= 7; i++)
                            {
                                for (int j = 1; j <= 5; j++)
                                {
                                    if (myStore.FileExists(string.Format("KulaGame\\{0}_{1}.txt", i, j)))
                                    {
                                        myStore.DeleteFile(string.Format("KulaGame\\{0}_{1}.txt", i, j));
                                    }
                                }
                            }

                            if (myStore.FileExists(string.Format("KulaGame\\intro.txt")))
                            {
                                myStore.DeleteFile(string.Format("KulaGame\\intro.txt"));
                            }
                        }
                        catch (Exception)
                        {
                        }
                        


                            ScreenManager.RemoveScreen(this);
                        LoadingScreen.Load(
                            ScreenManager,
                            true,
                            player,
                            new BackgroundScreen(), new MainMenuScreen(), new SettingsMenuScreen());
                    }
                    //no - set the 
                    else if (gesture.Position.X > 530 && gesture.Position.X < 710 && gesture.Position.Y > 230 &&
                                 gesture.Position.Y < 390)
                    {
                        Common.WriteMusicSettings(MusicSettings.DontStopZune);

                        ScreenManager.RemoveScreen(this);

                        LoadingScreen.Load(
                            ScreenManager,
                            true,
                            player,
                            new BackgroundScreen(), new MainMenuScreen(), new SettingsMenuScreen());
                    }
                }
            }
        }
        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
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

            Vector2 texturePosition = new Vector2((fullscreen.Width / 2), 240);
            Vector2 origin = new Vector2(aboutTexture.Width / 2, aboutTexture.Height / 2);
            Color color = Color.White;
            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 5) + 1;
            float textScale = 1.5f + pulsate * 0.09f;
            Color textColor = Color.Black;
            float scaling = 1.2f + pulsate * 0.09f;

            string txt1 = "Reset game and online account ?";
            string txt2 = "Do You wan't to delete Your online settings?";
            string txt3 = "The existing online highscores will not be deleted.";
            string txt5 = "Local highscores will be deleted.";
            string txt4 = "You can always setup new account or login to existing one later.";
            

            spriteBatch.Begin();
            spriteBatch.Draw(aboutTexture, texturePosition, null, color, 0.0f, origin, new Vector2(1.1f, 1.0f), SpriteEffects.None, 0.0f);
            spriteBatch.Draw(kulkaTexture, new Vector2(90, 300), null, color, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            spriteBatch.Draw(dymekTexture, new Vector2((fullscreen.Width / 2) - 10, 180), null, color, 0,
                             new Vector2(dymekTexture.Width / 2, dymekTexture.Height / 2), new Vector2(1.8f, 1.3f), SpriteEffects.None, 0);
            spriteBatch.Draw(yesTexture, new Vector2(360, 340), null, color, 0, new Vector2(yesTexture.Width / 2, yesTexture.Height / 2), scaling, SpriteEffects.None, 0);
            spriteBatch.Draw(noTexture, new Vector2(620, 340), null, color, 0, new Vector2(noTexture.Width / 2, noTexture.Height / 2), scaling, SpriteEffects.None, 0);
            spriteBatch.DrawString(statusFont, txt1, new Vector2(fullscreen.Width / 2, 85), textColor, 0.0f, new Vector2(statusFont.MeasureString(txt1).X / 2, 0), 1.0f, SpriteEffects.None, 0);
           // spriteBatch.DrawString(statusFont, txt1, new Vector2(fullscreen.Width / 2, 35), textColor, 0.0f, new Vector2(statusFont.MeasureString(txt1).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(headerFont, txt2, new Vector2(fullscreen.Width / 2, 135), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt2).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(headerFont, txt3, new Vector2(fullscreen.Width / 2, 155), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt3).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(headerFont, txt5, new Vector2(fullscreen.Width / 2, 175), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt5).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            spriteBatch.DrawString(headerFont, txt4, new Vector2(fullscreen.Width / 2, 195), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt4).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            
            spriteBatch.End();
        }


        #endregion
    }
}
