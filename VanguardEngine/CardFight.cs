using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

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
        public List<Tuple<Ability, int>> _player1AbilityQueue = new List<Tuple<Ability, int>>();
        public List<Tuple<Ability, int>> _player2AbilityQueue = new List<Tuple<Ability, int>>();
        public List<Ability> _alchemagicQueue = new List<Ability>();
        public List<Ability> _activatedAbilities = new List<Ability>();
        public List<Tuple<Ability, int>> _skippedAbilities = new List<Tuple<Ability, int>>();
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

        public bool Initialize(List<Card> Deck1, List<Card> Deck2, List<Card> tokens, InputManager inputManager, string luaPath)
        {
            if (File.Exists("enginelog.txt"))
            {
                using (StreamWriter writer = new StreamWriter("enginelog.txt", false))
                {
                    writer.Write(string.Empty);
                }
            }
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
            _abilities = new Abilities();
            inputManager.Initialize(_player1, _player2);
            _inputManager = inputManager;
            luaInterpreter = new LuaInterpreter(luaPath, this);
            field.Initialize(deck1, deck2, tokens);
            _player1.Initialize(1, field);
            _player2.Initialize(2, field);
            for (int i = 0; i < deck1.Count; i++)
            {
                _abilities.AddAbilities(deck1[i].tempID, luaInterpreter.GetAbilities(deck1[i], _player1, _player2, true));
                _abilities.AddAbilities(deck1[i].tempID, luaInterpreter.GetAbilities(deck1[i], _player2, _player1, false));
            }
            for (int i = 0; i < deck2.Count; i++)
            {
                _abilities.AddAbilities(deck2[i].tempID, luaInterpreter.GetAbilities(deck2[i], _player2, _player1, true));
                _abilities.AddAbilities(deck2[i].tempID, luaInterpreter.GetAbilities(deck2[i], _player1, _player2, false));
            }
            return true;
        }

        public void StartFight()
        {
            Player player1;
            Player player2;
            CardEventArgs args;
            int RPS1 = 0, RPS2 = 0;
            int first = 0;
            string input;
            while (RPS1 == RPS2)
            {
                RPS1 = _inputManager.RPS();
                RPS2 = _inputManager.RPS();
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
            //TriggerCheck(player1, player2, false);
            //TriggerCheck(player1, player2, false);
            //TriggerCheck(player2, player1, false);
            //TriggerCheck(player2, player1, false);
            //TriggerCheck(player1, player2, false);
            //TriggerCheck(player2, player1, false);
            //player1.SoulCharge(10);
            //player2.SoulCharge(10);
            //player1.AbyssalDarkNight();
            //player2.AbyssalDarkNight();
            //player1.Mill(10);
            //player2.Mill(10);
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
                AddAbilityTiming(Activation.OnEndPhase, 0, null);
                PerformCheckTiming(player1, player2);
                player1.EndTurn();
                player2.EndTurn();
                player1.CardStates.EndTurn();
                player1.IncrementTurn();
                _abilities.EndTurn();
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
            AddAbilityTiming(Activation.OnRidePhase, 0, null);
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
            AddAbilityTiming(Activation.OnMainPhase, 0, null);
            PerformCheckTiming(player1, player2);
            while (true)
            {
                canSelect.Clear();
                _inputManager._abilities.Clear();
                _inputManager._abilities.AddRange(GetACTAbilities(player1));
                if (!player1.OrderPlayed())
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
                    if (player1.CanPlayOrder())
                    {
                        ChooseOrderToActivate(player1, false);
                    }
                    else
                        Log.WriteLine("Already activated order this turn.");
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
                            ActivateOrder(player1, ability);
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
            AddAbilityTiming(Activation.PlacedOnRC, player1._playerID, player1.GetLastPlacedOnRC(), player1.IsAlchemagic());
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
            AddAbilityTiming(Activation.OnBattlePhase, 0, null);
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
                AddAbilityTiming(Activation.OnAttack, 0, null);
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
                            if (player2.CanPlayOrder())
                            {
                                ChooseOrderToActivate(player2, true);
                            }
                            else
                                Log.WriteLine("Already activated order this turn.");
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
                                    ActivateOrder(player2, ability);
                                    break;
                                }
                            }
                        }
                        PerformCheckTiming(player1, player2);
                    }
                }
                if (player1.GetAttacker() != null && (player1.AttackerIsVanguard() || player1.RearguardCanDriveCheck()))
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
                        AddAbilityTiming(Activation.OnAttackHitsVanguard, 0, null);
                    }
                    if (player2.GetLastPlayerRCRetired().Count > 0)
                        AddAbilityTiming(Activation.OnPlayerRCRetired, player2._playerID, player2.GetLastPlayerRCRetired());
                    for (int i = 0; i < player2.NumberOfTimesHit(); i++)
                        AddAbilityTiming(Activation.OnAttackHits, 0, null);
                    PerformCheckTiming(player1, player2);
                    player2.RetireAttackedUnit();
                    ////_inputManager.SwapPlayers();
                }
                //_inputManager.SwapPlayers();
            }
            AddAbilityTiming(Activation.OnBattleEnds, 0, null);
            PerformCheckTiming(player1, player2);
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
            else if (check == Trigger.Over) //OVER TRIGGER
            {
                if (drivecheck)
                {
                    List<Card> list = new List<Card>();
                    list.Add(player1.GetTrigger(C.Player));
                    AddAbilityTiming(Activation.OnOverTrigger, player1._playerID, list);
                }
                player1.RemoveTrigger();
                Draw(player1, player2, 1);
                Log.WriteLine("Choose unit to give 1000000000 power to.");
                selection = _inputManager.SelectActiveUnit(player1, PromptType.AddPower, 100000000 + power);
                player1.AddTempPower(selection, 100000000 + power, false);
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

        public void Resist(Player player1, Player player2, int tempID)
        {
            _player1.Resist(tempID);
            _player2.Resist(tempID);
        }

        public void AddAbilitiesToQueue(Player player1)
        {
            List<Card> cards = new List<Card>();
            List<Ability> abilities = new List<Ability>();
            AbilityTiming abilityTiming;
            foreach (int activation in _abilityTimings.GetActivations())
            {
                abilityTiming = _abilityTimings.GetAbilityTiming(activation);
                for (int i = 1; i <= abilityTiming.GetTotal(); i++)
                {
                    cards.Add(player1.GetTrigger(C.Player));
                    abilities.AddRange(_abilities.GetAbilities(activation, cards, i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetActiveUnits(), i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetHand(), i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetDrop(), i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetSoul(), i));
                    cards.Clear();
                    foreach (Card card in player1.GetGC())
                    {
                        if (card.originalOwner == player1._playerID)
                            cards.Add(card);
                    }
                    abilities.AddRange(_abilities.GetAbilities(activation, cards, i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetPlayerOrder(), i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetBind(), i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetRemoved(), i));
                    foreach (Ability ability in abilityTiming.GetActivatedAbilities(i))
                    {
                        while (abilities.Contains(ability))
                            abilities.Remove(ability);
                    }
                    foreach (Tuple<Ability, int> ability in _skippedAbilities)
                    {
                        if (abilities.Contains(ability.Item1) && ability.Item2 == i)
                            abilities.Remove(ability.Item1);
                    }
                    foreach (Ability ability in abilities)
                    {
                        Tuple<Ability, int> newAbility = new Tuple<Ability, int>(ability, i);
                        if (_player1 == player1 && !_player1AbilityQueue.Contains(newAbility))
                            _player1AbilityQueue.Add(newAbility);
                        else if (_player2 == player1 && !_player2AbilityQueue.Contains(newAbility))
                            _player2AbilityQueue.Add(newAbility);
                    }
                    abilities.Clear();
                }
            }
        }

        public void ActivateContAbilities(Player player1, Player player2)
        {
            List<Ability> abilities = new List<Ability>();
            Ability givenAbility;
            player1.RefreshContinuous();
            player2.RefreshContinuous();
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetOrderZone(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetActiveUnits(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetGC(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetDrop(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetDeck(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetHand(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetOrderZone(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetActiveUnits(), 0));
            //abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetGC(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetDrop(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetDeck(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetHand(), 0));
            foreach (Card card in player1.GetAllUnitsOnField())
            {
                if (player1.GetGivenAbility(card.tempID) != null)
                {
                    foreach (Tuple<int, int> tuple in player1.GetGivenAbility(card.tempID))
                    {
                        givenAbility = _abilities.GetAbility(tuple.Item1, tuple.Item2);
                        if (givenAbility.GetActivations.Contains(Activation.Cont))
                        {
                            givenAbility.CheckConditionAsGiven(card, Activation.Cont);
                            givenAbility.ActivateAsGiven(card);
                        }
                    }
                }
            }
            foreach (Ability ability in abilities)
            {
                ability.Activate();
            }
            player1.UpdateRecordedValues();
            player2.UpdateRecordedValues();
        }

        public List<Ability> GetAvailableOrders(Player player, bool blitz)
        {
            if (blitz)
                return _abilities.GetAbilities(Activation.OnBlitzOrder, player.GetOrderableCards(), 0);
            else
                return _abilities.GetAbilities(Activation.OnOrder, player.GetOrderableCards(), 0);
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
                ActivateOrder(player1, abilities[selection]);
        }

        public void ActivateOrder(Player player1, Ability ability)
        {
            int amSelection = -1;
            bool proceedWithAlchemagic = false;
            _currentAbility = ability;
            if (ability.GetCard().orderType == OrderType.Normal && (player1.CanAlchemagicSame() || player1.CanAlchemagicDiff()))
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
                CardEventArgs args;
                if (OnAlchemagic != null)
                {
                    args = new CardEventArgs();
                    args.message = ability.GetCard().name + " alchemagics with " + _alchemagicQueue[amSelection].Name + "!";
                    OnAlchemagic(this, args);
                }
                ability.PayCost();
                _currentAbility = _alchemagicQueue[amSelection];
                _alchemagicQueue[amSelection].PayCost();
                ability.Activate();
                _alchemagicQueue[amSelection].ActivateAsGiven(player1.GetCard(_alchemagicQueue[amSelection].GetID()));
                player1.EndAlchemagic();
                player1.EndOrder();
            }
            else
            {
                Log.WriteLine("----------\n" + ability.Name + "'s effect activates!");
                PlayOrder(player1, ability.GetID(), false);
                ability.PayCost();
                ability.Activate();
                player1.EndOrder();
            }
            List<Card> list = new List<Card>();
            list.Add(ability.GetCard());
            AddAbilityTiming(Activation.OnOrderPlayed, player1._playerID, list);
        }

        public List<Ability> GetACTAbilities(Player player)
        {
            List<Ability> abilities = new List<Ability>();
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetActiveUnits(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetSoul(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetDrop(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetOrderZone(), 0));
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
            ability.Activate();
            player1.UpdateRecordedValues();
        }

        public void CallFromPrison(Player player1, Player player2)
        {
            List<Card> cards = new List<Card>();
            cards.Add(player2.GetPlayerPrison());
            List<Ability> abilities = _abilities.GetAbilities(Activation.OnCallFromPrison, cards, 0);
            if (abilities.Count == 0)
                return;
            abilities[0].PayCost();
            abilities[0].Activate();
        }

        public int ActivateAbilities(Player player)
        {
            int selection;
            int abilityActivated = 0;
            List<Tuple<Ability, int>> abilities;
            if (player == _player1)
                abilities = _player1AbilityQueue;
            else
                abilities = _player2AbilityQueue;
            List<Ability> tempQueue = new List<Ability>();
            int ThenNum = 0;
            foreach (Tuple<Ability, int> ability in NoPromptAbilitiesInQueue(abilities))
            {
                ability.Item1.SetTimingCount(ability.Item2);
                if (OnAbilityActivated != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.card = ability.Item1.GetCard();
                    OnAbilityActivated(this, args);
                }
                ability.Item1.Activate();
                _player1.UpdateRecordedValues();
                _player2.UpdateRecordedValues();
                _abilityTimings.AddActivatedAbility(ability.Item1, ability.Item2);
                abilities.Remove(ability);
                abilityActivated++;
            }
            if (abilities.Count == 0)
                return abilityActivated;
            List<Tuple<Ability, int>> canActivate = new List<Tuple<Ability, int>>();
            foreach (Tuple<Ability, int> ability in abilities)
            {
                if (ability.Item1.CanActivate())
                    canActivate.Add(ability);
            }
            if (canActivate.Count == 1 && canActivate[0].Item1.isMandatory)
                selection = 0;
            else
                selection = _inputManager.SelectAbility(player, canActivate);
            if (selection == abilities.Count)
            {
                foreach (Tuple<Ability, int> ability in canActivate)
                    _skippedAbilities.Add(ability);
            }
            else
            {
                _currentAbility = canActivate[selection].Item1;
                Log.WriteLine("----------\n" + canActivate[selection].Item1.Name + "'s effect activates!");
                if (OnAbilityActivated != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.card = canActivate[selection].Item1.GetCard();
                    OnAbilityActivated(this, args);
                }
                canActivate[selection].Item1.SetTimingCount(canActivate[selection].Item2);
                canActivate[selection].Item1.PayCost();
                ThenNum = canActivate[selection].Item1.Activate();
                abilityActivated++;
                _player1.UpdateRecordedValues();
                _player2.UpdateRecordedValues();
                _abilityTimings.AddActivatedAbility(canActivate[selection].Item1, canActivate[selection].Item2);
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

        public List<Tuple<Ability, int>> NoPromptAbilitiesInQueue(List<Tuple<Ability, int>> abilities)
        {
            List<Tuple<Ability, int>> noPromptAbilities = new List<Tuple<Ability, int>>();
            foreach (Tuple<Ability, int> ability in abilities)
            {
                if (!ability.Item1.hasPrompt)
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

        public List<int> Search(Player player1, Player player2, List<Card> canSearch)
        {
            List<int> cardsToSearch = _inputManager.SelectFromList(player1, canSearch, 1, 1, "to search.");
            player1.Search(cardsToSearch);
            return cardsToSearch;
        }

        public List<Card> Stand(Player player1, Player player2, List<Card> canStand, int count, bool select)
        {
            List<int> cardsToStand = _inputManager.SelectFromList(player1, canStand, count, count, "card(s) to stand.");
            player1.Stand(cardsToStand);
            AddAbilityTiming(Activation.OnStand, player1._playerID, player1.GetLastStood());
            return player1.GetLastStood();
        }

        public void Stand(Player player1, Player player2, List<int> toStand)
        {
            player1.Stand(toStand);
            AddAbilityTiming(Activation.OnStand, player1._playerID, player1.GetLastStood());
        }

        public void Rest(Player player1, Player player2, List<Card> canRest, int count, bool select)
        {
            List<int> cardsToRest = _inputManager.SelectFromList(player1, canRest, count, count, "to rest.");
            player1.Rest(cardsToRest);
            player2.Rest(cardsToRest);
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

        public void AddSkill(Player player1, Player player2, Card card, int skill)
        {
            player1.AddSkill(card.tempID, skill);
            player2.AddSkill(card.tempID, skill);
        }

        public void AddToHand(Player player1, Player player2, List<Card> canAddToHand, int count, int min)
        {
            List<int> cardsToAddToHand = _inputManager.SelectFromList(player1, canAddToHand, count, min, "to add to hand.");
            player1.AddToHand(cardsToAddToHand);
        }

        public void AddToSoul(Player player1, Player player2, List<Card> canAddToHand, int count, int min)
        {
            List<int> cardsToAddToSoul = _inputManager.SelectFromList(player1, canAddToHand, count, min, "to add to soul.");
            player1.AddToSoul(cardsToAddToSoul);
        }

        public void SelectCardToRetire(Player player1, Player player2, List<Card> canRetire, int count, bool upto)
        {
            List<int> cardsToRetire = _inputManager.SelectFromList(player1, canRetire, count, count, "to retire.");
            player1.Retire(cardsToRetire);
            if (player1.GetLastPlayerRCRetired().Count > 0)
                AddAbilityTiming(Activation.OnPlayerRCRetired, player1._playerID, player1.GetLastPlayerRCRetired());
            if (player1.PlayerRetired() && _currentAbility.GetPlayer1() == player1 && _currentAbility.IsPayingCost())
            {
                AddAbilityTiming(Activation.OnRetiredForPlayerCost, player1._playerID, player1.GetLastPlayerRetired());
            }
            if (player1.EnemyRetired())
                AddAbilityTiming(Activation.OnEnemyRetired, 0, null);
            AddToChosen(cardsToRetire);
        }

        public void Retire(Player player1, Player player2, List<int> toRetire)
        {
            player1.Retire(toRetire);
            if (player1.EnemyRetired())
                AddAbilityTiming(Activation.OnEnemyRetired, 0, null);
        }

        public bool ChooseSendToBottom(Player player1, Player player2, List<Card> canSend, int max, int min, bool cost)
        {
            List<int> cardsToSend = _inputManager.SelectFromList(player1, canSend, max, min, "to send to bottom of deck.");
            AddToChosen(cardsToSend);
            if (cost)
                player1.Reveal(cardsToSend);
            return player1.SendToDeck(cardsToSend, true);
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
            int result = player1.PlayOrder(tempID);
            if (result == 1)
            {
                AddAbilityTiming(Activation.PutOnOrderZone, player1._playerID, player1.GetLastPutOnOrderZone());
            }
        }

        public void ChooseReveal(Player player1, Player player2, List<Card> canReveal, int max, int min)
        {
            List<int> cardsToReveal = _inputManager.SelectFromList(player1, canReveal, max, min, "to reveal.");
            player1.Reveal(cardsToReveal);
        }

        public void RevealFromDeck(Player player1, Player player2, int count)
        {
            player1.RevealFromDeck(count);
        }

        public void EndReveal(Player player1, Player player2)
        {
            player1.EndReveal();
        }

        public void ChooseImprison(Player player1, Player player2, List<Card> cardsToSelect, int count, int min)
        {
            List<int> cardsToImprison = _inputManager.SelectFromList(player1, cardsToSelect, count, min, "to imprison.");
            player1.Imprison(cardsToImprison);
            AddToChosen(cardsToImprison);
        }

        public void EnemyChooseImprison(Player player1, Player player2, List<Card> cardsToSelect, int count, int min)
        {
            List<int> cardsToImprison = _inputManager.SelectFromList(player2, cardsToSelect, count, min, "to imprison.");
            player1.Imprison(cardsToImprison);
        }

        public void ChooseMoveEnemyRearguard(Player player1, List<Card> cardsToSelect, List<int> availableCircles)
        {
            int selection = _inputManager.SelectFromList(player1, cardsToSelect, 1, 1, "to switch places.")[0];
            int selection2 = _inputManager.SelectCircle(player1, availableCircles, 1)[0];
            player1.MoveRearguardSpecific(selection, selection2);
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
            AddAbilityTiming(Activation.OnChosen, 0, null);
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

        public List<Card> GetList(int abilityTiming, int playerID, int timingCount)
        {
            return _abilityTimings.GetList(abilityTiming, playerID, timingCount);
        }

        public FieldSnapShot GetSnapShot(int abilityTiming, int timingCount)
        {
            return _abilityTimings.GetSnapShot(abilityTiming, timingCount);
        }

        public void Sing(Player player, List<Card> cardsToSing, int count)
        {
            List<int> selectedCards = _inputManager.SelectFromList(player, cardsToSing, count, count, "to sing.");
            if (selectedCards.Count == 0)
                return;
            List<Card> songs = new List<Card>();
            foreach (int tempID in selectedCards)
                songs.Add(player.GetCard(tempID));
            List<Ability> abilities = _abilities.GetAbilities(Activation.OnSing, songs, 0);
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
            ability.Activate();
            player.Flip(selectedCards, false);
            player.MyStates.AddUntilEndOfTurnState(PlayerState.VanguardHasSungSongThisTurn);
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

        public void AddAbilityTiming(int activation, int playerID, List<Card> cards)
        {
            _abilityTimings.AddAbilityTiming(activation, playerID, _player1.GenerateSnapShot(), cards, false);
        }

        public void AddAbilityTiming(int activation, int playerID, List<Card> cards, bool append)
        {
            _abilityTimings.AddAbilityTiming(activation, playerID, _player1.GenerateSnapShot(), cards, append);
        }

        public void PerformCheckTiming(Player turnPlayer, Player nonTurnPlayer)
        {
            int actionPerformed;
            do
            {
                actionPerformed = 0;
                AddAbilitiesToQueue(turnPlayer);
                AddAbilitiesToQueue(nonTurnPlayer);
                actionPerformed += ResolveRuleActions();
                ActivateContAbilities(turnPlayer, nonTurnPlayer);
                actionPerformed += ActivateAbilities(turnPlayer);
                actionPerformed += ActivateAbilities(nonTurnPlayer);
            } while (actionPerformed > 0);
            ActivateContAbilities(turnPlayer, nonTurnPlayer);
            AllAbilitiesResolved();
        }

        public int ResolveRuleActions()
        {
            int ruleActionPerformed = 0;

            ruleActionPerformed += _player1.ClearOverloadedCards();
            ruleActionPerformed += _player2.ClearOverloadedCards();
            //ruleActionPerformed += _player1.RetireGC();
            //ruleActionPerformed += _player2.RetireGC();
            //rule action for riding from soul onto empty VC

            if (_player1.GetDamageZone().Count >= 6 || _player2.GetDamageZone().Count >= 6)
                throw new ArgumentException("damage zone reached 6");
            if (_player1.GetDeck().Count == 0 || _player2.GetDeck().Count == 0)
                throw new ArgumentException("deck out");
            if (_player1.Vanguard() == null || _player2.Vanguard() == null)
                throw new ArgumentException("no vanguard");

            return ruleActionPerformed;
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
        Dictionary<int, AbilityTiming> _abilityTimings = new Dictionary<int, AbilityTiming>();
        TimingCountLists _player1TimingCountLists = new TimingCountLists();
        TimingCountLists _player2TimingCountLists = new TimingCountLists();

        public void AddAbilityTiming(int activation, int playerID, FieldSnapShot snapShot, List<Card> cards, bool append)
        {
            if (!_abilityTimings.ContainsKey(activation))
                _abilityTimings[activation] = new AbilityTiming(activation);
            if (!append || _abilityTimings[activation].GetTotal() == 0)
                _abilityTimings[activation].AddAbilityTiming(snapShot);
            if (cards != null)
            {
                if (playerID == 1)
                    _player1TimingCountLists.AddList(activation, _abilityTimings[activation].GetTotal(), cards);
                else if (playerID == 2)
                    _player2TimingCountLists.AddList(activation, _abilityTimings[activation].GetTotal(), cards);
            }
        }

        public AbilityTiming GetAbilityTiming(int activation)
        {
            return _abilityTimings[activation];
        }

        public List<int> GetActivations()
        {
            List<int> activations = new List<int>();
            foreach (int key in _abilityTimings.Keys)
                activations.Add(key);
            return activations;
        }

        public void AddActivatedAbility(Ability ability, int count)
        {
            foreach (int activation in ability.GetActivations)
                _abilityTimings[activation].AddActivatedAbility(ability, count);
        }

        public void Reset()
        {
            foreach (int key in _abilityTimings.Keys)
            {
                _abilityTimings[key].Reset();
            }
            _abilityTimings.Clear();
            _player1TimingCountLists.Reset();
            _player2TimingCountLists.Reset();
        }

        public int CurrentCount(int abilityTiming)
        {
            if (_abilityTimings.ContainsKey(abilityTiming))
                return (_abilityTimings[abilityTiming].GetTotal());
            return 0;
        }

        public List<Card> GetList(int abilityTiming, int playerID, int timingCount)
        {
            if (playerID == 1)
                return _player1TimingCountLists.GetList(abilityTiming, timingCount);
            else
                return _player2TimingCountLists.GetList(abilityTiming, timingCount);
        }

        public FieldSnapShot GetSnapShot(int abilityTiming, int timingCount)
        {
            if (_abilityTimings.ContainsKey(abilityTiming))
                return _abilityTimings[abilityTiming].GetSnapShot(timingCount);
            return null;
        }
    }

    public class AbilityTiming
    {
        int _activation;
        int _number = 0;
        Dictionary<int, List<Ability>> _activatedAbilities = new Dictionary<int, List<Ability>>();
        Dictionary<int, FieldSnapShot> _snapShots = new Dictionary<int, FieldSnapShot>();
        
        public AbilityTiming(int activation)
        {
            _activation = activation;
        }

        public void AddAbilityTiming(FieldSnapShot snapShot)
        {
            _number++;
            _activatedAbilities[_number] = new List<Ability>();
            _snapShots[_number] = snapShot;
        }

        public int GetTotal()
        {
            return _number;
        }

        public List<Ability> GetActivatedAbilities(int number)
        {
            return _activatedAbilities[number];
        }

        public void AddActivatedAbility(Ability ability, int count)
        {
            _activatedAbilities[count].Add(ability);
        }

        public void Reset()
        {
            foreach (int key in _activatedAbilities.Keys)
                _activatedAbilities[key].Clear();
            _activatedAbilities.Clear();
        }

        public FieldSnapShot GetSnapShot(int timingCount)
        {
            if (_snapShots.ContainsKey(timingCount))
                return _snapShots[timingCount];
            return null;
        }
    }

    public class TimingCountLists
    {
        Dictionary<int, Dictionary<int, List<Card>>> timingCountLists = new Dictionary<int, Dictionary<int, List<Card>>>();

        public void AddList(int abilityTiming, int timingCount, List<Card> cards)
        {
            if (!timingCountLists.ContainsKey(abilityTiming))
                timingCountLists[abilityTiming] = new Dictionary<int, List<Card>>();
            if (!timingCountLists[abilityTiming].ContainsKey(timingCount))
                timingCountLists[abilityTiming][timingCount] = new List<Card>();
            timingCountLists[abilityTiming][timingCount].AddRange(cards);
        }

        public List<Card> GetList(int abilityTiming, int timingCount)
        {
            if (timingCountLists.ContainsKey(abilityTiming) && timingCountLists[abilityTiming].Keys.Count > 0)
            {
                if (timingCount == -1)
                    return timingCountLists[abilityTiming][timingCountLists[abilityTiming].Keys.Max()];
                if (timingCountLists[abilityTiming].ContainsKey(timingCount))
                    return timingCountLists[abilityTiming][timingCount];
            }
            return new List<Card>();
        }

        public void Reset()
        {
            foreach (int key in timingCountLists.Keys)
            {
                if (timingCountLists[key] != null)
                {
                    foreach (int key2 in timingCountLists[key].Keys)
                    {
                        if (timingCountLists[key][key2] != null)
                            timingCountLists[key][key2].Clear();
                    }
                    timingCountLists[key].Clear();
                }
            }
        }
    }

    public static class Log
    {
        public static void WriteLine(string output)
        {
            Console.WriteLine(output);
            using (StreamWriter writer = new StreamWriter("enginelog.txt", true))
            {
                writer.WriteLine(output);
            }
        }
    }
}
