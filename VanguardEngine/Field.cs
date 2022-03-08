using System;
using System.Collections.Generic;
using System.Text;

namespace VanguardEngine
{
    public class Field
    {
        protected Hand _player1Hand;
        protected Hand _player2Hand;
        protected Deck _player1Deck;
        protected Deck _player2Deck;
        protected List<Card> _tokens = new List<Card>();
        protected Card[] _cardCatalog = new Card[200];
        protected List<int> _removedTokens = new List<int>();
        protected Dictionary<int, Tuple<Zone, int>> _cardLocations = new Dictionary<int, Tuple<Zone, int>>();
        protected Dictionary<int, Tuple<Zone, int>> _previousCardLocations = new Dictionary<int, Tuple<Zone, int>>();
        protected readonly Circle[] _circles = new Circle[14];
        protected int[] _circlePower = new int[14];
        protected int[] _circleCritical = new int[14];
        protected GuardianCircle _GC;
        protected Dictionary<int, List<Card>> _guardians = new Dictionary<int, List<Card>>();
        protected List<Card> _sentinel = new List<Card>();
        protected int _attacker = -1;
        protected List<Card> _attacked = new List<Card>();
        protected int _booster = -1;
        protected bool _player1PersonaRide = false;
        protected bool _player2PersonaRide = false;
        protected Drop _player1Drop;
        protected Drop _player2Drop;
        protected Damage _player1Damage;
        protected Damage _player2Damage;
        protected Bind _player1Bind;
        protected Bind _player2Bind;
        protected TriggerZone _player1Trigger;
        protected TriggerZone _player2Trigger;
        protected Order _player1Order;
        protected Order _player2Order;
        protected OrderArea _player1OrderArea;
        protected OrderArea _player2OrderArea;
        protected RideDeck _player1RideDeck;
        protected RideDeck _player2RideDeck;
        protected PseudoZone _player1Revealed;
        protected PseudoZone _player2Revealed;
        protected PseudoZone _player1Looking;
        protected PseudoZone _player2Looking;
        protected Card _player1Prison = null;
        protected Card _player2Prison = null;
        protected Card _player1World = null;
        protected Card _player2World = null;
        protected PseudoZone _player1Prisoners;
        protected PseudoZone _player2Prisoners;
        protected Zone _removed;
        protected List<Card> _unitsHit = new List<Card>();
        //protected int[] _shuffleKey;
        protected Queue<int> _seedsToBeRead = new Queue<int>();
        protected int _seedToBeSent = -1;
        protected int _turn = 1;
        protected CardStates _cardStates = new CardStates();
        protected PlayerStates _player1States = new PlayerStates();
        protected PlayerStates _player2States = new PlayerStates();
        protected Orientation _orientation = new Orientation();
        protected int _clientNumber = 0;
        public event EventHandler<CardEventArgs> OnZoneChanged;
        public event EventHandler<CardEventArgs> OnZoneSwapped;
        public event EventHandler<CardEventArgs> OnShuffle;
        public Random rand = new Random();

        public Deck Player1Deck
        {
            get => _player1Deck;
            set => _player1Deck = value;
        }

        public Deck Player2Deck
        {
            get => _player2Deck;
            set => _player2Deck = value;
        }

        public Card[] CardCatalog
        {
            get => _cardCatalog;
        }

        public Dictionary<int, Tuple<Zone, int>> CardLocations
        {
            get => _cardLocations;
        }

        public Dictionary<int, Tuple<Zone, int>> PreviousCardLocations
        {
            get => _previousCardLocations;
        }

        //public int[] ShuffleKey
        //{
        //    get => _shuffleKey;
        //    set => _shuffleKey = value;
        //}

        public Queue<int> SeedsToBeRead
        {
            get => _seedsToBeRead;
        }

        public int SeedToBeSent
        {
            get => _seedToBeSent;
            set => _seedToBeSent = value;
        }
        public Zone Player1Hand
        {
            get => _player1Hand;
        }

        public Zone Player2Hand
        {
            get => _player2Hand;
        }

        public Card GetUnit(int circle)
        {
            if (circle < 0 || circle >= FL.MaxFL() || _circles[circle] == null)
                return null;
            return _circles[circle].Index(0);
        }

        public List<Card> GetOverloadedUnits(int circle)
        {
            if (circle < 0 || circle >= FL.MaxFL() || _circles[circle] == null)
                new List<Card>();
            return _circles[circle].OverloadedUnits;
        }

        public void SetUnit(int circle, Card card)
        {
            _circles[circle].Add(card);
        }

        public void RideUnit(int circle, Card card)
        {
            _circles[circle].AddRide(card);
        }

        public void RemoveUnit(int circle, Zone destination)
        {
            destination.Add(GetUnit(circle));
        }

        public void SwapUnits(int circle1, int circle2)
        {
            if (_circles[circle1].GetType() != typeof(RearguardCircle) || _circles[circle2].GetType() != typeof(RearguardCircle))
                return;
            RearguardCircle rc = (RearguardCircle)_circles[circle1];
            rc.Swap((RearguardCircle)_circles[circle2]);
            if (OnZoneSwapped != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.previousLocation = new Tuple<int, int>(Location.RC, circle1);
                args.currentLocation = new Tuple<int, int>(Location.RC, circle2);
                OnZoneSwapped(this, args);
            }
        }

        public void Arm(int circle, Card arm, bool left)
        {
            if (circle != -1)
            {
                _circles[circle].Arm(arm, left);
            }
        }

        public int GetRow(int circle)
        {
            if (circle >= 0 && circle < _circles.Length && _circles[circle] != null)
                return _circles[circle].Row;
            else
                return -1;
        }

        public int GetColumn(int circle)
        {
            if (circle >= 0 && circle < _circles.Length && _circles[circle] != null)
                return _circles[circle].Column;
            return 0;
        }

        public List<Card> GetSoul(int circle)
        {
            return _circles[circle].GetSoul();
        }

        public Soul GetSoulZone(int circle)
        {
            return _circles[circle].GetSoulZone();
        }

        public Card GetArm(bool left, int circle)
        {
            return _circles[circle].GetArm(left);
        }

        public int[] CirclePower
        {
            get => _circlePower;
        }

        public int[] CircleCritical
        {
            get => _circleCritical;
        }

        public Zone Player1RideDeck
        {
            get => _player1RideDeck;
        }

        public Zone Player2RideDeck
        {
            get => _player2RideDeck;
        }

        public Zone GC
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

        public Zone Player1Drop
        {
            get => _player1Drop;
        }

        public Zone Player2Drop
        {
            get => _player2Drop;
        }

        public Zone Player1Bind
        {
            get => _player1Bind;
        }

        public Zone Player2Bind
        {
            get => _player2Bind;
        }

        public Zone Player1Order
        {
            get => _player1Order;
        }

        public Zone Player2Order
        {
            get => _player2Order;
        }

