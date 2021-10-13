using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Game
{
    public class Monster
    {
        protected string name;
        protected int minDmg;
        protected int maxDmg;
        protected int maxHealth;
        protected int health;
        protected string _description;
        protected Vector2 pos;
        protected List<ActionData> actions;
        public Monster(string _name, int _minDmg, int _maxDmg, int _health, Vector2 _pos,string description)
        {
            SetupAction();
               name = _name;
            minDmg = _minDmg;
            maxDmg = _maxDmg;
            maxHealth = _health;
            health = maxHealth;
            pos = _pos;
            _description = description;
        }
        public ref List<ActionData> GetActions()
        {
            return ref actions;
        }
        public Vector2 GetDamage()
        {
            return new Vector2(minDmg, maxDmg);
        }
        public Vector2 GetPosition2V()
        {
            return pos;
        }
        public void AddPosition(Vector2 _pos)
        {
            pos.x += _pos.x;
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
        public bool Move(CompassDirection dir)
        {
            if (!Program.GetWorld().GetRoom(pos).CanWalk(dir))
            {
                return false;
            }
            switch (dir)
            {
                case CompassDirection.North:
                    AddPosition(new Vector2(0, -1));
                    break;
                case CompassDirection.East:
                    AddPosition(new Vector2(1, 0));
                    break;
                case CompassDirection.South:
                    AddPosition(new Vector2(0, +1));
                    break;
                case CompassDirection.West:
                    AddPosition(new Vector2(-1, 0));
                    break;
            }
            return true;
        }
        public virtual void DoAction()
        {
            int chance = Program.randomAI.Next(0, 100);
            bool doneAction;
            foreach(ActionData act in actions)
            {
                doneAction = act.ActivateAction();
                if (doneAction)
                    break;
            }
            Console.ReadKey();
        }
        public int GetMaxHealth()
        {
            return maxHealth;
        }
        // Add Actions to Action List
        protected virtual void SetupAction()
        {
            actions = new List<ActionData>()
            {
                new ActionData(() => MendWound(ref Program.GetPlayer()), 10),
                new ActionData(() => AttackPlayer(ref Program.GetPlayer()), 100)
            };
        }
        // Actions
        public void MendWound(ref Player player)
        {
            int x = Program.randomAI.Next(minDmg, maxDmg);
            Console.WriteLine($"{name} Healed themself:{x}");
            health += x;
            Console.WriteLine($"{name} | {health}/{maxHealth}");
        }

        public void AttackPlayer(ref Player player)
        {
            Vector2 monsterDmg = GetDamage();
            int instanceMonsterDamage = Program.randomAI.Next(monsterDmg.x, monsterDmg.z);
            player.ReceiveDmg(instanceMonsterDamage);
            Console.WriteLine($"{name} Strikes you for {instanceMonsterDamage} Damage");
            Console.WriteLine($"{player.name} | {player.GetHealth()} / {player.GetMaxHealth()}");
        }
    }
    public class Goblin : Monster
    {
        public Goblin(Vector2 pos): base("goblin",1,3,4,pos, "a little Green man") 
        { 

        }
    }
    public class Kobold : Monster
    {
        public Kobold(Vector2 pos) : base("Kobold", 2, 3, 7, pos, "a big Furry Rat")
        {

        }
    }
    public class GiantSpider : Monster
    {
        public GiantSpider(Vector2 pos) : base("Giant Spider", 0, 6, 2, pos, "a ginormous Spider")
        {

        }
    }
    public class ActionData
    {
        private Action _action;
        private int _chance;
        public ActionData(Action action, int procentChanceOfAction)
        {
            _action = action;
            _chance = procentChanceOfAction;
        }

        public bool ActivateAction()
        {
            if (Program.randomAI.Next(0,100) > _chance)
                return false;
            _action.Invoke();
            return true;
        }
    }

    public enum MonsterType 
    {
        Goblin,
        Kobold,
        Giant_Spider,
        TotalAmountOfTypes
    }
}
