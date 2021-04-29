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
        Player _player1;
        Player _player2;
        bool bool_input;
        int int_input;
        List<int> intlist_input = new List<int>();
        List<Effect> _effects = null;
        string string_input;
        Card card_input;
        public static ManualResetEvent oSignalEvent = new ManualResetEvent(false);
        public EventHandler<CardEventArgs> OnPlayerSwap;

        public void Initialize(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public void SwapPlayers()
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

        public virtual bool YesNo()
        {
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
                    if (intlist_input.Contains(selection))
                        Console.WriteLine("----------\nAlready choose that card.");
                    else
                    {
                        intlist_input.Add(selection);
                        break;
                    }
                }
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
            int_input = SelectPrompt(7);
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

        public virtual int SelectCallLocation()
        {
            WaitForInput(SelectCallLocation_Input);
            return int_input;
        }

        protected virtual void SelectCallLocation_Input()
        {
            Console.WriteLine("----------\nChoose location.\n" +
                            "1. Front left.\n" +
                            "2. Bottom left.\n" +
                            "3. Back middle.\n" +
                            "4. Front right.\n" +
                            "5. Back right.\n");
            int_input = SelectPrompt(5) - 1;
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
            int_input = SelectPrompt(4);
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
            List<Card> cards = _player1.GetStandingFrontRow();
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
                        "5. End Guard.\n");
            int_input = SelectPrompt(5);
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
            List<Card> cards = _player1.GetGuardableCards();
            Console.WriteLine("Choose card to guard with.");
            for (int i = 0; i < cards.Count; i++)
                Console.WriteLine(i + 1 + ". " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectActiveUnit()
        {
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

        public virtual int SelectEffect(List<Effect> effects)
        {
            _effects = effects;
            WaitForInput(SelectEffect_Input);
            return int_input;
        }

        protected virtual void SelectEffect_Input()
        {
            Console.WriteLine("Select effect to activate.");
            for (int i = 0; i < _effects.Count; i++)
                Console.WriteLine(i + 1 + ". " + _effects[i].Name);
            if (!CheckForMandatoryEffects(_effects))
            {
                Console.WriteLine(_effects.Count + 2 + ". Don't activate effect.");
                int_input = SelectPrompt(_effects.Count + 1) - 1;
            }
            else
                int_input = SelectPrompt(_effects.Count) - 1;
            oSignalEvent.Set();
        }

        public bool CheckForMandatoryEffects(List<Effect> effects)
        {
            foreach (Effect effect in effects)
            {
                if (effect.isMandatory)
                    return true;
            }
            return false;
        }
    }
}

