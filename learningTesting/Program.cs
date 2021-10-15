using System;
using System.Collections.Generic;

namespace Text_Game
{
    static class Program
    {
        static public Random randomBuild;
        static public Random randomAI;
        static private Player player;
        static private World world;
        static public int seed = 24241151;
        static public bool randomSeed = true;

        static public string GetRoomDescription(RoomType type)
        {
            switch (type)
            {
                case RoomType.Crossroad:
                    string[] desc = new string[4]
                    {
                                    "1",
                                    "2",
                                    "3",
                                    "4"
                    };
                    return desc[randomBuild.Next(0, desc.Length)];
                case RoomType.Hallway_Horizontal:
                    break;
                case RoomType.Hallway_Vertical:
                    break;
                case RoomType.StairCase:
                    return "Stairs";
                case RoomType.TotalRooms:
                    return "How did this happen?";
            }
            return "No Description Exist for this room";
        }
        static public ref Player Player => ref player;
        static public World World => world;
        static public int Clamp(int value, int minValue, int maxValue)
        {
            if (value < minValue)
            {
                value = minValue;
            }
            else if (value > maxValue)
            {
                value = maxValue;
            }
            return value;
        }
        static void Main(string[] args)
        {
            // TODO:
            // Title
            // Rooms
            // Connected Rooms
            // Story / Dialogue
            // Theme
            // Input
            // Obstacles
            if (randomSeed)
                seed = new Random().Next(0, 10000);
            Program.randomBuild = new Random(seed);
            Program.randomAI = new Random(seed);
            Console.WriteLine("Hello, What's your name Adventurer?");
            Console.Write(">>");
            string _username = Console.ReadLine();
            Program.player = new Player(_username, new Vector3(1, -1, 1));
            while (true)
            {
                Console.WriteLine($"Hello {player.name}, Welcome."
                    + $"{Environment.NewLine}"
                    +"|Start game  [Y]|\n"
                    +$"|Name Change [C]|{player.name}\n"
                    +"|Change Seed [S]|"
                    + $"Seed:{seed}\n"
                    +"|Exit      [Esc]|"
                    );
                Console.Write(">>");
                ConsoleKeyInfo userInput;
                while (true)
                {
                    userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.Y)
                    {
                        if (player.Health <= 0)
                        {
                            player = new Player(player.name);
                        }
                        StartGame();
                        break;
                    }
                    else if (userInput.Key == ConsoleKey.Escape)
                    {
                        Console.WriteLine("\nBBye");
                        System.Threading.Thread.Sleep(1000);
                        return;
                    }
                    else if (userInput.Key == ConsoleKey.C)
                    {
                        Console.WriteLine("\nHello, What's your name Adventurer?");
                        Console.Write(">>");
                        player.name = Console.ReadLine();
                        break;
                    }
                    else if (userInput.Key == ConsoleKey.S)
                    {
                        Console.Clear();
                        while (true)
                        {
                            Console.Write("input new seed:");
                            try
                            {
                                seed = int.Parse(Console.ReadLine());
                                break;
                            }
                            catch { }
                            Console.Clear();
                            Console.WriteLine("'Number(s)' please.");
                        }
                        break;
                    }
                }
                Console.Clear();
            }
        }
        static public void StartGame()
        {
            world = new World(ref player);

            world.main();
        }
        static public void replace(ref String str, int index, char replace)
        {
            if (str == null)
            {
                return;
            }
            else if (index < 0 || index >= str.Length)
            {
                return;
            }
            str.Remove(index, 1).Insert(index, replace.ToString());
        }
    }
    public class Vector2
    {
        public int x;
        public int z;
        public Vector2(int _x, int _z)
        {
            x = _x;
            z = _z;
        }
    }
    public class Vector3
    {
        public int x;
        public int y;
        public int z;
        public Vector3(int _x,int _y, int _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }
    public enum CompassDirection
    {
        North,
        East,
        South,
        West
    }
    public enum RoomType
    {
        Crossroad,
        Hallway_Horizontal,
        Hallway_Vertical,

        TotalRooms,
        StairCase
    }
}
