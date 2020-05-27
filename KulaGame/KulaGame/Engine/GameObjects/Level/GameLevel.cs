using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KulaGame.Engine.GameObjects.Models;
using KulaGame.Engine.GameObjects.Models.Lists;
using KulaGame.Engine.Utils;
using LevelDefinition;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace KulaGame.Engine.GameObjects.Level
{
    public class GameLevel
    {
        public BrickModelList Bricks;
        public ObstaclesModelList Obstacles;
        public CoinsModelList Coins;
        public KulaModel Kula;
        public FlagModel Flag;
        public SkyBox Sky;
        public KeyModel Key;
        public FruitModelList Fruits;
        public SkyboardModelList Skyboards;
        public CrystalModelList Crystals;
        public TeleportModelList Teleports;
        public RockModelList Rocks;
        public SpoolModelList Spools;
        public bool IsBonusTransparent = false;
        //in future implement a constructor that takes the xml file with complete level definition, now just load
        public GameLevel(ContentManager contentManager, Campaign campaign, LevelDefinition.Level level)
        {
            //Load single objects
            Flag = new FlagModel(contentManager.Load<Model>(campaign.FlagModel), level.Flag.Position, level.Flag.InitialRotation);
            Key = new KeyModel(contentManager.Load<Model>(campaign.KeyModel), level.Key.Position, level.Key.InitialRotation);
            Sky = new SkyBox(contentManager.Load<Model>(campaign.SkySphereModel), new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            //load model lists
            Bricks = new BrickModelList(contentManager.Load<Model>(campaign.BrickModel));
            Obstacles = new ObstaclesModelList(contentManager.Load<Model>(campaign.ObstacleModel));
            Fruits = new FruitModelList(contentManager.Load<Model>(campaign.FruidModel));
            Coins = new CoinsModelList(contentManager.Load<Model>(campaign.CoinModel));
            Skyboards = new SkyboardModelList(contentManager.Load<Model>(campaign.SkyboardModel));
            Crystals = new CrystalModelList(contentManager.Load<Model>(campaign.CrystalModel));
            Teleports = new TeleportModelList(contentManager.Load<Model>(campaign.TeleportModelRed), contentManager.Load<Model>(campaign.TeleportModelBlue));
            Rocks = new RockModelList(contentManager.Load<Model>(campaign.RockModel));
            Spools=new SpoolModelList(contentManager.Load<Model>(campaign.SpoolModel));
            foreach (var brick in level.Bricks)
            {
                Bricks.AddModel(brick.Position);    
            }
            Model rubberBrickModel = contentManager.Load<Model>(campaign.TargetModel);
            foreach (var rubberBrick in level.Targets)
            {
                Bricks.AddRubberBrickModel(new BrickModel(rubberBrickModel, rubberBrick.Position, new Vector3(0, 0, 0),
                                                           BrickType.Rubber));
            }
            foreach (var obstacle in level.Obstacles)
            {
                Obstacles.AddModel(obstacle.Position, obstacle.InitialRotation);
            }
            foreach (var coin in level.Coins)
            {
                Coins.AddModel(coin.Position, coin.InitialRotation);
            }

            float scaleFactor = 1;

            if (campaign.FruidModel.Contains("banan")==false)
            {
                scaleFactor = 2.5f;
            }
            if (campaign.FruidModel.Contains("dudle"))
            {
                scaleFactor = 4f;
            }
            if (campaign.FruidModel.Contains("icecube"))
            {
                scaleFactor = 1.8f;
                IsBonusTransparent = true;
            }
            if (campaign.FruidModel.Contains("kropla"))
            {
                IsBonusTransparent = true;
            }

            foreach (var fruit in level.Fruits)
            {
                Fruits.AddModel(fruit.Position, fruit.InitialRotation, scaleFactor);
            }


            foreach (var skyboard in level.Skyboards)
            {
                Skyboards.AddModel(skyboard.Position, skyboard.InitialRotation);
            }

            foreach (var crystal in level.Crystals)
            {
                Crystals.AddModel(crystal.Position, crystal.InitialRotation);
            }

            foreach (var teleport in level.Teleports)
            {
                Teleports.AddModel(teleport.PositionRed, teleport.InitialRotationRed, teleport.PositionBlue, teleport.InitialRotationBlue, TeleportModelType.Red);
                Teleports.AddModel(teleport.PositionBlue, teleport.InitialRotationBlue, teleport.PositionRed, teleport.InitialRotationRed, TeleportModelType.Blue);
            }
            foreach (var rock in level.Rocks)
            {
                Rocks.AddModel(rock.Position, rock.InitialRotation);
            }
            foreach (var spool in level.Spools)
            {
                Spools.AddModel(spool.Position, spool.InitialRotation);
            }
            //add kula at last because it need the brick bounding
            Kula = new KulaModel(contentManager.Load<Model>(campaign.KulaModel), level.Kula.Position, level.Kula.InitialRotation, Bricks.GetModels(), Skyboards.GetModels());
        }
    }
}
