using System;
using System.Collections.Generic;
using System.Text;

namespace VanguardEngine
{
    public class Field
    {
        protected List<Card> _player1Hand = new List<Card>();
        protected List<Card> _player2Hand = new List<Card>();
        protected List<Card> _player1Deck = new List<Card>();
        protected List<Card> _player2Deck = new List<Card>();
        protected List<Card> _cardCatalog = new List<Card>();
        protected Card[] _units = new Card[14];
        protected List<Card> _GC = new List<Card>();
        protected bool _sentinel = false;
        protected int _attack = -1;
        protected int _attacker = -1;
        protected int _attacked = -1;
        protected int _booster = -1;
        protected bool _guarding = false;
        protected List<Card> _player1Drop = new List<Card>();
        protected List<Card> _player2Drop = new List<Card>();
        protected List<Card> _player1Damage = new List<Card>();
        protected List<Card> _player2Damage = new List<Card>();
        protected List<Card> _player1Bind = new List<Card>();
        protected List<Card> _player2Bind = new List<Card>();
        protected Card _player1Trigger;
        protected Card _player2Trigger;
        protected List<Card> _player1Order = new List<Card>();
        protected List<Card> _player2Order = new List<Card>();
        protected List<Card> _player1RideDeck = new List<Card>();
        protected List<Card> _player2RideDeck = new List<Card>();
        protected List<Card> _player1Revealed = new List<Card>();
        protected List<Card> _player2Revealed = new List<Card>();
        protected Player _player2Player;
        protected int[] _shuffleKey;
        protected int _turn = 1;

        public List<Card> Player1Deck
        {
            get => _player1Deck;
            set => _player1Deck = value;
        }

        public List<Card> Player2Deck
        {
            get => _player2Deck;
            set => _player2Deck = value;
        }

        public List<Card> CardCatalog
        {
            get => _cardCatalog;
            set => _cardCatalog = value;
        }

        public int[] ShuffleKey
        {
            get => _shuffleKey;
            set => _shuffleKey = value;
        }
        public List<Card> Player1Hand
        {
            get => _player1Hand;
        }

        public List<Card> Player2Hand
        {
            get => _player2Hand;
        }

        public Card[] Units
        {
            get => _units;
        }

        public List<Card> Player1RideDeck
        {
            get => _player1RideDeck;
        }

        public List<Card> Player2RideDeck
        {
            get => _player2RideDeck;
        }

        public List<Card> GC
        {
            get => _GC;
        }

        public bool Sentinel
        {
            get => _sentinel;
            set => _sentinel = value;
        }

        public bool Guarding
        {
            get => _guarding;
            set => _guarding = value;
        }
        public List<Card> Player1Drop
        {
            get => _player1Drop;
        }

        public List<Card> Player2Drop
        {
            get => _player2Drop;
        }

        public List<Card> Player1Bind
        {
            get => _player1Bind;
        }

        public List<Card> Player2Bind
        {
            get => _player2Bind;
        }

        public List<Card> Player1Order
        {
            get => _player1Order;
        }

        public List<Card> Player2Order
        {
            get => _player2Order;
        }

        public List<Card> Player1Revealed
        {
            get => _player1Revealed;
        }

        public List<Card> Player2Revealed
        {
            get => _player2Revealed;
        }

        public int Attack
        {
            get => _attack;
            set => _attack = value;
        }

        public int Attacker
        {
            get => _attacker;
            set => _attacker = value;
        }

        public int Attacked
        {
            get => _attacked;
            set => _attacked = value;
        }

        public int Booster
        {
            get => _booster;
            set => _booster = value;
        }

        public Card Player1Trigger
        {
            get => _player1Trigger;
            set => _player1Trigger = value;
        }

        public Card Player2Trigger
        {
            get => _player2Trigger;
            set => _player2Trigger = value;
        }

        public List<Card> Player1DamageZone
        {
            get => _player1Damage;
        }

