using System;
using System.Collections.Generic;
using System.Text;

namespace VanguardEngine
{
    public class Player
    {
        protected Field _field;
        protected int _damage = 0;
        protected int _startingTurn = -1;
        protected bool _guarding = false;
        protected Dictionary<int, List<Card>> _riddenOnThisTurn = new Dictionary<int, List<Card>>();
        protected Dictionary<int, List<Card>> _lastPlacedOnGC = new Dictionary<int, List<Card>>();
        protected Dictionary<int, List<Card>> _lastPutOnGC = new Dictionary<int, List<Card>>();
        protected Dictionary<int, List<Card>> _lastPlacedOnRC = new Dictionary<int, List<Card>>();
        protected int _lastPlacedOnRC_Count = 1;
        protected Dictionary<int, List<Card>> _lastPlacedOnRCFromHand = new Dictionary<int, List<Card>>();
        protected int _lastPlacedOnRCFromHand_Count = 1;
        protected List<Card> _lastCalledFromPrison = new List<Card>();
        protected Dictionary<int, List<Card>> _lastPlacedOnVC = new Dictionary<int, List<Card>>();
        protected List<Card> _lastRevealedTriggers = new List<Card>();
        protected Card _lastRevealedTrigger = null;
        protected Dictionary<int, List<Card>> _lastDiscarded = new Dictionary<int, List<Card>>();
        protected List<Card> _isIntercepting = new List<Card>();
        protected Card _lastOrderPlayed = null;
        protected Dictionary<int, List<Card>> _stoodByCardEffect = new Dictionary<int, List<Card>>();
        protected List<Card> _canAttackFromBackRow = new List<Card>();
        protected List<Card> _canAttackBackRow = new List<Card>();
        protected List<Card> _canColumnAttack = new List<Card>();
        protected List<Card> _countsAsTwoRetires = new List<Card>();
        protected Dictionary<int, List<int>> _bonusSkills = new Dictionary<int, List<int>>(); //tempID, skills
        protected Dictionary<int, List<Tuple<int, int>>> _givenAbilities = new Dictionary<int, List<Tuple<int, int>>>();
        protected Card _playedOrder;
        protected int _CBUsed = 0;
        protected bool _rearguardDriveCheck = false;
        protected bool _finalRush = false;
        protected bool _darkNight = false;
        protected bool _abyssalDarkNight = false;
        protected bool _enemyRetired = false;
        protected bool _playerRetired = false;
        protected bool _enemyRetiredThisTurn = false;
        protected bool _playerRetiredThisTurn = false;
        protected bool _soulChargedThisTurn = false;
        protected bool _orderPlayed = false;
        protected bool _isAlchemagic = false;
        protected bool _alchemagicFreeSB = false;
        protected int _alchemagicFreeCBAvailable = 0;
        protected bool _alchemagicUsed = false;
        protected bool _guardWithTwo = false;
        protected bool _canGuardFromHand = true;
        protected int _minGradeForGuard = 6;
        protected bool _freeSwap = false;
        protected bool _canIntercept = true;
        protected List<Card> PlayerHand;
        protected List<Card> EnemyHand;
        protected List<Card> PlayerDeck;
        protected List<Card> EnemyDeck;
        protected List<Card> PlayerDrop;
        protected List<Card> EnemyDrop;
        protected List<Card> PlayerDamage;
        protected List<Card> EnemyDamage;
        protected List<Card> PlayerBind;
        protected List<Card> EnemyBind;
        protected Card[] PlayerTrigger;
        protected Card[] EnemyTrigger;
        protected List<Card> PlayerOrder;
        protected List<Card> EnemyOrder;
        protected List<Card> PlayerRideDeck;
        protected List<Card> EnemyRideDeck;
        protected List<Card> PlayerRevealed;
        protected List<Card> EnemyRevealed;
        protected List<Card> PlayerLooking;
        protected Card PlayerPrison;
        protected Card EnemyPrison;
        protected List<Card> PlayerPrisoners;
        protected List<Card> EnemyPrisoners;
        protected Card PlayerWorld = null;
        protected Card EnemyWorld = null;

        int PlayerFrontLeft;
        int PlayerBackLeft;
        int PlayerVanguard;
        int PlayerBackCenter;
        int PlayerFrontRight;
        int PlayerBackRight;
        int EnemyFrontLeft;
        int EnemyBackLeft;
        int EnemyVanguard;
        int EnemyBackCenter;
        int EnemyFrontRight;
        int EnemyBackRight;

        public int _playerID = 0;
        public int _enemyID = 0;

        public event EventHandler<CardEventArgs> OnDraw;
        public event EventHandler<CardEventArgs> OnDiscard;
        public event EventHandler<CardEventArgs> OnReturnCardFromHandToDeck;
        public event EventHandler<CardEventArgs> OnRideFromRideDeck;
        public event EventHandler<CardEventArgs> OnRideFromHand;
        public event EventHandler<CardEventArgs> OnCallFromHand;
        public event EventHandler<CardEventArgs> OnCallFromDeck;
        public event EventHandler<CardEventArgs> OnChangeColumn;
        public event EventHandler<CardEventArgs> OnStandUpVanguard;
        public event EventHandler<CardEventArgs> OnBoost;
        public event EventHandler<CardEventArgs> OnAttack;
        public event EventHandler<CardEventArgs> OnAttackHits;
        public event EventHandler<CardEventArgs> OnGuard;
        public event EventHandler<CardEventArgs> OnDriveCheck;
        public event EventHandler<CardEventArgs> OnDamageCheck;
        public event EventHandler<CardEventArgs> OnRetire;

        public void Initialize(int playerID, Field field)
        {
            _field = field;
            _playerID = playerID;
            if (playerID == 1)
                _enemyID = 2;
            else
                _enemyID = 1;
            if (_playerID == 1)
            {
                _startingTurn = 1;
                PlayerHand = _field.Player1Hand;
                EnemyHand = _field.Player2Hand;
                PlayerDeck = _field.Player1Deck;
                EnemyDeck = _field.Player2Deck;
                PlayerDrop = _field.Player1Drop;
                EnemyDrop = _field.Player2Drop;
                PlayerDamage = _field.Player1DamageZone;
                EnemyDamage = _field.Player2DamageZone;
                PlayerBind = _field.Player1Bind;
                EnemyBind = _field.Player2Bind;
                PlayerTrigger = _field.Player1Trigger;
                EnemyTrigger = _field.Player2Trigger;
                PlayerOrder = _field.Player1Order;
                EnemyOrder = _field.Player2Order;
                PlayerRideDeck = _field.Player1RideDeck;
                EnemyRideDeck = _field.Player2RideDeck;
                PlayerRevealed = _field.Player1Revealed;
                EnemyRevealed = _field.Player2Revealed;
                PlayerLooking = _field.Player1Looking;
                PlayerPrison = _field.Player1Prison;
                EnemyPrison = _field.Player2Prison;
                PlayerPrisoners = _field.Player1Prisoners;
                EnemyPrisoners = _field.Player2Prisoners;
            }
            else
            {
                _startingTurn = 2;
                PlayerHand = _field.Player2Hand;
                EnemyHand = _field.Player1Hand;
                PlayerDeck = _field.Player2Deck;
                EnemyDeck = _field.Player1Deck;
                PlayerDrop = _field.Player2Drop;
                EnemyDrop = _field.Player1Drop;
                PlayerDamage = _field.Player2DamageZone;
                EnemyDamage = _field.Player1DamageZone;
                PlayerBind = _field.Player2Bind;
                EnemyBind = _field.Player1Bind;
                PlayerTrigger = _field.Player2Trigger;
                EnemyTrigger = _field.Player1Trigger;
                PlayerOrder = _field.Player2Order;
                EnemyOrder = _field.Player1Order;
                PlayerRideDeck = _field.Player2RideDeck;
                EnemyRideDeck = _field.Player1RideDeck;
                PlayerRevealed = _field.Player2Revealed;
                EnemyRevealed = _field.Player1Revealed;
                PlayerLooking = _field.Player2Looking;
                PlayerPrison = _field.Player2Prison;
                EnemyPrison = _field.Player1Prison;
                PlayerPrisoners = _field.Player2Prisoners;
                EnemyPrisoners = _field.Player1Prisoners;
            }
            PlayerFrontLeft = Convert(FL.PlayerFrontLeft);
            PlayerBackLeft = Convert(FL.PlayerBackLeft);
            PlayerVanguard = Convert(FL.PlayerVanguard);
            PlayerBackCenter = Convert(FL.PlayerBackCenter);
            PlayerFrontRight = Convert(FL.PlayerFrontRight);
            PlayerBackRight = Convert(FL.PlayerBackRight);
            EnemyFrontLeft = Convert(FL.EnemyFrontLeft);
            EnemyBackLeft = Convert(FL.EnemyBackLeft);
            EnemyVanguard = Convert(FL.EnemyVanguard);
            EnemyBackCenter = Convert(FL.EnemyBackCenter);
            EnemyFrontRight = Convert(FL.EnemyFrontRight);
            EnemyBackRight = Convert(FL.EnemyBackRight);
        }

        public int PlayerDeckCount()
        {
            return PlayerDeck.Count;
        }

        public int EnemyDeckCount()
        {
            return EnemyDeck.Count;
        }

        public int Turn
        {
            get => _field.Turn;
        }

        public List<Card> GetHand()
        {
            return PlayerHand;
        }

        public List<Card> GetOrderableCards()
        {
            List<Card> hand = PlayerHand;
            List<Card> orderableCards = new List<Card>();
            foreach (Card card in hand)
            {
                if (card.orderType >= 0 && card.grade <= _field.GetCircle(PlayerVanguard).grade)
                    orderableCards.Add(card);
            }
            return orderableCards;
        }

        public List<Card> GetAlchemagicTargets()
        {
            List<Card> cards = new List<Card>();
            foreach (Card card in PlayerDrop)
            {
                if (card.orderType == 0 && card.name != _playedOrder.name)
                    cards.Add(card);
            }
            return cards;
        }

        public List<Card> GetSoul()
        {
            return _field.GetCircle(PlayerVanguard).soul;
        }

        public List<Card> GetBackRow()
        {
            List<Card> backRow = new List<Card>();
            if (_field.GetCircle(PlayerBackLeft) != null)
                backRow.Add(_field.GetCircle(PlayerBackLeft));
            if (_field.GetCircle(PlayerBackRight) != null)
                backRow.Add(_field.GetCircle(PlayerBackRight));
            if (_field.GetCircle(PlayerBackCenter) != null)
                backRow.Add(_field.GetCircle(PlayerBackCenter));
            return backRow;
        }

        public List<Card> GetGC()
        {
            return _field.GC;
        }

        public List<Card> GetOrderZone()
        {
            return PlayerOrder;
        }

        public List<Card> GetBind()
        {
            return PlayerBind;
        }

        public List<Card> GetRevealed()
        {
            return PlayerRevealed;
        }

        public List<Card> GetLooking()
        {
            return PlayerLooking;
        }

        public List<Card> GetPlayerOrder()
        {
            return PlayerOrder;
        }

        public Card GetPlayerPrison()
        {
            return PlayerPrison;
        }

        public Card GetEnemyPrison()
        {
            return EnemyPrison;
        }

        public List<Card> GetPlayerPrisoners()
        {
            return PlayerPrisoners;
        }

        public List<Card> GetEnemyPrisoners()
        {
            return EnemyPrisoners;
        }

    
        public List<Card> GetDrop()
        {
            return PlayerDrop;
        }

        public List<Card> GetDeck()
        {
            return PlayerDeck;
        }

        public List<Card> GetRiddenOnThisTurn(int count)
        {
            if (_riddenOnThisTurn.ContainsKey(count))
                return _riddenOnThisTurn[count];
            else
                return new List<Card>();
        }

        public Card GetUnitAt(int circle)
        {
            circle = Convert(circle);
            return _field.GetCircle(circle);
        }

        public int GetCircle(Card card)
        {
            int circle = -1;
            for (circle = 0; circle < FL.MaxFL(); circle++)
            {
                if (_field.GetCircle(circle) != null && _field.GetCircle(circle).tempID == card.tempID)
                    return circle;
            }
            return -1;
        }

        public List<int> GetOpenCircles(bool player)
        {
            List<int> circles = new List<int>();
            if (player)
            {
                for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
                {
                    if (_field.GetCircle(i) == null)
                        circles.Add(i);
                }
            }
            else
            {
                for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                {
                    if (_field.GetCircle(i) == null)
                        circles.Add(i);
                }
            }
            return circles;
        }

