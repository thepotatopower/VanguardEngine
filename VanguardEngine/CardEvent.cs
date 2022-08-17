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
        public int currentPower { get; set; }
        public int currentShield { get; set; }
        public int currentCritical { get; set; }
        public int currentSoul { get; set; }
        public int currentGrade { get; set; }
        public int guardTarget { get; set; }
        public int circle { get; set; }
        public int movementType { get; set; }
        public List<Card> cardList { get; set; }
        public int playerID { get; set; }
        public List<int> intList { get; set; }
        public int[] intArray { get; set; }
        public bool faceup { get; set; }
        public bool upright { get; set; }
        public int i { get; set; }
        public int location { get; set; }
        public bool b { get; set; }
        public string abilityText { get; set; }
        public Tuple<int, int> previousLocation { get; set; }
        public Tuple<int, int> currentLocation { get; set; }
        public RecordedUnitValue recordedUnitValue { get; set; }
        public List<CardValues> cardValues { get; set; }
        public bool left = true;


        public string message { get; set; }
        //public int FL { get; set; }

        public CardEventArgs()
        {
            cardList = new List<Card>();
            intList = new List<int>();
            cardValues = new List<CardValues>();
        }
    }
}
