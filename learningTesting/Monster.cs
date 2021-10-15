using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Game
{
    public class Monster
    {
        public string name { get; private set; }
        public int minDmg { get; private set; }
        public int maxDmg { get; private set; }
        public int maxHealth { get; private set; }
        public int health { get; private set; }
        public string description { get; private set; }
        public int monsterGrade;
        protected Vector2 pos;
        protected List<ActionData> actions;
        public Vector2 Damage => new Vector2(minDmg, maxDmg);
        public Vector2 Position2V => pos;
        public int Health => health;

        public Monster(string _name, int _minDmg, int _maxDmg, int _maxHealth, Vector2 _pos,string description_)
        {
            SetupAction();
            name = _name;
            minDmg = _minDmg;
            maxDmg = _maxDmg;
            maxHealth = _maxHealth;
            health = maxHealth;
            pos = _pos;
            description = description_;
        }
        public ref List<ActionData> GetActions()
        {
            return ref actions;
        }
        public void AddPosition(Vector2 _pos)
        {
            pos.x += _pos.x;
            pos.z += _pos.z;
            return;
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
            if (!Program.World.GetRoom(pos).CanWalk(dir))
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
        public virtual void CombatAction()
        {
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
                // Mold // new ActionData(() => MyActionToDo(ref Program.GetPlayer()), (int)chanceOfAction), // Mold //
                new ActionData(() => MendWound(ref Program.Player), 10),
                new ActionData(() => AttackPlayer(ref Program.Player), 100)
            };
        }
        // Actions
        protected void MendWound(ref Player player)
        {
            int x = Program.randomAI.Next(minDmg, maxDmg);
            Console.WriteLine($"{name} Healed themself:{x}");
            health += x;
            Console.WriteLine($"{name} | {health}/{maxHealth}");
        }

        protected void AttackPlayer(ref Player player)
        {
            Vector2 monsterDmg = Damage;
            int instanceMonsterDamage = Program.randomAI.Next(monsterDmg.x, monsterDmg.z);
            player.ReceiveDmg(instanceMonsterDamage);
            Console.WriteLine($"{name} Strikes you for {instanceMonsterDamage} Damage");
            Console.WriteLine($"{player.name} | {player.Health} / {player.MaxHealth}");
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

}
