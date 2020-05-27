//-----------------------------------------------------------------------------
// HighScoreScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using KulaGame.Engine.Utils;
using KulaGame.ScreenManagement.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.ScreenManagement.Screens
{
    /// <summary>
    /// LeaderboardScreen is a GameScreen that creates a single PageFlipControl containing
    /// a collection of LeaderboardPanel controls, which display a game's leaderboards.
    /// 
    /// You will need to customize the LoadContent() method of this class to create the
    /// appropriate list of leaderboards to match your game configuration.
    /// </summary>
    public class HighScoreScreen : SingleControlScreen
    {
        private int levelId;
        private int campaignId;
        private SpriteFont headerFont;
        private SpriteFont statusFont;
        private SpriteFont textFont;
        private Texture2D aboutTexture;

        private OnlineCredentials onlineCredentials;
        private string _status;
        private ContentManager _content;

        public HighScoreScreen(int campaignId,int levelId)
        {
            this.levelId = levelId;
            this.campaignId = campaignId;
        }

        public override void LoadContent()
        {
            EnabledGestures = ScrollTracker.GesturesNeeded;
            _content= ScreenManager.Game.Content;

            


            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            headerFont = _content.Load<SpriteFont>("ScreenManager/Font/MenuHeader");
            textFont = _content.Load<SpriteFont>("ScreenManager/Font/MenuDetail");
            statusFont = _content.Load<SpriteFont>("Fonts/StatusFont");
            aboutTexture = _content.Load<Texture2D>(@"ScreenManager\settingsBoard");

            onlineCredentials = Common.ReadOnlineCredentials();


            try
            {
                _status = "Loading...";
                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(string.Format("http://kulagame.pl.hostingasp.pl/Api/GetTop100?username={0}&campaignId={1}&levelId={2}", onlineCredentials.UserName, campaignId, levelId));
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";

                httpWebRequest.BeginGetResponse(Response_Completed, httpWebRequest);

            }
            catch
            {
                _status = "Connection error - check the internet connection...";
            }


            base.LoadContent();
        }

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
            float pulsate = (float)Math.Sin(time * 2) + 1;
            Vector2 scale = new Vector2(1.1f + pulsate * 0.02f, 1.4f + pulsate * 0.02f);

            spriteBatch.Begin();
            spriteBatch.Draw(aboutTexture, texturePosition, null, color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);

            if (!string.IsNullOrEmpty(_status))
            {
                spriteBatch.DrawString(headerFont, _status, new Vector2(fullscreen.Width / 2, 300), Color.Black, 0.0f, new Vector2(headerFont.MeasureString(_status).X / 2, 0), 1.0f, SpriteEffects.None, 0);
            }

            spriteBatch.End();
            
            base.Draw(gameTime);
        }


        void Response_Completed(IAsyncResult result)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                List<Score> results = new List<Score>();
                int positionBest = 0;
                int positionBestToday = 0;
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    string resultTemp = null;
                    resultTemp = streamReader.ReadLine();
                    try
                    {
                        string[] positions = resultTemp.Split(new string[] { "|||" }, StringSplitOptions.None);
                        positionBest = int.Parse(positions[0]);
                        positionBestToday= int.Parse(positions[1]);
                    }
                    catch{}
                    resultTemp = streamReader.ReadLine();
                    while (resultTemp != null)
                    {
                        try
                        {
                            results.Add(new Score(resultTemp));
                        }
                        catch {}
                        resultTemp = streamReader.ReadLine();
                    }
                }
                RootControl = new HighScorePanel(_content, results, positionBest, positionBestToday);
                _status = "";
            }
            catch (Exception)
            {
                _status = "Connection error - check the internet connection...";
            }
        }

    }
}
