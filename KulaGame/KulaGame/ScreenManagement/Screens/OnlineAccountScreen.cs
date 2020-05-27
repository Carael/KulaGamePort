////-----------------------------------------------------------------------------
//// BackgroundScreen.cs
////
//// Microsoft XNA Community Game Platform
//// Copyright (C) Microsoft Corporation. All rights reserved.
////-----------------------------------------------------------------------------

//using System;
//using System.IO;
//using System.Net;
//using System.Xml;
//using KulaGame.Engine.Utils;
//using KulaGame.ScreenManagement.ScreenManager;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
//using Microsoft.Xna.Framework.Media;

//namespace KulaGame.ScreenManagement.Screens
//{
//    /// <summary>
//    /// The background screen sits behind all the other menu screens.
//    /// It draws a background image that remains fixed in place regardless
//    /// of whatever transitions the screens on top of it may be doing.
//    /// </summary>
//    class OnlineAccountScreen : GameScreen
//    {
//        #region Fields

//        ContentManager content;
//        private SpriteFont headerFont;
//        private SpriteFont statusFont;
//        private SpriteFont textFont;
//        private Texture2D aboutTexture;
//        private string _username;
//        private string _password;
//        private bool _yes;
//        private string _response;
//        private bool _makeRequest = true;
//        private GoOnlineStates _screenState = GoOnlineStates.Before;

//        private Texture2D pandaTexture;
//        private Texture2D dymekTexture;
//        private Texture2D yesTexture;
//        private Texture2D noTexture;
//        private Texture2D okTexture;
//        #endregion

//        #region Initialization


//        /// <summary>
//        /// Constructor.
//        /// </summary>
//        public OnlineAccountScreen()
//        {
//            EnabledGestures = GestureType.Tap;
//            TransitionOnTime = TimeSpan.FromSeconds(0.5);
//            TransitionOffTime = TimeSpan.FromSeconds(0.5);
//        }


//        /// <summary>
//        /// Loads graphics content for this screen. The background texture is quite
//        /// big, so we use our own local ContentManager to load it. This allows us
//        /// to unload before going from the menus into the game itself, wheras if we
//        /// used the shared ContentManager provided by the Game class, the content
//        /// would remain loaded forever.
//        /// </summary>
//        public override void LoadContent()
//        {
//            if (content == null)
//                content = new ContentManager(ScreenManager.Game.Services, "Content");
//            headerFont = content.Load<SpriteFont>("ScreenManager/Font/MenuHeader");
//            textFont = content.Load<SpriteFont>("ScreenManager/Font/MenuDetail");
//            statusFont = content.Load<SpriteFont>("Fonts/StatusFont");
//            aboutTexture = content.Load<Texture2D>(@"ScreenManager\settingsBoard");

//            pandaTexture = content.Load<Texture2D>(@"Textures\panda");
//            dymekTexture = content.Load<Texture2D>(@"Textures\dymek");

//            yesTexture = content.Load<Texture2D>(@"Textures\yes");
//            noTexture = content.Load<Texture2D>(@"Textures\no");
//            okTexture = content.Load<Texture2D>(@"Textures\ok");
//        }


//        /// <summary>
//        /// Unloads graphics content for this screen.
//        /// </summary>
//        public override void UnloadContent()
//        {
//            content.Unload();
//        }

//        #endregion

//        #region Update and Draw

//        /// <summary>
//        /// Responds to user input, changing the selected entry and accepting
//        /// or cancelling the menu.
//        /// </summary>
//        public override void HandleInput(InputState input)
//        {            
//            // we cancel the current menu screen if the user presses the back button
//            PlayerIndex player;
//            if (input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out player))
//            {
//                OnCancel(player);
//            }

//            if (_screenState == GoOnlineStates.Before)
//            {
//                foreach (GestureSample gesture in input.Gestures)
//                {
//                    if (gesture.GestureType == GestureType.Tap)
//                    {

//                        //yes
//                        if (gesture.Position.X > 275 && gesture.Position.X < 445 && gesture.Position.Y > 290 &&
//                            gesture.Position.Y < 390)
//                        {
//                            _yes = true;
//                            _screenState = GoOnlineStates.Request;
//                        }
//                        //no - set the 
//                        else if (gesture.Position.X > 530 && gesture.Position.X < 710 && gesture.Position.Y > 230 &&
//                                 gesture.Position.Y < 390)
//                        {
//                            Common.WriteCredentials("", "", false);

