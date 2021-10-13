using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Game
{
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
}
