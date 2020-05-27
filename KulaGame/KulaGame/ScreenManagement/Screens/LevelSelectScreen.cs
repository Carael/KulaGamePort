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
    // This class demonstrates the PageFlipControl, by letting the player choose from a
    // set of game levels. Each level is shown with an 
    public class LevelSelectScreen : SingleControlScreen
    {
        // Descriptions of the different levels.
        List<LevelInfo> _levelInfos = new List<LevelInfo>();
        private ContentManager _content;
        private int _campaignId;
        private int _selectedPage;

        public LevelSelectScreen(int campaignId)
        {
            _campaignId = campaignId;
            _selectedPage = 1;
        }

        public LevelSelectScreen(int campaignId, int selectedPage)
        {
            _campaignId = campaignId;
            _selectedPage = selectedPage;
        }

        public override void LoadContent()
        {
            EnabledGestures = PageFlipTracker.GesturesNeeded;
            _content = ScreenManager.Game.Content;

            RootControl = new PageFlipControl();

            //load campaign info

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

                    Campaign campaignTest = _content.Load<Campaign[]>("GameDefinition").SingleOrDefault(p => p.Id == _campaignId);
                    Level[] levelTest = campaignTest.Levels.OrderBy(p => p.Id).ToArray();
                }

            }
            catch (Exception)
            {
                saveCampaignId = 1;
                saveLevelId = 1;
                _campaignId = 1;
            }

            Campaign campaign = _content.Load<Campaign[]>("GameDefinition").SingleOrDefault(p => p.Id == _campaignId);
            Level[] levels = campaign.Levels.OrderBy(p => p.Id).ToArray();

            if (saveCampaignId == _campaignId)
            {

                //levels = campaign.Levels.OrderBy(p => p.Id).ToArray();

                levels = campaign.Levels.OrderBy(p => p.Id).Where(p => p.Id <= saveLevelId).ToArray();

            }

            foreach (var level in levels)
            {
                int points = 0;
                int stars = 0;

                IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();

                if (myStore.FileExists(string.Format("KulaGame\\{0}_{1}.txt", _campaignId, level.Id)))
                {
                    StreamReader streamReader = new StreamReader(new IsolatedStorageFileStream(string.Format("KulaGame\\{0}_{1}.txt",
                                                                 _campaignId, level.Id), FileMode.OpenOrCreate, myStore));

                    string resultString = streamReader.ReadLine();
                    streamReader.Close();
                    try
                    {
                        stars = int.Parse(resultString.Split('$')[0]);
                        points = int.Parse(resultString.Split('$')[1]);
                    }
                    catch
                    {
                    }
                }

                _levelInfos.Add(new LevelInfo { Description = level.Description, Name = level.Name, Image = level.Image, CampaignId = _campaignId ,LevelId = level.Id, Points = points, Stars = stars });
            }


            foreach (LevelInfo info in _levelInfos)
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
                int levelId = pageFlipControl.ObjectId;

                if (Microsoft.Xna.Framework.Media.MediaPlayer.GameHasControl == false && Common.ReadMusicSettings() == MusicSettings.NotSet)
                {
                    LoadingScreen.Load(ScreenManager, true, null, new BackgroundScreen(),
                                       new MainMenuScreen(), new MusicPromptScreen(_campaignId, levelId));
                }
                else
                {
                    LoadingScreen.Load(ScreenManager, true, null, new BackgroundScreen(), new MainMenuScreen(),
                                       new CampaignSelectScreen(_campaignId), new LevelSelectScreen(_campaignId, levelId), new GameplayScreen(_campaignId, levelId));
                }
            }
            else if (pageFlipControl.HighscoresWasTapped)
            {
                int levelId = pageFlipControl.ObjectId;
                LoadingScreen.Load(ScreenManager, true, null, new BackgroundScreen(), new MainMenuScreen(),
                                   new CampaignSelectScreen(_campaignId), new LevelSelectScreen(_campaignId, levelId), new HighScoreScreen(_campaignId, levelId));

            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

    }
}
