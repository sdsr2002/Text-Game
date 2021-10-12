using System;
using System.Collections.Generic;

namespace learningTesting
{
    static class Program
    {

        static public Random randomBuild;
        static public Random randomAI;
        static private Player player;
        static public World world;
        static public int seed = 24241151;

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
        static public Player GetPlayer()
        {
            return player;
        }
        static public World GetWorld()
        {
            return world;
        }
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

            Program.randomBuild = new Random(seed);
            Program.randomAI = new Random(seed);
            Console.WriteLine("Hello, What's your name Adventurer?");
            Console.Write(">>");
            string _username = Console.ReadLine();
            Program.player = new Player(_username, new Vector3(1, -1, 1));
            while (true)
            {
                Console.WriteLine($"Hello {player.name}, do you want to go on an adventure?"
                    + $"{Environment.NewLine}"
                    + "[Y]es or [N]o | Name Change [C]"
                    );
                Console.Write(">>");
                ConsoleKeyInfo userInput;
                while (true)
                {
                    userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.Y)
                    {
                        StartGame();
                        break;
                    }
                    else if (userInput.Key == ConsoleKey.N)
                    {
                        Console.WriteLine("\nBye");
                        Console.ReadKey();
                        return;
                    }
                    else if (userInput.Key == ConsoleKey.C)
                    {
                        Console.WriteLine("\nHello, What's your name Adventurer?");
                        Console.Write(">>");
                        player.name = Console.ReadLine();
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
            
            //char[] chars = str.ToCharArray();
            //chars[index] = replace;
            //string t = new String(chars);
            //// Console.WriteLine($"Replaced: {str} with {t} ");
            //return new String(chars);
        }
    }
    public class Player
    {
        // Combat
        private int _health = 10;
        private int _maxHealth = 10;
        private int _minDamage = 1;
        private int _maxDamage = 3;
        // Movement
        public string name;
        private Vector3 pos;
        private Vector2 pos2D;
        public CompassDirection playerDirection;
        //
        public bool ReceiveDmg(int damage)
        {
            // Receive Damage to Monster
            _health -= Program.Clamp(damage, 0, int.MaxValue);
            // Did not die return: False
            if (_health <= 0)
            {
                return true;
            }
            return false;
        }
        public Vector2 GetDamage()
        {
            return new Vector2(_minDamage, _maxDamage);
        }
        public Player(string _name, Vector3 _pos)
        {
            name = _name;
            pos = _pos;
            pos2D = new Vector2(_pos.x, _pos.z);
        }
        public Player(string _name)
        {
            Vector3 _pos = new Vector3(0, 0, 0);
            name = _name;
            pos = _pos;
            pos2D = new Vector2(_pos.x, _pos.z);
        }
        public Vector3 GetPositionV3()
        {
            return pos;
        }
        public Vector2 GetPositionV2()
        {
            return pos2D;
        }
        public void SetPosition(Vector3 _pos)
        {
            pos = _pos;
            pos2D = new Vector2(_pos.x, _pos.z);
            return;
        }
        public void SetPosition(Vector2 _pos)
        {
            SetPosition(new Vector3(_pos.x, pos.y, _pos.z));
            return;
        }
        public void AddPosition(Vector3 _pos)
        {
            pos.x += _pos.x;
            pos.y += _pos.y;
            pos.z += _pos.z;

            pos2D.x += _pos.x;
            pos2D.z += _pos.z;
            return;
        }
        public void AddPosition(Vector2 _pos)
        {
            this.AddPosition(new Vector3(_pos.x, 0, _pos.z));
            return;
        }

        internal int GetHealth()
        {
            return _health;
        }

        internal int GetMaxHealth()
        {
            return _maxHealth;
        }

