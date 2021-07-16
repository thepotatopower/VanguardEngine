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
        protected List<Card> _tokens = new List<Card>();
        protected Card[] _cardCatalog = new Card[200];
        protected Card[] _units = new Card[14];
        protected int[] _circlePower = new int[14];
        protected int[] _circleCritical = new int[14];
        protected List<Card> _GC = new List<Card>();
        protected Dictionary<int, List<Card>> _guardians = new Dictionary<int, List<Card>>();
        protected List<Card> _sentinel = new List<Card>();
        protected int _attacker = -1;
        protected List<Card> _attacked = new List<Card>();
        protected int _booster = -1;
        protected bool _player1PersonaRide = false;
        protected bool _player2PersonaRide = false;
        protected List<Card> _player1Drop = new List<Card>();
        protected List<Card> _player2Drop = new List<Card>();
        protected List<Card> _player1Damage = new List<Card>();
        protected List<Card> _player2Damage = new List<Card>();
        protected List<Card> _player1Bind = new List<Card>();
        protected List<Card> _player2Bind = new List<Card>();
        protected Card[] _player1Trigger = new Card[1];
        protected Card[] _player2Trigger = new Card[1];
        protected List<Card> _player1Order = new List<Card>();
        protected List<Card> _player2Order = new List<Card>();
        protected List<Card> _player1RideDeck = new List<Card>();
        protected List<Card> _player2RideDeck = new List<Card>();
        protected List<Card> _player1Revealed = new List<Card>();
        protected List<Card> _player2Revealed = new List<Card>();
        protected List<Card> _player1Looking = new List<Card>();
        protected List<Card> _player2Looking = new List<Card>();
        protected Card _player1Prison = null;
        protected Card _player2Prison = null;
        protected Card _player1World = null;
        protected Card _player2World = null;
        protected List<Card> _player1Prisoners = new List<Card>();
        protected List<Card> _player2Prisoners = new List<Card>();
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

        public Card[] CardCatalog
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

        public int[] CirclePower
        {
            get => _circlePower;
        }

        public int[] CircleCritical
        {
            get => _circleCritical;
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

        public Dictionary<int, List<Card>> Guardians
        {
            get => _guardians;
        }

        public List<Card> Sentinel
        {
            get => _sentinel;
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

        public List<Card> Player1Looking
        {
            get => _player1Looking;
        }

        public List<Card> Player2Looking
        {
            get => _player2Looking;
        }

        public int Attacker
        {
            get => _attacker;
            set => _attacker = value;
        }

        public List<Card> Attacked
        {
            get => _attacked;
        }

        public int Booster
        {
            get => _booster;
            set => _booster = value;
        }

        public Card[] Player1Trigger
        {
            get => _player1Trigger;
        }

        public Card[] Player2Trigger
        {
            get => _player2Trigger;
        }

        public List<Card> Player1DamageZone
        {
            get => _player1Damage;
        }

        public List<Card> Player2DamageZone
        {
            get => _player2Damage;
        }

        public Card Player1Prison
        {
            get => _player1Prison;
        }

        public Card Player2Prison
        {
            get => _player2Prison;
        }

        public List<Card> Player1Prisoners
        {
            get => _player1Prisoners;
        }

        public List<Card> Player2Prisoners
        {
            get => _player2Prisoners;
        }

        public Card Player1World
        {
            get => _player1World;
        }

        public Card Player2World
        {
            get => _player2World;
        }

        public int Turn
        {
            get => _turn;
        }

        public void IncrementTurn()
        {
            _turn++;
        }

        public void Initialize(List<Card> deck1, List<Card> deck2, List<Card> tokens)
        {
            foreach (Card card in tokens)
                _tokens.Add(card);
            _units[FL.PlayerVanguard] = deck1[0].Clone();
            _units[FL.PlayerVanguard].faceup = false;
            _units[FL.PlayerVanguard].location = Location.VC;
            _units[FL.EnemyVanguard] = deck2[0].Clone();
            _units[FL.EnemyVanguard].faceup = false;
            _units[FL.EnemyVanguard].location = Location.VC;
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
            foreach (Card card in _player1Deck)
            {
                _cardCatalog[card.tempID] = card;
                card.originalOwner = 1;
            }
            foreach (Card card in _player1RideDeck)
            {
                _cardCatalog[card.tempID] = card;
                card.originalOwner = 1;
            }
            foreach (Card card in _player2Deck)
            {
                _cardCatalog[card.tempID] = card;
                card.originalOwner = 2;
            }
            foreach (Card card in _player2RideDeck)
            {
                _cardCatalog[card.tempID] = card;
                card.originalOwner = 2;
            }
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

        public bool GetPersonaRide(int playerID)
        {
            if (playerID == 1)
                return _player1PersonaRide;
            else
                return _player2PersonaRide;
        }

        public void SetPersonaRide(bool active, int playerID)
        {
            if (playerID == 1)
                _player1PersonaRide = active;
            else
                _player2PersonaRide = active;
        }

        public int CreateToken(string tokenID)
        {
            Card template = null;
            foreach (Card card in _tokens)
            {
                if (card.id == tokenID)
                {
                    template = card;
                    break;
                }
            }
            Card token = template.Clone();
            for (int i = 0; i < _cardCatalog.Length; i++)
            {
                if (_cardCatalog[i] == null)
                {
                    token.tempID = i;
                    _cardCatalog[i] = token;
                    return i;
                }
            }
            return -1;
        }
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
        public const int OpenCircle = 100;

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
