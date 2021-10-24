﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VanguardEngine
{
    public class InputManager
    {
        //public Player _actingPlayer;
        //public Player _player2;
        public Player _actingPlayer;
        public bool bool_input;
        public int int_input;
        public int int_input2;
        public int int_input3;
        public int int_input4;
        public List<int> intlist_input = new List<int>();
        public Ability _ability = null;
        public List<Ability> _abilities = new List<Ability>();
        public List<Ability> _abilities2 = new List<Ability>();
        public List<Card> cardsToSelect = new List<Card>();
        public string string_input;
        public int prompt;
        public int value;
        public int int_value;
        public int int_value2;
        public int int_value3;
        public bool bool_value;
        public Card card_input;
        public string _query;
        public string[] _list;
        public List<int> _ints = new List<int>();
        public List<int> _ints2 = new List<int>();
        public static ManualResetEvent oSignalEvent = new ManualResetEvent(false);
        public EventHandler<CardEventArgs> OnPlayerSwap;
        public EventHandler<CardEventArgs> OnChosen;

        public void Initialize(Player player1, Player player2)
        {
            _actingPlayer = player1;
            //_player2 = player2;
        }

        //public virtual void SwapPlayers()
        //{
        //    Player temp = _actingPlayer;
        //    _actingPlayer = _player2;
        //    _player2 = temp;
        //    if (OnPlayerSwap != null)
        //        OnPlayerSwap(this, new CardEventArgs());
        //}

        public int SelectPrompt(int max)
        {
            int selection = 0;
            string input;
            while (selection < 1 || selection > max)
            {
                input = Console.ReadLine();
                int.TryParse(input, out selection);
            }
            return selection;
        }

        //protected virtual void WaitForInput(ThreadStart start)
        //{
        //    Thread InputThread = new Thread(start);
        //    InputThread.Start();
        //    oSignalEvent.WaitOne();
        //    oSignalEvent.Reset();
        //    InputThread.Abort();
        //}

        //public virtual bool YesNo(Player actingPlayer, int request)
        //{
        //    bool swapped = false;
        //    if (actingPlayer._playerID != _actingPlayer._playerID)
        //    {
        //        SwapPlayers();
        //        swapped = true;
        //    }
        //    prompt = request;
        //    string_input = "";
        //    YesNo_Input();
        //    if (swapped)
        //        SwapPlayers();
        //    return bool_input;
        //}

        public virtual bool YesNo(Player actingPlayer, string query)
        {
            _actingPlayer = actingPlayer;
            prompt = PromptType.UniqueQuery;
            string_input = query;
            YesNo_Input();
            return bool_input;
        }

        protected virtual void YesNo_Input()
        {
            Log.WriteLine("----------");
            if (string_input != "")
                Log.WriteLine(string_input);
            Log.WriteLine("1. Yes\n2. No");
            int input = SelectPrompt(2);
            if (input == 1)
                bool_input = true;
            if (input == 2)
                bool_input = false;
            oSignalEvent.Set();
        }

        public virtual int RPS(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            RPS_Input();
            return int_input;
        }

        protected virtual void RPS_Input()
        {
            Log.WriteLine("----------\nPlayer 1: Rock, Paper, or Scissors?\n" +
                    "1. Rock\n" +
                    "2. Paper\n" +
                    "3. Scissors");
            int_input = SelectPrompt(3) - 1;
            oSignalEvent.Set();
        }

        public virtual List<int> SelectCardsToMulligan(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            SelectCardsToMulligan_Input();
            return new List<int>(intlist_input);
        }

        protected virtual void SelectCardsToMulligan_Input()
        {
            List<Card> hand = _actingPlayer.GetHand();
            int selection = 0;
            intlist_input.Clear();
            while (selection != hand.Count)
            {
                Log.WriteLine("Choose card to mulligan.");
                for (int j = 0; j < hand.Count; j++)
                {
                    if (intlist_input.Contains(hand[j].tempID))
                        Log.WriteLine(j + 1 + ". " + hand[j].name + "(selected)");
                    else
                        Log.WriteLine(j + 1 + ". " + hand[j].name);
                }
                Log.WriteLine(hand.Count + 1 + ". End mulligan.");
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

        public virtual int SelectRidePhaseAction(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            SelectRidePhaseAction_Input();
            return int_input;
        }

        protected virtual void SelectRidePhaseAction_Input()
        {
            Log.WriteLine("----------\nChoose option.\n" +
                    "1. Ride from Ride Deck.\n" +
                    "2. Ride from hand.\n" +
                    "3. End Ride Phase.");
            int_input = SelectPrompt(3);
            oSignalEvent.Set();
        }

        public virtual int SelectMainPhaseAction(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            SelectMainPhaseAction_Input();
            return int_input;
        }

        protected virtual void SelectMainPhaseAction_Input()
        {
            Log.WriteLine("----------\nChoose option.\n" +
                    "1. See Hand\n" +
                    "2. See Field\n" +
                    "3. Call Rearguard\n" +
                    "4. Call from Prison\n" +
                    "5. Move Rearguard(s)\n" +
                    "6. Activate Ability\n" +
                    "7. Activate Order\n" +
                    "8. End Main Phase");
            int_input = SelectPrompt(10);
            oSignalEvent.Set();
        }

        public virtual int SelectRearguardToCall(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            SelectRearguardToCall_Input();
            return int_input;
        }

        protected virtual void SelectRearguardToCall_Input()
        {
            int selection = 0;
            List<Card> cards = _actingPlayer.GetCallableRearguards();
            Log.WriteLine("Choose Rearguard to call.");
            for (int i = 0; i < cards.Count; i++)
                Log.WriteLine(i + 1 + ". " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectCallLocation(Player actingPlayer, string query, Card card, List<int> selectedCircles, List<int> canSelect)
        {
            _actingPlayer = actingPlayer;
            _query = query;
            _ints.Clear();
            _ints.AddRange(selectedCircles);
            _ints2.Clear();
            card_input = card;
            if (canSelect != null)
                _ints2.AddRange(canSelect);
            SelectCallLocation_Input();
            return int_input;
        }

        protected virtual void SelectCallLocation_Input()
        {
            bool proceed = false;
            while (!proceed)
            {
                Log.WriteLine("----------\nChoose location.\n" +
                            "1. Front left.\n" +
                            "2. Back left.\n" +
                            "3. Back middle.\n" +
                            "4. Front right.\n" +
                            "5. Back right.\n");
                int_input = SelectPrompt(5);
                switch (int_input)
                {
                    case 1:
                        int_input = _actingPlayer.Convert(FL.PlayerFrontLeft);
                        break;
                    case 2:
                        int_input = _actingPlayer.Convert(FL.PlayerBackLeft);
                        break;
                    case 3:
                        int_input = _actingPlayer.Convert(FL.PlayerBackCenter);
                        break;
                    case 4:
                        int_input = _actingPlayer.Convert(FL.PlayerFrontRight);
                        break;
                    case 5:
                        int_input = _actingPlayer.Convert(FL.PlayerBackRight);
                        break;
                }
                if (!_ints.Contains(int_input) && ((_ints2.Count > 0 && _ints2.Contains(int_input)) || _ints2.Count == 0))
                {
                    proceed = true;
                }
                else
                    Log.WriteLine("This circle cannot be selected.");
            }
            oSignalEvent.Set();
        }

        public virtual List<int> SelectCircle(Player actingPlayer, List<int> availableCircles, int count)
        {
            _actingPlayer = actingPlayer;
            intlist_input.Clear();
            intlist_input.AddRange(availableCircles);
            int_value = count;
            SelectCircle_Input();
            return intlist_input;
        }

        protected virtual void SelectCircle_Input()
        {
            string output;
            int_input = -1;
            List<int> selectedCircles = new List<int>();
            while (selectedCircles.Count < int_value)
            {
                Log.WriteLine("Select a circle.");
                for (int i = 0; i < intlist_input.Count; i++)
                {
                    output = (i + 1) + ". ";
                    if (intlist_input[i] == _actingPlayer.Convert(FL.EnemyFrontRight))
                        output += "Enemy Front Right.";
                    else if (intlist_input[i] == _actingPlayer.Convert(FL.EnemyFrontLeft))
                        output += "Enemy Front Left.";
                    else if (intlist_input[i] == _actingPlayer.Convert(FL.EnemyBackRight))
                        output += "EnemyBack Right.";
                    else if (intlist_input[i] == _actingPlayer.Convert(FL.EnemyBackCenter))
                        output += "Enemy Back Center.";
                    else if (intlist_input[i] == _actingPlayer.Convert(FL.EnemyBackLeft))
                        output += "Enemy Back Left.";
                    else if (intlist_input[i] == _actingPlayer.Convert(FL.EnemyVanguard))
                        output += "Enemy Vanguard";
                    if (selectedCircles.Contains(intlist_input[i]))
                        output += "[selected]";
                    Log.WriteLine(output);
                }
                int_input = SelectPrompt(intlist_input.Count);
                if (selectedCircles.Contains(intlist_input[int_input - 1]))
                    selectedCircles.Remove(intlist_input[int_input - 1]);
                else
                    selectedCircles.Add(intlist_input[int_input - 1]);
            }
            intlist_input.Clear();
            intlist_input.AddRange(selectedCircles);
            oSignalEvent.Set();
        }

        public virtual int SelectRearguardColumn(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            SelectRearguardColumn_Input();
            return int_input;
        }
        
        protected virtual void SelectRearguardColumn_Input()
        {
            while (true)
            {
                Log.WriteLine("----------\nChoose column to move Rearguard(s).\n" +
                    "1. Left\n" +
                    "2. Right\n");
                int_input = SelectPrompt(2) - 1;
                if (_actingPlayer.CheckColumn(int_input))
                    break;
                else
                    Log.WriteLine("No Rearguards in that column.");
            }
        }

        public virtual Tuple<int, int> MoveRearguards(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            cardsToSelect = _actingPlayer.GetActiveUnits();
            cardsToSelect.RemoveAt(cardsToSelect.Count - 1);
            MoveRearguards_Input();
            return new Tuple<int, int>(int_input, int_input2);
        }

        protected virtual void MoveRearguards_Input()
        {
            int selection = cardsToSelect.Count + 1;
            while (selection == cardsToSelect.Count + 1)
            {
                Log.WriteLine("Select rearguard to move.");
                for (int i = 0; i < cardsToSelect.Count; i++)
                    Log.WriteLine(i + 1 + ". " + cardsToSelect[i].name);
                selection = SelectPrompt(cardsToSelect.Count + 1);
            }
            int_input = cardsToSelect[selection - 1].tempID;
            selection = 5;
            while (selection == 5)
            {
                Log.WriteLine("Choose a direction.\n" +
                    "1. Up.\n2. Right.\n3. Down.\n4. Left.");
                selection = SelectPrompt(5);
            }
            int_input2 = selection;
            oSignalEvent.Set();
        }

        public virtual int SelectBattlePhaseAction(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            SelectBattlePhaseAction_Input();
            return int_input;
        }

        protected virtual void SelectBattlePhaseAction_Input()
        {
            Log.WriteLine("----------\nChoose option.\n" +
                        "1. See Hand\n" +
                        "2. See Field\n" +
                        "3. Attack\n" +
                        "4. End Battle Phase");
            int_input = SelectPrompt(5);
            oSignalEvent.Set();
        }

        public virtual int SelectAttackingUnit(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            SelectAttackingUnit_Input();
            return int_input;
        }

        protected virtual void SelectAttackingUnit_Input()
        {
            int selection = 0;
            List<Card> cards = _actingPlayer.GetCardsToAttackWith();
            Log.WriteLine("Choose unit to attack with.");
            for (int i = 0; i < cards.Count; i++)
                Log.WriteLine(i + 1 + ". " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectUnitToAttack(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            SelectUnitToAttack_Input();
            return int_input;
        }

        protected virtual void SelectUnitToAttack_Input()
        {
            int selection = 0;
            List<Card> cards = _actingPlayer.GetPotentialAttackTargets();
            Log.WriteLine("Choose unit to attack.");
            for (int i = 0; i < cards.Count; i++)
                Log.WriteLine(i + 1 + ". " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectGuardPhaseAction(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            SelectGuardPhaseAction_Input();
            return int_input;
        }

        protected virtual void SelectGuardPhaseAction_Input()
        {
            Log.WriteLine("----------\nGuard?\n" +
                        "1. See Hand.\n" +
                        "2. See Field.\n" +
                        "3. See Current Shield.\n" +
                        "4. Guard.\n" +
                        "5. Intercept.\n" +
                        "6. Use Blitz Order.\n" +
                        "7. End Guard.\n");
            int_input = SelectPrompt(7);
            oSignalEvent.Set();
        }

        public virtual int SelectCardToGuardWith(Player player)
        {
            _actingPlayer = player;
            SelectCardToGuardWith_Input();
            return int_input;
        }

        protected virtual void SelectCardToGuardWith_Input()
        {
            int selection = 0;
            string output;
            List<Card> cards = _actingPlayer.GetGuardableCards();
            Log.WriteLine("Choose card to put on Guardian Circle.");
            for (int i = 0; i < cards.Count; i++)
            {
                output = i + 1 + ". " + cards[i].name;
                Log.WriteLine(output);
            }
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectCardToGuard(Player actingPlayer)
        {
            _actingPlayer = actingPlayer;
            SelectCardToGuard_Input();
            return int_input;
        }

        protected virtual void SelectCardToGuard_Input()
        {
            int selection = 0;
            string output;
            List<Card> cards = _actingPlayer.GetAttackedCards();
            Log.WriteLine("Choose card to guard.");
            for (int i = 0; i < cards.Count; i++)
            {
                output = i + 1 + ". " + cards[i].name;
                Log.WriteLine(output);
            }
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectActiveUnit(Player actingPlayer, int request, int amount)
        {
            _actingPlayer = actingPlayer;
            prompt = request;
            value = amount;
            SelectActiveUnit_Input();
            if (OnChosen != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.card = _actingPlayer.GetCard(int_input);
                OnChosen(this, args);
            }
            return int_input;
        }

        protected virtual void SelectActiveUnit_Input()
        {
            int selection = 0;
            List<Card> cards = _actingPlayer.GetActiveUnits();
            for (int i = 0; i < cards.Count; i++)
                Log.WriteLine(i + 1 + ". " + cards[i].name);
            selection = SelectPrompt(cards.Count) - 1;
            int_input = cards[selection].tempID;
            oSignalEvent.Set();
        }

        public virtual int SelectAbility(Player actingPlayer, List<Ability> abilities)
        {
            _actingPlayer = actingPlayer;
            _abilities.Clear();
            _abilities.AddRange(abilities);
            SelectAbility_Input();
            return int_input;
        }

        public virtual int SelectAbility(Player actingPlayer, List<Tuple<Ability, int>> abilities)
        {
            _actingPlayer = actingPlayer;
            _abilities.Clear();
            foreach (Tuple<Ability, int> ability in abilities)
                _abilities.Add(ability.Item1);
            SelectAbility_Input();
            return int_input;
        }

        protected virtual void SelectAbility_Input()
        {
            string output = "";
            Log.WriteLine("----------\nSelect effect to activate.");
            for (int i = 0; i < _abilities.Count; i++)
            {
                output = (i + 1) + ". " + _abilities[i].Name;
                if (!_abilities[i].CanFullyResolve())
                    output += " (May not fully resolve.)";
                Log.WriteLine(output);
            }
            if (!CheckForMandatoryEffects(_abilities))
            {
                Log.WriteLine(_abilities.Count + 1 + ". Don't activate effect.");
                int_input = SelectPrompt(_abilities.Count + 1) - 1;
            }
            else
                int_input = SelectPrompt(_abilities.Count) - 1;
            oSignalEvent.Set();
        }

        public bool CheckForMandatoryEffects(List<Ability> abilities)
        {
            if (_actingPlayer.IsAlchemagic())
                return true;
            foreach (Ability ability in abilities)
            {
                if (ability.isMandatory)
                    return true;
            }
            return false;
        }

        public List<int> ChooseOrder(Player actingPlayer, List<Card> cardsToRearrange)
        {
            _actingPlayer = actingPlayer;
            Log.WriteLine("Choose order for card(s).");
            cardsToSelect.Clear();
            cardsToSelect.AddRange(cardsToRearrange);
            intlist_input.Clear();
            if (cardsToRearrange.Count > 0)
                ChooseOrder_Input();
            return new List<int>(intlist_input);
        }

        protected virtual void ChooseOrder_Input()
        {
            int selection = 0;
            List<Card> newOrder = new List<Card>();
            string output;
            while (selection < cardsToSelect.Count + 1)
            {
                for (int i = 0; i < cardsToSelect.Count; i++)
                {
                    output = i + 1 + ". " + cardsToSelect[i].name;
                    if (newOrder.Contains(cardsToSelect[i]))
                        output += "(" + (newOrder.IndexOf(cardsToSelect[i]) + 1) + ")";
                    Log.WriteLine(output);
                }
                if (newOrder.Count == cardsToSelect.Count)
                {
                    Log.WriteLine(cardsToSelect.Count + 1 + ". Finish selecting.");
                    selection = SelectPrompt(cardsToSelect.Count + 1);
                }
                else
                    selection = SelectPrompt(cardsToSelect.Count);
                if (selection < cardsToSelect.Count + 1)
                {
                    if (newOrder.Contains(cardsToSelect[selection - 1]))
                        newOrder.Remove(cardsToSelect[selection - 1]);
                    else
                        newOrder.Add(cardsToSelect[selection - 1]);
                }
            }
            foreach (Card card in newOrder)
                intlist_input.Add(card.tempID);
            oSignalEvent.Set();
        }

        public List<int> SelectFromList(Player actingPlayer, List<Card> cards, int count, int min, string query)
        {
            _actingPlayer = actingPlayer;
            return SelectFromList(actingPlayer, cards, count, min, query, false);
        }

        public List<int> SelectFromList(Player actingPlayer, List<Card> cards, int count, int min, string query, bool sameName)
        {
            int trueMin = min;
            List<int> temporaryList = new List<int>();
            _query = query;
            cardsToSelect.Clear();
            cardsToSelect.AddRange(cards);
            if (cardsToSelect.Count < count)
            {
                if (min == count)
                    min = cardsToSelect.Count;
                count = cardsToSelect.Count;
            }
            _query = "Choose " + count + " card(s) " + query + " min: (" + min + ")";
            if (cardsToSelect.Count == 0 || cardsToSelect.Count < min)
            {
                intlist_input.Clear();
                return new List<int>(intlist_input);
            }
            if (_query.Contains("retire"))
            {
                while (count > 1 && _query.Contains("retire") && cards.Exists(card => _actingPlayer.CanCountAsTwoRetires(card.tempID) && !_actingPlayer.IsEnemy(card.tempID)))
                {
                    _query = "Choose " + count + " card(s)";
                    if (query == "")
                        _query += ".";
                    else
                        _query += " " + query;
                    _query += " min: (" + min + ")";
                    int_value = 1;
                    if (min > 0)
                        int_value2 = 1;
                    else
                        int_value2 = 0;
                    int_value3 = trueMin;
                    SelectFromList_Input();
                    if (intlist_input.Count == 0)
                        break;
                    else
                    {
                        if (_actingPlayer.CanCountAsTwoRetires(intlist_input[0]) && YesNo(actingPlayer, "Count as two retires?"))
                        {
                            count -= 2;
                            min -= 2;
                            trueMin -= 2;
                        }
                        else
                        {
                            count--;
                            min--;
                            trueMin--;
                        }
                        temporaryList.Add(intlist_input[0]);
                        cardsToSelect.Remove(cardsToSelect.Find(card => card.tempID == intlist_input[0]));
                        intlist_input.Clear();
                    }
                }
            }
            else if (sameName && count > 1)
            {
                int_value = 1;
                if (min > 0)
                    int_value2 = 1;
                else
                    int_value2 = 0;
                SelectFromList_Input();
                cardsToSelect.Remove(cardsToSelect.Find(card => card.tempID == intlist_input[0]));
                temporaryList.Add(intlist_input[0]);
                List<Card> currentList = new List<Card>(cardsToSelect);
                foreach (Card card in currentList)
                {
                    if (card.name != _actingPlayer.GetCard(intlist_input[0]).name)
                        cardsToSelect.Remove(card);
                }
                count--;
                min--;
                intlist_input.Clear();
            }
            if (count > 0 || min > 0)
            {
                int_value = count;
                int_value2 = min;
                SelectFromList_Input();
            }
            intlist_input.AddRange(temporaryList);
            foreach (int tempID in intlist_input)
            {
                if (OnChosen != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.card = actingPlayer.GetCard(tempID);
                    OnChosen(this, args);
                }
            }
            return new List<int>(intlist_input);
        }

        protected virtual void SelectFromList_Input()
        {
            int selection;
            int count = int_value;
            int min = int_value2;
            intlist_input.Clear();
            for (int j = 0; j < count; j++)
            {
                Log.WriteLine(_query);
                for (int i = 0; i < cardsToSelect.Count; i++)
                {
                    Log.WriteLine(i + 1 + ". " + cardsToSelect[i].name);
                }
                if (intlist_input.Count >= min)
                {
                    Log.WriteLine(cardsToSelect.Count + 1 + ". End selection.");
                    selection = SelectPrompt(cardsToSelect.Count + 1) - 1;
                }
                else
                    selection = SelectPrompt(cardsToSelect.Count) - 1;
                if (intlist_input.Count >= min && selection == cardsToSelect.Count)
                    return;
                intlist_input.Add(cardsToSelect[selection].tempID);
                cardsToSelect.RemoveAt(selection);
            }
        }

        public int SelectOption(Player actingPlayer, string[] list)
        {
            _actingPlayer = actingPlayer;
            _list = list;
            SelectOption_Input();
            return int_input;
        }

        protected virtual void SelectOption_Input()
        {
            int selection;
            Log.WriteLine("Choose an option.");
            for (int i = 0; i < _list.Length; i++)
            {
                Log.WriteLine(i + 1 + ". " + _list[i]);
            }
            selection = SelectPrompt(_list.Length);
            int_input = selection;
            oSignalEvent.Set();
        }

        public int SelectColumn(Player player)
        {
            _actingPlayer = player;
            SelectColumn_Input();
            return int_input;
        }

        protected void SelectColumn_Input()
        {
            Log.WriteLine("Choose column.\n" +
                "1. Left.\n" +
                "2. Center.\n" +
                "3. Right.");
            int_input = SelectPrompt(3);
            if (int_input == 1)
                int_input = -1;
            else if (int_input == 2)
                int_input = 0;
            else if (int_input == 3)
                int_input = 1;
            oSignalEvent.Set();
        }

        public void DisplayCards(Player actingPlayer, List<Card> cards)
        {
            _actingPlayer = actingPlayer;
            cardsToSelect.Clear();
            cardsToSelect.AddRange(cards);
            DisplayCards_Input();
        }

        protected virtual void DisplayCards_Input()
        {
            Log.WriteLine("----------");
            for (int i = 0; i < cardsToSelect.Count; i++)
            {
                Log.WriteLine(i + 1 + ". " + cardsToSelect[i].name);
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
        public const int UniqueQuery = 8;
    }

    public class RidePhaseAction
    {
        public const int RideFromHand = 4;
        public const int RideFromRideDeck = 1;
        public const int End = 3;
    }

    public class MainPhaseAction
    {
        public const int CallFromPrison = 4;
        public const int CallFromHand = 9;
        public const int MoveRearguard = 10;
        public const int ActivateAbility = 11;
        public const int End = 8;
        public const int SoulCharge = 12;
        public const int CounterCharge = 13;
        public const int TakeDamage = 14;
        public const int Heal = 15;
        public const int MoveRearguardFreeSwap = 16;
        public const int ActivateAbilityFromDrop = 17;
    }

    public class BattlePhaseAction
    {
        public const int Attack = 5;
        public const int End = 4;
    }

    public class GuardStepAction
    {
        public const int Guard = 4;
        public const int End = 7;
        public const int BlitzOrder = 8;
        public const int Intercept = 9;
    }
}