        internal void Heal(int healAmount)
        {
            _health += Program.Clamp(healAmount,0 , int.MaxValue);
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }
            return;
        }
    }
    public class Monster
    {
        string name;
        int minDmg;
        int maxDmg;
        int maxHealth;
        int health;
        Vector3 pos;
        public Monster(string _name, int _minDmg, int _maxDmg, int _health, Vector3 _pos)
        {
            name = _name;
            minDmg = _minDmg;
            maxDmg = _maxDmg;
            maxHealth = _health;
            health = maxHealth;
            pos = _pos;
        }
        public Vector2 GetDamage()
        {
            return new Vector2(minDmg, maxDmg);
        }
        public Vector2 GetPosition2V()
        {
            return new Vector2(pos.x, pos.z);
        }
        public Vector3 GetPosition3V()
        {
            return pos;
        }
        public void AddPosition(Vector3 _pos)
        {
            pos.x += _pos.x;
            pos.y += _pos.y;
            pos.z += _pos.z;
            return;
        }
        public string GetName()
        {
            return name;
        }
        public int GetHealth()
        {
            return health;
        }
        public bool ReceiveDmg(int _damage)
        {
            // Receive Damage to Monster
            health -= Program.Clamp(_damage, 0, int.MaxValue);
            // Did not die return: False
            if (health <= 0)
            {
                return true;
            }
            return false;
        }
        public void DrawEnemy()
        {
            // Draw Enemy
            Console.WriteLine("*Insert Monster Appearance*");
        }
        internal bool Move(CompassDirection dir)
        {
            if (!Program.GetWorld().GetRoom(pos).CanWalk(dir))
            {
                return false;
            }
            switch (dir)
            {
                case CompassDirection.North:
                    AddPosition(new Vector3(0, 0, -1));
                    break;
                case CompassDirection.East:
                    AddPosition(new Vector3(1, 0, 0));
                    break;
                case CompassDirection.South:
                    AddPosition(new Vector3(0, 0, +1));
                    break;
                case CompassDirection.West:
                    AddPosition(new Vector3(-1, 0, 0));
                    break;
            }
            return true;
        }

