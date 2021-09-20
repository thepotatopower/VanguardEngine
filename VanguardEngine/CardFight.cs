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
        public Player actingPlayer;
        public int _turn;
        protected int _phase = Phase.Stand;
        public bool _gameOver = false;
        public bool _activatedOrder = false;
        public LuaInterpreter luaInterpreter;
        public InputManager _inputManager;
        public List<Card> _deck1;
        public List<Card> _deck2;
        public Abilities _abilities;
        public List<Tuple<Ability, int>> _abilityQueue = new List<Tuple<Ability, int>>();
        public List<Ability> _alchemagicQueue = new List<Ability>();
        public List<Ability> _activatedAbilities = new List<Ability>();
        public PlayTimings _playTimings = new PlayTimings();
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
                _inputManager.SwapPlayers();
            }
            Mulligan(player1, player2);
            Log.WriteLine("----------\nSTAND UP! VANGUARD!");
            player1.StandUpVanguard();
            player2.StandUpVanguard();
            _turn = 1;
            _phase = 0;
            actingPlayer = player1;
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
                actingPlayer = player1;
                ActivateAbilities(player1, player2);
                player1.UpdateRecordedValues();
                player2.UpdateRecordedValues();
                _phase = Phase.Stand;
                if (OnStandPhase != null)
                    OnStandPhase(this, new CardEventArgs());
                player1.StandAll();
                _phase = Phase.Draw;
                if (OnDrawPhase != null)
                    OnDrawPhase(this, new CardEventArgs());
                Log.WriteLine("----------\nSTAND AND DRAW");
                Draw(player1, player2, 1);
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
                _playTimings.AddPlayTiming(Activation.OnEndPhase, 0, null);
                ActivateAbilities(player1, player2);
                player1.EndTurn();
                player2.EndTurn();
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
                    _inputManager.SwapPlayers();
                }
                else
                {
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                    player1 = _player1;
                    player2 = _player2;
                    _inputManager.SwapPlayers();
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
                selection = _inputManager.SelectCardsToMulligan();
                player1.MulliganCards(selection);
                Log.WriteLine("----------\nNew hand: ");
                player1.PrintHand();
                if (player1 == _player1)
                {
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                    player1 = _player2;
                    player2 = _player1;
                    _inputManager.SwapPlayers();
                }
                else
                {
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                    player1 = _player1;
                    player2 = _player2;
                    _inputManager.SwapPlayers();
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
            _playTimings.AddPlayTiming(Activation.OnRidePhase, 0, null);
            ActivateAbilities(player1, player2);
            while (player1.CanRideFromRideDeck() || player1.CanRideFromHand())
            {
                selection = _inputManager.SelectRidePhaseAction();
                if (selection == RidePhaseAction.RideFromRideDeck)
                {
                    if (player1.CanRideFromRideDeck())
                    {
                        Discard(player1, player2, player1.GetHand(), 1, 1);
                        input = _inputManager.SelectFromList(player1, player1.GetRideableCards(true), 1, 1, "Select card to ride.")[0];
                        Ride(player1, player2, 0, input);
                        ActivateAbilities(player1, player2);
                    }
                    else
                        Log.WriteLine("Invalid.");
                }
                else if (selection == 2)
                {
                    if (player1.CanRideFromHand())
                    {
                        input = _inputManager.SelectFromList(player1, player1.GetRideableCards(false), 1, 1, "Select card to ride.")[0];
                        Ride(player1, player2, 0, input);
                        ActivateAbilities(player1, player2);
                    }
                    else
                        Log.WriteLine("Invalid.");
                }
                else if (selection == RidePhaseAction.RideFromHand)
                {
                    Ride(player1, player2, 0, _inputManager.int_input2);
                }
                else if (selection == RidePhaseAction.End)
                    break;
            }
        }

        public void Ride(Player player1, Player player2, int location, int selection)
        {
            player1.Ride(selection);
            _playTimings.AddPlayTiming(Activation.OnRide, player1._playerID, player1.GetLastRidden());
            _playTimings.AddPlayTiming(Activation.PlacedOnVC, player1._playerID, player1.GetLastPlacedOnVC());
            ActivateAbilities(player1, player2);
        }

        public void Discard(Player player1, Player player2, List<Card> cardsToSelect, int max, int min)
        {
            List<int> cardsToDiscard = _inputManager.SelectFromList(player1, cardsToSelect, max, min, "Choose card(s) to discard.");
            player1.Discard(cardsToDiscard);
            _playTimings.AddPlayTiming(Activation.OnDiscard, player1._playerID, player1.GetLastDiscarded());
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
            while (true)
            {
                canSelect.Clear();
                _inputManager._abilities.Clear();
                _inputManager._abilities.AddRange(GetACTAbilities(player1));
                if (!player1.OrderPlayed())
                    _inputManager._abilities.AddRange(GetAvailableOrders(player1, false));
                selection = _inputManager.SelectMainPhaseAction();
                if (selection == 1)
                    BrowseHand(player1);
                else if (selection == 2)
                    BrowseField(player1);
                else if (selection == 3)
                {
                    if (player1.CanCallRearguard())
                    {
                        input = _inputManager.SelectRearguardToCall();
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
                    ActivateAbilities(player1, player2);
                }
                else if (selection == 5)
                {
                    if (player1.CanMoveRearguard())
                    {
                        if (player1.CanFreeSwap())
                        {
                            selections = _inputManager.MoveRearguards();
                            MoveRearguard(player1, player2, selections.Item1, selections.Item2);
                        }
                        else
                        {
                            selection = _inputManager.SelectRearguardColumn();
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
                    if (!player1.OrderPlayed())
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
            }
        }

        public void Call(Player player1, Player player2, int location, int selection, bool overDress)
        {
            player1.Call(location, selection, overDress);
            _abilities.ResetActivation(selection);
            _playTimings.AddPlayTiming(Activation.PlacedOnRC, player1._playerID, player1.GetLastPlacedOnRC());
            _playTimings.AddPlayTiming(Activation.PlacedOnRCFromHand, player1._playerID, player1.GetLastPlacedOnRCFromHand());
            ActivateAbilities(player1, player2);
            player1.DoneCalling();
            player1.ClearOverloadedCards();
            player2.ClearOverloadedCards();
        }

        public bool SuperiorCall(Player player1, Player player2, List<Card> cardsToSelect, int max, int min, List<int> circles, bool overDress, bool standing, bool free)
        {
            List<int> selections;
            int selectedCircle = 0;
            List<int> selectedCircles = new List<int>();
            List<int> canSelect = new List<int>();
            int sc = 0;
            bool successful = false;
            selections = _inputManager.SelectFromList(player1, cardsToSelect, max, min, "Choose card to Call.");
            foreach (int tempID in selections)
            {
                if (circles != null)
                {
                    foreach (int circle in circles)
                    {
                        if (player1.GetAvailableCircles(tempID).Contains(circle))
                            canSelect.Add(circle);
                    }
                }
                else
                    canSelect.AddRange(player1.GetAvailableCircles(tempID));
                if (canSelect.Count == 0)
                    continue;
                if (canSelect.Count == 1)
                    selectedCircle = canSelect[0];
                else
                    selectedCircle = _inputManager.SelectCallLocation(player1, "Choose RC.", player1.GetCard(tempID), selectedCircles, canSelect);
                selectedCircles.Add(selectedCircle);
                sc = player1.SuperiorCall(selectedCircle, tempID, overDress, standing);
                successful = true;
                if (sc == 2 && OnFree != null)
                {
                    CardEventArgs args = new CardEventArgs();
                    args.playerID = player2._playerID;
                    OnFree(this, args);
                }
                _abilities.ResetActivation(tempID);
            }
            _playTimings.AddPlayTiming(Activation.PlacedOnRC, player1._playerID, player1.GetLastPlacedOnRC(), true);
            if (sc == 1)
                _playTimings.AddPlayTiming(Activation.PlacedOnRCFromHand, player1._playerID, player1.GetLastPlacedOnRCFromHand(), true);
            else if (sc == 2)
                _playTimings.AddPlayTiming(Activation.PlacedOnRCFromPrison, player1._playerID, player1.GetLastPlacedOnRCFromPrison(), true);
            player1.DoneCalling();
            player1.ClearOverloadedCards();
            player2.ClearOverloadedCards();
            return successful;
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
            _playTimings.AddPlayTiming(Activation.OnBattlePhase, 0, null);
            ActivateAbilities(player1, player2);
            while (true)
            {
                if (player1.CanAttack())
                {
                    booster = -1;
                    selection = _inputManager.SelectBattlePhaseAction();
                    if (selection == 1)
                        BrowseHand(player1);
                    else if (selection == 2)
                        BrowseField(player1);
                    else if (selection == 3)
                    {
                        attacker = _inputManager.SelectAttackingUnit();
                        player1.SetAttacker(attacker);
                        if (player1.CanBeBoosted())
                        {
                            Log.WriteLine("----------\nBoost?");
                            if (_inputManager.YesNo(player1, "Boost?"))
                            {
                                booster = player1.GetBooster(attacker);
                            }
                        }
                        target = _inputManager.SelectUnitToAttack();
                        Attack(player1, player2, booster, target);
                        if (_gameOver)
                            return;
                    }
                    else if (selection == BattlePhaseAction.End)
                        break;
                    else if (selection == BattlePhaseAction.Attack) //for use outside of console only
                    {
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
                        target = _inputManager.SelectUnitToAttack();
                        if (target >= 0)
                            Attack(player1, player2, booster, target);
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

        public void Attack(Player player1, Player player2, int booster, int target)
        {
            int selection;
            int selection2;
            int drive;
            int critical;
            List<int> selections;
            bool attackHits = false;
            player1.InitiateAttack(booster, target);
            _playTimings.AddPlayTiming(Activation.OnAttack, 0, null);
            ActivateAbilities(player1, player2);
            if (player1 == _player1)
                Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
            else
                Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
            _inputManager.SwapPlayers();
            while (player2.GetAttackedCards().Count > 0)
            {
                _inputManager._abilities.Clear();
                if (!player2.OrderPlayed())
                    _inputManager._abilities.AddRange(GetAvailableOrders(player2, true));
                player2.PrintEnemyAttack();
                if (player2.GetAttackedCards().Count > 0)
                {
                    selection = _inputManager.SelectGuardPhaseAction();
                    if (selection == 1)
                        BrowseHand(player2);
                    else if (selection == 2)
                        BrowseField(player2);
                    else if (selection == 3)
                        player2.PrintShield();
                    else if (selection == GuardStepAction.Guard)
                    {
                        if (player2.GetAttackedCards().Count > 1)
                            selection2 = _inputManager.SelectCardToGuard();
                        else
                            selection2 = -1;
                        if (player2.MustGuardWithTwo())
                            selections = _inputManager.SelectFromList(player2, player2.GetGuardableCards(), player2.GetGuardableCards().Count, 2, "Choose card(s) to guard with.");
                        else
                            selections = _inputManager.SelectFromList(player2, player2.GetGuardableCards(), player2.GetGuardableCards().Count, 1, "Choose card(s) to guard with.");
                        if (selections.Count == 0)
                            continue;
                        player2.Guard(selections, selection2);
                        _playTimings.AddPlayTiming(Activation.PlacedOnGC, player2._playerID, player2.GetLastPlacedOnGC());
                        _playTimings.AddPlayTiming(Activation.PutOnGC, player2._playerID, player2.GetLastPutOnGC());
                        ActivateAbilities(player2, player1);
                    }
                    else if (selection == 5)
                    {
                        if (player2.GetInterceptableCards().Count > 0)
                        {
                            selections = _inputManager.SelectFromList(player2, player2.GetInterceptableCards(), 1, 1, "Select card to intercept with.");
                            if (player2.GetAttackedCards().Count > 1)
                                selection2 = _inputManager.SelectCardToGuard();
                            else
                                selection2 = -1;
                            player2.Guard(selections, selection2);
                            _playTimings.AddPlayTiming(Activation.PutOnGC, player2._playerID, player2.GetLastPutOnGC());
                            ActivateAbilities(player2, player1);
                        }
                        else
                            Log.WriteLine("No interceptable cards available.");
                    }
                    else if (selection == 6)
                    {
                        if (!player2.OrderPlayed())
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
                            selection2 = _inputManager.SelectCardToGuard();
                        else
                            selection2 = -1;
                        player2.Guard(selections, selection2);
                        _playTimings.AddPlayTiming(Activation.PutOnGC, player2._playerID, player2.GetLastPutOnGC());
                        ActivateAbilities(player2, player1);
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
                }
             }
            if (player1.AttackerIsVanguard() || player1.RearguardCanDriveCheck())
            {
                if (player1 == _player1)
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                else
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                _inputManager.SwapPlayers();
                drive = player1.Drive();
                for (int i = 0; i < drive; i++)
                    TriggerCheck(player1, player2, true);
                if (player1 == _player1)
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                else
                    Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                _inputManager.SwapPlayers();
            }
            if (player2.GetAttackedCards().Count > 0 && player2.AttackHits())
                attackHits = true;
            player2.RetireGC();
            if (attackHits)
            {
                if (player2.VanguardHit(C.Player))
                {
                    critical = player1.Critical();
                    for (int i = 0; i < critical; i++)
                    {
                        TriggerCheck(player2, player1, false);
                        if (player2.Damage() == 6)
                        {
                            _gameOver = true;
                            return;
                        }
                    }
                    _playTimings.AddPlayTiming(Activation.OnAttackHitsVanguard, 0, null);
                }
                player2.RetireAttackedUnit();
                for (int i = 0; i < player2.NumberOfTimesHit(); i++)
                    _playTimings.AddPlayTiming(Activation.OnAttackHits, 0, null);
                _inputManager.SwapPlayers();
                ActivateAbilities(player1, player2);
                _inputManager.SwapPlayers();
            }
            _playTimings.AddPlayTiming(Activation.OnBattleEnds, 0, null);
            ActivateAbilities(player1, player2);
            if (player1 == _player1)
                Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
            else
                Log.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
            _inputManager.SwapPlayers();
            player1.EndAttack();
            player2.EndAttack();
        }

        public void TriggerCheck(Player player1, Player player2, bool drivecheck)
        {
            int check;
            int selection;
            int max;
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
                selection = _inputManager.SelectActiveUnit(PromptType.AddPower, 10000 + power);
                player1.AddTempPower(selection, 10000 + power, false);
                player1.UpdateRecordedValues();
                player2.UpdateRecordedValues();
            }
            if (check == Trigger.Critical) 
            {
                Log.WriteLine("----------\nChoose unit to give Critical to.");
                selection = _inputManager.SelectActiveUnit(PromptType.AddCritical, 1);
                player1.AddCritical(selection, 1);
                player1.UpdateRecordedValues();
                player2.UpdateRecordedValues();
            }
            else if (check == Trigger.Stand) //STAND TRIGGER (no stand triggers rn, will fix later if needed)
            {
                Log.WriteLine("----------\nChoose unit to Stand.");
                selection = _inputManager.SelectActiveUnit(PromptType.Stand, 0);
                player1.Stand(selection);
            }
            else if (check == Trigger.Draw) //DRAW TRIGGER
            {
                Draw(player1, player2, 1);
            }
            else if (check == Trigger.Heal) //HEAL TRIGGER
            {
                if (player1.CanHeal())
                {
                    selection = _inputManager.SelectFromList(player1, player1.GetDamageZone(), 1, 1, "Choose damage to heal.")[0];
                    player1.Heal(selection);
                }
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
                    _playTimings.AddPlayTiming(Activation.OnOverTrigger, 0, null);
                    AddAbilitiesToQueue(player1, player2);
                }
                player1.RemoveTrigger();
                Draw(player1, player2, 1);
                Log.WriteLine("Choose unit to give 1000000000 power to.");
                selection = _inputManager.SelectActiveUnit(PromptType.AddPower, 100000000 + power);
                player1.AddTempPower(selection, 100000000 + power, false);
                player1.UpdateRecordedValues();
                player2.UpdateRecordedValues();
                if (drivecheck)
                    ActivateAbilities(player1, player2);
            }
            if (drivecheck)
            {
                _playTimings.AddPlayTiming(Activation.OnDriveCheck, 0, null);
                ActivateAbilities(player1, player2);
                player1.AddTriggerToHand();
            }
            else
            {
                player1.TakeDamage();
            }
            ActivateContAbilities(player1, player2);
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

        public void AddAbilitiesToQueue(Player player1, Player player2)
        {
            List<Card> cards = new List<Card>();
            List<Ability> abilities = new List<Ability>();
            PlayTiming playTiming;
            foreach (int activation in _playTimings.GetActivations())
            {
                playTiming = _playTimings.GetPlayTiming(activation);
                for (int i = 1; i <= playTiming.GetTotal(); i++)
                {
                    cards.Add(player1.GetTrigger(C.Player));
                    abilities.AddRange(_abilities.GetAbilities(activation, cards, i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetActiveUnits(), i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetHand(), i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetDrop(), i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetSoul(), i));
                    abilities.AddRange(_abilities.GetAbilities(activation, player1.GetGC(), i));
                    foreach (Ability ability in playTiming.GetActivatedAbilities(i))
                    {
                        while (abilities.Contains(ability))
                            abilities.Remove(ability);
                    }
                    foreach (Ability ability in abilities)
                    {
                        _abilityQueue.Add(new Tuple<Ability, int>(ability, i));
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
                        if (givenAbility.AbilityType == AbilityType.Cont)
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
                player1.UpdateRecordedValues();
                player2.UpdateRecordedValues();
            }
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
                if (OnAbilityActivated != null)
                {
                    args = new CardEventArgs();
                    args.card = ability.GetCard();
                    OnAbilityActivated(this, args);
                }
                ability.PayCost();
                _currentAbility = _alchemagicQueue[amSelection];
                if (OnAbilityActivated != null)
                {
                    args = new CardEventArgs();
                    args.card = _alchemagicQueue[amSelection].GetCard();
                    OnAbilityActivated(this, args);
                }
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
            _playTimings.AddPlayTiming(Activation.OnOrderPlayed, 0, null);
            _playTimings.AddPlayTiming(Activation.PutOnOrderZone, player1._playerID, player1.GetLastPutOnOrderZone());
            if (_player1 == player1)
                ActivateAbilities(_player1, _player2);
            else
                ActivateAbilities(_player2, _player1);
        }

        public List<Ability> GetACTAbilities(Player player)
        {
            List<Ability> abilities = new List<Ability>();
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetActiveUnits(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetSoul(), 0));
            abilities.AddRange(_abilities.GetAbilities(Activation.OnACT, player.GetDrop(), 0));
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
            if (player1 == _player1)
                ActivateAbilities(_player1, _player2);
            else
                ActivateAbilities(_player2, _player1);
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

        public void ActivateAbilities(Player player1, Player player2)
        {
            int selection;
            List<Tuple<Ability, int>> abilities = _abilityQueue;
            List<Ability> tempQueue = new List<Ability>();
            List<Tuple<Ability, int>> continuousAbilities;
            int ThenID = 0;
            Ability ThenAbility = null;
            int ThenNum = 0;
            bool player1Selected = true;
            bool player2Selected = true;
            Player choosingPlayer = player1;
            Player waitingPlayer = player2;
            ActivateContAbilities(player1, player2);
            while (player1Selected || player2Selected)
            {
                if (choosingPlayer == player1)
                    player1Selected = false;
                else
                    player2Selected = false;
                AddAbilitiesToQueue(choosingPlayer, waitingPlayer);
                while (abilities.Count > 0)
                {
                    continuousAbilities = ContinuousAbilitiesInQueue();
                    foreach (Tuple<Ability, int> ability in continuousAbilities)
                    {
                        ability.Item1.SetTimingCount(ability.Item2);
                        if (OnAbilityActivated != null)
                        {
                            CardEventArgs args = new CardEventArgs();
                            args.card = ability.Item1.GetCard();
                            OnAbilityActivated(this, args);
                        }
                        ability.Item1.Activate();
                        player1.UpdateRecordedValues();
                        player2.UpdateRecordedValues();
                        abilities.Remove(ability);
                    }
                    if (abilities.Count == 0)
                    {
                        break;
                    }
                    selection = _inputManager.SelectAbility(choosingPlayer, abilities);
                    if (selection == abilities.Count)
                    {
                        abilities.Clear();
                        break;
                    }
                    else
                    {
                        if (choosingPlayer == player1)
                            player1Selected = true;
                        else
                            player2Selected = true;
                        _currentAbility = abilities[selection].Item1;
                        Log.WriteLine("----------\n" + abilities[selection].Item1.Name + "'s effect activates!");
                        if (OnAbilityActivated != null)
                        {
                            CardEventArgs args = new CardEventArgs();
                            args.card = abilities[selection].Item1.GetCard();
                            OnAbilityActivated(this, args);
                        }
                        abilities[selection].Item1.PayCost();
                        ThenNum = abilities[selection].Item1.Activate();
                        player1.UpdateRecordedValues();
                        player2.UpdateRecordedValues();
                        _playTimings.AddActivatedAbility(abilities[selection].Item1, abilities[selection].Item2);
                        ThenID = abilities[selection].Item1.GetID();
                        abilities.Clear();
                        if (ThenNum > 0)
                        {
                            ThenAbility = _abilities.GetAbility(ThenID, ThenNum);
                            if (!ThenAbility.CheckCondition(Activation.Then))
                                continue;
                            if (ThenAbility.isMandatory)
                                ThenAbility.Activate();
                            else
                            {
                                //Log.WriteLine("Activate ability?");
                                if (_inputManager.YesNo(choosingPlayer, "Activate ability?"))
                                    ThenAbility.Activate();
                            }
                        }
                    }
                    abilities.Clear();
                    AddAbilitiesToQueue(choosingPlayer, waitingPlayer);
                }
                if (choosingPlayer == player1)
                {
                    choosingPlayer = player2;
                    waitingPlayer = player1;
                }
                else
                {
                    choosingPlayer = player1;
                    waitingPlayer = player2;
                }
            }
            abilities.Clear();
            _chosen.Clear();
            _playTimings.Reset();
            player1.AllAbilitiesResolved();
            player2.AllAbilitiesResolved();
            ActivateContAbilities(player1, player2);
        }

        public List<Tuple<Ability, int>> ContinuousAbilitiesInQueue()
        {
            List<Tuple<Ability, int>> continuousAbilities = new List<Tuple<Ability, int>>();
            foreach (Tuple<Ability, int> ability in _abilityQueue)
            {
                if (ability.Item1.isContinuous)
                    continuousAbilities.Add(ability);
            }
            return continuousAbilities;
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
            List<int> cardsToCB = _inputManager.SelectFromList(player1, canCB, count, min, "Choose card(s) to Counter Blast.");
            player1.AddAlchemagicFreeCB(-1 * (originalCount - cardsToCB.Count));
            player1.CounterBlast(cardsToCB);
        }

        public void SoulBlast(Player player1, Player player2, List<Card> canSB, int count)
        {
            if (player1.IsAlchemagic() && player1.AlchemagicFreeSBAvailable())
            {
                if (!player1.CanSB(count) || _inputManager.YesNo(player1, "Skip Soul Blast cost?"))
                    return;
            }
            List<int> cardsToSB = _inputManager.SelectFromList(player1, canSB, count, count, "Choose card(s) to Soul Blast.");
            player1.SoulBlast(cardsToSB);
        }

        public void CounterCharge(Player player1, List<Card> canCC, int count)
        {
            List<int> cardsToCharge = _inputManager.SelectFromList(player1, canCC, count, count, "Choose card(s) to flip up.");
            player1.CounterCharge(cardsToCharge);
        }

        public void SoulCharge(Player player1, Player player2, int count)
        {
            player1.SoulCharge(count);
        }

        public void Search(Player player1, Player player2, List<Card> canSearch)
        {
            List<int> cardsToSearch = _inputManager.SelectFromList(player1, canSearch, 1, 1, "Choose card(s) to search.");
            player1.Search(cardsToSearch);
        }

        public void Stand(Player player1, Player player2, List<Card> canStand, int count, bool select)
        {
            List<int> cardsToStand = _inputManager.SelectFromList(player1, canStand, count, count, "Choose card(s) to stand.");
            player1.Stand(cardsToStand);
            _playTimings.AddPlayTiming(Activation.OnStand, player1._playerID, player1.GetLastStood());
        }

        public void Stand(Player player1, Player player2, List<int> toStand)
        {
            player1.Stand(toStand);
            _playTimings.AddPlayTiming(Activation.OnStand, player1._playerID, player1.GetLastStood());
        }

        public void Rest(Player player1, Player player2, List<Card> canRest, int count, bool select)
        {
            List<int> cardsToRest = _inputManager.SelectFromList(player1, canRest, count, count, "Choose card(s) to rest.");
            player1.Rest(cardsToRest);
            player2.Rest(cardsToRest);
        }

        public void ChooseAddTempPower(Player player1, Player player2, List<Card> canAdd, int power, int count)
        {
            List<int> cardsToAddPower = _inputManager.SelectFromList(player1, canAdd, count, count, "Choose card(s) to give +" + power + " to.");
            player1.AddTempPower(cardsToAddPower, power);
            AddToChosen(cardsToAddPower);
        }

        public void ChooseAddBattleOnlyPower(Player player1, Player player2, List<Card> canAdd, int power, int count)
        {
            List<int> cardsToAddPower = _inputManager.SelectFromList(player1, canAdd, count, count, "Choose card(s) to give +" + power + " to.");
            foreach (int tempID in cardsToAddPower)
                player1.AddTempPower(tempID, power, true);
            AddToChosen(cardsToAddPower);
        }

        public void ChooseAddCritical(Player player1, Player player2, List<Card> canAdd, int critical, int count)
        {
            List<int> cardsToAddCritical = _inputManager.SelectFromList(player1, canAdd, count, count, "Choose card(s) to give +" + critical + " critical to.");
            foreach (int tempID in cardsToAddCritical)
            {
                player1.AddCritical(tempID, critical);
            }
            AddToChosen(cardsToAddCritical);
        }

        public void ChooseAddDrive(Player player1, Player player2, List<Card> canAdd, int drive, int count)
        {
            List<int> cardsToAddDrive = _inputManager.SelectFromList(player1, canAdd, count, count, "Choose card(s) to give +" + drive + " drive to.");
            foreach (int tempID in cardsToAddDrive)
            {
                player1.AddDrive(tempID, drive);
            }
            AddToChosen(cardsToAddDrive);
        }

        public void ChooseAddBattleOnlyCritical(Player player1, List<Card> canAdd, int critical, int count, int min)
        {
            List<int> cardsToAddCritical = _inputManager.SelectFromList(player1, canAdd, count, min, "Choose card(s) to give +" + critical + " critical to.");
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
            List<int> cardsToAddToHand = _inputManager.SelectFromList(player1, canAddToHand, count, min, "Choose card(s) to add to hand.");
            player1.AddToHand(cardsToAddToHand);
        }

        public void AddToSoul(Player player1, Player player2, List<Card> canAddToHand, int count, int min)
        {
            List<int> cardsToAddToSoul = _inputManager.SelectFromList(player1, canAddToHand, count, min, "Choose card(s) to add to soul.");
            player1.AddToSoul(cardsToAddToSoul);
        }

        public void SelectCardToRetire(Player player1, Player player2, List<Card> canRetire, int count, bool upto)
        {
            List<int> cardsToRetire = _inputManager.SelectFromList(player1, canRetire, count, count, "Choose card(s) to retire.");
            player1.Retire(cardsToRetire);
            if (player1.PlayerRetired() && _currentAbility.GetPlayer1() == player1 && _currentAbility.IsPayingCost())
            {
                _playTimings.AddPlayTiming(Activation.OnRetiredForPlayerCost, player1._playerID, player1.GetLastPlayerRetired());
            }
            if (player1.EnemyRetired())
                _playTimings.AddPlayTiming(Activation.OnEnemyRetired, 0, null);
            AddToChosen(cardsToRetire);
        }

        public void Retire(Player player1, Player player2, List<int> toRetire)
        {
            player1.Retire(toRetire);
            if (player1.EnemyRetired())
                _playTimings.AddPlayTiming(Activation.OnEnemyRetired, 0, null);
        }

        public void ChooseSendToBottom(Player player1, Player player2, List<Card> canSend, int max, int min)
        {
            List<int> cardsToSend = _inputManager.SelectFromList(player1, canSend, max, min, "Choose card(s) to send to bottom of deck.");
            player1.SendToDeck(cardsToSend, true);
            AddToChosen(cardsToSend);
        }

        public void ChooseSendToTop(Player player1, Player player2, List<Card> canSend, int max, int min)
        {
            List<int> cardsToSend = _inputManager.SelectFromList(player1, canSend, max, min, "Choose card(s) to send to top of deck.");
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
            player1.PlayOrder(tempID);
        }

        public void ChooseReveal(Player player1, Player player2, List<Card> canReveal, int max, int min)
        {
            List<int> cardsToReveal = _inputManager.SelectFromList(player1, canReveal, max, min, "Choose card(s) to reveal.");
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
            List<int> cardsToImprison = _inputManager.SelectFromList(player1, cardsToSelect, count, min, "Choose card(s) to imprison.");
            player1.Imprison(cardsToImprison);
            AddToChosen(cardsToImprison);
        }

        public void EnemyChooseImprison(Player player1, Player player2, List<Card> cardsToSelect, int count, int min)
        {
            List<int> cardsToImprison = _inputManager.SelectFromList(player2, cardsToSelect, count, min, "Choose card(s) to imprison.");
            player1.Imprison(cardsToImprison);
        }

        public void ChooseMoveEnemyRearguard(Player player1, List<Card> cardsToSelect, List<int> availableCircles)
        {
            bool swapped = false;
            if (_inputManager._player1._playerID != player1._playerID)
            {
                _inputManager.SwapPlayers();
                swapped = true;
            }
            int selection = _inputManager.SelectFromList(player1, cardsToSelect, 1, 1, "Choose unit to switch places.")[0];
            int selection2 = _inputManager.SelectCircle(player1, availableCircles);
            player1.MoveRearguardSpecific(selection, selection2);
            if (swapped)
                _inputManager.SwapPlayers();
        }

        public void Heal(Player player1)
        {
            int max;
            int selection;
            if (player1.CanHeal())
            {
                max = player1.PrintDamageZone();
                selection = SelectPrompt(max);
                player1.Heal(selection);
            }
        }

        public List<int> SelectCards(Player player1, List<Card> cardsToSelect, int max, int min, string query)
        {
            List<int> selectedCards = _inputManager.SelectFromList(player1, cardsToSelect, max, min, query);
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
            bool swapped = false;
            if (_inputManager._player1._playerID != player1._playerID)
            {
                _inputManager.SwapPlayers();
                swapped = true;
            }
            List<int> newOrder = _inputManager.ChooseOrder(player1, cardsToRearrange);
            player1.RearrangeOnTop(newOrder);
            if (swapped)
                _inputManager.SwapPlayers();
        }

        public void RearrangeOnBottom(Player player1, List<Card> cardsToRearrange)
        {
            bool swapped = false;
            if (_inputManager._player1._playerID != player1._playerID)
            {
                _inputManager.SwapPlayers();
                swapped = true;
            }
            List<int> newOrder = _inputManager.ChooseOrder(player1, cardsToRearrange);
            player1.RearrangeOnBottom(newOrder);
            if (swapped)
                _inputManager.SwapPlayers();
        }

        public void ChooseBind(Player player1, List<Card> cards, int max, int min)
        {
            List<int> toBind = _inputManager.SelectFromList(player1, cards, max, min, "Choose card(s) to bind.");
            player1.Bind(toBind);
        }

        public void DisplayCards(Player player1, List<Card> cardsToDisplay)
        {
            _inputManager.DisplayCards(cardsToDisplay);
        }

        public bool PlayerMainPhase(int playerID)
        {
            if (_phase == Phase.Main && playerID == actingPlayer._playerID)
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
            _playTimings.AddPlayTiming(Activation.OnChosen, 0, null);
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

        public List<Card> GetList(int playTiming, int playerID, int timingCount)
        {
            return _playTimings.GetList(playTiming, playerID, timingCount);
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

    public class PlayTimings
    {
        Dictionary<int, PlayTiming> _playTimings = new Dictionary<int, PlayTiming>();
        TimingCountLists _player1TimingCountLists = new TimingCountLists();
        TimingCountLists _player2TimingCountLists = new TimingCountLists();

        public void AddPlayTiming(int activation, int playerID, List<Card> cards)
        {
            AddPlayTiming(activation, playerID, cards, false);
        }

        public void AddPlayTiming(int activation, int playerID, List<Card> cards, bool append)
        {
            if (!_playTimings.ContainsKey(activation))
                _playTimings[activation] = new PlayTiming(activation);
            if (!append || _playTimings[activation].GetTotal() == 0)
                _playTimings[activation].AddPlayTiming();
            if (cards != null)
            {
                if (playerID == 1)
                    _player1TimingCountLists.AddList(activation, _playTimings[activation].GetTotal(), cards);
                else if (playerID == 2)
                    _player2TimingCountLists.AddList(activation, _playTimings[activation].GetTotal(), cards);
            }
        }

        public PlayTiming GetPlayTiming(int activation)
        {
            return _playTimings[activation];
        }

        public List<int> GetActivations()
        {
            List<int> activations = new List<int>();
            foreach (int key in _playTimings.Keys)
                activations.Add(key);
            return activations;
        }

        public void AddActivatedAbility(Ability ability, int count)
        {
            foreach (int activation in ability.GetActivations)
                _playTimings[activation].AddActivatedAbility(ability, count);
        }

        public void Reset()
        {
            foreach (int key in _playTimings.Keys)
            {
                _playTimings[key].Reset();
            }
            _playTimings.Clear();
            _player1TimingCountLists.Reset();
            _player2TimingCountLists.Reset();
        }

        public int CurrentCount(int playTiming)
        {
            if (_playTimings.ContainsKey(playTiming))
                return (_playTimings[playTiming].GetTotal());
            return 0;
        }

        public List<Card> GetList(int playTiming, int playerID, int timingCount)
        {
            if (playerID == 1)
                return _player1TimingCountLists.GetList(playTiming, timingCount);
            else
                return _player2TimingCountLists.GetList(playTiming, timingCount);
        }
    }

    public class PlayTiming
    {
        int _activation;
        int _number = 0;
        Dictionary<int, List<Ability>> _activatedAbilities = new Dictionary<int, List<Ability>>();
        
        public PlayTiming(int activation)
        {
            _activation = activation;
        }

        public void AddPlayTiming()
        {
            _number++;
            _activatedAbilities[_number] = new List<Ability>();
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
    }

    public class TimingCountLists
    {
        Dictionary<int, Dictionary<int, List<Card>>> timingCountLists = new Dictionary<int, Dictionary<int, List<Card>>>();

        public void AddList(int playTiming, int timingCount, List<Card> cards)
        {
            if (!timingCountLists.ContainsKey(playTiming))
                timingCountLists[playTiming] = new Dictionary<int, List<Card>>();
            if (!timingCountLists[playTiming].ContainsKey(timingCount))
                timingCountLists[playTiming][timingCount] = new List<Card>();
            timingCountLists[playTiming][timingCount].AddRange(cards);
        }

        public List<Card> GetList(int playTiming, int timingCount)
        {
            if (timingCountLists.ContainsKey(playTiming) && timingCountLists[playTiming].Keys.Count > 0)
            {
                if (timingCount == -1)
                    return timingCountLists[playTiming][timingCountLists[playTiming].Keys.Max()];
                if (timingCountLists[playTiming].ContainsKey(timingCount))
                    return timingCountLists[playTiming][timingCount];
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
