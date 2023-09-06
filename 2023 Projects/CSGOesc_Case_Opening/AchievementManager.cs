using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    internal static class AchievementManager
    {
        private static Dictionary<string, Dictionary<string, Achievement>> achievements = new Dictionary<string, Dictionary<string, Achievement>>();

        public static Dictionary<string, Dictionary<string, Achievement>> Achievements { get { return achievements; } }
        public static int NumAchievements { get; private set; }

        public static void GenerateAchievements()
        {
            List<string> itemInfo = FileIO.ReadFrom("Achievements");

            String[] line = null;

            string[] type = itemInfo[0].Split(',');

            foreach (string item in type)
            {
                achievements.Add(item, new Dictionary<string, Achievement>());
            }

            for (int i = 1; i < itemInfo.Count; i++)
            {
                line = itemInfo[i].Split(',');

                if (int.Parse(line[2].ToString()) == 0)
                {
                    achievements[line[line.Length - 1]].Add(line[0], new Achievement(line[0].ToString(), line[1].ToString(), Convert.ToBoolean(line[3])));
                }
                else
                {
                    achievements[line[line.Length - 1]].Add(line[0], new Achievement(line[0].ToString(), line[1].ToString(), Convert.ToInt32(line[2]), Convert.ToBoolean(line[3])));
                }

                NumAchievements++;
            }
        }
    }
}