        internal int GetMaxHealth()
        {
            return maxHealth;
        }
    }
    public class World
    {
        private Player player;
        private int startRoomCount = 5;
        private List<FloorLevel> floorLevels;
        private int currentFloor = -1;
        private bool bigMap = true;
        bool gameRunning = true;
        public World(ref Player _player)
        {
            player = _player;
        }
        public void main()
        {
            gameRunning = true;
            Setup();
            GameLoop();
            if (player.GetHealth() <= 0) Console.WriteLine("--You Died--");
            else Console.WriteLine($"Hope to see you soon:{player.name}");
            Console.Write(">>");
            Console.ReadLine();
        }
        private void GameLoop()
        {
            bool playerAlive;
            while (gameRunning)
            {
                PlayerTurn();
                if (gameRunning)
                {
                    MonsterTurn();
                    playerAlive = CheckMonsterOverlap();
                    if (!playerAlive) gameRunning = false;
                }
            }
        }
        private bool CheckMonsterOverlap()
        {
            Vector2 playerPos = player.GetPositionV2();
            while (true)
            {
                int x = 0;
                Monster monstersAtPlayer = null;
                foreach (Monster monster in floorLevels[currentFloor].GetMonsterList())
                {
                    Vector2 mPos = monster.GetPosition2V();
                    if (mPos.x == playerPos.x && mPos.z == playerPos.z)
                    {
                        monstersAtPlayer = monster;
                        break;
                    }
                    x++;
                }
                if (monstersAtPlayer != null)
                {
                    bool playerDied = MonsterEncounter(monstersAtPlayer);
                    if (playerDied)
                    {
                        return false;
                    }
                    floorLevels[currentFloor].GetMonsterList().Remove(monstersAtPlayer); // Removes Monster from list of monsters
                }
                else
                    break;
            }
            return true;
        }
        private void Setup()
        {
            Program.randomBuild = new Random(Program.seed);
            Program.randomAI = new Random(Program.seed);
            floorLevels = new List<FloorLevel>();
            NewFloor();
            currentFloor = 0;
            player.SetPosition(new Vector3(player.GetPositionV2().x,0, player.GetPositionV2().z));
        }
        private void GoUpStairs()
        {
            if (floorLevels.Count >= currentFloor + 1)
            {
                NewFloor();
            }
            else
            {
                currentFloor++;
                player.SetPosition(new Vector3(player.GetPositionV2().x, player.GetPositionV3().y + 1, player.GetPositionV2().z));
            }
        }
        private void GoDownStairs()
        {
            if (currentFloor == 0) return;
                currentFloor--;
            player.SetPosition(new Vector3(player.GetPositionV2().x, player.GetPositionV3().y - 1, player.GetPositionV2().z));
        }
        private void PlayerTurn()
        {
            bool actionTaken = false;
            Room currentRoom = GetRoom(player.GetPositionV2());
            while (!actionTaken)
            {
                Console.Clear();
                Console.WriteLine($"{ player.GetPositionV2().x}  { player.GetPositionV2().z}");
                if (bigMap)
                DrawBigMap();
                Console.WriteLine("");
                Console.WriteLine("-------------------------");
                if (currentRoom.CanWalk(CompassDirection.North))
                {
                    Console.Write(" Walk North [W]");
                }
                if (currentRoom.CanWalk(CompassDirection.West))
                {
                    Console.Write(" Walk West [A]");
                }
                if (currentRoom.CanWalk(CompassDirection.South))
                {
                    Console.Write(" Walk South [S]");
                }
                if (currentRoom.CanWalk(CompassDirection.East))
                {
                    Console.Write(" Walk East [D]");
                }
                Console.WriteLine("");
                if (currentRoom.GetRoomType() == RoomType.StairCase)
                {
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("Go Up Stairs [U]");
                    if (currentFloor != 0)
                    Console.WriteLine("Go Down Stairs [J]");
                }
                Console.WriteLine("-------------------------");
                Console.WriteLine("Open Map [M]");
                Console.WriteLine($"Big Map toggle:{bigMap}");
                Console.WriteLine("-------------------------");
                Console.WriteLine("Quit [Esc]");
                
                ConsoleKeyInfo input = Console.ReadKey();
                if (input.Key == ConsoleKey.W && currentRoom.CanWalk(CompassDirection.North))
                {
                    player.AddPosition(new Vector2(0, -1));
                    actionTaken = true;
                }
                else if (input.Key == ConsoleKey.D && currentRoom.CanWalk(CompassDirection.East))
                {
                    player.AddPosition(new Vector2(1, 0));
                    actionTaken = true;
                }
                else if (input.Key == ConsoleKey.A && currentRoom.CanWalk(CompassDirection.West))
                {
                    player.AddPosition(new Vector2(-1, 0));
                    actionTaken = true;
                }
                else if (input.Key == ConsoleKey.S && currentRoom.CanWalk(CompassDirection.South))
                {
                    player.AddPosition(new Vector2(0, 1));
                    actionTaken = true;
                }
                else if (input.Key == ConsoleKey.U && currentRoom.GetRoomType() == RoomType.StairCase)
                {
                    GoUpStairs();
                    actionTaken = true;
                }
                else if (input.Key == ConsoleKey.J && currentRoom.GetRoomType() == RoomType.StairCase)
                {
                    GoDownStairs();
                    actionTaken = true;
                }
                else if (input.Key == ConsoleKey.Escape)
                {
                    ConsoleKeyInfo securityCheck;
                    while (true)
                    {
                        Console.WriteLine("Are You sure you want to Quit the game");
                        Console.WriteLine("Yes [Y]");
                        Console.WriteLine("No  [N]");
                        securityCheck = Console.ReadKey();

                        if (securityCheck.Key == ConsoleKey.Y)
                        {
                            gameRunning = false;
                            actionTaken = true;
                            return;
                        }
                        else if (securityCheck.Key == ConsoleKey.N)
                            break;
                    }
                }
                else if (input.Key == ConsoleKey.M)
                {
                    DrawCurrentRoom();

                    Console.Write(">>");
                    Console.ReadLine();
                }
            }
        }
        private void MonsterTurn()
        {
            int emergenceBreak = 0;
            foreach (Monster monster in floorLevels[currentFloor].GetMonsterList())
            {
                int dir = Program.randomAI.Next(0, 5);
                if (dir != 4)
                while (!monster.Move((CompassDirection)dir))
                {
                    if (emergenceBreak > 10)
                    {
                        break;
                    }
                    emergenceBreak++;
                }
            }
        }
        public void NewFloor()
        {
            startRoomCount = Program.randomBuild.Next(4, 10);
            floorLevels.Add(new FloorLevel(player.GetPositionV2(), new Vector2(startRoomCount, startRoomCount), floorLevels.Count));
            currentFloor++;
            player.SetPosition(new Vector3(player.GetPositionV2().x, player.GetPositionV3().y + 1, player.GetPositionV2().z));
            // Console.Write(floorLevels[currentFloor].GetMap());
        }
        public Room GetRoom(Vector2 pos)
        {
            return floorLevels[currentFloor].rooms[pos.x, pos.z];
        }
        public Room GetRoom(Vector3 pos)
        {
            return floorLevels[pos.y].rooms[pos.x, pos.z];
        }
        private void DrawBigMap()
        {
            Vector3 pos = player.GetPositionV3();
            // Draw Current room your in
            Console.WriteLine(floorLevels[pos.y].GetMap());
        }
        private void DrawCurrentRoom()
        {
            Vector3 pos = player.GetPositionV3();
            // Draw Current room you are in
            Console.Clear();
            string[] str = floorLevels[pos.y].rooms[pos.x, pos.z].GetRoomMapView(false);
            string printValue = "";
            foreach (string s in str)
            {
                printValue += "\n";
                printValue += s;
            }
            Console.WriteLine(printValue);
        }
        private bool MonsterEncounter(Monster monster)
        {
            bool tempBattle = true;
            while (tempBattle)
            {
                ConsoleKeyInfo input;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("----Battle----\n" +
                                     $"{player.name} | {player.GetHealth()} / {player.GetMaxHealth()} Hp\n" +
                                     $"{monster.GetName()} | {monster.GetHealth()} / {monster.GetMaxHealth()} Hp\n" +
                                      "Attack [A]\n" +
                                      "Heal [H]");
                    Console.Write(">>");
                    input = Console.ReadKey();
                    Console.WriteLine("--------------");
                    if (input.Key == ConsoleKey.A)
                    {
                        Vector2 pDmg = player.GetDamage();
                        int instancePlayerDamage = Program.randomAI.Next(pDmg.x, pDmg.z);
                        monster.ReceiveDmg(instancePlayerDamage);
                        Console.WriteLine($"You strike: {instancePlayerDamage} Damage\n" +
                                          $"To {monster.GetName()} | {monster.GetHealth()} / {monster.GetMaxHealth()} Hp");
                        if (monster.GetHealth() <= 0)
                        {
                            return false;
                        }
                        break;
                    }
                    else if (input.Key == ConsoleKey.H)
                    {
                        Vector2 pDmg = player.GetDamage();
                        int instancePlayerHeal = Program.randomAI.Next(pDmg.x, pDmg.z);
                        player.Heal(instancePlayerHeal);
                        Console.WriteLine($"{player.name} Heals: {instancePlayerHeal} Hp");
                        Console.WriteLine($"{player.name} | {player.GetHealth()} / {player.GetMaxHealth()}");
                        break;
                    }
                }
                if (monster.GetHealth() <= 0)
                {
                    Console.WriteLine($"{monster.GetName()} Died");
                    tempBattle = false;
                    return false;
                }
                else
                {
                    Vector2 monsterDmg = monster.GetDamage();
                    int instanceMonsterDamage = Program.randomAI.Next(monsterDmg.x, monsterDmg.z);
                    player.ReceiveDmg(instanceMonsterDamage);
                    Console.WriteLine($"{monster.GetName()} Strikes you for {instanceMonsterDamage} Damage");
                    Console.WriteLine($"{player.name} | {player.GetHealth()} / {player.GetMaxHealth()}");
                }
                if (player.GetHealth() <= 0)
                {
                    return true; // player Died
                }
                Console.WriteLine($"-------------");
                Console.Write(">>");
                Console.ReadKey();
            }

