namespace Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            const double Value = 4000;
            const string Letter = ("|");
            while (true) 
            {
                i++;
                i %= 50;

                switch (i)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 1:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(Letter);
                        }
                        break;
                    case 2:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Letter);
                        }
                        break;
                    case 3:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Letter);

                        }
                        break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.Red;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 5:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Letter);

                        }
                        break;
                    case 6:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(Letter);
                        }
                        break;
                    case 7:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(Letter);

                        }
                        break;
                    case 8:
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 9:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(Letter);

                        }
                        break;
                    case 10:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(Letter);
                        }
                        break;
                    case 11:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(Letter);

                        }
                        break;
                    case 12:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 13:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(Letter);

                        }
                        break;
                    case 14:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Letter);
                        }
                        break;
                    case 15:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Letter);

                        }
                        break;
                    case 16:
                        Console.ForegroundColor = ConsoleColor.Green;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 17:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Letter);

                        }
                        break;
                    case 18:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(Letter);
                        }
                        break;
                    case 19:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(Letter);

                        }
                        break;
                    case 20:
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 21:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(Letter);

                        }
                        break;
                    case 22:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(Letter);
                        }
                        break;
                    case 23:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(Letter);

                        }
                        break;
                    case 24:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 25:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(Letter);

                        }
                        break;
                    case 26:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(Letter);
                        }
                        break;
                    case 27:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(Letter);

                        }
                        break;
                    case 28:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 29:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(Letter);

                        }
                        break;
                    case 30:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(Letter);
                        }
                        break;
                    case 31:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(Letter);

                        }
                        break;
                    case 32:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 33:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(Letter);

                        }
                        break;
                    case 34:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(Letter);
                        }
                        break;
                    case 35:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(Letter);
                        }
                        break;
                    case 36:
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 37:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(Letter);
                        }
                        break;
                    case 38:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(Letter);
                        }
                        break;
                    case 40:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(Letter);
                        }
                        break;
                    case 41:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 42:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(Letter);
                        }
                        break;
                    case 43:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(Letter);
                        }
                        break;
                    case 44:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(Letter);
                        }
                        break;
                    case 45:
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        for (int j = 0; j < Value; j++)
                        {
                            Console.Write(Letter);
                        }
                        break;
                    case 46:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(Letter);
                        }
                        break;
                    case 47:

                        for (int j = 0; j < Value / 2; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(Letter);
                        }
                        break;
                    case 48:

                        for (int j = 0; j < Value / 3; j++)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write(Letter);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write(Letter);
                        }
                        break;

                }
                //////for (int j = 0; j < Value; j++)
                //////{
                //////    Console.Write("|");
                //////}
            }
        }
    }
}