//                            ScreenManager.RemoveScreen(this);
//                            LoadingScreen.Load(
//                            ScreenManager,
//                            true,
//                            player,
//                            new BackgroundScreen(), new MainMenuScreen(), new CampaignSelectScreen());
//                        }
//                    }
//                }
//            }
//            else if (_screenState == GoOnlineStates.Request)
//            {

//            }
//            else if (_screenState == GoOnlineStates.Registration)
//            {
//                foreach (GestureSample gesture in input.Gestures)
//                {
//                    if (gesture.GestureType == GestureType.Tap)
//                    {

//                        //yes
//                        if (gesture.Position.X > 275 && gesture.Position.X < 445 && gesture.Position.Y > 290 &&
//                            gesture.Position.Y < 390)
//                        {
//                            _yes = true;
//                            _makeRequest = true;
//                            _screenState = GoOnlineStates.Register;
//                        }
//                        //no - set the 
//                        else if (gesture.Position.X > 530 && gesture.Position.X < 710 && gesture.Position.Y > 230 &&
//                                 gesture.Position.Y < 390)
//                        {
//                            Common.WriteCredentials("", "", false);

//                            ScreenManager.RemoveScreen(this);
//                            LoadingScreen.Load(
//                            ScreenManager,
//                            true,
//                            player,
//                            new BackgroundScreen(), new MainMenuScreen(), new CampaignSelectScreen());
//                        }
//                    }
//                }
//            }
//            else if (_screenState == GoOnlineStates.Registered)
//            {
//                foreach (GestureSample gesture in input.Gestures)
//                {
//                    //write settings

//                    if (gesture.GestureType == GestureType.Tap)
//                    {
//                        if (gesture.Position.X > 310 && gesture.Position.X < 490 && gesture.Position.Y > 302 &&
//                            gesture.Position.Y < 378)
//                        {
//                            Common.WriteCredentials(_username, _password, true);

//                            ScreenManager.RemoveScreen(this);
//                            LoadingScreen.Load(
//                                ScreenManager,
//                                true,
//                                player,
//                                new BackgroundScreen(), new MainMenuScreen(), new CampaignSelectScreen());
//                        }
//                    } 
//                }
//            }
//            else if(_screenState == GoOnlineStates.ErrorRegister)
//            {
//                foreach (GestureSample gesture in input.Gestures)
//                {
//                    if (gesture.GestureType == GestureType.Tap)
//                    {

//                        //yes
//                        if (gesture.Position.X > 275 && gesture.Position.X < 445 && gesture.Position.Y > 290 &&
//                            gesture.Position.Y < 390)
//                        {
//                            _yes = true;
//                            _screenState = GoOnlineStates.Request;
//                            _username = string.Empty;
//                            _password = string.Empty;
//                            _makeRequest = true;
//                        }
//                        //no - set the 
//                        else if (gesture.Position.X > 530 && gesture.Position.X < 710 && gesture.Position.Y > 230 &&
//                                 gesture.Position.Y < 390)
//                        {
//                            Common.WriteCredentials("", "", false);

//                            ScreenManager.RemoveScreen(this);
//                            LoadingScreen.Load(
//                            ScreenManager,
//                            true,
//                            player,
//                            new BackgroundScreen(), new MainMenuScreen(), new CampaignSelectScreen());
//                        }
//                    }
//                }
//            }
//            else if (_screenState == GoOnlineStates.LogOn)
//            {
//                foreach (GestureSample gesture in input.Gestures)
//                {
//                    //write settings
//                    if (gesture.GestureType == GestureType.Tap)
//                    {
//                        if (gesture.Position.X > 310 && gesture.Position.X < 490 && gesture.Position.Y > 302 &&
//                            gesture.Position.Y < 378)
//                        {
//                            Common.WriteCredentials(_username, _password, true);

//                            ScreenManager.RemoveScreen(this);
//                            LoadingScreen.Load(
//                                ScreenManager,
//                                true,
//                                player,
//                                new BackgroundScreen(), new MainMenuScreen(), new CampaignSelectScreen());
//                        }
//                    }
//                }
//            }
//            //exist and timeout
//            else
//            {
//                foreach (GestureSample gesture in input.Gestures)
//                {
//                    if (gesture.GestureType == GestureType.Tap)
//                    {

