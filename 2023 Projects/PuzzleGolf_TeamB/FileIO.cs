using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleGolf_TeamB
{
    internal class FileIO
    {
        /// <summary>
        /// Intial payer position read in from File
        /// </summary>
        private Vector2 ballPos;

        /// <summary>
        /// Goal position read in to switch to win state
        /// </summary>
        private Point goalPos;

        /// <summary>
        /// Number of puts needed for par read in from file 
        /// </summary>
        // (Should be added as it's own row underneath the x,y)
        private int parNum;

        /// <summary>
        /// A list of the file names being used for LoadNext()
        /// </summary>
        private List<string> fileNames;

        /// <summary>
        /// Textures for the walls
        /// </summary>
        private Dictionary<String, Texture2D> textures;

        /// <summary>
        ///  Keeps track of the level which we are in currently
        /// </summary>
        private int fileIOPosition;

        public int FilePos
        {
            get { return fileIOPosition; }
        }

        /// <summary>
        ///  Public get for game manager
        /// </summary>
        public int ParNum
        {
            get { return parNum; }
        }

        /// <summary>
        /// Ball position for next level in file manager
        /// </summary>
        public Vector2 BallPos
        {
            get { return ballPos; }
        }

        /// <summary>
        /// Returns point row and column of goal pos in Walls
        /// </summary>
        public Point GoalPos
        {
            get { return goalPos; }
        }

        public FileIO() 
        {
            fileNames = new List<string>();
        }
        
        /// <summary>
        /// Loads an array of names into an internal list of names
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadAssets(string[] fileNames, Dictionary<String, Texture2D> textures)
        {
            foreach (string fileName in fileNames)
            {
                this.fileNames.Add(fileName);
            }

            this.textures = textures; 
        }

        /// <summary>
        /// Reloads current level
        /// </summary>
        /// <returns></returns>
        public Wall[,] ThisLevel()
        {
            return StreamReader(fileIOPosition);
        }

        /// <summary>
        /// Loads next level
        /// </summary>
        /// <returns></returns>
        public Wall[,] LoadNext()
        {
            Wall[,] walls = null;

            /*
            So far, what I am enviosioning for the file layout would be:

                25,25                                             // [width,height] for the board
                560,800                                           // Player x and y
                14,17                                             // Goal [row, column]
                5                                                 // Par for the level
                W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W // Actual level data
                W,W,W,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,W,W,W,W,T,T,T,T,W,W,W,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,W,W,W,W,T,T,T,T,W,W,W,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,W,W,W,W,T,T,T,T,W,W,W,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,W,W,W,W,T,T,T,T,W,W,W,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,W,W,W,W,T,T,T,T,W,W,W,T,T,T,T,W,W,W,W,W
                W,W,W,T,T,W,W,W,W,T,T,T,T,W,W,W,T,T,T,T,W,W,W,W,W
                W,T,T,T,T,W,W,W,W,T,T,T,T,W,W,W,T,T,T,T,W,W,W,W,W
                W,T,T,T,T,W,W,W,W,B,B,B,B,W,W,W,B,B,B,B,W,W,W,W,W
                W,T,T,T,T,W,W,W,W,T,T,T,T,T,T,T,T,T,T,T,W,W,W,W,W
                W,T,T,T,T,W,W,W,W,T,T,T,T,T,T,T,T,T,T,T,W,W,W,W,W
                W,T,T,T,T,W,W,W,W,T,T,T,T,T,G,T,T,T,T,T,W,W,W,W,W
                W,T,T,T,T,W,W,W,W,T,T,T,T,T,T,T,T,T,T,T,W,W,W,W,W
                W,T,T,T,T,W,W,W,W,T,T,T,T,T,T,T,T,T,T,T,W,W,W,W,W
                W,T,T,T,T,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W
                W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W
                W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W
                W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W
                W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W,W
            
            */

            // BallPos = player x and y
            // ParNum = Par for the level
            // GoalPos = goal pos

            // Will use and increment the fileIOPosition int for which file we are on
            // Something like this
            //     StreamReader ... (fileNames[fileIOPosition]) ****** Done
            // This should be similar to what I am thinking and then
            //     fileIOPosition++

            // Returns the wall 2D array that is read from the file

            fileIOPosition++;

            walls = StreamReader(fileIOPosition);

            return walls;
        }

        /// <summary>
        /// Loads level based on the index passed through
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Wall[,] LoadSpecificLevel(int index)
        {
            Wall[,] walls;

            fileIOPosition = index;

            walls = StreamReader(fileIOPosition);

            return walls; 
        }

        /// <summary>
        /// File reader that accepts an index for file to read
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Wall[,] StreamReader(int index)
        {
            Wall[,] walls = null;

            int height = 1080;
            int width = 1920;

            Random random = new Random();

            Color color = new Color(random.Next(50, 255), random.Next(50, 255), random.Next(50, 255), 255);

            /// Loading file, getting the dimensions of the board
            StreamReader reader = new StreamReader("../../../Content/" + fileNames[index] + ".csv");

            // Reading in the board dimensions
            string dimensions = reader.ReadLine();

            string[] boardDimensions = dimensions.Split(",");

            int boardX = int.Parse(boardDimensions[1]);
            int boardY = int.Parse(boardDimensions[0]);
            walls = new Wall[boardX, boardY];


            // Reading in the player position
            string playerPosition = reader.ReadLine();
            string[] playerPos = playerPosition.Split(",");
            int playerX = int.Parse(playerPos[0]);
            int playerY = int.Parse(playerPos[1]);

            ballPos = new Vector2(playerX, playerY);

            // Reading in the par for the level
            string par = reader.ReadLine();
            parNum = int.Parse(par);

            int overX = 0;
            int downY = 0;

            for (int y = 0; y < boardY; y++)
            {
                string[] letters = reader.ReadLine().Split(",");

                for (int x = 0; x < boardX; x++)
                {
                    if (letters[x] != "T" && letters[x] == "B")
                    {
                        walls[x, y] = new Wall(true, false, null, (width / 3 - 200) + overX, downY, height, textures["Breakable"], Color.White);
                    }

                    else if (letters[x] != "T" && letters[x] == "W")
                    {
                        walls[x, y] = new Wall(false, false, null, (width / 3 - 200) + overX, downY, height, textures["Wall"], color);
                    }

                    else if (letters[x] != "T" && letters[x] == "G")
                    {
                        goalPos = new Point(x, y);
                        walls[x, y] = new Wall(true, false, null, (width / 3 - 200) + overX, downY, height, textures["Goal"], Color.DarkGoldenrod);
                    }

                    else if (letters[x] != "T" && letters[x] == "▲")
                    {
                        walls[x, y] = new Wall(false, true, "up", (width / 3 - 200) + overX, downY, height, textures["UpDown"], Color.White);
                    }

                    else if (letters[x] != "T" && letters[x] == "►")
                    {
                        walls[x, y] = new Wall(false, true, "right", (width / 3 - 200) + overX, downY, height, textures["LeftRight"], Color.White);
                    }

                    else if (letters[x] != "T" && letters[x] == "▼")
                    {
                        walls[x, y] = new Wall(false, true, "down", (width / 3 - 200) + overX, downY, height, textures["UpDown"], Color.White);
                    }

                    else if (letters[x] != "T" && letters[x] == "◄")
                    {
                        walls[x, y] = new Wall(false, true, "left", (width / 3 - 200) + overX, downY, height, textures["LeftRight"], Color.White);
                    }

                    else if (letters[x] != "T" && letters[x] == "A")
                    {
                        walls[x, y] = new Tiles((width / 3 - 200) + overX, downY, height, textures["Water"], Color.White);
                    }

                    else if (letters[x] != "T" && letters[x] == "K")
                    {
                        
                    }

                    else if (letters[x] != "T" && letters[x] == "L")
                    {
                        
                    }

                    overX += height / 25;
                }
                downY += height / 25;
                overX = 0;
            }

            return walls;
        }
    }
}
