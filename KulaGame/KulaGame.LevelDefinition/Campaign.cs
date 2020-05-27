using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelDefinition
{
    public class Campaign
    {
        public string Name { get; set; }
        public string Description{ get; set; }
        public int Id{ get; set; }
        public string Image { get; set; }
        public string BrickModel { get; set; }
        public string KulaModel { get; set; }
        public string SkySphereModel { get; set; }
        public string FruidModel{ get; set; }
        public string CoinModel{ get; set; }
        public string KeyModel { get; set; }
        public string ObstacleModel { get; set; }
        public string FlagModel { get; set; }
        public string TargetModel { get; set; }
        public string SkyboardModel { get; set; }
        public string RockModel { get; set; }
        public string SpoolModel { get; set; }
        public string TeleportModelRed { get; set; }
        public string TeleportModelBlue { get; set; }
        public string CrystalModel { get; set; }

        public List<Level> Levels{ get; set; }
    }
}
