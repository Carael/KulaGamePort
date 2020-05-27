//-----------------------------------------------------------------------------
// HighScorePanel.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using KulaGame.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.ScreenManagement.Controls
{
    /// <remarks>
    /// This class displays a list of high scores, to give an example of presenting
    /// a list of data that the player can scroll through.
    /// </remarks>
    public class HighScorePanel : ScrollingPanelControl
    {
        Control resultListControl = null;

        SpriteFont titleFont;
        SpriteFont headerFont;
        SpriteFont detailFont;

        public HighScorePanel(ContentManager content, List<Score> scores, int positionBest, int positionBestToday)
        {
            titleFont = content.Load<SpriteFont>("ScreenManager/Font/MenuTitle");
            headerFont = content.Load<SpriteFont>("ScreenManager/Font/MenuHeader");
            detailFont = content.Load<SpriteFont>("ScreenManager/Font/MenuDetail");

            
            AddChild(CreateHeaderControl(positionBest, positionBestToday));
            PopulateWithData(scores);
        }


        private void PopulateWithData(List<Score> scores)
        {
            PanelControl newList = new PanelControl();
            Random rng = new Random();
            foreach (var score in scores.OrderBy(p=>p.Position))
            {
                newList.AddChild(CreateLeaderboardEntryControl(score.Position, score.UserName, score.Points));
            }
                
            
            newList.LayoutColumn(100, 15, 0);

            if (resultListControl != null)
            {
                RemoveChild(resultListControl);
            }
            resultListControl = newList;
            AddChild(resultListControl);
            LayoutColumn(0, 30, 0);
        }

        protected Control CreateHeaderControl(int positionBest, int positionBestToday)
        {
            PanelControl panel = new PanelControl();

            string positionBestString = positionBest != 0 ? positionBest.ToString() : "not ranked";
            string positionBestTodayString = positionBestToday != 0 ? positionBestToday.ToString() : "not ranked";

            panel.AddChild(new TextControl("Online high scores", titleFont, Color.White, new Vector2(100, 0)));
            panel.AddChild(new TextControl(string.Format("Position in the rank: {0}", positionBestString), titleFont, Color.Red, new Vector2(100, 50)));
            panel.AddChild(new TextControl(string.Format("Best position today: {0}", positionBestTodayString), titleFont, Color.Blue, new Vector2(100, 100)));

            panel.AddChild(new TextControl("Position", headerFont, Color.Turquoise, new Vector2(100, 150)));
            panel.AddChild(new TextControl("Player", headerFont, Color.Turquoise, new Vector2(200, 150)));
            panel.AddChild(new TextControl("Score", headerFont, Color.Turquoise, new Vector2(350, 150)));

            return panel;
        }

        // Create a Control to display one entry in a leaderboard. The content is broken out into a parameter
        // list so that we can easily create a control with fake data when running under the emulator.
        //
        // Note that for time leaderboards, this function interprets the time as a count in seconds. The
        // value posted is simply a long, so your leaderboard might actually measure time in ticks, milliseconds,
        // or microfortnights. If that is the case, adjust this function to display appropriately.
        protected Control CreateLeaderboardEntryControl(int position, string userName, int points)
        {
            Color textColor = Color.Black;

            var panel = new PanelControl();

            // Player name
            panel.AddChild(
                new TextControl
                {
                    Text = string.Format("{0}.", position),
                    Font = detailFont,
                    Color = textColor,
                    Position = new Vector2(0, 0)
                });

            // Score
            panel.AddChild(
                new TextControl
                {
                    Text = userName,
                    Font = detailFont,
                    Color = textColor,
                    Position = new Vector2(100, 0)
                });

            // Time
            panel.AddChild(
                new TextControl
                    {
                        Text = points.ToString(),
                        Font = detailFont,
                        Color = textColor,
                        Position = new Vector2(250, 0)
                    });

            return panel;
        }
    }
}
