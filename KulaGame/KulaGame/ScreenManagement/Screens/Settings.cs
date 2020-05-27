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
    class SettingsMenuScreen : MenuScreen
    {
        private string accelerometerStatus;
        private string soundStatus;
        private string vibrationStatus;
        ContentManager content;
        private MenuEntry accelerometer;
        private MenuEntry sound;
        private MenuEntry vibration;
        private MenuEntry _stopZune;
        private MenuEntry _backgroundMusic;
        private MenuEntry _deleteOnlineSettings;
        private MusicSettings _musicSettings;
        private MenuEntry _showIntro;
        private string _musicSettingsStatus;

        private SettingsEnum _backgroundMusicSettings;
        private string _backgroundMusicSettingsStatus;

        private Texture2D settingsTexture;
        private Vector2 position = Vector2.Zero;
        private Vector2 origin = Vector2.Zero;

        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public SettingsMenuScreen()
            : base("Settings")
        {
            offSet = -30;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            // Create our menu entries.
            if (ReadSoundStatus() == 0)
            {
                soundStatus = "Off";
            }
            else
            {
                soundStatus = "On";
            }
            if (ReadVibrationStatus() == 0)
            {
                vibrationStatus = "Off";
            }
            else
            {
                vibrationStatus = "On";
            }
            if (ReadAccelerometerStatus() == 0)
            {
                accelerometerStatus = "Off";
            }
            else
            {
                accelerometerStatus = "On";
            }

            sound = new MenuEntry("Sounds - " + soundStatus);
            sound.Selected += SoundHandler;
            MenuEntries.Add(sound);

            _backgroundMusicSettings = Common.ReadBackgroundMusicSettings();

            switch (_backgroundMusicSettings)
            {
                case SettingsEnum.On:
                    _backgroundMusicSettingsStatus = "On";
                    break;
                case SettingsEnum.Off:
                    _backgroundMusicSettingsStatus = "Off";
                    break;
            }

            _backgroundMusic = new MenuEntry("Background music - " + _backgroundMusicSettingsStatus);
            _backgroundMusic.Selected += BackgroundMusicHandler;
            MenuEntries.Add(_backgroundMusic);

            vibration = new MenuEntry("Vibration - " + vibrationStatus);
            vibration.Selected += VibrationHandler;
            MenuEntries.Add(vibration);

            accelerometer = new MenuEntry("Accelerometer - " + accelerometerStatus);
            accelerometer.Selected += AccelerHandler;
            MenuEntries.Add(accelerometer);

            //_showIntro = new MenuEntry("Show intro video");
            //_showIntro.Selected += ShowIntroHandler;
            //MenuEntries.Add(_showIntro);

            _musicSettings = Common.ReadMusicSettings();

            switch (_musicSettings)
            {
                case MusicSettings.StopZune:
                    _musicSettingsStatus = "Yes";
                    break;
                case MusicSettings.DontStopZune:
                    _musicSettingsStatus = "No";
                    break;
                case MusicSettings.NotSet:
                    _musicSettingsStatus = "Not set";
                    break;
            }

            _stopZune = new MenuEntry("Stop zune music during the game - " + vibrationStatus);
            _stopZune.Selected += StopZuneHandler;
            MenuEntries.Add(_stopZune);

            if (Common.OnlineCredentialsExist())
            {
                _deleteOnlineSettings = new MenuEntry("Reset game and online account");
                _deleteOnlineSettings.Selected += DeleteOnlineSettings;
                MenuEntries.Add(_deleteOnlineSettings);
            }


        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            settingsTexture = content.Load<Texture2D>(@"ScreenManager\settingsBoard");

            // base.LoadContent();
        }


        public int ReadSoundStatus()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader reader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\sound.txt", FileMode.OpenOrCreate, myStore));
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
            return value;
        }

        public void WriteSoundValue(int value)
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\sound.txt", FileMode.OpenOrCreate, myStore));
            writer.Write(value);
            writer.Close();
        }

        public int ReadVibrationStatus()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader reader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\vibration.txt", FileMode.OpenOrCreate, myStore));
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
            return value;
        }

        public void WriteVibrationValue(int value)
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\vibration.txt", FileMode.OpenOrCreate, myStore));
            writer.Write(value);
            writer.Close();
        }

        public int ReadAccelerometerStatus()
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader reader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\accelerometer.txt", FileMode.OpenOrCreate, myStore));
            string valueS = reader.ReadLine();
            reader.Close();
            int value = 0;
            try
            {
                value = int.Parse(valueS);

            }
            catch (Exception)
            {
                value = 0;
            }
            return value;
        }

        public void WriteAccelerometerValue(int value)
        {
            IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
            StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("KulaGame\\accelerometer.txt", FileMode.OpenOrCreate, myStore));
            writer.Write(value);
            writer.Close();
        }



        //private void ShowIntroHandler(object sender, PlayerIndexEventArgs e)
        //{
        //    try
        //    {
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

        /// <summary>
        /// Event handler for our Select Level button.
        /// </summary>
        private void SoundHandler(object sender, PlayerIndexEventArgs e)
        {
            if (ReadSoundStatus() == 1)
            {
                soundStatus = "Off";
                WriteSoundValue(0);
            }
            else
            {
                soundStatus = "On";
                WriteSoundValue(1);
            }
        }

        void AccelerHandler(object sender, PlayerIndexEventArgs e)
        {
            if (ReadAccelerometerStatus() == 0)
            {
                accelerometerStatus = "On";
                WriteAccelerometerValue(1);
            }
            else
            {
                accelerometerStatus = "Off";
                WriteAccelerometerValue(0);
            }
        }


        /// <summary>
        /// Event handler for our Select Level button.
        /// </summary>
        private void DeleteOnlineSettings(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(
    ScreenManager,
    true,
    e.PlayerIndex,
    new BackgroundScreen(), new MainMenuScreen(), new DeleteOnlineSettingsScreen());
        }
        /// <summary>
        /// Event handler for our Select Level button.
        /// </summary>
        private void BackgroundMusicHandler(object sender, PlayerIndexEventArgs e)
        {
            SettingsEnum currentSetting = Common.ReadBackgroundMusicSettings();

            switch (currentSetting)
            {
                case SettingsEnum.On:
                    _backgroundMusicSettingsStatus = "Off";
                    Common.WriteBackgroundMusicSettings(SettingsEnum.Off);
                    break;
                case SettingsEnum.Off:
                    _backgroundMusicSettingsStatus = "On";
                    Common.WriteBackgroundMusicSettings(SettingsEnum.On);
                    break;
            }
        }

        /// <summary>
        /// Event handler for our Select Level button.
        /// </summary>
        private void StopZuneHandler(object sender, PlayerIndexEventArgs e)
        {
            MusicSettings currentSetting = Common.ReadMusicSettings();

            switch (currentSetting)
            {
                case MusicSettings.StopZune:
                    _musicSettingsStatus = "No";
                    Common.WriteMusicSettings(MusicSettings.DontStopZune);
                    break;
                case MusicSettings.DontStopZune:
                    _musicSettingsStatus = "Not set";
                    Common.WriteMusicSettings(MusicSettings.NotSet);
                    break;
                case MusicSettings.NotSet:
                    _musicSettingsStatus = "Yes";
                    Common.WriteMusicSettings(MusicSettings.StopZune);
                    break;
            }
        }


        /// <summary>
        /// Event handler for our Select Level button.
        /// </summary>
        private void VibrationHandler(object sender, PlayerIndexEventArgs e)
        {
            if (ReadVibrationStatus() == 1)
            {
                vibrationStatus = "Off";
                WriteVibrationValue(0);
            }
            else
            {
                vibrationStatus = "On";
                WriteVibrationValue(1);
            }
        }

        /// <summary>
        /// When the user cancels the main menu, we exit the game.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen(), playerIndex);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            sound.Text = string.Format("Sound - " + soundStatus);
            vibration.Text = string.Format("Vibration - " + vibrationStatus);
            accelerometer.Text = string.Format("Accelerometer - " + accelerometerStatus);
            _stopZune.Text = string.Format("Stop zune music - " + _musicSettingsStatus);
            _backgroundMusic.Text = string.Format("Background music - " + _backgroundMusicSettingsStatus);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public void UpdateMenuBoardLocation()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            position = new Vector2(0f, 245);
            origin = new Vector2(settingsTexture.Width / 2, settingsTexture.Height / 2);
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

            //Vector2 position = new Vector2((fullscreen.Width / 2), 245);
            //Vector2 origin = new Vector2(settingsTexture.Width / 2, settingsTexture.Height / 2);
            Color color = Color.White * TransitionAlpha;
            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 2) + 1;
            Vector2 scale = new Vector2(0.9f + pulsate * 0.02f, 1.1f + pulsate * 0.02f);

            //float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            //position.Y += transitionOffset * 150;

            spriteBatch.Begin();
            spriteBatch.Draw(settingsTexture, position, null, color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
