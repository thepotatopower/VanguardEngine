using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanguardEngine
{
    public class CardEventArgs : EventArgs
    {
        public Card card { get; set; }
        public List<Card> cardList { get; set; }
        public Card[] cardArray { get; set; }
        public int playerID { get; set; }
        public int i { get; set; }
        public bool b { get; set; }
    }
}
