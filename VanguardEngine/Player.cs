using System;
using System.Collections.Generic;
using System.Text;

namespace VanguardEngine
{
    public class Player
    {
        protected Field _field;
        protected int _damage = 0;
        protected int _turn = 1;
        protected List<Card> _riddenOnThisTurn = new List<Card>();
        protected List<Card> _lastPlacedOnGC = new List<Card>();
        public int _playerID = 0;

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

        public void Initialize(List<Card> deck1, List<Card> deck2, Player player2, int playerID)
        {
            _field = new Field();
            _field.Initialize(deck1, deck2, player2);
            _playerID = playerID;
        }

        public int PlayerDeckCount()
        {
            return _field.PlayerDeck.Count;
        }

        public int EnemyDeckCount()
        {
            return _field.EnemyDeck.Count;
        }

        public int Turn
        {
            get => _turn;
        }

        public void IncrementTurn()
        {
            _turn++;
        }

        public List<Card> GetHand()
        {
            return _field.PlayerHand;
        }

        public List<Card> GetSoul()
        {
            return _field.Units[FL.PlayerVanguard].soul;
        }

        public List<Card> GetGC()
        {
            return _field.GC;
        }

        public List<Card> GetDrop()
        {
            return _field.PlayerDrop;
        }

        public List<Card> GetDeck()
        {
            return _field.PlayerDeck;
        }

        public List<Card> GetRiddenOnThisTurn()
        {
            return _riddenOnThisTurn;
        }

        public Card GetUnitAt(int circle)
        {
            return _field.Units[circle];
        }

