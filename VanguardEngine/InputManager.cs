using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VanguardEngine
{
    public class InputManager
    {
        public Player _player1;
        public Player _player2;
        public bool bool_input;
        public int int_input;
        public List<int> intlist_input = new List<int>();
        public List<Ability> _abilities = null;
        public List<Card> cardsToSelect = new List<Card>();
        public string string_input;
        public int prompt;
        public int value;
        public int int_value;
        public int int_value2;
        public bool bool_value;
        public Card card_input;
        string _query;
        public static ManualResetEvent oSignalEvent = new ManualResetEvent(false);
        public EventHandler<CardEventArgs> OnPlayerSwap;

        public void Initialize(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public virtual void SwapPlayers()
        {
            Player temp = _player1;
            _player1 = _player2;
            _player2 = temp;
            if (OnPlayerSwap != null)
                OnPlayerSwap(this, new CardEventArgs());
        }

        public int SelectPrompt(int max)
        {
            int selection = 0;
            string input;
            while (selection < 1 || selection > max)
            {
                input = Console.ReadLine();
                if (input == "")
                    selection = 0;
                else
                    selection = int.Parse(input);
            }
            return selection;
        }

        protected virtual void WaitForInput(ThreadStart start)
        {
            Thread InputThread = new Thread(start);
            InputThread.Start();
            oSignalEvent.WaitOne();
            oSignalEvent.Reset();
            InputThread.Abort();
        }

        public virtual bool YesNo(int request)
        {
            prompt = request;
            WaitForInput(YesNo_Input);
            return bool_input;
        }

        protected virtual void YesNo_Input()
        {
            Console.WriteLine("1. Yes\n2. No");
            int input = SelectPrompt(2);
            if (input == 1)
                bool_input = true;
            if (input == 2)
                bool_input = false;
            oSignalEvent.Set();
        }

        public virtual int RPS()
        {
            WaitForInput(RPS_Input);
            return int_input;
        }

        protected virtual void RPS_Input()
        {
            Console.WriteLine("----------\nPlayer 1: Rock, Paper, or Scissors?\n" +
                    "1. Rock\n" +
                    "2. Paper\n" +
                    "3. Scissors");
            int_input = SelectPrompt(3) - 1;
            oSignalEvent.Set();
        }

        public virtual int SelectCardFromHand()
        {
            WaitForInput(SelectCardFromHand_Input);
            return int_input;
        }

        protected virtual void SelectCardFromHand_Input()
        {
            List<Card> hand = _player1.GetHand();
            Console.WriteLine("----------\nSelect card from hand:");
            for (int i = 0; i < hand.Count; i++)
            {
                Console.WriteLine(i + 1 + ". " + hand[i].name);
            }
            int_input = SelectPrompt(hand.Count) - 1;
            oSignalEvent.Set();
        }

        public virtual List<int> SelectCardsFromHand(int count)
        {
            int_input = count;
            WaitForInput(SelectCardsFromHand_Input);
            return intlist_input;
        }

        protected virtual void SelectCardsFromHand_Input()
        {
            List<Card> hand = _player1.GetHand();
            int selection = 0;
            intlist_input.Clear();
            for (int i = 0; i < int_input; i++)
            {
                while (true)
                {
                    Console.WriteLine("----------\nDiscard " + int_input + " card(s).");
                    for (int j = 0; j < hand.Count; j++)
                        Console.WriteLine(j + 1 + ". " + hand[j].name);
                    selection = SelectPrompt(hand.Count) - 1;
                    if (intlist_input.Contains(hand[selection].tempID))
                        Console.WriteLine("----------\nAlready choose that card.");
                    else
                    {
                        intlist_input.Add(hand[selection].tempID);
                        break;
                    }
                }
            }
            oSignalEvent.Set();
        }

        public virtual List<int> SelectCardsToMulligan()
        {
            WaitForInput(SelectCardsToMulligan_Input);
            return intlist_input;
        }

        protected virtual void SelectCardsToMulligan_Input()
        {
            List<Card> hand = _player1.GetHand();
            int selection = 0;
            intlist_input.Clear();
            while (selection != hand.Count)
            {
                Console.WriteLine("Choose card to mulligan.");
                for (int j = 0; j < hand.Count; j++)
                {
                    if (intlist_input.Contains(hand[j].tempID))
                        Console.WriteLine(j + 1 + ". " + hand[j].name + "(selected)");
                    else
                        Console.WriteLine(j + 1 + ". " + hand[j].name);
                }
                Console.WriteLine(hand.Count + 1 + ". End mulligan.");
                selection = SelectPrompt(hand.Count + 1) - 1;
                if (selection == hand.Count)
                    break;
                else if (intlist_input.Contains(hand[selection].tempID))
                    intlist_input.Remove(hand[selection].tempID);
                else
                    intlist_input.Add(hand[selection].tempID);
            }
            oSignalEvent.Set();
        }

        public virtual int SelectCardFromRideDeck()
        {
            WaitForInput(SelectCardFromRideDeck_Input);
            return int_input;
        }

        protected virtual void SelectCardFromRideDeck_Input()
        {
            int selection;
            List<Card> cards = _player1.GetRideableCards(true);
            Console.WriteLine("----------\nSelect card from Ride Deck to Ride.");
            for (int i = 0; i < cards.Count; i++)
                Console.WriteLine(i + 1 + ". G" + cards[i].grade + " " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectCardFromHandToRide()
        {
            WaitForInput(SelectCardFromHandToRide_Input);
            return int_input;
        }

        protected virtual void SelectCardFromHandToRide_Input()
        {
            int selection;
            List<Card> cards = _player1.GetRideableCards(false);
            Console.WriteLine("----------\nSelect card from hand to Ride.");
            for (int i = 0; i < cards.Count; i++)
                Console.WriteLine(i + 1 + ". G" + cards[i].grade + " " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectMainPhaseAction()
        {
            WaitForInput(SelectMainPhaseAction_Input);
            return int_input;
        }

        protected virtual void SelectMainPhaseAction_Input()
        {
            Console.WriteLine("----------\nChoose option.\n" +
                    "1. See Hand\n" +
                    "2. See Field\n" +
                    "3. Call Rearguard\n" +
                    "4. Move Rearguard(s)\n" +
                    "5. Activate Ability\n" +
                    "6. Activate Order\n" +
                    "7. End Main Phase");
            int_input = SelectPrompt(9);
            oSignalEvent.Set();
        }

        public virtual int SelectRearguardToCall()
        {
            WaitForInput(SelectRearguardToCall_Input);
            return int_input;
        }

        protected virtual void SelectRearguardToCall_Input()
        {
            int selection = 0;
            List<Card> cards = _player1.GetCallableRearguards();
            Console.WriteLine("Choose Rearguard to call.");
            for (int i = 0; i < cards.Count; i++)
                Console.WriteLine(i + 1 + ". " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectCallLocation(string query)
        {
            _query = query;
            WaitForInput(SelectCallLocation_Input);
            return int_input;
        }

        protected virtual void SelectCallLocation_Input()
        {
            Console.WriteLine("----------\nChoose location.\n" +
                            "1. Front left.\n" +
                            "2. Back left.\n" +
                            "3. Back middle.\n" +
                            "4. Front right.\n" +
                            "5. Back right.\n");
            int_input = SelectPrompt(5);
            switch (int_input)
            {
                case 1:
                    int_input = FL.PlayerFrontLeft;
                    break;
                case 2:
                    int_input = FL.PlayerBackLeft;
                    break;
                case 3:
                    int_input = FL.PlayerBackCenter;
                    break;
                case 4:
                    int_input = FL.PlayerFrontRight;
                    break;
                case 5:
                    int_input = FL.PlayerBackRight;
                    break;
            }
            oSignalEvent.Set();
        }

        public virtual int SelectRearguardColumn()
        {
            WaitForInput(SelectRearguardColumn_Input);
            return int_input;
        }
        
        protected virtual void SelectRearguardColumn_Input()
        {
            while (true)
            {
                Console.WriteLine("----------\nChoose column to move Rearguard(s).\n" +
                    "1. Left\n" +
                    "2. Right\n");
                int_input = SelectPrompt(2) - 1;
                if (_player1.CheckColumn(int_input))
                    oSignalEvent.Set();
                else
                    Console.WriteLine("No Rearguards in that column.");
            }
        }

        public virtual int SelectBattlePhaseAction()
        {
            WaitForInput(SelectBattlePhaseAction_Input);
            return int_input;
        }

        protected virtual void SelectBattlePhaseAction_Input()
        {
            Console.WriteLine("----------\nChoose option.\n" +
                        "1. See Hand\n" +
                        "2. See Field\n" +
                        "3. Attack\n" +
                        "4. End Battle Phase");
            int_input = SelectPrompt(5);
            oSignalEvent.Set();
        }

        public virtual int SelectAttackingUnit()
        {
            WaitForInput(SelectAttackingUnit_Input);
            return int_input;
        }

        protected virtual void SelectAttackingUnit_Input()
        {
            int selection = 0;
            List<Card> cards = _player1.GetCardsToAttackWith();
            Console.WriteLine("Choose unit to attack with.");
            for (int i = 0; i < cards.Count; i++)
                Console.WriteLine(i + 1 + ". " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectUnitToAttack()
        {
            WaitForInput(SelectUnitToAttack_Input);
            return int_input;
        }

        protected virtual void SelectUnitToAttack_Input()
        {
            int selection = 0;
            List<Card> cards = _player1.GetEnemyFrontRow();
            Console.WriteLine("Choose unit to attack.");
            for (int i = 0; i < cards.Count; i++)
                Console.WriteLine(i + 1 + ". " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectGuardPhaseAction()
        {
            WaitForInput(SelectGuardPhaseAction_Input);
            return int_input;
        }

        protected virtual void SelectGuardPhaseAction_Input()
        {
            Console.WriteLine("----------\nGuard?\n" +
                        "1. See Hand.\n" +
                        "2. See Field.\n" +
                        "3. See Current Shield.\n" +
                        "4. Guard.\n" +
                        "5. Use Blitz Order.\n" +
                        "6. End Guard.\n");
            int_input = SelectPrompt(6);
            oSignalEvent.Set();
        }

        public virtual int SelectCardToGuardWith()
        {
            WaitForInput(SelectCardToGuardWith_Input);
            return int_input;
        }

        protected virtual void SelectCardToGuardWith_Input()
        {
            int selection = 0;
            string output;
            List<Card> cards = _player1.GetGuardableCards();
            Console.WriteLine("Choose card to guard with.");
            for (int i = 0; i < cards.Count; i++)
            {
                output = i + 1 + ". " + cards[i].name;
                if (cards[i].location == Location.PlayerRC)
                    output += " [Intercept]";
                Console.WriteLine(output);
            }
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectActiveUnit(int request, int amount)
        {
            prompt = request;
            value = amount;
            WaitForInput(SelectActiveUnit_Input);
            return int_input;
        }

        protected virtual void SelectActiveUnit_Input()
        {
            int selection = 0;
            List<Card> cards = _player1.GetActiveUnits();
            for (int i = 0; i < cards.Count; i++)
                Console.WriteLine(i + 1 + ". " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectDamageZone()
        {
            WaitForInput(SelectDamageZone_Input);
            return int_input;
        }

        protected virtual void SelectDamageZone_Input()
        {
            int selection = 0;
            List<Card> cards = _player1.GetDamageZone();
            for (int i = 0; i < cards.Count; i++)
                Console.WriteLine(i + 1 + ". " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectAbility(List<Ability> abilities)
        {
            _abilities = abilities;
            WaitForInput(SelectAbility_Input);
            return int_input;
        }

        protected virtual void SelectAbility_Input()
        {
            Console.WriteLine("----------\nSelect effect to activate.");
            for (int i = 0; i < _abilities.Count; i++)
                Console.WriteLine(i + 1 + ". " + _abilities[i].Name);
            if (!CheckForMandatoryEffects(_abilities))
            {
                Console.WriteLine(_abilities.Count + 1 + ". Don't activate effect.");
                int_input = SelectPrompt(_abilities.Count + 1) - 1;
            }
            else
                int_input = SelectPrompt(_abilities.Count) - 1;
            oSignalEvent.Set();
        }

        public bool CheckForMandatoryEffects(List<Ability> abilities)
        {
            foreach (Ability ability in abilities)
            {
                if (ability.isMandatory)
                    return true;
            }
            return false;
        }

        public List<int> SelectFromList(List<Card> cards, int count, int min, string query)
        {
            _query = query;
            cardsToSelect.Clear();
            cardsToSelect.AddRange(cards);
            if (cardsToSelect.Count < count)
                count = cardsToSelect.Count;
            int_value = count;
            int_value2 = min;
            WaitForInput(SelectFromList_Input);
            return intlist_input;
        }

        protected void SelectFromList_Input()
        {
            int selection;
            int count = int_value;
            int min = int_value2;
            intlist_input.Clear();
            for (int j = 0; j < count; j++)
            {
                Console.WriteLine(_query);
                for (int i = 0; i < cardsToSelect.Count; i++)
                {
                    Console.WriteLine(i + 1 + ". " + cardsToSelect[i].name);
                }
                if (intlist_input.Count > min)
                {
                    Console.WriteLine(cardsToSelect.Count + 1 + ". End selection.");
                    selection = SelectPrompt(cardsToSelect.Count);
                }
                else
                    selection = SelectPrompt(cardsToSelect.Count) - 1;
                if (intlist_input.Count > min && selection == cardsToSelect.Count)
                    oSignalEvent.Set();
                intlist_input.Add(cardsToSelect[selection].tempID);
                cardsToSelect.RemoveAt(selection);
            }
            oSignalEvent.Set();
        }
    }

    public class PromptType
    {
        public const int Mulligan = 0;
        public const int Ride = 1;
        public const int RideFromRideDeck = 2;
        public const int Boost = 3;
        public const int AddPower = 4;
        public const int AddCritical = 5;
        public const int Stand = 6;
        public const int OverDress = 7;
    }
}

