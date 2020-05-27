//-----------------------------------------------------------------------------
// LevelSelectScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using KulaGame.ScreenManagement.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.ScreenManagement.Screens
{

    // This class demonstrates the PageFlipControl, by letting the player choose from a
    // set of game levels. Each level is shown with an 
    public class HowToPlayScreen : SingleControlScreen
    {
        // Descriptions of the different levels.
        LevelInfo[] LevelInfos = new LevelInfo[] {
            new LevelInfo
            {
                Name="",
                Description="",
                Image="ScreenManager/HowTo/howto_1"
            },
            new LevelInfo
            {
                Name="",
                Description="",
                Image="ScreenManager/HowTo/howto_2"
            },
            new LevelInfo
            {
                Name="",
                Description="",
                Image="ScreenManager/HowTo/howto_3"
            },            
            new LevelInfo
            {
                Name="",
                Description="",
                Image="ScreenManager/HowTo/howto_4"
            },
            new LevelInfo
            {
                Name="",
                Description="",
                Image="ScreenManager/HowTo/howto_5"
            },
            new LevelInfo
            {
                Name="",
                Description="",
                Image="ScreenManager/HowTo/howto_6"
            },
            new LevelInfo
            {
                Name="",
                Description="",
                Image="ScreenManager/HowTo/howto_7"
            },
        };

        public override void LoadContent()
        {
            EnabledGestures = PageFlipTracker.GesturesNeeded;
            ContentManager content = ScreenManager.Game.Content;

            RootControl = new PageFlipControl();

            foreach (LevelInfo info in LevelInfos)
            {
                RootControl.AddChild(new LevelDescriptionPanel(content, info));
            }
        }
    }
}
