using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KulaGame.Engine.Utils
{
    public class Score
    {
        public Score(string resultTemp)
        {
            string[] results = resultTemp.Split(new string[] { "|||" }, StringSplitOptions.None);

            this.Position = int.Parse(results[0]);
            this.UserName= results[1];
            this.Points = int.Parse(results[2]);
        }

        public string UserName { get; set; }
        public int Position { get; set; }
        public int Points { get; set; }
    }
}
