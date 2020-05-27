//-----------------------------------------------------------------------------
// CampaignSelectScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using KulaGame.Engine.Utils;
using KulaGame.ScreenManagement.Controls;
using LevelDefinition;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.ScreenManagement.Screens
{
    public class LevelInfo
    {
        public string Name;
        public int CampaignId;
        public int LevelId;
        public string Description;
        public string Image;
        public int Stars;
        public int Points;
        public int MaxPoints;
    }

    // This class demonstrates the PageFlipControl, by letting the player choose from a
    // set of game levels. Each level is shown with an 
    public class CampaignSelectScreen : SingleControlScreen
    {
        // Descriptions of the different levels.
        List<LevelInfo> _campaignInfos = new List<LevelInfo>();
        private ContentManager _content;
        private int _selectedPage;

        public CampaignSelectScreen()
        {
            _selectedPage = 1;
        }

        public CampaignSelectScreen(int selectedPage)
        {
            _selectedPage = selectedPage;
        }


        public override void LoadContent()
        {
            EnabledGestures = PageFlipTracker.GesturesNeeded;
            _content = ScreenManager.Game.Content;

            RootControl = new PageFlipControl();

            //load campaign info

            Campaign[] campaigns = _content.Load<Campaign[]>("GameDefinition");

            int saveCampaignId = 1;
            int saveLevelId = 1;
            try
            {
                IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
                using (StreamReader streamReader = new StreamReader(new IsolatedStorageFileStream("KulaGame\\save.txt", FileMode.OpenOrCreate, myStore)))
                {

                    saveCampaignId = int.Parse(streamReader.ReadLine());
                    saveLevelId = int.Parse(streamReader.ReadLine());
                    streamReader.Close();
                }

            }
            catch (Exception)
            {
                saveCampaignId = 1;
                saveLevelId = 1;
            }

            //foreach (var campaign in campaigns.OrderBy(p => p.Id))
            //{
            //    _campaignInfos.Add(new LevelInfo { Description = campaign.Description, Name = campaign.Name, Image = campaign.Image, CampaignId = campaign.Id, MaxPoints = 0, Points = 0, Stars = 0 });
            //}

            foreach (var campaign in campaigns.OrderBy(p => p.Id).Where(p => p.Id <= saveCampaignId))
            {
                _campaignInfos.Add(new LevelInfo { Description = campaign.Description, Name = campaign.Name, Image = campaign.Image, CampaignId = campaign.Id, MaxPoints = 0, Points = 0, Stars = 0 });
            }

            //if (!Common.IntroPlayed())
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



            foreach (LevelInfo info in _campaignInfos)
            {
                RootControl.AddChild(new LevelDescriptionPanel(_content, info));
            }

            ((PageFlipControl)RootControl).SetTrackerPage(_selectedPage - 1);
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            var pageFlipControl = (PageFlipControl)RootControl;


            if (pageFlipControl.WasTapped == true)
            {
                int campaignId = pageFlipControl.ObjectId;

                Campaign campaign = _content.Load<Campaign[]>("GameDefinition").SingleOrDefault(p => p.Id == campaignId);

                LoadingScreen.Load(ScreenManager, true, 0, new BackgroundScreen(), new MainMenuScreen(),
                                   new CampaignSelectScreen(campaignId), new LevelSelectScreen(campaignId));
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

    }

    public class LevelDescriptionPanel : PanelControl
    {
        const float MarginLeft = 20;
        const float MarginTop = 20;
        const float DescriptionTop = 440;

        public LevelDescriptionPanel(ContentManager content, LevelInfo info)
        {
            Texture2D starTexture = content.Load<Texture2D>("ScreenManager/star");

            Texture2D backgroundTexture = content.Load<Texture2D>(info.Image);
            ImageControl background = new ImageControl(backgroundTexture, Vector2.Zero);
            AddChild(background);

            Texture2D boardTexture = content.Load<Texture2D>(@"ScreenManager\tabliczka");
            if (info.CampaignId != 0 && info.LevelId != 0)
            {
                ImageControl board = new ImageControl(boardTexture, new Vector2(550, 440));
                board.OffSet = new Vector2(110, -2);
                board.SizeOf = new Vector2(0.9f, 0.9f);
                board.Special = true;
                AddChild(board);
            }
            SpriteFont andyFont = content.Load<SpriteFont>("ScreenManager/Font/Andy");
            SpriteFont titleFont = content.Load<SpriteFont>("ScreenManager/Font/MenuTitle");
            SpriteFont bigFont = content.Load<SpriteFont>("Fonts/bigAndy");
            if (info.CampaignId != 0 && info.LevelId != 0)
            {
                ImageControl board3 = new ImageControl(boardTexture, new Vector2(100, 40));
                board3.OffSet = new Vector2(-5, -5);
                board3.SizeOf = new Vector2(0.65f, 0.8f);
                board3.Special = true;
                AddChild(board3);
            }
            TextControl title = new TextControl(info.Name, andyFont, Color.Black, new Vector2(23, 16));
            AddChild(title);

           // if (info.CampaignId != 0)
            //{
                TextControl information = new TextControl("Swipe to change", andyFont, Color.White, new Vector2(400, 455));
                information.Special = true;
                AddChild(information);

                TextControl arrowLeft = new TextControl("<", bigFont, Color.White, new Vector2(50, 240));
                arrowLeft.Special = true;    
                arrowLeft.SizeOf = new Vector2(1.0f, 1.0f);
                arrowLeft.OffSet = new Vector2(-30, 0);
                AddChild(arrowLeft);

                TextControl arrowRight = new TextControl(">", bigFont, Color.White, new Vector2(750, 240));
                arrowRight.Special = true;    
                arrowRight.SizeOf = new Vector2(1.0f, 1.0f);
                arrowRight.OffSet = new Vector2(30, 0);
                AddChild(arrowRight);
            //}


            SpriteFont highscoreFont = content.Load<SpriteFont>("ScreenManager/Font/Highscore");
            if (info.Points != 0)
            {
                ImageControl board2 = new ImageControl(boardTexture, new Vector2(550, 40));
                board2.OffSet = new Vector2(115, 5);
                board2.SizeOf = new Vector2(1.0f, 1.3f);
                board2.Special = true;
                AddChild(board2);

                TextControl highscore = new TextControl(string.Format("Score: {0}", info.Points), andyFont, Color.Black, new Vector2(565, 10));
                AddChild(highscore);


                for (int i = 0; i < 5; i++)
                {
                    ImageControl star = new ImageControl(starTexture, new Vector2(575 + (i * 35), 45));

                    if (i > info.Stars - 1 || info.Stars == 0)
                    {
                        star.Color = new Color(0, 0, 0, 25);
                    }
                    else
                    {
                        star.Color = Color.YellowGreen;
                    }

                    AddChild(star);

                }

            }



            SpriteFont descriptionFont = content.Load<SpriteFont>("ScreenManager/Font/MenuDetail");
            if (info.CampaignId != 0 && info.LevelId != 0)
            {
                ButtonControl przycisk = new ButtonControl("Online Scores", andyFont, Color.Black, new Vector2(555, 420));
                AddChild(przycisk);
            }

            /*TextControl description = new TextControl(info.Description, andyFont, Color.Black, new Vector2(10, 10));
            AddChild(description);*/
        }
    }
}
