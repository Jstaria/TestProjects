using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOesc_Case_Opening
{
    internal static class FileIO
    {
        /// <summary>
        /// Initializes file path for writing
        /// </summary>
        /// <param name="name"></param>
        public static void CreateTxtFile(string name)
        {
            string stream = "../../../Content/Saved Files/" + name + ".txt";

            StreamWriter fileWriter = new StreamWriter(stream);

            fileWriter.Close();
        }

        public static void WriteTo(string name, List<string> data)
        {
            string stream = "../../../Content/Saved Files/" + name + ".txt";

            StreamWriter fileWriter = new StreamWriter(stream);

            foreach (string item in data)
            {
                fileWriter.WriteLine(item);
            }

            fileWriter.Close();
        }

        public static void WriteTo(string name, List<float> data)
        {
            string stream = "../../../Content/Saved Files/" + name + ".txt";

            StreamWriter fileWriter = new StreamWriter(stream);

            foreach (float item in data)
            {
                fileWriter.WriteLine(item);
            }

            fileWriter.Close();
        }

        public static List<String> ReadFrom(string name)
        {
            List<String> data = new List<String>();
            string stream = "../../../Content/Saved Files/" + name + ".txt";

            StreamReader fileReader = new StreamReader(stream);

            string line = null;

            while ((line = fileReader.ReadLine()) != null)
            {
                data.Add(line);
            }

            if (fileReader != null)
            {
                fileReader.Close();
            }

            return data;
        }

        public static List<float> NumReadFrom(string name)
        {
            List<float> data = new List<float>();
            string stream = "../../../Content/Saved Files/" + name + ".txt";

            StreamReader fileReader = new StreamReader(stream);

            string line = null;

            while ((line = fileReader.ReadLine()) != null)
            {
                data.Add(float.Parse(line));
            }

            if (fileReader != null)
            {
                fileReader.Close();
            }

            return data;
        }
    }
}
