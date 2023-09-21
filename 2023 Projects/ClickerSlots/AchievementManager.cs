using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickerSlots
{
    /// <summary>
    /// Manages all achievements via a dictionary
    /// </summary>
    internal static class AchievementManager
    {
        private static Dictionary<string, Dictionary<string, Achievement>> achievements;
        private static List<Achievement> achievementList;

        public static Dictionary<string, Dictionary<string, Achievement>> Achievements { get { return achievements; } }
        public static List<Achievement> AchievementList { get { return achievementList; } }
        public static int NumAchievements { get; private set; }

        public static void Update()
        {
            // For every achievement marked as click based
            foreach (Achievement achievement in Achievements["click"].Values)
            {
                if (achievement.Completed) { continue; }
                //System.Diagnostics.Debug.WriteLine(achievement.Name + achievement.Progress.ToString());
                achievement.Update(PointManager.TotalClicks);
            }

            // For every achievement marked as points based
            foreach (Achievement achievement in Achievements["points"].Values)
            {
                if (achievement.Completed) { continue; }

                achievement.Update(PointManager.TotalPoints);
            }

            // For every achievement marked as spin based
            foreach (Achievement achievement in Achievements["spins"].Values)
            {
                if (achievement.Completed) { continue; }

                achievement.Update(SlotUI.TotalSpins);
            }

            // For every achievement marked as item based
            // A little more complicated since it is more than just numbers
            // Items can be sold, gotten, or generated from save, probably a better way of doing this
            List<bool> Completed = new List<bool>();

            for (int i = 0; i < Achievements["item"].Values.Count; i++)
            {
                if (Achievements["item"].Values.ElementAt(i).Completed)
                {
                    Completed.Add(Achievements["item"].Values.ElementAt(i).Completed);
                    continue;
                }

                if (i != 7 && SlotMachine.OneOfEach[i] != null)
                {
                    Achievements["item"].Values.ElementAt(i).Update();
                }
            }

            Achievements["item"].Values.ElementAt(7).Update(Completed.Count);
        }

        /// <summary>
        /// Generate achievements from save file
        /// </summary>
        public static void GenerateAchievements()
        {
            achievements = new Dictionary<string, Dictionary<string, Achievement>>();
            achievementList = new List<Achievement>();

            List<string> itemInfo = FileIO.ReadFrom("Achievements");

            String[] line = null;

            string[] type = itemInfo[0].Split(',');

            // Adds all basic categories of achievements, labeled at the top of the document
            foreach (string item in type)
            {
                achievements.Add(item, new Dictionary<string, Achievement>());
            }

            // Then goes into more detail as it sifts through the last element of each line to sort the achievement by type
            // I'm pretty sure this is safe to have achievements of type order while writing them to the document, 
            // But for my own sanity while writing them, I kept them to an orderly list
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

        // In hind-sight, I really wanted to use my systems so much that I didn't really think of a better way to do this
        /// <summary>
        /// Saves current achievement progress based on completed/not completed
        /// </summary>
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

        /// <summary>
        /// Resets all achievements completed progress to false
        /// </summary>
        public static void Clear()
        {
            List<string> info = FileIO.ReadFrom("Achievements");

            for (int i = 1; i < info.Count; i++)
            {
                string[] line = info[i].Split(',');
                line[3] = false.ToString();

                string fullLine = null;
                for (int j = 0; j < line.Length; j++)
                {
                    string str = "";

                    if (j != line.Length - 1)
                    {
                        str = ",";
                    }

                    fullLine += line[j] + str;
                }

                info[i] = fullLine;
            }

            FileIO.WriteTo("Achievements", info);

            achievementList = null;
            achievements = null;

            // Regenerates achievements and updates them to make sure
            GenerateAchievements();
            Update();
        }
    }
}
