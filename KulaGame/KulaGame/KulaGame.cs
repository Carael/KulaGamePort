using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Xml;
//using System.Xml.Linq;
using KulaGame.Engine.GameObjects.Camera;
using KulaGame.Engine.Utils;
using KulaGame.ScreenManagement.ScreenManager;
using KulaGame.ScreenManagement.Screens;
using LevelDefinition;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace KulaGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class KulaGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ScreenManager _screenManager;

        public KulaGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            //set supported landscapes
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            graphics.IsFullScreen = true;

            //set supported gestures
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Flick;


            IsolatedStorageFile myStore =
                IsolatedStorageFile.GetUserStoreForDomain();

            //Create a new folder and call it "ImageFolder"
            if (!myStore.DirectoryExists("KulaGame"))
            {
                myStore.CreateDirectory("KulaGame");

                if (!myStore.FileExists("KulaGame\\save.txt"))
                {
                    using (
                        StreamWriter save =
                            new StreamWriter(new IsolatedStorageFileStream("KulaGame\\save.txt", FileMode.OpenOrCreate,
                                                                           myStore)))
                    {
                        save.WriteLine(1);
                        save.WriteLine(1);
                        save.Close();
                    }
                }
            }

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //GameplayScreen gameplayScreen = new GameplayScreen(this,1,1);
            //Components.Add(gameplayScreen);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);


            // attempt to deserialize the screen manager from disk. if that fails, we add our default screens.
            if (!_screenManager.DeserializeState())
            {
                _screenManager.AddScreen(new SplashScreen(), null);
            }
            _screenManager.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