        public Zone Player1OrderArea
        {
            get => _player1OrderArea;
        }

        public Zone Player2OrderArea
        {
            get => _player2OrderArea;
        }

        public PseudoZone Player1Revealed
        {
            get => _player1Revealed;
        }

        public PseudoZone Player2Revealed
        {
            get => _player2Revealed;
        }

        public PseudoZone Player1Looking
        {
            get => _player1Looking;
        }

        public PseudoZone Player2Looking
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

        public Zone Player1Trigger
        {
            get => _player1Trigger;
        }

        public Zone Player2Trigger
        {
            get => _player2Trigger;
        }

        public Zone Player1DamageZone
        {
            get => _player1Damage;
        }

        public Zone Player2DamageZone
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

        public PseudoZone Player1Prisoners
        {
            get => _player1Prisoners;
        }

        public PseudoZone Player2Prisoners
        {
            get => _player2Prisoners;
        }

        public Zone Removed
        {
            get => _removed;
        }

        public List<Card> UnitsHit
        {
            get => _unitsHit;
        }

        public Card Player1World
        {
            get => _player1World;
        }

        public Card Player2World
        {
            get => _player2World;
        }

        public CardStates CardStates
        {
            get => _cardStates;
        }

        public PlayerStates Player1States
        {
            get => _player1States;
        }

        public PlayerStates Player2States
        {
            get => _player2States;
        }

        public Orientation Orientation
        {
            get => _orientation;
        }

        public int Turn
        {
            get => _turn;
        }

        public void IncrementTurn()
        {
            _turn++;
        }

        public void Initialize(List<Card> deck1, List<Card> deck2, List<Card> tokens, int clientNumber)
        {
            List<Card> cards = new List<Card>();
            List<Card> cards2 = new List<Card>();
            _clientNumber = clientNumber;
            _player1Hand = new Hand(this);
            _player2Hand = new Hand(this);
            _player1Deck = new Deck(this);
            _player2Deck = new Deck(this);
            _player1Drop = new Drop(this);
            _player2Drop = new Drop(this);
            _player1Damage = new Damage(this);
            _player2Damage = new Damage(this);
            _player1Bind = new Bind(this);
            _player2Bind = new Bind(this);
            _player1Trigger = new TriggerZone(this);
            _player2Trigger = new TriggerZone(this);
            _player1Order = new Order(this);
            _player2Order = new Order(this);
            _player1OrderArea = new OrderArea(this);
            _player2OrderArea = new OrderArea(this);
            _player1RideDeck = new RideDeck(this);
            _player2RideDeck = new RideDeck(this);
            _player1Revealed = new PseudoZone(this);
            _player2Revealed = new PseudoZone(this);
            _player1Looking = new PseudoZone(this);
            _player2Looking = new PseudoZone(this);
            _player1Prisoners = new PseudoZone(this);
            _player2Prisoners = new PseudoZone(this);
            _removed = new Zone(this);
            _GC = new GuardianCircle(this);
            foreach (Card card in tokens)
                _tokens.Add(card);
            for (int i = 1; i < 14; i++)
            {
                if (i == FL.PlayerFrontLeft)
                    _circles[i] = new RearguardCircle(0, FL.LeftColumn, i, this);
                else if (i == FL.PlayerBackLeft)
                    _circles[i] = new RearguardCircle(1, FL.LeftColumn, i, this);
                else if (i == FL.PlayerVanguard)
                    _circles[i] = new VanguardCircle(0, FL.MiddleColumn, i, this);
                else if (i == FL.PlayerBackCenter)
                    _circles[i] = new RearguardCircle(1, FL.MiddleColumn, i, this);
                else if (i == FL.PlayerFrontRight)
                    _circles[i] = new RearguardCircle(0, FL.RightColumn, i, this);
                else if (i == FL.PlayerBackRight)
                    _circles[i] = new RearguardCircle(1, FL.RightColumn, i, this);
                else if (i == FL.EnemyFrontLeft)
                    _circles[i] = new RearguardCircle(0, FL.RightColumn, i, this);
                else if (i == FL.EnemyBackLeft)
                    _circles[i] = new RearguardCircle(1, FL.RightColumn, i, this);
                else if (i == FL.EnemyVanguard)
                    _circles[i] = new VanguardCircle(0, FL.MiddleColumn, i, this);
                else if (i == FL.EnemyBackCenter)
                    _circles[i] = new RearguardCircle(1, FL.MiddleColumn, i, this);
                else if (i == FL.EnemyFrontRight)
                    _circles[i] = new RearguardCircle(0, FL.LeftColumn, i, this);
                else if (i == FL.EnemyBackRight)
                    _circles[i] = new RearguardCircle(1, FL.LeftColumn, i, this);
                else if (i == 7)
                    _circles[i] = new RearguardCircle(100, 100, i, this);
            }
            cards.Clear();
            cards2.Clear();
            cards.Add(deck1[0].Clone());
            cards[0].originalOwner = 1;
            cards[0].fromRideDeck = true;
            _orientation.SetFaceUp(cards[0].tempID, false);
            _circles[FL.PlayerVanguard].Initialize(cards);
            cards2.Add(deck2[0].Clone());
            cards2[0].originalOwner = 2;
            cards[0].fromRideDeck = true;
            _orientation.SetFaceUp(cards2[0].tempID, false);
            _circles[FL.EnemyVanguard].Initialize(cards2);
            for (int i = 1; i < 4; i++)
            {
                cards.Clear();
                cards.Add(deck1[i].Clone());
                _player1RideDeck.Initialize(cards);
                cards.Clear();
                cards.Add(deck2[i].Clone());
                _player2RideDeck.Initialize(cards);
                cards.Clear();
            }
            cards2.Clear();
            for (int i = 4; i < 50; i++)
            {
                cards.Add(deck1[i].Clone());
                cards2.Add(deck2[i].Clone());
                _player1Deck.Initialize(cards);
                _player2Deck.Initialize(cards2);
                cards.Clear();
                cards2.Clear();
            }
            foreach (Card card in _player1Deck.GetCards())
            {
                _cardCatalog[card.tempID] = card;
                card.originalOwner = 1;
            }
            foreach (Card card in _player1RideDeck.GetCards())
            {
                _cardCatalog[card.tempID] = card;
                card.originalOwner = 1;
                card.fromRideDeck = true;
            }
            foreach (Card card in _player2Deck.GetCards())
            {
                _cardCatalog[card.tempID] = card;
                card.originalOwner = 2;
            }
            foreach (Card card in _player2RideDeck.GetCards())
            {
                _cardCatalog[card.tempID] = card;
                card.originalOwner = 2;
                card.fromRideDeck = true;
            }
            _cardCatalog[_circles[FL.PlayerVanguard].Index(0).tempID] = _circles[FL.PlayerVanguard].Index(0);
            _cardCatalog[_circles[FL.EnemyVanguard].Index(0).tempID] = _circles[FL.EnemyVanguard].Index(0);
        }

