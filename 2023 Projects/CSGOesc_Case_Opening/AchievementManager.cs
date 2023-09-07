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
        private static List<Achievement> achievementList = new List<Achievement>();

        public static Dictionary<string, Dictionary<string, Achievement>> Achievements { get { return achievements; } }
        public static List<Achievement> AchievementList { get { return achievementList; } }
        public static int NumAchievements { get; private set; }

        public static void Update()
        {
            foreach (Achievement achievement in Achievements["click"].Values)
            {
                if (achievement.Completed) { continue; }
                //System.Diagnostics.Debug.WriteLine(achievement.Name + achievement.Progress.ToString());
                achievement.Update(PointManager.TotalClicks);
            }

            foreach (Achievement achievement in Achievements["points"].Values)
            {
                if (achievement.Completed) { continue; }

                achievement.Update(PointManager.CurrentPoints);
            }

            foreach (Achievement achievement in Achievements["spins"].Values)
            {
                if (achievement.Completed) { continue; }

                achievement.Update(SlotUI.TotalSpins);
            }

            for (int i = 0; i < Achievements["item"].Values.Count; i++)
            {
                if (Achievements["item"].Values.ElementAt(i).Completed) { continue; }

                Achievements["item"].Values.ElementAt(i).Update();
            }
        }

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
                    achievements[line[line.Length - 1]].Add(line[0], new Achievement(line[0].ToString(), line[1].ToString(), Convert.ToBoolean(line[3]), line[4]));
                }
                else
                {
                    achievements[line[line.Length - 1]].Add(line[0], new Achievement(line[0].ToString(), line[1].ToString(), Convert.ToInt32(line[2]), Convert.ToBoolean(line[3]), line[4]));
                }

                achievementList.Add(achievements[line[line.Length - 1]][line[0]]);

                NumAchievements++;
            }
        }

        public static void SaveAchievements()
        {
            List<string> info = new List<string>();

            info.Add("spins,points,click,item");

            for (int i = 0; i < AchievementList.Count; i++)
            {
                info.Add(string.Format("{0},{1},{2},{3},{4}", AchievementList[i].Name, AchievementList[i].Description, AchievementList[i].NeededAmount, AchievementList[i].Completed, AchievementList[i].Type));
            }

            FileIO.WriteTo("Achievements", info);
        }
    }
}