            return false; // returns true if player died
        }
    }
    public class FloorLevel
    {
        public int level;
        public Room[,] rooms;
        Vector2 floorSize;
        List<Monster> monsters;
        public FloorLevel(Vector2 _stairsPos, Vector2 _floorSize, int _level)
        {
            level = _level;
            floorSize = _floorSize;
            CreateRooms(_floorSize);
            rooms[_stairsPos.x, _stairsPos.z].ChangeType(RoomType.StairCase);
            generatemonsters();
        }
        // Create Monsters Randomly in Floor
        private void generatemonsters()
        {
            monsters = new List<Monster>();
            int monstercounter = Program.randomAI.Next(3, 5);
            for (int i = 0; i < monstercounter; i++)
            {
                monsters.Add
                (
                    new Monster("test", 1, 2, 3,
                        new Vector3(
                            Program.randomAI.Next(0, floorSize.x),
                            level,
                            Program.randomAI.Next(0, floorSize.z)
                                    )
                        )
                );
            }
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
                    rooms[x, z] = new Room(this, (RoomType)Program.randomBuild.Next(0, (int)RoomType.TotalRooms), new Vector2(x,z));
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

        internal List<Monster> GetMonsterList()
        {
            return monsters;
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
        private FloorLevel floor;
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
        public Room(FloorLevel _floor, RoomType _type, Vector2 _pos)
        {
            floor = _floor;
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
            return GetRoomMapView(true);
        }
        public string[] GetRoomMapView(bool withPlayer)
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
                        " ' ",
                        "# #"
                    };
                    break;
                case RoomType.Hallway_Horizontal:
                    returnValue = new string[3]
                    {
                        "###",
                        " ' ",
                        "###"
                    };
                    break;
                case RoomType.Hallway_Vertical:
                    returnValue = new string[3]
                    {
                        "# #",
                        "#'#",
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
            Vector2 playerPos = Program.GetPlayer().GetPositionV2();
            if (pos.x == playerPos.x && pos.z == playerPos.z && withPlayer)
            {
                returnValue[1] = returnValue[1].Remove(1, 1).Insert(1, "P");
            }
            else
            {
                Vector2 monsterPosition;
                foreach(Monster monster in floor.GetMonsterList())
                {
                    monsterPosition = monster.GetPosition2V();
                    if (monsterPosition.x == pos.x && monsterPosition.z == pos.z)
                    {
                        returnValue[1] = returnValue[1].Remove(1, 1).Insert(1, "E");
                    }
                }
            }

            return returnValue;
        }
        public bool CanWalk(CompassDirection dir)
        {
            if (!door[dir])
            {
                return false;
            }

            switch (dir)
            {
                case CompassDirection.North:
                    if (adjacentRooms[CompassDirection.North] == null)
                    {
                        break;
                    }
                    if (adjacentRooms[CompassDirection.North].door[CompassDirection.South])
                    {
                        return true;
                    }
                    break;
                case CompassDirection.East:
                    if (adjacentRooms[CompassDirection.East] == null)
                    {
                        break;
                    }
                    if (adjacentRooms[CompassDirection.East].door[CompassDirection.West])
                    {
                        return true;
                    }
                    break;
                case CompassDirection.South:
                    if (adjacentRooms[CompassDirection.South] == null)
                    {
                        break;
                    }
                    if (adjacentRooms[CompassDirection.South].door[CompassDirection.North])
                    {
                        return true;
                    }
                    break;
                case CompassDirection.West:
                    if (adjacentRooms[CompassDirection.West] == null)
                    {
                        break;
                    }
                    if (adjacentRooms[CompassDirection.West].door[CompassDirection.East])
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }
        private string CreateRoomDescription()
        {

            return "None";
        }

        internal RoomType GetRoomType()
        {
            return type;
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
