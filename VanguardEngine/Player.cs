using System;
using System.Collections.Generic;
using System.Text;

namespace VanguardEngine
{
    public class Player
    {
        protected Field _field;
        protected int _damage = 0;
        public int _startingTurn = -1;
        protected bool _guarding = false;
        protected Dictionary<int, RecordedCardValue> _recordedCardValues = new Dictionary<int, RecordedCardValue>();
        protected Dictionary<int, int> _recordedShieldValues = new Dictionary<int, int>();
        protected List<Card> _lastRidden = new List<Card>();
        //protected Dictionary<int, Tuple<int, string>> _cardRiddenBy = new Dictionary<int, Tuple<int, string>>();
        protected List<Card> _lastPlacedOnGC = new List<Card>();
        protected List<Card> _lastPutOnGC = new List<Card>();
        protected List<Card> _lastPlacedOnRC = new List<Card>();
        protected List<Card> _lastPlacedOnRCFromHand = new List<Card>();
        protected List<Card> _lastPlacedOnRCFromPrison = new List<Card>();
        protected List<Card> _lastPlacedOnRCOtherThanFromHand = new List<Card>();
        protected List<Card> _unitsCalledThisTurn = new List<Card>();
        protected List<Card> _unitsCalledFromHandThisTurn = new List<Card>();
        protected List<Card> _lastPlayerRetired = new List<Card>();
        protected List<Card> _lastPlayerRCRetired = new List<Card>();
        protected List<Card> _lastPlacedOnVC = new List<Card>();
        protected List<Card> _lastPutOnOrderZone = new List<Card>();
        protected List<Card> _lastRevealedDriveChecks = new List<Card>();
        protected List<Card> _lastRevealedDamageChecks = new List<Card>();
        protected Card _lastRevealedTrigger = null;
        protected List<Card> _lastDiscarded = new List<Card>();
        protected List<Card> _isIntercepting = new List<Card>();
        protected Card _lastOrderPlayed = null;
        protected List<Card> _stoodByCardEffect = new List<Card>();
        protected List<int> _stoodByCardEffectThisTurn = new List<int>();
        protected List<Card> _retiredForPlayerCost = new List<Card>();
        protected Card _playedOrder;
        protected int _CBUsed = 0;
        protected int _bonusDriveCheckPower = 0;
        protected bool _enemyRetired = false;
        protected bool _playerRetired = false;
        protected bool _enemyRetiredThisTurn = false;
        protected bool _playerRetiredThisTurn = false;
        protected bool _soulChargedThisTurn = false;
        protected bool _orderPlayed = false;
        protected bool _isAlchemagic = false;
        protected bool _alchemagicFreeSB = false;
        protected bool _alchemagicFreeSBActive = false;
        protected int _alchemagicFreeCBAvailable = 0;
        protected bool _alchemagicUsed = false;
        protected bool _riddenThisTurn = false;
        protected Zone PlayerHand;
        protected Zone EnemyHand;
        protected Zone PlayerDeck;
        protected Zone EnemyDeck;
        protected Zone PlayerDrop;
        protected Zone EnemyDrop;
        protected Zone PlayerDamage;
        protected Zone EnemyDamage;
        protected Zone PlayerBind;
        protected Zone EnemyBind;
        protected Zone PlayerTrigger;
        protected Zone EnemyTrigger;
        protected Zone PlayerOrder;
        protected Zone EnemyOrder;
        protected Zone PlayerOrderArea;
        protected Zone EnemyOrderArea;
        protected Zone PlayerRideDeck;
        protected Zone EnemyRideDeck;
        protected PseudoZone PlayerRevealed;
        protected PseudoZone EnemyRevealed;
        protected PseudoZone PlayerLooking;
        protected Card PlayerPrison;
        protected Card EnemyPrison;
        protected PseudoZone PlayerPrisoners;
        protected PseudoZone EnemyPrisoners;
        protected Card PlayerWorld = null;
        protected Card EnemyWorld = null;
        public PlayerStates MyStates;
        public PlayerStates EnemyStates;
        public CardStates CardStates;

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
        public event EventHandler<CardEventArgs> OnZoneChanged;
        public event EventHandler<CardEventArgs> OnZoneSwapped;
        public event EventHandler<CardEventArgs> OnFaceUpChanged;
        public event EventHandler<CardEventArgs> OnUpRightChanged;
        public event EventHandler<CardEventArgs> OnCardValueChanged;
        public event EventHandler<CardEventArgs> OnShieldValueChanged;
        public event EventHandler<CardEventArgs> OnAttackEnds;
        public event EventHandler<CardEventArgs> OnReveal;
        public event EventHandler<CardEventArgs> OnSetPrison;
        public event EventHandler<CardEventArgs> OnImprison;

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
                PlayerOrderArea = _field.Player1OrderArea;
                EnemyOrderArea = _field.Player2OrderArea;
                PlayerRideDeck = _field.Player1RideDeck;
                EnemyRideDeck = _field.Player2RideDeck;
                PlayerRevealed = _field.Player1Revealed;
                EnemyRevealed = _field.Player2Revealed;
                PlayerLooking = _field.Player1Looking;
                PlayerPrison = _field.Player1Prison;
                EnemyPrison = _field.Player2Prison;
                PlayerPrisoners = _field.Player1Prisoners;
                EnemyPrisoners = _field.Player2Prisoners;
                MyStates = _field.Player1States;
                EnemyStates = _field.Player2States;
            }
            else
            {
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
                PlayerOrderArea = _field.Player2OrderArea;
                EnemyOrderArea = _field.Player1OrderArea;
                PlayerRideDeck = _field.Player2RideDeck;
                EnemyRideDeck = _field.Player1RideDeck;
                PlayerRevealed = _field.Player2Revealed;
                EnemyRevealed = _field.Player1Revealed;
                PlayerLooking = _field.Player2Looking;
                PlayerPrison = _field.Player2Prison;
                EnemyPrison = _field.Player1Prison;
                PlayerPrisoners = _field.Player2Prisoners;
                EnemyPrisoners = _field.Player1Prisoners;
                MyStates = _field.Player2States;
                EnemyStates = _field.Player1States;
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
            CardStates = _field.CardStates;
            _field.OnZoneChanged += _fieldOnZoneChanged;
            _field.OnZoneSwapped += _fieldOnZoneSwapped;
            _field.Orientation.FaceUpChanged += _fieldOnFaceUpChanged;
            _field.Orientation.UpRightChanged += _fieldOnUpRightChanged;
        }

        public void UpdateRecordedValues()
        {
            for (int i = 0; i <= PlayerVanguard; i++)
            {
                if (_field.GetUnit(i) != null)
                {
                    if (!_recordedCardValues.ContainsKey(i) || 
                        CardValueChanged(_recordedCardValues[i], CalculatePowerOfUnit(i), Critical(_field.GetUnit(i).tempID)))
                    {
                        if (!_recordedCardValues.ContainsKey(i))
                            _recordedCardValues[i] = new RecordedCardValue(CalculatePowerOfUnit(i), Critical(_field.GetUnit(i).tempID));
                        CardEventArgs args = new CardEventArgs();
                        args.circle = i;
                        args.currentPower = _recordedCardValues[i].currentPower;
                        args.currentCritical = _recordedCardValues[i].currentCritical;
                        if (OnCardValueChanged != null)
                            OnCardValueChanged(this, args);
                    }
                }
            }
            foreach (Card attacked in _field.Attacked)
            {
                if (_field.GetUnit(GetCircle(attacked)) == null)
                    continue;
                if ((_recordedShieldValues.ContainsKey(GetCircle(attacked)) && _recordedShieldValues[GetCircle(attacked)] != CalculateShield(attacked.tempID)) ||
                    !_recordedShieldValues.ContainsKey(GetCircle(attacked)))
                {
                    _recordedShieldValues[GetCircle(attacked)] = CalculateShield(attacked.tempID);
                    if (OnShieldValueChanged != null)
                    {
                        CardEventArgs args = new CardEventArgs();
                        args.circle = GetCircle(attacked);
                        args.currentShield = _recordedShieldValues[GetCircle(attacked)];
                        OnShieldValueChanged(this, args);
                    }
                }
            }
        }

        void _fieldOnZoneChanged(object sender, CardEventArgs e)
        {
            if (OnZoneChanged != null)
            {
                OnZoneChanged(this, e);
            }
            UpdateRecordedValues();
        }

        void _fieldOnZoneSwapped(object sender, CardEventArgs e)
        {
            if (OnZoneSwapped != null)
            {
                OnZoneSwapped(this, e);
            }
            UpdateRecordedValues();
        }

        void _fieldOnFaceUpChanged(object sender, CardEventArgs e)
        {
            if (OnFaceUpChanged != null)
            {
                OnFaceUpChanged(this, e);
            }
            UpdateRecordedValues();
        }

        void _fieldOnUpRightChanged(object sender, CardEventArgs e)
        {
            if (OnUpRightChanged != null)
            {
                OnUpRightChanged(this, e);
            }
            UpdateRecordedValues();
        }

        public int Turn
        {
            get => _field.Turn;
        }

        public List<Card> GetHand()
        {
            return PlayerHand.GetCards();
        }

        public List<Card> GetOrderableCards()
        {
            List<Card> hand = PlayerHand.GetCards();
            List<Card> orderableCards = new List<Card>();
            foreach (Card card in hand)
            {
                if (card.orderType >= 0 && card.grade <= _field.GetUnit(PlayerVanguard).grade)
                    orderableCards.Add(card);
            }
            return orderableCards;
        }

        public List<Card> GetAlchemagicTargets()
        {
            List<Card> cards = new List<Card>();
            foreach (Card card in PlayerDrop.GetCards())
            {
                if (card.orderType == 0 && card.name != _playedOrder.name)
                    cards.Add(card);
            }
            return cards;
        }

        public List<Card> GetSoul()
        {
            return _field.GetSoul(PlayerVanguard);
        }

        public List<Card> GetBackRow()
        {
            List<Card> backRow = new List<Card>();
            if (_field.GetUnit(PlayerBackLeft) != null)
                backRow.Add(_field.GetUnit(PlayerBackLeft));
            if (_field.GetUnit(PlayerBackRight) != null)
                backRow.Add(_field.GetUnit(PlayerBackRight));
            if (_field.GetUnit(PlayerBackCenter) != null)
                backRow.Add(_field.GetUnit(PlayerBackCenter));
            return backRow;
        }

        public List<Card> GetGC()
        {
            return _field.GC.GetCards();
        }

        public List<Card> GetOrderZone()
        {
            return PlayerOrder.GetCards();
        }

        public List<Card> GetBind()
        {
            return PlayerBind.GetCards();
        }

