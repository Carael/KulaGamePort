using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelDefinition
{
    public class Level
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public int TimeLimit { get; set; }
        public string Image { get; set; }



        public Kula Kula { get; set; }
        public Key Key{ get; set; } 
        public Flag Flag{ get; set; } 
        public List<Brick> Bricks { get; set; }
        public List<Target> Targets { get; set; } 
        public List<Coin> Coins { get; set; }
        public List<Fruit> Fruits{ get; set; }
        public List<Crystal> Crystals { get; set; }
        public List<Obstacle> Obstacles { get; set; }
        public List<Skyboard> Skyboards { get; set; }
        public List<Rock> Rocks { get; set; }
        public List<Spool> Spools { get; set; }
        public List<Teleport> Teleports { get; set; }
    }
}
