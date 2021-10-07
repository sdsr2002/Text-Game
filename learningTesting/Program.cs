﻿using System;
using System.Collections.Generic;

namespace learningTesting
{
    class Program
    {
        static public Random random;
        static private Player player;
        static private World world;
        static private int seed = 241515;

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
                    return desc[random.Next(0, desc.Length)];
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

            Program.random = new Random(seed);

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
            Console.ReadKey();
        }

        public static void StartGame()
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
            
            //char[] chars = str.ToCharArray();
            //chars[index] = replace;
            //string t = new String(chars);
            //// Console.WriteLine($"Replaced: {str} with {t} ");
            //return new String(chars);
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
        private int startRoomCount = 20;
        private List<FloorLevel> floorLevels;
        private int currentFloor = -1;
        public World(ref Player _player)
        {
            player = _player;
        }

        public void main()
        {
            Setup();
        }

        private void Setup()
        {
            floorLevels = new List<FloorLevel>();
            NewFloor();
            currentFloor = 0;
            player.pos.y = 0;
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
            rooms[_stairsPos.x, _stairsPos.z].ChangeType(RoomType.StairCase);
        }

        // Returns Map (Currently in Numbers)
        public string GetMap()
        {
            string returnValue = "";
            string[] str = new string[3];
            for (int z = 0; z < floorSize.x; z++)
            {
                str = new string[3];
                for (int x = 0; x < floorSize.z; x++)
                {
                    string[] tempStr = rooms[x, z].GetRoomMapView();

                    for (int i = 0; i < 3; i++)
                    {
                        str[i] += tempStr[i];
                    }

                }

                for (int i = 0; i < 3; i++)
                {
                    returnValue += str[i];
                    returnValue += "\n";
                }

            }
            return returnValue;
        }

        // Create Rooms for this floor
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

        // Connect Existing Rooms of floor
        private void ConnectRooms(Vector2 _floorSize)
        {
            for (int x = 0; x < _floorSize.x; x++)
            {
                for (int z = 0; z < _floorSize.z; z++)
                {
                    //
                    // North
                    //

                    if (z - 1 < 0)
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.North, null); // rooms[x, 0]
                    }
                    else
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.North, rooms[x, z - 1]);
                    }

                    //
                    // East
                    //

                    if (x + 1 == _floorSize.x)
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.East, null); // rooms[_floorSize.x - 1, z]
                    }
                    else
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.East, rooms[x + 1, z]);
                    }

                    //
                    // South
                    //

                    if (z + 1 == _floorSize.z)
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.South, null); // rooms[x, _floorSize.z - 1]
                    }
                    else
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.South, rooms[x, z + 1]);
                    }

                    //
                    // West
                    //

                    if (x - 1 < 0)
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.West, null); // rooms[0, z]
                    }
                    else
                    {
                        rooms[x, z].adjacentRooms.Add(CompassDirection.West, rooms[x - 1, z]);
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
        public Dictionary<CompassDirection, bool> door;

        public void ChangeType(RoomType _type)
        {
            type = _type;
            GetDoors();
        }

        public Room(RoomType _type, Vector2 _pos)
        {
            type = _type;
            pos = _pos;
            adjacentRooms = new Dictionary<CompassDirection, Room>();
            GetDoors();
            description = Program.GetRoomDescription(type);
        }

        private void GetDoors()
        {
            door = new Dictionary<CompassDirection, bool>();
            switch (type)
            {
                case RoomType.Crossroad:
                    door.Add(CompassDirection.North, true);
                    door.Add(CompassDirection.East, true);
                    door.Add(CompassDirection.South, true);
                    door.Add(CompassDirection.West, true);
                    break;
                case RoomType.Hallway_Horizontal:
                    door.Add(CompassDirection.North, false);
                    door.Add(CompassDirection.East, true);
                    door.Add(CompassDirection.South, false);
                    door.Add(CompassDirection.West, true);
                    break;
                case RoomType.Hallway_Vertical:
                    door.Add(CompassDirection.North, true);
                    door.Add(CompassDirection.East, false);
                    door.Add(CompassDirection.South, true);
                    door.Add(CompassDirection.West, false);
                    break;
                case RoomType.StairCase:
                    door.Add(CompassDirection.North, true);
                    door.Add(CompassDirection.East, true);
                    door.Add(CompassDirection.South, true);
                    door.Add(CompassDirection.West, true);
                    break;
                case RoomType.TotalRooms:
                    door.Add(CompassDirection.North, true);
                    door.Add(CompassDirection.East, true);
                    door.Add(CompassDirection.South, true);
                    door.Add(CompassDirection.West, true);
                    break;
            }
        }

        public string[] GetRoomMapView()
        {
            string[] returnValue = new string[3]
                    {
                        "NaN",
                        "NaN",
                        "NaN"
                    };
            switch (type)
            {
                case RoomType.Crossroad:
                    returnValue = new string[3]
                    {
                        "# #",
                        " 1 ",
                        "# #"
                    };
                    break;
                case RoomType.Hallway_Horizontal:
                    returnValue = new string[3]
                    {
                        "###",
                        " 2 ",
                        "###"
                    };
                    break;
                case RoomType.Hallway_Vertical:
                    returnValue = new string[3]
                    {
                        "# #",
                        "#3#",
                        "# #"
                    };
                    break;
                case RoomType.StairCase:
                    returnValue = new string[3]
                    {
                        "# #",
                        " @ ",
                        "# #"
                    };
                    break;
                case RoomType.TotalRooms:
                    break;
            }

            for(int i = 0; i < 4; i++)
            {
                switch ((CompassDirection)i)
                {
                    case CompassDirection.North:
                        if (adjacentRooms[CompassDirection.North] == null)
                        {
                            returnValue[0] = returnValue[0].Remove(1, 1).Insert(1, "#");
                            continue;
                        }
                        if (!adjacentRooms[CompassDirection.North].door[CompassDirection.South])
                        {
                            returnValue[0] = returnValue[0].Remove(1,1).Insert(1,"#");
                        }
                        break;
                    case CompassDirection.East:
                        if (adjacentRooms[CompassDirection.East] == null)
                        {
                            returnValue[1] = returnValue[1].Remove(2, 1).Insert(2, "#");
                            continue;
                        }
                        if (!adjacentRooms[CompassDirection.East].door[CompassDirection.West])
                        {
                            returnValue[1] = returnValue[1].Remove(2, 1).Insert(2, "#");
                            //Program.replace(ref returnValue[1], 0, '#');
                        }
                        break;
                    case CompassDirection.South:
                        if (adjacentRooms[CompassDirection.South] == null)
                        {
                            returnValue[2] = returnValue[2].Remove(1, 1).Insert(1, "#");
                            continue;
                        }
                        if (!adjacentRooms[CompassDirection.South].door[CompassDirection.North])
                        {
                            returnValue[2] = returnValue[2].Remove(1, 1).Insert(1, "#");
                            //Program.replace(ref returnValue[2], 1, '#');
                        }
                        break;
                    case CompassDirection.West:
                        if (adjacentRooms[CompassDirection.West] == null)
                        {
                            returnValue[1] = returnValue[1].Remove(0, 1).Insert(0, "#");
                            continue;
                        }
                        if (!adjacentRooms[CompassDirection.West].door[CompassDirection.East])
                        {
                            returnValue[1] = returnValue[1].Remove(0, 1).Insert(0, "#");
                            //Program.replace(ref returnValue[1], 2, '#');
                        }
                        break;
                }
            }

            return returnValue;
        }

        private string CreateRoomDescription()
        {

            return "None";
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