        public void ActivateEvent(CardEventArgs args)
        {
            if (OnZoneChanged != null)
            {
                OnZoneChanged(this, args);
            }
        }

        //public void sendShuffleKey(List<Card> list)
        //{
        //    int[] key = FisherYates.Shuffle(list);
        //    _player2Player.GiveShuffleKey(key);
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
            if (_removedTokens.Count > 0)
            {
                int newID = _removedTokens[0];
                _removedTokens.RemoveAt(0);
                token.tempID = newID;
                _cardCatalog[newID] = token;
                _cardLocations[newID] = null;
                _previousCardLocations[newID] = null;
                return newID;
            }
            for (int i = 0; i < _cardCatalog.Length; i++)
            {
                if (_cardCatalog[i] == null)
                {
                    token.tempID = i;
                    _cardCatalog[i] = token;
                    _cardLocations[i] = new Tuple<Zone, int>(new Zone(this), -1);
                    _previousCardLocations[i] = new Tuple<Zone, int>(new Zone(this), -1);
                    return i;
                }
            }
            return -1;
        }

        public void RemoveToken(Card card)
        {
            if (!_removedTokens.Contains(card.tempID))
                _removedTokens.Add(card.tempID);
        }

        public void RemoveFromPseudoZones(Card card)
        {
            _player1Revealed.RemoveCard(card);
            _player2Revealed.RemoveCard(card);
            _player1Looking.RemoveCard(card);
            _player2Looking.RemoveCard(card);
            _player1Prisoners.RemoveCard(card);
            _player2Prisoners.RemoveCard(card);
        }

        public bool ClearOverloadedCards()
        {
            bool retired = false;
            Circle circle;
            for (int i = FL.PlayerFrontLeft; i < FL.PlayerVanguard; i++)
            {
                circle = _circles[i];
                if (circle.OverloadedUnits.Count > 0)
                    retired = true;
                while (circle.OverloadedUnits.Count > 0)
                    _player1Drop.Add(circle.OverloadedUnits[0]);
                while (circle.GetArmZone(true).OverloadedUnits.Count > 0)
                    _player1Drop.Add(circle.GetArmZone(true).OverloadedUnits[0]);
                while (circle.GetArmZone(false).OverloadedUnits.Count > 0)
                    _player1Drop.Add(circle.GetArmZone(false).OverloadedUnits[0]);
            }
            for (int i = FL.EnemyFrontLeft; i < FL.EnemyVanguard; i++)
            {
                circle = _circles[i];
                if (circle.OverloadedUnits.Count > 0)
                    retired = true;
                while (circle.OverloadedUnits.Count > 0)
                    _player2Drop.Add(circle.OverloadedUnits[0]);
                while (circle.GetArmZone(true).OverloadedUnits.Count > 0)
                    _player2Drop.Add(circle.GetArmZone(true).OverloadedUnits[0]);
                while (circle.GetArmZone(false).OverloadedUnits.Count > 0)
                    _player2Drop.Add(circle.GetArmZone(false).OverloadedUnits[0]);
            }
            circle = _circles[FL.PlayerVanguard];
            if (circle.OverloadedUnits.Count > 0)
                circle.GetSoulZone().Add(circle.OverloadedUnits[0]);
            circle = _circles[FL.EnemyVanguard];
            if (circle.OverloadedUnits.Count > 0)
                circle.GetSoulZone().Add(circle.OverloadedUnits[0]);
            return retired;
        }

        public void Shuffle(int playerID)
        {
            Deck deck;
            if (playerID == 1)
                deck = Player1Deck;
            else
                deck = Player2Deck;
            if (_clientNumber != 0)
            {
                if (playerID == _clientNumber)
                {
                    int seed = FisherYates.Shuffle(deck.GetCards());
                    deck.readSeed(seed);
                    _seedToBeSent = seed;
                    if (OnShuffle != null)
                    {
                        CardEventArgs args = new CardEventArgs();
                        args.i = seed;
                        args.playerID = playerID;
                        OnShuffle(this, args);
                    }
                }
                else
                {
                    Log.WriteLine("waiting for seed");
                    while (_seedsToBeRead.Count == 0) ;
                    Log.WriteLine("reading seed");
                    deck.readSeed(_seedsToBeRead.Dequeue());
                    Log.WriteLine("seed read");
                }
            }
            else
            {
                deck.readSeed(FisherYates.Shuffle(deck.GetCards()));
            }
        }