        public List<Card> GetRevealed()
        {
            return PlayerRevealed.GetCards();
        }

        public List<Card> GetLooking()
        {
            return PlayerLooking.GetCards();
        }

        public List<Card> GetPlayerOrder()
        {
            return PlayerOrder.GetCards();
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
            return PlayerPrisoners.GetCards();
        }

        public List<Card> GetEnemyPrisoners()
        {
            return EnemyPrisoners.GetCards();
        }
    
        public List<Card> GetDrop()
        {
            return PlayerDrop.GetCards();
        }

        public List<Card> GetDeck()
        {
            return PlayerDeck.GetCards();
        }

        public List<Card> GetLastRidden()
        {
            return new List<Card>(_lastRidden);
        }

        public List<Card> GetLastPlayerRetired()
        {
            return new List<Card>(_lastPlayerRetired);
        }

        public List<Card> GetLastPlayerRCRetired()
        {
            return new List<Card>(_lastPlayerRCRetired);
        }

        public Card GetUnitAt(int circle, bool convert)
        {
            if (convert)
                circle = Convert(circle);
            return _field.GetUnit(circle);
        }

        public int GetCircle(Card card)
        {
            int circle = -1;
            for (circle = 0; circle < FL.MaxFL(); circle++)
            {
                if (_field.GetUnit(circle) != null && _field.GetUnit(circle).tempID == card.tempID)
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
                    if (_field.GetUnit(i) == null)
                        circles.Add(i);
                }
            }
            else
            {
                for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                {
                    if (_field.GetUnit(i) == null)
                        circles.Add(i);
                }
            }
            return circles;
        }

        public List<Card> GetUnitsAtColumn(int column)
        {
            List<Card> cards = new List<Card>();
            for (int i = FL.EnemyFrontLeft; i <= FL.PlayerVanguard; i++)
            {
                if (_field.GetUnit(i) != null && _field.GetColumn(i) == column)
                    cards.Add(_field.GetUnit(i));
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
            if (_field.GetUnit(PlayerFrontLeft) != null &&
                _field.GetUnit(PlayerFrontLeft).tempID == front &&
                _field.GetUnit(PlayerBackLeft) != null &&
                _field.GetUnit(PlayerBackLeft).tempID == behind)
                return true;
            if (_field.GetUnit(PlayerVanguard) != null &&
                _field.GetUnit(PlayerVanguard).tempID == front &&
                _field.GetUnit(PlayerBackCenter) != null &&
                _field.GetUnit(PlayerBackCenter).tempID == behind)
                return true;
            if (_field.GetUnit(PlayerFrontRight) != null &&
                _field.GetUnit(PlayerFrontRight).tempID == front &&
                _field.GetUnit(PlayerBackRight) != null &&
                _field.GetUnit(PlayerBackRight).tempID == behind)
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
                    if (c != null)
                    {
                        if (IsEnemy(c.tempID))
                            inFront.Add(c);
                        else if (c.tempID != tempID && _field.GetRow(GetCircle(c)) == 0)
                            inFront.Add(c);
                    }
                }
            }
            return inFront;
        }

        public int NumEnemyOpenCircles()
        {
            int count = 0;
            for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
            {
                if (_field.GetUnit(i) != null)
                    count++;
            }
            return 5 - count;
        }

        public List<Card> GetRevealedTriggers()
        {
            List<Card> triggers = new List<Card>();
            triggers.AddRange(_lastRevealedDriveChecks);
            triggers.AddRange(_lastRevealedDamageChecks);
            return triggers;
        }

        public List<Card> GetRevealedDamageChecks()
        {
            return _lastRevealedDamageChecks;
        }

        public List<Card> GetRevealedDriveChecks()
        {
            return _lastRevealedDriveChecks;
        }

        public Card GetRevealedTrigger()
        {
            return _lastRevealedTrigger;
        }

        public void Draw(int count)
        {
            List<Card> cardsAdded = new List<Card>();
            CardEventArgs args = new CardEventArgs();
            for (int i = 0; i < count && PlayerDeck.Count() > 0; i++)
            {
                cardsAdded.Add(PlayerHand.Add(PlayerDeck.Index(0)));
            }
            args.i = count;
            args.cardList = cardsAdded;
            args.playerID = _playerID;
            if (OnDraw != null)
            {
                OnDraw(this, args);
            }
            Log.WriteLine("----------\nDrew " + count + " card(s).");
        }

        public void Mill(int count)
        {
            Card milled;
            for (int i = 0; i < count && PlayerDeck.Count() > 0; i++)
            {
                milled = PlayerDrop.Add(PlayerDeck.Index(0));
            }
        }

        public void MulliganCards(List<int> selection)
        {
            foreach (int tempID in selection)
            {
                ReturnCardFromHandToDeck(tempID);
            }
            Draw(selection.Count);
            PlayerDeck.Shuffle();
        }

        public void ReturnCardFromHandToDeck(int selection)
        {
            CardEventArgs args = new CardEventArgs();
            Card card;
            card = PlayerDeck.Add(selection);
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
            Card card;
            _lastDiscarded.Clear();
            foreach (int tempID in list)
            {
                card = PlayerDrop.Add(tempID);
                _lastDiscarded.Add(card);
                args.cardList.Add(card);
            }
            args.playerID = _playerID;
            if (OnDiscard != null)
            {
                OnDiscard(this, args);
            }
        }

        public void Shuffle()
        {
            PlayerDeck.Shuffle();
        }

        public void StandAll()
        {
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.GetUnit(i) != null)
                    _field.Orientation.Rotate(_field.GetUnit(i).tempID, true);
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
            List<Card> hand = PlayerHand.GetCards();
            Log.WriteLine("----------");
            //Log.WriteLine("Choose a card to examine.");
            for (int i = 0; i < hand.Count; i++)
            {
                Log.WriteLine(i + 1 + ". " + hand[i].name);
            }
            //Log.WriteLine(hand.Count + 1 + ". Go back.");
            return hand.Count + 1;
        }

        public int PrintField()
        {
            string output;
            string pstand, estand;
            List<Card> hand = EnemyHand.GetCards();
            List<Card> GC = _field.GC.GetCards();
            if (_field.Orientation.IsUpRight(_field.GetUnit(EnemyVanguard).tempID))
                estand = "S";
            else
                estand = "R";
            if (_field.Orientation.IsUpRight(_field.GetUnit(PlayerVanguard).tempID))
                pstand = "S";
            else
                pstand = "R";
            output = "----------\nEnemy Hand: " + hand.Count + " Enemy Soul: " + _field.GetSoul(EnemyVanguard).Count + " Player Soul: " + _field.GetSoul(PlayerVanguard).Count + " Player Damage: " + PlayerDamage.Count() + " Enemy Damage: " + EnemyDamage.Count() + "\n" +
                "Choose circle to examine.\n" +
                "1. " + PrintRGData(EnemyBackRight) + " | " + "2. " + PrintRGData(EnemyBackCenter) + " | " + "3. " + PrintRGData(EnemyBackLeft) + "\n" +
                "4. " + PrintRGData(EnemyFrontRight) + " | " + "5. " + CalculatePowerOfUnit(EnemyVanguard) + " G" + _field.GetUnit(EnemyVanguard).grade + " " + estand + " | 6. " + PrintRGData(EnemyFrontLeft) + "\n" +
                "7.                 (to-do)\n" +
                "8. " + PrintRGData(PlayerFrontLeft) + " | 9. " + CalculatePowerOfUnit(PlayerVanguard) + " G" + _field.GetUnit(PlayerVanguard).grade + " " + pstand + " | 10. " + PrintRGData(PlayerFrontRight) + "\n" +
                "11. " + PrintRGData(PlayerBackLeft) + " | 12. " + PrintRGData(PlayerBackCenter) + " | 13. " + PrintRGData(PlayerBackRight) + "\n" +
                "14. Display Drop.\n" +
                "15. Display Soul.\n" +
                "16. Display Bind.\n" +
                "17. Go back.";
            Log.WriteLine(output);
            return 17;
        }

        public string PrintRGData(int location)
        {
            string output;
            if (_field.GetUnit(location) != null)
            {
                output = CalculatePowerOfUnit(location) + " G" + _field.GetUnit(location).grade + " ";
                if (_field.Orientation.IsUpRight(_field.GetUnit(location).tempID))
                    output += "S";
                else
                    output += "R";
            }
            else
                output = "--empty--";
            return output;
        }

        public void DisplayCardInHand(int handNumber)
        {
            List<Card> hand = PlayerHand.GetCards();
            DisplayCard(hand[handNumber]);
        }

        public void DisplayCard(Card card)
        {
            if (card == null)
            {
                Log.WriteLine("No card.");
                return;
            }
            string output = card.name + "\n" +
                "Grade: " + card.grade + " Power: " + card.power + " Shield: " + card.shield + " " + card.id + "\n" +
                card.effect;
            Log.WriteLine("----------" + output);
        }

        public void DisplayDrop()
        {
            if (PlayerDrop.Count() == 0)
            {
                Log.WriteLine("----------\nNo cards in Drop.");
                return;
            }
            List<Card> drop = PlayerDrop.GetCards();
            Log.WriteLine("----------");
            for (int i = 0; i < drop.Count; i++)
            {
                Log.WriteLine(i + 1 + ". " + drop[i].name);
            }
        }

        public void DisplaySoul()
        {
            List<Card> soul = _field.GetSoul(PlayerVanguard);
            if (soul.Count == 0)
            {
                Log.WriteLine("----------No cards in Soul.");
                return;
            }
            Log.WriteLine("----------");
            for (int i = 0; i < soul.Count; i++)
            {
                Log.WriteLine(i + 1 + ". " + soul[i].name);
            }
        }

        public void DisplayBind()
        {
            List<Card> bind = PlayerBind.GetCards();
            if (bind.Count == 0)
            {
                Log.WriteLine("----------No cards in Bind.");
                return;
            }
            Log.WriteLine("----------");
            for (int i = 0; i < bind.Count; i++)
            {
                Log.WriteLine(i + 1 + ". " + bind[i].name);
            }
        }

        public void DisplayVanguard(bool player)
        {
            if (player)
                DisplayCard(_field.GetUnit(EnemyVanguard));
            else
                DisplayCard(_field.GetUnit(PlayerVanguard));
        }

        public void DisplayRearguard(int selection)
        {
            if (selection == 1)
                DisplayCard(_field.GetUnit(EnemyBackRight));
            else if (selection == 2)
                DisplayCard(_field.GetUnit(EnemyBackCenter));
            else if (selection == 3)
                DisplayCard(_field.GetUnit(EnemyBackLeft));
            else if (selection == 4)
                DisplayCard(_field.GetUnit(EnemyFrontRight));
            else if (selection == 6)
                DisplayCard(_field.GetUnit(EnemyFrontLeft));
            else if (selection == 8)
                DisplayCard(_field.GetUnit(PlayerFrontLeft));
            else if (selection == 10)
                DisplayCard(_field.GetUnit(PlayerFrontRight));
            else if (selection == 11)
                DisplayCard(_field.GetUnit(PlayerBackLeft));
            else if (selection == 12)
                DisplayCard(_field.GetUnit(PlayerBackCenter));
            else if (selection == 13)
                DisplayCard(_field.GetUnit(PlayerBackRight));
        }

        public List<Card> GetRideableCards(bool rideDeck)
        {
            List<Card> cards = new List<Card>();
            if (rideDeck)
            {
                foreach (Card card in PlayerRideDeck.GetCards())
                {
                    if (!_field.CardStates.HasState(card.tempID, CardState.CannotBeRidden) && card.unitType >= 0 && (card.grade == _field.GetUnit(PlayerVanguard).grade || card.grade == _field.GetUnit(PlayerVanguard).grade + 1))
                        cards.Add(card);
                }
            }
            else
            {
                foreach (Card card in PlayerHand.GetCards())
                {
                    if (!_field.CardStates.HasState(card.tempID, CardState.CannotBeRidden) && card.unitType >= 0 && (card.grade == _field.GetUnit(PlayerVanguard).grade || card.grade == _field.GetUnit(PlayerVanguard).grade + 1))
                        cards.Add(card);
                }
            }
            return cards;
        }

        public List<Card> GetCallableRearguards()
        {
            List<Card> hand = PlayerHand.GetCards();
            List<Card> callableCards = new List<Card>();
            Card VG = _field.GetUnit(PlayerVanguard);
            foreach (Card card in hand)
            {
                if (card.grade <= VG.grade && card.orderType < 0)
                    callableCards.Add(card);
            }
            return callableCards;
        }

        public List<Card> GetMoveableRearguards()
        {
            List<Card> cards = new List<Card>();
            for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
            {
                if (i != PlayerBackCenter && !_field.CardStates.HasState(_field.GetUnit(i).tempID, CardState.CannotMove))
                    cards.Add(_field.GetUnit(i));
            }
            return cards;
        }

        public bool CheckColumn(int column) //0 is left, 1 is right
        {
            if (column == 0)
            {
                if (_field.GetUnit(PlayerFrontLeft) != null || _field.GetUnit(PlayerBackLeft) != null)
                    return true;
                return false;
            }
            else
            {
                if (_field.GetUnit(PlayerFrontRight) != null || _field.GetUnit(PlayerBackRight) != null)
                    return true;
                return false;
            }
        }

        public List<Card> GetCardsToAttackWith()
        {
            List<Card> cards = new List<Card>();
            Card card;
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                card = _field.GetUnit(i);
                if (card != null && _field.Orientation.IsUpRight(card.tempID) && !_field.CardStates.HasState(card.tempID, CardState.CannotAttack))
                {
                    if (_field.GetRow(i) == 0 || _field.CardStates.HasState(card.tempID, CardState.CanAttackFromBackRow))
                        cards.Add(card);
                }
            }
            return cards;
        }

        public List<Card> GetPotentialAttackTargets()
        {
            List<Card> cards = new List<Card>();
            Card Attacker = _field.GetUnit(_field.Attacker);
            for (int i = EnemyFrontLeft; i <= EnemyVanguard; i++)
            {
                if (i == EnemyVanguard && _field.CardStates.HasState(Attacker.tempID, CardState.CannotAttackVanguard))
                    continue;
                if (_field.GetUnit(i) != null && 
                    !CardStates.GetValues(Attacker.tempID, CardState.CannotAttackUnit).Contains(_field.GetUnit(i).tempID) && 
                    (_field.GetRow(i) == 0 || _field.CardStates.HasState(Attacker.tempID, CardState.CanAttackBackRow) || _field.CardStates.HasState(Attacker.tempID, CardState.CanColumnAttack)))
                    cards.Add(_field.GetUnit(i));
            }
            return cards;
        }

        public List<Card> GetEnemyFrontRowRearguards()
        {
            List<Card> cards = new List<Card>();
            if (_field.GetUnit(EnemyFrontLeft) != null)
                cards.Add(_field.GetUnit(EnemyFrontLeft));
            if (_field.GetUnit(EnemyFrontRight) != null)
                cards.Add(_field.GetUnit(EnemyFrontRight));
            return cards;
        }

        public List<Card> GetEnemyBackRowRearguards()
        {
            List<Card> cards = new List<Card>();
            if (_field.GetUnit(EnemyBackLeft) != null)
                cards.Add(_field.GetUnit(EnemyBackLeft));
            if (_field.GetUnit(EnemyBackCenter) != null)
                cards.Add(_field.GetUnit(EnemyBackCenter));
            if (_field.GetUnit(EnemyBackRight) != null)
                cards.Add(_field.GetUnit(EnemyBackRight));
            return cards;
        }

        public List<Card> GetPlayerFrontRow()
        {
            List<Card> cards = new List<Card>();
            if (_field.GetUnit(PlayerFrontLeft) != null)
                cards.Add(_field.GetUnit(PlayerFrontLeft));
            cards.Add(_field.GetUnit(PlayerVanguard));
            if (_field.GetUnit(PlayerFrontRight) != null)
                cards.Add(_field.GetUnit(PlayerFrontRight));
            return cards;
        }

        public List<Card> GetGuardableCards()
        {
            List<Card> cards = new List<Card>();
            if (!MyStates.HasState(PlayerState.CannotGuardFromHand))
            {
                foreach (Card card in PlayerHand.GetCards())
                {
                    if (card.orderType < 0 && !_field.CardStates.HasState(card.tempID, CardState.CanOnlyBeCalledToBackRowCenter) &&
                        (MyStates.GetValue(PlayerState.MinGradeForGuard) == -1 || card.grade >= MyStates.GetValue(PlayerState.MinGradeForGuard)) &&
                        MyStates.GetValue(PlayerState.CannotGuardWithUnitType) != card.unitType)
                        cards.Add(card);
                }
            }
            return cards;
        }

        public List<Card> GetInterceptableCards()
        {
            List<Card> cards = new List<Card>();
            for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
            {
                if (CanIntercept(_field.GetUnit(i)))
                    cards.Add(_field.GetUnit(i));
            }
            return cards;
        }

        public bool CanIntercept(Card card)
        {
            if (!MyStates.HasState(PlayerState.CannotIntercept) && card != null && !_field.Attacked.Contains(card) && 
                (card.skill == Skill.Intercept || _field.CardStates.GetValues(card.tempID, CardState.BonusSkills).Contains(Skill.Intercept)) && 
                (_field.GetRow(GetCircle(card)) == 0 || _field.CardStates.HasState(card.tempID, CardState.CanInterceptFromBackRow)))
                return true;
            return false;
        }

        public void DisableIntercept()
        {
            MyStates.AddUntilEndOfBattleState(PlayerState.CannotIntercept);
        }

        public List<Card> GetActiveUnits()
        {
            List<Card> cards = new List<Card>();
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.GetUnit(i) != null)
                    cards.Add(_field.GetUnit(i));
            }
            return cards;
        }

        public List<Card> GetAllUnitsOnField()
        {
            List<Card> cards = new List<Card>();
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.GetUnit(i) != null)
                    cards.Add(_field.GetUnit(i));
            }
            for (int i = EnemyFrontLeft; i <= EnemyVanguard; i++)
            {
                if (_field.GetUnit(i) != null)
                    cards.Add(_field.GetUnit(i));
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
                    if (_field.GetUnit(i) != null)
                        rearguards.Add(_field.GetUnit(i));
                }
            }
            else
            {
                for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                {
                    if (_field.GetUnit(i) != null)
                        rearguards.Add(_field.GetUnit(i));
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
            return PlayerDamage.GetCards();
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
            int shield = CalculatePowerOfUnit(GetCircle(_field.CardCatalog[tempID]));
            if (_field.Sentinel.Contains(attacked) || (_field.GetUnit(_field.Attacker) != null && _field.CardStates.GetValues(attacked.tempID, CardState.CannotBeHitByGrade).Contains(_field.GetUnit(_field.Attacker).grade)))
                return 1000000000;
            if (!_field.Guardians.ContainsKey(tempID))
                return shield;
            guardians = _field.Guardians[tempID];
            if (guardians == null)
                return shield;
            foreach (Card card in guardians)
            {
                if (_field.GC.Contains(card))
                {
                    shield += card.shield;
                    foreach (int value in _field.CardStates.GetValues(card.tempID, CardState.BonusShield))
                        shield += value;
                }
            }
            return shield;
        }

        public void PrintShield()
        {
            foreach (Card card in _field.Attacked)
            {
                if (_field.Sentinel.Contains(_field.CardCatalog[card.tempID]) || _field.CardStates.GetValues(card.tempID, CardState.CannotBeHitByGrade).Contains(_field.GetUnit(_field.Attacker).grade))
                {
                    Log.WriteLine("[" + card.name + "] Hit immunity active.");
                    return;
                }
                Log.WriteLine("[" + card.name + "] Shield: " + CalculateShield(card.tempID));
            }
        }

        public void PrintAttack()
        {
            Log.WriteLine("Attacking for " + CalculatePowerOfUnit(_field.Attacker) + ".");
        }

        public void PrintEnemyAttack()
        {
            Log.WriteLine("Enemy attacking for " + CalculatePowerOfUnit(_field.Attacker) + ".");
        }

        public int CalculatePowerOfUnit(int location)
        {
            Card card = _field.GetUnit(location);
            int power = card.power;
            foreach (int value in _field.CardStates.GetValues(card.tempID, CardState.BonusPower))
                power += value;
            power += _field.CirclePower[location];
            if (IsFrontRow(location) && IsPlayer(location) && _field.GetPersonaRide(_playerID))
                power += 10000;
            else if (IsFrontRow(location) && !IsPlayer(location) && _field.GetPersonaRide(_enemyID))
                power += 10000;
            if (_field.Booster >= 0 && location == _field.Attacker)
            {
                if (_field.GetUnit(_field.Booster) == null)
                    _field.Booster = -1;
                else
                    power += CalculatePowerOfUnit(_field.Booster);
            }
            return power;
        }

        public int PrintDamageZone()
        {
            int i = 0;
            Log.WriteLine("Choose damage to heal.");
            string output;
            List<Card> damage = PlayerDamage.GetCards();
            for (i = 0; i < damage.Count; i++)
            {
                output = i + 1 + ". " + damage[i].name + " ";
                if (!_field.Orientation.IsFaceUp(damage[i].tempID))
                    Log.WriteLine(output + "(facedown).");
                else
                    Log.WriteLine(output + "(faceup).");
            }
            return i + 1;
        }

        public bool CanRideFromRideDeck()
        {
            if (_riddenThisTurn)
                return false;
            List<Card> rideDeck = PlayerRideDeck.GetCards();
            Card VG = _field.GetUnit(PlayerVanguard);
            if (PlayerHand.GetCards().Count < 1)
                return false;
            foreach (Card card in rideDeck)
            {
                if (VG.grade + 1 == card.grade &&
                    !_field.CardStates.HasState(card.tempID, CardState.CannotBeRidden))
                    return true;
            }
            return false;
        }

        public bool CanRideFromHand()
        {
            if (_riddenThisTurn)
                return false;
            List<Card> hand = PlayerHand.GetCards();
            Card VG = _field.GetUnit(PlayerVanguard);
            foreach (Card card in hand)
            {
                if (card.unitType >= 0 && (card.grade == VG.grade || card.grade - 1 == VG.grade) &&
                    !_field.CardStates.HasState(card.tempID, CardState.CannotBeRidden))
                    return true;
            }
            return false;
        }

        public bool CanCallRearguard()
        {
            List<Card> hand = PlayerHand.GetCards();
            Card VG = _field.GetUnit(PlayerVanguard);
            foreach (Card card in hand)
            {
                if (card.grade <= VG.grade && card.orderType < 0)
                    return true;
            }
            return false;
        }

        public bool CanMoveRearguard()
        {
            if (MyStates.HasState(PlayerState.CannotMove))
                return false;
            for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
            {
                if (_field.GetUnit(i) != null)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanAttack()
        {
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.GetUnit(i) != null)
                {
                    if (i == PlayerFrontLeft || i == PlayerFrontRight || i == PlayerVanguard)
                    {
                        if (_field.Orientation.IsUpRight(_field.GetUnit(i).tempID))
                            return true;
                    }
                    else if (_field.CardStates.HasState(_field.GetUnit(i).tempID, CardState.CanAttackFromBackRow))
                    {
                        if (_field.Orientation.IsUpRight(_field.GetUnit(i).tempID))
                            return true;
                    }
                }
            }
            return false;
        }

        public bool CanBeBoosted()
        {
            if (_field.Attacker == PlayerFrontLeft)
            {
                if (_field.GetUnit(PlayerBackLeft) != null && CanBoost(_field.GetUnit(PlayerBackLeft)))
                    return true;
            }
            else if (_field.Attacker == PlayerVanguard)
            {
                if (_field.GetUnit(PlayerBackCenter) != null && CanBoost(_field.GetUnit(PlayerBackCenter)))
                    return true;
            }
            else if (_field.Attacker == PlayerFrontRight)
            {
                if (_field.GetUnit(PlayerBackRight) != null && CanBoost(_field.GetUnit(PlayerBackRight)))
                    return true;
            }
            return false;
        }

        public bool CanBoost(Card card)
        {
            if (MyStates.HasState(PlayerState.CannotBoost))
                return false;
            if (IsUpRight(card))
            {
                if (card.skill == 0 || _field.CardStates.GetValues(card.tempID, CardState.BonusSkills).Contains(Skill.Boost))
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
            if (PlayerDamage.Count() > 0 && PlayerDamage.Count() >= EnemyDamage.Count())
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
            if (_field.Attacked.Contains(_field.GetUnit(Vanguard)))
                return true;
            return false;
        }

        public bool TargetIsRearguard(bool player)
        {
            if (player)
            {
                for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
                {
                    if (_field.Attacked.Contains(_field.GetUnit(i)))
                        return true;
                }
            }
            else
            {
                for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                {
                    if (_field.Attacked.Contains(_field.GetUnit(i)))
                        return true;
                }
            }
            return false;
        }

        public bool VanguardHit(bool player)
        {
            if (player && _field.UnitsHit.Contains(_field.GetUnit(PlayerVanguard)))
                return true;
            if (!player && _field.UnitsHit.Contains(_field.GetUnit(EnemyVanguard)))
                return true;
            return false;
        }

        public bool IsRearguard(int tempID)
        {
            for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
            {
                if (_field.GetUnit(i) != null && _field.GetUnit(i).tempID == tempID)
                    return true;
            }
            for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
            {
                if (_field.GetUnit(i) != null && _field.GetUnit(i).tempID == tempID)
                    return true;
            }
            return false;
        }

        public List<Card> GetLastPlacedOnGC()
        {
            return new List<Card>(_lastPlacedOnGC);
        }

        public List<Card> GetLastPutOnGC()
        {
            return new List<Card>(_lastPutOnGC);
        }

        public List<Card> GetLastPlacedOnRCFromHand()
        {
            return new List<Card>(_lastPlacedOnRCFromHand);
        }

        public List<Card> GetLastPlacedOnRCFromPrison()
        {
            return new List<Card>(_lastPlacedOnRCFromPrison);
        }

        public List<Card> GetLastPlacedOnRCOtherThanFromHand()
        {
            return new List<Card>(_lastPlacedOnRCOtherThanFromHand);
        }

        public List<Card> GetUnitsCalledThisTurn()
        {
            return new List<Card>(_unitsCalledThisTurn);
        }

        public List<Card> GetUnitsCalledFromHandThisTurn()
        {
            return new List<Card>(_unitsCalledFromHandThisTurn);
        }

        public List<Card> GetLastPlacedOnRC()
        {
            return new List<Card>(_lastPlacedOnRC);
        }

        public List<Card> GetLastPlacedOnVC()
        {
            return new List<Card>(_lastPlacedOnVC);
        }

        public List<Card> GetLastStood()
        {
            return new List<Card>(_stoodByCardEffect);
        }

        public List<Card> GetLastDiscarded()
        {
            return new List<Card>(_lastDiscarded);
        }

        public int Drive()
        {
            Card attacker = _field.GetUnit(_field.Attacker);
            int drive = 0;
            foreach (int value in _field.CardStates.GetValues(attacker.tempID, CardState.BonusDrive))
                drive += value;
            if (attacker.skill == Skill.TripleDrive || _field.CardStates.GetValues(attacker.tempID, CardState.BonusSkills).Contains(Skill.TripleDrive))
                drive += 3;
            else if (attacker.skill == Skill.TwinDrive || _field.CardStates.GetValues(attacker.tempID, CardState.BonusSkills).Contains(Skill.TwinDrive))
                drive += 2;
            else
                drive += 1;
            return drive;
        }

        public int Critical()
        {
            Card card = _field.GetUnit(_field.Attacker);
            int critical = card.critical;
            foreach (int value in _field.CardStates.GetValues(card.tempID, CardState.BonusCritical))
                critical += value;
            critical += _field.CircleCritical[GetCircle(card)];
            return critical;
        }

        public int Critical(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            int critical = card.critical;
            foreach (int value in _field.CardStates.GetValues(card.tempID, CardState.BonusCritical))
                critical += value;
            return critical;
        }

        public int Damage()
        {
            return PlayerDamage.Count(); 
        }

        public Card GetTrigger(bool player)
        {
            if (player)
                return PlayerTrigger.Index(0);
            else
                return EnemyTrigger.Index(0);
        }

        public bool StillAttacking()
        {
            if (_field.GetUnit(_field.Attacker) != null)
                return true;
            return false;
        }

        public bool AttackHits()
        {
            Card attacker = _field.GetUnit(_field.Attacker);
            CardEventArgs args = new CardEventArgs();
            foreach (Card card in _field.Attacked)
            {
                if (!GetAllUnitsOnField().Contains(card))
                    return false;
                if (CalculatePowerOfUnit(_field.Attacker) >= CalculateShield(card.tempID))
                {
                    _field.UnitsHit.Add(card);
                    Log.WriteLine("----------\n" + attacker.name + "'s attack makes a hit on " + card.name + "!");
                    if (OnAttackHits != null)
                    {
                        args.b = true;
                        OnAttackHits(this, args);
                    }
                }
                else
                {
                    Log.WriteLine("----------\n" + attacker.name + "'s attack against " + card.name + " was successfully guarded!");
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

        public void Ride(int selection)
        {
            Card card = _field.CardCatalog[selection];
            _lastRidden.Clear();
            _lastPlacedOnVC.Clear();
            _lastRidden.Add(_field.GetUnit(PlayerVanguard));
            bool personaRide = false;
            if (card.name == _field.GetUnit(PlayerVanguard).name && _field.GetUnit(PlayerVanguard).personaRide == 1)
            {
                personaRide = true;
            }
            _field.RideUnit(PlayerVanguard, card);
            if (personaRide)
            {
                Log.WriteLine("---------\nPersona Ride!! " + _field.GetUnit(PlayerVanguard).name + "!");
                _field.SetPersonaRide(true, _playerID);
                Draw(1);
            }
            else
                Log.WriteLine("---------\nRide!! " + _field.GetUnit(PlayerVanguard).name + "!");
            _lastPlacedOnVC.Add(card);
            _riddenThisTurn = true;
        }

        public void Call(int location, int selection, bool overDress)
        {
            SuperiorCall(location, selection, overDress, true);
            _field.ClearOverloadedCards();
        }

        public int SuperiorCall(int circle, int tempID, bool overDress, bool standing)
        {
            bool fromHand = false;
            bool fromPrison = false;
            Card ToBeCalled = _field.CardCatalog[tempID];
            if (EnemyPrisoners.Contains(_field.CardCatalog[tempID]))
                fromPrison = true;
            if (_field.CardLocations[tempID] == PlayerHand)
                fromHand = true;
            if (ToBeCalled.orderType >= 0)
            {
                PlayerDrop.Add(ToBeCalled);
                return -1;
            }
            else
            {
                if (!overDress)
                {
                    _field.SetUnit(circle, ToBeCalled);
                }
                else
                {
                    _field.RideUnit(circle, ToBeCalled);
                }
                if (!standing)
                    _field.Orientation.SetUpRight(ToBeCalled.tempID, false);
            }
            if (overDress)
                Log.WriteLine("----------\nSuperior overDress! " + ToBeCalled.name + "!");
            else
                Log.WriteLine("----------\nSuperior Call! " + ToBeCalled.name + "!");
            _lastPlacedOnRC.Add(ToBeCalled);
            _unitsCalledThisTurn.Add(ToBeCalled);
            if (fromHand)
            {
                _unitsCalledFromHandThisTurn.Add(ToBeCalled);
                _lastPlacedOnRCFromHand.Add(ToBeCalled);
                return 1;
            }
            else
                _lastPlacedOnRCOtherThanFromHand.Add(ToBeCalled);
            if (fromPrison)
            {
                _lastPlacedOnRCFromPrison.Add(ToBeCalled);
                return 2;
            }
            return 0;
        }

        public void DoneCalling()
        {
            _lastPlacedOnRC.Clear();
            _lastPlacedOnRCFromHand.Clear();
            _lastPlacedOnRCFromPrison.Clear();
            _lastPlacedOnRCOtherThanFromHand.Clear();
        }

        public int ClearOverloadedCards()
        {
            bool cleared = _field.ClearOverloadedCards();
            if (cleared)
            {
                _playerRetiredThisTurn = true;
                return 1;
            }
            return 0;
        }

        public void MoveRearguard(int location)
        {
            Card temp;
            if (location == 0)
            {
                if ((_field.GetUnit(PlayerFrontLeft) != null && _field.CardStates.HasState(_field.GetUnit(PlayerFrontLeft).tempID, CardState.CannotMove)) || (_field.GetUnit(PlayerBackLeft) != null && _field.CardStates.HasState(_field.GetUnit(PlayerBackLeft).tempID, CardState.CannotMove)))
                    return;
                if (_field.GetUnit(PlayerBackLeft) != null && _field.CardStates.HasState(_field.GetUnit(PlayerBackLeft).tempID, CardState.CannotMoveToFrontRow))
                    return;
                temp = _field.GetUnit(PlayerFrontLeft);
                _field.SwapUnits(PlayerFrontLeft, PlayerBackLeft);
                Log.WriteLine("----------\nLeft column Rearguard(s) changed position.");
            }
            else
            {
                if ((_field.GetUnit(PlayerFrontRight) != null && _field.CardStates.HasState(_field.GetUnit(PlayerFrontRight).tempID, CardState.CannotMove)) || (_field.GetUnit(PlayerBackRight) != null && _field.CardStates.HasState(_field.GetUnit(PlayerBackRight).tempID, CardState.CannotMove)))
                    return;
                if (_field.GetUnit(PlayerBackRight) != null && _field.CardStates.HasState(_field.GetUnit(PlayerBackRight).tempID, CardState.CannotMoveToFrontRow))
                    return;
                _field.SwapUnits(PlayerFrontRight, PlayerBackRight);
                Log.WriteLine("----------\nRight column Rearguard(s) changed position.");
            }
            if (OnChangeColumn != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.playerID = _playerID;
                args.i = location;
                OnChangeColumn(this, args);
            }
        }

        public void AlternateMoveRearguard(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (GetCircle(card) == PlayerFrontLeft || GetCircle(card) == PlayerBackLeft)
                _field.SwapUnits(PlayerFrontLeft, PlayerBackLeft);
            else if (GetCircle(card) == PlayerFrontRight || GetCircle(card) == PlayerBackRight)
                _field.SwapUnits(PlayerFrontRight, PlayerBackRight);
        }

        public void MoveRearguard(int location, int direction)
        {
            if (!MyStates.HasState(PlayerState.CannotMove))
                return;
            if (_field.CardStates.HasState(location, CardState.CannotMove))
                return;
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
                {
                    if (_field.CardStates.HasState(location, CardState.CannotMoveToFrontRow))
                        return;
                    location2 = PlayerFrontLeft;
                }
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
                {
                    if (_field.CardStates.HasState(location, CardState.CannotMoveToFrontRow))
                        return;
                    location2 = PlayerFrontRight;
                }
                else if (direction == Direction.Left)
                    location2 = PlayerBackCenter;
            }
            _field.SwapUnits(location1, location2);
        }

        public void MoveRearguardSpecific(int tempID, int location)
        {
            if (!MyStates.HasState(PlayerState.CannotMove))
                return;
            if ((location == PlayerFrontLeft || location == PlayerFrontRight) &&
                _field.CardStates.HasState(tempID, CardState.CannotMoveToFrontRow))
                return;
            if (!_field.CardStates.HasState(tempID, CardState.CannotMove))
            {
                _field.SwapUnits(GetCircle(_field.CardCatalog[tempID]), location);
            }
        }

        //public void MoveRearguardFreeSwap(int tempID, int circle)
        //{
        //    if (!IsPlayer(circle))
        //        return;
        //    if (_field.CardStates.HasState(tempID, CardState.CannotMove) || (_field.GetUnit(circle) != null && _field.CardStates.HasState(_field.GetUnit(circle).tempID, CardState.CannotMove)))
        //        return;
        //    int row = _field.GetRow(circle);
        //    int column = _field.GetColumn(circle);
        //    Card card = _field.CardCatalog[tempID];
        //    if (card == null)
        //        return;
        //    if (circle == PlayerVanguard || GetCircle(card) == PlayerVanguard)
        //        return;
        //    if (!((Math.Abs(row - _field.GetRow(GetCircle(card))) == 1 && Math.Abs(column - _field.GetColumn(GetCircle(card))) == 0) ||
        //        (Math.Abs(row - _field.GetRow(GetCircle(card))) == 0 && Math.Abs(column - _field.GetColumn(GetCircle(card))) == 1)))
        //        return;
        //    if (row == 0 && _field.CardStates.HasState(tempID, CardState.CannotMoveToFrontRow))
        //        return;
        //    if (_field.GetUnit(circle) != null && _field.GetRow(GetCircle(card)) == 0 && _field.CardStates.HasState(tempID, CardState.CannotMoveToFrontRow))
        //        return;
        //    _field.SwapUnits(circle, GetCircle(card));
        //}

        public void MoveRearguardFreeSwap(int tempID, int circle)
        {
            _field.SwapUnits(GetCircle(_field.CardCatalog[tempID]), circle);
        }

        public List<int> GetCirclesForFreeSwap(int tempID)
        {
            List<int> availableCircles = new List<int>();
            Card card = _field.CardCatalog[tempID];
            if (_field.CardStates.HasState(tempID, CardState.CannotMove))
                return availableCircles;
            for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
            {
                if (GetCircle(card) != i && 
                    ((Math.Abs(_field.GetRow(GetCircle(card)) - _field.GetRow(i)) == 1 && Math.Abs(_field.GetColumn(GetCircle(card)) - _field.GetColumn(i)) == 0) ||
                    (Math.Abs(_field.GetRow(GetCircle(card)) - _field.GetRow(i)) == 0 && Math.Abs(_field.GetColumn(GetCircle(card)) - _field.GetColumn(i)) == 1)))
                {
                    if (!((_field.GetRow(i) == 0 && _field.CardStates.HasState(tempID, CardState.CannotMoveToFrontRow)) ||
                        (_field.GetRow(GetCircle(card)) == 0 && _field.GetUnit(i) != null && _field.CardStates.HasState(_field.GetUnit(i).tempID, CardState.CannotMoveToFrontRow)) ||
                        (_field.GetUnit(i) != null && _field.CardStates.HasState(_field.GetUnit(i).tempID, CardState.CannotMove))))
                        availableCircles.Add(i);
                }
            }
            return availableCircles;
        }

        public int GetBooster(int attacker)
        {
            if (GetCircle(_field.CardCatalog[attacker]) == PlayerFrontLeft)
            {
                return PlayerBackLeft;
            }
            else if (GetCircle(_field.CardCatalog[attacker]) == PlayerVanguard)
            {
                return PlayerBackCenter;
            }
            else if (GetCircle(_field.CardCatalog[attacker]) == PlayerFrontRight)
            {
                return PlayerBackRight;
            }
            return -1;
        }

        public void SetAttacker(int tempID)
        {
            _field.Attacker = GetCircle(_field.CardCatalog[tempID]);
        }

        public void InitiateAttack(int booster, params int[] targets)
        {
            Card Attacker = _field.GetUnit(_field.Attacker);
            Card Attacked;
            _field.Orientation.Rotate(Attacker.tempID, false);
            _field.Attacker = GetCircle(Attacker);
            if (booster >= 0)
            {
                _field.Booster = booster;
                _field.Orientation.Rotate(_field.GetUnit(_field.Booster).tempID, false);
            }
            if (targets.Length > 1)
            {
                foreach (int target in targets)
                    _field.Attacked.Add(_field.CardCatalog[target]);
            }
            else
            {
                Attacked = _field.CardCatalog[targets[0]];
                int circle = GetCircle(Attacked);
                _field.Attacked.Add(Attacked);
                if (_field.CardStates.HasState(Attacker.tempID, CardState.CanColumnAttack))
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
                    if (_field.GetUnit(circle) != null && !_field.Attacked.Contains(_field.GetUnit(circle)))
                        _field.Attacked.Add(_field.GetUnit(circle));
                }
                if (_field.CardStates.HasState(Attacker.tempID, CardState.CanAttackAllFrontRow))
                {
                    if (GetCircle(Attacked) == EnemyFrontLeft || GetCircle(Attacked) == EnemyVanguard || GetCircle(Attacked) == EnemyFrontRight)
                    {
                        if (_field.GetUnit(EnemyFrontLeft) != null && !_field.Attacked.Contains(_field.GetUnit(EnemyFrontLeft)))
                            _field.Attacked.Add(_field.GetUnit(EnemyFrontLeft));
                        if (_field.GetUnit(EnemyVanguard) != null && !_field.Attacked.Contains(_field.GetUnit(EnemyVanguard)))
                            _field.Attacked.Add(_field.GetUnit(EnemyVanguard));
                        if (_field.GetUnit(EnemyFrontRight) != null && !_field.Attacked.Contains(_field.GetUnit(EnemyFrontRight)))
                            _field.Attacked.Add(_field.GetUnit(EnemyFrontRight));
                    }
                }
            }
            Log.WriteLine("----------");
            foreach (Card card in _field.Attacked)
                Log.WriteLine(Attacker.name + " attacks " + card.name + "!");
            if (OnAttack != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.playerID = _playerID;
                args.card = Attacker;
                foreach (Card attacked in _field.Attacked)
                    args.intList.Add(attacked.tempID);
                args.currentPower = CalculatePowerOfUnit(_field.Attacker);
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
            return _field.GetUnit(_field.Attacker);
        }

        public bool Guard(List<int> selections, int target)
        {
            Card card;
            bool intercept = false;
            _lastPlacedOnGC.Clear();
            _lastPutOnGC.Clear();
            foreach (int selection in selections)
            {
                card = _field.CardCatalog[selection];
                if (target == -1)
                    target = _field.Attacked[0].tempID;
                if (!_field.Guardians.ContainsKey(target))
                    _field.Guardians[target] = new List<Card>();
                _field.Guardians[target].Add(card);
                if (IsRearguard(selection))
                {
                    intercept = true;
                    _isIntercepting.Add(card);
                }
                _field.GC.Add(card);
                _lastPutOnGC.Add(card);
                if (!intercept)
                {
                    _lastPlacedOnGC.Add(card);
                }
                Log.WriteLine("----------\nAdded " + card.name + " to Guardian Circle.");
                //if (OnGuard != null)
                //{
                //    CardEventArgs args = new CardEventArgs();
                //    args.playerID = _playerID;
                //    args.card = card;
                //    OnGuard(this, args);
                //}
            }
            return intercept;
        }

        public void GuardWithTwo()
        {
            MyStates.AddUntilEndOfBattleState(PlayerState.GuardWithTwo);
        }

        public bool MustGuardWithTwo()
        {
            return MyStates.HasState(PlayerState.GuardWithTwo);
        }

        public void CannotGuardFromHand()
        {
            MyStates.AddUntilEndOfBattleState(PlayerState.CannotGuardFromHand);
        }

        public bool CanGuardFromHand()
        {
            return MyStates.HasState(PlayerState.CannotGuardFromHand);
        }

        public void SetMinGradeForGuard(int min)
        {
            MyStates.AddUntilEndOfBattleValue(PlayerState.MinGradeForGuard, min);
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
                    _field.CardStates.AddUntilEndOfBattleValue(tempID, CardState.CannotBeHitByGrade, grade);
                }
            }
        }

        public bool CannotBeHitByGrade(int tempID, int grade)
        {
            Card card = _field.CardCatalog[tempID];
            if (_field.CardStates.GetValues(tempID, CardState.CannotBeHitByGrade).Contains(grade))
                return true;
            return false;
        }

        public void Resist(int tempID)
        {
            _field.CardStates.AddContinuousState(tempID, CardState.Resist);
        }

        public void CannotMove()
        {
            MyStates.AddUntilEndOfTurnState(PlayerState.CannotMove);
        }

        public void CannotBoost()
        {
            MyStates.AddUntilEndOfTurnState(PlayerState.CannotBoost);
        }

        public void Bind(List<int> tempIDs)
        {
            Card card;
            foreach (int tempID in tempIDs)
            {
                PlayerBind.Add(tempID);
            }
        }

        public void CountsAsTwoRetires(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (GetActiveUnits().Contains(card))
            {
                _field.CardStates.AddContinuousState(card.tempID, CardState.CountsAsTwoRetires);
            }
        }

        public bool CanCountAsTwoRetires(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            if (GetActiveUnits().Contains(card) && _field.CardStates.HasState(card.tempID, CardState.CountsAsTwoRetires))
            {
                return true;
            }
            return false;
        }

        public void Retire(List<int> tempIDs)
        {
            _lastPlayerRetired.Clear();
            _lastPlayerRCRetired.Clear();
            _playerRetired = false;
            _enemyRetired = false;
            foreach (int tempID in tempIDs)
            {
                for (int i = EnemyFrontLeft; i <= EnemyVanguard; i++)
                {
                    if (_field.GetUnit(i) != null && _field.GetUnit(i).tempID == tempID)
                    {
                        _field.RemoveUnit(i, EnemyDrop);
                        _enemyRetired = true;
                        _enemyRetiredThisTurn = true;
                        break;
                    }
                }
                for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
                {
                    if (_field.GetUnit(i) != null && _field.GetUnit(i).tempID == tempID)
                    {
                        _lastPlayerRetired.Add(_field.GetUnit(i));
                        if (i != PlayerVanguard)
                            _lastPlayerRCRetired.Add(_field.GetUnit(i));
                        _field.RemoveUnit(i, PlayerDrop);
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
                if (card != _field.GetUnit(PlayerVanguard))
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

        public int RetireGC()
        {
            bool retired = false;
            _isIntercepting.Clear();
            foreach (Card card in _field.GC.GetCards())
            {
                retired = true;
                PlayerDrop.Add(card);
            }
            if (retired)
                return 1;
            return 0;
        }

        public bool IsIntercepting(int tempID)
        {
            if (_isIntercepting.Contains(_field.CardCatalog[tempID]))
                return true;
            return false;
        }

        public bool SendToDeck(List<int> cardsToSend, bool bottom)
        {
            Card card;
            Zone deck;
            Zone hand;
            Zone drop;
            List<Card> leftover = new List<Card>();
            foreach (int tempID in cardsToSend)
            {
                card = _field.CardCatalog[tempID];
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
                if (bottom)
                    deck.Add(card);
                else
                    deck.AddToTop(card);
            }
            return true;
        }

        public int TriggerCheck(bool drivecheck)
        {
            Card trigger = PlayerTrigger.Add(PlayerDeck.Index(0));
            if (drivecheck)
                _lastRevealedDriveChecks.Add(trigger);
            else
                _lastRevealedDamageChecks.Add(trigger);
            _lastRevealedTrigger = trigger;
            if (trigger.trigger == Trigger.Critical)
                Log.WriteLine("----------\nCritical Trigger!");
            else if (trigger.trigger == Trigger.Draw)
                Log.WriteLine("----------\nDraw Trigger!");
            else if (trigger.trigger == Trigger.Front)
                Log.WriteLine("----------\nFront Trigger!");
            else if (trigger.trigger == Trigger.Stand)
                Log.WriteLine("----------\nStand Trigger!");
            else if (trigger.trigger == Trigger.Heal)
                Log.WriteLine("----------\nHeal Trigger!");
            else if (trigger.trigger == Trigger.Over)
                Log.WriteLine("----------\nOver Trigger!");
            else
                Log.WriteLine("----------\nNo Trigger.");
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

        public void AddTempPower(int selection, int power, bool battleOnly)
        {
            Card target = _field.CardCatalog[selection];
            if (!GetAllUnitsOnField().Contains(target))
                return;
            if (battleOnly)
                _field.CardStates.AddUntilEndOfBattleValue(target.tempID, CardState.BonusPower, power);
            else
                _field.CardStates.AddUntilEndOfTurnValue(target.tempID, CardState.BonusPower, power);
            Log.WriteLine("----------\n" + power + " power to " + target.name + "!");
            UpdateRecordedValues();
        }

        public void AddTempPower(List<int> selections, int power)
        {
            foreach (int tempID in selections)
            {
                _field.CardStates.AddUntilEndOfTurnValue(tempID, CardState.BonusPower, power);
                Log.WriteLine("----------\n" + power + " power to " + _field.CardCatalog[tempID].name + "!");
            }
            UpdateRecordedValues();
        }

        public void AddBonusDriveCheckPower(int power)
        {
            _bonusDriveCheckPower += power;
        }

        public int GetBonusDriveCheckPower()
        {
            return _bonusDriveCheckPower;
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
            _field.CardStates.AddUntilEndOfTurnValue(target.tempID, CardState.BonusCritical, critical);
            Log.WriteLine("----------\n+" + critical + " critical to " + target.name + "!");
            UpdateRecordedValues();
        }

        public void AddBattleOnlyCritical(int selection, int critical)
        {
            Card target = FindActiveUnit(selection);
            _field.CardStates.AddUntilEndOfBattleValue(target.tempID, CardState.BonusCritical, critical);
            Log.WriteLine("----------\n+" + critical + " critical to " + target.name + "!");
            UpdateRecordedValues();
        }

        public void DoublePower(int selection)
        {
            Card target = _field.CardCatalog[selection];
            int currentPower = CalculatePowerOfUnit(GetCircle(target));
            if (_field.Booster >= 0 && GetCircle(target) == _field.Attacker)
            {
                currentPower -= CalculatePowerOfUnit(_field.Booster);
            }
            _field.CardStates.AddUntilEndOfTurnValue(target.tempID, CardState.BonusPower, currentPower);
        }

        public void DoubleCritical(int selection)
        {
            Card target = _field.CardCatalog[selection];
            _field.CardStates.AddUntilEndOfTurnValue(target.tempID, CardState.BonusCritical, Critical(target.tempID));
        }

        public void AddDrive(int selection, int drive)
        {
            Card target = FindActiveUnit(selection);
            _field.CardStates.AddUntilEndOfTurnValue(target.tempID, CardState.BonusDrive, drive);
            Log.WriteLine("----------\n+" + drive + " drive to " + target.name + "!");
        }

        public void AddTempShield(int selection, int shield)
        {
            Card target = _field.CardCatalog[selection];
            _field.CardStates.AddUntilEndOfTurnValue(target.tempID, CardState.BonusShield, shield);
            Log.WriteLine("----------\n" + shield + " shield to " + target.name + "!");
        }

        public void SetAbilityPower(int selection, int power)
        {
            Card target = _field.CardCatalog[selection];
            _field.CardStates.AddContinuousValue(target.tempID, CardState.BonusPower, power);
        }

        public void SetAbilityShield(int selection, int shield)
        {
            Card target = _field.CardCatalog[selection];
            _field.CardStates.AddContinuousValue(target.tempID, CardState.BonusShield, shield);
        }

        public void SetAbilityDrive(int selection, int drive)
        {
            Card target = _field.CardCatalog[selection];
            _field.CardStates.AddContinuousValue(target.tempID, CardState.BonusDrive, drive);
        }

        public void SetAbilityCritical(int selection, int critical)
        {
            Card target = _field.CardCatalog[selection];
            _field.CardStates.AddContinuousValue(target.tempID, CardState.BonusCritical, critical);
        }

        public void Stand(int selection)
        {
            Card target = FindActiveUnit(selection);
            _stoodByCardEffect.Clear();
            if (!IsUpRight(target))
            {
                _stoodByCardEffect.Add(target);
                if (!_stoodByCardEffectThisTurn.Contains(selection))
                    _stoodByCardEffectThisTurn.Add(selection);
            }
            _field.Orientation.Rotate(target.tempID, true);
        }

        public void Heal(int selection)
        {
            PlayerDrop.Add(_field.CardCatalog[selection]);
            Log.WriteLine("----------\nDamage healed!");
        }

        public void AddTriggerToHand()
        {
            if (PlayerTrigger.Count() > 0)
                PlayerHand.Add(PlayerTrigger.Index(0));
        }

        public void RemoveTrigger()
        {
            if (PlayerTrigger.Count() > 0)
                _field.RemoveCard(PlayerTrigger.Index(0));
        }

        public void AddToHand(List<int> selections)
        {
            Card cardToAdd;
            foreach (int tempID in selections)
            {
                cardToAdd = _field.CardCatalog[tempID];
                PlayerHand.Add(cardToAdd);
            }
            Log.WriteLine(selections.Count + " card(s) added to hand.");
        }

        public void AddToSoul(List<int> selections)
        {
            Card cardToAdd;
            foreach (int tempID in selections)
            {
                cardToAdd = _field.CardCatalog[tempID];
                if (cardToAdd.originalOwner == _playerID)
                    _field.GetSoulZone(PlayerVanguard).Add(cardToAdd);
                else
                    _field.GetSoulZone(EnemyVanguard).Add(cardToAdd);
            }
        }

        public void AddToDrop(List<int> selections)
        {
            Card cardToAdd;
            foreach (int tempID in selections)
            {
                cardToAdd = _field.CardCatalog[tempID];
                PlayerDrop.Add(cardToAdd);
            }
        }

        public void AddToDamageZone(List<int> tempIDs)
        {
            Card card;
            foreach (int tempID in tempIDs)
            {
                card = _field.CardCatalog[tempID];
                PlayerDamage.Add(card);
            }
        }

        public void TakeDamage()
        {
            if (PlayerTrigger.Count() > 0)
            {
                PlayerDamage.Add(PlayerTrigger.Index(0));
                Log.WriteLine("----------\nDamage taken!");
            }
        }

        public bool CanPlayOrder()
        {
            if (!_orderPlayed || MyStates.GetValue(PlayerState.AdditionalOrder) > 0)
                return true;
            return false;
        }

        public int PlayOrder(int tempID)
        {
            _lastPutOnOrderZone.Clear();
            _lastOrderPlayed = null;
            Card card = _field.CardCatalog[tempID];
            if (PlayerDrop.Contains(card))
            {
                PlayerBind.Add(card);
                _isAlchemagic = true;
                return 2;
            }
            else if (PlayerHand.Contains(card))
            {
                _playedOrder = card;
                _lastOrderPlayed = card;
                if (_orderPlayed)
                    MyStates.DecrementUntilEndOfTurnValue(PlayerState.AdditionalOrder, 1);
                else
                    _orderPlayed = true;
                if (card.orderType == OrderType.Normal || card.orderType == OrderType.Blitz)
                {
                    PlayerOrderArea.Add(card);
                    return 0;
                }
                else
                {
                    PlayerOrder.Add(card);
                    _lastPutOnOrderZone.Add(card);
                    return 1;
                }
            }
            return 0;
        }

        public Card GetPlayedOrder()
        {
            return _playedOrder;
        }

        public void EndOrder()
        {
            if (PlayerOrderArea.Contains(_playedOrder))
            {
                PlayerDrop.Add(_playedOrder);
            }
            _playedOrder = null;
        }

        public bool OrderPlayed()
        {
            return _orderPlayed;
        }

        public void AllowFreeSwap()
        {
            MyStates.AddContinuousState(PlayerState.FreeSwap);
        }

        public bool CanFreeSwap()
        {
            return MyStates.HasState(PlayerState.FreeSwap);
        }

        public void AllowColumnAttack(int tempID)
        {
            _field.CardStates.AddContinuousState(tempID, CardState.CanColumnAttack);
        }

        public void AllowInterceptFromBackRow(int tempID)
        {
            _field.CardStates.AddContinuousState(tempID, CardState.CanInterceptFromBackRow);
        }

        public void SetAlchemagicDiff()
        {
            MyStates.AddContinuousState(PlayerState.CanAlchemagicDiff);
        }

        public void SetAlchemagicSame()
        {
            MyStates.AddContinuousState(PlayerState.CanAlchemagicSame);
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
            _alchemagicFreeSBActive = false;
        }

        public bool CanAlchemagicDiff()
        {
            return MyStates.HasState(PlayerState.CanAlchemagicDiff);
        }

        public bool CanAlchemagicSame()
        {
            return MyStates.HasState(PlayerState.CanAlchemagicSame);
        }

        public bool IsAlchemagic()
        {
            return _isAlchemagic;
        }

        public void AlchemagicFreeSB()
        {
            _alchemagicFreeSB = true;
        }

        public void UsedAlchemagicFreeSB()
        {
            _alchemagicFreeSB = false;
            _alchemagicFreeSBActive = true;
        }

        public bool AlchemagicFreeSBActive()
        {
            return _alchemagicFreeSBActive;
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
            if (OnSetPrison != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.playerID = _playerID;
                OnSetPrison(this, args);
            }
        }

        public void Imprison(List<int> cardsToImprison)
        {
            if (!HasPrison())
                return;
            Card card;
            foreach (int tempID in cardsToImprison)
            {
                card = _field.CardCatalog[tempID];
                PlayerOrder.Add(card);
                PlayerPrisoners.Add(card);
                if (OnImprison != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.playerID = _playerID;
                    OnImprison(this, args);
                }
            }
        }

        public bool HasPrison()
        {
            if (PlayerPrison != null)
                return true;
            return false;
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
            foreach (Card card in PlayerOrder.GetCards())
            {
                if (card.orderType != OrderType.World)
                    return false;
            }
            return true;
        }

        public int NumWorlds()
        {
            int count = 0;
            foreach (Card card in PlayerOrder.GetCards())
            {
                if (card.orderType == OrderType.World)
                    count++;
            }
            return count;
        }

        //public void DarkNight()
        //{
        //    MyStates.AddContinuousState(PlayerState.DarkNight);
        //}

        //public void AbyssalDarkNight()
        //{
        //    MyStates.AddContinuousState(PlayerState.AbyssalDarkNight);
        //}

        public bool IsDarkNight()
        {
            int worlds = 0;
            foreach (Card card in PlayerOrder.GetCards())
            {
                if (card.orderType != OrderType.World)
                    return false;
                else
                    worlds++;
            }
            if (worlds == 1)
                return true;
            return false;
        }

        public bool IsAbyssalDarkNight()
        {
            int worlds = 0;
            foreach (Card card in PlayerOrder.GetCards())
            {
                if (card.orderType != OrderType.World)
                    return false;
                else
                    worlds++;
            }
            if (worlds >= 2)
                return true;
            return false;
        }

        public bool WorldPlayed()
        {
            if (_lastOrderPlayed != null && _lastOrderPlayed.orderType == OrderType.World)
                return true;
            return false;
        }

        public List<Card> GetLastPutOnOrderZone()
        {
            return _lastPutOnOrderZone;
        }

        public int CreateToken(string tokenID)
        {
            return _field.CreateToken(tokenID);
        }

        public bool CanCB(int count)
        {
            int faceup = 0;
            foreach (Card card in PlayerDamage.GetCards())
            {
                if (IsFaceUp(card))
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
                if (_field.GetUnit(i) != null)
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
            damage = PlayerDamage.GetCards();
            foreach (int tempID in cardsToCB)
            {
                foreach (Card card in damage)
                {
                    if (card.tempID == tempID)
                    {
                        _field.Orientation.Flip(card.tempID, false);
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
            if (_alchemagicFreeSB || _field.GetSoul(PlayerVanguard).Count >= count)
                return true;
            return false;
        }

        public void SoulBlast(List<int> cardsToSB)
        {
            foreach (int tempID in cardsToSB)
            {
                PlayerDrop.Add(_field.CardCatalog[tempID]);
            }
        }

        public void CounterCharge(List<int> cardsToCharge)
        {
            foreach (int tempID in cardsToCharge)
            {
                _field.Orientation.Flip(tempID, true);
            }
        }

        public void SoulCharge(int count)
        {
            Log.WriteLine("Soul Charging " + count + " card(s)!");
            _soulChargedThisTurn = true;
            for (int i = 0; i < count && PlayerDeck.GetCards().Count > 0; i++)
            {
                _field.GetSoulZone(PlayerVanguard).Add(PlayerDeck.Index(0));
            }
        }

        public bool SoulChargedThisTurn()
        {
            return _soulChargedThisTurn;
        }

        public void Search(List<int> cardsToSearch)
        {
            foreach (int tempID in cardsToSearch)
            {
                if (PlayerDeck.Contains(_field.CardCatalog[tempID]))
                    PlayerHand.Add(_field.CardCatalog[tempID]);
                List<int> list = new List<int>();
                list.Add(tempID);
            }
        }

        public void Stand(List<int> cardsToStand)
        {
            _stoodByCardEffect.Clear();
            foreach (int tempID in cardsToStand)
            {
                if (!_field.Orientation.IsUpRight(tempID))
                {
                    _field.Orientation.Rotate(tempID, true);
                    _stoodByCardEffect.Add(_field.CardCatalog[tempID]);
                    _stoodByCardEffectThisTurn.Add(tempID);
                }
            }
        }

        public void Rest(List<int> cardsToRest)
        {
            foreach (int tempID in cardsToRest)
            {
                _field.Orientation.Rotate(tempID, false);
            }
        }

        public void Reveal(List<int> cardsToReveal)
        {
            Card card;
            string dialogue = "";
            EndReveal();
            foreach (int tempID in cardsToReveal)
            {
                card = _field.CardCatalog[tempID];
                _field.Orientation.SetFaceUp(card.tempID, true);
                PlayerRevealed.Add(card);
                if (PlayerHand.Contains(card))
                    dialogue = "hand";
                else if (PlayerDeck.Contains(card))
                    dialogue = "deck";
                Log.WriteLine("----------\n" + card.name + " revealed from " + dialogue + "!");
                if (OnReveal != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.card = card;
                    args.currentLocation = new Tuple<int, int>(_field.CardLocations[card.tempID].GetLocation(), -1);
                    OnReveal(this, args);
                }
            }
        }

        public void RevealFromDeck(int count)
        {
            EndReveal();
            for (int i = 0; i < count && PlayerDeck.GetCards().Count > i; i++)
            {
                PlayerRevealed.Add(PlayerDeck.Index(i));
                if (OnReveal != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.card = PlayerDeck.Index(i);
                    args.currentLocation = new Tuple<int, int>(_field.CardLocations[PlayerDeck.Index(i).tempID].GetLocation(), -1);
                    OnReveal(this, args);
                }
            }
        }

        public void EndReveal()
        {
            //foreach (Card card in PlayerRevealed)
            //{
            //    if (card.location == Location.EnemyHand || card.location == Location.Deck)
            //        IsFaceUp(card) = false;
            //}
            PlayerRevealed.Clear();
        }

        public void LookAtTopOfDeck(int count)
        {
            PlayerLooking.Clear();
            for (int i = 0; i < count && PlayerDeck.GetCards().Count > i; i++)
                PlayerLooking.Add(PlayerDeck.Index(i));
        }

        public void RearrangeOnTop(List<int> tempIDs)
        {
            Card card;
            for (int i = tempIDs.Count - 1; i >= 0; i--)
            {
                card = _field.CardCatalog[tempIDs[i]];
                PlayerDeck.AddToTop(card);
            }
        }

        public void RearrangeOnBottom(List<int> tempIDs)
        {
            Card card;
            for (int i = 0; i < tempIDs.Count; i++)
            {
                card = _field.CardCatalog[tempIDs[i]];
                PlayerDeck.Add(card);
            }
        }

        public void AddSkill(int tempID, int skill)
        {
            _field.CardStates.AddContinuousValue(tempID, CardState.BonusSkills, skill);
        }

        public void AddSkillUntilEndOfTurn(int tempID, int skill)
        {
            _field.CardStates.AddUntilEndOfTurnValue(tempID, CardState.BonusSkills, skill);
        }

        public void AllowBackRowAttack(int tempID)
        {
            _field.CardStates.AddUntilEndOfTurnState(tempID, CardState.CanAttackFromBackRow);
        }

        public void AllowAttackAllFrontRow(int tempID)
        {
            _field.CardStates.AddUntilEndOfTurnState(tempID, CardState.CanAttackAllFrontRow);
        }

        public void AllowAttackingBackRow(int tempID)
        {
            _field.CardStates.AddUntilEndOfTurnState(tempID, CardState.CanAttackBackRow);
        }

        public void DisableAttack(int tempID)
        {
            _field.CardStates.AddContinuousState(tempID, CardState.CannotAttack);
        }

        public void DisableMove(int tempID)
        {
            _field.CardStates.AddUntilEndOfTurnState(tempID, CardState.CannotMove);
        }

        public void GiveAbility(List<int> tempIDs, int abilityTempID, int activationNumber)
        {
            foreach (int tempID in tempIDs)
            {
                if (GetAllUnitsOnField().Contains(_field.CardCatalog[tempID]))
                {
                    _field.CardStates.AddUntilEndOfTurnAbility(tempID, abilityTempID, activationNumber);
                }
            }
        }

        public List<Tuple<int, int>> GetGivenAbility(int tempID)
        {
            return _field.CardStates.GetAbilities(tempID);
        }

        public void EndAttack()
        {
            List<int> sendToDeck = new List<int>();
            foreach (Card card in GetActiveUnits())
            {
                if (CardStates.HasState(card.tempID, CardState.SendToBottomAtEndOfBattle))
                    sendToDeck.Add(card.tempID);
            }
            SendToDeck(sendToDeck, true);
            MyStates.EndAttack();
            _field.CardStates.EndAttack();
            _field.Sentinel.Clear();
            _field.Attacker = -1;
            _field.Attacked.Clear();
            _field.Guardians.Clear();
            _field.Booster = -1;
            _lastRevealedDriveChecks.Clear();
            _lastRevealedDamageChecks.Clear();
            _field.UnitsHit.Clear();
            if (OnAttackEnds != null)
                OnAttackEnds(this, new CardEventArgs());
        }

        public void EndTurn()
        {
            RetireCardsMarkedForRetire();
            MyStates.EndTurn();
            _field.CardStates.EndTurn();
            _alchemagicFreeSB = false;
            _alchemagicFreeCBAvailable = 0;
            _alchemagicUsed = false;
            _field.SetPersonaRide(false, _playerID);
            _stoodByCardEffect.Clear();
            _stoodByCardEffectThisTurn.Clear();
            _orderPlayed = false;
            _soulChargedThisTurn = false;
            _playerRetiredThisTurn = false;
            _enemyRetiredThisTurn = false;
            _bonusDriveCheckPower = 0;
            _unitsCalledThisTurn.Clear();
            _unitsCalledFromHandThisTurn.Clear();
            _riddenThisTurn = false;
            //for (int i = 0; i < _field.CirclePower.Length; i++)
            //    _field.CirclePower[i] = 0;
            //for (int i = 0; i < _field.CircleCritical.Length; i++)
            //    _field.CircleCritical[i] = 0;
        }

        public void RetireCardsMarkedForRetire()
        {
            foreach (Card card in GetActiveUnits())
            {
                if (_field.CardStates.HasState(card.tempID, CardState.RetireAtEndOfTurn))
                {
                    List<int> list = new List<int>();
                    list.Add(card.tempID);
                    Retire(list);
                }
            }
        }

        public void RefreshContinuous()
        {
            MyStates.RefreshContinuousStates();
            _field.CardStates.RefreshContinuousStates();
        }

        public void AllAbilitiesResolved()
        {
            _lastPlacedOnRC.Clear();
            _lastPlacedOnRCFromHand.Clear();
            _lastPlacedOnRCFromPrison.Clear();
            _lastPlacedOnGC.Clear();
            _lastPutOnGC.Clear();
            //_lastRevealedTriggers.Clear();
            _lastDiscarded.Clear();
            _lastPlacedOnVC.Clear();
            _lastPutOnOrderZone.Clear();
            _retiredForPlayerCost.Clear();
            _stoodByCardEffect.Clear();
        }

        public void IncrementTurn()
        {
            _field.IncrementTurn();
        }

        //public void ResetCardValues(Card card)
        //{
        //    if (_givenAbilities.ContainsKey(card.tempID))
        //        _givenAbilities[card.tempID].Clear();
        //}

        public void OnRideAbilityResolved(int tempID)
        { 

        }

        public bool IsEnemy(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            for (int i = EnemyFrontLeft; i <= EnemyVanguard; i++)
            {
                if (_field.GetUnit(i) != null && _field.GetUnit(i).tempID == tempID)
                    return true;
            }
            return false;
        }

        public List<Card> Soul()
        {
            return _field.GetSoul(PlayerVanguard);
        }

        public Card Vanguard()
        {
            return _field.GetUnit(PlayerVanguard);
        }

        public Card AttackingUnit()
        {
            if (_field.Attacker > 0)
                return _field.GetUnit(_field.Attacker);
            return null;
        }

        public Card Booster()
        {
            if (_field.Booster > 0)
                return _field.GetUnit(_field.Booster);
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
            return _field.GetUnit(_field.Attacker).tempID;
        }

        public bool HasCardInDeck(string name)
        {
            foreach (Card card in PlayerDeck.GetCards())
            {
                if (card.name == name)
                    return true;
            }
            return false;
        }

        public bool InFinalRush()
        {
            return MyStates.HasState(PlayerState.FinalRush);
        }

        public void FinalRush()
        {
            MyStates.AddUntilEndOfTurnState(PlayerState.FinalRush);
        }

        public void RearguardDriveCheck()
        {
            MyStates.AddUntilEndOfTurnState(PlayerState.RearguardDriveCheck);
        }

        public bool RearguardCanDriveCheck()
        {
            return MyStates.HasState(PlayerState.RearguardDriveCheck);
        }

        public bool StoodByCardEffectThisTurn(int tempID)
        {
            if (_stoodByCardEffectThisTurn.Contains(tempID))
                return true;
            return false;
        }

        public int NumOriginalDress(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            return _field.GetSoul(GetCircle(card)).Count;
        }

        public List<int> GetAvailableCircles(int tempID)
        {
            List<int> circles = new List<int>();
            if (_field.CardStates.HasState(tempID, CardState.CanOnlyBeCalledToBackRowCenter))
            {
                circles.Add(PlayerBackCenter);
                return circles;
            }
            circles.Add(PlayerBackRight);
            circles.Add(PlayerBackCenter);
            circles.Add(PlayerBackLeft);
            if (!_field.CardStates.HasState(tempID, CardState.CannotBeCalledToFrontRow))
            {
                circles.Add(PlayerFrontRight);
                circles.Add(PlayerFrontLeft);
            }
            return circles;
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

        public bool IsUpRight(Card card)
        {
            return _field.Orientation.IsUpRight(card.tempID);
        }

        public bool IsFaceUp(Card card)
        {
            return _field.Orientation.IsFaceUp(card.tempID);
        }

        public int GetLocation(Card card)
        {
            if (_field.CardLocations[card.tempID] != null)
                return _field.CardLocations[card.tempID].GetLocation();
            else
                return -1;
        }

        public bool CardValueChanged(RecordedCardValue previousValues, int currentPower, int currentCritical)
        {
            if (previousValues.currentPower != currentPower || previousValues.currentCritical != currentCritical)
            {
                previousValues.currentPower = currentPower;
                previousValues.currentCritical = currentCritical;
                return true;
            }
            return false;
        }

        public void RetireAtEndOfTurn(List<int> tempIDs)
        {
            Card card;
            foreach (int tempID in tempIDs)
            {
                card = _field.CardCatalog[tempID];
                if (card != null && GetActiveUnits().Contains(card))
                    _field.CardStates.AddUntilEndOfTurnState(tempID, CardState.RetireAtEndOfTurn);
            }
        }

        public bool IsOverDress(int tempID)
        {
            foreach (Card card in GetActiveUnits())
            {
                if (card.tempID == tempID && _field.GetSoul(GetCircle(card)).Count > 0)
                    return true;
            }
            return false;
        }

        public int GetRowNum(int circle)
        {
            return _field.GetRow(circle);
        }

        public int GetColumnNum(int circle)
        {
            return _field.GetColumn(circle);
        }

        public List<int> PlayerRCCircles()
        {
            List<int> circles = new List<int>();
            for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
                circles.Add(i);
            return circles;
        }

        public List<int> PlayerCircles()
        {
            List<int> circles = new List<int>();
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
                circles.Add(i);
            return circles;
        }

        public bool CanChooseThreeCirclesWhenAttacking(int tempID)
        {
            return _field.CardStates.HasState(tempID, CardState.CanChooseThreeCirclesWhenAttacking);
        }

        public void PutIntoOrderZone(List<int> tempIDs)
        {
            _lastPutOnOrderZone.Clear();
            foreach (int tempID in tempIDs)
            {
                Card card = _field.CardCatalog[tempID];
                PlayerOrder.Add(card);
                _lastPutOnOrderZone.Add(card);
            }
        }

        public void Flip(List<int> tempIDs, bool faceup)
        {
            foreach (int tempID in tempIDs)
            {
                _field.Orientation.Flip(tempID, faceup);
            }
        }

        public List<Card> GetLastOrderPlayed()
        {
            List<Card> list = new List<Card>();
            if (_lastOrderPlayed != null)
                list.Add(_lastOrderPlayed);
            return list;
        }

        public List<int> ConvertToTempIDs(List<Card> cards)
        {
            List<int> tempIDs = new List<int>();
            if (cards == null)
                return tempIDs;
            foreach (Card card in cards)
            {
                tempIDs.Add(card.tempID);
            }
            return tempIDs;
        }

        public List<Card> ConvertToCards(List<int> tempIDs)
        {
            List<Card> cards = new List<Card>();
            if (tempIDs == null)
                return cards;
            foreach (int tempID in tempIDs)
            {
                cards.Add(GetCard(tempID));
            }
            return cards;
        }

        public FieldSnapShot GenerateSnapShot()
        {
            return new FieldSnapShot(_field);
        }
    }

    public class Direction
    {
        public const int Up = 1;
        public const int Right = 2;
        public const int Down = 3;
        public const int Left = 4;
    }

    public class RecordedCardValue
    {
        public int currentPower = 0;
        public int currentCritical = 0;

        public RecordedCardValue(int power, int critical)
        {
            currentPower = power;
            currentCritical = critical;
        }
    }
}
