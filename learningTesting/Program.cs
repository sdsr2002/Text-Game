using System;
using System.Collections.Generic;

namespace learningTesting
{
    class Program
    {
        static public Random random;
        static private Player player;
        static private World world;
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

            Program.random = new Random(1);

            Console.WriteLine("Hello, What's your name Adventurer?");
            Console.Write(">");
            string _username = Console.ReadLine();
            Program.player = new Player(_username, new Vector3(1,-1,1));
            Console.WriteLine($"Hello {_username}, do you want to go on an adventure?"
                + $"{Environment.NewLine}" 
                + "[Y]es or [N]o"
                );
            ConsoleKeyInfo userInput = Console.ReadKey();
            StartGame();
        }

        public static void StartGame()
        {
            world = new World(ref Program.player);

            world.main();
        }
    }

    public class Player
    {
        public string name;
        public Vector3 pos;
        public CompassDirection playerDirection;
        public Player(string _name, Vector3 _pos)
        {
            name = _name;
            pos = _pos;
        }
    }

    public class World
    {
        private Player player;
        private int startRoomCount = 3;
        private List<FloorLevel> floorLevels;
        private int currentFloor = -1;
        public World(ref Player _player)
        {
            player = _player;
        }

        public void main()
        {
            floorLevels = new List<FloorLevel>();
            NewFloor();
            currentFloor--;
            player.pos.y--;
        }

        public void NewFloor()
        {
            floorLevels.Add(new FloorLevel(new Vector2(player.pos.x, player.pos.z), new Vector2(startRoomCount, startRoomCount), floorLevels.Count + 1));
            currentFloor++;
            player.pos.y++;
            Console.Write(floorLevels[currentFloor].GetMap());
        }
    }

    public class FloorLevel
    {
        public int level;
        public Room[,] rooms;
        Vector2 floorSize;
        public FloorLevel(Vector2 _stairsPos, Vector2 _floorSize, int _level)
        {
            level = _level;
            floorSize = _floorSize;
            CreateRooms(_floorSize);
            rooms[_stairsPos.x, _stairsPos.z].type = RoomType.StairCase;
        }

        public string GetMap()
        {
            string returnValue = "";
            for (int x = 0; x < floorSize.x; x++)
            {
                for (int z = 0; z < floorSize.z; z++)
                {
                    returnValue += ((int)rooms[x, z].type).ToString();
                }
                returnValue += "\n";
            }
            return returnValue;
        }

        private void CreateRooms(Vector2 _floorSize)
        {
            rooms = new Room[_floorSize.x, _floorSize.z];
            for (int x = 0; x < _floorSize.x; x++)
            {
                for (int z = 0; z < _floorSize.z; z++)
                {
                    rooms[x, z] = new Room((RoomType)Program.random.Next(0, (int)RoomType.TotalRooms), new Vector2(x,z));
                    // Console.WriteLine(rooms[x,z]);
                }
            }
            ConnectRooms(_floorSize);
            return;
        }

        private void ConnectRooms(Vector2 _floorSize)
        {
            for (int x = 0; x < _floorSize.x; x++)
            {
                for (int z = 0; z < _floorSize.z; z++)
                {
                    //
                    // North
                    //

                    if (z + 1 == _floorSize.z)
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.North, rooms[x, 0]);
                    }
                    else
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.North, rooms[x, z + 1]);
                    }

                    //
                    // East
                    //

                    if (x + 1 == _floorSize.x)
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.East, rooms[_floorSize.x - 1, z]);
                    }
                    else
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.East, rooms[x + 1, z]);
                    }

                    //
                    // South
                    //

                    if (z - 1 < 0)
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.South, rooms[x, _floorSize.z - 1]);
                    }
                    else
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.South, rooms[x, z - 1]);
                    }

                    //
                    // West
                    //

                    if (x + 1 == _floorSize.x)
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.West, rooms[0, z]);
                    }
                    else
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.West, rooms[x + 1, z]);
                    }

                    //
                    //
                    //
                }
            }
            return;
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

    public class Room
    {
        public RoomType type;
        public Vector2 pos;
        public string description;
        public Dictionary<CompassDirection, Room> adjacentRooms;

        public Room(RoomType _type, Vector2 _pos)
        {
            type = _type;
            pos = _pos;
            adjacentRooms = new Dictionary<CompassDirection, Room>();
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