        public void RemoveCard(Card card)
        {
            _removed.Add(card);
        }
    }

    public class Circle : Zone
    {
        protected Soul _soul; //doubles as originalDress for rearguard circles
        protected int _row = 0;
        protected int _column = 0;
        protected int _power = 0;
        protected int _critical = 0;
        protected Arm _leftArm;
        protected Arm _rightArm;

        public Circle(int row, int column, int FL, Field field) : base(field)
        {
            _row = row;
            _column = column;
            _soul = new Soul(field, FL);
            _FL = FL;
            _leftArm = new Arm(field);
            _rightArm = new Arm(field);
        }

        protected override void UpdateLocation(Zone zone, int tempID)
        {
            _field.PreviousCardLocations[tempID] = _field.CardLocations[tempID];
            _field.CardLocations[tempID] = new Tuple<Zone, int>(this, _field.CardLocations[tempID].Item2);
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _field.Orientation.SetFaceUp(card.tempID, true);
            _field.Orientation.SetUpRight(card.tempID, true);
            if (_cards.Count > 0)
            {
                OverloadedUnits.AddRange(_cards);
                _cards.Clear();
            }
            return base.AddToZone(card, bottom);
        }

        public virtual Card AddRide(Card card)
        {
            //_soul.Add(_cards[0]);
            //Log.WriteLine("placed on FL: " + _FL);
            return AddToZone(card, true);
        }

        protected override List<Card> Remove(Card card)
        {
            if (_field.Attacked.Contains(card))
                _field.Attacked.Remove(card);
            return base.Remove(card);
        }

        protected override void ActivateEvent()
        {
            base.ActivateEvent();
        }

        public List<Card> GetSoul()
        {
            return _soul.GetCards();
        }

        public Soul GetSoulZone()
        {
            return _soul;
        }

        public Card GetArm(bool left)
        {
            List<Card> arm;
            if (left)
                arm = _leftArm.GetCards();
            else
                arm = _rightArm.GetCards();
            if (arm.Count > 0)
                return arm[0];
            return null;
        }

        public Arm GetArmZone(bool left)
        {
            if (left)
                return _leftArm;
            return _rightArm;
        }

        public int Row
        {
            get => _row;
        }

        public int Column
        {
            get => _column;
        }

        public int Power
        {
            get => _power;
        }

        public int Critical
        {
            get => _critical;
        }

        public void Arm(Card card, bool left)
        {
            if (left)
                _leftArm.Add(card);
            else
                _rightArm.Add(card);
        }
    }

    public class Arm : Zone
    {
        public Arm(Field field) : base(field)
        {
            location = Location.VC;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            Card returnedCard = base.AddToZone(card, true);
            if (_cards.Count > 1)
            {
                Card overloaded = _cards[0];
                _cards.RemoveAt(0);
                _overloadedCards.Add(overloaded);
            }
            return returnedCard;
        }
    }

    public class RearguardCircle : Circle
    {
        public RearguardCircle(int row, int column, int FL, Field field) : base(row, column, FL, field)
        {
            location = Location.RC;
            _soul = new OriginalDress(field, FL);
        }

        protected override List<Card> AssociatedCards()
        {
            return _soul.GetCards();
        }

        //protected override List<Card> Remove(Card card)
        //{
        //    if (OverloadedUnits.Contains(card))
        //    {
        //        OverloadedUnits.Remove(card);
        //        return new List<Card>();
        //    }
        //    else
        //        return base.Remove(card);
        //}

        protected override Card AddToZone(Card card, bool bottom)
        {
            return base.AddToZone(card, bottom);
        }

        protected override bool RemoveToken(Card card)
        {
            return false;
        }

        public override Card AddRide(Card card)
        {
            base.AddRide(card);
            return card;
        }

        public void Swap(RearguardCircle circle)
        {
            Soul tempSoul = _soul;
            List<Card> tempCards = _cards;
            List<Card> tempOverloaded = _overloadedCards;
            _soul = circle._soul;
            _cards = circle._cards;
            _overloadedCards = circle._overloadedCards;
            circle._soul = tempSoul;
            circle._cards = tempCards;
            circle._overloadedCards = tempOverloaded;
            if (circle._cards.Count > 0)
            {
                _field.PreviousCardLocations[circle._cards[0].tempID] = _field.CardLocations[circle._cards[0].tempID];
                _field.CardLocations[circle._cards[0].tempID] = new Tuple<Zone, int>(circle, _field.CardLocations[circle._cards[0].tempID].Item2);
            }
            if (_cards.Count > 0)
            {
                _field.PreviousCardLocations[_cards[0].tempID] = _field.CardLocations[_cards[0].tempID];
                _field.CardLocations[_cards[0].tempID] = new Tuple<Zone, int>(this, _field.CardLocations[_cards[0].tempID].Item2);
            }
        }
    }

    public class VanguardCircle : Circle
    {
        public VanguardCircle(int row, int column, int FL, Field field) : base(row, column, FL, field)
        {
            location = Location.VC;
        }
    }

    public class Soul : Zone
    {
        public Soul(Field field, int FL) : base(field)
        {
            location = Location.Soul;
            _FL = FL;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            return base.AddToZone(card, bottom);
        }

        protected override void ActivateEvent()
        {
            _field.ActivateEvent(args);
        }
    }

    public class OriginalDress : Soul
    {
        public OriginalDress(Field field, int FL) : base(field, FL)
        {
            location = Location.originalDress;
            _FL = FL;
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
        public const int BackRow = 101;
        public const int FrontRow = 102;
        public const int EnemyCircle = 103;
        public const int LeftColumn = 200;
        public const int MiddleColumn = 201;
        public const int RightColumn = 202;

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

    public class PlayerStates
    {
        List<int> _continuous = new List<int>();
        Dictionary<int, List<int>> _continuousValues = new Dictionary<int, List<int>>();
        Dictionary<int, List<string>> _continuousStrings = new Dictionary<int, List<string>>();
        List<int> _untilEndOfTurn = new List<int>();
        Dictionary<int, List<int>> _untilEndOfTurnValues = new Dictionary<int, List<int>>();
        List<int> _untilEndOfNextTurn = new List<int>();
        List<int> _untilEndOfBattle = new List<int>();
        Dictionary<int, List<int>> _untilEndOfBattleValues = new Dictionary<int, List<int>>();
        List<int> _permanent = new List<int>();

        public void AddContinuousState(int state)
        {
            if (!_continuous.Contains(state))
                _continuous.Add(state);
        }

        public void AddContinuousValue(int state, int value)
        {
            if (!_continuousValues.ContainsKey(state))
                _continuousValues[state] = new List<int>();
            if (!_continuousValues[state].Contains(value))
                _continuousValues[state].Add(value);
        }

        public void AddContinuousString(int state, string value)
        {
            if (!_continuousStrings.ContainsKey(state))
                _continuousStrings[state] = new List<string>();
            _continuousStrings[state].Add(value);
        }

        public void RefreshContinuousStates()
        {
            _continuous.Clear();
            _continuousValues.Clear();
            _continuousStrings.Clear();
        }

        public void AddUntilEndOfTurnState(int state)
        {
            if (!_untilEndOfTurn.Contains(state))
                _untilEndOfTurn.Add(state);
        }

        public void AddUntilEndOfTurnValue(int state, int value)
        {
            if (!_untilEndOfTurnValues.ContainsKey(state))
                _untilEndOfTurnValues[state] = new List<int>();
            _untilEndOfTurnValues[state].Add(value);
        }

        public void IncrementUntilEndOfTurnValue(int state, int value)
        {
            if (!_untilEndOfTurnValues.ContainsKey(state))
            {
                _untilEndOfTurnValues[state] = new List<int>();
                _untilEndOfTurnValues[state].Add(0);
            }
            _untilEndOfTurnValues[state][0] += value;
            if (_untilEndOfTurnValues[state][0] < 0)
                _untilEndOfTurnValues[state][0] = 0;
        }

        public void AddUntilEndOfNextTurnState(int state)
        {
            if (!_untilEndOfNextTurn.Contains(state))
                _untilEndOfNextTurn.Add(state);
        }

        public void EndTurn()
        {
            _untilEndOfTurn.Clear();
            _untilEndOfTurnValues.Clear();
            _untilEndOfTurn.AddRange(_untilEndOfNextTurn);
            _untilEndOfNextTurn.Clear();
        }

        public void AddUntilEndOfBattleState(int state)
        {
            if (!_untilEndOfBattle.Contains(state))
                _untilEndOfBattle.Add(state);
        }

        public void AddUntilEndOfBattleValue(int state, int value)
        {
            if (!_untilEndOfBattleValues.ContainsKey(state))
                _untilEndOfBattleValues[state] = new List<int>();
            _untilEndOfBattleValues[state].Add(value);
        }

        public void EndAttack()
        {
            _untilEndOfBattle.Clear();
            _untilEndOfBattleValues.Clear();
        }

        public void AddPermanentState(int state)
        {
            _permanent.Add(state);
        }

        public bool HasState(int state)
        {
            if (_continuous.Contains(state) || _untilEndOfTurn.Contains(state) || _untilEndOfBattle.Contains(state) || _untilEndOfNextTurn.Contains(state) || _permanent.Contains(state))
                return true;
            if (_continuousValues.ContainsKey(state) || _untilEndOfTurnValues.ContainsKey(state) || _untilEndOfBattleValues.ContainsKey(state))
                return true;
            return false;
        }

        public int GetValue(int state)
        {
            List<int> list = null;
            if (_continuousValues.ContainsKey(state))
                list = _continuousValues[state];
            if (_untilEndOfTurnValues.ContainsKey(state))
                list = _untilEndOfTurnValues[state];
            if (_untilEndOfBattleValues.ContainsKey(state))
                list = _untilEndOfBattleValues[state];
            if (list != null && list.Count > 0)
                return list[list.Count - 1];
            return -1;
        }

        public List<int> GetValues(int state)
        {
            List<int> list = new List<int>();
            if (_continuousValues.ContainsKey(state))
                list = _continuousValues[state];
            if (_untilEndOfTurnValues.ContainsKey(state))
                list = _untilEndOfTurnValues[state];
            if (_untilEndOfBattleValues.ContainsKey(state))
                list = _untilEndOfBattleValues[state];
            return list;
        }

        public List<string> GetStrings(int state)
        {
            if (_continuousStrings.ContainsKey(state))
                return _continuousStrings[state];
            return new List<string>();
        }
    }

    public class CardStates
    {
        //tempId, cardState, abilityID
        Dictionary<int, List<Tuple<int, int>>> _continuous = new Dictionary<int, List<Tuple<int, int>>>();
        //tempID, cardState, value, abilityID
        Dictionary<Tuple<int, int>, List<Tuple<int, int>>> _continuousValues = new Dictionary<Tuple<int, int>, List<Tuple<int, int>>>();
        Dictionary<int, List<int>> _untilEndOfTurn = new Dictionary<int, List<int>>();
        Dictionary<Tuple<int, int>, List<int>> _untilEndOfTurnValues = new Dictionary<Tuple<int, int>, List<int>>();
        Dictionary<int, List<Tuple<int, int>>> _untilEndOfTurnAbilities = new Dictionary<int, List<Tuple<int, int>>>();
        Dictionary<int, List<int>> _untilEndOfNextTurn = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> _untilEndOfBattle = new Dictionary<int, List<int>>();
        Dictionary<Tuple<int, int>, List<int>> _untilEndOfBattleValues = new Dictionary<Tuple<int, int>, List<int>>();

        public void AddContinuousState(int tempID, int state, int abilityID)
        {
            Tuple<int, int> newTuple = new Tuple<int, int>(state, abilityID);
            if (!_continuous.ContainsKey(tempID))
                _continuous[tempID] = new List<Tuple<int, int>>();
            if (!_continuous[tempID].Exists(tuple => tuple == newTuple))
                _continuous[tempID].Add(newTuple);
        }

        public void AddContinuousValue(int tempID, int state, int value, int abilityID)
        {
            Tuple<int, int> newTuple = new Tuple<int, int>(value, abilityID);
            Tuple<int, int> tuple = new Tuple<int, int>(tempID, state);
            if (!_continuousValues.ContainsKey(tuple))
                _continuousValues[tuple] = new List<Tuple<int, int>>();
            if (!_continuousValues[tuple].Exists(existingTuple => existingTuple.Item2 == newTuple.Item2))
            _continuousValues[tuple].Add(newTuple);
        }

        public void RefreshContinuousStates()
        {
            foreach (int key in _continuous.Keys)
                _continuous[key].Clear();
            foreach (Tuple<int, int> key in _continuousValues.Keys)
                _continuousValues[key].Clear();
        }

        public void AddUntilEndOfTurnState(int tempID, int state)
        {
            if (!_untilEndOfTurn.ContainsKey(tempID))
                _untilEndOfTurn[tempID] = new List<int>();
            if (!_untilEndOfTurn[tempID].Contains(state))
                _untilEndOfTurn[tempID].Add(state);
        }

        public void AddUntilEndOfTurnValue(int tempID, int state, int value)
        {
            Tuple<int, int> tuple = new Tuple<int, int>(tempID, state);
            if (!_untilEndOfTurnValues.ContainsKey(tuple))
                _untilEndOfTurnValues[tuple] = new List<int>();
            _untilEndOfTurnValues[tuple].Add(value);
        }

        public void AddUntilEndOfTurnAbility(int tempID, int abilityTempID, int activationNumber)
        {
            Tuple<int, int> tuple = new Tuple<int, int>(abilityTempID, activationNumber);
            if (!_untilEndOfTurnAbilities.ContainsKey(tempID))
                _untilEndOfTurnAbilities[tempID] = new List<Tuple<int, int>>();
            if (!_untilEndOfTurnAbilities[tempID].Contains(tuple))
                _untilEndOfTurnAbilities[tempID].Add(tuple);
        }

        public void AddUntilEndOfNextTurnState(int tempID, int state)
        {
            if (!_untilEndOfNextTurn.ContainsKey(tempID))
                _untilEndOfNextTurn[tempID] = new List<int>();
            if (!_untilEndOfNextTurn[tempID].Contains(state))
                _untilEndOfNextTurn[tempID].Add(state);
        }

        public void AddUntilEndOfBattleState(int tempID, int state)
        {
            if (!_untilEndOfBattle.ContainsKey(tempID))
                _untilEndOfBattle[tempID] = new List<int>();
            if (!_untilEndOfBattle[tempID].Contains(state))
                _untilEndOfBattle[tempID].Add(state);
        }

        public void AddUntilEndOfBattleValue(int tempID, int state, int value)
        {
            Tuple<int, int> tuple = new Tuple<int, int>(tempID, state);
            if (!_untilEndOfBattleValues.ContainsKey(tuple))
                _untilEndOfBattleValues[tuple] = new List<int>();
            _untilEndOfBattleValues[tuple].Add(value);
        }

        public void EndAttack()
        {
            foreach (int key in _untilEndOfBattle.Keys)
                _untilEndOfBattle[key].Clear();
            foreach (Tuple<int, int> key in _untilEndOfBattleValues.Keys)
                _untilEndOfBattleValues[key].Clear();
        }

        public void EndTurn()
        {
            foreach (int key in _untilEndOfTurn.Keys)
                _untilEndOfTurn[key].Clear();
            foreach (Tuple<int, int> key in _untilEndOfTurnValues.Keys)
                _untilEndOfTurnValues[key].Clear();
            foreach (int key in _untilEndOfTurnAbilities.Keys)
                _untilEndOfTurnAbilities[key].Clear();
            foreach (int key in _untilEndOfNextTurn.Keys)
            {
                if (!_untilEndOfTurn.ContainsKey(key))
                    _untilEndOfTurn[key] = new List<int>();
                _untilEndOfTurn[key].AddRange(_untilEndOfNextTurn[key]);
                _untilEndOfNextTurn[key].Clear();
            }
        }

        public bool HasState(int tempID, int state)
        {
            if (_continuous.ContainsKey(tempID) && _continuous[tempID].Exists(tuple => tuple.Item1 == state))
                return true;
            if (_untilEndOfTurn.ContainsKey(tempID) && _untilEndOfTurn[tempID].Contains(state))
                return true;
            if (_untilEndOfNextTurn.ContainsKey(tempID) && _untilEndOfNextTurn[tempID].Contains(state))
                return true;
            if (_untilEndOfBattle.ContainsKey(tempID) && _untilEndOfBattle[tempID].Contains(state))
                return true;
            return false;
        }

        public List<int> GetValues(int tempID, int state)
        {
            Tuple<int, int> tuple = new Tuple<int, int>(tempID, state);
            List<int> values = new List<int>();
            if (_continuousValues.ContainsKey(tuple))
            {
                foreach (Tuple<int, int> value in _continuousValues[tuple])
                    values.Add(value.Item1);
            }
            if (_untilEndOfTurnValues.ContainsKey(tuple))
            {
                values.AddRange(_untilEndOfTurnValues[tuple]);
            }
            if (_untilEndOfBattleValues.ContainsKey(tuple))
            {
                values.AddRange(_untilEndOfBattleValues[tuple]);
            }
            return values;
        }

        public List<Tuple<int, int>> GetAbilities(int tempID)
        {
            if (_untilEndOfTurnAbilities.ContainsKey(tempID))
                return _untilEndOfTurnAbilities[tempID];
            return new List<Tuple<int, int>>();
        }

        public void ResetCardValues(int tempID)
        {
            if (_untilEndOfBattle.ContainsKey(tempID))
                _untilEndOfBattle[tempID].Clear();
            foreach (Tuple<int, int> key in _untilEndOfBattleValues.Keys)
            {
                if (key.Item1 == tempID)
                    _untilEndOfBattleValues[key].Clear();
            }
            if (_untilEndOfNextTurn.ContainsKey(tempID))
                _untilEndOfNextTurn[tempID].Clear();
            if (_untilEndOfTurn.ContainsKey(tempID))
                _untilEndOfTurn[tempID].Clear();
            foreach (Tuple<int, int> key in _untilEndOfTurnValues.Keys)
            {
                if (key.Item1 == tempID)
                    _untilEndOfTurnValues[key].Clear();
            }
            if (_untilEndOfTurnAbilities.ContainsKey(tempID))
                _untilEndOfTurnAbilities[tempID].Clear();
            if (_continuous.ContainsKey(tempID))
                _continuous[tempID].Clear();
            foreach (Tuple<int, int> key in _continuousValues.Keys)
            {
                if (key.Item1 == tempID)
                    _continuousValues[key].Clear();
            }

        }
    }

    public class PlayerState
    {
        public const int FinalRush = 1;
        //public const int DarkNight = 2;
        //public const int AbyssalDarkNight = 3;
        public const int FreeSBAvailable = 4;
        public const int GuardWithTwo = 5;
        public const int CannotGuardFromHand = 6;
        public const int FreeSwap = 7;
        public const int CannotIntercept = 8;
        public const int CannotMove = 9;
        public const int CannotBoost = 10;
        public const int CanAlchemagicDiff = 11;
        public const int CanAlchemagicSame = 12;
        public const int MinGradeForGuard = 13;
        public const int RearguardDriveCheck = 14;
        public const int SoulBlastForRideDeck = 15;
        public const int CannotGuardWithUnitType = 16;
        public const int BlackAndWhiteWingsActive = 17;
        public const int AdditionalOrder = 18;
        public const int VanguardHasSungSongThisTurn = 19;
        public const int RearguardPower10000 = 20;
        public const int PlayerVanguardHitThisTurn = 21;
        public const int RetiredEvenGradeUnitsCanBeAddedToSoul = 22;
        public const int DamageNeededToLose = 23;
        public const int GuardRestrict = 24;
        public const int DoubleTriggerEffects = 25;
        public const int NumOfAttacks = 26;
        public const int CannotCallGradeToGC = 27;
        public const int VanguardAttackHitThisTurn = 28;
        public const int RearguardStoodByEffectThisTurn = 29;
        public const int EnemyRCRetiredThisTurn = 30;
        public const int AdditionalArms = 31;
        public const int CanRideFromRideDeckWithoutDiscard = 32;
        public const int FreeCB = 33;
    }

    public class CardState
    {
        public const int CanAttackFromBackRow = 1;
        public const int CanAttackBackRow = 2;
        public const int CanColumnAttack = 3;
        public const int CanAttackAllFrontRow = 4;
        public const int CannotAttack = 5;
        public const int CannotMove = 6;
        public const int CanInterceptFromBackRow = 7;
        public const int CountsAsTwoRetires = 8;
        public const int BonusPower = 9;
        public const int BonusCritical = 10;
        public const int BonusDrive = 11;
        public const int BonusSkills = 12;
        public const int BonusShield = 13;
        public const int Resist = 14;
        public const int CannotBeHitByGrade = 15;
        public const int CannotBeRidden = 16;
        public const int CannotBeCalledToFrontRow = 17;
        public const int CannotMoveToFrontRow = 18;
        public const int RetireAtEndOfTurn = 19;
        public const int CanOnlyBeCalledToBackRowCenter = 20;
        public const int CannotAttackVanguard = 21;
        public const int CanChooseThreeCirclesWhenAttacking = 22;
        public const int CannotAttackUnit = 23;
        public const int SendToBottomAtEndOfBattle = 24;
        public const int BonusGrade = 25;
        public const int CannotBeAttackedByRearguard = 26;
        public const int GuardWithTwoOnAttack = 27;
        public const int Paralyze = 28;
        public const int DiscardAllOriginalDressAtEndOfBattle = 29;
        public const int CanDriveCheck = 30;
        public const int UniversalPersonaRide = 31;
        public const int CannotIntercept = 32;
        public const int CannotGainIntercept = 33;
        public const int CannotBoost = 34;
        public const int CannotGainBoost = 35;
        public const int CannotBeHit = 36;
        public const int CountsAsTwoMeteorites = 37;
        public const int CanAttackBackRowInSameColumn = 38;
        public const int CannotBeRetiredByCardEffect = 39;
        public const int CannotBeNormalCalled = 40;
        public const int PlacedThisTurn = 41;
        public const int Friend = 42;
        public const int CanAttackGrade3OrGreaterVanguardFromBackRow = 43;
        public const int CannotBeAttacked = 44;
    }

    public class Zone
    {
        protected List<Card> _cards = new List<Card>();
        protected List<Card> _overloadedCards = new List<Card>();
        protected Field _field;
        protected CardEventArgs args = new CardEventArgs();
        protected Zone previousZone;
        protected int location = -1;
        protected int _FL = -1;
        protected bool modified = false;

        public Zone(Field field)
        {
            _field = field;
        }

        public void Initialize(List<Card> cards)
        {
            foreach (Card card in cards)
            {
                _cards.Add(card);
                _field.CardLocations[card.tempID] = new Tuple<Zone, int>(this, _field.rand.Next());
                _field.PreviousCardLocations[card.tempID] = _field.CardLocations[card.tempID];
            }
        }

        protected virtual List<Card> Remove(Card card)
        {
            if (OverloadedUnits.Contains(card))
            {
                OverloadedUnits.Remove(card);
                //return new List<Card>();
            }
            _cards.Remove(card);
            _field.RemoveFromPseudoZones(card);
            return AssociatedCards();
        }

        protected virtual List<Card> AssociatedCards()
        {
            return new List<Card>();
        }

        protected virtual void HandleAssociatedCards(Card card, List<Card> associatedCards)
        {

        }

        protected virtual void ResetCard(Card card)
        {
            _field.CardStates.ResetCardValues(card.tempID);
        }

        protected virtual void UpdateLocation(Zone zone, int tempID)
        {
            _field.PreviousCardLocations[tempID] = _field.CardLocations[tempID];
            _field.CardLocations[tempID] = new Tuple<Zone, int>(this, _field.rand.Next());
        }

        protected virtual Card AddToZone(Card card, bool bottom)
        {
            ResetCard(card);
            if (_field.Booster != -1 && (_field.GetUnit(_field.Booster) == null || _field.GetUnit(_field.Booster).tempID == card.tempID))
                _field.Booster = -1;
            if (_field.Attacker != -1 && (_field.GetUnit(_field.Attacker) == null || _field.GetUnit(_field.Attacker).tempID == card.tempID))
                _field.Attacker = -1;
            if (_field.Attacked.Exists(c => c.tempID == card.tempID))
                _field.Attacked.Remove(_field.Attacked.Find(c => c.tempID == card.tempID));
            previousZone = _field.CardLocations[card.tempID].Item1;
            _field.PreviousCardLocations[card.tempID] = _field.CardLocations[card.tempID];
            List<Card> associatedCards = new List<Card>();
            bool tokenRemoved = false;
            if (previousZone != null)
                associatedCards.AddRange(previousZone.Remove(card));
            if (card.unitType == UnitType.Token)
                tokenRemoved = RemoveToken(card);
            if (!tokenRemoved)
            {
                UpdateLocation(this, card.tempID);
                if (bottom)
                    _cards.Add(card);
                else
                    _cards.Insert(0, card);
                HandleAssociatedCards(card, associatedCards);
                while (associatedCards.Count > 0)
                {
                    AddToZone(associatedCards[0], bottom);
                    associatedCards.RemoveAt(0);
                }
            }
            args = new CardEventArgs();
            if (previousZone == null)
                args.previousLocation = new Tuple<int, int>(-1, -1);
            else
                args.previousLocation = new Tuple<int, int>(previousZone.GetLocation(), previousZone.GetFL());
            if (tokenRemoved)
                args.currentLocation = new Tuple<int, int>(-1, -1);
            else
                args.currentLocation = new Tuple<int, int>(location, _FL);
            args.faceup = _field.Orientation.IsFaceUp(card.tempID);
            args.upright = _field.Orientation.IsUpRight(card.tempID);
            args.card = card;
            ActivateEvent();
            //if (_field.Player1Deck.GetCards().Count == 0 || _field.Player2Deck.GetCards().Count == 0)
            //    throw new ArgumentException("deck out.");
            modified = true;
            return card;
        }

        public virtual Card Add(Card card)
        {
            return AddToZone(card, true);
        }

        public virtual Card AddToTop(Card card)
        {
            return AddToZone(card, false);
        }

        protected virtual bool RemoveToken(Card card)
        {
            _field.RemoveToken(card);
            return true;
        }

        public Card Add(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            return AddToZone(card, true);
        }

        protected virtual void ActivateEvent()
        {
            if (previousZone != this)
                _field.ActivateEvent(args);
        }

        public List<Card> GetCards()
        {
            List<Card> newList = new List<Card>();
            foreach (Card card in _cards)
                newList.Add(card);
            return newList;
        }

        public int Count()
        {
            return _cards.Count;
        }

        public bool Contains(Card card)
        {
            if (_cards.Contains(card))
                return true;
            return false;
        }

        public Card Index(int index)
        {
            if (index < _cards.Count)
                return _cards[index];
            return null;
        }

        public void readSeed(int seed)
        {
            //Card temp;
            //for (int n = _cards.Count - 1; n > 0; --n)
            //{
            //    temp = _cards[n];
            //    _cards[n] = _cards[_shuffleKey[n]];
            //    _cards[_shuffleKey[n]] = temp;
            //}
            FisherYates.Shuffle(_cards, seed);
        }

        //public void Shuffle()
        //{
        //    FisherYates.Shuffle(_cards);
        //}

        public int GetLocation()
        {
            return location;
        }

        public int GetFL()
        {
            return _FL;
        }

        public List<Card> OverloadedUnits
        {
            get => _overloadedCards;
        }

        public void ResetModified()
        {
            modified = false;
        }

        public bool WasModified()
        {
            return modified;
        }
    }

    public class PseudoZone : Zone
    {
        public PseudoZone(Field field) : base(field)
        {

        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _cards.Add(card);
            return card;
        }

        public void RemoveCard(Card card)
        {
            _cards.Remove(card);
        }

        public void Clear()
        {
            _cards.Clear();
        }
    }

    public class Deck : Zone
    {
        public Deck(Field field) : base(field)
        {
            location = Location.Deck;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            if (card.fromRideDeck)
            {
                if (card.originalOwner == 1)
                    return _field.Player1RideDeck.Add(card);
                else
                    return _field.Player2RideDeck.Add(card);
            }
            _field.Orientation.SetFaceUp(card.tempID, false);
            _field.Orientation.SetUpRight(card.tempID, true);
            return base.AddToZone(card, bottom);
        }
    }

    public class RideDeck : Zone
    {
        public RideDeck(Field field) : base(field)
        {
            location = Location.RideDeck;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _field.Orientation.SetFaceUp(card.tempID, false);
            _field.Orientation.SetUpRight(card.tempID, true);
            return base.AddToZone(card, bottom);
        }
    }

    public class Drop : Zone
    {
        public Drop(Field field) : base(field)
        {
            location = Location.Drop;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _field.Orientation.SetFaceUp(card.tempID, true);
            _field.Orientation.SetUpRight(card.tempID, true);
            base.AddToZone(card, bottom);
            return card;
        }
    }

    public class GuardianCircle : Zone
    {
        Dictionary<Card, Soul> _souls = new Dictionary<Card, Soul>();
        Card cardBeingRemoved = null;

        public GuardianCircle(Field field) : base(field)
        {
            location = Location.GC;
        }

        protected override void HandleAssociatedCards(Card card, List<Card> associatedCards)
        {
            if (!_souls.ContainsKey(card))
                _souls[card] = new Soul(_field, _FL);
            foreach (Card c in associatedCards)
            {
                _souls[card].Add(c);
            }
            associatedCards.Clear();
        }

        protected override void UpdateLocation(Zone zone, int tempID)
        {
            _field.PreviousCardLocations[tempID] = _field.CardLocations[tempID];
            _field.CardLocations[tempID] = new Tuple<Zone, int>(this, _field.CardLocations[tempID].Item2);
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _field.Orientation.SetUpRight(card.tempID, false);
            _field.Orientation.SetFaceUp(card.tempID, true);
            return base.AddToZone(card, bottom);
        }

        protected override List<Card> AssociatedCards()
        {
            if (cardBeingRemoved != null && _souls.ContainsKey(cardBeingRemoved))
            {
                return _souls[cardBeingRemoved].GetCards();
            }
            return new List<Card>();
        }

        protected override void ResetCard(Card card)
        {
            
        }

        protected override List<Card> Remove(Card card)
        {
            cardBeingRemoved = card;
            return base.Remove(card);
        }
    }

    public class Bind : Zone
    {
        public Bind(Field field) : base(field)
        {
            location = Location.Bind;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _field.Orientation.SetUpRight(card.tempID, true);
            _field.Orientation.SetFaceUp(card.tempID, true);
            return base.AddToZone(card, bottom);
        }
    }

    public class Hand : Zone
    {
        public Hand(Field field) : base(field)
        {
            location = Location.Hand;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _field.Orientation.SetUpRight(card.tempID, true);
            _field.Orientation.SetFaceUp(card.tempID, false);
            return base.AddToZone(card, bottom);
        }
    }

    public class Damage : Zone
    {
        public Damage(Field field) : base(field)
        {
            location = Location.Damage;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _field.Orientation.SetUpRight(card.tempID, false);
            _field.Orientation.SetFaceUp(card.tempID, true);
            return base.AddToZone(card, bottom);
        }
    }

    public class TriggerZone : Zone
    {
        public TriggerZone(Field field) : base(field)
        {
            location = Location.Trigger;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _field.Orientation.SetUpRight(card.tempID, false);
            _field.Orientation.SetFaceUp(card.tempID, true);
            return base.AddToZone(card, bottom);
        }
    }

    public class Order : Zone
    {
        public Order(Field field) : base(field)
        {
            location = Location.Order;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _field.Orientation.SetUpRight(card.tempID, true);
            _field.Orientation.SetFaceUp(card.tempID, true);
            return base.AddToZone(card, bottom);
        }
    }

    public class OrderArea : Zone
    {
        public OrderArea(Field field) : base(field)
        {
            location = Location.OrderArea;
        }

        protected override Card AddToZone(Card card, bool bottom)
        {
            _field.Orientation.SetUpRight(card.tempID, true);
            _field.Orientation.SetFaceUp(card.tempID, true);
            return base.AddToZone(card, bottom);
        }
    }

    public class Orientation
    {
        bool[] _faceup = new bool[200];
        bool[] _upright = new bool[200];

        public EventHandler<CardEventArgs> FaceUpChanged;
        public EventHandler<CardEventArgs> UpRightChanged;

        public Orientation()
        {
            for (int i = 0; i < 200; i++)
            {
                _faceup[i] = false;
                _upright[i] = true; 
            }
        }

        public void SetFaceUp(int tempID, bool faceup)
        {
            if (tempID >= 0)
            {
                _faceup[tempID] = faceup;
            }
        }

        public void SetUpRight(int tempID, bool upright)
        {
            if (tempID >= 0)
            {
                _upright[tempID] = upright;
            }
        }

        public void Rotate(int tempID, bool upright)
        {
            if (tempID >= 0)
            {
                bool temp = _upright[tempID];
                _upright[tempID] = upright;
                if (temp != upright && UpRightChanged != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.i = tempID;
                    args.upright = upright;
                    UpRightChanged(this, args);
                }
            }
        }

        public void Flip(int tempID, bool faceup)
        {
            if (tempID >= 0)
            {
                bool temp = _faceup[tempID];
                _faceup[tempID] = faceup;
                if (temp != faceup && FaceUpChanged != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.i = tempID;
                    args.faceup = faceup;
                    FaceUpChanged(this, args);
                }
            }
        }

        public bool IsFaceUp(int tempID)
        {
            if (tempID >= 0)
                return _faceup[tempID];
            return false;
        }

        public bool IsUpRight(int tempID)
        {
            if (tempID >= 0)
                return _upright[tempID];
            return false;
        }
    }

    static public class FisherYates
    {
        static Random rand = new Random();
        //  Based on Java code from wikipedia:
        //  http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
        static public int Shuffle(List<Card> list, int seed)
        {
            Random r;
            int newSeed = -1;
            if (seed == -1)
            {
                newSeed = rand.Next();
                r = new Random(newSeed);
            }
            else
                r = new Random(seed);
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
            return newSeed;
        }

        static public int Shuffle(List<Card> list)
        {
            return Shuffle(list, -1);
        }
    }

    public class Snapshot
    {
        public readonly int location;
        public readonly int previousLocation;
        public readonly int circle;
        public readonly string name;
        public readonly int fieldID;
        public readonly int tempID;
        public readonly string cardID;
        public readonly int grade;
        public readonly Snapshot abilitySource;

        public Snapshot(int _tempID, int _location, int _previousLocation, int _circle, string _name, int _fieldID, string _cardID, int _grade)
        {
            tempID = _tempID;
            location = _location;
            previousLocation = _previousLocation;
            circle = _circle;
            name = _name;
            fieldID = _fieldID;
            cardID = _cardID;
            grade = _grade;
        }

        public Snapshot(int _tempID, int _location, int _previousLocation, int _circle, string _name, int _fieldID, string _cardID, int _grade, Snapshot _abilitySource)
        {
            tempID = _tempID;
            location = _location;
            previousLocation = _previousLocation;
            circle = _circle;
            name = _name;
            fieldID = _fieldID;
            cardID = _cardID;
            grade = _grade;
            abilitySource = _abilitySource;
        }
    }

    //public class FieldSnapshot
    //{
    //    Card[] _snapShot = new Card[FL.MaxFL() + 1];
    //    Field _field;

    //    public FieldSnapshot(Field field)
    //    {
    //        _field = field;
    //        for (int i = 0; i < FL.MaxFL(); i++)
    //        {
    //            _snapShot[i] = _field.GetUnit(i);
    //        }
    //    }

    //    public int GetColumn(int tempID)
    //    {
    //        for (int i = 0; i < _snapShot.Length; i++)
    //        {
    //            if (_snapShot[i] != null && _snapShot[i].tempID == tempID)
    //                return _field.GetColumn(i);
    //        }
    //        return -1;
    //    }
    //}
}
