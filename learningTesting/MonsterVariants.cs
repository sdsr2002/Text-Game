
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Game
{
    public class Goblin : Monster
    {
        public Goblin(Vector2 pos): base("Goblin",1,3,4,pos, "a little Green man") 
        {
        }
        protected override void SetupAction()
        {
            actions = new List<ActionData>()
            {
                new ActionData(() => Scream(), 50),
                new ActionData(() => AttackPlayer(ref Program.Player), 100)
            };
        }
        protected void Scream()
        {
            Console.WriteLine("Goblin Screams");
            Console.Beep();
        }
    }
    public class Kobold : Monster
    {
        public Kobold(Vector2 pos) : base("Kobold", 2, 3, 7, pos, "a big Furry Rat")
        {

        }
        protected override void SetupAction()
        {
            actions = new List<ActionData>()
            {
                new ActionData(() => AttackPlayer(ref Program.Player), 100)
            };
        }
    }
    public class GiantSpider : Monster
    {
        public GiantSpider(Vector2 pos) : base("Giant Spider", 0, 6, 2, pos, "a ginormous Spider")
        {

        }
        protected override void SetupAction()
        {
            actions = new List<ActionData>()
            {
                new ActionData(() => AttackPlayer(ref Program.Player), 100)
            };
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
