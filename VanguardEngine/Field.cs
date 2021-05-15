using System;
using System.Collections.Generic;
using System.Text;

namespace VanguardEngine
{
    public class Field
    {
        protected List<Card> _playerHand = new List<Card>();
        protected List<Card> _enemyHand = new List<Card>();
        protected List<Card> _playerDeck = new List<Card>();
        protected List<Card> _enemyDeck = new List<Card>();
        //protected Card _playerVG;
        //protected Card _enemyVG;
        //protected Card[] _playerRG = new Card[5];
        //protected Card[] _enemyRG = new Card[5];
        protected Card[] _units = new Card[14];
        protected List<Card> _GC = new List<Card>();
        protected bool _sentinel = false;
        protected int _attack = -1;
        protected int _attacker = -1;
        protected int _attacked = -1;
        protected int _booster = -1;
        protected bool _guarding = false;
        protected List<Card> _playerDrop = new List<Card>();
        protected List<Card> _enemyDrop = new List<Card>();
        protected List<Card> _playerDamage = new List<Card>();
        protected List<Card> _enemyDamage = new List<Card>();
        protected List<Card> _playerBind = new List<Card>();
        protected List<Card> _enemyBind = new List<Card>();
        protected Card _playerTrigger;
        protected Card _enemyTrigger;
        protected Card _playerOrder;
        protected Card _enemyOrder;
        protected List<Card> _playerRideDeck = new List<Card>();
        protected List<Card> _enemyRideDeck = new List<Card>();
        protected Player _enemyPlayer;
        protected int[] _shuffleKey;

        public List<Card> PlayerDeck
        {
            get => _playerDeck;
            set => _playerDeck = value;
        }

        public List<Card> EnemyDeck
        {
            get => _enemyDeck;
            set => _enemyDeck = value;
        }

        public int[] ShuffleKey
        {
            get => _shuffleKey;
            set => _shuffleKey = value;
        }
        public List<Card> PlayerHand
        {
            get => _playerHand;
        }

        public List<Card> EnemyHand
        {
            get => _enemyHand;
        }

        //public Card PlayerVG
        //{
        //    get => _playerVG;
        //    set => _playerVG = value;
        //}

        //public Card EnemyVG
        //{
        //    get => _enemyVG;
        //    set => _enemyVG = value;
        //}

        //public Card[] PlayerRG
        //{
        //    get => _playerRG;
        //}

        //public Card[] EnemyRG
        //{
        //    get => _enemyRG;
        //}

        public Card[] Units
        {
            get => _units;
        }

        public List<Card> PlayerRideDeck
        {
            get => _playerRideDeck;
        }

        public List<Card> EnemyRideDeck
        {
            get => _enemyRideDeck;
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
        public List<Card> PlayerDrop
        {
            get => _playerDrop;
        }

        public List<Card> EnemyDrop
        {
            get => _enemyDrop;
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

        public Card PlayerTrigger
        {
            get => _playerTrigger;
            set => _playerTrigger = value;
        }

        public Card EnemyTrigger
        {
            get => _enemyTrigger;
            set => _enemyTrigger = value;
        }

        public List<Card> PlayerDamageZone
        {
            get => _playerDamage;
        }

        public List<Card> EnemyDamageZone
        {
            get => _enemyDamage;
        }

        public void Initialize(List<Card> deck1, List<Card> deck2, Player enemyPlayer)
        {
            _enemyPlayer = enemyPlayer;
            _units[FL.PlayerVanguard] = deck1[0].Clone();
            _units[FL.PlayerVanguard].faceup = false;
            _units[FL.PlayerVanguard].location = Location.Field;
            _units[FL.EnemyVanguard] = deck2[0].Clone();
            _units[FL.EnemyVanguard].faceup = false;
            _units[FL.EnemyVanguard].location = Location.Field;
            for (int i = 1; i < 4; i++)
            {
                _playerRideDeck.Add(deck1[i].Clone());
                _playerRideDeck[_playerRideDeck.Count - 1].location = Location.RideDeck;
                _enemyRideDeck.Add(deck2[i].Clone());
                _enemyRideDeck[_enemyRideDeck.Count - 1].location = Location.RideDeck;
            }
            for (int i = 4; i < 50; i++)
            {
                _playerDeck.Add(deck1[i].Clone());
                _enemyDeck.Add(deck2[i].Clone());
            }
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

        public void sendShuffleKey(List<Card> list)
        {
            int[] key = FisherYates.Shuffle(list);
            _enemyPlayer.GiveShuffleKey(key);
        }

        public void readShuffleKey(List<Card> list)
        {
            Card temp;
            for (int n = list.Count - 1; n > 0; --n)
            {
                temp = list[n];
                list[n] = list[_shuffleKey[n]];
                list[_shuffleKey[n]] = temp;
            }
        }
    }
}