        public List<Card> GetUnitsAtColumn(int column)
        {
            List<Card> cards = new List<Card>();
            if (column == 0)
            {
                cards.Add(_field.GetCircle(EnemyBackCenter));
                cards.Add(_field.GetCircle(EnemyVanguard));
                cards.Add(_field.GetCircle(PlayerVanguard));
                cards.Add(_field.GetCircle(PlayerBackCenter));
            }
            else if (column == 1)
            {
                cards.Add(_field.GetCircle(EnemyBackLeft));
                cards.Add(_field.GetCircle(EnemyFrontLeft));
                cards.Add(_field.GetCircle(PlayerFrontRight));
                cards.Add(_field.GetCircle(PlayerBackRight));
            }
            else if (column == -1)
            {
                cards.Add(_field.GetCircle(EnemyBackRight));
                cards.Add(_field.GetCircle(EnemyFrontRight));
                cards.Add(_field.GetCircle(PlayerFrontLeft));
                cards.Add(_field.GetCircle(PlayerBackLeft));
            }
            return cards;
        }

        public int GetColumn(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (GetUnitsAtColumn(1).Contains(card))
                return 1;
            else if (GetUnitsAtColumn(0).Contains(card))
                return 0;
            else if (GetUnitsAtColumn(-1).Contains(card))
                return -1;
            return 0;
        }

        public bool IsInFront(int front, int behind)
        {
            if (_field.GetCircle(PlayerFrontLeft) != null &&
                _field.GetCircle(PlayerFrontLeft).tempID == front &&
                _field.GetCircle(PlayerBackLeft) != null &&
                _field.GetCircle(PlayerBackLeft).tempID == behind)
                return true;
            if (_field.GetCircle(PlayerVanguard) != null &&
                _field.GetCircle(PlayerVanguard).tempID == front &&
                _field.GetCircle(PlayerBackCenter) != null &&
                _field.GetCircle(PlayerBackCenter).tempID == behind)
                return true;
            if (_field.GetCircle(PlayerFrontRight) != null &&
                _field.GetCircle(PlayerFrontRight).tempID == front &&
                _field.GetCircle(PlayerBackRight) != null &&
                _field.GetCircle(PlayerBackRight).tempID == behind)
                return true;
            return false;
        }

        public List<Card> GetInFront(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            List<Card> columnUnits;
            List<Card> inFront = new List<Card>();
            if (GetActiveUnits().Contains(card))
            {
                int column = GetColumn(tempID);
                columnUnits = GetUnitsAtColumn(column);
                foreach (Card c in columnUnits)
                {
                    if (c != null && c.tempID != tempID)
                        inFront.Add(c);
                    else
                        break;
                }
            }
            return inFront;
        }

        public int NumEnemyOpenCircles()
        {
            int count = 0;
            for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                    count++;
            }
            return 5 - count;
        }

        public List<Card> GetRevealedTriggers()
        {
            return _lastRevealedTriggers;
        }

        public Card GetRevealedTrigger()
        {
            return _lastRevealedTrigger;
        }

        public void Draw(int count)
        {
            List<Card> cardsAdded = new List<Card>();
            CardEventArgs args = new CardEventArgs();
            for (int i = 0; i < count; i++)
            {
                PlayerHand.Add(PlayerDeck[0]);
                PlayerDeck[0].location = Location.Hand;
                cardsAdded.Add(PlayerDeck[0]);
                PlayerDeck.Remove(PlayerDeck[0]);
            }
            args.i = count;
            args.cardList = cardsAdded;
            args.playerID = _playerID;
            if (OnDraw != null)
            {
                OnDraw(this, args);
            }
            Console.WriteLine("----------\nDrew " + count + " card(s).");
        }

        public void Mill(int count)
        {
            Card milled;
            for (int i = 0; i < count; i++)
            {
                milled = PlayerDeck[0];
                PlayerDeck.Remove(milled);
                milled.location = Location.Drop;
                PlayerDrop.Insert(0, milled);
            }
        }

        public void MulliganCards(List<int> selection)
        {
            int i = 0;
            int draw = 0;
            foreach (int tempID in selection)
            {
                for (i = 0; i < PlayerHand.Count; i++)
                {
                    if (PlayerHand[i].tempID == tempID)
                    {
                        ReturnCardFromHandToDeck(i);
                        draw++;
                        break;
                    }
                }
            }
            Draw(draw);
            _field.Shuffle(PlayerDeck);
        }

        public void ReturnCardFromHandToDeck(int selection)
        {
            CardEventArgs args = new CardEventArgs();
            Card card;
            card = PlayerHand[selection];
            PlayerHand.RemoveAt(selection);
            card.location = Location.Deck;
            PlayerDeck.Add(card);
            args.card = card;
            args.playerID = _playerID;
            if (OnReturnCardFromHandToDeck != null)
            {
                OnReturnCardFromHandToDeck(this, args);
            }
        }

        public void Discard(List<int> list)
        {
            CardEventArgs args = new CardEventArgs();
            List<Card> toDiscard = new List<Card>();
            _lastDiscarded[_lastDiscarded.Keys.Count + 1] = new List<Card>();
            foreach (int tempID in list)
            {
                foreach (Card card in PlayerHand)
                {
                    if (card.tempID == tempID)
                    {
                        toDiscard.Add(card);
                        _lastDiscarded[_lastDiscarded.Keys.Count].Add(card);
                        break;
                    }
                }
            }
            foreach (Card card in toDiscard)
            {
                PlayerHand.Remove(card);
                PlayerDrop.Insert(0, card);
                card.location = Location.Drop;
            }
            args.cardList = toDiscard;
            args.playerID = _playerID;
            if (OnDiscard != null)
            {
                OnDiscard(this, args);
            }
        }

        public void Shuffle()
        {
            _field.Shuffle(PlayerDeck);
        }

