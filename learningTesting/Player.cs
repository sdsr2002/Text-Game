using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Game
{
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
        internal int Health => _health;
        internal int MaxHealth => _maxHealth;
        internal void Heal(int healAmount)
        {
            _health += Program.Clamp(healAmount, 0, int.MaxValue);
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }
            return;
        }
    }
}