        public void Draw(int count)
        {
            List<Card> cardsAdded = new List<Card>();
            CardEventArgs args = new CardEventArgs();
            for (int i = 0; i < count; i++)
            {
                _field.PlayerDeck[0].faceup = true;
                _field.PlayerHand.Add(_field.PlayerDeck[0]);
                _field.PlayerDeck[0].location = Location.PlayerHand;
                cardsAdded.Add(_field.PlayerDeck[0]);
                _field.PlayerDeck.Remove(_field.PlayerDeck[0]);
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

        public void EnemyDraw(int count)
        {
            List<Card> cardsAdded = new List<Card>();
            CardEventArgs args = new CardEventArgs();
            for (int i = 0; i < count; i++)
            {
                _field.EnemyDeck[0].faceup = true;
                _field.EnemyHand.Add(_field.EnemyDeck[0]);
                _field.EnemyDeck[0].location = Location.EnemyHand;
                cardsAdded.Add(_field.EnemyDeck[0]);
                _field.EnemyDeck.Remove(_field.EnemyDeck[0]);
            }
            //args.i = count;
            //args.cardList = cardsAdded;
            //args.player = false;
            //if (OnDraw != null)
            //{
            //    OnDraw(this, args);
            //}
        }

        public void MulliganCards(List<int> selection, bool player)
        {
            int i = 0;
            int draw = 0;
            if (player)
            {
                foreach (int tempID in selection)
                {
                    for (i = 0; i < _field.PlayerHand.Count; i++)
                    {
                        if (_field.PlayerHand[i].tempID == tempID)
                        {
                            ReturnCardFromHandToDeck(i, true);
                            draw++;
                            break;
                        }
                    }
                }
                Draw(draw);
            }
            else
            {
                foreach (int tempID in selection)
                {
                    for (i = 0; i < _field.EnemyHand.Count; i++)
                    {
                        if (_field.EnemyHand[i].tempID == tempID)
                        {
                            ReturnCardFromHandToDeck(i, false);
                            draw++;
                            break;
                        }
                    }
                }
                EnemyDraw(draw);
            }
        }

        public void ReturnCardFromHandToDeck(int selection, bool player)
        {
            CardEventArgs args = new CardEventArgs();
            Card card;
            if (player)
            {
                card = _field.PlayerHand[selection];
                _field.PlayerHand.RemoveAt(selection);
                card.faceup = false;
                card.location = Location.Deck;
                _field.PlayerDeck.Add(card);
                args.card = card;
                args.playerID = _playerID;
                if (OnReturnCardFromHandToDeck != null)
                {
                    OnReturnCardFromHandToDeck(this, args);
                }
            }
            else
            {
                card = _field.EnemyHand[selection];
                card.location = Location.Deck;
                _field.EnemyHand.RemoveAt(selection);
                _field.EnemyDeck.Add(card);
            }
        }

        public void Discard(List<int> list, bool player)
        {
            CardEventArgs args = new CardEventArgs();
            List<Card> toDiscard = new List<Card>();
            if (player)
            {
                foreach (int tempID in list)
                {
                    foreach(Card card in _field.PlayerHand)
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
                    _field.PlayerHand.Remove(card);
                    _field.PlayerDrop.Insert(0, card);
                    card.location = Location.Drop;
                }
                args.cardList = toDiscard;
                args.playerID = _playerID;
                if (OnDiscard != null)
                {
                    OnDiscard(this, args);
                }
            }
            else
            {
                foreach (int tempID in list)
                {
                    foreach (Card card in _field.EnemyHand)
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
                    _field.EnemyHand.Remove(card);
                    _field.EnemyDrop.Insert(0, card);
                    card.location = Location.Drop;
                }
            }
        }

        public void ShuffleDeck()
        {
            _field.sendShuffleKey(_field.PlayerDeck);
        }

        public void EnemyShuffleDeck()
        {
            _field.readShuffleKey(_field.EnemyDeck);
        }

        public void GiveShuffleKey(int[] key)
        {
            _field.ShuffleKey = key;
        }

        public void StandAll()
        {
            Card[] Units = _field.Units;
            foreach (Card card in Units)
            {
                if (card != null)
                    card.upright = true;
            }
        }

        public void EnemyStandAll()
        {
            Card[] Units = _field.Units;
            foreach (Card card in Units)
            {
                if (card != null)
                    card.upright = true;
            }
        }

        public void StandUpVanguard()
        {
            CardEventArgs args = new CardEventArgs();
            _field.Units[FL.PlayerVanguard].faceup = true;
            _field.Units[FL.EnemyVanguard].faceup = true;
            if (OnStandUpVanguard != null)
                OnStandUpVanguard(this, args);
        }

        public int PrintHand()
        {
            List<Card> hand = _field.PlayerHand;
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
            List<Card> hand = _field.EnemyHand;
            Card[] Units = _field.Units;
            List<Card> GC = _field.GC;
            if (Units[FL.EnemyVanguard].upright)
                estand = "S";
            else
                estand = "R";
            if (Units[FL.PlayerVanguard].upright)
                pstand = "S";
            else
                pstand = "R";
            output = "----------\nEnemy Hand: " + hand.Count + " Enemy Soul: " + Units[FL.EnemyVanguard].soul.Count + " Player Soul: " + Units[FL.PlayerVanguard].soul.Count + " Player Damage: " + _field.PlayerDamageZone.Count + " Enemy Damage: " + _field.EnemyDamageZone.Count + "\n" +
                "Choose circle to examine.\n" +
                "1. " + PrintRGData(FL.EnemyBackRight) + " | " + "2. " + PrintRGData(FL.EnemyBackMiddle) + " | " + "3. " + PrintRGData(FL.EnemyBackLeft) + "\n" +
                "4. " + PrintRGData(FL.EnemyFrontRight) + " | " + "5. " + CalculatePowerOfUnit(FL.EnemyVanguard) + " G" + Units[FL.EnemyVanguard].grade + " " + estand + " | 6. " + PrintRGData(FL.EnemyFrontLeft) + "\n" +
                "7.                 (to-do)\n" +
                "8. " + PrintRGData(FL.PlayerFrontLeft) + " | 9. " + CalculatePowerOfUnit(FL.PlayerVanguard) + " G" + Units[FL.PlayerVanguard].grade + " " + pstand + " | 10. " + PrintRGData(FL.PlayerFrontRight) + "\n" +
                "11. " + PrintRGData(FL.PlayerBackLeft) + " | 12. " + PrintRGData(FL.PlayerBackMiddle) + " | 13. " + PrintRGData(FL.PlayerBackRight) + "\n" +
                "14. Go back.";
            Console.WriteLine(output);
            return 14;
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
            List<Card> hand = _field.PlayerHand;
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
            List<Card> hand = _field.PlayerHand;
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
                "Grade: " + card.grade + " Power: " + card.power + " Shield: " + card.shield + "\n" +
                card.effect;
            Console.WriteLine("----------" + output);
        }

        public void DisplayVanguard(bool player)
        {
            if (player)
                DisplayCard(_field.Units[FL.PlayerVanguard]);
            else
                DisplayCard(_field.Units[FL.EnemyVanguard]);
        }

        public void DisplayRearguard(int selection)
        {
            if (selection == 1)
                DisplayCard(_field.Units[FL.EnemyBackRight]);
            else if (selection == 2)
                DisplayCard(_field.Units[FL.EnemyBackMiddle]);
            else if (selection == 3)
                DisplayCard(_field.Units[FL.EnemyBackLeft]);
            else if (selection == 4)
                DisplayCard(_field.Units[FL.EnemyFrontRight]);
            else if (selection == 6)
                DisplayCard(_field.Units[FL.EnemyFrontLeft]);
            else if (selection == 8)
                DisplayCard(_field.Units[FL.PlayerFrontLeft]);
            else if (selection == 10)
                DisplayCard(_field.Units[FL.PlayerFrontRight]);
            else if (selection == 11)
                DisplayCard(_field.Units[FL.PlayerBackRight]);
            else if (selection == 12)
                DisplayCard(_field.Units[FL.PlayerBackMiddle]);
            else if (selection == 13)
                DisplayCard(_field.Units[FL.PlayerBackRight]);
        }

        public List<Card> GetRideableCards(bool rideDeck)
        {
            List<Card> cards = new List<Card>();
            if (rideDeck)
            {
                foreach (Card card in _field.PlayerRideDeck)
                {
                    if (card.grade == _field.Units[FL.PlayerVanguard].grade || card.grade == _field.Units[FL.PlayerVanguard].grade + 1)
                        cards.Add(card);
                }
            }
            else
            {
                foreach (Card card in _field.PlayerHand)
                {
                    if (card.grade == _field.Units[FL.PlayerVanguard].grade || card.grade == _field.Units[FL.PlayerVanguard].grade + 1)
                        cards.Add(card);
                }
            }
            return cards;
        }

        public List<Card> GetCallableRearguards()
        {
            List<Card> hand = _field.PlayerHand;
            List<Card> callableCards = new List<Card>();
            Card VG = _field.Units[FL.PlayerVanguard];
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
                if (_field.Units[FL.PlayerFrontLeft] != null || _field.Units[FL.PlayerBackLeft] != null)
                    return true;
                return false;
            }
            else
            {
                if (_field.Units[FL.PlayerFrontRight] != null || _field.Units[FL.PlayerBackRight] != null)
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

        public List<Card> GetStandingFrontRow()
        {
            List<Card> cards = new List<Card>();
            if (_field.Units[FL.PlayerFrontLeft] != null && _field.Units[FL.PlayerFrontLeft].upright)
                cards.Add(_field.Units[FL.PlayerFrontLeft]);
            if (_field.Units[FL.PlayerVanguard].upright)
                cards.Add(_field.Units[FL.PlayerVanguard]);
            if (_field.Units[FL.PlayerFrontRight] != null && _field.Units[FL.PlayerFrontRight].upright)
                cards.Add(_field.Units[FL.PlayerFrontRight]);
            return cards;
        }

        public List<Card> GetEnemyStandingFrontRow()
        {
            List<Card> cards = new List<Card>();
            if (_field.Units[FL.EnemyFrontLeft] != null && _field.Units[FL.EnemyFrontLeft].upright)
                cards.Add(_field.Units[FL.EnemyFrontLeft]);
            if (_field.Units[FL.EnemyVanguard].upright)
                cards.Add(_field.Units[FL.EnemyVanguard]);
            if (_field.Units[FL.EnemyFrontRight] != null && _field.Units[FL.EnemyFrontRight].upright)
                cards.Add(_field.Units[FL.EnemyFrontRight]);
            return cards;
        }

        public List<Card> GetEnemyFrontRow()
        {
            List<Card> cards = new List<Card>();
            if (_field.Units[FL.EnemyFrontLeft] != null)
                cards.Add(_field.Units[FL.EnemyFrontLeft]);
            cards.Add(_field.Units[FL.EnemyVanguard]);
            if (_field.Units[FL.EnemyFrontRight] != null)
                cards.Add(_field.Units[FL.EnemyFrontRight]);
            return cards;
        }

        public List<Card> GetPlayerFrontRow()
        {
            List<Card> cards = new List<Card>();
            if (_field.Units[FL.PlayerFrontLeft] != null)
                cards.Add(_field.Units[FL.PlayerFrontLeft]);
            cards.Add(_field.Units[FL.PlayerVanguard]);
            if (_field.Units[FL.PlayerFrontRight] != null)
                cards.Add(_field.Units[FL.PlayerFrontRight]);
            return cards;
        }

        public List<Card> GetGuardableCards()
        {
            List<Card> cards = new List<Card>();
            foreach (Card card in _field.PlayerHand)
            {
                if (card.orderType < 0)
                    cards.Add(card);
            }
            return cards;
        }

        public List<Card> GetActiveUnits()
        {
            List<Card> cards = new List<Card>();
            for (int i = FL.PlayerFrontLeft; i <= FL.PlayerVanguard; i++)
            {
                if (_field.Units[i] != null)
                    cards.Add(_field.Units[i]);
            }
            return cards;
        }

        public List<Card> GetAllUnitsOnField()
        {
            List<Card> cards = new List<Card>();
            for (int i = FL.PlayerFrontLeft; i <= FL.PlayerVanguard; i++)
            {
                if (_field.Units[i] != null)
                    cards.Add(_field.Units[i]);
            }
            for (int i = FL.EnemyFrontLeft; i <= FL.EnemyVanguard; i++)
            {
                if (_field.Units[i] != null)
                    cards.Add(_field.Units[i]);
            }
            return cards;
        }

        public List<Card> GetDamageZone()
        {
            return _field.PlayerDamageZone;
        }

        public int CalculateShield()
        {
            int shield = 0;
            if (_field.Sentinel)
                return 1000000000;
            foreach (Card card in _field.GC)
                shield += card.shield;
            return shield;
        }

        public void PrintShield()
        {
            if (_field.Sentinel)
            {
                Console.WriteLine("Perfect Shield is active.");
                return;
            }
            Console.WriteLine("Shield: " + CalculateShield());
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
            int power = Units[location].power + Units[location].bonusPower + Units[location].tempPower;
            if (_field.Booster >= 0)
            {
                if (_field.Guarding && isEnemyFrontRow(location))
                    power += CalculatePowerOfUnit(_field.Booster);
                else if (!_field.Guarding && isPlayerFrontRow(location))
                    power += CalculatePowerOfUnit(_field.Booster);
            }
            return power;
        }

        public int PrintActiveUnits()
        {
            int max = 0;
            //PrintAttack();
            string output;
            if (_field.Units[FL.PlayerFrontLeft] != null)
            {
                max++;
                output = max + ". FrontLeft " + _field.Units[FL.PlayerFrontLeft].name + " ";
                if (!_field.Guarding && _field.Booster == 0)
                    output += (_field.Units[FL.PlayerFrontLeft].power + _field.Units[FL.PlayerFrontLeft].bonusPower + _field.Units[FL.PlayerBackLeft].power + _field.Units[FL.PlayerBackLeft].bonusPower);
                else
                    output += (_field.Units[FL.PlayerFrontLeft].power + _field.Units[FL.PlayerFrontLeft].bonusPower);
                Console.WriteLine(output);
            }
            max++;
            output = max + ". FrontMiddle " + _field.Units[FL.PlayerVanguard].name + " ";
            if (!_field.Guarding && _field.Booster == 1)
                output += (_field.Units[FL.PlayerVanguard].power + _field.Units[FL.PlayerVanguard].bonusPower + _field.Units[FL.PlayerBackMiddle].power + _field.Units[FL.PlayerBackMiddle].bonusPower);
            else
                output += (_field.Units[FL.PlayerVanguard].power + _field.Units[FL.PlayerVanguard].bonusPower);
            Console.WriteLine(output);
            if (_field.Units[FL.PlayerFrontRight] != null)
            {
                max++;
                output = max + ". FrontMiddle " + _field.Units[FL.PlayerFrontRight].name + " ";
                if (!_field.Guarding && _field.Booster == 2)
                    output += (_field.Units[FL.PlayerFrontRight].power + _field.Units[FL.PlayerFrontRight].bonusPower + _field.Units[FL.PlayerBackRight].power + _field.Units[FL.PlayerBackRight].bonusPower);
                else
                    output += (_field.Units[FL.PlayerBackRight].power + _field.Units[FL.PlayerBackRight].bonusPower);
                Console.WriteLine(output);
            }
            return max;
        }

        public int PrintDamageZone()
        {
            int i = 0;
            Console.WriteLine("Choose damage to heal.");
            string output;
            for (i = 0; i < _field.PlayerDamageZone.Count; i++)
            {
                output = i + 1 + ". " + _field.PlayerDamageZone[i].name + " ";
                if (_field.PlayerDamageZone[i].faceup == false)
                    Console.WriteLine(output + "(facedown).");
                else
                    Console.WriteLine(output + "(faceup).");
            }
            return i + 1;
        }

        public bool CanRideFromRideDeck()
        {
            List<Card> rideDeck = _field.PlayerRideDeck;
            Card VG = _field.Units[FL.PlayerVanguard];
            foreach (Card card in rideDeck)
            {
                if (VG.grade + 1 == card.grade)
                    return true;
            }
            return false;
        }

        public bool CanRideFromHand()
        {
            List<Card> hand = _field.PlayerHand;
            Card VG = _field.Units[FL.PlayerVanguard];
            foreach (Card card in hand)
            {
                if (card.grade == VG.grade || card.grade - 1 == VG.grade)
                    return true;
            }
            return false;
        }

        public bool CanCallRearguard()
        {
            List<Card> hand = _field.PlayerHand;
            Card VG = _field.Units[FL.PlayerVanguard];
            foreach (Card card in hand)
            {
                if (card.grade <= VG.grade && card.orderType < 0)
                    return true;
            }
            return false;
        }

        public bool CanMoveRearguard()
        {
            Card[] slots = _field.Units;
            foreach (Card card in slots)
            {
                if (card != null && card != slots[FL.PlayerVanguard])
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
            if (_field.Units[FL.PlayerVanguard].upright || (_field.Units[FL.PlayerFrontLeft] != null && _field.Units[FL.PlayerFrontLeft].upright) || (_field.Units[FL.PlayerFrontRight] != null && _field.Units[FL.PlayerFrontRight].upright))
                return true;
            return false;
        }

        public bool CanBeBoosted()
        {
            if (_field.Attacker == FL.PlayerFrontLeft)
            {
                if (_field.Units[FL.PlayerBackRight] != null && _field.Units[FL.PlayerBackRight].upright)
                    return true;
            }
            else if (_field.Attacker == FL.PlayerVanguard)
            {
                if (_field.Units[FL.PlayerBackMiddle] != null && _field.Units[FL.PlayerBackMiddle].upright)
                    return true;
            }
            else
            {
                if (_field.Units[FL.PlayerBackRight] != null && _field.Units[FL.PlayerBackRight].upright)
                    return true;
            }
            return false;
        }

        public bool CanGuard()
        {
            foreach (Card card in _field.PlayerHand)
            {
                if (card.orderType < 0)
                    return true;
            }
            return false;
        }

        public bool CanHeal()
        {
            if (_field.PlayerDamageZone.Count > 0 && _field.PlayerDamageZone.Count >= _field.EnemyDamageZone.Count)
                return true;
            return false;
        }

        public bool TargetIsVanguard(bool player)
        {
            int Vanguard;
            if (player)
                Vanguard = FL.PlayerVanguard;
            else
                Vanguard = FL.EnemyVanguard;
            if (_field.Attacked == Vanguard)
                return true;
            return false;
        }

        public bool IsPlacedOnGC(int tempID)
        {
            foreach (Card card in _lastPlacedOnGC)
            {
                if (card.tempID == tempID)
                    return true;
            }
            return false;
        }

        public int Drive()
        {
            return _field.Units[FL.PlayerVanguard].drive;
        }

        public int Critical()
        {
            return _field.Units[FL.PlayerVanguard].critical; 
        }

        public int Damage()
        {
            return _field.PlayerDamageZone.Count; 
        }

        public Card TriggerZone()
        {
            return null;
        }

        public Card EnemyTriggerZone()
        {
            return null;
        }

        public void RecognizeAttack(int attack)
        {
            _field.Attack = attack;
        }

        public bool AttackHits()
        {
            Card attacker = _field.Units[_field.Attacker];
            Card card = _field.Units[_field.Attacked];
            CardEventArgs args = new CardEventArgs();
            if (CalculatePowerOfUnit(_field.Attacker) >= CalculateShield() + card.power + card.bonusPower)
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
            if (_field.Attacker == FL.PlayerVanguard)
                return true;
            else
                return false;
        }

        public void Ride(int location, int selection, bool player)
        {
            CardEventArgs args = new CardEventArgs();
            List<Card> list;
            Card card = null;
            if (player)
            {
                _riddenOnThisTurn.Add(_field.Units[FL.PlayerVanguard]);
                if (location == 0)
                    list = _field.PlayerRideDeck;
                else
                    list = _field.PlayerHand;
                foreach (Card item in list)
                {
                    if (item.tempID == selection)
                    {
                        card = item;
                        break;
                    }
                }
                list.Remove(card);
                card.faceup = true;
                card.soul.AddRange(_field.Units[FL.PlayerVanguard].soul);
                card.soul.Insert(0, _field.Units[FL.PlayerVanguard]);
                _field.Units[FL.PlayerVanguard].location = Location.Soul;
                _field.Units[FL.PlayerVanguard] = card;
                card.location = Location.PlayerVC;
                card.soul[0].soul.Clear();
                card.soul[0].bonusPower = 0;
                Console.WriteLine("---------\nRide!! " + _field.Units[FL.PlayerVanguard].name + "!");
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
            else
            {
                if (location == 0)
                    list = _field.EnemyRideDeck;
                else
                    list = _field.EnemyHand;
                foreach (Card item in list)
                {
                    if (item.tempID == selection)
                    {
                        card = item;
                        break;
                    }
                }
                list.Remove(card);
                card.faceup = true;
                card.location = Location.EnemyVC;
                card.soul.AddRange(_field.Units[FL.EnemyVanguard].soul);
                card.soul.Insert(0, _field.Units[FL.EnemyVanguard]);
                _field.Units[FL.EnemyVanguard].location = Location.Soul;
                _field.Units[FL.EnemyVanguard] = card;
                card.soul[0].soul.Clear();
                card.soul[0].bonusPower = 0;
            }
        }

        public void Call(int location, int selection, bool player, bool overDress)
        {
            List<Card> hand;
            Card[] slots = _field.Units;
            Card card = null;
            Card toBeRetired = null;
            List<Card> retired = new List<Card>();
            CardEventArgs args;
            if (player)
            {
                hand = _field.PlayerHand;
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
                        _field.PlayerDrop.Insert(0, soul);
                        soul.location = Location.Drop;
                        soul.overDress = false;
                        retired.Add(soul);
                    }
                    slots[location] = null;
                    toBeRetired.bonusPower = 0;
                    toBeRetired.upright = true;
                    toBeRetired.overDress = false;
                    slots[location].soul.Clear();
                    _field.PlayerDrop.Insert(0, slots[location]);
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
                    slots[location] = card;
                    card.location = Location.PlayerRC;
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
                    card.location = Location.PlayerRC;
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
            }
            else
            {
                hand = _field.EnemyHand;
                foreach (Card item in hand)
                {
                    if (item.tempID == selection)
                    {
                        card = item;
                        break;
                    }
                }
                location = FL.SwitchSides(location);
                if (!overDress && slots[location] != null)
                {
                    toBeRetired = slots[location];
                    foreach (Card soul in slots[location].soul)
                    {
                        _field.EnemyDrop.Insert(0, soul);
                        soul.location = Location.Drop;
                        soul.overDress = false;
                    }
                    slots[location] = null;
                    toBeRetired.bonusPower = 0;
                    toBeRetired.upright = true;
                    toBeRetired.overDress = false;
                    slots[location].soul.Clear();
                    slots[location].location = Location.Drop;
                    _field.EnemyDrop.Insert(0, slots[location]);
                }
                hand.Remove(card);
                if (overDress)
                {
                    card.soul.Add(slots[location]);
                    card.soul.AddRange(slots[location].soul);
                    slots[location].soul.Clear();
                    foreach (Card c in card.soul)
                        c.location = Location.originalDress;
                    slots[location] = card;
                    card.location = Location.EnemyRC;
                    card.overDress = true;
                }
                else
                {
                    card.faceup = true;
                    slots[location] = card;
                    card.location = Location.EnemyRC;
                }
            }
        }

        public void SuperiorCall(int circle, int tempID, int loc, bool player)
        {
            Card ToBeCalled = null;
            Card[] slots = _field.Units;
            List<Card> location = null;
            if (player)
            {
                if (loc == Location.Drop)
                    location = _field.PlayerDrop;
                else if (loc == Location.Deck)
                    location = _field.PlayerDeck;
                else if (loc == Location.PlayerHand)
                    location = _field.PlayerHand;
                foreach (Card card in location)
                {
                    if (card.tempID == tempID)
                    {
                        ToBeCalled = card;
                        location.Remove(card);
                        break;
                    }
                }
                if (slots[circle] != null)
                {
                    slots[circle].bonusPower = 0;
                    slots[circle].upright = true;
                    slots[circle].overDress = false;
                    foreach (Card soul in slots[circle].soul)
                    {
                        _field.PlayerDrop.Insert(0, soul);
                        soul.location = Location.Drop;
                    }

                    slots[circle].soul.Clear();
                    _field.PlayerDrop.Insert(0, slots[circle]);
                }
                ToBeCalled.faceup = true;
                ToBeCalled.location = Location.PlayerRC;
                slots[circle] = ToBeCalled;
                Console.WriteLine("----------\nSuperior Call! " + ToBeCalled.name + "!");
                ShuffleDeck();
            }
            else
            {
                if (loc == Location.Drop)
                    location = _field.EnemyDrop;
                else if (loc == Location.Deck)
                    location = _field.EnemyDeck;
                else if (loc == Location.EnemyHand)
                    location = _field.EnemyHand;
                foreach (Card card in location)
                {
                    if (card.tempID == tempID)
                    {
                        ToBeCalled = card;
                        location.Remove(card);
                        break;
                    }
                }
                circle = FL.SwitchSides(circle);
                if (slots[circle] != null)
                {
                    slots[circle].bonusPower = 0;
                    slots[circle].upright = true;
                    slots[circle].overDress = false;
                    foreach (Card soul in slots[circle].soul)
                    {
                        _field.EnemyDrop.Insert(0, soul);
                        soul.location = Location.Drop;
                    }

                    slots[circle].soul.Clear();
                    _field.EnemyDrop.Insert(0, slots[circle]);
                }
                ToBeCalled.faceup = true;
                ToBeCalled.location = Location.EnemyRC;
                slots[circle] = ToBeCalled;
                EnemyShuffleDeck();
            }
        }

        public void MoveRearguard(int location)
        {
            Card[] slots = _field.Units;
            Card temp;
            if (location == 0)
            {
                temp = slots[FL.PlayerFrontLeft];
                slots[FL.PlayerFrontLeft] = slots[FL.PlayerBackLeft];
                slots[FL.PlayerBackLeft] = temp;
                Console.WriteLine("----------\nLeft column Rearguard(s) changed position.");
            }
            else
            {
                temp = slots[FL.PlayerFrontRight];
                slots[FL.PlayerFrontRight] = slots[FL.PlayerBackRight];
                slots[FL.PlayerBackRight] = temp;
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

        public void EnemyMoveRearguard(int location)
        {
            Card[] slots = _field.Units;
            Card temp;
            if (location == 0)
            {
                temp = slots[FL.EnemyFrontLeft];
                slots[FL.EnemyFrontLeft] = slots[FL.EnemyBackLeft];
                slots[FL.EnemyBackLeft] = temp;
            }
            else
            {
                temp = slots[FL.EnemyFrontRight];
                slots[FL.EnemyFrontRight] = slots[FL.EnemyBackRight];
                slots[FL.EnemyBackRight] = temp;
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
            if (_field.Units[FL.PlayerFrontLeft] != null && _field.Units[FL.PlayerFrontLeft].upright)
                selection--;
            if (selection == 0)
                return 0;
            if (_field.Units[FL.PlayerVanguard].upright)
                selection--;
            if (selection == 0)
                return 1;
            return 2;
        }

        public int FindEnemyAttacker(int selection)
        {
            int i = selection;
            if (_field.Units[FL.EnemyFrontLeft] != null && _field.Units[FL.EnemyFrontLeft].upright)
                selection--;
            if (selection == 0)
                return 0;
            if (_field.Units[FL.EnemyVanguard].upright)
                selection--;
            if (selection == 0)
                return 1;
            return 2;
        }

        public int FindTarget(int selection)
        {
            int i = selection;
            if (_field.Units[FL.EnemyFrontLeft] != null && _field.Units[FL.EnemyFrontLeft].upright)
                selection--;
            if (selection == 0)
                return 0;
            if (_field.Units[FL.EnemyVanguard].upright)
                selection--;
            if (selection == 0)
                return 1;
            return 2;
        }

        public int FindEnemyTarget(int selection)
        {
            int i = selection;
            if (_field.Units[FL.PlayerFrontLeft] != null && _field.Units[FL.PlayerFrontLeft].upright)
                selection--;
            if (selection == 0)
                return 0;
            if (_field.Units[FL.PlayerVanguard].upright)
                selection--;
            if (selection == 0)
                return 1;
            return 2;
        }

        public int FindBooster(Card card)
        {
            if (_field.Units[FL.PlayerFrontLeft] != null && _field.Units[FL.PlayerFrontLeft] == card)
                return 1;
            if (_field.Units[FL.PlayerVanguard] == card)
                return 2;
            if (_field.Units[FL.PlayerFrontRight] != null && _field.Units[FL.PlayerFrontRight] == card)
                return 4;
            return -1;
        }

        public void Boost()
        {
            if (_field.Attacker == FL.PlayerFrontLeft)
            {
                _field.Units[FL.PlayerBackLeft].upright = false;
                _field.Booster = FL.PlayerBackLeft;
                Console.WriteLine("----------\n" + _field.Units[FL.PlayerBackLeft].name + " boosts " + _field.Units[FL.PlayerFrontRight].name + "!");
            }
            else if (_field.Attacker == FL.PlayerVanguard)
            {
                _field.Units[FL.PlayerBackMiddle].upright = false;
                _field.Booster = FL.PlayerBackMiddle;
                Console.WriteLine("----------\n" + _field.Units[FL.PlayerBackMiddle].name + " boosts " + _field.Units[FL.PlayerVanguard].name + "!");
            }
            else if (_field.Attacker == FL.PlayerFrontRight)
            {
                _field.Units[FL.PlayerBackRight].upright = false;
                _field.Booster = FL.PlayerBackRight;
                Console.WriteLine("----------\n" + _field.Units[FL.PlayerBackRight].name + " boosts " + _field.Units[FL.PlayerFrontRight].name + "!");
            }
            if (OnBoost != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.playerID = _playerID;
                args.card = _field.Units[_field.Booster];
                OnBoost(this, args);
            }
        }

        public void EnemyBoost()
        {
            if (_field.Attacker == FL.PlayerFrontLeft)
            {
                _field.Units[FL.EnemyBackLeft].upright = false;
                _field.Booster = FL.EnemyBackLeft;
            }
            else if (_field.Attacker == FL.PlayerVanguard)
            {
                _field.Units[FL.EnemyBackMiddle].upright = false;
                _field.Booster = FL.EnemyBackMiddle;
            }
            else if (_field.Attacker == FL.PlayerFrontRight)
            {
                _field.Units[FL.EnemyBackRight].upright = false;
                _field.Booster = FL.EnemyBackRight;
            }
        }

        public void InitiateAttack(int selection, int target, bool player)
        {
            Card Attacker = null;
            Card Attacked = null;
            List<Card> list = null;
            if (player)
            {
                list = GetStandingFrontRow();
                foreach (Card card in list)
                {
                    if (card.tempID == selection)
                    {
                        Attacker = card;
                        break;
                    }
                }
                list = GetEnemyFrontRow();
                foreach (Card card in list)
                {
                    if (card.tempID == target)
                    {
                        Attacked = card;
                        break;
                    }
                }
                if (Attacker == _field.Units[FL.PlayerFrontLeft])
                {
                    _field.Attacker = FL.PlayerFrontLeft;
                    _field.Units[FL.PlayerFrontLeft].upright = false;
                }
                else if (Attacker == _field.Units[FL.PlayerVanguard])
                {
                    _field.Attacker = FL.PlayerVanguard;
                    _field.Units[FL.PlayerVanguard].upright = false;
                }
                else
                {
                    _field.Attacker = FL.PlayerFrontRight;
                    _field.Units[FL.PlayerFrontRight].upright = false;
                }
                if (Attacked == _field.Units[FL.EnemyFrontLeft])
                    _field.Attacked = FL.EnemyFrontLeft;
                else if (Attacked == _field.Units[FL.EnemyVanguard])
                    _field.Attacked = FL.EnemyVanguard;
                else
                    _field.Attacked = FL.EnemyFrontRight;
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
            else
            {
                list = GetEnemyStandingFrontRow();
                foreach (Card card in list)
                {
                    if (card.tempID == selection)
                    {
                        Attacker = card;
                        break;
                    }
                }
                list = GetPlayerFrontRow();
                foreach (Card card in list)
                {
                    if (card.tempID == target)
                    {
                        Attacked = card;
                    }
                }
                if (Attacker == _field.Units[FL.EnemyFrontLeft])
                {
                    _field.Attacker = FL.EnemyFrontLeft;
                    _field.Units[FL.EnemyFrontLeft].upright = false;
                }
                else if (Attacker == _field.Units[FL.EnemyVanguard])
                {
                    _field.Attacker = FL.EnemyVanguard;
                    _field.Units[FL.EnemyVanguard].upright = false;
                }
                else
                {
                    _field.Attacker = FL.EnemyFrontRight;
                    _field.Units[FL.EnemyFrontRight].upright = false;
                }
                if (Attacked == _field.Units[FL.PlayerFrontLeft])
                    _field.Attacked = FL.PlayerFrontLeft;
                else if (Attacked == _field.Units[FL.PlayerVanguard])
                    _field.Attacked = FL.PlayerVanguard;
                else
                    _field.Attacked = FL.PlayerFrontRight;
            }
        }

        public void Guard(int selection, bool player)
        {
            _lastPlacedOnGC.Clear();
            if (player)
            {
                _field.Guarding = true;
                foreach (Card card in _field.PlayerHand)
                {
                    if (card.tempID == selection)
                    {
                        _field.PlayerHand.Remove(card);
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
                }
            }
            else
            {
                foreach (Card card in _field.EnemyHand)
                {
                    if (card.tempID == selection)
                    {
                        _field.EnemyHand.Remove(card);
                        card.upright = false;
                        card.location = Location.GC;
                        _field.GC.Insert(0, card);
                        _lastPlacedOnGC.Add(card);
                        return;
                    }
                }
            }
        }

        public void PerfectGuard()
        {
            _field.Sentinel = true;
        }

        public void Retire(int tempID)
        {
            List<Card> drop;
            Card toBeRetired;
            for (int i = FL.EnemyFrontLeft; i <= FL.EnemyVanguard; i++)
            {
                if (_field.Units[i] != null && _field.Units[i].tempID == tempID)
                {
                    drop = _field.EnemyDrop;
                    toBeRetired = _field.Units[i];
                    _field.Units[i] = null;
                    toBeRetired.location = Location.Drop;
                    toBeRetired.overDress = false;
                    drop.Insert(0, toBeRetired);
                    return;
                }
            }
            for (int i = FL.PlayerFrontLeft; i <= FL.PlayerVanguard; i++)
            {
                if (_field.Units[i] != null && _field.Units[i].tempID == tempID)
                {
                    drop = _field.PlayerDrop;
                    toBeRetired = _field.Units[i];
                    _field.Units[i] = null;
                    toBeRetired.location = Location.Drop;
                    toBeRetired.overDress = false;
                    drop.Insert(0, toBeRetired);
                    return;
                }
            }
        }

        public void RetireGC()
        {
            foreach (Card card in _field.GC)
            {
                card.upright = true;
                card.location = Location.Drop;
                _field.EnemyDrop.Insert(0, card);
            }
            _field.GC.Clear();
        }

        public void EnemyRetireGC()
        {
            foreach (Card card in _field.GC)
            {
                card.upright = true;
                card.location = Location.Drop;
                _field.PlayerDrop.Insert(0, card);
            }
            _field.GC.Clear();
        }

        public int TriggerCheck(bool drivecheck)
        {
            Card trigger = _field.PlayerDeck[0];
            trigger.upright = false;
            trigger.faceup = true;
            trigger.location = Location.Trigger;
            _field.PlayerDeck.Remove(trigger);
            _field.PlayerTrigger = trigger;
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

        public int EnemyTriggerCheck()
        {
            Card trigger = _field.EnemyDeck[0];
            trigger.upright = false;
            trigger.faceup = true;
            trigger.location = Location.Trigger;
            _field.EnemyDeck.Remove(trigger);
            _field.EnemyTrigger = trigger;
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

        public void AddTempPower(int selection, int power, bool player)
        {
            Card target = FindActiveUnit(selection);
            target.tempPower += power;
            if (player)
                Console.WriteLine("----------\n" + power + " power to " + target.name + "!");
        }

        public void AddAbilityPower(int tempID, int abilityNum, int selection, int power)
        {
            Card target = FindActiveUnit(selection);
            if (!target.abilityPower.ContainsKey(new Tuple<int, int>(tempID, abilityNum)))
            {
                target.abilityPower.Add(new Tuple<int, int>(tempID, abilityNum), true);
                target.bonusPower += power;
            }
            else
            {
                if (!target.abilityPower[new Tuple<int, int>(tempID, abilityNum)])
                {
                    target.abilityPower[new Tuple<int, int>(tempID, abilityNum)] = true;
                    target.bonusPower += power;
                }
            }
        }

        public void AddCritical(int selection, int critical)
        {
            Card target = FindActiveUnit(selection);
            target.critical += critical;
        }
        
        public void Stand(int selection)
        {
            Card target = FindActiveUnit(selection);
            target.upright = true;
        }

        public void Heal(int selection, bool player)
        {
            if (player)
            {
                foreach (Card card in _field.PlayerDamageZone)
                {
                    if (card.tempID == selection)
                    {
                        card.faceup = true;
                        card.upright = true;
                        card.location = Location.Drop;
                        _field.PlayerDamageZone.Remove(card);
                        _field.PlayerDrop.Insert(0, card);
                        Console.WriteLine("----------\nDamage healed!");
                        return;
                    }
                }
            }
            else
            {
                foreach (Card card in _field.EnemyDamageZone)
                {
                    if (card.tempID == selection)
                    {
                        card.faceup = true;
                        card.upright = true;
                        card.location = Location.Drop;
                        _field.EnemyDamageZone.Remove(card);
                        _field.EnemyDrop.Insert(0, card);
                        Console.WriteLine("----------\nDamage healed!");
                        return;
                    }
                }
            }
        }

        public void RemoveTrigger()
        {
            _field.PlayerTrigger = null;
        }

        public void RemoveEnemyTrigger()
        {
            _field.EnemyTrigger = null;
        }

        public void AddTriggerToHand()
        {
            Card card = _field.PlayerTrigger;
            card.location = Location.PlayerHand;
            _field.PlayerTrigger = null;
            card.upright = true;
            _field.PlayerHand.Add(card);
        }

        public void EnemyAddTriggerToHand()
        {
            Card card = _field.EnemyTrigger;
            card.location = Location.EnemyHand;
            _field.EnemyTrigger = null;
            card.upright = true;
            _field.EnemyHand.Add(card);
        }

        public void Remove(Card card)
        {

        }

        public void EnemyRemove(Card card)
        {

        }

        public void AddToHand(Card card)
        {

        }

        public void EnemyAddToHand(Card card)
        {

        }

        public void TakeDamage()
        {
            Card card = _field.PlayerTrigger;
            card.location = Location.Damage;
            _field.PlayerTrigger = null;
            _field.PlayerDamageZone.Add(card);
            Console.WriteLine("----------\nDamage taken!");
        }

        public void EnemyTakeDamage()
        {
            Card card = _field.EnemyTrigger;
            card.location = Location.Damage;
            _field.EnemyTrigger = null;
            _field.EnemyDamageZone.Add(card);
        }

        public void CounterBlast(List<int> cardsToCB, bool player)
        {
            List<Card> damage = null;
            if (player)
                damage = _field.PlayerDamageZone;
            else
                damage = _field.EnemyDamageZone;
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

        public void SoulBlast(List<int> cardsToSB, bool player)
        {
            List<Card> soul = null;
            List<Card> drop = null;
            if (player)
            {
                soul = _field.Units[FL.PlayerVanguard].soul;
                drop = _field.PlayerDrop;
            }
            else
            {
                soul = _field.Units[FL.EnemyVanguard].soul;
                drop = _field.EnemyDrop;
            }
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

        public void Search(List<int> cardsToSearch, bool player)
        {
            List<Card> hand = null;
            List<Card> deck = null;
            int handLocation = 0;
            if (player)
            {
                hand = _field.PlayerHand;
                deck = _field.PlayerDeck;
                handLocation = Location.PlayerHand;
            }
            else
            {
                hand = _field.EnemyHand;
                deck = _field.EnemyDeck;
                handLocation = Location.EnemyHand;
            }
            foreach (int tempID in cardsToSearch)
            {
                foreach (Card card in deck)
                {
                    if (card.tempID == tempID)
                    {
                        deck.Remove(card);
                        if (player)
                            card.faceup = true;
                        hand.Add(card);
                        card.location = handLocation;
                        if (player)
                            ShuffleDeck();
                        else
                            EnemyShuffleDeck();
                        break;
                    }
                }
            }
        }

        public void ResetPower()
        {
            foreach (Card card in _field.Units)
            {
                if (card != null)
                {
                    card.tempPower = 0;
                }
            }
        }

        public void EnemyResetPower()
        {
            //_field.Units[FL.EnemyVanguard].bonusPower = 0;
            //foreach (Card card in _field.EnemyRG)
            //{
            //    if (card != null)
            //        card.bonusPower = 0;
            //}
        }

        public void EndAttack()
        {
            _field.Sentinel = false;
            _field.Attack = -1;
            _field.Attacker = -1;
            _field.Attacked = -1;
            _field.Booster = -1;
            _field.Guarding = false;
        }

        public void EndTurn()
        {
            ResetPower();
            EnemyResetPower();
            foreach (Card card in GetActiveUnits())
            {
                foreach (var key in card.abilityPower.Keys)
                {
                    card.abilityPower[key] = false;
                }
            }
            _riddenOnThisTurn.Clear();
            _turn++;
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
            if (location == FL.PlayerFrontLeft || location == FL.PlayerVanguard || location == FL.PlayerFrontRight)
                return true;
            return false;
        }

        public bool isEnemyFrontRow(int location)
        {
            if (location == FL.EnemyFrontLeft || location == FL.EnemyVanguard || location == FL.EnemyFrontRight)
                return true;
            return false;
        }

        public List<Card> Soul()
        {
            return _field.Units[FL.PlayerVanguard].soul;
        }

        public List<Card> CardsForOnRide()
        {
            List<Card> cards =  new List<Card>();
            cards.AddRange(_field.PlayerHand);
            for (int i = FL.PlayerFrontLeft; i < FL.PlayerVanguard + 1; i++)
                cards.Add(_field.Units[i]);
            cards.AddRange(Soul());
            return cards;
        }

        public List<Card> CardsForOnAttack()
        {
            List<Card> cards = new List<Card>();
            cards.AddRange(_field.PlayerHand);
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
            return _field.Units[FL.PlayerVanguard].name;
        }

        public Card Vanguard()
        {
            return _field.Units[FL.PlayerVanguard];
        }

        public Card AttackingUnit()
        {
            return _field.Units[_field.Attacker];
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
            foreach (Card card in _field.PlayerDeck)
            {
                if (card.name == name)
                    return true;
            }
            return false;
        }

        public bool IsTopSoul(Card card)
        {
            if (card == _field.Units[FL.PlayerVanguard].soul[0])
                return true;
            return false;
        }

        public List<Card> GetCardsWithName(string name)
        {
            List<Card> cardsWithName = new List<Card>();
            foreach (Card card in _field.PlayerDeck)
            {
                if (card.name == name)
                    cardsWithName.Add(card);
            }
            return cardsWithName;
        }
    }

    public static class FL //FieldLocation
    {
        public const int EnemyFrontLeft = 1;
        public const int EnemyBackLeft = 2;
        public const int EnemyBackMiddle = 3;
        public const int EnemyFrontRight = 4;
        public const int EnemyBackRight = 5;
        public const int EnemyVanguard = 6;
        public const int PlayerFrontLeft = 8;
        public const int PlayerBackLeft = 9;
        public const int PlayerBackMiddle = 10;
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
    }
}