//                        //yes
//                        if (gesture.Position.X > 275 && gesture.Position.X < 445 && gesture.Position.Y > 290 &&
//                            gesture.Position.Y < 390)
//                        {
//                            _yes = true;
//                            _screenState = GoOnlineStates.Request;
//                            _username = string.Empty;
//                            _password = string.Empty;
//                            _makeRequest = true;
//                        }
//                        //no - set the 
//                        else if (gesture.Position.X > 530 && gesture.Position.X < 710 && gesture.Position.Y > 230 &&
//                                 gesture.Position.Y < 390)
//                        {

//                            Common.WriteCredentials("", "", false);

//                            ScreenManager.RemoveScreen(this);
//                            LoadingScreen.Load(
//                            ScreenManager,
//                            true,
//                            player,
//                            new BackgroundScreen(), new MainMenuScreen(), new CampaignSelectScreen());
//                        }
//                    }
//                }
//            }
//        }
//        /// <summary>
//        /// Handler for when the user has cancelled the menu.
//        /// </summary>
//        protected virtual void OnCancel(PlayerIndex playerIndex)
//        {
//            ScreenManager.RemoveScreen(this);
//            ScreenManager.AddScreen(new MainMenuScreen(), playerIndex);
//        }

//        /// <summary>
//        /// Updates the background screen. Unlike most screens, this should not
//        /// transition off even if it has been covered by another screen: it is
//        /// supposed to be covered, after all! This overload forces the
//        /// coveredByOtherScreen parameter to false in order to stop the base
//        /// Update method wanting to transition off.
//        /// </summary>
//        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
//                                                       bool coveredByOtherScreen)
//        {
//            if (_yes)
//            {
//                if (string.IsNullOrEmpty(_username))
//                {
//                    if (!Guide.IsVisible)
//                        Guide.BeginShowKeyboardInput(PlayerIndex.One, "Enter Your user name:", "The user name must be unique. There are no limitations in character count or type.", _username, KeyboardResultUserName, null);
//                }
//                else if (string.IsNullOrEmpty(_password))
//                {
//                    if (!Guide.IsVisible)
//                        Guide.BeginShowKeyboardInput(PlayerIndex.One, "Enter Your password:", "The password will be stored on Your device. There are no limitations in character count or type. ", _password, KeyboardResulPassword, null,true);
     
//                }
//                else if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password) && _makeRequest)
//                {
//                    //tutaj sprawdziæ czy konto istnieje/mo¿na siê zalogowaæ itd - i wyœwietliæ odpowiedni komunikat
//                    _makeRequest = false;
//                    try
//                    {
//                        if (_screenState == GoOnlineStates.Register)
//                        {
//                            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://kulagame.pl.hostingasp.pl/Api/Register?username={0}&password={1}", _username, _password));
//                            httpWebRequest.Method = "POST";
//                            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

//                            httpWebRequest.BeginGetResponse(Response_Completed, httpWebRequest);
//                        }
//                        else
//                        {
//                            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://kulagame.pl.hostingasp.pl/Api/LogOn?username={0}&password={1}", _username, _password));
//                            httpWebRequest.Method = "POST";
//                            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

//                            httpWebRequest.BeginGetResponse(Response_Completed, httpWebRequest);
//                        }


//                    }
//                    catch
//                    {
//                        _response = "timeout";
//                    }

//                }
//            }
//        }


//        public void KeyboardResultUserName(IAsyncResult result)
//        {
//            _username = Guide.EndShowKeyboardInput(result);
//        }
//        public void KeyboardResulPassword(IAsyncResult result)
//        {
//            _password = Guide.EndShowKeyboardInput(result);
//        }


//        void Response_Completed(IAsyncResult result)
//        {
//            try
//            {
//                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
//                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);


//                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
//                {
//                    _response = streamReader.ReadToEnd();
//                }
//            }
//            catch (Exception)
//            {
//                _response = "timeout";
//            }