        public void StandAll()
        {
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                    _field.GetCircle(i).upright = true;
            }
        }

        public void StandUpVanguard()
        {
            CardEventArgs args = new CardEventArgs();
            if (OnStandUpVanguard != null)
                OnStandUpVanguard(this, args);
        }

        public int PrintHand()
        {
            List<Card> hand = PlayerHand;
            Console.WriteLine("----------");
            //Console.WriteLine("Choose a card to examine.");
            for (int i = 0; i < hand.Count; i++)
            {
                Console.WriteLine(i + 1 + ". " + hand[i].name);
            }
            //Console.WriteLine(hand.Count + 1 + ". Go back.");
            return hand.Count + 1;
        }

        public int PrintField()
        {
            string output;
            string pstand, estand;
            List<Card> hand = EnemyHand;
            List<Card> GC = _field.GC;
            if (_field.GetCircle(EnemyVanguard).upright)
                estand = "S";
            else
                estand = "R";
            if (_field.GetCircle(PlayerVanguard).upright)
                pstand = "S";
            else
                pstand = "R";
            output = "----------\nEnemy Hand: " + hand.Count + " Enemy Soul: " + _field.GetCircle(EnemyVanguard).soul.Count + " Player Soul: " + _field.GetCircle(PlayerVanguard).soul.Count + " Player Damage: " + PlayerDamage.Count + " Enemy Damage: " + EnemyDamage.Count + "\n" +
                "Choose circle to examine.\n" +
                "1. " + PrintRGData(EnemyBackRight) + " | " + "2. " + PrintRGData(EnemyBackCenter) + " | " + "3. " + PrintRGData(EnemyBackLeft) + "\n" +
                "4. " + PrintRGData(EnemyFrontRight) + " | " + "5. " + CalculatePowerOfUnit(EnemyVanguard) + " G" + _field.GetCircle(EnemyVanguard).grade + " " + estand + " | 6. " + PrintRGData(EnemyFrontLeft) + "\n" +
                "7.                 (to-do)\n" +
                "8. " + PrintRGData(PlayerFrontLeft) + " | 9. " + CalculatePowerOfUnit(PlayerVanguard) + " G" + _field.GetCircle(PlayerVanguard).grade + " " + pstand + " | 10. " + PrintRGData(PlayerFrontRight) + "\n" +
                "11. " + PrintRGData(PlayerBackLeft) + " | 12. " + PrintRGData(PlayerBackCenter) + " | 13. " + PrintRGData(PlayerBackRight) + "\n" +
                "14. Display Drop.\n" +
                "15. Display Soul.\n" +
                "16. Go back.";
            Console.WriteLine(output);
            return 16;
        }

        public string PrintRGData(int location)
        {
            string output;
            if (_field.GetCircle(location) != null)
            {
                output = CalculatePowerOfUnit(location) + " G" + _field.GetCircle(location).grade + " ";
                if (_field.GetCircle(location).upright)
                    output += "S";
                else
                    output += "R";
            }
            else
                output = "--empty--";
            return output;
        }

        public int PrintHandForMulligan(bool[] toMulligan)
        {
            List<Card> hand = PlayerHand;
            int j = 1;
            Console.WriteLine("----------\nChoose card to mulligan.");
            for (int i = 0; i < hand.Count; i++)
            {
                if (!toMulligan[i])
                Console.WriteLine((j++) + ". " + hand[i].name);
            }
            Console.WriteLine(j + ". End mulligan.");
            return j;
        }

        public void DisplayCardInHand(int handNumber)
        {
            List<Card> hand = PlayerHand;
            DisplayCard(hand[handNumber]);
        }

        public void DisplayCard(Card card)
        {
            if (card == null)
            {
                Console.WriteLine("No card.");
                return;
            }
            string output = card.name + "\n" +
                "Grade: " + card.grade + " Power: " + card.power + " Shield: " + card.shield + " " + card.id + "\n" +
                card.effect;
            Console.WriteLine("----------" + output);
        }

        public void DisplayDrop()
        {
            if (PlayerDrop.Count == 0)
            {
                Console.WriteLine("----------No cards in Drop.");
                return;
            }
            Console.WriteLine("----------");
            for (int i = 0; i < PlayerDrop.Count; i++)
            {
                Console.WriteLine(i + 1 + ". " + PlayerDrop[i].name);
            }
        }

        public void DisplaySoul()
        {
            if (_field.GetCircle(PlayerVanguard).soul.Count == 0)
            {
                Console.WriteLine("----------No cards in Soul.");
                return;
            }
            Console.WriteLine("----------");
            for (int i = 0; i < _field.GetCircle(PlayerVanguard).soul.Count; i++)
            {
                Console.WriteLine(i + 1 + ". " + _field.GetCircle(PlayerVanguard).soul[i].name);
            }
        }

        public void DisplayVanguard(bool player)
        {
            if (player)
                DisplayCard(_field.GetCircle(EnemyVanguard));
            else
                DisplayCard(_field.GetCircle(PlayerVanguard));
        }

        public void DisplayRearguard(int selection)
        {
            if (selection == 1)
                DisplayCard(_field.GetCircle(EnemyBackRight));
            else if (selection == 2)
                DisplayCard(_field.GetCircle(EnemyBackCenter));
            else if (selection == 3)
                DisplayCard(_field.GetCircle(EnemyBackLeft));
            else if (selection == 4)
                DisplayCard(_field.GetCircle(EnemyFrontRight));
            else if (selection == 6)
                DisplayCard(_field.GetCircle(EnemyFrontLeft));
            else if (selection == 8)
                DisplayCard(_field.GetCircle(PlayerFrontLeft));
            else if (selection == 10)
                DisplayCard(_field.GetCircle(PlayerFrontRight));
            else if (selection == 11)
                DisplayCard(_field.GetCircle(PlayerBackLeft));
            else if (selection == 12)
                DisplayCard(_field.GetCircle(PlayerBackCenter));
            else if (selection == 13)
                DisplayCard(_field.GetCircle(PlayerBackRight));
        }

        public List<Card> GetRideableCards(bool rideDeck)
        {
            List<Card> cards = new List<Card>();
            if (rideDeck)
            {
                foreach (Card card in PlayerRideDeck)
                {
                    if (card.unitType >= 0 && card.grade == _field.GetCircle(PlayerVanguard).grade || card.grade == _field.GetCircle(PlayerVanguard).grade + 1)
                        cards.Add(card);
                }
            }
            else
            {
                foreach (Card card in PlayerHand)
                {
                    if (card.unitType >= 0 && card.grade == _field.GetCircle(PlayerVanguard).grade || card.grade == _field.GetCircle(PlayerVanguard).grade + 1)
                        cards.Add(card);
                }
            }
            return cards;
        }

        public List<Card> GetCallableRearguards()
        {
            List<Card> hand = PlayerHand;
            List<Card> callableCards = new List<Card>();
            Card VG = _field.GetCircle(PlayerVanguard);
            foreach (Card card in hand)
            {
                if (card.grade <= VG.grade && card.orderType < 0)
                    callableCards.Add(card);
            }
            return callableCards;
        }

        public int GetShield(int tempID)
        {
            return CalculateShield(tempID);
        }

        public bool CheckColumn(int column) //0 is left, 1 is right
        {
            if (column == 0)
            {
                if (_field.GetCircle(PlayerFrontLeft) != null || _field.GetCircle(PlayerBackLeft) != null)
                    return true;
                return false;
            }
            else
            {
                if (_field.GetCircle(PlayerFrontRight) != null || _field.GetCircle(PlayerBackRight) != null)
                    return true;
                return false;
            }
        }

        public int PrintACTableCards()
        {
            return 0;
        }

        public int PrintAvailableOrders()
        {
            return 0;
        }

        public List<Card> GetCardsToAttackWith()
        {
            List<Card> cards = new List<Card>();
            if (_field.GetCircle(PlayerFrontLeft) != null && _field.GetCircle(PlayerFrontLeft).upright)
                cards.Add(_field.GetCircle(PlayerFrontLeft));
            if (_field.GetCircle(PlayerVanguard).upright)
                cards.Add(_field.GetCircle(PlayerVanguard));
            if (_field.GetCircle(PlayerFrontRight) != null && _field.GetCircle(PlayerFrontRight).upright)
                cards.Add(_field.GetCircle(PlayerFrontRight));
            foreach (Card card in _canAttackFromBackRow)
            {
                if (card.upright)
                    cards.Add(card);
            }
            return cards;
        }

        public List<Card> GetAttackableUnits()
        {
            List<Card> cards = new List<Card>();
            if (_canColumnAttack.Contains(_field.GetCircle(_field.Attacker)))
            {
                for (int i = EnemyFrontLeft; i <= EnemyVanguard; i++)
                {
                    if (_field.GetCircle(i) != null)
                        cards.Add(_field.GetCircle(i));
                }
            }
            else
            {
                if (_field.GetCircle(EnemyFrontLeft) != null)
                    cards.Add(_field.GetCircle(EnemyFrontLeft));
                cards.Add(_field.GetCircle(EnemyVanguard));
                if (_field.GetCircle(EnemyFrontRight) != null)
                    cards.Add(_field.GetCircle(EnemyFrontRight));
                if (_canAttackBackRow.Contains(_field.GetCircle(_field.Attacker)))
                {
                    if (_field.GetCircle(EnemyBackRight) != null && !cards.Contains(_field.GetCircle(EnemyBackRight)))
                        cards.Add(_field.GetCircle(EnemyBackRight));
                    if (_field.GetCircle(EnemyBackLeft) != null && !cards.Contains(_field.GetCircle(EnemyBackLeft)))
                        cards.Add(_field.GetCircle(EnemyBackLeft));
                    if (_field.GetCircle(EnemyBackCenter) != null && !cards.Contains(_field.GetCircle(EnemyBackCenter)))
                        cards.Add(_field.GetCircle(EnemyBackCenter));
                }
            }
            return cards;
        }

        public List<Card> GetEnemyFrontRowRearguards()
        {
            List<Card> cards = new List<Card>();
            if (_field.GetCircle(EnemyFrontLeft) != null)
                cards.Add(_field.GetCircle(EnemyFrontLeft));
            if (_field.GetCircle(EnemyFrontRight) != null)
                cards.Add(_field.GetCircle(EnemyFrontRight));
            return cards;
        }

        public List<Card> GetPlayerFrontRow()
        {
            List<Card> cards = new List<Card>();
            if (_field.GetCircle(PlayerFrontLeft) != null)
                cards.Add(_field.GetCircle(PlayerFrontLeft));
            cards.Add(_field.GetCircle(PlayerVanguard));
            if (_field.GetCircle(PlayerFrontRight) != null)
                cards.Add(_field.GetCircle(PlayerFrontRight));
            return cards;
        }

        public List<Card> GetGuardableCards()
        {
            List<Card> cards = new List<Card>();
            if (_canGuardFromHand)
            {
                foreach (Card card in PlayerHand)
                {
                    if (card.orderType < 0 && card.grade <= _field.GetCircle(PlayerVanguard).grade && card.grade <= _minGradeForGuard)
                        cards.Add(card);
                }
            }
            for (int i = 1; i < FL.PlayerVanguard; i++)
            {
                if (CanIntercept(_field.GetCircle(i)))
                    cards.Add(_field.GetCircle(i));
            }
            return cards;
        }

        public bool CanIntercept(Card card)
        {
            if (_canIntercept && card != null && !_field.Attacked.Contains(card) && card.skill == 1 && (_field.GetCircle(PlayerFrontLeft) == card || _field.GetCircle(PlayerFrontRight) == card))
                return true;
            return false;
        }

        public void DisableIntercept()
        {
            _canIntercept = false;
        }

        public List<Card> GetActiveUnits()
        {
            List<Card> cards = new List<Card>();
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                    cards.Add(_field.GetCircle(i));
            }
            return cards;
        }

        public List<Card> GetAllUnitsOnField()
        {
            List<Card> cards = new List<Card>();
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                    cards.Add(_field.GetCircle(i));
            }
            for (int i = EnemyFrontLeft; i <= EnemyVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                    cards.Add(_field.GetCircle(i));
            }
            return cards;
        }

        public List<Card> GetRearguards(bool player)
        {
            List<Card> rearguards = new List<Card>();
            if (player)
            {
                for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
                {
                    if (_field.GetCircle(i) != null)
                        rearguards.Add(_field.GetCircle(i));
                }
            }
            else
            {
                for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                {
                    if (_field.GetCircle(i) != null)
                        rearguards.Add(_field.GetCircle(i));
                }
            }
            return rearguards;
        }

        public bool IsPlayerTurn()
        {
            if (_startingTurn == 1 && _field.Turn % 2 != 0)
                return true;
            if (_startingTurn == 2 && _field.Turn % 2 == 0)
                return true;
            return false;
        }

        public List<Card> GetDamageZone()
        {
            return PlayerDamage;
        }

        public bool PersonaRode()
        {
            return _field.GetPersonaRide(_playerID);
        }

        public int CalculateShield(int tempID)
        {
            List<Card> guardians;
            Card attacked = _field.CardCatalog[tempID];
            if (!_field.Attacked.Contains(attacked))
                return 0;
            int shield = 0;
            if (_field.Sentinel.Contains(attacked) || attacked.hitImmunity.Contains(_field.GetCircle(_field.Attacker).grade))
                return 1000000000;
            if (!_field.Guardians.ContainsKey(tempID))
                return 0;
            guardians = _field.Guardians[tempID];
            if (guardians == null)
                return 0;
            foreach (Card card in guardians)
            {
                if (_field.GC.Contains(card))
                {
                    shield += card.shield + card.tempShield;
                    foreach (Tuple<int, int> key in card.abilityShield.Keys)
                        shield += card.abilityShield[key];
                }
            }
            return shield;
        }

        public void PrintShield()
        {
            foreach (Card card in _field.Attacked)
            {
                if (_field.Sentinel.Contains(_field.CardCatalog[card.tempID]) || card.hitImmunity.Contains(_field.GetCircle(_field.Attacker).grade))
                {
                    Console.WriteLine("[" + card.name + "] Hit immunity active.");
                    return;
                }
                Console.WriteLine("[" + card.name + "] Shield: " + (CalculateShield(card.tempID) + CalculatePowerOfUnit(GetCircle(card))));
            }
        }

        public void PrintAttack()
        {
            Console.WriteLine("Attacking for " + CalculatePowerOfUnit(_field.Attacker) + ".");
        }

        public void PrintEnemyAttack()
        {
            Console.WriteLine("Enemy attacking for " + CalculatePowerOfUnit(_field.Attacker) + ".");
        }

        public int CalculatePowerOfUnit(int location)
        {
            int power = _field.GetCircle(location).power + _field.GetCircle(location).bonusPower + _field.GetCircle(location).tempPower + _field.GetCircle(location).battleOnlyPower + _field.CirclePower[location];
            if (IsFrontRow(location) && IsPlayer(location) && _field.GetPersonaRide(_playerID))
                power += 10000;
            else if (IsFrontRow(location) && !IsPlayer(location) && _field.GetPersonaRide(_enemyID))
                power += 10000;
            foreach (Tuple<int, int> key in _field.GetCircle(location).abilityPower.Keys)
                power += _field.GetCircle(location).abilityPower[key];
            if (_field.Booster >= 0 && location == _field.Attacker)
            {
                if (_field.GetCircle(_field.Booster) == null)
                    _field.Booster = -1;
                else
                    power += CalculatePowerOfUnit(_field.Booster);
            }
            return power;
        }

        public int PrintActiveUnits()
        {
            int max = 0;
            //PrintAttack();
            string output;
            if (_field.GetCircle(PlayerFrontLeft) != null)
            {
                max++;
                output = max + ". FrontLeft " + _field.GetCircle(PlayerFrontLeft).name + " ";
                if (!_guarding && _field.Booster == 0)
                    output += (_field.GetCircle(PlayerFrontLeft).power + _field.GetCircle(PlayerFrontLeft).bonusPower + _field.GetCircle(PlayerBackLeft).power + _field.GetCircle(PlayerBackLeft).bonusPower);
                else
                    output += (_field.GetCircle(PlayerFrontLeft).power + _field.GetCircle(PlayerFrontLeft).bonusPower);
                Console.WriteLine(output);
            }
            max++;
            output = max + ". FrontCenter " + _field.GetCircle(PlayerVanguard).name + " ";
            if (!_guarding && _field.Booster == 1)
                output += (_field.GetCircle(PlayerVanguard).power + _field.GetCircle(PlayerVanguard).bonusPower + _field.GetCircle(PlayerBackCenter).power + _field.GetCircle(PlayerBackCenter).bonusPower);
            else
                output += (_field.GetCircle(PlayerVanguard).power + _field.GetCircle(PlayerVanguard).bonusPower);
            Console.WriteLine(output);
            if (_field.GetCircle(PlayerFrontRight) != null)
            {
                max++;
                output = max + ". FrontCenter " + _field.GetCircle(PlayerFrontRight).name + " ";
                if (!_guarding && _field.Booster == 2)
                    output += (_field.GetCircle(PlayerFrontRight).power + _field.GetCircle(PlayerFrontRight).bonusPower + _field.GetCircle(PlayerBackRight).power + _field.GetCircle(PlayerBackRight).bonusPower);
                else
                    output += (_field.GetCircle(PlayerBackRight).power + _field.GetCircle(PlayerBackRight).bonusPower);
                Console.WriteLine(output);
            }
            return max;
        }

        public int PrintDamageZone()
        {
            int i = 0;
            Console.WriteLine("Choose damage to heal.");
            string output;
            for (i = 0; i < PlayerDamage.Count; i++)
            {
                output = i + 1 + ". " + PlayerDamage[i].name + " ";
                if (PlayerDamage[i].faceup == false)
                    Console.WriteLine(output + "(facedown).");
                else
                    Console.WriteLine(output + "(faceup).");
            }
            return i + 1;
        }

        public bool CanRideFromRideDeck()
        {
            List<Card> rideDeck = PlayerRideDeck;
            Card VG = _field.GetCircle(PlayerVanguard);
            foreach (Card card in rideDeck)
            {
                if (VG.grade + 1 == card.grade)
                    return true;
            }
            return false;
        }

        public bool CanRideFromHand()
        {
            List<Card> hand = PlayerHand;
            Card VG = _field.GetCircle(PlayerVanguard);
            foreach (Card card in hand)
            {
                if (card.unitType >= 0 && (card.grade == VG.grade || card.grade - 1 == VG.grade))
                    return true;
            }
            return false;
        }

        public bool CanCallRearguard()
        {
            List<Card> hand = PlayerHand;
            Card VG = _field.GetCircle(PlayerVanguard);
            foreach (Card card in hand)
            {
                if (card.grade <= VG.grade && card.orderType < 0)
                    return true;
            }
            return false;
        }

        public bool CanMoveRearguard()
        {
            for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                    return true;
            }
            return false;
        }

        public bool CanACT()
        {
            return false;
        }

        public bool CanActivateOrder()
        {
            return false;
        }

        public bool CanAttack()
        {
            if (_field.GetCircle(PlayerVanguard).upright || (_field.GetCircle(PlayerFrontLeft) != null && _field.GetCircle(PlayerFrontLeft).upright) || (_field.GetCircle(PlayerFrontRight) != null && _field.GetCircle(PlayerFrontRight).upright))
                return true;
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (i == PlayerFrontLeft || i == PlayerFrontRight || i == PlayerVanguard)
                {
                    if (_field.GetCircle(i) != null && _field.GetCircle(i).upright)
                        return true;
                }
                else if (_canAttackFromBackRow.Contains(_field.GetCircle(i)))
                {
                    if (_field.GetCircle(i) != null && _field.GetCircle(i).upright)
                        return true;
                }
            }
            return false;
        }

        public bool CanBeBoosted()
        {
            if (_field.Attacker == PlayerFrontLeft)
            {
                if (_field.GetCircle(PlayerBackLeft) != null && CanBoost(_field.GetCircle(PlayerBackLeft)))
                    return true;
            }
            else if (_field.Attacker == PlayerVanguard)
            {
                if (_field.GetCircle(PlayerBackCenter) != null && CanBoost(_field.GetCircle(PlayerBackCenter)))
                    return true;
            }
            else if (_field.Attacker == PlayerFrontRight)
            {
                if (_field.GetCircle(PlayerBackRight) != null && CanBoost(_field.GetCircle(PlayerBackRight)))
                    return true;
            }
            return false;
        }

        public bool CanBoost(Card card)
        {
            if (card.upright)
            {
                if (card.skill == 0 || (_bonusSkills.ContainsKey(card.tempID) && _bonusSkills[card.tempID].Contains(0)))
                    return true;
            }
            return false;
        }

        public bool CanGuard()
        {
            if (GetGuardableCards().Count > 0)
                return true;
            return false;
        }

        public bool CanHeal()
        {
            if (PlayerDamage.Count > 0 && PlayerDamage.Count >= EnemyDamage.Count)
                return true;
            return false;
        }

        public bool TargetIsVanguard(bool player)
        {
            int Vanguard;
            if (player)
                Vanguard = PlayerVanguard;
            else
                Vanguard = EnemyVanguard;
            if (_field.Attacked.Contains(_field.GetCircle(Vanguard)))
                return true;
            return false;
        }

        public bool TargetIsRearguard(bool player)
        {
            if (player)
            {
                for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
                {
                    if (_field.Attacked.Contains(_field.GetCircle(i)))
                        return true;
                }
            }
            else
            {
                for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                {
                    if (_field.Attacked.Contains(_field.GetCircle(i)))
                        return true;
                }
            }
            return false;
        }

        public bool VanguardHit(bool player)
        {
            if (player && _field.UnitsHit.Contains(_field.GetCircle(PlayerVanguard)))
                return true;
            if (!player && _field.UnitsHit.Contains(_field.GetCircle(EnemyVanguard)))
                return true;
            return false;
        }

        public bool IsRearguard(int tempID)
        {
            for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == tempID)
                    return true;
            }
            for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
            {
                if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == tempID)
                    return true;
            }
            return false;
        }

        public bool IsLastPlacedOnGC(int tempID, int count)
        {
            if (!_lastPlacedOnGC.ContainsKey(count))
                return false;
            foreach (Card card in _lastPlacedOnGC[count])
            {
                if (card.tempID == tempID)
                    return true;
            }
            return false;
        }

        public bool IsLastPutOnGC(int tempID, int count)
        {
            if (!_lastPutOnGC.ContainsKey(count))
                return false;
            foreach (Card card in _lastPutOnGC[count])
            {
                if (card.tempID == tempID)
                    return true;
            }
            return false;
        }


        public bool IsLastPlacedOnRC(int tempID, int count)
        {
            if (!_lastPlacedOnRC.ContainsKey(count))
                return false;
            foreach (Card card in _lastPlacedOnRC[count])
            {
                if (card.tempID == tempID && card.location == Location.RC)
                    return true;
            }
            return false;
        }

        public bool IsLastPlacedOnRCFromHand(int tempID, int count)
        {
            if (!_lastPlacedOnRCFromHand.ContainsKey(count))
                return false;
            foreach (Card card in _lastPlacedOnRCFromHand[count])
            {
                if (card.tempID == tempID && card.location == Location.RC)
                    return true;
            }
            return false;
        }

        public List<Card> GetLastPlacedOnRC(int count)
        {
            if (!_lastPlacedOnRC.ContainsKey(count))
                return new List<Card>();
            return _lastPlacedOnRC[count];
        }

        public List<Card> GetLastPlacedOnVC(int count)
        {
            if (!_lastPlacedOnVC.ContainsKey(count))
                return new List<Card>();
            return _lastPlacedOnVC[count];
        }

        public List<Card> GetLastStood(int count)
        {
            if (!_stoodByCardEffect.ContainsKey(count))
                return new List<Card>();
            return _stoodByCardEffect[count];
        }

        public bool IsLastPlacedOnVC(int tempID, int count)
        {
            if (!_lastPlacedOnVC.ContainsKey(count))
                return false;
            foreach (Card card in _lastPlacedOnVC[count])
            {
                if (card.tempID == tempID)
                    return true;
            }
            return false;
        }

        public bool IsLastDiscarded(int tempID, int count)
        {
            if (!_lastDiscarded.ContainsKey(count))
                return false;
            Card card = _field.CardCatalog[tempID];
            if (_lastDiscarded[count].Contains(card) && PlayerDrop.Contains(card))
                return true;
            return false;
        }

        public int Drive()
        {
            Card attacker = _field.GetCircle(_field.Attacker);
            int drive = attacker.tempDrive;
            foreach (Tuple<int, int> key in attacker.abilityDrive.Keys)
            {
                drive += _field.GetCircle(_field.Attacker).abilityDrive[key];
            }
            if (attacker.skill == Skill.TripleDrive || (_bonusSkills.ContainsKey(attacker.tempID) && _bonusSkills[attacker.tempID].Contains(Skill.TripleDrive)))
                drive += 3;
            else if (attacker.skill == Skill.TwinDrive || (_bonusSkills.ContainsKey(attacker.tempID) && _bonusSkills[attacker.tempID].Contains(Skill.TwinDrive)))
                drive += 2;
            else
                drive += 1;
            return drive;
        }

        public int Critical()
        {
            int critical = _field.GetCircle(_field.Attacker).critical + _field.GetCircle(_field.Attacker).tempCritical + _field.CircleCritical[_field.Attacker] + _field.GetCircle(_field.Attacker).battleOnlyCritical;
            foreach (Tuple<int, int> key in _field.GetCircle(_field.Attacker).abilityCritical.Keys)
            {
                critical += _field.GetCircle(_field.Attacker).abilityCritical[key];
            }
            return critical;
        }

        public int Critical(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            int critical = card.critical + card.tempCritical + _field.CircleCritical[GetCircle(card)] + card.battleOnlyCritical;
            foreach (Tuple<int, int> key in card.abilityCritical.Keys)
            {
                critical += card.abilityCritical[key];
            }
            return critical;
        }

        public int Damage()
        {
            return PlayerDamage.Count; 
        }

        public Card GetTrigger(bool player)
        {
            if (player)
                return PlayerTrigger[0];
            else
                return EnemyTrigger[0];
        }

        public bool StillAttacking()
        {
            if (_field.GetCircle(_field.Attacker) != null)
                return true;
            return false;
        }

        public bool AttackHits()
        {
            Card attacker = _field.GetCircle(_field.Attacker);
            CardEventArgs args = new CardEventArgs();
            foreach (Card card in _field.Attacked)
            {
                if (!GetAllUnitsOnField().Contains(card))
                    return false;
                if (CalculatePowerOfUnit(_field.Attacker) >= CalculateShield(card.tempID) + CalculatePowerOfUnit(GetCircle(card)))
                {
                    _field.UnitsHit.Add(card);
                    Console.WriteLine("----------\n" + attacker.name + "'s attack makes a hit on " + card.name + "!");
                    if (OnAttackHits != null)
                    {
                        args.b = true;
                        OnAttackHits(this, args);
                    }
                }
                else
                {
                    Console.WriteLine("----------\n" + attacker.name + "'s attack against " + card.name + " was successfully guarded!");
                    if (OnAttackHits != null)
                    {
                        args.b = false;
                        OnAttackHits(this, args);
                    }
                }
            }
            if (_field.UnitsHit.Count > 0)
                return true;
            return false;
        }

        public int NumberOfTimesHit()
        {
            return _field.UnitsHit.Count;
        }

        public bool AttackerIsVanguard()
        {
            if (_field.Attacker == PlayerVanguard)
                return true;
            else
                return false;
        }

        public void Ride(int location, int selection)
        {
            CardEventArgs args = new CardEventArgs();
            List<Card> list;
            Card card = null;
            _riddenOnThisTurn[_riddenOnThisTurn.Keys.Count + 1] = new List<Card>();
            _riddenOnThisTurn[_riddenOnThisTurn.Keys.Count].Add(_field.GetCircle(PlayerVanguard));
            _lastPlacedOnVC[_lastPlacedOnVC.Keys.Count + 1] = new List<Card>();
            if (location == 0)
                list = PlayerRideDeck;
            else
                list = PlayerHand;
            foreach (Card item in list)
            {
                if (item.tempID == selection)
                {
                    card = item;
                    break;
                }
            }
            if (card.name == _field.GetCircle(PlayerVanguard).name && _field.GetCircle(PlayerVanguard).personaRide == 1)
            {
                _field.SetPersonaRide(true, _playerID);
                Draw(1);
            }
            list.Remove(card);
            card.faceup = true;
            card.soul.AddRange(_field.GetCircle(PlayerVanguard).soul);
            card.soul.Insert(0, _field.GetCircle(PlayerVanguard));
            _field.GetCircle(PlayerVanguard).location = Location.Soul;
            _field.SetCircle(PlayerVanguard, card);
            card.location = Location.VC;
            card.soul[0].soul.Clear();
            ResetCardValues(card.soul[0]);
            if (_field.GetPersonaRide(_playerID))
                Console.WriteLine("---------\nPersona Ride!! " + _field.GetCircle(PlayerVanguard).name + "!");
            else
                Console.WriteLine("---------\nRide!! " + _field.GetCircle(PlayerVanguard).name + "!");
            _lastPlacedOnVC[_lastPlacedOnVC.Count].Add(card);
            args.card = card;
            args.playerID = _playerID;
            if (location == 0)
            {
                if (OnRideFromRideDeck != null)
                {
                    Console.WriteLine("ridedeck event fired");
                    OnRideFromRideDeck(this, args);
                }
            }
            else
            {
                if (OnRideFromHand != null)
                {
                    OnRideFromHand(this, args);
                }
            }
        }

        public void Call(int location, int selection, bool overDress)
        {
            List<Card> hand;
            Card card = null;
            Card toBeRetired = null;
            List<Card> retired = new List<Card>();
            CardEventArgs args;
            hand = PlayerHand;
            //location = Convert(location);
            foreach (Card item in hand)
            {
                if (item.tempID == selection)
                {
                    card = item;
                    break;
                }
            }
            if (!overDress && _field.GetCircle(location) != null)
            {
                _playerRetiredThisTurn = true;
                toBeRetired = _field.GetCircle(location);
                retired.Add(toBeRetired);
                foreach (Card soul in _field.GetCircle(location).soul)
                {
                    PlayerDrop.Insert(0, soul);
                    soul.location = Location.Drop;
                    soul.overDress = false;
                    retired.Add(soul);
                }
                ResetCardValues(toBeRetired);
                toBeRetired.upright = true;
                toBeRetired.overDress = false;
                _field.GetCircle(location).soul.Clear();
                retired.Add(_field.GetCircle(location));
                if (_field.GetCircle(location).unitType == UnitType.Token)
                    _field.SetCircle(location, null);
                else
                {
                    PlayerDrop.Insert(0, _field.GetCircle(location));
                    _field.GetCircle(location).location = Location.Drop;
                }
                if (OnRetire != null)
                {
                    args = new CardEventArgs();
                    args.playerID = _playerID;
                    args.cardList = retired;
                    OnRetire(this, args);
                }
            }
            hand.Remove(card);
            if (overDress)
            {
                card.soul.Add(_field.GetCircle(location));
                card.soul.AddRange(_field.GetCircle(location).soul);
                _field.GetCircle(location).soul.Clear();
                foreach (Card c in card.soul)
                {
                    c.location = Location.originalDress;
                    c.overDress = false;
                }
                ResetCardValues(_field.GetCircle(location));
                _field.SetCircle(location, card);
                card.location = Location.RC;
                card.overDress = true;
                Console.WriteLine("---------\nOverDress! " + card.name + "!");
                if (OnCallFromHand != null)
                {
                    args = new CardEventArgs();
                    args.card = card;
                    args.i = location;
                    args.playerID = _playerID;
                    args.b = true;
                    OnCallFromHand(this, args);
                }
            }
            else
            {
                _field.SetCircle(location, card);
                card.location = Location.RC;
                Console.WriteLine("---------\nCall! " + card.name + "!");
                if (OnCallFromHand != null)
                {
                    args = new CardEventArgs();
                    args.card = card;
                    args.i = location;
                    args.playerID = _playerID;
                    args.b = false;
                    OnCallFromHand(this, args);
                }
            }
            card.upright = true;
            _lastPlacedOnRC[_lastPlacedOnRC_Count] = new List<Card>();
            _lastPlacedOnRC[_lastPlacedOnRC_Count].Add(card);
            _lastPlacedOnRC_Count++;
            _lastPlacedOnRCFromHand[_lastPlacedOnRCFromHand_Count] = new List<Card>();
            _lastPlacedOnRCFromHand[_lastPlacedOnRCFromHand_Count].Add(card);
            _lastPlacedOnRCFromHand_Count++;
        }

        public int SuperiorCall(int circle, int tempID, bool overDress, bool standing)
        {
            bool fromHand = false;
            Card ToBeCalled = _field.CardCatalog[tempID];
            int loc = ToBeCalled.location;
            List<Card> location = null;
            //circle = Convert(circle);
            if (loc == Location.Drop)
                location = PlayerDrop;
            else if (loc == Location.Deck)
            {
                location = PlayerDeck;
                _field.Shuffle(PlayerDeck);
            }
            else if (loc == Location.Hand)
            {
                location = PlayerHand;
                fromHand = true;
            }
            else if (loc == Location.Soul)
                location = _field.GetCircle(PlayerVanguard).soul;
            else if (loc == Location.Prison)
            {
                location = EnemyPrisoners;
                _lastCalledFromPrison.Add(ToBeCalled);
            }
            if (PlayerLooking.Contains(ToBeCalled))
                PlayerLooking.Remove(ToBeCalled);
            if (PlayerRevealed.Contains(ToBeCalled))
                PlayerRevealed.Remove(ToBeCalled);
            location.Remove(ToBeCalled);
            if (location == EnemyPrisoners)
                EnemyOrder.Remove(ToBeCalled);
            if (_field.GetCircle(circle) != null)
            {
                _playerRetiredThisTurn = true;
                if (_field.GetCircle(circle).unitType == UnitType.Token)
                    _field.SetCircle(circle, null);
                else
                {
                    _field.GetCircle(circle).bonusPower = 0;
                    _field.GetCircle(circle).upright = true;
                    _field.GetCircle(circle).overDress = false;
                    foreach (Card soul in _field.GetCircle(circle).soul)
                    {
                        PlayerDrop.Insert(0, soul);
                        soul.location = Location.Drop;
                    }
                    _field.GetCircle(circle).soul.Clear();
                    PlayerDrop.Insert(0, _field.GetCircle(circle));
                }
            }
            if (overDress)
            {
                ToBeCalled.soul.Add(_field.GetCircle(circle));
                ResetCardValues(_field.GetCircle(circle));
                _field.GetCircle(circle).location = Location.originalDress;
                foreach (Card originalDress in _field.GetCircle(circle).soul)
                {
                    ToBeCalled.soul.Add(originalDress);
                    originalDress.location = Location.originalDress;
                }
                _field.GetCircle(circle).soul.Clear();
                ToBeCalled.overDress = true;
            }
            ToBeCalled.faceup = true;
            if (standing)
                ToBeCalled.upright = true;
            else
                ToBeCalled.upright = false;
            ToBeCalled.location = Location.RC;
            _field.SetCircle(circle, ToBeCalled);
            if (overDress)
                Console.WriteLine("----------\nSuperior overDress! " + ToBeCalled.name + "!");
            else
                Console.WriteLine("----------\nSuperior Call! " + ToBeCalled.name + "!");
            if (ToBeCalled.orderType >= 0)
            {
                ResetCardValues(ToBeCalled);
                ToBeCalled.location = Location.Drop;
                PlayerDrop.Add(ToBeCalled);
                _field.SetCircle(circle, null);
            }
            if (!_lastPlacedOnRC.ContainsKey(_lastPlacedOnRC_Count))
                _lastPlacedOnRC[_lastPlacedOnRC_Count] = new List<Card>();
            _lastPlacedOnRC[_lastPlacedOnRC_Count].Add(ToBeCalled);
            if (fromHand)
            {
                if (!_lastPlacedOnRCFromHand.ContainsKey(_lastPlacedOnRCFromHand_Count))
                    _lastPlacedOnRCFromHand[_lastPlacedOnRCFromHand_Count] = new List<Card>();
                return 1;
            }
            return 0;
        }

        public void DoneSuperiorCalling()
        {
            _lastPlacedOnRC_Count++;
            _lastPlacedOnRCFromHand_Count++;
        }

        public void MoveRearguard(int location)
        {
            Card temp;
            if (location == 0)
            {
                temp = _field.GetCircle(PlayerFrontLeft);
                _field.SetCircle(PlayerFrontLeft, _field.GetCircle(PlayerBackLeft));
                _field.SetCircle(PlayerBackLeft, temp);
                Console.WriteLine("----------\nLeft column Rearguard(s) changed position.");
            }
            else
            {
                temp = _field.GetCircle(PlayerFrontRight);
                _field.SetCircle(PlayerFrontRight, _field.GetCircle(PlayerBackRight));
                _field.SetCircle(PlayerBackRight, temp);
                Console.WriteLine("----------\nRight column Rearguard(s) changed position.");
            }
            if (OnChangeColumn != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.playerID = _playerID;
                args.i = location;
                OnChangeColumn(this, args);
            }
        }

        public void MoveRearguard(int location, int direction)
        {
            int location1 = GetCircle(_field.CardCatalog[location]);
            int location2 = location1;
            if (location1 == PlayerFrontLeft)
            {
                if (direction == Direction.Down)
                    location2 = PlayerBackLeft;
            }
            else if (location1 == PlayerBackLeft)
            {
                if (direction == Direction.Up)
                    location2 = PlayerFrontLeft;
                else if (direction == Direction.Right)
                    location2 = PlayerBackCenter;
            }
            else if (location1 == PlayerBackCenter)
            {
                if (direction == Direction.Left)
                    location2 = PlayerBackLeft;
                else if (direction == Direction.Right)
                    location2 = PlayerBackRight;
            }
            else if (location1 == PlayerFrontRight)
            {
                if (direction == Direction.Down)
                    location2 = PlayerBackRight;
            }
            else if (location1 == PlayerBackRight)
            {
                if (direction == Direction.Up)
                    location2 = PlayerFrontRight;
                else if (direction == Direction.Left)
                    location2 = PlayerBackCenter;
            }
            Card temp = _field.GetCircle(location1);
            _field.SetCircle(location1, _field.GetCircle(location2));
            _field.SetCircle(location2, temp);
        }

        public void MoveRearguardSpecific(int tempID, int location)
        {
            _field.SetCircle(GetCircle(_field.CardCatalog[tempID]), null);
            _field.SetCircle(location, _field.CardCatalog[tempID]);
        }

        public void ActivateACT(int selection)
        {

        }

        public void ActivateOrder(int selection)
        {

        }

        public void EnemyActivateOrder(int selection)
        {

        }

        public void Boost()
        {
            if (_field.Attacker == PlayerFrontLeft)
            {
                _field.GetCircle(PlayerBackLeft).upright = false;
                _field.Booster = PlayerBackLeft;
                Console.WriteLine("----------\n" + _field.GetCircle(PlayerBackLeft).name + " boosts " + _field.GetCircle(PlayerFrontLeft).name + "!");
            }
            else if (_field.Attacker == PlayerVanguard)
            {
                _field.GetCircle(PlayerBackCenter).upright = false;
                _field.Booster = PlayerBackCenter;
                Console.WriteLine("----------\n" + _field.GetCircle(PlayerBackCenter).name + " boosts " + _field.GetCircle(PlayerVanguard).name + "!");
            }
            else if (_field.Attacker == PlayerFrontRight)
            {
                _field.GetCircle(PlayerBackRight).upright = false;
                _field.Booster = PlayerBackRight;
                Console.WriteLine("----------\n" + _field.GetCircle(PlayerBackRight).name + " boosts " + _field.GetCircle(PlayerFrontRight).name + "!");
            }
            if (OnBoost != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.playerID = _playerID;
                args.card = _field.GetCircle(_field.Booster);
                OnBoost(this, args);
            }
        }

        public void SetAttacker(int selection)
        {
            _field.Attacker = GetCircle(_field.CardCatalog[selection]);
        }

        public void InitiateAttack(int target)
        {
            Card Attacker = _field.GetCircle(_field.Attacker);
            Card Attacked = _field.CardCatalog[target];
            int circle = GetCircle(Attacked);
            Attacker.upright = false;
            _field.Attacker = GetCircle(Attacker);
            _field.Attacked.Add(Attacked);
            if (_canColumnAttack.Contains(Attacker))
            {
                if (GetCircle(Attacked) == EnemyFrontLeft)
                    circle = EnemyBackLeft;
                else if (GetCircle(Attacked) == EnemyBackLeft)
                    circle = EnemyFrontLeft;
                else if (GetCircle(Attacked) == EnemyVanguard)
                    circle = EnemyBackCenter;
                else if (GetCircle(Attacked) == EnemyBackCenter)
                    circle = EnemyVanguard;
                else if (GetCircle(Attacked) == EnemyFrontRight)
                    circle = EnemyBackRight;
                else if (GetCircle(Attacked) == EnemyBackRight)
                    circle = EnemyFrontRight;
                if (_field.GetCircle(circle) != null && !_field.Attacked.Contains(_field.GetCircle(circle)))
                    _field.Attacked.Add(_field.GetCircle(circle));
            }
            Console.WriteLine("----------");
            foreach (Card card in _field.Attacked)
                Console.WriteLine(Attacker.name + " attacks " + card.name + "!");
            if (OnAttack != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.playerID = _playerID;
                args.card = Attacker;
                args.i = Attacked.tempID;
                OnAttack(this, args);
            }
        }

        public List<Card> GetAttackedCards()
        {
            List<Card> cards = new List<Card>();
            foreach (Card card in _field.Attacked)
            {
                if (GetAllUnitsOnField().Contains(card))
                    cards.Add(card);
            }
            return cards;
        }

        public Card GetAttacker()
        {
            if (_field.Attacker == -1)
                return null;
            return _field.GetCircle(_field.Attacker);
        }

        public bool Guard(List<int> selections, int target)
        {
            Card card;
            bool intercept = false;
            int _lastPlacedOnGC_Count = _lastPlacedOnGC.Keys.Count + 1;
            if (!_lastPutOnGC.ContainsKey(_lastPutOnGC.Keys.Count + 1))
                _lastPutOnGC[_lastPutOnGC.Keys.Count + 1] = new List<Card>();
            foreach (int selection in selections)
            {
                card = _field.CardCatalog[selection];
                if (target == -1)
                    target = _field.Attacked[0].tempID;
                if (!_field.Guardians.ContainsKey(target))
                    _field.Guardians[target] = new List<Card>();
                _field.Guardians[target].Add(card);
                if (card.location == Location.Hand)
                    PlayerHand.Remove(card);
                else if (card.location == Location.RC)
                {
                    _field.SetCircle(GetCircle(card), null);
                    _isIntercepting.Add(card);
                    intercept = true;
                }
                card.upright = false;
                card.location = Location.GC;
                _field.GC.Insert(0, card);
                _lastPutOnGC[_lastPutOnGC.Keys.Count].Add(card);
                if (!intercept)
                {
                    if (!_lastPlacedOnGC.ContainsKey(_lastPlacedOnGC_Count))
                        _lastPlacedOnGC[_lastPlacedOnGC_Count] = new List<Card>();
                    _lastPlacedOnGC[_lastPlacedOnGC_Count].Add(card);
                }
                Console.WriteLine("----------\nAdded " + card.name + " to Guardian Circle.");
                if (OnGuard != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.playerID = _playerID;
                    args.card = card;
                    OnGuard(this, args);
                }
            }
            return intercept;
        }

        public void GuardWithTwo()
        {
            _guardWithTwo = true;
        }

        public bool MustGuardWithTwo()
        {
            return _guardWithTwo;
        }

        public void CannotGuardFromHand()
        {
            _canGuardFromHand = false;
        }

        public bool CanGuardFromHand()
        {
            return _canGuardFromHand;
        }

        public void SetMinGradeForGuard(int min)
        {
            _minGradeForGuard = min;
        }

        public void PerfectGuard(int tempID)
        {
            _field.Sentinel.Add(_field.CardCatalog[tempID]);
        }

        public void HitImmunity(List<int> tempIDs, List<int> grades)
        {
            Card card = null;
            foreach (int tempID in tempIDs)
            {
                card = _field.CardCatalog[tempID];
                foreach (int grade in grades)
                {
                    if (!card.hitImmunity.Contains(grade))
                        card.hitImmunity.Add(grade);
                }
            }
        }

        public bool CannotBeHitByGrade(int tempID, int grade)
        {
            Card card = _field.CardCatalog[tempID];
            if (card.hitImmunity.Contains(grade))
                return true;
            return false;
        }

        public void TargetImmunity(int tempID)
        {
            for (int i = 1; i <= PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == tempID)
                {
                    _field.GetCircle(i).targetImmunity = true;
                    return;
                }
            }
        }

        public void Bind(List<int> tempIDs)
        {
            Card card;
            foreach (int tempID in tempIDs)
            {
                card = _field.CardCatalog[tempID];
                if (PlayerDrop.Contains(card))
                    PlayerDrop.Remove(card);
                else if (IsRearguard(tempID))
                    RemoveFromRC(tempID, false);
                PlayerBind.Add(card);
                card.location = Location.Bind;
            }
        }

        public void CountsAsTwoRetires(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (GetActiveUnits().Contains(card) && !_countsAsTwoRetires.Contains(card))
            {
                _countsAsTwoRetires.Add(card);
            }
        }

        public void DoesNotCountAsTwoRetires(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (GetActiveUnits().Contains(card) && _countsAsTwoRetires.Contains(card))
            {
                _countsAsTwoRetires.Remove(card);
            }
        }

        public bool CanCountAsTwoRetires(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (GetActiveUnits().Contains(card) && _countsAsTwoRetires.Contains(card))
            {
                return true;
            }
            return false;
        }

        public void RemoveFromRC(int tempID, bool imprison)
        {
            Card card = _field.CardCatalog[tempID];
            if (GetAllUnitsOnField().Contains(card))
            {
                foreach (Card originalDress in card.soul)
                {
                    if (imprison)
                    {
                        if (card.originalOwner == _playerID)
                        {
                            EnemyPrisoners.Add(originalDress);
                            EnemyOrder.Add(originalDress);
                        }
                        else
                        {
                            PlayerPrisoners.Add(originalDress);
                            PlayerOrder.Add(originalDress);
                        }
                        originalDress.location = Location.Prison;
                    }
                    else
                    {
                        PlayerDrop.Add(originalDress);
                        originalDress.location = Location.Drop;
                    }
                }
                card.soul.Clear();
                _field.SetCircle(GetCircle(card), null);
            }
        }

        public void Retire(List<int> tempIDs)
        {
            List<Card> drop;
            Card toBeRetired;
            _playerRetired = false;
            _enemyRetired = false;
            foreach (int tempID in tempIDs)
            {
                for (int i = EnemyFrontLeft; i <= EnemyVanguard; i++)
                {
                    if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == tempID)
                    {
                        drop = EnemyDrop;
                        toBeRetired = _field.GetCircle(i);
                        _field.SetCircle(i, null);
                        toBeRetired.location = Location.Drop;
                        toBeRetired.overDress = false;
                        drop.Insert(0, toBeRetired);
                        ResetCardValues(toBeRetired);
                        foreach (Card card in toBeRetired.soul)
                        {
                            card.location = Location.Drop;
                            drop.Insert(0, card);
                        }
                        toBeRetired.soul.Clear();
                        _enemyRetired = true;
                        _enemyRetiredThisTurn = true;
                        break;
                    }
                }
                for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
                {
                    if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == tempID)
                    {
                        drop = PlayerDrop;
                        toBeRetired = _field.GetCircle(i);
                        _field.SetCircle(i, null);
                        toBeRetired.location = Location.Drop;
                        toBeRetired.overDress = false;
                        drop.Insert(0, toBeRetired);
                        ResetCardValues(toBeRetired);
                        foreach (Card card in toBeRetired.soul)
                        {
                            card.location = Location.Drop;
                            drop.Insert(0, card);
                        }
                        toBeRetired.soul.Clear();
                        _playerRetired = true;
                        _playerRetiredThisTurn = true;
                        break;
                    }
                }
            }
        }

        public void RetireAttackedUnit()
        {
            List<int> list = new List<int>();
            foreach (Card card in _field.UnitsHit)
            {
                if (card != _field.GetCircle(PlayerVanguard))
                    list.Add(card.tempID);
            }
            Retire(list);
        }

        public bool EnemyRetired()
        {
            return _enemyRetired;
        }

        public bool EnemyRetiredThisTurn()
        {
            return _enemyRetiredThisTurn;
        }

        public bool PlayerRetired()
        {
            return _playerRetired;
        }

        public bool PlayerRetiredThisTurn()
        {
            return _playerRetiredThisTurn;
        }

        public void RetireGC()
        {
            _isIntercepting.Clear();
            foreach (Card card in _field.GC)
            {
                card.upright = true;
                card.location = Location.Drop;
                ResetCardValues(card);
                PlayerDrop.Insert(0, card);
            }
            _field.GC.Clear();
        }

        public bool IsIntercepting(int tempID)
        {
            if (_isIntercepting.Contains(_field.CardCatalog[tempID]))
                return true;
            return false;
        }

        public void SendToDeck(List<int> cardsToSend, bool bottom)
        {
            Card card;
            List<Card> deck;
            List<Card> hand;
            List<Card> drop;
            List<Card> leftover = new List<Card>();
            foreach (int tempID in cardsToSend)
            {
                card = _field.CardCatalog[tempID];
                if (PlayerLooking.Contains(card))
                    PlayerLooking.Remove(card);
                if (card.originalOwner == _playerID)
                {
                    deck = PlayerDeck;
                    hand = PlayerHand;
                    drop = PlayerDrop;
                }
                else
                {
                    deck = EnemyDeck;
                    hand = EnemyHand;
                    drop = EnemyDrop;
                }
                if (card.unitType == UnitType.Token)
                    _field.SetCircle(GetCircle(card), null);
                else
                {
                    if (card.location == Location.RC)
                    {
                        foreach (Card originalDress in card.soul)
                            leftover.Add(originalDress);
                        _field.SetCircle(GetCircle(card), null);
                    }
                    else if (card.location == Location.Deck)
                        deck.Remove(card);
                    else if (card.location == Location.Hand)
                        hand.Remove(card);
                    card.location = Location.Deck;
                    if (bottom)
                        deck.Add(card);
                    else
                        deck.Insert(0, card);
                    foreach (Card originalDress in leftover)
                        drop.Add(originalDress);
                }
            }
        }

        public int TriggerCheck(bool drivecheck)
        {
            Card trigger = PlayerDeck[0];
            trigger.upright = false;
            trigger.faceup = true;
            trigger.location = Location.Trigger;
            PlayerDeck.Remove(trigger);
            PlayerTrigger[0] = trigger;
            _lastRevealedTriggers.Add(trigger);
            _lastRevealedTrigger = trigger;
            if (trigger.trigger == Trigger.Critical)
                Console.WriteLine("----------\nCritical Trigger!");
            else if (trigger.trigger == Trigger.Draw)
                Console.WriteLine("----------\nDraw Trigger!");
            else if (trigger.trigger == Trigger.Front)
                Console.WriteLine("----------\nFront Trigger!");
            else if (trigger.trigger == Trigger.Stand)
                Console.WriteLine("----------\nStand Trigger!");
            else if (trigger.trigger == Trigger.Heal)
                Console.WriteLine("----------\nHeal Trigger!");
            else if (trigger.trigger == Trigger.Over)
                Console.WriteLine("----------\nOver Trigger!");
            else
                Console.WriteLine("----------\nNo Trigger.");
            CardEventArgs args = new CardEventArgs();
            args.playerID = _playerID;
            args.i = trigger.trigger;
            args.card = trigger;
            if (drivecheck && OnDriveCheck != null)
                OnDriveCheck(this, args);
            if (!drivecheck && OnDamageCheck != null)
                OnDamageCheck(this, args);
            return trigger.trigger;
        }

        public Card FindActiveUnit(int selection)
        {
            List<Card> cards = GetAllUnitsOnField();
            foreach (Card card in cards)
            {
                if (card.tempID == selection)
                {
                    return card;
                }
            }
            return null;
        }

        public void AddPower(int selection, int power)
        {
            Card target = FindActiveUnit(selection);
            target.bonusPower += power;
        }

        public void AddTempPower(int selection, int power, bool battleOnly)
        {
            Card target = _field.CardCatalog[selection];
            if (target.location != Location.RC && target.location != Location.VC)
                return;
            if (battleOnly)
                target.battleOnlyPower += power;
            else
                target.tempPower += power;
                Console.WriteLine("----------\n" + power + " power to " + target.name + "!");
        }

        public void AddTempPower(List<int> selections, int power)
        {
            foreach (int tempID in selections)
            {
                _field.CardCatalog[tempID].tempPower += power;
                Console.WriteLine("----------\n" + power + " power to " + _field.CardCatalog[tempID].name + "!");
            }
        }

        public void AddCirclePower(int location, int power)
        {
            int circle = Convert(location);
            _field.CirclePower[circle] += power;
        }

        public void AddCircleCritical(int location, int critical)
        {
            int circle = Convert(location);
            _field.CircleCritical[circle] += critical;
        }

        public void AddCritical(int selection, int critical)
        {
            Card target = FindActiveUnit(selection);
            target.tempCritical += critical;
            Console.WriteLine("----------\n+" + critical + " critical to " + target.name + "!");
        }

        public void AddBattleOnlyCritical(int selection, int critical)
        {
            Card target = FindActiveUnit(selection);
            target.battleOnlyCritical += critical;
            Console.WriteLine("----------\n+" + critical + " critical to " + target.name + "!");
        }

        public void DoublePower(int selection)
        {
            Card target = _field.CardCatalog[selection];
            target.tempPower += CalculatePowerOfUnit(GetCircle(target));
            if (_field.Booster >= 0 && GetCircle(target) == _field.Attacker)
            {
                target.tempPower -= CalculatePowerOfUnit(_field.Booster);
            }
        }

        public void DoubleCritical(int selection)
        {
            Card target = _field.CardCatalog[selection];
            target.tempCritical += Critical(selection);
        }

        public void AddDrive(int selection, int drive)
        {
            Card target = FindActiveUnit(selection);
            target.tempDrive += drive;
            Console.WriteLine("----------\n+" + drive + " drive to " + target.name + "!");
        }

        public void AddTempShield(int selection, int shield)
        {
            Card target = _field.CardCatalog[selection];
            target.tempShield += shield;
            Console.WriteLine("----------\n" + shield + " shield to " + target.name + "!");
        }

        public void SetAbilityPower(int tempID, int abilityNum, int selection, int power)
        {
            Card target = _field.CardCatalog[selection];
            if (!target.abilityPower.ContainsKey(new Tuple<int, int>(tempID, abilityNum)))
            {
                target.abilityPower.Add(new Tuple<int, int>(tempID, abilityNum), power);
            }
            else
                target.abilityPower[new Tuple<int, int>(tempID, abilityNum)] = power;
        }

        public void SetAbilityShield(int tempID, int abilityNum, int selection, int shield)
        {
            Card target = _field.CardCatalog[selection];
            if (!target.abilityShield.ContainsKey(new Tuple<int, int>(tempID, abilityNum)))
            {
                target.abilityShield.Add(new Tuple<int, int>(tempID, abilityNum), shield);
            }
            else
                target.abilityShield[new Tuple<int, int>(tempID, abilityNum)] = shield;
        }

        public void SetAbilityDrive(int tempID, int abilityNum, int selection, int drive)
        {
            Card target = _field.CardCatalog[selection];
            if (!target.abilityDrive.ContainsKey(new Tuple<int, int>(tempID, abilityNum)))
            {
                target.abilityDrive.Add(new Tuple<int, int>(tempID, abilityNum), drive);
            }
            else
                target.abilityDrive[new Tuple<int, int>(tempID, abilityNum)] = drive;
        }

        public void SetAbilityCritical(int tempID, int abilityNum, int selection, int critical)
        {
            Card target = _field.CardCatalog[selection];
            if (!target.abilityCritical.ContainsKey(new Tuple<int, int>(tempID, abilityNum)))
            {
                target.abilityCritical.Add(new Tuple<int, int>(tempID, abilityNum), critical);
            }
            else
                target.abilityCritical[new Tuple<int, int>(tempID, abilityNum)] = critical;
        }

        public void Stand(int selection)
        {
            Card target = FindActiveUnit(selection);
            _stoodByCardEffect[_stoodByCardEffect.Keys.Count + 1] = new List<Card>();
            if (!target.upright)
                _stoodByCardEffect[_stoodByCardEffect.Keys.Count].Add(target);
            target.upright = true;
        }

        public void Heal(int selection)
        {
            foreach (Card card in PlayerDamage)
            {
                if (card.tempID == selection)
                {
                    card.faceup = true;
                    card.upright = true;
                    card.location = Location.Drop;
                    PlayerDamage.Remove(card);
                    PlayerDrop.Insert(0, card);
                    Console.WriteLine("----------\nDamage healed!");
                    return;
                }
            }
        }

        public void RemoveTrigger()
        {
            PlayerTrigger[0] = null;
        }

        public void AddTriggerToHand()
        {
            Card card = PlayerTrigger[0];
            if (card != null)
            {
                card.location = Location.Hand;
                PlayerTrigger[0] = null;
                card.upright = true;
                PlayerHand.Add(card);
            }
        }

        public void AddToHand(List<int> selections)
        {
            Card cardToAdd;
            List<Card> drop;
            drop = PlayerDrop;
            foreach (int tempID in selections)
            {
                cardToAdd = _field.CardCatalog[tempID];
                if (drop.Contains(cardToAdd))
                    drop.Remove(cardToAdd);
                else if (cardToAdd.location == Location.RC)
                    RemoveFromRC(tempID, false);
                else if (_field.GetCircle(PlayerVanguard).soul.Contains(cardToAdd))
                    _field.GetCircle(PlayerVanguard).soul.Remove(cardToAdd);
                else if (PlayerBind.Contains(cardToAdd))
                    PlayerBind.Remove(cardToAdd);
                if (PlayerLooking.Contains(cardToAdd))
                    PlayerLooking.Remove(cardToAdd);
                if (PlayerRevealed.Contains(cardToAdd))
                    PlayerRevealed.Remove(cardToAdd);
                cardToAdd.location = Location.Hand;
                PlayerHand.Add(cardToAdd);
            }
            Console.WriteLine(selections.Count + " card(s) added to hand.");
        }

        public void AddToSoul(List<int> selections)
        {
            Card cardToAdd;
            foreach (int tempID in selections)
            {
                cardToAdd = _field.CardCatalog[tempID];
                if (cardToAdd.location == Location.RC)
                {
                    for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
                    {
                        if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == cardToAdd.tempID)
                            _field.SetCircle(i, null);
                    }
                    for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                    {
                        if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == cardToAdd.tempID)
                            _field.SetCircle(i, null);
                    }
                }
                else if (cardToAdd.location == Location.Deck)
                {
                    PlayerDeck.Remove(cardToAdd);
                }
                else if (cardToAdd.location == Location.Drop)
                {
                    PlayerDrop.Remove(cardToAdd);
                }
                else if (cardToAdd.location == Location.Order)
                {
                    if (cardToAdd == _playedOrder)
                        _playedOrder = null;
                    PlayerOrder.Remove(cardToAdd);
                }
                else if (cardToAdd.location == Location.Hand)
                {
                    PlayerHand.Remove(cardToAdd);
                }
                if (PlayerLooking.Contains(cardToAdd))
                    PlayerLooking.Remove(cardToAdd);
                cardToAdd.location = Location.Soul;
                cardToAdd.faceup = true;
                cardToAdd.upright = true;
                ResetCardValues(cardToAdd);
                _field.GetCircle(PlayerVanguard).soul.Add(cardToAdd);
            }
        }

        public void AddToDrop(List<int> selections)
        {
            Card cardToAdd;
            foreach (int tempID in selections)
            {
                cardToAdd = _field.CardCatalog[tempID];
                if (cardToAdd.location == Location.RC)
                {
                    for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
                    {
                        if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == cardToAdd.tempID)
                            _field.SetCircle(i, null);
                    }
                }
                if (cardToAdd.location == Location.RC)
                {
                    for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                    {
                        if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == cardToAdd.tempID)
                            _field.SetCircle(i, null);
                    }
                }
                if (PlayerPrisoners.Contains(cardToAdd))
                    PlayerPrisoners.Remove(cardToAdd);
                else if (cardToAdd.location == Location.Deck)
                {
                    PlayerDeck.Remove(cardToAdd);
                }
                else if (PlayerBind.Contains(cardToAdd))
                    PlayerBind.Remove(cardToAdd);
                if (PlayerLooking.Contains(cardToAdd))
                    PlayerLooking.Remove(cardToAdd);
                if (PlayerRevealed.Contains(cardToAdd))
                    PlayerRevealed.Remove(cardToAdd);
                cardToAdd.location = Location.Drop;
                cardToAdd.faceup = true;
                cardToAdd.upright = true;
                ResetCardValues(cardToAdd);
                PlayerDrop.Add(cardToAdd);
            }
        }

        public void ChangeLocation(int destination, List<int> selections)
        {
            Card cardToAdd;
            foreach (int tempID in selections)
            {
                cardToAdd = _field.CardCatalog[tempID];
                if (cardToAdd.location == Location.RC)
                {
                    for (int i = 0; i < FL.MaxFL(); i++)
                    {
                        if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == cardToAdd.tempID)
                        {
                            _field.SetCircle(i, null);
                            break;
                        }
                    }
                }
                else if (cardToAdd.location == Location.Deck)
                {
                    PlayerDeck.Remove(cardToAdd);
                }
                if (destination == Location.Hand)
                {
                    cardToAdd.location = Location.Hand;
                    PlayerHand.Add(cardToAdd);
                }
                else if (destination == Location.Drop)
                {
                    cardToAdd.location = Location.Drop;
                    PlayerDrop.Insert(0, cardToAdd);
                }
                ResetCardValues(cardToAdd);
            }
        }

        public void TakeDamage()
        {
            Card card = PlayerTrigger[0];
            card.location = Location.Damage;
            card.faceup = true;
            PlayerTrigger[0] = null;
            PlayerDamage.Add(card);
            Console.WriteLine("----------\nDamage taken!");
        }

        public void PlayOrder(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (card.location == Location.Drop)
            {
                PlayerBind.Add(card);
                PlayerDrop.Remove(card);
                card.location = Location.Bind;
                _isAlchemagic = true;
            }
            else if (card.location == Location.Hand)
            {
                _playedOrder = card;
                _lastOrderPlayed = card;
                _orderPlayed = true;
                PlayerHand.Remove(card);
                PlayerOrder.Add(card);
                card.location = Location.Order;
            }
        }

        public Card GetPlayedOrder()
        {
            return _playedOrder;
        }

        public void EndOrder()
        {
            if (_playedOrder != null && (_playedOrder.orderType == OrderType.Normal || _playedOrder.orderType == OrderType.Blitz))
            {
                PlayerOrder.Remove(_playedOrder);
                PlayerDrop.Add(_playedOrder);
                _playedOrder.location = Location.Drop;
                _playedOrder = null;
            }
        }

        public bool OrderPlayed()
        {
            return _orderPlayed;
        }

        public void AllowFreeSwap()
        {
            _freeSwap = true;
        }

        public bool CanFreeSwap()
        {
            return _freeSwap;
        }

        public void AllowColumnAttack(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (!_canColumnAttack.Contains(card))
                _canColumnAttack.Add(card);
        }

        public void DisableColumnAttack(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (_canColumnAttack.Contains(card))
                _canColumnAttack.Remove(card);
        }

        public void SetAlchemagic(int tempID)
        {
            _field.CardCatalog[tempID].alchemagic = true;
        }

        public void EnterAlchemagic()
        {
            _isAlchemagic = true;
            _alchemagicUsed = true;
        }

        public void EndAlchemagic()
        {
            _isAlchemagic = false;
            _alchemagicFreeSB = false;
        }

        public bool CanAlchemagic()
        {
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null && _field.GetCircle(i).alchemagic)
                    return true;
            }
            return false;
        }

        public bool IsAlchemagic()
        {
            return _isAlchemagic;
        }

        public void AlchemagicFreeSB()
        {
            _alchemagicFreeSB = true;
        }

        public void AddAlchemagicFreeCB(int count)
        {
            _alchemagicFreeCBAvailable += count;
            if (_alchemagicFreeCBAvailable < 0)
                _alchemagicFreeCBAvailable = 0;
        }

        public bool AlchemagicUsedThisTurn()
        {
            return _alchemagicUsed;
        }

        public void ResetAlchemagicFreeCB()
        {
            _alchemagicFreeCBAvailable = 0;
        }

        public bool AlchemagicFreeSBAvailable()
        {
            return _alchemagicFreeSB;
        }

        public int AlchemagicFreeCBAvailable()
        {
            return _alchemagicFreeCBAvailable;
        }

        public void SetPrison()
        {
            PlayerPrison = _playedOrder;
            _playedOrder = null;
        }

        public void Imprison(List<int> cardsToImprison)
        {
            Card card;
            foreach (int tempID in cardsToImprison)
            {
                card = _field.CardCatalog[tempID];
                if (card.location == Location.Hand)
                {
                    EnemyHand.Remove(card);
                }
                else if (card.location == Location.RC)
                {
                    RemoveFromRC(tempID, true);
                }
                else if (card.location == Location.Drop)
                {
                    PlayerDrop.Remove(card);
                }
                card.location = Location.Prison;
                PlayerPrisoners.Add(card);
                PlayerOrder.Add(card);
            }
        }

        public bool HasPrison()
        {
            if (PlayerPrison != null)
                return true;
            return false;
        }

        public List<Card> GetLastCalledFromPrison()
        {
            return _lastCalledFromPrison;
        }

        public void SetWorld()
        {
            PlayerWorld = _playedOrder;
            _playedOrder = null;
        }

        public bool IsWorld(int tempID)
        {
            if (PlayerWorld != null && PlayerWorld.tempID == tempID)
                return true;
            return false;
        }

        public bool OnlyWorlds()
        {
            foreach (Card card in PlayerOrder)
            {
                if (card.orderType != OrderType.World)
                    return false;
            }
            return true;
        }

        public int NumWorlds()
        {
            int count = 0;
            foreach (Card card in PlayerOrder)
            {
                if (card.orderType == OrderType.World)
                    count++;
            }
            return count;
        }

        public void DarkNight()
        {
            _darkNight = true;
            _abyssalDarkNight = false;
        }

        public void AbyssalDarkNight()
        {
            _abyssalDarkNight = true;
            _darkNight = false;
        }

        public void NoWorld()
        {
            _darkNight = false;
            _abyssalDarkNight = false;
        }

        public bool IsDarkNight()
        {
            return _darkNight;
        }

        public bool IsAbyssalDarkNight()
        {
            return _abyssalDarkNight;
        }

        public bool WorldPlayed()
        {
            if (_lastOrderPlayed != null && _lastOrderPlayed.orderType == OrderType.World)
                return true;
            return false;
        }

        public int CreateToken(string tokenID)
        {
            return _field.CreateToken(tokenID);
        }

        public bool CanCB(int count)
        {
            int faceup = 0;
            foreach (Card card in PlayerDamage)
            {
                if (card.faceup)
                    faceup++;
            }
            if (faceup + _alchemagicFreeCBAvailable < count)
                return false;
            return true;
        }

        public bool CanRetire(int count)
        {
            int existing = 0;
            for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                    existing++;
            }
            if (existing >= count)
                return true;
            return false;
        }

        public void CounterBlast(List<int> cardsToCB)
        {
            List<Card> damage = null;
            _CBUsed = 0;
            damage = PlayerDamage;
            foreach (int tempID in cardsToCB)
            {
                foreach (Card card in damage)
                {
                    if (card.tempID == tempID)
                    {
                        card.faceup = false;
                        _CBUsed++;
                        break;
                    }
                }
            }
        }

        public int CBUsed()
        {
            return _CBUsed;
        }

        public bool CanSB(int count)
        {
            if (_alchemagicFreeSB || _field.GetCircle(PlayerVanguard).soul.Count >= count)
                return true;
            return false;
        }

        public void SoulBlast(List<int> cardsToSB)
        {
            List<Card> soul = null;
            List<Card> drop = null;
            soul = _field.GetCircle(PlayerVanguard).soul;
            drop = PlayerDrop;
            foreach (int tempID in cardsToSB)
            {
                foreach (Card card in soul)
                {
                    if (card.tempID == tempID)
                    {
                        soul.Remove(card);
                        drop.Insert(0, card);
                        card.location = Location.Drop;
                        break;
                    }
                }
            }
        }

        public void CounterCharge(List<int> cardsToCharge)
        {
            foreach (int tempID in cardsToCharge)
            {
                _field.CardCatalog[tempID].faceup = true;
            }
        }

        public void SoulCharge(int count)
        {
            Card card;
            int location = 0;
            List<Card> deck = null;
            location = PlayerVanguard;
            deck = PlayerDeck;
            Console.WriteLine("Soul Charging " + count + " card(s)!");
            _soulChargedThisTurn = true;
            for (int i = 0; i < count; i++)
            {
                card = deck[0];
                deck.RemoveAt(0);
                _field.GetCircle(location).soul.Add(card);
                card.location = Location.Soul;
                card.faceup = true;
            }
        }

        public bool SoulChargedThisTurn()
        {
            return _soulChargedThisTurn;
        }

        public void Search(List<int> cardsToSearch)
        {
            List<Card> hand = null;
            List<Card> deck = null;
            int handLocation = 0;
            hand = PlayerHand;
            deck = PlayerDeck;
            handLocation = Location.Hand;
            foreach (int tempID in cardsToSearch)
            {
                foreach (Card card in deck)
                {
                    if (card.tempID == tempID)
                    {
                        deck.Remove(card);
                        card.faceup = true;
                        hand.Add(card);
                        card.location = handLocation;
                        _field.Shuffle(PlayerDeck);
                        break;
                    }
                }
            }
        }

        public void Stand(List<int> cardsToStand)
        {
            _stoodByCardEffect[_stoodByCardEffect.Keys.Count + 1] = new List<Card>();
            foreach (int tempID in cardsToStand)
            {
                if (_field.CardCatalog[tempID].upright == false)
                {
                    _field.CardCatalog[tempID].upright = true;
                    _stoodByCardEffect[_stoodByCardEffect.Keys.Count].Add(_field.CardCatalog[tempID]);
                }
            }
        }

        public void Rest(List<int> cardsToRest)
        {
            foreach (int tempID in cardsToRest)
            {
                _field.CardCatalog[tempID].upright = false;
            }
        }

        public void Reveal(List<int> cardsToReveal)
        {
            Card card;
            List<Card> revealed = null;
            string dialogue = "";
            revealed = PlayerRevealed;
            EndReveal();
            foreach (int tempID in cardsToReveal)
            {
                card = _field.CardCatalog[tempID];
                card.faceup = true;
                revealed.Add(card);
                if (card.location == Location.Hand)
                    dialogue = "hand";
                Console.WriteLine("----------\n" + card.name + " revealed from " + dialogue + "!");
            }
        }

        public void RevealFromDeck(int count)
        {
            List<Card> deck = null;
            List<Card> revealed = null;
            EndReveal();
            deck = PlayerDeck;
            revealed = PlayerRevealed;
            for (int i = 0; i < count; i++)
            {
                revealed.Add(deck[i]);
                deck[i].faceup = true;
                Console.WriteLine("----------\n" + deck[i].name + " revealed from deck!");
            }
        }

        public void EndReveal()
        {
            //foreach (Card card in PlayerRevealed)
            //{
            //    if (card.location == Location.EnemyHand || card.location == Location.Deck)
            //        card.faceup = false;
            //}
            PlayerRevealed.Clear();
        }

        public void LookAtTopOfDeck(int count)
        {
            PlayerLooking.Clear();
            for (int i = 0; i < count; i++)
                PlayerLooking.Add(PlayerDeck[i]);
        }

        public void RearrangeOnTop(List<int> tempIDs)
        {
            Card card;
            for (int i = 0; i < tempIDs.Count; i++)
            {
                card = _field.CardCatalog[tempIDs[i]];
                PlayerDeck[i] = card;
                if (PlayerLooking.Contains(card))
                    PlayerLooking.Remove(card);
            }
        }

        public void RearrangeOnBottom(List<int> tempIDs)
        {
            Card card;
            for (int i = PlayerDeck.Count - 1 - tempIDs.Count; i < tempIDs.Count; i++)
            {
                card = _field.CardCatalog[tempIDs[i]];
                PlayerDeck[i] = card;
                if (PlayerLooking.Contains(card))
                    PlayerLooking.Remove(card);
            }
        }

        public void AddSkill(int tempID, int skill)
        {
            if (!_bonusSkills.ContainsKey(tempID))
                _bonusSkills[tempID] = new List<int>();
            if (!_bonusSkills[tempID].Contains(skill))
                _bonusSkills[tempID].Add(skill);
        }

        public void RemoveSkill(int tempID, int skill)
        {
            if (_bonusSkills.ContainsKey(skill) && _bonusSkills[tempID].Contains(skill))
                _bonusSkills[tempID].Remove(skill);
        }

        public void AllowBackRowAttack(int tempID)
        {
            _canAttackFromBackRow.Add(_field.CardCatalog[tempID]);
        }

        public void AllowAttackingBackRow(int tempID)
        {
            _canAttackBackRow.Add(_field.CardCatalog[tempID]);
        }

        public void GiveAbility(List<int> tempIDs, int abilityTempID, int activationNumber)
        {
            foreach (int tempID in tempIDs)
            {
                if (GetAllUnitsOnField().Contains(_field.CardCatalog[tempID]))
                {
                    if (!_givenAbilities.ContainsKey(tempID))
                        _givenAbilities[tempID] = new List<Tuple<int, int>>();
                    if (!_givenAbilities[tempID].Contains(new Tuple<int, int>(abilityTempID, activationNumber)))
                        _givenAbilities[tempID].Add(new Tuple<int, int>(abilityTempID, activationNumber));
                }
            }
        }

        public List<Tuple<int, int>> GetGivenAbility(int tempID)
        {
            if (_givenAbilities.ContainsKey(tempID))
                return _givenAbilities[tempID];
            return null;
        }

        public void ResetPower()
        {
            for (int i = 1; i <= PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                {
                    _field.GetCircle(i).tempPower = 0;
                    _field.GetCircle(i).tempCritical = 0;
                    _field.GetCircle(i).tempDrive = 0;
                    _field.GetCircle(i).tempShield = 0;
                }
            }
        }

        public void EndAttack()
        {
            _field.Sentinel.Clear();
            _field.Attacker = -1;
            _field.Attacked.Clear();
            _field.Guardians.Clear();
            _field.Booster = -1;
            _lastRevealedTriggers.Clear();
            _field.UnitsHit.Clear();
            _guardWithTwo = false;
            _canGuardFromHand = true;
            _minGradeForGuard = 6;
            for (int i = 1; i <= PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                {
                    _field.GetCircle(i).battleOnlyPower = 0;
                    _field.GetCircle(i).battleOnlyCritical = 0;
                    _field.GetCircle(i).hitImmunity.Clear();
                }
            }
        }

        public void EndTurn()
        {
            ResetPower();
            _rearguardDriveCheck = false;
            _finalRush = false;
            _alchemagicFreeSB = false;
            _alchemagicFreeCBAvailable = 0;
            _alchemagicUsed = false;
            _field.SetPersonaRide(false, _playerID);
            _canAttackFromBackRow.Clear();
            _canAttackBackRow.Clear();
            _canColumnAttack.Clear();
            _bonusSkills.Clear();
            _stoodByCardEffect.Clear();
            _countsAsTwoRetires.Clear();
            _givenAbilities.Clear();
            _orderPlayed = false;
            _soulChargedThisTurn = false;
            _playerRetiredThisTurn = false;
            _enemyRetiredThisTurn = false;
            _canIntercept = true;
            for (int i = 0; i < _field.CirclePower.Length; i++)
                _field.CirclePower[i] = 0;
            for (int i = 0; i < _field.CircleCritical.Length; i++)
                _field.CircleCritical[i] = 0;
        }

        public void RefreshContinuous()
        {
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                {
                    _field.GetCircle(i).targetImmunity = false;
                }
            }
            _freeSwap = false;
        }

        public void AllAbilitiesResolved()
        {
            _lastPlacedOnRC.Clear();
            _lastPlacedOnRCFromHand.Clear();
            _lastPlacedOnRC_Count = 1;
            _lastPlacedOnRCFromHand_Count = 1;
            _lastPlacedOnGC.Clear();
            _lastPutOnGC.Clear();
            _riddenOnThisTurn.Clear();
            _lastRevealedTriggers.Clear();
            _lastDiscarded.Clear();
            _lastCalledFromPrison.Clear();
            _lastPlacedOnVC.Clear();
            _stoodByCardEffect.Clear();
        }

        public void IncrementTurn()
        {
            _field.IncrementTurn();
        }

        public void ResetCardValues(Card card)
        {
            card.bonusPower = 0;
            card.battleOnlyPower = 0;
            card.tempPower = 0;
            card.tempShield = 0;
            card.tempCritical = 0;
            card.tempDrive = 0;
            card.abilityPower.Clear();
            card.abilityDrive.Clear();
            card.abilityShield.Clear();
            card.abilityCritical.Clear();
            card.hitImmunity.Clear();
            if (_givenAbilities.ContainsKey(card.tempID))
                _givenAbilities[card.tempID].Clear();
            _canAttackFromBackRow.Remove(card);
            _canAttackBackRow.Remove(card);
            for (int i = 1; i <= PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                {
                    foreach (Tuple<int, int> key in _field.GetCircle(i).abilityPower.Keys)
                    {
                        if (key.Item1 == card.tempID)
                        {
                            SetAbilityPower(key.Item1, key.Item2, _field.GetCircle(i).tempID, 0);
                            SetAbilityShield(key.Item1, key.Item2, _field.GetCircle(i).tempID, 0);
                            SetAbilityDrive(key.Item1, key.Item2, _field.GetCircle(i).tempID, 0);
                            SetAbilityCritical(key.Item1, key.Item2, _field.GetCircle(i).tempID, 0);
                        }
                    }
                }
            }
            card.targetImmunity = false;
            card.alchemagic = false;
            card.overDress = false;
            if (_bonusSkills.ContainsKey(card.tempID))
                _bonusSkills[card.tempID].Clear();
        }

        public void OnRideAbilityResolved(int tempID)
        { 

        }

        public bool IsEnemy(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            for (int i = EnemyFrontLeft; i <= EnemyVanguard; i++)
            {
                if (_field.GetCircle(i) != null && _field.GetCircle(i).tempID == tempID)
                    return true;
            }
            return false;
        }

        public List<Card> Soul()
        {
            return _field.GetCircle(PlayerVanguard).soul;
        }

        public List<Card> CardsForOnRide()
        {
            List<Card> cards =  new List<Card>();
            cards.AddRange(PlayerHand);
            for (int i = PlayerFrontLeft; i < PlayerVanguard + 1; i++)
                cards.Add(_field.GetCircle(i));
            cards.AddRange(Soul());
            return cards;
        }

        public List<Card> CardsForOnAttack()
        {
            List<Card> cards = new List<Card>();
            cards.AddRange(PlayerHand);
            for (int i = 1; i <= PlayerVanguard; i++)
            {
                if (_field.GetCircle(i) != null)
                    cards.Add(_field.GetCircle(i));
            }
            cards.AddRange(Soul());
            return cards;
        }

        public string PlayerVanguardName()
        {
            return _field.GetCircle(PlayerVanguard).name;
        }

        public Card Vanguard()
        {
            return _field.GetCircle(PlayerVanguard);
        }

        public Card AttackingUnit()
        {
            if (_field.Attacker > 0)
                return _field.GetCircle(_field.Attacker);
            return null;
        }

        public Card Booster()
        {
            if (_field.Booster > 0)
                return _field.GetCircle(_field.Booster);
            return null;
        }

        public List<Card> AttackedUnits()
        {
            return _field.Attacked;
        }

        public int AttackingUnitLocation()
        {
            return _field.Attacker;
        }

        public int AttackingUnitID()
        {
            return _field.GetCircle(_field.Attacker).tempID;
        }

        public bool HasCardInDeck(string name)
        {
            foreach (Card card in PlayerDeck)
            {
                if (card.name == name)
                    return true;
            }
            return false;
        }

        public bool IsTopSoul(Card card)
        {
            if (card == _field.GetCircle(PlayerVanguard).soul[0])
                return true;
            return false;
        }

        public bool InFinalRush()
        {
            return _finalRush;
        }

        public void FinalRush()
        {
            _finalRush = true;
        }

        public void RearguardDriveCheck()
        {
            _rearguardDriveCheck = true;
        }

        public bool RearguardCanDriveCheck()
        {
            return _rearguardDriveCheck;
        }

        public bool StoodByCardEffect(int tempID, int count)
        {
            Card card = _field.CardCatalog[tempID];
            if (_stoodByCardEffect.ContainsKey(count) && _stoodByCardEffect[count].Contains(card))
                return true;
            return false;
        }

        public int NumOriginalDress(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (card.overDress)
                return card.soul.Count;
            return 0;
        }

        public List<Card> GetCardsWithName(string name)
        {
            List<Card> cardsWithName = new List<Card>();
            foreach (Card card in PlayerDeck)
            {
                if (card.name == name)
                    cardsWithName.Add(card);
            }
            return cardsWithName;
        }

        public Card GetCard(int tempID)
        {
            return _field.CardCatalog[tempID];
        }

        public int Convert(int fl)
        {
            if (_playerID == 1)
                return fl;
            else
                return FL.SwitchSides(fl);
        }

        public bool IsPlayer(int circle)
        {
            if (circle >= PlayerFrontLeft && circle <= PlayerVanguard)
                return true;
            return false;
        }

        public bool IsFrontRow(int circle)
        {
            if (circle == FL.PlayerFrontLeft ||
                circle == FL.PlayerVanguard ||
                circle == FL.PlayerFrontRight ||
                circle == FL.EnemyFrontLeft ||
                circle == FL.EnemyVanguard ||
                circle == FL.EnemyFrontRight)
                return true;
            return false;
        }

        public class Direction
        {
            public const int Up = 1;
            public const int Right = 2;
            public const int Down = 3;
            public const int Left = 4;
        }
    }
}