        public List<Card> Player2DamageZone
        {
            get => _player2Damage;
        }

        public int Turn
        {
            get => _turn;
        }

        public void IncrementTurn()
        {
            _turn++;
        }

        public void Initialize(List<Card> deck1, List<Card> deck2)
        {
            _units[FL.PlayerVanguard] = deck1[0].Clone();
            _units[FL.PlayerVanguard].faceup = false;
            _units[FL.PlayerVanguard].location = Location.Field;
            _units[FL.EnemyVanguard] = deck2[0].Clone();
            _units[FL.EnemyVanguard].faceup = false;
            _units[FL.EnemyVanguard].location = Location.Field;
            for (int i = 1; i < 4; i++)
            {
                _player1RideDeck.Add(deck1[i].Clone());
                _player1RideDeck[_player1RideDeck.Count - 1].location = Location.RideDeck;
                _player2RideDeck.Add(deck2[i].Clone());
                _player2RideDeck[_player2RideDeck.Count - 1].location = Location.RideDeck;
            }
            for (int i = 4; i < 50; i++)
            {
                _player1Deck.Add(deck1[i].Clone());
                _player2Deck.Add(deck2[i].Clone());
            }
            for (int i = 0; i < deck1.Count + deck2.Count; i++)
            {
                _cardCatalog.Add(null);
            }
            foreach (Card card in _player1Deck)
                _cardCatalog[card.tempID] = card;
            foreach (Card card in _player1RideDeck)
                _cardCatalog[card.tempID] = card;
            foreach (Card card in _player2Deck)
                _cardCatalog[card.tempID] = card;
            foreach (Card card in _player2RideDeck)
                _cardCatalog[card.tempID] = card;
            _cardCatalog[_units[FL.PlayerVanguard].tempID] = _units[FL.PlayerVanguard];
            _cardCatalog[_units[FL.EnemyVanguard].tempID] = _units[FL.EnemyVanguard];
            Shuffle(_player1Deck);
            Shuffle(_player2Deck);
        }

        static public class FisherYates
        {
            static Random r = new Random();
            //  Based on Java code from wikipedia:
            //  http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
            static public int[] Shuffle(List<Card> list)
            {
                Card temp;
                int k;
                int[] key = new int[list.Count];
                for (int n = list.Count - 1; n > 0; --n)
                {
                    k = r.Next(n + 1);
                    key[n] = k;
                    temp = list[n];
                    list[n] = list[k];
                    list[k] = temp;
                }
                return key;
            }
        }

        public void Shuffle(List<Card> list)
        {
            FisherYates.Shuffle(list);
        }

        //public void sendShuffleKey(List<Card> list)
        //{
        //    int[] key = FisherYates.Shuffle(list);
        //    _player2Player.GiveShuffleKey(key);
        //}

        //public void readShuffleKey(List<Card> list)
        //{
        //    Card temp;
        //    for (int n = list.Count - 1; n > 0; --n)
        //    {
        //        temp = list[n];
        //        list[n] = list[_shuffleKey[n]];
        //        list[_shuffleKey[n]] = temp;
        //    }
        //}
    }

    public class FL //FieldLocation
    {
        public const int EnemyFrontLeft = 1;
        public const int EnemyBackLeft = 2;
        public const int EnemyBackCenter = 3;
        public const int EnemyFrontRight = 4;
        public const int EnemyBackRight = 5;
        public const int EnemyVanguard = 6;
        public const int PlayerFrontLeft = 8;
        public const int PlayerBackLeft = 9;
        public const int PlayerBackCenter = 10;
        public const int PlayerFrontRight = 11;
        public const int PlayerBackRight = 12;
        public const int PlayerVanguard = 13;
        public const int GuardianCircle = 14;

        public static int SwitchSides(int location)
        {
            if (location >= EnemyFrontLeft && location <= EnemyVanguard)
                return location + 7;
            else
                return location - 7;
        }

        public static int MaxFL()
        {
            return 14;
        }
    }
}
