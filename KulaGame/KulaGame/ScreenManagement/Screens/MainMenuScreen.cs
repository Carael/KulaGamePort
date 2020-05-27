//-----------------------------------------------------------------------------
// MainMenuScreen.cs
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

namespace KulaGame.ScreenManagement.Screens
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        ContentManager content;
        private Texture2D menuTexture;
        private Vector2 position = Vector2.Zero;
        private Vector2 origin = Vector2.Zero;

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("")
        {
            offSet = 40;
            MenuEntry startResume = new MenuEntry("Start / Resume");
            startResume.Selected += StartResumePressed;
            MenuEntries.Add(startResume);

            MenuEntry campaignSelect = new MenuEntry("Select level");
            campaignSelect.Selected += SelectCampaignPressed;
            MenuEntries.Add(campaignSelect);

            MenuEntry howTo = new MenuEntry("Instructions");
            howTo.Selected += HowToPressed;
            MenuEntries.Add(howTo);

            MenuEntry settings = new MenuEntry("Settings");
            settings.Selected += SettingsPressed;
            MenuEntries.Add(settings);


            //MenuEntry highScores = new MenuEntry("High scores");
            //highScores.Selected += HighScoresPressed;
            //MenuEntries.Add(highScores);

            MenuEntry aboutScreen = new MenuEntry("About");
            aboutScreen.Selected += AboutScreenPressed;
            MenuEntries.Add(aboutScreen);

        }

        /// <summary>
        /// Event handler for our Select Level button.
        /// </summary>
        private void StartResumePressed(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();

            if (Common.OnlineCredentialsExist()==false)
            {
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                                   new BackgroundScreen());
            }
            else
            {



                // We use the loading screen to move to our level selection screen because the
                // level selection screen needs to load in a decent amount of level art. The Load
                // method will cause all current screens to exit, so to enable us to be able to
                // easily come back from the level select screen, we must also pass down the
                // background and main menu screens.


                //read the current campaign/level and go to the apropriate screen
                //if no save then go to the first screen of campaign select
                IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
                try
                {

                    using (StreamReader reader = new StreamReader(
                        new IsolatedStorageFileStream("KulaGame\\save.txt", FileMode.OpenOrCreate, myStore)))
                    {
                        int campaign = int.Parse(reader.ReadLine());
                        int level = int.Parse(reader.ReadLine());
                        reader.Close();

                        if (level == 1)
                        {
                            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new BackgroundScreen(),
                                               new MainMenuScreen(), new CampaignSelectScreen(campaign));
                        }
                        else
                        {
                            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new BackgroundScreen(),
                                               new MainMenuScreen(), new CampaignSelectScreen(campaign),
                                               new LevelSelectScreen(campaign, level));
                        }
                    }
                }
                catch (Exception)
                {
                    using (StreamWriter save = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\save.txt", FileMode.OpenOrCreate, myStore)))
                    {
                        save.WriteLine(1);
                        save.WriteLine(1);
                        save.Close();

                        LoadingScreen.Load(
                            ScreenManager,
                            true,
                            e.PlayerIndex,
                            new BackgroundScreen(), new MainMenuScreen(), new CampaignSelectScreen());
                    }

                }
            }
        }


        /// <summary>
        /// Event handler for our Select Level button.
        /// </summary>
        private void SelectCampaignPressed(object sender, PlayerIndexEventArgs e)
        {
            // We use the loading screen to move to our level selection screen because the
            // level selection screen needs to load in a decent amount of level art. The Load
            // method will cause all current screens to exit, so to enable us to be able to
            // easily come back from the level select screen, we must also pass down the
            // background and main menu screens.
            //ExitScreen();
            /*LoadingScreen.Load(
                ScreenManager,
                true,
                e.PlayerIndex,
                new BackgroundScreen(), new MainMenuScreen(), new CampaignSelectScreen());*/

            ExitScreen();

            //if (Common.OnlineCredentialsExist() == false)
            //{
            //    LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
            //                       new BackgroundScreen(), new OnlineAccountScreen());
            //}
            //else
            //{
                LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                                   new BackgroundScreen(), new MainMenuScreen(),new CampaignSelectScreen());    
            //}
        }

        /// <summary>
        /// Event handler for our How To Button
        /// </summary>
        private void HowToPressed(object sender, PlayerIndexEventArgs e)
        {
            // We use the loading screen to move to our level selection screen because the
            // level selection screen needs to load in a decent amount of level art. The Load
            // method will cause all current screens to exit, so to enable us to be able to
            // easily come back from the level select screen, we must also pass down the
            // background and main menu screens.
           /* ExitScreen();
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                new BackgroundScreen(), new MainMenuScreen(), new HowToPlayScreen());*/
            ScreenManager.AddScreen(new HowToPlayScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for our Settings Button
        /// </summary>
        private void SettingsPressed(object sender, PlayerIndexEventArgs e)
        {
            //We use the loading screen to move to our level selection screen because the
            //level selection screen needs to load in a decent amount of level art. The Load
            //method will cause all current screens to exit, so to enable us to be able to
            //easily come back from the level select screen, we must also pass down the
            //background and main menu screens.
            ExitScreen();
            ScreenManager.AddScreen(new SettingsMenuScreen(), e.PlayerIndex);
        }


        /// <summary>
        /// When the user cancels the main menu, we exit the game.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {

            ScreenManager.Game.Exit();
        }


        /// <summary>
        /// Event hangler for the About Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutScreenPressed(object sender, PlayerIndexEventArgs e)
        {
            ExitScreen();
            ScreenManager.AddScreen(new AboutScreen(), e.PlayerIndex);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            menuTexture = content.Load<Texture2D>(@"ScreenManager\menuBoard");
        }

        public void UpdateMenuBoardLocation()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            position = new Vector2(0f, 325);
            origin = new Vector2(menuTexture.Width / 2, menuTexture.Height / 2);
            position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;
        }

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
            float scale = 1 + pulsate * 0.02f;

            //float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            //position.Y += transitionOffset * 150;

            spriteBatch.Begin();
            spriteBatch.Draw(menuTexture, position, null, color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
