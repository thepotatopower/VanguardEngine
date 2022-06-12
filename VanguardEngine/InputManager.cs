using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

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
        public List<AbilityTimingCount> _abilities = new List<AbilityTimingCount>();
        public List<Ability> _abilities2 = new List<Ability>();
        public List<Card> cardsToSelect = new List<Card>();
        public List<List<Card>> cardSets = new List<List<Card>>();
        public string string_input;
        public int prompt;
        public int value;
        public int int_value;
        public int int_value2;
        public int int_value3;
        public bool bool_value;
        public int _max = 0;
        public int _min = 0;
        public int _trueMin = 0;
        public Card card_input;
        public string _query;
        public string[] _list;
        public List<int> _ints = new List<int>();
        public List<int> _ints2 = new List<int>();
        public List<int> _circles = new List<int>();
        public List<int> _tempIDs = new List<int>();
        public List<bool> _bools = new List<bool>();
        public static ManualResetEvent oSignalEvent = new ManualResetEvent(false);
        public EventHandler<CardEventArgs> OnPlayerSwap;
        public EventHandler<CardEventArgs> OnChosen;
        public bool _cancellable = false;
        string inputlog = "";
        int POV = -1;
        int seed = -1;
        List<string> inputLogInputs;
        public string logDirectory = "Replays/";

        public void Initialize(Player player1, Player player2, List<Card> player1Deck, List<Card> player2Deck, int pov, int seed)
        {
            _actingPlayer = player1;
            DateTime dt = DateTime.Now;
            if (inputLogInputs == null || inputLogInputs.Count == 0)
            {
                POV = pov;
                inputlog = logDirectory + dt.Hour.ToString() + dt.Minute.ToString() + dt.Second.ToString() + dt.Millisecond.ToString() + ".txt";
                if (player1Deck.Count < 50 || player2Deck.Count < 50)
                    throw new ArgumentException("error with deck size");
                RecordInput("#RideDeck");
                for (int i = 0; i < 4; i++)
                    RecordInput(player1Deck[i].id);
                RecordInput("#MainDeck");
                for (int i = 4; i < 50; i++)
                    RecordInput(player1Deck[i].id);
                RecordInput("#RideDeck");
                for (int i = 0; i < 4; i++)
                    RecordInput(player2Deck[i].id);
                RecordInput("#MainDeck");
                for (int i = 4; i < 50; i++)
                    RecordInput(player2Deck[i].id);
                RecordInput(POV.ToString());
                RecordInput(seed.ToString());
            }
            //_player2 = player2;
        }

        public void SetReplayDirectory(string directory)
        {
            logDirectory = directory;
        }

        public void ReadFromInputLog(string inputfile)
        {
            inputLogInputs = new List<string>();
            List<string> lines = File.ReadLines(inputfile).ToList();
            for (int i = 0; i < 104; i++)
                lines.RemoveAt(0);
            POV = Int32.Parse(lines[0]);
            seed = Int32.Parse(lines[1]);
            lines.RemoveAt(0);
            lines.RemoveAt(0);
            inputLogInputs.AddRange(lines);
        }

        public int GetPOV()
        {
            return POV;
        }

        public int GetSeed()
        {
            return seed;
        }

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

        public virtual bool YesNo(Player actingPlayer, string query)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetBoolInput();
            _actingPlayer = actingPlayer;
            prompt = PromptType.UniqueQuery;
            string_input = query;
            YesNo_Input();
            RecordInput(actingPlayer, bool_input);
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            RPS_Input();
            RecordInput(actingPlayer, int_input);
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetListInput();
            _actingPlayer = actingPlayer;
            SelectCardsToMulligan_Input();
            RecordInput(actingPlayer, intlist_input);
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

        public virtual Tuple<int, int> SelectRidePhaseAction(Player actingPlayer)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetTupleInput();
            _actingPlayer = actingPlayer;
            SelectRidePhaseAction_Input();
            Tuple<int, int> input = new Tuple<int, int>(int_input, int_input2);
            RecordInput(actingPlayer, input);
            return input;
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

        public virtual Tuple<int, int> SelectMainPhaseAction(Player actingPlayer)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetTupleInput();
            _actingPlayer = actingPlayer;
            SelectMainPhaseAction_Input();
            Tuple<int, int> input = new Tuple<int, int>(int_input, int_input2);
            RecordInput(actingPlayer, input);
            return input;
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            SelectRearguardToCall_Input();
            RecordInput(actingPlayer, int_input);
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            _query = query;
            _circles.Clear();
            _circles.AddRange(selectedCircles);
            _tempIDs.Clear();
            card_input = card;
            if (canSelect != null)
                _tempIDs.AddRange(canSelect);
            SelectCallLocation_Input();
            RecordInput(actingPlayer, int_input);
            return int_input;
        }

        protected virtual void SelectCallLocation_Input()
        {
            bool proceed = false;
            bool isEnemy = false;
            if (_tempIDs.Count > 0 && !_actingPlayer.GetMyCircles().Contains(_tempIDs[0]))
                isEnemy = true;
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
                if (isEnemy)
                    int_input = FL.SwitchSides(int_input);
                if (!_circles.Contains(int_input) && ((_tempIDs.Count > 0 && _tempIDs.Contains(int_input)) || _tempIDs.Count == 0))
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetListInput();
            if (availableCircles.Count == 0)
                return new List<int>();
            _actingPlayer = actingPlayer;
            intlist_input.Clear();
            intlist_input.AddRange(availableCircles);
            _max = count;
            SelectCircle_Input();
            RecordInput(actingPlayer, intlist_input);
            return new List<int>(intlist_input);
        }

        protected virtual void SelectCircle_Input()
        {
            string output;
            int_input = -1;
            List<int> selectedCircles = new List<int>();
            while (selectedCircles.Count < _max)
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            SelectRearguardColumn_Input();
            RecordInput(actingPlayer, int_input);
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetTupleInput();
            _actingPlayer = actingPlayer;
            cardsToSelect = _actingPlayer.GetActiveUnits();
            cardsToSelect.RemoveAt(cardsToSelect.Count - 1);
            MoveRearguards_Input();
            RecordInput(actingPlayer, new Tuple<int, int>(int_input, int_input2));
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

        public virtual Tuple<int, int> SelectBattlePhaseAction(Player actingPlayer)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetTupleInput();
            _actingPlayer = actingPlayer;
            SelectBattlePhaseAction_Input();
            Tuple<int, int> tupleInput = new Tuple<int, int>(int_input, int_input2);
            RecordInput(actingPlayer, tupleInput);
            return tupleInput;
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            SelectAttackingUnit_Input();
            RecordInput(actingPlayer, int_input);
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            SelectUnitToAttack_Input();
            RecordInput(actingPlayer, int_input);
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

        public virtual Tuple<int, int> SelectGuardPhaseAction(Player actingPlayer)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetTupleInput();
            _actingPlayer = actingPlayer;
            SelectGuardPhaseAction_Input();
            Tuple<int, int> input = new Tuple<int, int>(int_input, int_input2);
            RecordInput(actingPlayer, input);
            return input;
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = player;
            SelectCardToGuardWith_Input();
            RecordInput(player, int_input);
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            SelectCardToGuard_Input();
            RecordInput(actingPlayer, int_input);
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
            {
                int input = GetIntInput();
                if (OnChosen != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.card = _actingPlayer.GetCard(input);
                    OnChosen(this, args);
                }
                return input;
            }
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
            RecordInput(actingPlayer, int_input);
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            _abilities.Clear();
            foreach (Ability ability in abilities)
            {
                AbilityTimingCount abilityTimingCount = new AbilityTimingCount(ability, 0, new Tuple<int, int>(0, 0));
                _abilities.Add(abilityTimingCount);
            }
            SelectAbility_Input();
            RecordInput(actingPlayer, int_input);
            return int_input;
        }

        public virtual int SelectAbility(Player actingPlayer, List<AbilityTimingCount> abilities)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            _abilities.Clear();
            _abilities.AddRange(abilities);
            SelectAbility_Input();
            RecordInput(actingPlayer, int_input);
            return int_input;
        }

        protected virtual void SelectAbility_Input()
        {
            string output = "";
            Log.WriteLine("----------\nSelect effect to activate.");
            for (int i = 0; i < _abilities.Count; i++)
            {
                output = (i + 1) + ". " + _abilities[i].ability.Name;
                if (!_abilities[i].ability.CanFullyResolve(_abilities[i].activation, _abilities[i].timingCount))
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

        public bool CheckForMandatoryEffects(List<AbilityTimingCount> abilities)
        {
            if (_actingPlayer.IsAlchemagic())
                return true;
            foreach (AbilityTimingCount ability in abilities)
            {
                if (ability.ability.isMandatory)
                    return true;
            }
            return false;
        }

        public List<int> ChooseOrder(Player actingPlayer, List<Card> cardsToRearrange)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetListInput();
            _actingPlayer = actingPlayer;
            Log.WriteLine("Choose order for card(s).");
            cardsToSelect.Clear();
            cardsToSelect.AddRange(cardsToRearrange);
            intlist_input.Clear();
            if (cardsToRearrange.Count == 1)
            {
                intlist_input.Add(cardsToRearrange[0].tempID);
                return new List<int>(intlist_input);
            }
            if (cardsToRearrange.Count > 0)
                ChooseOrder_Input();
            RecordInput(actingPlayer, intlist_input);
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
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetListInput();
            _actingPlayer = actingPlayer;
            return SelectFromList(actingPlayer, cards, count, min, query, new List<int>());
        }

        public List<int> SelectFromList(Player actingPlayer, List<List<Card>> cards, int count, int min, string query)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetListInput();
            _actingPlayer = actingPlayer;
            List<Card> allCards = new List<Card>();
            foreach (List<Card> list in cards)
                allCards.AddRange(list);
            cardSets = cards;
            return SelectFromList(actingPlayer, allCards, count, min, query);
        }

        public List<int> SelectFromList(Player actingPlayer, List<Card> cards, int count, int min, string query, List<int> specifications)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
            {
                List<int> inputs = GetListInput();
                foreach (int tempID in inputs)
                {
                    if (OnChosen != null)
                    {
                        CardEventArgs args = new CardEventArgs();
                        args.card = actingPlayer.GetCard(tempID);
                        OnChosen(this, args);
                    }
                }
                return inputs;
            }
            if (count == 0 && min == 0)
            {
                RecordInput(actingPlayer, new List<int>());
                return new List<int>();
            }
            if (specifications.Contains(Property.Cancellable))
                _cancellable = true;
            else
                _cancellable = false;
            _actingPlayer = actingPlayer;
            intlist_input.Clear();
            int originalCount = count;
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
                RecordInput(actingPlayer, intlist_input);
                return new List<int>(intlist_input);
            }
            if (count == min && cards.Count == count)
            {
                foreach (Card card in cards)
                    intlist_input.Add(card.tempID);
            }
            else
            {
                if (_query.Contains("retire") && cards.Count >= originalCount && cards.Exists(card => _actingPlayer.CanCountAsTwoRetires(card.tempID) && !_actingPlayer.IsEnemy(card.tempID)))
                {
                    while (count > 1 && _query.Contains("retire") && cards.Exists(card => _actingPlayer.CanCountAsTwoRetires(card.tempID) && !_actingPlayer.IsEnemy(card.tempID)))
                    {
                        _query = "Choose " + count + " card(s)";
                        if (query == "")
                            _query += ".";
                        else
                            _query += " " + query;
                        _query += " min: (" + min + ")";
                        _max = 1;
                        if (min > 0)
                            _min = 1;
                        else
                            _min = 0;
                        _trueMin = trueMin;
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
                if ((specifications.Contains(Property.SameName) || specifications.Contains(Property.DifferentNames)) && count > 1)
                {
                    while (temporaryList.Count < min)
                    {
                        _max = 1;
                        if (min > 0)
                            _min = 1;
                        else
                            _min = 0;
                        SelectFromList_Input();
                        if (intlist_input.Count > 0)
                        {
                            cardsToSelect.Remove(cardsToSelect.Find(card => card.tempID == intlist_input[0]));
                            temporaryList.Add(intlist_input[0]);
                            foreach (int tempID in temporaryList)
                            {
                                if (specifications.Contains(Property.SameName))
                                    cardsToSelect.RemoveAll(card => card.name != _actingPlayer.GetCard(tempID).name);
                                if (specifications.Contains(Property.DifferentNames))
                                    cardsToSelect.RemoveAll(card => card.name == _actingPlayer.GetCard(tempID).name);
                            }
                            intlist_input.Clear();
                        }
                        else
                            break;
                    }
                    intlist_input.Clear();
                }
                else if (cardSets.Count > 1)
                {
                    cardsToSelect.Clear();
                    List<int> selectedIDs = new List<int>();
                    while (selectedIDs.Count < min)
                    {
                        _max = 1;
                        _min = 1;
                        cardsToSelect.Clear();
                        foreach (List<Card> set in cardSets)
                        {
                            if (selectedIDs.Exists(id => set.Exists(card => card.tempID == id)))
                                continue;
                            cardsToSelect.AddRange(set);
                        }
                        SelectFromList_Input();
                        selectedIDs.AddRange(intlist_input);
                    }
                    intlist_input.Clear();
                    intlist_input.AddRange(selectedIDs);
                }
                else
                {
                    if (count > 0 || min > 0)
                    {
                        _max = count;
                        _min = min;
                    }
                    if (count == min && cards.Count == count)
                    {
                        foreach (Card card in cards)
                            intlist_input.Add(card.tempID);
                    }
                    else
                        SelectFromList_Input();
                }
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
            cardSets.Clear();
            _cancellable = false;
            RecordInput(actingPlayer, intlist_input);
            return new List<int>(intlist_input);
        }

        protected virtual void SelectFromList_Input()
        {
            int selection;
            int count = _max;
            int min = _min;
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

        public int SelectOption(Player actingPlayer, params string[] list)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            _list = list;
            _bools.Clear();
            foreach (string s in list)
                _bools.Add(true);
            SelectOption_Input();
            RecordInput(actingPlayer, int_input);
            return int_input;
        }

        public int SelectOption(Player actingPlayer, params Tuple<string, bool>[] list)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = actingPlayer;
            _list = new string[list.Length];
            _bools.Clear();
            for (int i = 0; i < list.Length; i++)
            {
                _list[i] = list[i].Item1;
                _bools.Add(list[i].Item2);
            }
            SelectOption_Input();
            RecordInput(actingPlayer, int_input);
            return int_input;
        }

        protected virtual void SelectOption_Input()
        {
            int selection = 0;
            Log.WriteLine("Choose an option.");
            while (selection <= 0 || !_bools[selection - 1])
            {
                for (int i = 0; i < _list.Length; i++)
                {
                    Log.WriteLine(i + 1 + ". " + _list[i]);
                }
                selection = SelectPrompt(_list.Length);
            }
            int_input = selection;
            oSignalEvent.Set();
        }

        public int SelectColumn(Player player)
        {
            if (inputLogInputs != null && inputLogInputs.Count > 0)
                return GetIntInput();
            _actingPlayer = player;
            SelectColumn_Input();
            RecordInput(player, int_input);
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
            if (inputLogInputs.Count > 0)
                return;
            Log.WriteLine("----------");
            for (int i = 0; i < cardsToSelect.Count; i++)
            {
                Log.WriteLine(i + 1 + ". " + cardsToSelect[i].name);
            }
            oSignalEvent.Set();
        }

        public void RecordInput(Player player, int input)
        {
            RecordInput("P" + player._playerID + " int" + input);
        }

        public int GetIntInput()
        {
            string line = inputLogInputs[0];
            inputLogInputs.RemoveAt(0);
            int index = line.IndexOf("int");
            return Int32.Parse(line.Substring(index + 3));
        }

        public void RecordInput(Player player, List<int> inputs)
        {
            string line = "P" + player._playerID + " list";
            foreach (int input in inputs)
                line += input + "|";
            RecordInput(line);
        }

        public List<int> GetListInput()
        {
            string line = inputLogInputs[0];
            inputLogInputs.RemoveAt(0);
            int index = line.IndexOf("list");
            List<int> inputs = new List<int>();
            List<string> split = line.Substring(index + 4).Split('|').ToList<string>();
            List<int> converted = new List<int>();
            foreach (string item in split)
            {
                if (item != "")
                    converted.Add(Int32.Parse(item));
            }
            return converted;
        }

        public void RecordInput(Player player, bool input)
        {
            string line = "P" + player._playerID + " bool";
            if (input)
                line += "true";
            else
                line += "false";
            RecordInput(line);
        }

        public bool GetBoolInput()
        {
            string line = inputLogInputs[0];
            inputLogInputs.RemoveAt(0);
            return line.Contains("true");
        }

        public void RecordInput(Player player, Tuple<int, int> input)
        {
            string line = "P" + player._playerID + " tuple" + input.Item1 + "|" + input.Item2;
            RecordInput(line);
        }

        public Tuple<int, int> GetTupleInput()
        {
            string line = inputLogInputs[0];
            inputLogInputs.RemoveAt(0);
            int index = line.IndexOf("tuple");
            List<int> inputs = new List<int>();
            List<string> split = line.Substring(index + 5).Split('|').ToList<string>();
            return new Tuple<int, int>(Int32.Parse(split[0]), Int32.Parse(split[1]));
        }

        public void RecordInput(string input)
        {
            using (StreamWriter writer = new StreamWriter(inputlog, true))
            {
                writer.WriteLine(input);
            }
        }

        public bool IsReadingInputLog()
        {
            return inputLogInputs != null;
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
        public const int ActivateAbilityFromSoul = 18;
        public const int ActivateAbilityFromOrderZone = 19;
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

