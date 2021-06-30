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
        protected List<Card> _riddenOnThisTurn = new List<Card>();
        protected List<Card> _lastPlacedOnGC = new List<Card>();
        protected List<Card> _lastPlacedOnRC = new List<Card>();
        protected List<Card> _lastPlacedOnVC = new List<Card>();
        protected List<Card> _lastRevealedTriggers = new List<Card>();
        protected List<Card> _canAttackFromBackRow = new List<Card>();
        protected Dictionary<int, List<int>> _bonusSkills = new Dictionary<int, List<int>>(); //tempID, skills
        protected Card _playedOrder;
        protected bool _finalRush = false;
        protected bool _enemyRetired = false;
        protected bool _playerRetired = false;
        protected bool _enemyRetiredThisTurn = false;
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
        protected Card PlayerTrigger;
        protected Card EnemyTrigger;
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
                if (card.orderType >= 0 && card.grade <= _field.Units[PlayerVanguard].grade)
                    orderableCards.Add(card);
            }
            return orderableCards;
        }

        public List<Card> GetSoul()
        {
            return _field.Units[PlayerVanguard].soul;
        }

        public List<Card> GetBackRow()
        {
            List<Card> backRow = new List<Card>();
            if (_field.Units[PlayerBackLeft] != null)
                backRow.Add(_field.Units[PlayerBackLeft]);
            if (_field.Units[PlayerBackRight] != null)
                backRow.Add(_field.Units[PlayerBackRight]);
            if (_field.Units[PlayerBackCenter] != null)
                backRow.Add(_field.Units[PlayerBackCenter]);
            return backRow;
        }

        public List<Card> GetGC()
        {
            return _field.GC;
        }

        public List<Card> GetRevealed()
        {
            return PlayerRevealed;
        }

        public List<Card> GetLooking()
        {
            return PlayerLooking;
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

        public List<Card> GetRiddenOnThisTurn()
        {
            return _riddenOnThisTurn;
        }

        public Card GetUnitAt(int circle)
        {
            circle = Convert(circle);
            return _field.Units[circle];
        }

        public int GetCircle(Card card)
        {
            int circle = -1;
            for (circle = 0; circle < FL.MaxFL(); circle++)
            {
                if (_field.Units[circle] != null && _field.Units[circle].tempID == card.tempID)
                    return circle;
            }
            return -1;
        }

        public int NumEnemyOpenCircles()
        {
            int count = 0;
            for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
            {
                if (_field.Units[i] != null)
                    count++;
            }
            return 5 - count;
        }

        public List<Card> GetRevealedTriggers()
        {
            return _lastRevealedTriggers;
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
            foreach (int tempID in list)
            {
                foreach (Card card in PlayerHand)
                {
                    if (card.tempID == tempID)
                    {
                        toDiscard.Add(card);
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

        public void GiveShuffleKey(int[] key)
        {
            _field.ShuffleKey = key;
        }

        public void StandAll()
        {
            Card[] Units = _field.Units;
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (Units[i] != null)
                    Units[i].upright = true;
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
            Card[] Units = _field.Units;
            List<Card> GC = _field.GC;
            if (Units[EnemyVanguard].upright)
                estand = "S";
            else
                estand = "R";
            if (Units[PlayerVanguard].upright)
                pstand = "S";
            else
                pstand = "R";
            output = "----------\nEnemy Hand: " + hand.Count + " Enemy Soul: " + Units[EnemyVanguard].soul.Count + " Player Soul: " + Units[PlayerVanguard].soul.Count + " Player Damage: " + PlayerDamage.Count + " Enemy Damage: " + EnemyDamage.Count + "\n" +
                "Choose circle to examine.\n" +
                "1. " + PrintRGData(EnemyBackRight) + " | " + "2. " + PrintRGData(EnemyBackCenter) + " | " + "3. " + PrintRGData(EnemyBackLeft) + "\n" +
                "4. " + PrintRGData(EnemyFrontRight) + " | " + "5. " + CalculatePowerOfUnit(EnemyVanguard) + " G" + Units[EnemyVanguard].grade + " " + estand + " | 6. " + PrintRGData(EnemyFrontLeft) + "\n" +
                "7.                 (to-do)\n" +
                "8. " + PrintRGData(PlayerFrontLeft) + " | 9. " + CalculatePowerOfUnit(PlayerVanguard) + " G" + Units[PlayerVanguard].grade + " " + pstand + " | 10. " + PrintRGData(PlayerFrontRight) + "\n" +
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
            Card[] Units = _field.Units;
            if (Units[location] != null)
            {
                output = CalculatePowerOfUnit(location) + " G" + Units[location].grade + " ";
                if (Units[location].upright)
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
            if (_field.Units[PlayerVanguard].soul.Count == 0)
            {
                Console.WriteLine("----------No cards in Soul.");
                return;
            }
            Console.WriteLine("----------");
            for (int i = 0; i < _field.Units[PlayerVanguard].soul.Count; i++)
            {
                Console.WriteLine(i + 1 + ". " + _field.Units[PlayerVanguard].soul[i].name);
            }
        }

        public void DisplayVanguard(bool player)
        {
            if (player)
                DisplayCard(_field.Units[PlayerVanguard]);
            else
                DisplayCard(_field.Units[EnemyVanguard]);
        }

        public void DisplayRearguard(int selection)
        {
            if (selection == 1)
                DisplayCard(_field.Units[EnemyBackRight]);
            else if (selection == 2)
                DisplayCard(_field.Units[EnemyBackCenter]);
            else if (selection == 3)
                DisplayCard(_field.Units[EnemyBackLeft]);
            else if (selection == 4)
                DisplayCard(_field.Units[EnemyFrontRight]);
            else if (selection == 6)
                DisplayCard(_field.Units[EnemyFrontLeft]);
            else if (selection == 8)
                DisplayCard(_field.Units[PlayerFrontLeft]);
            else if (selection == 10)
                DisplayCard(_field.Units[PlayerFrontRight]);
            else if (selection == 11)
                DisplayCard(_field.Units[PlayerBackLeft]);
            else if (selection == 12)
                DisplayCard(_field.Units[PlayerBackCenter]);
            else if (selection == 13)
                DisplayCard(_field.Units[PlayerBackRight]);
        }

        public List<Card> GetRideableCards(bool rideDeck)
        {
            List<Card> cards = new List<Card>();
            if (rideDeck)
            {
                foreach (Card card in PlayerRideDeck)
                {
                    if (card.grade == _field.Units[PlayerVanguard].grade || card.grade == _field.Units[PlayerVanguard].grade + 1)
                        cards.Add(card);
                }
            }
            else
            {
                foreach (Card card in PlayerHand)
                {
                    if (card.grade == _field.Units[PlayerVanguard].grade || card.grade == _field.Units[PlayerVanguard].grade + 1)
                        cards.Add(card);
                }
            }
            return cards;
        }

        public List<Card> GetCallableRearguards()
        {
            List<Card> hand = PlayerHand;
            List<Card> callableCards = new List<Card>();
            Card VG = _field.Units[PlayerVanguard];
            foreach (Card card in hand)
            {
                if (card.grade <= VG.grade && card.orderType < 0)
                    callableCards.Add(card);
            }
            return callableCards;
        }

        public int GetShield()
        {
            return CalculateShield();
        }

        public bool CheckColumn(int column) //0 is left, 1 is right
        {
            if (column == 0)
            {
                if (_field.Units[PlayerFrontLeft] != null || _field.Units[PlayerBackLeft] != null)
                    return true;
                return false;
            }
            else
            {
                if (_field.Units[PlayerFrontRight] != null || _field.Units[PlayerBackRight] != null)
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
            if (_field.Units[PlayerFrontLeft] != null && _field.Units[PlayerFrontLeft].upright)
                cards.Add(_field.Units[PlayerFrontLeft]);
            if (_field.Units[PlayerVanguard].upright)
                cards.Add(_field.Units[PlayerVanguard]);
            if (_field.Units[PlayerFrontRight] != null && _field.Units[PlayerFrontRight].upright)
                cards.Add(_field.Units[PlayerFrontRight]);
            foreach (Card card in _canAttackFromBackRow)
            {
                if (card.upright)
                    cards.Add(card);
            }
            return cards;
        }

        public List<Card> GetEnemyFrontRow()
        {
            List<Card> cards = new List<Card>();
            if (_field.Units[EnemyFrontLeft] != null)
                cards.Add(_field.Units[EnemyFrontLeft]);
            cards.Add(_field.Units[EnemyVanguard]);
            if (_field.Units[EnemyFrontRight] != null)
                cards.Add(_field.Units[EnemyFrontRight]);
            return cards;
        }

        public List<Card> GetPlayerFrontRow()
        {
            List<Card> cards = new List<Card>();
            if (_field.Units[PlayerFrontLeft] != null)
                cards.Add(_field.Units[PlayerFrontLeft]);
            cards.Add(_field.Units[PlayerVanguard]);
            if (_field.Units[PlayerFrontRight] != null)
                cards.Add(_field.Units[PlayerFrontRight]);
            return cards;
        }

        public List<Card> GetGuardableCards()
        {
            List<Card> cards = new List<Card>();
            foreach (Card card in PlayerHand)
            {
                if (card.orderType < 0 && card.grade <= _field.Units[PlayerVanguard].grade)
                    cards.Add(card);
            }
            foreach (Card card in _field.Units)
            {
                if (CanIntercept(card))
                    cards.Add(card);
            }
            return cards;
        }

        public bool CanIntercept(Card card)
        {
            if (card != null && card.skill == 1 && card != _field.Units[_field.Attacked] && (_field.Units[PlayerFrontLeft] == card || _field.Units[PlayerFrontRight] == card))
                return true;
            return false;
        }

        public List<Card> GetActiveUnits()
        {
            List<Card> cards = new List<Card>();
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.Units[i] != null)
                    cards.Add(_field.Units[i]);
            }
            return cards;
        }

        public List<Card> GetAllUnitsOnField()
        {
            List<Card> cards = new List<Card>();
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (_field.Units[i] != null)
                    cards.Add(_field.Units[i]);
            }
            for (int i = EnemyFrontLeft; i <= EnemyVanguard; i++)
            {
                if (_field.Units[i] != null)
                    cards.Add(_field.Units[i]);
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
                    if (_field.Units[i] != null)
                        rearguards.Add(_field.Units[i]);
                }
            }
            else
            {
                for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                {
                    if (_field.Units[i] != null)
                        rearguards.Add(_field.Units[i]);
                }
            }
            return rearguards;
        }

        public bool IsPlayerTurn()
        {
            if (_startingTurn == 1 && _field.Turn % 2 != 0)
                return true;
            if (_startingTurn == 2 && _field.Turn % 2 == 0)
                return false;
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

        public int CalculateShield()
        {
            int shield = 0;
            if (_field.Sentinel)
                return 1000000000;
            foreach (Card card in _field.GC)
            {
                shield += card.shield + card.tempShield;
                foreach (Tuple<int, int> key in card.abilityShield.Keys)
                    shield += card.abilityShield[key];
            }
            return shield;
        }

        public void PrintShield()
        {
            if (_field.Sentinel)
            {
                Console.WriteLine("Perfect Shield is active.");
                return;
            }
            Console.WriteLine("Shield: " + (CalculateShield() + CalculatePowerOfUnit(_field.Attacked)));
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
            Card[] Units = _field.Units;
            int power = Units[location].power + Units[location].bonusPower + Units[location].tempPower + Units[location].battleOnlyPower;
            if (IsFrontRow(location) && IsPlayer(location) && _field.GetPersonaRide(_playerID))
                power += 10000;
            else if (IsFrontRow(location) && !IsPlayer(location) && _field.GetPersonaRide(_enemyID))
                power += 10000;
            foreach (Tuple<int, int> key in Units[location].abilityPower.Keys)
                power += Units[location].abilityPower[key];
            if (_field.Booster >= 0 && location == _field.Attacker)
            {
                power += CalculatePowerOfUnit(_field.Booster);
            }
            return power;
        }

        public int PrintActiveUnits()
        {
            int max = 0;
            //PrintAttack();
            string output;
            if (_field.Units[PlayerFrontLeft] != null)
            {
                max++;
                output = max + ". FrontLeft " + _field.Units[PlayerFrontLeft].name + " ";
                if (!_guarding && _field.Booster == 0)
                    output += (_field.Units[PlayerFrontLeft].power + _field.Units[PlayerFrontLeft].bonusPower + _field.Units[PlayerBackLeft].power + _field.Units[PlayerBackLeft].bonusPower);
                else
                    output += (_field.Units[PlayerFrontLeft].power + _field.Units[PlayerFrontLeft].bonusPower);
                Console.WriteLine(output);
            }
            max++;
            output = max + ". FrontCenter " + _field.Units[PlayerVanguard].name + " ";
            if (!_guarding && _field.Booster == 1)
                output += (_field.Units[PlayerVanguard].power + _field.Units[PlayerVanguard].bonusPower + _field.Units[PlayerBackCenter].power + _field.Units[PlayerBackCenter].bonusPower);
            else
                output += (_field.Units[PlayerVanguard].power + _field.Units[PlayerVanguard].bonusPower);
            Console.WriteLine(output);
            if (_field.Units[PlayerFrontRight] != null)
            {
                max++;
                output = max + ". FrontCenter " + _field.Units[PlayerFrontRight].name + " ";
                if (!_guarding && _field.Booster == 2)
                    output += (_field.Units[PlayerFrontRight].power + _field.Units[PlayerFrontRight].bonusPower + _field.Units[PlayerBackRight].power + _field.Units[PlayerBackRight].bonusPower);
                else
                    output += (_field.Units[PlayerBackRight].power + _field.Units[PlayerBackRight].bonusPower);
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
            Card VG = _field.Units[PlayerVanguard];
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
            Card VG = _field.Units[PlayerVanguard];
            foreach (Card card in hand)
            {
                if (card.grade == VG.grade || card.grade - 1 == VG.grade)
                    return true;
            }
            return false;
        }

        public bool CanCallRearguard()
        {
            List<Card> hand = PlayerHand;
            Card VG = _field.Units[PlayerVanguard];
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
                if (_field.Units[i] != null)
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
            if (_field.Units[PlayerVanguard].upright || (_field.Units[PlayerFrontLeft] != null && _field.Units[PlayerFrontLeft].upright) || (_field.Units[PlayerFrontRight] != null && _field.Units[PlayerFrontRight].upright))
                return true;
            for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
            {
                if (i == PlayerFrontLeft || i == PlayerFrontRight || i == PlayerVanguard)
                {
                    if (_field.Units[i] != null && _field.Units[i].upright)
                        return true;
                }
                else if (_canAttackFromBackRow.Contains(_field.Units[i]))
                {
                    if (_field.Units[i] != null && _field.Units[i].upright)
                        return true;
                }
            }
            return false;
        }

        public bool CanBeBoosted()
        {
            if (_field.Attacker == PlayerFrontLeft)
            {
                if (_field.Units[PlayerBackRight] != null && CanBoost(_field.Units[PlayerBackRight]))
                    return true;
            }
            else if (_field.Attacker == PlayerVanguard)
            {
                if (_field.Units[PlayerBackCenter] != null && CanBoost(_field.Units[PlayerBackCenter]))
                    return true;
            }
            else if (_field.Attacker == PlayerFrontRight)
            {
                if (_field.Units[PlayerBackRight] != null && CanBoost(_field.Units[PlayerBackRight]))
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
            foreach (Card card in PlayerHand)
            {
                if (card.orderType < 0)
                    return true;
            }
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
            if (_field.Attacked == Vanguard)
                return true;
            return false;
        }

        public bool IsLastPlacedOnGC(int tempID)
        {
            foreach (Card card in _lastPlacedOnGC)
            {
                if (card.tempID == tempID)
                    return true;
            }
            return false;
        }

        public bool IsLastPlacedOnRC(int tempID)
        {
            foreach (Card card in _lastPlacedOnRC)
            {
                if (card.tempID == tempID)
                    return true;
            }
            return false;
        }

        public bool IsLastPlacedOnVC(int tempID)
        {
            foreach (Card card in _lastPlacedOnVC)
            {
                if (card.tempID == tempID)
                    return true;
            }
            return false;
        }

        public int Drive()
        {
            int drive = _field.Units[PlayerVanguard].drive;
            foreach (Tuple<int, int> key in _field.Units[PlayerVanguard].abilityDrive.Keys)
            {
                drive += _field.Units[PlayerVanguard].abilityDrive[key];
            }
            return drive;
        }

        public int Critical()
        {
            return _field.Units[PlayerVanguard].critical + _field.Units[PlayerVanguard].tempCritical;
        }

        public int Damage()
        {
            return PlayerDamage.Count; 
        }

        public Card GetTrigger(bool player)
        {
            if (player)
                return PlayerTrigger;
            return EnemyTrigger;
        }

        public bool AttackHits()
        {
            Card attacker = _field.Units[_field.Attacker];
            Card card = _field.Units[_field.Attacked];
            CardEventArgs args = new CardEventArgs();
            if (CalculatePowerOfUnit(_field.Attacker) >= CalculateShield() + CalculatePowerOfUnit(_field.Attacked))
            {
                Console.WriteLine("----------\n" + attacker.name + "'s attack makes a hit on " + card.name + "!");
                if (OnAttackHits != null)
                {
                    args.b = true;
                    OnAttackHits(this, args);
                }
                return true;
            }
            else
            {
                Console.WriteLine("----------\n" + attacker.name + "'s attack against " + card.name + " was successfully guarded!");
                if (OnAttackHits != null)
                {
                    args.b = false;
                    OnAttackHits(this, args);
                }
                return false;
            }
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
            _riddenOnThisTurn.Add(_field.Units[PlayerVanguard]);
            _lastPlacedOnVC.Clear();
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
            if (card.name == _field.Units[PlayerVanguard].name && _field.Units[PlayerVanguard].personaRide == 1)
            {
                _field.SetPersonaRide(true, _playerID);
                Draw(1);
            }
            list.Remove(card);
            card.faceup = true;
            card.soul.AddRange(_field.Units[PlayerVanguard].soul);
            card.soul.Insert(0, _field.Units[PlayerVanguard]);
            _field.Units[PlayerVanguard].location = Location.Soul;
            _field.Units[PlayerVanguard] = card;
            card.location = Location.VC;
            card.soul[0].soul.Clear();
            ResetCardValues(card.soul[0]);
            if (_field.GetPersonaRide(_playerID))
                Console.WriteLine("---------\nPersona Ride!! " + _field.Units[PlayerVanguard].name + "!");
            else
                Console.WriteLine("---------\nRide!! " + _field.Units[PlayerVanguard].name + "!");
            _lastPlacedOnVC.Add(card);
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
            Card[] slots = _field.Units;
            Card card = null;
            Card toBeRetired = null;
            List<Card> retired = new List<Card>();
            CardEventArgs args;
            _lastPlacedOnRC.Clear();
            hand = PlayerHand;
            location = Convert(location);
            foreach (Card item in hand)
            {
                if (item.tempID == selection)
                {
                    card = item;
                    break;
                }
            }
            if (!overDress && slots[location] != null)
            {
                toBeRetired = slots[location];
                retired.Add(toBeRetired);
                foreach (Card soul in slots[location].soul)
                {
                    PlayerDrop.Insert(0, soul);
                    soul.location = Location.Drop;
                    soul.overDress = false;
                    retired.Add(soul);
                }
                ResetCardValues(toBeRetired);
                toBeRetired.upright = true;
                toBeRetired.overDress = false;
                slots[location].soul.Clear();
                PlayerDrop.Insert(0, slots[location]);
                slots[location].location = Location.Drop;
                retired.Add(slots[location]);
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
                card.soul.Add(slots[location]);
                card.soul.AddRange(slots[location].soul);
                slots[location].soul.Clear();
                foreach (Card c in card.soul)
                {
                    c.location = Location.originalDress;
                    c.overDress = false;
                }
                ResetCardValues(slots[location]);
                slots[location] = card;
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
                slots[location] = card;
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
            _lastPlacedOnRC.Add(card);
        }

        public void SuperiorCall(int circle, int tempID)
        {
            Card ToBeCalled = _field.CardCatalog[tempID];
            int loc = ToBeCalled.location;
            Card[] slots = _field.Units;
            List<Card> location = null;
            _lastPlacedOnRC.Clear();
            circle = Convert(circle);
            if (loc == Location.Drop)
                location = PlayerDrop;
            else if (loc == Location.Deck)
                location = PlayerDeck;
            else if (loc == Location.Hand)
                location = PlayerHand;
            else if (loc == Location.Soul)
                location = _field.Units[PlayerVanguard].soul;
            else if (loc == Location.Prison)
                location = EnemyPrisoners;
            if (PlayerLooking.Contains(ToBeCalled))
                PlayerLooking.Remove(ToBeCalled);
            location.Remove(ToBeCalled);
            if (location == EnemyPrisoners)
                EnemyOrder.Remove(ToBeCalled);
            if (slots[circle] != null)
            {
                slots[circle].bonusPower = 0;
                slots[circle].upright = true;
                slots[circle].overDress = false;
                foreach (Card soul in slots[circle].soul)
                {
                    PlayerDrop.Insert(0, soul);
                    soul.location = Location.Drop;
                }

                slots[circle].soul.Clear();
                PlayerDrop.Insert(0, slots[circle]);
            }
            ToBeCalled.faceup = true;
            ToBeCalled.upright = true;
            ToBeCalled.location = Location.RC;
            slots[circle] = ToBeCalled;
            Console.WriteLine("----------\nSuperior Call! " + ToBeCalled.name + "!");
            if (ToBeCalled.orderType >= 0)
            {
                ResetCardValues(ToBeCalled);
                ToBeCalled.location = Location.Drop;
                PlayerDrop.Add(ToBeCalled);
            }
            _field.Shuffle(PlayerDeck);
            _lastPlacedOnRC.Add(ToBeCalled);
        }

        public void MoveRearguard(int location)
        {
            Card[] slots = _field.Units;
            Card temp;
            if (location == 0)
            {
                temp = slots[PlayerFrontLeft];
                slots[PlayerFrontLeft] = slots[PlayerBackLeft];
                slots[PlayerBackLeft] = temp;
                Console.WriteLine("----------\nLeft column Rearguard(s) changed position.");
            }
            else
            {
                temp = slots[PlayerFrontRight];
                slots[PlayerFrontRight] = slots[PlayerBackRight];
                slots[PlayerBackRight] = temp;
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

        public void ActivateACT(int selection)
        {

        }

        public void ActivateOrder(int selection)
        {

        }

        public void EnemyActivateOrder(int selection)
        {

        }

        public int FindAttacker(int selection)
        {
            int i = selection;
            if (_field.Units[PlayerFrontLeft] != null && _field.Units[PlayerFrontLeft].upright)
                selection--;
            if (selection == 0)
                return 0;
            if (_field.Units[PlayerVanguard].upright)
                selection--;
            if (selection == 0)
                return 1;
            return 2;
        }

        public int FindEnemyAttacker(int selection)
        {
            int i = selection;
            if (_field.Units[EnemyFrontLeft] != null && _field.Units[EnemyFrontLeft].upright)
                selection--;
            if (selection == 0)
                return 0;
            if (_field.Units[EnemyVanguard].upright)
                selection--;
            if (selection == 0)
                return 1;
            return 2;
        }

        public int FindTarget(int selection)
        {
            int i = selection;
            if (_field.Units[EnemyFrontLeft] != null && _field.Units[EnemyFrontLeft].upright)
                selection--;
            if (selection == 0)
                return 0;
            if (_field.Units[EnemyVanguard].upright)
                selection--;
            if (selection == 0)
                return 1;
            return 2;
        }

        public int FindEnemyTarget(int selection)
        {
            int i = selection;
            if (_field.Units[PlayerFrontLeft] != null && _field.Units[PlayerFrontLeft].upright)
                selection--;
            if (selection == 0)
                return 0;
            if (_field.Units[PlayerVanguard].upright)
                selection--;
            if (selection == 0)
                return 1;
            return 2;
        }

        public int FindBooster(Card card)
        {
            if (_field.Units[PlayerFrontLeft] != null && _field.Units[PlayerFrontLeft] == card)
                return 1;
            if (_field.Units[PlayerVanguard] == card)
                return 2;
            if (_field.Units[PlayerFrontRight] != null && _field.Units[PlayerFrontRight] == card)
                return 4;
            return -1;
        }

        public void Boost()
        {
            if (_field.Attacker == PlayerFrontLeft)
            {
                _field.Units[PlayerBackLeft].upright = false;
                _field.Booster = PlayerBackLeft;
                Console.WriteLine("----------\n" + _field.Units[PlayerBackLeft].name + " boosts " + _field.Units[PlayerFrontRight].name + "!");
            }
            else if (_field.Attacker == PlayerVanguard)
            {
                _field.Units[PlayerBackCenter].upright = false;
                _field.Booster = PlayerBackCenter;
                Console.WriteLine("----------\n" + _field.Units[PlayerBackCenter].name + " boosts " + _field.Units[PlayerVanguard].name + "!");
            }
            else if (_field.Attacker == PlayerFrontRight)
            {
                _field.Units[PlayerBackRight].upright = false;
                _field.Booster = PlayerBackRight;
                Console.WriteLine("----------\n" + _field.Units[PlayerBackRight].name + " boosts " + _field.Units[PlayerFrontRight].name + "!");
            }
            if (OnBoost != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.playerID = _playerID;
                args.card = _field.Units[_field.Booster];
                OnBoost(this, args);
            }
        }

        public void InitiateAttack(int selection, int target)
        {
            Card Attacker = _field.CardCatalog[selection];
            Card Attacked = _field.CardCatalog[target];
            Attacker.upright = false;
            _field.Attacker = GetCircle(Attacker);
            _field.Attacked = GetCircle(Attacked);
            Console.WriteLine("----------\n" + Attacker.name + " attacks " + Attacked.name + "!");
            if (OnAttack != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.playerID = _playerID;
                args.card = Attacker;
                args.i = _field.Attacked;
                OnAttack(this, args);
            }
        }

        public void Guard(int selection)
        {
            Card card = _field.CardCatalog[selection];
            _lastPlacedOnGC.Clear();
            if (card.location == Location.Hand)
                PlayerHand.Remove(card);
            else if (card.location == Location.RC)
                _field.Units[GetCircle(card)] = null;
            card.upright = false;
            card.location = Location.GC;
            _field.GC.Insert(0, card);
            _lastPlacedOnGC.Add(card);
            Console.WriteLine("----------\nAdded " + card.name + " to Guardian Circle.");
            if (OnGuard != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.playerID = _playerID;
                args.card = card;
                OnGuard(this, args);
            }
            return;
        }

        public void PerfectGuard()
        {
            _field.Sentinel = true;
        }

        public void TargetImmunity(int tempID)
        {
            foreach (Card card in _field.Units)
            {
                if (card != null && card.tempID == tempID)
                {
                    card.targetImmunity = true;
                    return;
                }
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
                    if (_field.Units[i] != null && _field.Units[i].tempID == tempID)
                    {
                        drop = EnemyDrop;
                        toBeRetired = _field.Units[i];
                        _field.Units[i] = null;
                        toBeRetired.location = Location.Drop;
                        toBeRetired.overDress = false;
                        drop.Insert(0, toBeRetired);
                        ResetCardValues(toBeRetired);
                        _enemyRetired = true;
                        _enemyRetiredThisTurn = true;
                        break;
                    }
                }
                for (int i = PlayerFrontLeft; i <= PlayerVanguard; i++)
                {
                    if (_field.Units[i] != null && _field.Units[i].tempID == tempID)
                    {
                        drop = PlayerDrop;
                        toBeRetired = _field.Units[i];
                        _field.Units[i] = null;
                        toBeRetired.location = Location.Drop;
                        toBeRetired.overDress = false;
                        drop.Insert(0, toBeRetired);
                        ResetCardValues(toBeRetired);
                        _playerRetired = true;
                        break;
                    }
                }
            }
        }

        public void RetireAttackedUnit()
        {
            List<int> list = new List<int>();
            list.Add(_field.Units[_field.Attacked].tempID);
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

        public void RetireGC()
        {
            foreach (Card card in _field.GC)
            {
                card.upright = true;
                card.location = Location.Drop;
                ResetCardValues(card);
                PlayerDrop.Insert(0, card);
            }
            _field.GC.Clear();
        }

        public int TriggerCheck(bool drivecheck)
        {
            Card trigger = PlayerDeck[0];
            trigger.upright = false;
            trigger.faceup = true;
            trigger.location = Location.Trigger;
            PlayerDeck.Remove(trigger);
            PlayerTrigger = trigger;
            _lastRevealedTriggers.Add(trigger);
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
            Card target = FindActiveUnit(selection);
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

        public void AddCritical(int selection, int critical)
        {
            Card target = FindActiveUnit(selection);
            target.tempPower += critical;
            Console.WriteLine("----------\n+" + critical + " critical to " + target.name + "!");
        }

        public void AddTempShield(int selection, int shield)
        {
            Card target = _field.CardCatalog[selection];
            target.tempShield += shield;
            Console.WriteLine("----------\n" + shield + " shield to " + target.name + "!");
        }

        public void SetAbilityPower(int tempID, int abilityNum, int selection, int power)
        {
            Card target = FindActiveUnit(selection);
            if (!target.abilityPower.ContainsKey(new Tuple<int, int>(tempID, abilityNum)))
            {
                target.abilityPower.Add(new Tuple<int, int>(tempID, abilityNum), power);
            }
            else
                target.abilityPower[new Tuple<int, int>(tempID, abilityNum)] = power;
        }

        public void SetAbilityShield(int tempID, int abilityNum, int selection, int shield)
        {
            Card target = FindActiveUnit(selection);
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
        
        public void Stand(int selection)
        {
            Card target = FindActiveUnit(selection);
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
            PlayerTrigger = null;
        }

        public void AddTriggerToHand()
        {
            Card card = PlayerTrigger;
            card.location = Location.Hand;
            PlayerTrigger = null;
            card.upright = true;
            PlayerHand.Add(card);
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
                cardToAdd.location = Location.Hand;
                PlayerHand.Add(cardToAdd);
            }
            Console.WriteLine(selections.Count + " card(s) added to hand.");
        }

        public void AddToSoul(List<int> selections)
        {
            Card cardToAdd;
            List<Card> location = null;
            foreach (int tempID in selections)
            {
                cardToAdd = _field.CardCatalog[tempID];
                if (cardToAdd.location == Location.RC)
                {
                    for (int i = PlayerFrontLeft; i < PlayerVanguard; i++)
                    {
                        if (_field.Units[i] != null && _field.Units[i].tempID == cardToAdd.tempID)
                            _field.Units[i] = null;
                    }
                }
                else if (cardToAdd.location == Location.RC)
                {
                    for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                    {
                        if (_field.Units[i] != null && _field.Units[i].tempID == cardToAdd.tempID)
                            _field.Units[i] = null;
                    }
                }
                else if (cardToAdd.location == Location.Deck)
                {
                    PlayerDeck.Remove(cardToAdd);
                }
                else if (cardToAdd.location == Location.Drop)
                {
                    _field.Units[PlayerVanguard].soul.Remove(cardToAdd);
                }
                if (PlayerLooking.Contains(cardToAdd))
                    PlayerLooking.Remove(cardToAdd);
                cardToAdd.location = Location.Soul;
                cardToAdd.faceup = true;
                cardToAdd.upright = true;
                ResetCardValues(cardToAdd);
                _field.Units[PlayerVanguard].soul.Add(cardToAdd);
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
                        if (_field.Units[i] != null && _field.Units[i].tempID == cardToAdd.tempID)
                            _field.Units[i] = null;
                    }
                }
                else if (cardToAdd.location == Location.RC)
                {
                    for (int i = EnemyFrontLeft; i < EnemyVanguard; i++)
                    {
                        if (_field.Units[i] != null && _field.Units[i].tempID == cardToAdd.tempID)
                            _field.Units[i] = null;
                    }
                }
                else if (cardToAdd.location == Location.Deck)
                {
                    PlayerDeck.Remove(cardToAdd);
                }
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
                        if (_field.Units[i] != null && _field.Units[i].tempID == cardToAdd.tempID)
                        {
                            _field.Units[i] = null;
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
            Card card = PlayerTrigger;
            card.location = Location.Damage;
            card.faceup = true;
            PlayerTrigger = null;
            PlayerDamage.Add(card);
            Console.WriteLine("----------\nDamage taken!");
        }

        public void PlayOrder(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            _playedOrder = card;
            PlayerHand.Remove(card);
            PlayerOrder.Add(card);
            card.location = Location.Order;
        }

        public void EndOrder()
        {
            if (_playedOrder != null)
            {
                PlayerOrder.Remove(_playedOrder);
                PlayerDrop.Add(_playedOrder);
                _playedOrder.location = Location.Drop;
                _playedOrder = null;
            }
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
                    _field.Units[GetCircle(card)] = null;
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

        public void CounterBlast(List<int> cardsToCB)
        {
            List<Card> damage = null;
            damage = PlayerDamage;
            foreach (int tempID in cardsToCB)
            {
                foreach (Card card in damage)
                {
                    if (card.tempID == tempID)
                    {
                        card.faceup = false;
                        break;
                    }
                }
            }
        }

        public void SoulBlast(List<int> cardsToSB)
        {
            List<Card> soul = null;
            List<Card> drop = null;
            soul = _field.Units[PlayerVanguard].soul;
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

        public void SoulCharge(int count)
        {
            Card card;
            int location = 0;
            List<Card> deck = null;
            location = PlayerVanguard;
            deck = PlayerDeck;
            Console.WriteLine("Soul Charging " + count + " card(s)!");
            for (int i = 0; i < count; i++)
            {
                card = deck[0];
                deck.RemoveAt(0);
                _field.Units[location].soul.Add(card);
                card.location = Location.Soul;
                card.faceup = true;
            }
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
            foreach (int tempID in cardsToStand)
            {
                _field.CardCatalog[tempID].upright = true;
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
            Card card;
            PlayerLooking.Clear();
            for (int i = 0; i < count; i++)
                PlayerLooking.Add(PlayerDeck[i]);
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

        public void ResetPower()
        {
            foreach (Card card in _field.Units)
            {
                if (card != null)
                {
                    card.tempPower = 0;
                    card.tempCritical = 0;
                }
            }
        }

        public void EndAttack()
        {
            _field.Sentinel = false;
            _field.Attacker = -1;
            _field.Attacked = -1;
            _field.Booster = -1;
            _lastRevealedTriggers.Clear();
            foreach (Card unit in _field.Units)
            {
                if (unit != null)
                    unit.battleOnlyPower = 0;
            }
        }

        public void EndTurn()
        {
            ResetPower();
            _riddenOnThisTurn.Clear();
            _finalRush = false;
            _field.IncrementTurn();
            _field.SetPersonaRide(false, _playerID);
            _field.SetPersonaRide(false, _enemyID);
            _canAttackFromBackRow.Clear();
            _bonusSkills.Clear();
        }

        public void ResetCardValues(Card card)
        {
            card.bonusPower = 0;
            card.battleOnlyPower = 0;
            card.tempPower = 0;
            card.tempShield = 0;
            card.tempCritical = 0;
            card.abilityPower.Clear();
            card.abilityDrive.Clear();
            foreach (Card item in _field.Units)
            {
                if (item != null)
                {
                    foreach (Tuple<int, int> key in item.abilityPower.Keys)
                    {
                        if (key.Item1 == card.tempID)
                        {
                            SetAbilityPower(key.Item1, key.Item2, item.tempID, 0);
                            SetAbilityShield(key.Item1, key.Item2, item.tempID, 0);
                            SetAbilityDrive(key.Item1, key.Item2, item.tempID, 0);
                        }
                    }
                }
            }
            card.targetImmunity = false;
            card.overDress = false;
            if (_bonusSkills.ContainsKey(card.tempID))
                _bonusSkills[card.tempID].Clear();
        }

        public void OnRideAbilityResolved(int tempID)
        { 
            foreach (Card card in _riddenOnThisTurn)
            {
                if (card.tempID == tempID)
                {
                    _riddenOnThisTurn.Remove(card);
                    break;
                }
            }
        }

        public bool isRearguard(int location)
        {
            if (location != 6 && location != 13)
                return true;
            return false;
        }

        public bool isPlayerFrontRow(int location)
        {
            if (location == PlayerFrontLeft || location == PlayerVanguard || location == PlayerFrontRight)
                return true;
            return false;
        }

        public bool isEnemyFrontRow(int location)
        {
            if (location == EnemyFrontLeft || location == EnemyVanguard || location == EnemyFrontRight)
                return true;
            return false;
        }

        public bool IsEnemy(int tempID)
        {
            Card card = _field.CardCatalog[tempID];
            for (int i = EnemyFrontLeft; i <= EnemyVanguard; i++)
            {
                if (_field.Units[i] != null && _field.Units[i].tempID == tempID)
                    return true;
            }
            return false;
        }

        public List<Card> Soul()
        {
            return _field.Units[PlayerVanguard].soul;
        }

        public List<Card> CardsForOnRide()
        {
            List<Card> cards =  new List<Card>();
            cards.AddRange(PlayerHand);
            for (int i = PlayerFrontLeft; i < PlayerVanguard + 1; i++)
                cards.Add(_field.Units[i]);
            cards.AddRange(Soul());
            return cards;
        }

        public List<Card> CardsForOnAttack()
        {
            List<Card> cards = new List<Card>();
            cards.AddRange(PlayerHand);
            foreach (Card card in _field.Units)
            {
                if (card != null)
                    cards.Add(card);
            }
            cards.AddRange(Soul());
            return cards;
        }

        public string PlayerVanguardName()
        {
            return _field.Units[PlayerVanguard].name;
        }

        public Card Vanguard()
        {
            return _field.Units[PlayerVanguard];
        }

        public Card AttackingUnit()
        {
            return _field.Units[_field.Attacker];
        }

        public Card Booster()
        {
            if (_field.Booster > 0)
                return _field.Units[_field.Booster];
            return null;
        }

        public Card AttackedUnit()
        {
            return _field.Units[_field.Attacked];
        }

        public int AttackingUnitLocation()
        {
            return _field.Attacker;
        }

        public int AttackingUnitID()
        {
            return _field.Units[_field.Attacker].tempID;
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
            if (card == _field.Units[PlayerVanguard].soul[0])
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
    }
}