//            if (_response == "timeout")
//            {
//                _screenState = GoOnlineStates.TimeOut;
//            }
//            else if (_response == "logon")
//            {
//                _screenState = GoOnlineStates.LogOn;
//            }
//            else if (_response == "registration")
//            {
//                _screenState = GoOnlineStates.Registration;
//            }
//            else if (_response == "errorregister")
//            {
//                _screenState = GoOnlineStates.ErrorRegister;
//            }
//            else if (_response == "registered")
//            {
//                _screenState = GoOnlineStates.Registered;
//            }
//            else if (_response == "exist")
//            {
//                _screenState = GoOnlineStates.Exist;
//            }
//            else
//            {
//                //error
//                _screenState = GoOnlineStates.TimeOut;
//            }


//        }


//        /// <summary>
//        /// Draws the background screen.
//        /// </summary>
//        public override void Draw(GameTime gameTime)
//        {
//            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
//            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
//            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

//            Vector2 texturePosition = new Vector2((fullscreen.Width / 2), 240);
//            Vector2 origin = new Vector2(aboutTexture.Width / 2, aboutTexture.Height / 2);
//            Color color = Color.White;
//            // Pulsate the size of the selected menu entry.
//            double time = gameTime.TotalGameTime.TotalSeconds;
//            float pulsate = (float)Math.Sin(time * 5) + 1;
//            float textScale = 1.5f + pulsate * 0.09f;
//            Color textColor = Color.Black;
//            float scaling = 1.2f + pulsate * 0.09f;

//            string txt1 = "Go online with highscores?";
//            string txt2 = "If yes You will be prompted for the username and password";
//            string txt3 = "to register / login.";
//            string txt4 = "You can always change Your choice in the game 'settings'.";
//            string txt5 = "Loading...";
//            string txt6 = "Registered successfully. Tap to start the game.";
//            string txt7 = "Logged in successfully. Tap to start the game.";
//            string txt8 = "User already exist / incorrect password.";
//            string txt9 = "Connection error. Press 'Yes' to try again or 'No' to stay offline.";
//            string txt10 = "Do You want to register new account ?";
//            string txt11 = "Error occurred. Try again ?";
//            string txt12 = "Registering...";
//            string txt13 = "Press 'Yes' to try again or 'No' to stay offline.";

//            spriteBatch.Begin();
//            spriteBatch.Draw(aboutTexture, texturePosition, null, color, 0.0f, origin, new Vector2(1.2f, 1.0f), SpriteEffects.None, 0.0f);
//            spriteBatch.Draw(pandaTexture, new Vector2(90, 300), null, color, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
//            spriteBatch.Draw(dymekTexture, new Vector2(fullscreen.Width / 2, 180), null, color, 0,
//                             new Vector2(dymekTexture.Width / 2, dymekTexture.Height / 2), new Vector2(1.8f, 1.3f), SpriteEffects.None, 0);

//            spriteBatch.DrawString(statusFont, txt1, new Vector2(fullscreen.Width / 2, 85), textColor, 0.0f, new Vector2(statusFont.MeasureString(txt1).X / 2, 0), 1.0f, SpriteEffects.None, 0);

//            if (_screenState == GoOnlineStates.Before)
//            {
//                spriteBatch.DrawString(headerFont, txt2, new Vector2(fullscreen.Width / 2, 135), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt2).X / 2, 0), 1.0f, SpriteEffects.None, 0);
//                spriteBatch.DrawString(headerFont, txt3, new Vector2(fullscreen.Width / 2, 155), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt3).X / 2, 0), 1.0f, SpriteEffects.None, 0);
//                spriteBatch.DrawString(headerFont, txt4, new Vector2(fullscreen.Width / 2, 175), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt4).X / 2, 0), 1.0f, SpriteEffects.None, 0);

//                spriteBatch.Draw(yesTexture, new Vector2(360, 340), null, color, 0, new Vector2(yesTexture.Width / 2, yesTexture.Height / 2), scaling, SpriteEffects.None, 0);
//                spriteBatch.Draw(noTexture, new Vector2(620, 340), null, color, 0, new Vector2(noTexture.Width / 2, noTexture.Height / 2), scaling, SpriteEffects.None, 0);
//            }
//            else if (_screenState == GoOnlineStates.Request)
//            {
//                spriteBatch.DrawString(headerFont, txt5, new Vector2(fullscreen.Width / 2, 155), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt5).X / 2, 0), 1.0f, SpriteEffects.None, 0);
//            }
//            else if (_screenState == GoOnlineStates.Registered)
//            {
//                spriteBatch.DrawString(headerFont, txt6, new Vector2(fullscreen.Width / 2, 155), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt6).X / 2, 0), 1.0f, SpriteEffects.None, 0);
//                spriteBatch.Draw(okTexture, new Vector2(400, 340), null, color, 0, new Vector2(okTexture.Width / 2, okTexture.Height / 2), scaling, SpriteEffects.None, 0);
//            }
//            else if (_screenState == GoOnlineStates.Registration)
//            {
//                spriteBatch.DrawString(headerFont, txt10, new Vector2(fullscreen.Width / 2, 155), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt10).X / 2, 0), 1.0f, SpriteEffects.None, 0);
                
