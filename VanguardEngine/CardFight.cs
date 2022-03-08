using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;


namespace VanguardEngine
{
    public class CardFight
    {
        public Player _player1;
        public Player _player2;
        public Player _actingPlayer;
        public int _turn;
        protected int _phase = Phase.Stand;
        public bool _gameOver = false;
        public bool _activatedOrder = false;
        public LuaInterpreter luaInterpreter;
        public InputManager _inputManager;
        public List<Card> _deck1;
        public List<Card> _deck2;
        public Abilities _abilities;
        public List<AbilityTimingCount> _player1AbilityQueue = new List<AbilityTimingCount>();
        public List<AbilityTimingCount> _player2AbilityQueue = new List<AbilityTimingCount>();
        public List<Ability> _continuousAbilities = new List<Ability>();
        public List<Ability> _alchemagicQueue = new List<Ability>();
        public List<Ability> _activatedAbilities = new List<Ability>();
        public List<AbilityTimingCount> _skippedAbilities = new List<AbilityTimingCount>();
        public AbilityTimings _abilityTimings = new AbilityTimings();
        public Dictionary<int, List<int>> _chosen = new Dictionary<int, List<int>>();
        public Ability _currentAbility = null;
        public EventHandler<CardEventArgs> OnStandPhase;
        public EventHandler<CardEventArgs> OnDrawPhase;
        public EventHandler<CardEventArgs> OnRidePhase;
        public EventHandler<CardEventArgs> OnMainPhase;
        public EventHandler<CardEventArgs> OnBattlePhase;
        public EventHandler<CardEventArgs> OnEndPhase;
        public EventHandler<CardEventArgs> OnAttackHits;
        public EventHandler<CardEventArgs> OnAbilityActivated;
        public EventHandler<CardEventArgs> OnFree;
        public EventHandler<CardEventArgs> OnAlchemagic;
        int _clientNumber = 0;
        string _connectionString;
        public Dictionary<int, List<ActionLog>> actionLogs = new Dictionary<int, List<ActionLog>>();

        public bool Initialize(List<Card> Deck1, List<Card> Deck2, List<Card> tokens, InputManager inputManager, string luaPath, string connectionString, int clientNumber)
        {
            //if (File.Exists("enginelog.txt"))
            //{
            //    using (StreamWriter writer = new StreamWriter("enginelog.txt", false))
            //    {
            //        writer.Write(string.Empty);
            //    }
            //}
            _clientNumber = clientNumber;
            _connectionString = connectionString;
            List<Card> deck1;
            List<Card> deck2;
            Field field = new Field();
            deck1 = Deck1;
            deck2 = Deck2;
            foreach (Card card in deck2)
                card.tempID += deck1.Count;
            _player1 = new Player();
            _player2 = new Player();
            _player1.OnAbilityTiming += OnAbilityTiming;
            _player2.OnAbilityTiming += OnAbilityTiming;
            _player1.OnMarkedForRetire += MarkedForRetire;
            _player2.OnMarkedForRetire += MarkedForRetire;
            _player1.OnZoneChanged += UpdateTracking;
            _abilities = new Abilities();
            inputManager.Initialize(_player1, _player2);
            _inputManager = inputManager;
            luaInterpreter = new LuaInterpreter(luaPath, this);
            field.Initialize(deck1, deck2, tokens, clientNumber);
            _player1.Initialize(1, field);
            _player2.Initialize(2, field);
            for (int i = 0; i < deck1.Count; i++)
            {
                _abilities.AddAbilities(deck1[i].tempID, luaInterpreter.GetAbilities(deck1[i], _player1, _player2, true));
                //_abilities.AddAbilities(deck1[i].tempID, luaInterpreter.GetAbilities(deck1[i], _player2, _player1, false));
            }
            for (int i = 0; i < deck2.Count; i++)
            {
                _abilities.AddAbilities(deck2[i].tempID, luaInterpreter.GetAbilities(deck2[i], _player2, _player1, true));
                //_abilities.AddAbilities(deck2[i].tempID, luaInterpreter.GetAbilities(deck2[i], _player1, _player2, false));
            }
            return true;
        }

        public void LoadConfigFile(string filename)
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = filename;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            Console.WriteLine(ConfigurationManager.AppSettings.AllKeys.Length);
            foreach (string s in ConfigurationManager.AppSettings.AllKeys)
            {
                Console.WriteLine("key: " + s + " value: " + ConfigurationManager.AppSettings.Get(s));
            }
        }

        public string LoadName(string name)
        {
            return ConfigurationManager.AppSettings.Get(name);
        }

        public Card LoadCard(string cardID)
        {
            List<string> list = new List<string>();
            list.Add(cardID);
            List<Card> cards = LoadCards.GenerateCardsFromList(list, _connectionString);
            if (cards.Count > 0)
                return cards[0];
            return null;
        }

        public void AddAbility(Ability ability)
        {
            List<Ability> abilities = new List<Ability>();
            abilities.Add(ability);
            _abilities.AddAbilities(ability.GetID(), abilities);
        }

        public Ability GetAbility(int tempID, int activation)
        {
            return _abilities.GetAbilityWithActivation(tempID, activation);
        }

        public List<Ability> GetAbilities(int tempID)
        {
            return _abilities.GetAbilities(tempID);
        }

