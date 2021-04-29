using System;
using System.Collections.Generic;
using System.Text;

namespace VanguardEngine
{
    public class Player
    {
        public Field _field;
        protected int _damage = 0;
        protected int _turn = 1;

        public event EventHandler<CardEventArgs> OnDraw;
        public event EventHandler<CardEventArgs> OnRideFromRideDeck;
        public event EventHandler<CardEventArgs> OnRideFromHand;
        public event EventHandler<CardEventArgs> OnCallFromHand;
        public event EventHandler<CardEventArgs> OnCallFromDeck;
        public event EventHandler<CardEventArgs> OnChangeColumn;

        public void Initialize(List<Card> deck1, List<Card> deck2, Player player2)
        {
            _field = new Field();
            _field.Initialize(deck1, deck2, player2);
        }

        //public Field Field
        //{
        //    get => _field;
        //}

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

        public void Draw(int count)
        {
            List<Card> cardsAdded = new List<Card>();
            CardEventArgs args = new CardEventArgs();
            for (int i = 0; i < count; i++)
            {
                _field.PlayerDeck[0].faceup = true;
                _field.PlayerHand.Add(_field.PlayerDeck[0]);
                cardsAdded.Add(_field.PlayerDeck[0]);
                _field.PlayerDeck.Remove(_field.PlayerDeck[0]);
            }
            args.i = count;
            args.cardList = cardsAdded;
            args.player = true;
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
                cardsAdded.Add(_field.EnemyDeck[0]);
                _field.EnemyDeck.Remove(_field.EnemyDeck[0]);
            }
            args.i = count;
            args.cardList = cardsAdded;
            args.player = false;
            if (OnDraw != null)
            {
                OnDraw(this, args);
            }
        }

        public void ReturnCardFromHandToDeck(int selection, bool player)
        {
            Card card;
            if (player)
            {
                card = _field.PlayerHand[selection];
                _field.PlayerHand.RemoveAt(selection);
                card.faceup = false;
                _field.PlayerDeck.Add(card);
            }
            else
            {
                card = _field.EnemyHand[selection];
                _field.EnemyHand.RemoveAt(selection);
                _field.EnemyDeck.Add(card);
            }
        }