//                spriteBatch.Draw(yesTexture, new Vector2(360, 340), null, color, 0, new Vector2(yesTexture.Width / 2, yesTexture.Height / 2), scaling, SpriteEffects.None, 0);
//                spriteBatch.Draw(noTexture, new Vector2(620, 340), null, color, 0, new Vector2(noTexture.Width / 2, noTexture.Height / 2), scaling, SpriteEffects.None, 0);
//            }
//            else if (_screenState == GoOnlineStates.ErrorRegister)
//            {
//                spriteBatch.DrawString(headerFont, txt11, new Vector2(fullscreen.Width / 2, 155), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt11).X / 2, 0), 1.0f, SpriteEffects.None, 0);

//                spriteBatch.Draw(yesTexture, new Vector2(360, 340), null, color, 0, new Vector2(yesTexture.Width / 2, yesTexture.Height / 2), scaling, SpriteEffects.None, 0);
//                spriteBatch.Draw(noTexture, new Vector2(620, 340), null, color, 0, new Vector2(noTexture.Width / 2, noTexture.Height / 2), scaling, SpriteEffects.None, 0);
//            }
//            else if (_screenState == GoOnlineStates.Register)
//            {

//                spriteBatch.DrawString(headerFont, txt12, new Vector2(fullscreen.Width / 2, 155), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt12).X / 2, 0), 1.0f, SpriteEffects.None, 0);
//            }
//            else if (_screenState == GoOnlineStates.LogOn)
//            {
//                spriteBatch.DrawString(headerFont, txt7, new Vector2(fullscreen.Width / 2, 155), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt7).X / 2, 0), 1.0f, SpriteEffects.None, 0);
//                spriteBatch.Draw(okTexture, new Vector2(400, 340), null, color, 0, new Vector2(okTexture.Width / 2, okTexture.Height / 2), scaling, SpriteEffects.None, 0);
//            }
//            else if (_screenState == GoOnlineStates.Exist)
//            {
//                spriteBatch.DrawString(headerFont, txt8, new Vector2(fullscreen.Width / 2, 145), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt8).X / 2, 0), 1.0f, SpriteEffects.None, 0);
//                spriteBatch.DrawString(headerFont, txt13, new Vector2(fullscreen.Width / 2, 165), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt13).X / 2, 0), 1.0f, SpriteEffects.None, 0);
//                spriteBatch.Draw(yesTexture, new Vector2(360, 340), null, color, 0, new Vector2(yesTexture.Width / 2, yesTexture.Height / 2), scaling, SpriteEffects.None, 0);
//                spriteBatch.Draw(noTexture, new Vector2(620, 340), null, color, 0, new Vector2(noTexture.Width / 2, noTexture.Height / 2), scaling, SpriteEffects.None, 0);
               
//             }
//            //timeout
//            else
//            {
//                spriteBatch.DrawString(headerFont, txt9, new Vector2(fullscreen.Width / 2, 155), textColor, 0.0f, new Vector2(headerFont.MeasureString(txt9).X / 2, 0), 1.0f, SpriteEffects.None, 0);
//                spriteBatch.Draw(yesTexture, new Vector2(360, 340), null, color, 0, new Vector2(yesTexture.Width / 2, yesTexture.Height / 2), scaling, SpriteEffects.None, 0);
//                spriteBatch.Draw(noTexture, new Vector2(620, 340), null, color, 0, new Vector2(noTexture.Width / 2, noTexture.Height / 2), scaling, SpriteEffects.None, 0);
               
//            }

//            spriteBatch.End();


//        }


//        #endregion
//    }
//}