        public void StartFight()
        {
            Player player1;
            Player player2;
            CardEventArgs args;
            int RPS1 = 0, RPS2 = 0;
            int first = 0;
            string input;
            _player1.Shuffle();
            _player2.Shuffle();
            while (RPS1 == RPS2)
            {
                RPS1 = _inputManager.RPS(_player1);
                RPS2 = _inputManager.RPS(_player2);
                if (RPS1 < 0 || RPS1 > 2)
                    RPS1 = 0;
                if (RPS2 < 0 || RPS2 > 2)
                    RPS2 = 0;
                if (RPS1 == RPS2)
                    continue;
                if ((RPS1 == 0 && RPS2 == 1) || (RPS1 == 1 && RPS2 == 2) || (RPS1 == 2 && RPS2 == 0))
                    first = 2;
                else
                    first = 1;
            }
            if (first == 1)
            {
                player1 = _player1;
                player2 = _player2;
            }
            else
            {
                player1 = _player2;
                player2 = _player1;
            }
            player1._startingTurn = 1;
            player2._startingTurn = 2;
            Draw(player1, player2, 5);
            Draw(player2, player1, 5);
            if (player1 == _player1)
                Log.WriteLine("----------\nPLAYER 1 GOES FIRST.");
            else
            {
                Log.WriteLine("----------\nPLAYER 2 GOES FIRST.");
                //_inputManager.SwapPlayers();
            }
            Mulligan(player1, player2);
            Log.WriteLine("----------\nSTAND UP! VANGUARD!");
            player1.StandUpVanguard();
            player2.StandUpVanguard();
            _turn = 1;
            _phase = 0;
            _actingPlayer = player1;
            TriggerCheck(player1, player2, false);
            TriggerCheck(player1, player2, false);
            TriggerCheck(player2, player1, false);
            TriggerCheck(player2, player1, false);
            //player1.SoulCharge(10);
            //player2.SoulCharge(10);
            //player1.AbyssalDarkNight();
            //player2.AbyssalDarkNight();
            //player1.Mill(5);
            //player2.Mill(5);
            while (true)
            {
                _actingPlayer = player1;
                player1.UpdateRecordedValues();
                player2.UpdateRecordedValues();
                _phase = Phase.Stand;
                if (OnStandPhase != null)
                    OnStandPhase(this, new CardEventArgs());
                PerformCheckTiming(player1, player2);
                player1.StandAll();
                PerformCheckTiming(player1, player2);
                _phase = Phase.Draw;
                if (OnDrawPhase != null)
                    OnDrawPhase(this, new CardEventArgs());
                Log.WriteLine("----------\nSTAND AND DRAW");
                PerformCheckTiming(player1, player2);
                Draw(player1, player2, 1);
                PerformCheckTiming(player1, player2);
                Log.WriteLine("----------\nRIDE PHASE");
                _phase = Phase.Ride;
                RidePhaseMenu(player1, player2);
                Log.WriteLine("----------\nMAIN PHASE");
                _phase = Phase.Main;
                MainPhaseMenu(player1, player2);
                if (_turn > 1)
                {
                    Log.WriteLine("----------\nBATTLE PHASE");
                    _phase = Phase.Battle;
                    BattlePhaseMenu(player1, player2);
                }
                if (_gameOver)
                {
                    if (player1 == _player1)
                        Log.WriteLine("----------\nPLAYER 1 WINS.");
                    else
                        Log.WriteLine("PLAYER 2 WINS.");
                    input = Console.ReadLine();
                    return;
                }
                Log.WriteLine("----------\nEND PHASE");
                _phase = Phase.End;
                if (OnEndPhase != null)
                {
                    args = new CardEventArgs();
                    OnEndPhase(this, args);
                }
                AddAbilityTiming(Activation.OnEndPhase, 0);
                PerformCheckTiming(player1, player2);
                player1.EndTurn();
                player2.EndTurn();
                player1.CardStates.EndTurn();
                player1.IncrementTurn();
                _abilities.EndTurn();
                actionLogs.Clear();
                _activatedOrder = false;
                _turn++;
                player1.UpdateRecordedValues();
                player2.UpdateRecordedValues();
                if (player1 == _player1)
                {
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                    player1 = _player2;
                    player2 = _player1;
                    //_inputManager.SwapPlayers();
                }
                else
                {
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                    player1 = _player1;
                    player2 = _player2;
                    //_inputManager.SwapPlayers();
                }
            }
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
        public void Draw(Player player1, Player player2, int count)
        {
            player1.Draw(count);
        }

        public void Mulligan(Player player1, Player player2)
        {
            List<int> selection;
            for (int i = 0; i < 2; i++)
            {
                selection = _inputManager.SelectCardsToMulligan(player1);
                player1.MulliganCards(selection);
                Log.WriteLine("----------\nNew hand: ");
                player1.PrintHand();
                if (player1 == _player1)
                {
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                    player1 = _player2;
                    player2 = _player1;
                    //_inputManager.SwapPlayers();
                }
                else
                {
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                    player1 = _player1;
                    player2 = _player2;
                    //_inputManager.SwapPlayers();
                }
            }
        }

        public void BrowseHand(Player player1)
        {
            int max;
            int selection;
            while (true)
            {
                max = player1.PrintHand();
                Log.WriteLine(max + ". Go back.");
                selection = SelectPrompt(max);
                if (selection == max)
                    break;
                player1.DisplayCardInHand(selection - 1);
            }
        }

        public void BrowseField(Player player1)
        {
            int max;
            int selection;
            while (true)
            {
                max = player1.PrintField();
                selection = SelectPrompt(max);
                if (selection == max)
                    break;
                if (selection == 5) //enemy vanguard
                    player1.DisplayVanguard(true);
                else if (selection == 9) //player vanguard
                    player1.DisplayVanguard(false);
                else if (selection == 14)
                    player1.DisplayDrop();
                else if (selection == 15)
                    player1.DisplaySoul();
                else if (selection == 16)
                    player1.DisplayBind();
                else
                    player1.DisplayRearguard(selection);
            }
        }

        public void RidePhaseMenu(Player player1, Player player2)
        {
            int input;
            int selection = 0;
            CardEventArgs args;
            if (OnRidePhase != null)
            {
                args = new CardEventArgs();
                OnRidePhase(this, args);
            }
            AddAbilityTiming(Activation.OnRidePhase, 0);
            PerformCheckTiming(player1, player2);
            while (player1.CanRideFromRideDeck() || player1.CanRideFromHand())
            {
                selection = _inputManager.SelectRidePhaseAction(player1);
                if (selection == RidePhaseAction.RideFromRideDeck)
                {
                    if (player1.CanRideFromRideDeck())
                    {
                        if (player1.MyStates.HasState(PlayerState.SoulBlastForRideDeck) && player1.GetSoul().Count > 0 && _inputManager.YesNo(player1, "Soul Blast 1 instead of discarding card?"))
                            SoulBlast(player1, player2, player1.GetSoul(), 1);
                        else if (RideDeckDiscardCostReplaced(player1)) ;
                        else if (player1.MyStates.GetStrings(PlayerState.CanRideFromRideDeckWithoutDiscard).Contains(player1.GetRideDeck().Find(card => card.OriginalGrade() == player1.Grade(player1.Vanguard().tempID) + 1).id) &&
                            (_player1.GetHand().Count == 0 || _inputManager.YesNo(player1, "Skip discard cost?"))) ;
                        else
                            Discard(player1, player2, player1.GetHand(), 1, 1);
                        input = _inputManager.SelectFromList(player1, player1.GetRideableCards(true), 1, 1, "to ride.")[0];
                        Ride(player1, player2, 0, input);
                        PerformCheckTiming(player1, player2);
                    }
                    else
                        Log.WriteLine("Invalid.");
                }
                else if (selection == 2)
                {
                    if (player1.CanRideFromHand())
                    {
                        input = _inputManager.SelectFromList(player1, player1.GetRideableCards(false), 1, 1, "to ride.")[0];
                        Ride(player1, player2, 0, input);
                        PerformCheckTiming(player1, player2);
                    }
                    else
                        Log.WriteLine("Invalid.");
                }
                else if (selection == RidePhaseAction.RideFromHand)
                {
                    Ride(player1, player2, 0, _inputManager.int_input2);
                    PerformCheckTiming(player1, player2);
                }
                else if (selection == RidePhaseAction.End)
                    break;
            }
        }

        public void Ride(Player player1, Player player2, int location, int selection)
        {
            player1.Ride(selection);
            AddAbilityTiming(Activation.OnRide, player1._playerID, player1.GetLastRidden());
            AddAbilityTiming(Activation.PlacedOnVC, player1._playerID, player1.GetLastPlacedOnVC());
        }

        public void Discard(Player player1, Player player2, List<Card> cardsToSelect, int max, int min)
        {
            List<int> cardsToDiscard = _inputManager.SelectFromList(player1, cardsToSelect, max, min, "to discard.");
            player1.Discard(cardsToDiscard);
            AddAbilityTiming(Activation.OnDiscard, player1._playerID, player1.GetLastDiscarded());
        }

        public void MainPhaseMenu(Player player1, Player player2)
        {
            int selection;
            Tuple<int, int> selections;
            List<int> canSelect = new List<int>();
            int location;
            int input;
            CardEventArgs args;
            if (OnMainPhase != null)
            {
                args = new CardEventArgs();
                OnMainPhase(this, args);
            }
            AddAbilityTiming(Activation.OnMainPhase, 0);
            PerformCheckTiming(player1, player2);
            while (true)
            {
                canSelect.Clear();
                _inputManager._abilities.Clear();
                _inputManager._abilities.AddRange(GetACTAbilities(player1));
                _inputManager._abilities.AddRange(GetAvailableOrders(player1, false));
                selection = _inputManager.SelectMainPhaseAction(player1);
                if (selection == 1)
                    BrowseHand(player1);
                else if (selection == 2)
                    BrowseField(player1);
                else if (selection == 3)
                {
                    if (player1.CanCallRearguard())
                    {
                        input = _inputManager.SelectRearguardToCall(player1);
                        canSelect.AddRange(player1.GetAvailableCircles(input));
                        location = _inputManager.SelectCallLocation(player1, "Select circle to call to.", player1.GetCard(input), new List<int>(), canSelect);
                        Log.WriteLine("input: " + input + " location: " + location);
                        if (_abilities.CanOverDress(input, location) && _inputManager.YesNo(player1, "Perform overDress?"))
                        {
                            Call(player1, player2, location, input, true);
                        }
                        else
                            Call(player1, player2, location, input, false);
                    }
                    else
                        Log.WriteLine("No Rearguards can be called.");
                }
                else if (selection == MainPhaseAction.CallFromPrison)
                {
                    CallFromPrison(player1, player2);
                }
                else if (selection == 5)
                {
                    if (player1.CanMoveRearguard())
                    {
                        if (player1.CanFreeSwap())
                        {
                            selections = _inputManager.MoveRearguards(player1);
                            MoveRearguard(player1, player2, selections.Item1, selections.Item2);
                        }
                        else
                        {
                            selection = _inputManager.SelectRearguardColumn(player1);
                            MoveRearguard(player1, player2, selection);
                        }
                    }
                    else
                        Log.WriteLine("No Rearguards can be moved.");
                }
                else if (selection == 6) //ACT
                {
                    ChooseACTToActivate(player1);
                }
                else if (selection == 7) //Order
                {
                    //if (player1.CanPlayOrder())
                    //{
                    //    ChooseOrderToActivate(player1, false);
                    //}
                    //else
                    //    Log.WriteLine("Already activated order this turn.");
                    ChooseOrderToActivate(player1, false);
                }
                else if (selection == 8)
                    break;
                else if (selection == MainPhaseAction.CallFromHand) //call specific RG (for use outside of console only)
                {
                    input = _inputManager.int_input2;
                    canSelect.AddRange(player1.GetAvailableCircles(input));
                    location = _inputManager.SelectCallLocation(player1, "Select circle to call to.", player1.GetCard(input), new List<int>(), canSelect);
                    if (_abilities.CanOverDress(input, location))
                    {
                        //Log.WriteLine("Perform overDress?");
                        if (_inputManager.YesNo(player1, "Perform overDress?"))
                        {
                            Call(player1, player2, location, input, true);
                            continue;
                        }
                    }
                    Call(player1, player2, location, input, false);
                }
                else if (selection == MainPhaseAction.MoveRearguard) //move specific column (for use outside of console only)
                {
                    if (player1.CanFreeSwap())
                    {
                        int selectedID = _inputManager.int_input2;
                        List<int> availableCircles = player1.GetCirclesForFreeSwap(_inputManager.int_input2);
                        if (availableCircles.Count > 0)
                        {
                            int newCircle = _inputManager.SelectCircle(player1, availableCircles, 1)[0];
                            player1.MoveRearguardFreeSwap(selectedID, newCircle);
                        }
                    }
                    else
                        player1.AlternateMoveRearguard(_inputManager.int_input2);
                }
                else if (selection == MainPhaseAction.ActivateAbility)
                {
                    List<Ability> ACTs = new List<Ability>();
                    foreach (Ability ability in GetACTAbilities(player1))
                    {
                        if (ability.GetCard().tempID == _inputManager.int_input2)
                        {
                            ACTs.Add(ability);
                        }
                    }
                    if (ACTs.Count > 1)
                    {
                        List<string> options = new List<string>();
                        foreach (Ability ability in ACTs)
                        {
                            options.Add(ability.Description);
                        }
                        int ACTselection = _inputManager.SelectOption(player1, options.ToArray());
                        ActivateACT(player1, ACTs[ACTselection - 1]);
                    }
                    else if (ACTs.Count == 1)
                        ActivateACT(player1, ACTs[0]);
                    foreach (Ability ability in GetAvailableOrders(player1, false))
                    {
                        if (ability.GetCard().tempID == _inputManager.int_input2)
                        {
                            ActivateOrder(player1, ability, true, true);
                            break;
                        }
                    }
                }
                else if (selection == MainPhaseAction.ActivateAbilityFromDrop)
                {
                    List<Ability> ACTs = new List<Ability>();
                    foreach (Ability ability in GetACTAbilities(player1))
                    {
                        if (player1.GetDrop().Exists(card => card.tempID == ability.GetID()))
                            ACTs.Add(ability);
                    }
                    int selectedAbility = _inputManager.SelectAbility(player1, ACTs);
                    if (selectedAbility != ACTs.Count)
                    {
                        ActivateACT(player1, ACTs[selectedAbility]);
                    }
                }
                else if (selection == MainPhaseAction.SoulCharge)
                {
                    player1.SoulCharge(1);
                }
                else if (selection == MainPhaseAction.CounterCharge)
                {
                    List<Card> canCC = new List<Card>();
                    foreach (Card card in player1.GetDamageZone())
                    {
                        if (!player1.IsFaceUp(card))
                            canCC.Add(card);
                    }
                    if (canCC.Count > 0)
                        CounterCharge(player1, canCC, 1);
                }
                else if (selection == MainPhaseAction.TakeDamage)
                {
                    TriggerCheck(player1, player2, false);
                }
                else if (selection == MainPhaseAction.Heal)
                {
                    Heal(player1);
                }
                PerformCheckTiming(player1, player2);
            }
        }

        public void Call(Player player1, Player player2, int location, int selection, bool overDress)
        {
            player1.Call(location, selection, overDress);
            _abilities.ResetActivation(selection);
            AddAbilityTiming(Activation.PlacedOnRC, player1._playerID, player1.GetLastPlacedOnRC());
            AddAbilityTiming(Activation.PlacedOnRCFromHand, player1._playerID, player1.GetLastPlacedOnRCFromHand());
            player1.DoneCalling();
        }

        public List<Card> SuperiorCall(Player player1, Player player2, List<Card> cardsToSelect, int max, int min, int[] circles, bool overDress, bool standing, bool free, bool differentRows)
        {
            List<int> selections;
            int selectedCircle = 0;
            List<int> selectedCircles = new List<int>();
            List<int> canSelect = new List<int>();
            int sc = 0;
            selections = _inputManager.SelectFromList(player1, cardsToSelect, max, min, "to call.");
            foreach (int tempID in selections)
            {
                if (circles != null)
                {
                    canSelect.AddRange(player1.GetTotalAvailableCircles(player1.GetCard(tempID), circles));
                }
                else
                    canSelect.AddRange(player1.GetAvailableCircles(tempID));
                foreach (int circle in selectedCircles)
                    canSelect.Remove(circle);
                if (canSelect.Count == 0)
                    continue;
                if (canSelect.Count == 1)
                    selectedCircle = canSelect[0];
                else
                    selectedCircle = _inputManager.SelectCallLocation(player1, "Choose RC.", player1.GetCard(tempID), selectedCircles, canSelect);
                selectedCircles.Add(selectedCircle);
                if (differentRows)
                {
                    foreach (int circle in player1.PlayerRCCircles())
                    {
                        if (player1.GetRowNum(circle) == player1.GetRowNum(selectedCircle))
                            selectedCircles.Add(circle);
                    }
                }
                sc = player1.SuperiorCall(selectedCircle, tempID, overDress, standing);
                if (sc == 2 && OnFree != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.playerID = player2._playerID;
                    OnFree(this, args);
                }
                _abilities.ResetActivation(tempID);
            }
            List<Card> lastPlacedOnRC = player1.GetLastPlacedOnRC();
            foreach (Card card in lastPlacedOnRC)
            {
                AddAbilityTiming(Activation.PlacedOnRC, player1._playerID, card);
            }
            //AddAbilityTiming(Activation.PlacedOnRC, player1._playerID, player1.GetLastPlacedOnRC(), player1.IsAlchemagic());
            if (sc == 1)
                AddAbilityTiming(Activation.PlacedOnRCFromHand, player1._playerID, player1.GetLastPlacedOnRCFromHand(), player1.IsAlchemagic());
            else
                AddAbilityTiming(Activation.PlacedOnRCOtherThanFromHand, player1._playerID, player1.GetLastPlacedOnRCOtherThanFromHand(), player1.IsAlchemagic());
            if (sc == 2)
                AddAbilityTiming(Activation.PlacedOnRCFromPrison, player1._playerID, player1.GetLastPlacedOnRCFromPrison(), player1.IsAlchemagic());
            player1.DoneCalling();
            return lastPlacedOnRC;
        }

        public void MoveRearguard(Player player1, Player player2, int selection)
        {
            player1.MoveRearguard(selection);
        }

        public void MoveRearguard(Player player1, Player player2, int selection, int direction)
        {
            player1.MoveRearguard(selection, direction);
        }

        public void BattlePhaseMenu(Player player1, Player player2)
        {
            int selection;
            int booster = -1;
            int attacker;
            int target;
            CardEventArgs args;
            if (OnBattlePhase != null)
            {
                args = new CardEventArgs();
                OnBattlePhase(this, args);
            }
            AddAbilityTiming(Activation.OnBattlePhase, 0);
            PerformCheckTiming(player1, player2);
            while (true)
            {
                if (player1.CanAttack())
                {
                    booster = -1;
                    selection = _inputManager.SelectBattlePhaseAction(player1);
                    if (selection == 1)
                        BrowseHand(player1);
                    else if (selection == 2)
                        BrowseField(player1);
                    else if (selection == BattlePhaseAction.End)
                        break;
                    else if (selection == 3 || selection == BattlePhaseAction.Attack) //for use outside of console only
                    {
                        if (selection == 3)
                            attacker = _inputManager.SelectAttackingUnit(player1);
                        else
                            attacker = _inputManager.int_input2;
                        player1.SetAttacker(attacker);
                        if (player1.CanBeBoosted())
                        {
                            Log.WriteLine("----------\nBoost?");
                            if (_inputManager.YesNo(player1, "Boost?"))
                            {
                                booster = player1.GetBooster(attacker);
                            }
                        }
                        if (player1.GetPotentialAttackTargets().Count == 0 && _inputManager.YesNo(player1, "No possible attack targets. Proceed with attack?"))
                        {
                            Attack(player1, player2, booster, new List<int>().ToArray());
                        }    
                        else if (player1.CanChooseThreeCirclesWhenAttacking(attacker))
                        {
                            List<int> selectedCircles;
                            List<int> availableCircles = player2.PlayerRCCircles();
                            availableCircles.Add(player1.Convert(FL.EnemyVanguard));
                            selectedCircles = _inputManager.SelectCircle(_actingPlayer, availableCircles, 3);
                            List<int> targets = new List<int>();
                            foreach (int circle in selectedCircles)
                            {
                                if (player1.GetUnitAt(circle, false) != null)
                                    targets.Add(player1.GetUnitAt(circle, false).tempID);
                            }
                            Attack(player1, player2, booster, targets.ToArray());
                        }
                        else
                        {
                            target = _inputManager.SelectUnitToAttack(player1);
                            if (target >= 0)
                                Attack(player1, player2, booster, target);
                        }
                        if (_gameOver)
                            return;
                    }
                    else
                        break;
                }
                else
                    break;
            }
        }

        public void Attack(Player player1, Player player2, int booster, params int[] targets)
        {
            int selection;
            int selection2;
            int drive;
            int critical;
            List<int> selections;
            bool attackHits = false;
            player1.InitiateAttack(booster, targets);
            if (targets.Length > 0)
            {
                //AddAbilityTiming(Activation.OnAttack, 0);
                AddAbilityTiming(Activation.OnAttack, 1, player1.GetAttacker());
                PerformCheckTiming(player1, player2);
                if (player1 == _player1)
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                else
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                //_inputManager.SwapPlayers();
                while (player1.GetAttacker() != null && player2.GetAttackedCards().Count > 0)
                {
                    _inputManager._abilities.Clear();
                    if (!player2.OrderPlayed())
                        _inputManager._abilities.AddRange(GetAvailableOrders(player2, true));
                    player2.PrintEnemyAttack();
                    if (player2.GetAttackedCards().Count > 0)
                    {
                        selection = _inputManager.SelectGuardPhaseAction(player2);
                        if (selection == 1)
                            BrowseHand(player2);
                        else if (selection == 2)
                            BrowseField(player2);
                        else if (selection == 3)
                            player2.PrintShield();
                        else if (selection == GuardStepAction.Guard)
                        {
                            if (player2.GetAttackedCards().Count > 1)
                                selection2 = _inputManager.SelectCardToGuard(player2);
                            else
                                selection2 = -1;
                            if (player2.MustGuardWithTwo())
                                selections = _inputManager.SelectFromList(player2, player2.GetGuardableCards(), player2.GetGuardableCards().Count, 2, "to guard with.");
                            else if (player2.MyStates.GetValue(PlayerState.GuardRestrict) > 1)
                                selections = _inputManager.SelectFromList(player2, player2.GetGuardableCards(), player2.GetGuardableCards().Count, player2.MyStates.GetValue(PlayerState.GuardRestrict), "to guard with.");
                            else
                                selections = _inputManager.SelectFromList(player2, player2.GetGuardableCards(), player2.GetGuardableCards().Count, 1, "to guard with.");
                            if (selections.Count == 0)
                                continue;
                            player2.Guard(selections, selection2);
                            AddAbilityTiming(Activation.PlacedOnGC, player2._playerID, player2.GetLastPlacedOnGC());
                            AddAbilityTiming(Activation.PutOnGC, player2._playerID, player2.GetLastPutOnGC());
                        }
                        else if (selection == 5)
                        {
                            if (player2.GetInterceptableCards().Count > 0)
                            {
                                selections = _inputManager.SelectFromList(player2, player2.GetInterceptableCards(), 1, 1, "Select card to intercept with.");
                                if (player2.GetAttackedCards().Count > 1)
                                    selection2 = _inputManager.SelectCardToGuard(player2);
                                else
                                    selection2 = -1;
                                player2.Guard(selections, selection2);
                                AddAbilityTiming(Activation.PutOnGC, player2._playerID, player2.GetLastPutOnGC());
                            }
                            else
                                Log.WriteLine("No interceptable cards available.");
                        }
                        else if (selection == 6)
                        {
                            //if (player2.CanPlayOrder())
                            //{
                            //    ChooseOrderToActivate(player2, true);
                            //}
                            //else
                            //    Log.WriteLine("Already activated order this turn.");
                            ChooseOrderToActivate(player2, true);
                        }
                        else if (selection == GuardStepAction.End) // end guard
                        {
                            break;
                        }
                        else if (selection == GuardStepAction.Intercept)
                        {
                            selections = new List<int>();
                            selections.Add(_inputManager.int_input2);
                            if (player2.GetAttackedCards().Count > 1)
                                selection2 = _inputManager.SelectCardToGuard(player2);
                            else
                                selection2 = -1;
                            player2.Guard(selections, selection2);
                            AddAbilityTiming(Activation.PutOnGC, player2._playerID, player2.GetLastPutOnGC());
                        }
                        else if (selection == GuardStepAction.BlitzOrder)
                        {
                            foreach (Ability ability in GetAvailableOrders(player2, true))
                            {
                                if (ability.GetCard().tempID == _inputManager.int_input2)
                                {
                                    ActivateOrder(player2, ability, true, true);
                                    break;
                                }
                            }
                        }
                        PerformCheckTiming(player1, player2);
                    }
                }
                if (player1.GetAttacker() != null && (player1.AttackerIsVanguard() || player1.RearguardCanDriveCheck() || player1.CardStates.HasState(player1.GetAttacker().tempID, CardState.CanDriveCheck)))
                {
                    if (player1 == _player1)
                        Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                    else
                        Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                    //_inputManager.SwapPlayers();
                    for (int i = 0; i < player1.Drive(); i++)
                    {
                        TriggerCheck(player1, player2, true);
                        PerformCheckTiming(player1, player2);
                    }
                    if (player1 == _player1)
                        Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                    else
                        Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                    //_inputManager.SwapPlayers();
                }
                if (player1.GetAttacker() != null && player2.GetAttackedCards().Count > 0 && player2.AttackHits())
                    attackHits = true;
                player2.RetireGC();
                PerformCheckTiming(player1, player2);
                if (attackHits)
                {
                    if (player2.VanguardHit(C.Player))
                    {
                        critical = player1.Critical();
                        for (int i = 0; i < critical; i++)
                        {
                            TriggerCheck(player2, player1, false);
                            PerformCheckTiming(player1, player2);
                        }
                        AddAbilityTiming(Activation.OnAttackHitsVanguard, 0);
                    }
                    if (player2.GetLastPlayerRCRetired().Count > 0)
                        AddAbilityTiming(Activation.OnPlayerRCRetired, player2._playerID, player2.GetLastPlayerRCRetired());
                    for (int i = 0; i < player2.NumberOfTimesHit(); i++)
                        AddAbilityTiming(Activation.OnAttackHits, 0);
                    PerformCheckTiming(player1, player2);
                    player2.RetireAttackedUnit();
                    ////_inputManager.SwapPlayers();
                }
                //_inputManager.SwapPlayers();
            }
            AddAbilityTiming(Activation.OnBattleEnds, player1._playerID, player1.GetAttacker());
            PerformCheckTiming(player1, player2);
            _abilities.EndOfBattle();
            if (player1 == _player1)
                Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
            else
                Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
            player1.EndAttack();
            player2.EndAttack();
            player1.UpdateRecordedValues();
            player2.UpdateRecordedValues();
        }

        public void TriggerCheck(Player player1, Player player2, bool drivecheck)
        {
            int check;
            int selection;
            int power = 0;
            List<Card> cards;
            if (drivecheck)
                Log.WriteLine("----------\nPerforming Drive Check.");
            else
                Log.WriteLine("----------\nPerforming Damage Check.");
            check = player1.TriggerCheck(drivecheck);
            if (drivecheck)
                power = player1.GetBonusDriveCheckPower();
            else
                power = 0;
            int times = 1;
            if (player1.MyStates.HasState(PlayerState.DoubleTriggerEffects))
                times++;
            for (int i = 0; i < times; i++)
            {
                if (check != Trigger.NotTrigger && check != Trigger.Over && check != Trigger.Front)
                {
                    Log.WriteLine("----------\nChoose unit to give +10000 power to.");
                    selection = _inputManager.SelectActiveUnit(player1, PromptType.AddPower, 10000 + power);
                    player1.AddTempPower(selection, 10000 + power, false);
                    player1.UpdateRecordedValues();
                    player2.UpdateRecordedValues();
                }
                if (check == Trigger.Critical)
                {
                    Log.WriteLine("----------\nChoose unit to give Critical to.");
                    selection = _inputManager.SelectActiveUnit(player1, PromptType.AddCritical, 1);
                    player1.AddCritical(selection, 1);
                    player1.UpdateRecordedValues();
                    player2.UpdateRecordedValues();
                }
                else if (check == Trigger.Stand) //STAND TRIGGER (no stand triggers rn, will fix later if needed)
                {
                    Log.WriteLine("----------\nChoose unit to Stand.");
                    selection = _inputManager.SelectActiveUnit(player1, PromptType.Stand, 0);
                    player1.Stand(selection);
                }
                else if (check == Trigger.Draw) //DRAW TRIGGER
                {
                    Draw(player1, player2, 1);
                }
                else if (check == Trigger.Heal) //HEAL TRIGGER
                {
                    Heal(player1);
                }
                else if (check == Trigger.Front) //FRONT TRIGGER
                {
                    cards = player1.GetPlayerFrontRow();
                    foreach (Card card in cards)
                    {
                        player1.AddTempPower(card.tempID, 10000 + power, false);
                    }
                }
            }
            if (check == Trigger.Over) //OVER TRIGGER
            {
                if (drivecheck)
                {
                    List<Card> list = new List<Card>();
                    list.Add(player1.GetTrigger(C.Player));
                    AddAbilityTiming(Activation.OnOverTrigger, player1._playerID, list);
                }
                player1.RemoveTrigger();
                Draw(player1, player2, 1);
                for (int i = 0; i < times; i++)
                {
                    Log.WriteLine("Choose unit to give 1000000000 power to.");
                    selection = _inputManager.SelectActiveUnit(player1, PromptType.AddPower, 100000000 + power);
                    player1.AddTempPower(selection, 100000000 + power, false);
                }
                player1.UpdateRecordedValues();
                player2.UpdateRecordedValues();
                if (drivecheck)
                    PerformCheckTiming(player1, player2);
            }
            if (drivecheck)
            {
                List<Card> list = new List<Card>();
                if (player1.GetTrigger(true) != null)
                    list.Add(player1.GetTrigger(true));
                AddAbilityTiming(Activation.OnDriveCheck, player1._playerID, list);
                PerformCheckTiming(player1, player2);
                player1.AddTriggerToHand();
            }
            else
            {
                player1.TakeDamage();
            }
        }

        public void PerfectGuard(Player player1, Player player2)
        {
            List<Card> units = player1.GetActiveUnits();
            int target = _inputManager.SelectFromList(player1, units, 1, 1, "Choose unit to give Perfect Guard.")[0];
            player1.PerfectGuard(target);
        }

        public void Resist(Player player1, Player player2, int tempID, int abilityID)
        {
            _player1.Resist(tempID, abilityID);
            _player2.Resist(tempID, abilityID);
        }

        public void AddAbilitiesToQueue(Player player1)
        {
            List<Card> cards = new List<Card>();
            List<Ability> abilities = new List<Ability>();
            AbilityTiming abilityTiming;
            List<AbilityTimingCount> abilityQueue = null;
            if (_player1 == player1)
                abilityQueue = _player1AbilityQueue;
            else if (_player2 == player1)
                abilityQueue = _player2AbilityQueue;
            foreach (int activation in _abilityTimings.GetActivations())
            {
                abilityTiming = _abilityTimings.GetAbilityTiming(activation, player1._playerID);
                if (abilityTiming == null)
                    continue;
                foreach (int key in abilityTiming.GetTimingCounts().Keys)
                {
                    for (int i = 1; i <= abilityTiming.GetTimingCounts()[key]; i++)
                    {
                        cards.Clear();
                        cards.Add(player1.GetTrigger(C.Player));
                        abilities.AddRange(_abilities.GetAbilities(activation, cards, new Tuple<int, int>(key, i)));
                        abilities.AddRange(_abilities.GetAbilities(activation, player1.GetActiveUnits(), new Tuple<int, int>(key, i)));
                        abilities.AddRange(_abilities.GetAbilities(activation, player1.GetHand(), new Tuple<int, int>(key, i)));
                        abilities.AddRange(_abilities.GetAbilities(activation, player1.GetDrop(), new Tuple<int, int>(key, i)));
                        abilities.AddRange(_abilities.GetAbilities(activation, player1.GetSoul(), new Tuple<int, int>(key, i)));
                        abilities.AddRange(_abilities.GetAbilities(activation, player1.GetOverloadedCircles(), new Tuple<int, int>(key, i)));
                        cards.Clear();
                        foreach (Card card in player1.GetGC())
                        {
                            if (card.originalOwner == player1._playerID)
                                cards.Add(card);
                        }
                        abilities.AddRange(_abilities.GetAbilities(activation, cards, new Tuple<int, int>(key, i)));
                        abilities.AddRange(_abilities.GetAbilities(activation, player1.GetPlayerOrder(), new Tuple<int, int>(key, i)));
                        abilities.AddRange(_abilities.GetAbilities(activation, player1.GetBind(), new Tuple<int, int>(key, i)));
                        abilities.AddRange(_abilities.GetAbilities(activation, player1.GetRemoved(), new Tuple<int, int>(key, i)));
                        abilities.AddRange(_abilities.GetAbilities(activation, player1.GetArms(player1.Vanguard().tempID), new Tuple<int, int>(key, i)));
                        foreach (AbilityTimingCount ability in abilityTiming.GetActivatedAbilities(key))
                        {
                            if (ability.timingCount.Item1 == key && ability.timingCount.Item2 == i && abilities.Contains(ability.ability))
                                abilities.Remove(ability.ability);
                        }
                        foreach (AbilityTimingCount ability in _skippedAbilities)
                        {
                            if (abilities.Contains(ability.ability) && ability.timingCount.Item1 == key)
                                abilities.Remove(ability.ability);
                        }
                        foreach (Ability ability in abilities)
                        {
                            AbilityTimingCount newAbility = new AbilityTimingCount(ability, activation, new Tuple<int, int>(key, i));
                            if (!abilityQueue.Exists(a => a.ability == newAbility.ability && a.timingCount.Item1 == newAbility.timingCount.Item1 && a.timingCount.Item2 == newAbility.timingCount.Item2))
                                abilityQueue.Add(newAbility);
                        }
                        abilities.Clear();
                    }
                }
            }
        }

        public void ActivateContAbilities(Player player1, Player player2)
        {
            List<Ability> abilities = new List<Ability>();
            Ability givenAbility;
            player1.RefreshContinuous();
            player2.RefreshContinuous();
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetOrderZone(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetActiveUnits(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetGC(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetDrop(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetDeck(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetHand(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetSoul(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetOrderZone(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetActiveUnits(), null));
            //abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetGC(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetDrop(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetDeck(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetHand(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetSoul(), null));
            foreach (Card card in player1.GetAllUnitsOnField())
            {
                if (player1.GetGivenAbility(card.tempID) != null)
                {
                    foreach (Tuple<int, int> tuple in player1.GetGivenAbility(card.tempID))
                    {
                        givenAbility = _abilities.GetAbility(tuple.Item1, tuple.Item2);
                        if (givenAbility.GetActivations.Contains(Activation.Cont))
                        {
                            givenAbility.CheckConditionAsGiven(card, Activation.Cont, null);
                            givenAbility.ActivateAsGiven(card);
                        }
                    }
                }
            }
            for (int i = 0; i < 2; i++)
            {
                foreach (Ability ability in abilities)
                {
                    if (ability.CanActivate())
                        ability.Activate(Activation.Cont, null);
                }
            }
            player1.UpdateRecordedValues();
            player2.UpdateRecordedValues();
        }

        public List<Ability> GetAvailableOrders(Player player, bool blitz)
        {
            List<Ability> abilities = new List<Ability>();
            if (blitz)
                abilities = _abilities.GetAbilities(Activation.OnBlitzOrder, player.GetOrderableCards(), null);
            else
                abilities = _abilities.GetAbilities(Activation.OnOrder, player.GetOrderableCards(), null);
            List<Ability> available = new List<Ability>();
            if (!player.CanPlayOrder())
            {
                if (player.MyStates.GetValue(PlayerState.AdditionalArms) >= 1)
                {
                    while (abilities.Exists(ability => !OrderType.IsArms(ability.GetCard().orderType)))
                        abilities.Remove(abilities.Find(ability => !OrderType.IsArms(ability.GetCard().orderType)));
                }
                else
                    abilities.Clear();
            }
            foreach (Ability ability in abilities)
            {
                if (ability.CanPayCost())
                    available.Add(ability);
            }
            return available;
        }

        public List<Card> GetAvailableOrders(Player player, List<Card> cards)
        {
            List<Ability> abilites = _abilities.GetAbilities(Activation.OnOrder, cards, null);
            List<Card> orders = new List<Card>();
            foreach (Ability ability in abilites)
            {
                if (!orders.Exists(card => card.tempID == ability.GetCard().tempID))
                    orders.Add(ability.GetCard());
            }
            return orders;
        }

        public void ChooseOrderToActivate(Player player1, bool blitz)
        {
            List<Ability> abilities = GetAvailableOrders(player1, blitz);
            int selection;
            if (abilities.Count == 0)
                return;
            selection = _inputManager.SelectAbility(player1, abilities);
            if (selection == abilities.Count)
                return;
            else
                ActivateOrder(player1, abilities[selection], true, true);
        }

        public void ActivateOrder(Player player, Card card)
        {
            List<Card> cards = new List<Card>();
            cards.Add(card);
            List<Ability> abilities = _abilities.GetAbilities(Activation.OnOrder, cards, null);
            if (abilities.Count > 0)
                ActivateOrder(player, abilities[0], false, false);
        }

        public void ActivateOrder(Player player1, Ability ability, bool isPlayTiming, bool payCost)
        {
            int amSelection = -1;
            bool proceedWithAlchemagic = false;
            //_currentAbility = ability;
            if (isPlayTiming && ability.GetCard().orderType == OrderType.Normal && (player1.CanAlchemagicSame() || player1.CanAlchemagicDiff()))
            {
                _alchemagicQueue.AddRange(_abilities.GetAlchemagicableCards(player1, ability.GetID()));
                if (_alchemagicQueue.Count >= 1 && (!ability.CheckConditionWithoutAlchemagic() || _inputManager.YesNo(player1, "Use Alchemagic?")))
                {
                    proceedWithAlchemagic = true;
                }
            }
            if (proceedWithAlchemagic)
            {
                player1.EnterAlchemagic();
                amSelection = _inputManager.SelectAbility(player1, _alchemagicQueue);
                Log.WriteLine("----------\n" + ability.Name + "'s effect activates!");
                PlayOrder(player1, ability.GetID(), false);
                PlayOrder(player1, _alchemagicQueue[amSelection].GetID(), true);
                List<Card> list = new List<Card>();
                list.Add(ability.GetCard());
                AddAbilityTiming(Activation.OnOrderPlayed, player1._playerID, list);
                CardEventArgs args;
                if (OnAlchemagic != null)
                {
                    args = new CardEventArgs();
                    args.message = ability.GetCard().name + " alchemagics with " + _alchemagicQueue[amSelection].Name + "!";
                    OnAlchemagic(this, args);
                }
                ability.PayCost();
                //_currentAbility = _alchemagicQueue[amSelection];
                _alchemagicQueue[amSelection].PayCost();
                ability.Activate(Activation.OnOrder, null);
                _alchemagicQueue[amSelection].ActivateAsGiven(player1.GetCard(_alchemagicQueue[amSelection].GetID()));
                player1.EndAlchemagic();
                player1.EndOrder();
            }
            else
            {
                Log.WriteLine("----------\n" + ability.Name + "'s effect activates!");
                PlayOrder(player1, ability.GetID(), false);
                List<Card> list = new List<Card>();
                list.Add(ability.GetCard());
                AddAbilityTiming(Activation.OnOrderPlayed, player1._playerID, list);
                if (payCost)
                    ability.PayCost();
                ability.Activate(Activation.OnOrder, null);
                player1.EndOrder();
            }
        }

        public List<Ability> GetACTAbilities(Player player)
        {
            List<Ability> abilities = new List<Ability>();
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetActiveUnits(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetSoul(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetDrop(), null));
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetOrderZone(), null));
            //List<Ability> canActivate = new List<Ability>();
            //foreach (Ability ability in abilities)
            //{
            //    if (ability.CanPayCost())
            //        canActivate.Add(ability);
            //}
            //return canActivate;
            return abilities;
        }

        public void ChooseACTToActivate(Player player)
        {
            List<Ability> abilities = GetACTAbilities(player);
            int selection = _inputManager.SelectAbility(player, abilities);
            if (selection == abilities.Count)
                return;
            else
                ActivateACT(player, abilities[selection]);
        }

        public void ActivateACT(Player player1, Ability ability)
        {
            Log.WriteLine("----------\n" + ability.Name + "'s effect activates!");
            _currentAbility = ability;
            if (OnAbilityActivated != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.card = ability.GetCard();
                OnAbilityActivated(this, args);
            }
            ability.PayCost();
            ability.Activate(Activation.OnACT, null);
            player1.UpdateRecordedValues();
            _currentAbility = null;
        }
        
        public void CallFromPrison(Player player1, Player player2)
        {
            List<Card> cards = new List<Card>();
            cards.AddRange(player2.GetPlayerPrisoners());
            //List<Ability> abilities = _abilities.GetAbilities(Activation.OnCallFromPrison, cards, null);
            //if (abilities.Count == 0)
            //    return;
            //abilities[0].PayCost();
            //abilities[0].Activate(abilities[0].GetActivations[0], null);
            if (cards.Count > 0)
            {
                int selection = 0;
                if (player1.CanCB(1) && player1.CanSB(1))
                    selection = _inputManager.SelectOption(player1, "Counter Blast 1", "Soul Blast 1");
                else if (player1.CanCB(1))
                    selection = 1;
                else if (player1.CanSB(1))
                    selection = 2;
                if (selection == 1)
                {
                    CounterBlast(player1, player2, 1, 1);
                    SuperiorCall(player1, player2, cards, 2, 0, null, false, true, true, false);
                }
                else if (selection == 2)
                {
                    SoulBlast(player1, player2, player1.GetSoul(), 1);
                    SuperiorCall(player1, player2, cards, 1, 0, null, false, true, true, false);
                }
            }
        }

        public int ActivateNoPromptAbilities(Player player)
        {
            List<AbilityTimingCount> abilities;
            if (player == _player1)
                abilities = _player1AbilityQueue;
            else
                abilities = _player2AbilityQueue;
            List<Ability> tempQueue = new List<Ability>();
            List<AbilityTimingCount> availableAbilities = NoPromptAbilitiesInQueue(abilities);
            List<AbilityTimingCount> canActivate = new List<AbilityTimingCount>();
            foreach (AbilityTimingCount ability in availableAbilities)
            {
                if (ability.ability.CanActivate() && ability.ability.CanPayCost())
                    canActivate.Add(ability);
            }
            if (canActivate.Count > 0)
            {
                AbilityTimingCount ability = canActivate[0];
                if (OnAbilityActivated != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.card = ability.ability.GetCard();
                    OnAbilityActivated(this, args);
                }
                ability.ability.Activate(ability.activation, ability.timingCount);
                _player1.UpdateRecordedValues();
                _player2.UpdateRecordedValues();
                _abilityTimings.AddActivatedAbility(ability.ability, ability.activation, ability.timingCount);
                abilities.Remove(ability);
                return 1;
            }
            return 0;
        }

        public int ActivateAbilities(Player player)
        {
            int selection;
            int abilityActivated = 0;
            List<AbilityTimingCount> abilities;
            if (player == _player1)
                abilities = _player1AbilityQueue;
            else
                abilities = _player2AbilityQueue;
            List<Ability> tempQueue = new List<Ability>();
            int ThenNum = 0;
            if (abilities.Count == 0)
                return abilityActivated;
            List<AbilityTimingCount> canActivate = new List<AbilityTimingCount>();
            foreach (AbilityTimingCount ability in abilities)
            {
                if (ability.ability.CanActivate() && ability.ability.CanPayCost())
                    canActivate.Add(ability);
            }
            if (canActivate.Count == 0)
                return abilityActivated;
            if (canActivate.Count == 1 && canActivate[0].ability.isMandatory)
                selection = 0;
            else
                selection = _inputManager.SelectAbility(player, canActivate);
            if (selection == canActivate.Count)
            {
                foreach (AbilityTimingCount ability in canActivate)
                    _skippedAbilities.Add(ability);
            }
            else
            {
                _currentAbility = canActivate[selection].ability;
                Log.WriteLine("----------\n" + canActivate[selection].ability.Name + "'s effect activates!");
                if (OnAbilityActivated != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.card = canActivate[selection].ability.GetCard();
                    OnAbilityActivated(this, args);
                }
                canActivate[selection].ability.PayCost();
                _currentAbility = canActivate[selection].ability;
                ThenNum = canActivate[selection].ability.Activate(canActivate[selection].activation, canActivate[selection].timingCount);
                abilityActivated++;
                _player1.UpdateRecordedValues();
                _player2.UpdateRecordedValues();
                _abilityTimings.AddActivatedAbility(canActivate[selection].ability, canActivate[selection].activation, canActivate[selection].timingCount);
                abilities.Remove(canActivate[selection]);
                //ThenID = abilities[selection].Item1.GetID();
                //abilities.Clear();
                //if (ThenNum > 0)
                //{
                //    ThenAbility = _abilities.GetAbility(ThenID, ThenNum);
                //    if (!ThenAbility.CheckCondition(Activation.Then))
                //        continue;
                //    if (ThenAbility.isMandatory)
                //        ThenAbility.Activate();
                //    else
                //    {
                //        //Log.WriteLine("Activate ability?");
                //        if (_inputManager.YesNo(choosingPlayer, "Activate ability?"))
                //            ThenAbility.Activate();
                //    }
                //}
                _currentAbility = null;
            }
            return abilityActivated;
        }

        public void MarkedForRetire(object sender, CardEventArgs e)
        {
            if (e.playerID == 1 && e.card.OriginalGrade() % 2 == 0 && _player1.MyStates.HasState(PlayerState.RetiredEvenGradeUnitsCanBeAddedToSoul) && _inputManager.YesNo(_player1, "Add " + e.card.name + " to soul?"))
                _player1.FinalizeRetire(e.card.tempID, true);
            else if (e.playerID == 2 && e.card.OriginalGrade() % 2 == 0 && _player1.MyStates.HasState(PlayerState.RetiredEvenGradeUnitsCanBeAddedToSoul) && _inputManager.YesNo(_player2, "Add " + e.card.name + " to soul?"))
                _player2.FinalizeRetire(e.card.tempID, true);
            else
                _player1.FinalizeRetire(e.card.tempID, false);
        }

        public void AllAbilitiesResolved()
        {
            _player1AbilityQueue.Clear();
            _player2AbilityQueue.Clear();
            _chosen.Clear();
            _abilityTimings.Reset();
            _skippedAbilities.Clear();
            _player1.AllAbilitiesResolved();
            _player2.AllAbilitiesResolved();
        }

        public List<AbilityTimingCount> NoPromptAbilitiesInQueue(List<AbilityTimingCount> abilities)
        {
            List<AbilityTimingCount> noPromptAbilities = new List<AbilityTimingCount>();
            foreach (AbilityTimingCount ability in abilities)
            {
                if (!ability.ability.hasPrompt)
                    noPromptAbilities.Add(ability);
            }
            return noPromptAbilities;
        }

        public bool CanOverDress(int tempID, int circle)
        {
            if (_abilities.CanOverDress(tempID, circle))
                return true;
            return false;
        }

        public void CounterBlast(Player player1, Player player2, int count, int min)
        {
            List<Card> canCB = new List<Card>();
            if (player1.MyStates.GetValue(PlayerState.FreeCB) > 0)
            {
                count--;
                player1.MyStates.IncrementUntilEndOfTurnValue(PlayerState.FreeCB, -1);
            }
            foreach (Card card in player1.GetDamageZone())
            {
                if (player1.IsFaceUp(card))
                    canCB.Add(card);
            }
            CounterBlast(player1, player2, canCB, count, min);
        }

        public void CounterBlast(Player player1, Player player2, List<Card> canCB, int count, int min)
        {
            int originalCount = count;
            if (player1.IsAlchemagic())
            {
                count -= player1.AlchemagicFreeCBAvailable();
                min -= player1.AlchemagicFreeCBAvailable();
                if (count < 0)
                    count = 0;
                if (min < 0)
                    min = 0;
            }
            List<int> cardsToCB = _inputManager.SelectFromList(player1, canCB, count, min, "to Counter Blast.");
            player1.AddAlchemagicFreeCB(-1 * (originalCount - cardsToCB.Count));
            player1.CounterBlast(cardsToCB);
        }

        public void SoulBlast(Player player1, Player player2, List<Card> canSB, int count)
        {
            if (player1.IsAlchemagic() && (player1.AlchemagicFreeSBAvailable() || player1.AlchemagicFreeSBActive()))
            {
                if (!player1.CanSB(count) || player1.AlchemagicFreeSBActive() || _inputManager.YesNo(player1, "Skip Soul Blast cost?"))
                {
                    player1.UsedAlchemagicFreeSB();
                    return;
                }
            }
            List<int> cardsToSB = _inputManager.SelectFromList(player1, canSB, count, count, "to Soul Blast.");
            player1.SoulBlast(cardsToSB);
            foreach (int tempID in cardsToSB)
            {
                Card card = _player1.GetCard(tempID);
                Snapshot abilitySnapshot = null;
                if (_currentAbility != null)
                    abilitySnapshot = player1.GetSnapshot(_currentAbility.GetCard().tempID);
                Snapshot snapshot = new Snapshot(tempID, -1, -1, -1, card.name, -1, card.id, -1, abilitySnapshot);
                if (!actionLogs.ContainsKey(Location.SoulBlasted))
                    actionLogs[Location.SoulBlasted] = new List<ActionLog>();
                ActionLog actionLog = new ActionLog(player1._playerID, snapshot);
                actionLogs[Location.SoulBlasted].Add(actionLog);
            }
        }

        public void CounterCharge(Player player1, List<Card> canCC, int count)
        {
            List<int> cardsToCharge = _inputManager.SelectFromList(player1, canCC, count, count, "to flip up.");
            player1.CounterCharge(cardsToCharge);
        }

        public void SoulCharge(Player player1, Player player2, int count)
        {
            player1.SoulCharge(count);
        }

        public List<int> Search(Player player1, Player player2, List<Card> canSearch, int max, int min)
        {
            List<int> cardsToSearch = _inputManager.SelectFromList(player1, canSearch, max, min, "to search.");
            player1.Search(cardsToSearch);
            return cardsToSearch;
        }

        public List<int> Search(Player player, List<Card> cards, int max, int min)
        {
            List<int> cardsToSearch = _inputManager.SelectFromList(_player1, cards, max, min, "to search.");
            _player1.Search(cardsToSearch);
            return cardsToSearch;
        }

        public List<Card> Stand(Player player1, Player player2, List<Card> canStand, int count, int min, bool select)
        {
            List<int> cardsToStand = _inputManager.SelectFromList(player1, canStand, count, min, "card(s) to stand.");
            player1.Stand(cardsToStand);
            return player1.GetLastStood();
        }

        public void Stand(Player player1, Player player2, List<int> toStand)
        {
            player1.Stand(toStand);
        }

        public List<int> Rest(Player player1, Player player2, List<Card> canRest, int count, bool select)
        {
            List<int> cardsToRest = _inputManager.SelectFromList(player1, canRest, count, count, "to rest.");
            return player1.Rest(cardsToRest);
        }

        public void ChooseAddTempPower(Player player1, Player player2, List<Card> canAdd, int power, int count)
        {
            List<int> cardsToAddPower = _inputManager.SelectFromList(player1, canAdd, count, count, "to give +" + power + " to.");
            player1.AddTempPower(cardsToAddPower, power);
            AddToChosen(cardsToAddPower);
        }

        public void ChooseAddBattleOnlyPower(Player player1, Player player2, List<Card> canAdd, int power, int count)
        {
            List<int> cardsToAddPower = _inputManager.SelectFromList(player1, canAdd, count, count, "to give +" + power + " to.");
            foreach (int tempID in cardsToAddPower)
                player1.AddTempPower(tempID, power, true);
            AddToChosen(cardsToAddPower);
        }

        public void ChooseAddCritical(Player player1, Player player2, List<Card> canAdd, int critical, int count)
        {
            List<int> cardsToAddCritical = _inputManager.SelectFromList(player1, canAdd, count, count, "to give +" + critical + " critical to.");
            foreach (int tempID in cardsToAddCritical)
            {
                player1.AddCritical(tempID, critical);
            }
            AddToChosen(cardsToAddCritical);
        }

        public void ChooseAddDrive(Player player1, Player player2, List<Card> canAdd, int drive, int count)
        {
            List<int> cardsToAddDrive = _inputManager.SelectFromList(player1, canAdd, count, count, "to give +" + drive + " drive to.");
            foreach (int tempID in cardsToAddDrive)
            {
                player1.AddDrive(tempID, drive);
            }
            AddToChosen(cardsToAddDrive);
        }

        public void ChooseAddBattleOnlyCritical(Player player1, List<Card> canAdd, int critical, int count, int min)
        {
            List<int> cardsToAddCritical = _inputManager.SelectFromList(player1, canAdd, count, min, "to give +" + critical + " critical to.");
            foreach (int tempID in cardsToAddCritical)
            {
                player1.AddBattleOnlyCritical(tempID, critical);
            }
            AddToChosen(cardsToAddCritical);
        }

        public void AddSkill(Player player1, Player player2, Card card, int skill, int abilityID)
        {
            player1.AddSkill(card.tempID, skill, abilityID);
            player2.AddSkill(card.tempID, skill, abilityID);
        }

        public List<int> AddToHand(Player player1, Player player2, List<Card> canAddToHand, int count, int min)
        {
            List<int> cardsToAddToHand = _inputManager.SelectFromList(player1, canAddToHand, count, min, "to add to hand.");
            player1.AddToHand(cardsToAddToHand);
            return cardsToAddToHand;
        }

        public List<int> AddToSoul(Player player1, Player player2, List<Card> canAddToHand, int count, int min)
        {
            List<int> cardsToAddToSoul = _inputManager.SelectFromList(player1, canAddToHand, count, min, "to add to soul.");
            player1.AddToSoul(cardsToAddToSoul);
            return cardsToAddToSoul;
        }

        public void SelectCardToRetire(Player player1, Player player2, List<Card> canRetire, int count, int min)
        {
            List<int> cardsToRetire = _inputManager.SelectFromList(player1, canRetire, count, min, "to retire.");
            player1.Retire(cardsToRetire);
            if (player1.GetLastPlayerRCRetired().Count > 0)
                AddAbilityTiming(Activation.OnPlayerRCRetired, player1._playerID, player1.GetLastPlayerRCRetired());
            if (player1.PlayerRetired() && _currentAbility != null && _currentAbility.GetPlayer1() == player1 && _currentAbility.IsPayingCost())
            {
                AddAbilityTiming(Activation.OnRetiredForPlayerCost, player1._playerID, player1.GetLastPlayerRetired());
            }
            if (player1.EnemyRetired())
                AddAbilityTiming(Activation.OnEnemyRetired, 0);
            AddToChosen(cardsToRetire);
        }

        public void Retire(Player player1, Player player2, List<int> toRetire)
        {
            player1.Retire(toRetire);
            if (player1.EnemyRetired())
                AddAbilityTiming(Activation.OnEnemyRetired, 0);
        }

        public void AddToDrop(int tempID)
        {
            _player1.AddToDrop(tempID);
        }

        public List<int> ChooseSendToBottom(Player player1, Player player2, List<Card> canSend, int max, int min, bool cost)
        {
            List<int> cardsToSend = _inputManager.SelectFromList(player1, canSend, max, min, "to send to bottom of deck.");
            AddToChosen(cardsToSend);
            player1.SendToDeck(cardsToSend, true);
            return cardsToSend;
        }

        public void ChooseSendToTop(Player player1, Player player2, List<Card> canSend, int max, int min, bool cost)
        {
            List<int> cardsToSend = _inputManager.SelectFromList(player1, canSend, max, min, "to send to top of deck.");
            if (cost)
                player1.Reveal(cardsToSend);
            player1.SendToDeck(cardsToSend, false);
            AddToChosen(cardsToSend);
        }

        public void FinalRush(Player player1, Player player2)
        {
            player1.FinalRush();
            Log.WriteLine("----------\nEntered Final Rush!");
        }

        public void PlayOrder(Player player1, int tempID, bool alchemagic)
        {
            int result = player1.PlayOrder(tempID, alchemagic);
            if (result == 1)
            {
                AddAbilityTiming(Activation.PutOnOrderZone, player1._playerID, player1.GetLastPutOnOrderZone());
            }
            Card card = _player1.GetCard(tempID);
            Snapshot snapshot = new Snapshot(tempID, -1, -1, -1, card.name, -1, card.id, -1, null);
            if (!actionLogs.ContainsKey(Location.PlayedOrdersThisTurn))
                actionLogs[Location.PlayedOrdersThisTurn] = new List<ActionLog>();
            ActionLog actionLog = new ActionLog(player1._playerID, snapshot);
            actionLogs[Location.PlayedOrdersThisTurn].Add(actionLog);
        }

        public List<int> ChooseReveal(Player player1, Player player2, List<Card> canReveal, int max, int min)
        {
            List<int> cardsToReveal = _inputManager.SelectFromList(player1, canReveal, max, min, "to reveal.");
            player1.Reveal(cardsToReveal);
            return cardsToReveal;
        }

        public void RevealFromDeck(Player player1, Player player2, int count)
        {
            player1.RevealFromDeck(count);
        }

        public void EndReveal(Player player1, Player player2)
        {
            player1.EndReveal();
        }

        public List<int> ChooseImprison(Player player1, Player player2, List<Card> cardsToSelect, int count, int min)
        {
            List<int> cardsToImprison = _inputManager.SelectFromList(player1, cardsToSelect, count, min, "to imprison.");
            player1.Imprison(cardsToImprison);
            AddToChosen(cardsToImprison);
            return cardsToImprison;
        }

        public List<int> EnemyChooseImprison(Player player1, Player player2, List<Card> cardsToSelect, int count, int min)
        {
            List<int> cardsToImprison = _inputManager.SelectFromList(player2, cardsToSelect, count, min, "to imprison.");
            player1.Imprison(cardsToImprison);
            return cardsToImprison;
        }

        public void ChooseMoveEnemyRearguard(Player player1, List<Card> cardsToSelect, List<int> availableCircles)
        {
            int selection = _inputManager.SelectFromList(player1, cardsToSelect, 1, 1, "to switch places.")[0];
            int selection2 = _inputManager.SelectCircle(player1, availableCircles, 1)[0];
            player1.MoveRearguardSpecific(selection, selection2);
        }

        public void ChooseMoveRearguard(Player player, List<Card> cardsToSelect, List<int> availableCircles)
        {
            int selection = _inputManager.SelectFromList(player, cardsToSelect, 1, 1, "to switch places.")[0];
            int selection2 = _inputManager.SelectCallLocation(player, "Choose circle to move to.", player.GetCard(selection), new List<int>(), player.GetTotalAvailableCircles(player.GetCard(selection), availableCircles.ToArray()));
            player.MoveRearguardSpecific(selection, selection2);
        }

        public void Heal(Player player1)
        {
            int selection;
            if (player1.CanHeal())
            {
                selection = _inputManager.SelectFromList(player1, player1.GetDamageZone(), 1, 1, "to heal.")[0];
                player1.Heal(selection);
            }
        }

        public List<int> SelectCards(Player player1, List<Card> cardsToSelect, int max, int min, string query)
        {
            return SelectCards(player1, cardsToSelect, max, min, query, false);
        }

        public List<int> SelectCards(Player player1, List<Card> cardsToSelect, int max, int min, string query, bool sameName)
        {
            List<int> selectedCards = _inputManager.SelectFromList(player1, cardsToSelect, max, min, query, sameName);
            AddToChosen(selectedCards);
            return selectedCards;
        }

        public int SelectOption(Player player, string[] list)
        {
            return _inputManager.SelectOption(player, list);
        }

        public int SelectColumn(Player player)
        {
            return _inputManager.SelectColumn(player);
        }

        public bool YesNo(Player player, string query)
        {
            return _inputManager.YesNo(player, query);
        }

        public void RearrangeOnTop(Player player1, List<Card> cardsToRearrange)
        {
            List<int> newOrder = _inputManager.ChooseOrder(player1, cardsToRearrange);
            player1.RearrangeOnTop(newOrder);
        }

        public void RearrangeOnBottom(Player player1, List<Card> cardsToRearrange)
        {
            List<int> newOrder = _inputManager.ChooseOrder(player1, cardsToRearrange);
            player1.RearrangeOnBottom(newOrder);
        }

        public void ChooseBind(Player player1, List<Card> cards, int max, int min)
        {
            List<int> toBind = _inputManager.SelectFromList(player1, cards, max, min, "to bind.");
            player1.Bind(toBind);
            AddAbilityTiming(Activation.OnBind, player1._playerID, player1.ConvertToCards(toBind));
        }

        public void Bind(Player player, List<int> tempIDs)
        {
            player.Bind(tempIDs);
            AddAbilityTiming(Activation.OnBind, player._playerID, player.ConvertToCards(tempIDs));
        }

        public void DisplayCards(Player player1, List<Card> cardsToDisplay)
        {
            _inputManager.DisplayCards(player1, cardsToDisplay);
        }

        public bool PlayerMainPhase(int playerID)
        {
            if (_phase == Phase.Main && playerID == _actingPlayer._playerID)
                return true;
            return false;
        }

        public void AddToChosen(List<int> targets)
        {
            if (_currentAbility == null)
                return;
            int source = _currentAbility.GetID();
            if (!_chosen.ContainsKey(source))
                _chosen[source] = new List<int>();
            foreach (int target in targets)
            {
                if (!_chosen[source].Contains(target))
                    _chosen[source].Add(target);
            }
            AddAbilityTiming(Activation.OnChosen, 0);
        }

        public bool AlchemagicableCardsAvailable(Player player, int tempID)
        {
            List<Ability> alchemagicable = _abilities.GetAlchemagicableCards(player, tempID);
            if (alchemagicable.Count > 0)
                return true;
            return false;
        }

        public bool Activated(int tempID, int activationNumber)
        {
            Ability ability = _abilities.GetAbility(tempID, activationNumber);
            if (ability.Activated())
                return true;
            return false;
        }

        public List<AbilityTimingData> GetAbilityTimingData(int abilityTiming, int timingCount, int playerID)
        {
            return _abilityTimings.GetAbilityTimingData(abilityTiming, timingCount, playerID);
        }

        public List<AbilityTimingData> GetAbilityTimingData(int abilityTiming)
        {
            return _abilityTimings.GetAbilityTimingData(abilityTiming);
        }

        public AbilityTimingData GetAbilityTimingData(int abilityTiming, Tuple<int, int> timingCount, int playerID)
        {
            List<AbilityTimingData> list = _abilityTimings.GetAbilityTimingData(abilityTiming, timingCount.Item1, playerID);
            if (list.Count > 0 && list.Count >= timingCount.Item2)
                return list[timingCount.Item2 - 1];
            return null;
        }

        public void Sing(Player player, List<Card> cardsToSing, int count)
        {
            List<int> selectedCards = _inputManager.SelectFromList(player, cardsToSing, count, count, "to sing.");
            if (selectedCards.Count == 0)
                return;
            List<Card> songs = new List<Card>();
            foreach (int tempID in selectedCards)
                songs.Add(player.GetCard(tempID));
            List<Ability> abilities = _abilities.GetAbilities(Activation.OnSing, songs, null);
            //int index = _inputManager.SelectAbility(player, abilities);
            Ability ability = abilities[0];
            Log.WriteLine("----------\n" + ability.Name + "'s effect activates!");
            //_currentAbility = ability;
            if (OnAbilityActivated != null)
            {
                CardEventArgs args = new CardEventArgs();
                args.card = ability.GetCard();
                OnAbilityActivated(this, args);
            }
            ability.PayCost();
            ability.Activate(Activation.OnSing, null);
            player.Flip(selectedCards, false);
            player.MyStates.AddUntilEndOfTurnState(PlayerState.VanguardHasSungSongThisTurn);
            Snapshot snapshot = new Snapshot(ability.GetCard().tempID, -1, -1, -1, ability.GetCard().name, -1, ability.GetCard().id, -1, null);
            if (!actionLogs.ContainsKey(Location.SungThisTurn))
                actionLogs[Location.SungThisTurn] = new List<ActionLog>();
            ActionLog actionLog = new ActionLog(player._playerID, snapshot);
            actionLogs[Location.SungThisTurn].Add(actionLog);
        }

        public void ChooseFlip(Player actingPlayer, List<Card> cardsToFlip, int max, int min, bool faceup)
        {
            List<int> selectedCards;
            if (faceup)
                selectedCards = _inputManager.SelectFromList(actingPlayer, cardsToFlip, max, min, "to flip up.");
            else
                selectedCards = _inputManager.SelectFromList(actingPlayer, cardsToFlip, max, min, "to flip down.");
            actingPlayer.Flip(selectedCards, faceup);
        }

        public void PutIntoOrderZone(Player player)
        {
            AddAbilityTiming(Activation.PutOnOrderZone, player._playerID, player.GetLastPutOnOrderZone());
        }

        public int GetPhase()
        {
            return _phase;
        }

        public void OnAbilityTiming(object sender, CardEventArgs e)
        {
            Player player = sender as Player;
            AddAbilityTiming(e.i, e.playerID, e.cardList);
        }

        public void AddAbilityTiming(int activation, int playerID)
        {
            List<Card> cards = new List<Card>();
            AddAbilityTiming(activation, playerID, cards);
        }

        public void AddAbilityTiming(int activation, int playerID, Card card)
        {
            List<Card> cards = new List<Card>();
            if (card != null)
                cards.Add(card);
            AddAbilityTiming(activation, playerID, cards);
        }

        public void AddAbilityTiming(int activation, int playerID, List<Card> cards)
        {
            AddAbilityTiming(activation, playerID, cards, false);
        }

        public void AddAbilityTiming(int activation, int playerID, List<Card> cards, bool append)
        {
            Player player;
            AbilityTimingData abilityTimingData = new AbilityTimingData();
            Snapshot[] snapShots = new Snapshot[200];
            abilityTimingData.playerID = playerID;
            if (playerID == 1)
                player = _player1;
            else
                player = _player2;
            for (int i = 0;  i < 200; i++)
            {
                snapShots[i] = _player1.GetSnapshot(i);
            }
            abilityTimingData.allSnapshots = snapShots;
            List<Snapshot> relevantSnapshots = new List<Snapshot>();
            if (cards != null && cards.Count > 0)
            {
                relevantSnapshots.Add(snapShots[cards[0].tempID]);
                abilityTimingData.AddRelevantSnapshots(relevantSnapshots, 0);
            }
            if (_currentAbility != null && snapShots[_currentAbility.GetCard().tempID] != null)
                abilityTimingData.abilitySource = snapShots[_currentAbility.GetCard().tempID];
            if (relevantSnapshots.Count > 0)
            {
                abilityTimingData.movedFrom = relevantSnapshots[0].previousLocation;
                abilityTimingData.movedTo = relevantSnapshots[0].location;
            }
            if (activation == Activation.OnRide)
            {
                List<Snapshot> temp = new List<Snapshot>();
                if (playerID == 1)
                    temp.Add(_player1.GetSnapshot(_player1.Vanguard().tempID));
                else
                    temp.Add(_player2.GetSnapshot(_player2.Vanguard().tempID));
                abilityTimingData.AddRelevantSnapshots(temp, 1);
            }
            else if (activation == Activation.PlacedOnVC)
            {
                List<Snapshot> temp = new List<Snapshot>();
                if (playerID == 1 && _player1.GetSoul().Count > 0)
                    temp.Add(_player1.GetSnapshot(_player1.GetSoul()[0].tempID));
                else if (playerID == 2 && _player2.GetSoul().Count > 0)
                    temp.Add(_player2.GetSnapshot(_player2.GetSoul()[0].tempID));
                abilityTimingData.AddRelevantSnapshots(temp, 1);
            }
            else if (activation == Activation.OnOrderPlayed && player.IsAlchemagic())
                abilityTimingData.additionalInfo = true;
            else
            {
                if (cards != null)
                {
                    for (int i = 1; i < cards.Count; i++)
                    {
                        if (snapShots[cards[i].tempID] != null)
                        {
                            relevantSnapshots.Clear();
                            relevantSnapshots.Add(snapShots[cards[i].tempID]);
                            abilityTimingData.AddRelevantSnapshots(relevantSnapshots, i);
                        }
                    }
                }
            }
            if (player.PayingCost)
                abilityTimingData.asCost = true;
            _abilityTimings.AddAbilityTiming(activation, playerID, abilityTimingData, append);
            if (playerID == 1)
                AddAbilitiesToQueue(_player1);
            else
                AddAbilitiesToQueue(_player2);
        }

        public void PerformCheckTiming(Player turnPlayer, Player nonTurnPlayer)
        {
            int actionPerformed;
            do
            {
                _abilityTimings.UpdateCount();
                actionPerformed = 0;
                AddAbilitiesToQueue(turnPlayer);
                AddAbilitiesToQueue(nonTurnPlayer);
                actionPerformed += ResolveRuleActions();
                ActivateContAbilities(turnPlayer, nonTurnPlayer);
                if (ActivateNoPromptAbilities(turnPlayer) == 1 ||
                    ActivateAbilities(turnPlayer) == 1 ||
                    ActivateNoPromptAbilities(nonTurnPlayer) == 1 ||
                    ActivateAbilities(nonTurnPlayer) == 1)
                    actionPerformed++;
            } while (actionPerformed > 0);
            ActivateContAbilities(turnPlayer, nonTurnPlayer);
            AllAbilitiesResolved();
            turnPlayer.CheckTimingPerformed();
            nonTurnPlayer.CheckTimingPerformed();
        }

        public int ResolveRuleActions()
        {
            int ruleActionPerformed = 0;

            ruleActionPerformed += _player1.ClearOverloadedCards();
            ruleActionPerformed += _player2.ClearOverloadedCards();
            //ruleActionPerformed += _player1.RetireGC();
            //ruleActionPerformed += _player2.RetireGC();
            //rule action for riding from soul onto empty VC
            if (_player1.DamageThresholdReached() || _player2.DamageThresholdReached())
                throw new ArgumentException("damage threshold reached");
            if (_player1.GetDeck().Count == 0 || _player2.GetDeck().Count == 0)
                throw new ArgumentException("deck out");
            if (_player1.Vanguard() == null || _player2.Vanguard() == null)
                throw new ArgumentException("no vanguard");

            return ruleActionPerformed;
        }

        public void UpdateTracking(object sender, CardEventArgs e)
        {
            if (e.currentLocation == e.previousLocation)
                return;
            if (!(e.currentLocation.Item1 == Location.GC || e.currentLocation.Item1 == Location.RC || e.currentLocation.Item1 == Location.VC)
                && !(e.previousLocation.Item1 == Location.GC || e.currentLocation.Item1 == Location.RC || e.currentLocation.Item1 == Location.VC))
            {
                _abilities.OnZoneChange(e.card.tempID);
                List<Card> cards = new List<Card>();
                cards.Add(e.card);
                AddAbilityTiming(Activation.OnPut, e.card.originalOwner, cards);
            }
            ActivateContAbilities(_player1, _player2);
        }

        public bool RideDeckDiscardCostReplaced(Player player)
        {
            List<Card> cards = new List<Card>();
            cards.AddRange(player.GetActiveUnits());
            List<Ability> abilities = _abilities.GetAbilities(Activation.RideDeckDiscardReplace, cards, null);
            foreach (Ability ability in abilities)
            {
                if (_inputManager.YesNo(player, ability.GetPrompt()))
                {
                    ability.PayCost();
                    return true;
                }
            }
            return false;
        }

        public void Arm(Player player, List<Card> cardsToArm)
        {
            if (cardsToArm.Count > 0)
            {
                if (_abilities.CanArm(cardsToArm[0].tempID, _player1.Vanguard().name))
                    player.Arm(_player1.Vanguard().tempID, cardsToArm[0].tempID);
            }
        }

        public List<Snapshot> GetSnapshots(int playerID, int location)
        {
            List<Snapshot> snapshots = new List<Snapshot>();
            if (actionLogs.ContainsKey(location))
            {
                foreach (ActionLog actionLog in actionLogs[location])
                {
                    if (playerID == actionLog.playerID)
                        snapshots.Add(actionLog.snapshot);
                }
            }
            return snapshots;
        }
    }

    public class C
    {
        public const bool Player = true;
        public const bool Enemy = false;
    }

    public class Phase
    {
        public const int Stand = 0;
        public const int Draw = 1;
        public const int Ride = 2;
        public const int Stride = 3;
        public const int Main = 4;
        public const int Battle = 5;
        public const int End = 6;
    }

    public class AbilityTimings
    {
        //Dictionary<int, AbilityTiming> _player1AbilityTimings = new Dictionary<int, AbilityTiming>();
        //Dictionary<int, AbilityTiming> _player2AbilityTimings = new Dictionary<int, AbilityTiming>();
        Dictionary<int, AbilityTiming> _abilityTimings = new Dictionary<int, AbilityTiming>();

        public void AddAbilityTiming(int activation, int playerID, AbilityTimingData data, bool append)
        {
            //Dictionary<int, AbilityTiming> _abilityTimings;
            //if (playerID == 1)
            //    _abilityTimings = _player1AbilityTimings;
            //else
            //    _abilityTimings = _player2AbilityTimings;
            if (!_abilityTimings.ContainsKey(activation))
                _abilityTimings[activation] = new AbilityTiming(activation);
            if (!append || !_abilityTimings[activation].GetTimingCounts().ContainsKey(1))
            {
                _abilityTimings[activation].AddAbilityTiming(data);
            }
        }

        public AbilityTiming GetAbilityTiming(int activation, int playerID)
        {
            //Dictionary<int, AbilityTiming> _abilityTimings;
            //if (playerID == 1)
            //    _abilityTimings = _player1AbilityTimings;
            //else
            //    _abilityTimings = _player2AbilityTimings;
            if (_abilityTimings.ContainsKey(activation))
                return _abilityTimings[activation];
            return null;
        }

        public List<AbilityTimingData> GetAbilityTimingData(int activation, int timingCount, int playerID)
        {
            //Dictionary<int, AbilityTiming> _abilityTimings;
            //if (playerID == 1)
            //    _abilityTimings = _player1AbilityTimings;
            //else
            //    _abilityTimings = _player2AbilityTimings;
            if (_abilityTimings.ContainsKey(activation))
                return _abilityTimings[activation].GetAbilityTimingData(timingCount);
            return new List<AbilityTimingData>();
        }

        public List<AbilityTimingData> GetAbilityTimingData(int activation)
        {
            if (_abilityTimings.ContainsKey(activation))
                return _abilityTimings[activation].GetAbilityTimingData();
            return new List<AbilityTimingData>();
        }

        public List<int> GetActivations()
        {
            List<int> activations = new List<int>();
            //foreach (int key in _player1AbilityTimings.Keys)
            //    activations.Add(key);
            //foreach (int key in _player2AbilityTimings.Keys)
            //{
            //    if (!activations.Contains(key))
            //        activations.Add(key);
            //}
            foreach (int key in _abilityTimings.Keys)
            {
                if (!activations.Contains(key))
                    activations.Add(key);
            }
            return activations;
        }

        public void UpdateCount()
        {
            //foreach (int key in _player1AbilityTimings.Keys)
            //    _player1AbilityTimings[key].UpdateCount();
            //foreach (int key in _player2AbilityTimings.Keys)
            //    _player2AbilityTimings[key].UpdateCount();
            foreach (int key in _abilityTimings.Keys)
                _abilityTimings[key].UpdateCount();
        }

        public void AddActivatedAbility(Ability ability, int activation, Tuple<int, int> count)
        {
            //Dictionary<int, AbilityTiming> _abilityTimings;
            //if (ability.GetPlayer1()._playerID == 1)
            //    _abilityTimings = _player1AbilityTimings;
            //else
            //    _abilityTimings = _player2AbilityTimings;
            AbilityTimingCount abilityTimingCount = new AbilityTimingCount(ability, activation, count);
            _abilityTimings[activation].AddActivatedAbility(abilityTimingCount, count.Item1);
        }

        public void Reset()
        {
            //_player1AbilityTimings.Clear();
            //_player2AbilityTimings.Clear();
            _abilityTimings.Clear();
        }
    }

    public class AbilityTiming
    {
        int _activation;
        Dictionary<int, int> _timingCount = new Dictionary<int, int>();
        Dictionary<int, List<AbilityTimingCount>> _activatedAbilities = new Dictionary<int, List<AbilityTimingCount>>();
        Dictionary<int, List<AbilityTimingData>> _abilityTimingData = new Dictionary<int, List<AbilityTimingData>>();
        
        public AbilityTiming(int activation)
        {
            _activation = activation;
            _timingCount[1] = 0;
        }

        public void UpdateCount()
        {
            _timingCount[_timingCount.Keys.Count + 1] = 0;
        }

        public void AddAbilityTiming(AbilityTimingData data)
        {
            if (!_abilityTimingData.ContainsKey(_timingCount.Keys.Count))
                _abilityTimingData[_timingCount.Keys.Count] = new List<AbilityTimingData>();
            _abilityTimingData[_timingCount.Keys.Count].Add(data);
            _timingCount[_timingCount.Keys.Count]++;
        }

        public List<AbilityTimingData> GetAbilityTimingData(int timingCount)
        {
            if (_abilityTimingData.ContainsKey(timingCount))
                return _abilityTimingData[timingCount];
            return new List<AbilityTimingData>();
        }

        public List<AbilityTimingData> GetAbilityTimingData()
        {
            List<AbilityTimingData> list = new List<AbilityTimingData>();
            foreach (int key in _abilityTimingData.Keys)
            {
                foreach (AbilityTimingData data in _abilityTimingData[key])
                    list.Add(data);
            }
            return list;
        }

        public Dictionary<int, int> GetTimingCounts()
        {
            return _timingCount;
        }

        public List<AbilityTimingCount> GetActivatedAbilities(int timingCount)
        {
            if (_activatedAbilities.ContainsKey(timingCount))
                return _activatedAbilities[timingCount];
            return new List<AbilityTimingCount>();
        }

        public void AddActivatedAbility(AbilityTimingCount abilityTimingCount, int timingCount)
        {
            if (!_activatedAbilities.ContainsKey(timingCount))
                _activatedAbilities[timingCount] = new List<AbilityTimingCount>();
            _activatedAbilities[timingCount].Add(abilityTimingCount);
        }

        public void Reset()
        {
            _activatedAbilities.Clear();
            _abilityTimingData.Clear();
            _timingCount.Clear();
            _timingCount[1] = 0;
        }
    }

    public class AbilityTimingData
    {
        public Snapshot[] allSnapshots = new Snapshot[200];
        List<Snapshot>[] relevantSnapshots = new List<Snapshot>[5];
        public Snapshot abilitySource;
        public bool asCost = false;
        public bool additionalInfo = false;
        public int movedTo = -1;
        public int movedFrom = -1;
        public int playerID = 0;

        public AbilityTimingData()
        {
            for (int i = 0; i < 5; i++)
                relevantSnapshots[i] = new List<Snapshot>();
        }

        public List<Snapshot> GetRelevantSnapshots(int index)
        {
            if (index < 0 || index >= 4)
                return new List<Snapshot>();
            return relevantSnapshots[index];
        }

        public void AddRelevantSnapshots(List<Snapshot> snapShots, int index)
        {
            if (index >= 0 && index <= 4)
                relevantSnapshots[index].AddRange(snapShots);
        }
    }

    public class AbilityTimingCount
    {
        public Ability ability;
        public Tuple<int, int> timingCount;
        public int activation;

        public AbilityTimingCount(Ability _ability, int _activation, Tuple<int, int> _timingCount)
        {
            ability = _ability;
            timingCount = _timingCount;
            activation = _activation;
        }
    }

    public class ActionLog
    {
        public readonly int playerID;
        public readonly Snapshot snapshot;

        public ActionLog(int _playerID, Snapshot _snapshot)
        {
            playerID = _playerID;
            snapshot = _snapshot;
        }
    }

    public static class Log
    {
        public static void WriteLine(string output)
        {
            Console.WriteLine(output);
            //using (StreamWriter writer = new StreamWriter("enginelog.txt", true))
            //{
            //    writer.WriteLine(output);
            //}
        }
    }
}