        public void Discard(List<int> list, bool player)
        {
            List<Card> toDiscard = new List<Card>();
            Card card = null;
            if (player)
            {
                foreach (int item in list)
                    toDiscard.Add(_field.PlayerHand[item]);
                while (toDiscard.Count > 0)
                {
                    card = toDiscard[0];
                    _field.PlayerHand.Remove(card);
                    _field.PlayerDrop.Insert(0, card);
                    toDiscard.Remove(card);
                }
            }
            else
            {
                foreach (int item in list)
                    toDiscard.Add(_field.EnemyHand[item]);
                while (toDiscard.Count > 0)
                {
                    card = toDiscard[0];
                    _field.EnemyHand.Remove(card);
                    card.faceup = true;
                    _field.EnemyDrop.Insert(0, card);
                    toDiscard.Remove(card);
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
            Draw(5);
            EnemyDraw(5);
            //_field.FlipVG();
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

        public bool CheckColumn(int column)
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
            if (_field.Units[FL.EnemyVanguard].upright)
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
            if (_field.Units[FL.PlayerVanguard].upright)
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
            int power = Units[location].power + Units[location].bonusPower + Units[location].bonusBattlePower;
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
                if (card.grade == VG.grade || card.grade - 1 == VG.grade)
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

        public bool TargetIsVanguard()
        {
            if (_field.Attacked == FL.PlayerVanguard)
                return true;
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
            if (CalculatePowerOfUnit(_field.Attacker) >= CalculateShield() + card.power + card.bonusPower)
            {
                Console.WriteLine("----------\n" + attacker.name + "'s attack makes a hit on " + card.name + "!");
                return true;
            }
            else
            {
                Console.WriteLine("----------\n" + attacker.name + "'s attack against " + card.name + " was successfully guarded!");
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
            List<Card> list;
            Card card = null;
            if (player)
            {
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
                _field.Units[FL.PlayerVanguard] = card;
                card.soul[0].soul.Clear();
                card.soul[0].bonusPower = 0;
                Console.WriteLine("---------\nRide!! " + _field.Units[FL.PlayerVanguard].name + "!");
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
                card.soul.AddRange(_field.Units[FL.EnemyVanguard].soul);
                card.soul.Insert(0, _field.Units[FL.EnemyVanguard]);
                _field.Units[FL.EnemyVanguard] = card;
                card.soul[0].soul.Clear();
                card.soul[0].bonusPower = 0;
            }
        }

        public void EnemyRide(int location, int selection)
        {
            List<Card> list;
            if (location == 0)
                list = _field.EnemyRideDeck;
            else
                list = _field.EnemyHand;
            foreach (Card card in list)
            {
                if (card.grade == _field.Units[FL.EnemyVanguard].grade || card.grade - 1 == _field.Units[FL.EnemyVanguard].grade)
                    selection--;
                if (selection == 0)
                {
                    if (_field.Units[FL.EnemyVanguard].soul == null)
                        _field.Units[FL.EnemyVanguard].soul = new List<Card>();
                    _field.Units[FL.EnemyVanguard].soul.Insert(0, _field.Units[FL.EnemyVanguard]);
                    card.soul.AddRange(_field.Units[FL.EnemyVanguard].soul);
                    _field.Units[FL.EnemyVanguard].soul.Clear();
                    _field.Units[FL.EnemyVanguard].bonusPower = 0;
                    card.faceup = true;
                    _field.Units[FL.EnemyVanguard] = card;
                    list.Remove(card);
                    break;
                }
            }
        }

        public void Call(int location, int selection, bool player)
        {
            List<Card> hand;
            Card[] slots = _field.Units;
            Card card = null;
            Card toBeRetired = null;
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
                location += FL.PlayerFrontLeft;
                if (slots[location] != null)
                {
                    toBeRetired = slots[location];
                    foreach (Card soul in slots[location].soul)
                        _field.PlayerDrop.Insert(0, soul);
                    slots[location] = null;
                    toBeRetired.bonusPower = 0;
                    toBeRetired.upright = true;
                    slots[location].soul.Clear();
                    _field.PlayerDrop.Insert(0, slots[location]);
                }
                hand.Remove(card);
                slots[location] = card;
                Console.WriteLine("---------\nCall! " + card.name + "!");
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
                location += FL.EnemyFrontLeft;
                if (slots[location] != null)
                {
                    toBeRetired = slots[location];
                    foreach (Card soul in slots[location].soul)
                        _field.EnemyDrop.Insert(0, soul);
                    slots[location] = null;
                    toBeRetired.bonusPower = 0;
                    toBeRetired.upright = true;
                    slots[location].soul.Clear();
                    _field.EnemyDrop.Insert(0, slots[location]);
                }
                hand.Remove(card);
                card.faceup = true;
                slots[location] = card;
            }
        }

        public void SuperiorCall(int location, string name)
        {
            Card ToBeCalled = null;
            Card[] slots = _field.Units;
            foreach (Card card in _field.PlayerDeck)
            {
                if (card.name == "Trickstar")
                {
                    ToBeCalled = card;
                    _field.PlayerDeck.Remove(card);
                    break;
                }
            }
            location += FL.PlayerFrontLeft;
            if (slots[location] != null)
            {
                slots[location].bonusPower = 0;
                slots[location].upright = true;
                foreach (Card soul in slots[location].soul)
                    _field.PlayerDrop.Insert(0, soul);
                slots[location].soul.Clear();
                _field.PlayerDrop.Insert(0, slots[location]);
            }
            ToBeCalled.faceup = true;
            slots[location] = ToBeCalled;
            Console.WriteLine("----------\nSuperior Call! " + ToBeCalled.name + "!");
            ShuffleDeck();
        }

        public void EnemySuperiorCall(int location, string name)
        {
            Card ToBeCalled = null;
            Card[] slots = _field.Units;
            foreach (Card card in _field.EnemyDeck)
            {
                if (card.name == "Trickstar")
                {
                    ToBeCalled = card;
                    _field.EnemyDeck.Remove(card);
                    break;
                }
            }
            location += FL.EnemyFrontLeft;
            if (slots[location] != null)
            {
                slots[location].bonusPower = 0;
                slots[location].upright = true;
                foreach (Card soul in slots[location].soul)
                    _field.EnemyDrop.Insert(0, soul);
                slots[location].soul.Clear();
                _field.EnemyDrop.Insert(0, slots[location]);
            }
            ToBeCalled.faceup = true;
            slots[location] = ToBeCalled;
            EnemyShuffleDeck();
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
            if (player)
            {
                _field.Guarding = true;
                foreach (Card card in _field.PlayerHand)
                {
                    if (card.tempID == selection)
                    {
                        _field.PlayerHand.Remove(card);
                        card.upright = false;
                        _field.GC.Insert(0, card);
                        Console.WriteLine("----------\nAdded " + card.name + " to Guardian Circle.");
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
                        _field.GC.Insert(0, card);
                        Console.WriteLine("----------\nAdded " + card.name + " to Guardian Circle.");
                        return;
                    }
                }
            }
        }

        public void RetireGC()
        {
            foreach (Card card in _field.GC)
            {
                card.upright = true;
                _field.EnemyDrop.Insert(0, card);
            }
            _field.GC.Clear();
        }

        public void EnemyRetireGC()
        {
            foreach (Card card in _field.GC)
            {
                card.upright = true;
                _field.PlayerDrop.Insert(0, card);
            }
            _field.GC.Clear();
        }

        public int TriggerCheck()
        {
            Card trigger = _field.PlayerDeck[0];
            trigger.upright = false;
            trigger.faceup = true;
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
            return trigger.trigger;
        }

        public int EnemyTriggerCheck()
        {
            Card trigger = _field.EnemyDeck[0];
            trigger.upright = false;
            trigger.faceup = true;
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
            Console.WriteLine("----------\n" + power + " power to " + target.name + "!");
        }

        public void AddBattleOnlyPower(int selection, int power)
        {
            Card target = FindActiveUnit(selection);
            target.bonusBattlePower += power;
        }

        public void AddBattleOnlyPower(Card card, int power)
        {
            card.bonusBattlePower += power;
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
            _field.PlayerTrigger = null;
            card.upright = true;
            _field.PlayerHand.Add(card);
        }

        public void EnemyAddTriggerToHand()
        {
            Card card = _field.EnemyTrigger;
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
            _field.PlayerTrigger = null;
            _field.PlayerDamageZone.Add(card);
            Console.WriteLine("----------\nDamage taken!");
        }

        public void EnemyTakeDamage()
        {
            Card card = _field.EnemyTrigger;
            _field.EnemyTrigger = null;
            _field.EnemyDamageZone.Add(card);
        }

        public void ResetPower()
        {
            foreach (Card card in _field.Units)
            {
                if (card != null)
                    card.bonusPower = 0;
            }
        }

        public void ResetBattleOnlyPower()
        {
            foreach (Card card in _field.Units)
            {
                if (card != null)
                    card.bonusBattlePower = 0;
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

        public List<Effect> CheckForRideEffects()
        {
            return null;
        }

        public List<Effect> CheckForCallEffects()
        {
            return null;
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

        public bool isTopSoul(Card card)
        {
            if (card == _field.Units[FL.PlayerVanguard].soul[0])
                return true;
            return false;
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

        public static int SwitchSides(int location)
        {
            if (location >= EnemyFrontLeft && location <= EnemyVanguard)
                return location + 7;
            else
                return location - 7;
        }
    }
}
