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
        protected Player actingPlayer;
        public int _turn;
        protected int _phase = Phase.Stand;
        public bool _gameOver = false;
        public bool _activatedOrder = false;
        public LuaInterpreter luaInterpreter;
        protected InputManager _inputManager;
        public List<Card> _deck1;
        public List<Card> _deck2;
        public Abilities _abilities;
        public List<Ability> _abilityQueue = new List<Ability>();
        public List<Ability> _alchemagicQueue = new List<Ability>();
        public List<Ability> _activatedAbilities = new List<Ability>();
        public List<int> _currentActivations = new List<int>();
        public Dictionary<int, List<int>> _chosen = new Dictionary<int, List<int>>();
        public Ability _currentAbility = null;
        public EventHandler<CardEventArgs> OnDrawPhase;
        public EventHandler<CardEventArgs> OnRidePhase;
        public EventHandler<CardEventArgs> OnMainPhase;
        public EventHandler<CardEventArgs> OnBattlePhase;
        public EventHandler<CardEventArgs> OnEndPhase;
        public EventHandler<CardEventArgs> OnAttackHits;

        public bool Initialize(List<Card> Deck1, List<Card> Deck2, List<Card> tokens, InputManager inputManager, string luaPath)
        {
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
            Draw(player1, player2, 5);
            Draw(player2, player1, 5);
            if (player1 == _player1)
                Console.WriteLine("----------\nPLAYER 1 GOES FIRST.");
            else
            {
                Console.WriteLine("----------\nPLAYER 2 GOES FIRST.");
                _inputManager.SwapPlayers();
            }
            Mulligan(player1, player2);
            Console.WriteLine("----------\nSTAND UP! VANGUARD!");
            player1.StandUpVanguard();
            player2.StandUpVanguard();
            _turn = 1;
            _phase = 0;
            TriggerCheck(player1, player2, false);
            TriggerCheck(player1, player2, false);
            TriggerCheck(player2, player1, false);
            TriggerCheck(player2, player1, false);
            //TriggerCheck(player1, player2, false);
            //TriggerCheck(player2, player1, false);
            player1.SoulCharge(10);
            player2.SoulCharge(10);
            //player1.AbyssalDarkNight();
            //player2.AbyssalDarkNight();
            while (true)
            {
                actingPlayer = player1;
                _phase = Phase.Stand;
                player1.StandAll();
                if (_turn > 1)
                {
                    Console.WriteLine("----------\nSTAND AND DRAW");
                    _phase = Phase.Draw;
                    Draw(player1, player2, 1);
                }
                Console.WriteLine("----------\nRIDE PHASE");
                _phase = Phase.Ride;
                RidePhaseMenu(player1, player2);
                Console.WriteLine("----------\nMAIN PHASE");
                _phase = Phase.Main;
                MainPhaseMenu(player1, player2);
                if (_turn > 1)
                {
                    Console.WriteLine("----------\nBATTLE PHASE");
                    _phase = Phase.Battle;
                    BattlePhaseMenu(player1, player2);
                }
                if (_gameOver)
                {
                    if (player1 == _player1)
                        Console.WriteLine("----------\nPLAYER 1 WINS.");
                    else
                        Console.WriteLine("PLAYER 2 WINS.");
                    input = Console.ReadLine();
                    return;
                }
                Console.WriteLine("----------\nEND PHASE");
                _phase = Phase.End;
                if (OnEndPhase != null)
                {
                    args = new CardEventArgs();
                    OnEndPhase(this, args);
                }
                player1.EndTurn();
                player2.EndTurn();
                player1.IncrementTurn();
                _abilities.EndTurn();
                _activatedOrder = false;
                _turn++;
                if (player1 == _player1)
                {
                    Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                    player1 = _player2;
                    player2 = _player1;
                    _inputManager.SwapPlayers();
                }
                else
                {
                    Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
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
                Console.WriteLine("----------\nNew hand: ");
                player1.PrintHand();
                if (player1 == _player1)
                {
                    Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                    player1 = _player2;
                    player2 = _player1;
                    _inputManager.SwapPlayers();
                }
                else
                {
                    Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
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
                Console.WriteLine(max + ". Go back.");
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
            bool CanRideFromRideDeck = player1.CanRideFromRideDeck();
            bool CanRideFromHand = player1.CanRideFromHand();
            CardEventArgs args;
            if (OnRidePhase != null)
            {
                args = new CardEventArgs();
                OnRidePhase(this, args);
            }
            AddAbilitiesToQueue(player1, player2, Activation.OnRidePhase);
            ActivateAbilities(player1, player2, Activation.OnRidePhase);
            while (true)
            {
                if (CanRideFromRideDeck || CanRideFromHand)
                {
                    Console.WriteLine("----------\nDo you wish to Ride?");
                    if (_inputManager.YesNo(player1, PromptType.Ride))
                    {
                        if (CanRideFromRideDeck)
                        {
                            Console.WriteLine("----------\nRide from Ride Deck?");
                            if (_inputManager.YesNo(player1, PromptType.RideFromRideDeck))
                            {
                                Discard(player1, player2, player1.GetHand(), 1, 1);
                                input = _inputManager.SelectCardFromRideDeck();
                                Ride(player1, player2, 0, input);
                                break;
                            }
                        }
                        if (CanRideFromHand)
                        {
                            input = _inputManager.SelectCardFromHandToRide();
                            Ride(player1, player2, 1, input);
                            break;
                        }
                    }
                    else
                        break;
                }
                else
                    break;
            }
        }

        public void Ride(Player player1, Player player2, int location, int selection)
        {
            player1.Ride(location, selection);
            AddAbilitiesToQueue(player1, player2, Activation.OnRide);
            AddAbilitiesToQueue(player1, player2, Activation.PlacedOnVC);
            ActivateAbilities(player1, player2, Activation.OnRide);
        }

        public void Discard(Player player1, Player player2, List<Card> cardsToSelect, int max, int min)
        {
            List<int> cardsToDiscard = _inputManager.SelectFromList(player1, cardsToSelect, max, min, "Choose card(s) to discard.");
            player1.Discard(cardsToDiscard);
            AddAbilitiesToQueue(player1, player2, Activation.OnDiscard);
        }

        public void MainPhaseMenu(Player player1, Player player2)
        {
            int selection;
            int location;
            int max;
            int input;
            CardEventArgs args;
            if (OnMainPhase != null)
            {
                args = new CardEventArgs();
                OnMainPhase(this, args);
            }
            while (true)
            {
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
                        location = _inputManager.SelectCallLocation(player1, "Select circle to call to.", new List<int>(), null);
                        if (_abilities.CanOverDress(input, location))
                        {
                            Console.WriteLine("Perform overDress?");
                            if (_inputManager.YesNo(player1, PromptType.OverDress))
                            {
                                Call(player1, player2, location, input, true);
                                continue;
                            }
                        }
                        Call(player1, player2, location, input, false);
                    }
                    else
                        Console.WriteLine("No Rearguards can be called.");
                }
                else if (selection == 4)
                {
                    AddAbilitiesToQueue(player1, player2, Activation.OnCallFromPrison);
                    ActivateAbilities(player1, player2, Activation.OnCallFromPrison);
                }
                else if (selection == 5)
                {
                    if (player1.CanMoveRearguard())
                    {
                        selection = _inputManager.SelectRearguardColumn();
                        MoveRearguard(player1, player2, selection);
                    }
                    else
                        Console.WriteLine("No Rearguards can be moved.");
                }
                else if (selection == 6) //ACT
                {
                    AddAbilitiesToQueue(player1, player2, Activation.OnACT);
                    ActivateAbilities(player1, player1, Activation.OnACT);
                }
                else if (selection == 7) //Order
                {
                    if (!player1.OrderPlayed())
                    {
                        AddAbilitiesToQueue(player1, player2, Activation.OnOrder);
                        ActivateAbilities(player1, player1, Activation.OnOrder);
                    }
                    else
                        Console.WriteLine("Already activated order this turn.");
                }
                else if (selection == 8)
                    break;
                else if (selection == 9) //call specific RG (for use outside of console only)
                {
                    Call(player1, player2, _inputManager.intlist_input[0], _inputManager.intlist_input[1], false);
                }
                else if (selection == 10) //move specific column (for use outside of console only)
                {
                    MoveRearguard(player1, player2, _inputManager.intlist_input[1]);
                }
            }
        }

        public void Call(Player player1, Player player2, int location, int selection, bool overDress)
        {
            player1.Call(location, selection, overDress);
            _abilities.ResetActivation(selection);
            AddAbilitiesToQueue(player1, player2, Activation.PlacedOnRC);
            AddAbilitiesToQueue(player1, player2, Activation.PlacedOnRCFromHand);
            ActivateAbilities(player1, player2, Activation.PlacedOnRC);
        }

        public void SuperiorCall(Player player1, Player player2, List<Card> cardsToSelect, int max, int min, List<int> circles, bool overDress, bool standing)
        {
            int selection = 0;
            List<int> selections;
            int selectedCircle = 0;
            List<int> selectedCircles = new List<int>();
            List<int> canSelect = new List<int>();
            int sc = 0;
            for (int i = 0; i < max; i++)
            {
                if (i == min)
                {
                    selections = _inputManager.SelectFromList(player1, cardsToSelect, 1, 0, "Choose card to Call.");
                    if (selections.Count == 0)
                        break;
                    else
                        selection = selections[0];
                }
                else
                    selection = _inputManager.SelectFromList(player1, cardsToSelect, 1, 1, "Choose card to Call.")[0];
                foreach (Card card in cardsToSelect)
                {
                    if (card.tempID == selection)
                    {
                        cardsToSelect.Remove(card);
                        break;
                    }
                }
                if (circles != null)
                    canSelect.AddRange(circles);
                if (canSelect.Count == 1)
                    selectedCircle = canSelect[0];
                else
                    selectedCircle = _inputManager.SelectCallLocation(player1, "Choose RC.", selectedCircles, canSelect);
                selectedCircles.Add(selectedCircle);
                sc = player1.SuperiorCall(selectedCircle, selection, overDress, standing);
                _abilities.ResetActivation(selection);
            }
            AddAbilitiesToQueue(player1, player2, Activation.PlacedOnRC);
            if (sc == 1)
                AddAbilitiesToQueue(player1, player2, Activation.PlacedOnRCFromHand);
        }

        public void MoveRearguard(Player player1, Player player2, int selection)
        {
            player1.MoveRearguard(selection);
        }

        public void ActivateACT(Player player1, int selection)
        {
            player1.ActivateACT(selection);
        }

        public void ActivateOrder(Player player1, Player player2, int selection)
        {
            player1.ActivateOrder(selection);
            player2.EnemyActivateOrder(selection);
        }

        public void BattlePhaseMenu(Player player1, Player player2)
        {
            int selection;
            int target;
            CardEventArgs args;
            if (OnBattlePhase != null)
            {
                args = new CardEventArgs();
                OnBattlePhase(this, args);
            }
            while (true)
            {
                if (player1.CanAttack())
                {
                    selection = _inputManager.SelectBattlePhaseAction();
                    if (selection == 1)
                        BrowseHand(player1);
                    else if (selection == 2)
                        BrowseField(player1);
                    else if (selection == 3)
                    {
                        Attack(player1, player2);
                        if (_gameOver)
                            return;
                    }
                    else if (selection == 4)
                        break;
                    else if (selection == 5) //for use outside of console only
                    {
                        //Attack(player1, player2, _inputManager.intlist_input[0], _inputManager.intlist_input[1]);
                    }
                    else
                        break;
                }
                else
                    break;
            }
        }

        public void Attack(Player player1, Player player2)
        {
            int selection;
            int selection2;
            int drive;
            int critical;
            int attacker;
            int target;
            List<int> selections;
            selection = _inputManager.SelectAttackingUnit();
            player1.SetAttacker(selection);
            target = _inputManager.SelectUnitToAttack();
            player1.InitiateAttack(target);

            if (player1.CanBeBoosted())
            {
                Console.WriteLine("----------\nBoost?");
                if (_inputManager.YesNo(player1, PromptType.Boost))
                {
                    player1.Boost();
                }
            }
            AddAbilitiesToQueue(player1, player2, Activation.OnAttack);
            ActivateAbilities(player1, player2, Activation.OnAttack);
            if (player1 == _player1)
                Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
            else
                Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
            _inputManager.SwapPlayers();
            while (true)
            {
                if (!player1.StillAttacking())
                    break;
                player2.PrintEnemyAttack();
                if (true)
                {
                    selection = _inputManager.SelectGuardPhaseAction();
                    if (selection == 1)
                        BrowseHand(player2);
                    else if (selection == 2)
                        BrowseField(player2);
                    else if (selection == 3)
                        player2.PrintShield();
                    else if (selection == 4)
                    {
                        if (player2.GetAttackedCards().Count > 1)
                            selection2 = _inputManager.SelectCardToGuard();
                        else
                            selection2 = -1;
                        if (player2.MustGuardWithTwo())
                            selections = _inputManager.SelectFromList(player2, player2.GetGuardableCards(), 2, 2, "Choose card(s) to guard with.");
                        else
                            selections = _inputManager.SelectFromList(player2, player2.GetGuardableCards(), 1, 1, "Choose card(s) to guard with.");
                        player2.Guard(selections, selection2);
                        AddAbilitiesToQueue(player2, player1, Activation.PlacedOnGC);
                        ActivateAbilities(player2, player1, Activation.PlacedOnGC);
                    }
                    else if (selection == 5)
                    {
                        if (!player2.OrderPlayed())
                        {
                            AddAbilitiesToQueue(player2, player1, Activation.OnBlitzOrder);
                            ActivateAbilities(player2, player1, Activation.OnBlitzOrder);
                        }
                        else
                            Console.WriteLine("Already activated order this turn.");
                    }
                    else if (selection == 6) // end guard
                    {
                        break;
                    }
                    //else if (selection == 7) //guard with specific card (for use outside of console only)
                    //{
                    //    player2.Guard(_inputManager.intlist_input[0], -1);
                    //    AddAbilitiesToQueue(player2, player1, Activation.PlacedOnGC);
                    //    ActivateAbilities(player2, player1, Activation.PlacedOnGC);
                    //}
                }
             }
            if (player1.StillAttacking() && (player1.AttackerIsVanguard() || player1.RearguardCanDriveCheck()))
            {
                if (player1 == _player1)
                    Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                else
                    Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                _inputManager.SwapPlayers();
                drive = player1.Drive();
                for (int i = 0; i < drive; i++)
                    TriggerCheck(player1, player2, true);
                if (player1 == _player1)
                    Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                else
                    Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                _inputManager.SwapPlayers();
            }
            if (player1.StillAttacking() && player2.AttackHits())
            {
                if (player2.TargetIsVanguard(C.Player))
                {
                    critical = player1.Critical();
                    for (int i = 0; i < critical; i++)
                    {
                        //if (player1.AttackerIsVanguard())
                        //{
                        //    if (player1 == _player1)
                        //        Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
                        //    else
                        //        Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
                        //}
                        TriggerCheck(player2, player1, false);
                        if (player2.Damage() == 6)
                        {
                            _gameOver = true;
                            return;
                        }
                    }
                    AddAbilitiesToQueue(player1, player2, Activation.OnAttackHitsVanguard);
                }
                player2.RetireAttackedUnit();
                AddAbilitiesToQueue(player1, player2, Activation.OnAttackHits);
                _inputManager.SwapPlayers();
                ActivateAbilities(player1, player2, Activation.OnAttackHits);
                _inputManager.SwapPlayers();
            }
            player2.RetireGC();
            AddAbilitiesToQueue(player1, player2, Activation.OnBattleEnds);
            ActivateAbilities(player1, player2, Activation.OnBattleEnds);
            if (player1 == _player1)
                Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
            else
                Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
            _inputManager.SwapPlayers();
            player1.EndAttack();
            player2.EndAttack();
        }

        public void TriggerCheck(Player player1, Player player2, bool drivecheck)
        {
            int check;
            int selection;
            int max;
            List<Card> cards;
            if (drivecheck)
                Console.WriteLine("----------\nPerforming Drive Check.");
            else
                Console.WriteLine("----------\nPerforming Damage Check.");
            check = player1.TriggerCheck(drivecheck);
            if (check != Trigger.NotTrigger && check != Trigger.Over && check != Trigger.Front) 
            {
                Console.WriteLine("----------\nChoose unit to give +10000 power to.");
                selection = _inputManager.SelectActiveUnit(PromptType.AddPower, 10000);
                player1.AddTempPower(selection, 10000, false);
            }
            if (check == Trigger.Critical) 
            {
                Console.WriteLine("----------\nChoose unit to give Critical to.");
                selection = _inputManager.SelectActiveUnit(PromptType.AddCritical, 1);
                player1.AddCritical(selection, 1);
            }
            else if (check == Trigger.Stand) //STAND TRIGGER (no stand triggers rn, will fix later if needed)
            {
                Console.WriteLine("----------\nChoose unit to Stand.");
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
                    max = player1.PrintDamageZone();
                    selection = SelectPrompt(max);
                    player1.Heal(selection);
                }
            }
            else if (check == Trigger.Front) //FRONT TRIGGER
            {
                cards = player1.GetPlayerFrontRow();
                foreach (Card card in cards)
                {
                    player1.AddTempPower(card.tempID, 10000, false);
                }
            }
            else if (check == Trigger.Over) //OVER TRIGGER
            {
                Draw(player1, player2, 1);
                Console.WriteLine("Choose unit to give 1000000000 power to.");
                selection = _inputManager.SelectActiveUnit(PromptType.AddPower, 100000000);
                player1.AddTempPower(selection, 100000000, false);
                if (drivecheck)
                {
                    AddAbilitiesToQueue(player1, player2, Activation.OnOverTrigger);
                    ActivateAbilities(player1, player2, Activation.OnOverTrigger);
                }
                return;
            }
            if (drivecheck)
            {
                AddAbilitiesToQueue(player1, player2, Activation.OnDriveCheck);
                ActivateAbilities(player1, player2, Activation.OnDriveCheck);
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

        public void TargetImmunity(Player player1, Player player2, int tempID)
        {
            _player1.TargetImmunity(tempID);
            _player2.TargetImmunity(tempID);
        }

        public void AddAbilitiesToQueue(Player player1, Player player2, int activation)
        {
            List<Card> cards = new List<Card>();
            List<Ability> abilities = new List<Ability>();
            if (activation == Activation.OnOrder || activation == Activation.OnBlitzOrder)
            {
                abilities.AddRange(_abilities.GetAbilities(activation, player1.GetOrderableCards()));
            }
            else if (activation == Activation.OnCallFromPrison)
            {
                cards.Add(player2.GetPlayerPrison());
                abilities.AddRange(_abilities.GetAbilities(activation, cards));
                cards.Clear();
            }
            else
            {
                cards.Add(player1.GetTrigger(C.Player));
                abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetActiveUnits()));
                abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetGC()));
                abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player1.GetOrderZone()));
                abilities.AddRange(_abilities.GetAbilities(activation, cards));
                abilities.AddRange(_abilities.GetAbilities(activation, player1.GetActiveUnits()));
                abilities.AddRange(_abilities.GetAbilities(activation, player2.GetActiveUnits()));
                abilities.AddRange(_abilities.GetAbilities(activation, player1.GetHand()));
                abilities.AddRange(_abilities.GetAbilities(activation, player1.GetDrop()));
                abilities.AddRange(_abilities.GetAbilities(activation, player1.GetSoul()));
                abilities.AddRange(_abilities.GetAbilities(activation, player1.GetGC()));
                abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetActiveUnits()));
                abilities.AddRange(_abilities.GetAbilities(Activation.Cont, player2.GetGC()));
            }
            foreach (Ability ability in _activatedAbilities)
            {
                while (abilities.Contains(ability))
                    abilities.Remove(ability);
            }
            foreach (Ability ability in abilities)
            {
                if (!_abilityQueue.Contains(ability))
                    _abilityQueue.Add(ability);
            }
            if (activation != Activation.OnACT && activation != Activation.OnOrder && activation != Activation.OnBlitzOrder && activation != Activation.OnCallFromPrison && !_currentActivations.Contains(activation))
                _currentActivations.Add(activation);
        }

        public void ActivateAbilities(Player player1, Player player2, int activation)
        {
            int selection;
            int amSelection = -1;
            bool proceedWithAlchemagic = false;
            List<Ability> abilities = _abilityQueue;
            List<Ability> tempQueue = new List<Ability>();
            List<Ability> continuousAbilities;
            int ThenID = 0;
            Ability ThenAbility = null;
            int ThenNum = 0;
            while (abilities.Count > 0)
            {
                continuousAbilities = ContinuousAbilitiesInQueue();
                foreach (Ability ability in continuousAbilities)
                {
                    ability.Activate();
                    abilities.Remove(ability);
                }
                if (abilities.Count == 0)
                    break;
                selection = _inputManager.SelectAbility(abilities);
                if (selection == abilities.Count)
                    break;
                else
                {
                    _currentAbility = abilities[selection];
                    Console.WriteLine("----------\n" + abilities[selection].Name + "'s effect activates!");
                    if (activation == Activation.OnOrder || activation == Activation.OnBlitzOrder)
                    {
                        if (player1.CanAlchemagic())
                        {
                            _alchemagicQueue.AddRange(_abilities.GetAlchemagicableCards(player1, abilities[selection].GetID()));
                            if (_alchemagicQueue.Count >= 1 && (!abilities[selection].CheckConditionWithoutAlchemagic() || _inputManager.YesNo(player1, "Use Alchemagic?")))
                            {
                                proceedWithAlchemagic = true;
                            }
                        }
                        if (proceedWithAlchemagic)
                        {
                            player1.EnterAlchemagic();
                            amSelection = _inputManager.SelectAbility(_alchemagicQueue);
                            PlayOrder(player1, player2, abilities[selection].GetID(), false);
                            PlayOrder(player1, player2, _alchemagicQueue[amSelection].GetID(), true);
                            abilities[selection].PayCost();
                            _alchemagicQueue[amSelection].PayCost();
                            abilities[selection].Activate();
                            _alchemagicQueue[amSelection].Activate();
                            player1.EndAlchemagic();
                            player1.EndOrder();
                        }
                        else
                        {
                            PlayOrder(player1, player2, abilities[selection].GetID(), false);
                            abilities[selection].PayCost();
                            abilities[selection].Activate();
                            player1.EndOrder();
                        }
                    }
                    else
                    {
                        abilities[selection].PayCost();
                        ThenNum = abilities[selection].Activate();
                        _activatedAbilities.Add(abilities[selection]);
                        ThenID = abilities[selection].GetID();
                    }
                    abilities.Clear();
                    if (ThenNum > 0)
                    {
                        ThenAbility = _abilities.GetAbility(ThenID, ThenNum);
                        if (!ThenAbility.CheckCondition())
                            continue;
                        if (ThenAbility.isMandatory)
                            ThenAbility.Activate();
                        else
                        {
                            Console.WriteLine("Activate ability?");
                            if (_inputManager.YesNo(player1, 0))
                                ThenAbility.Activate();
                        }
                    }
                    if (activation == Activation.OnOrder || activation == Activation.OnBlitzOrder)
                        AddAbilitiesToQueue(player1, player2, Activation.OnOrderPlayed);
                    activation = 0;
                    foreach (int item in _currentActivations)
                        AddAbilitiesToQueue(player1, player2, item);
                }
            }
            abilities.Clear();
            _activatedAbilities.Clear();
            _currentActivations.Clear();
            _chosen.Clear();
            player1.AllAbilitiesResolved();
        }

        public List<Ability> ContinuousAbilitiesInQueue()
        {
            List<Ability> continuousAbilities = new List<Ability>();
            foreach (Ability ability in _abilityQueue)
            {
                if (ability.isContinuous)
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
            if (player1.IsAlchemagic())
            {
                count -= player1.AlchemagicFreeCBAvailable();
                min -= player1.AlchemagicFreeCBAvailable();
                if (count < 0)
                    count = 0;
                if (min < 0)
                    min = 0;
                player1.ResetAlchemagicFreeCB();
            }
            List<int> cardsToCB = _inputManager.SelectFromList(player1, canCB, count, min, "Choose card(s) to Counter Blast.");
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
            player2.Stand(cardsToStand);
            AddAbilitiesToQueue(player1, player2, Activation.OnStand);
        }

        public void Stand(Player player1, Player player2, List<int> toStand)
        {
            player1.Stand(toStand);
            AddAbilitiesToQueue(player1, player2, Activation.OnStand);
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

        public void RemoveSkill(Player player1, Player player2, Card card, int skill)
        {
            _player1.RemoveSkill(card.tempID, skill);
            _player2.RemoveSkill(card.tempID, skill);
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
            if (player1.EnemyRetired())
                AddAbilitiesToQueue(player1, player2, Activation.OnEnemyRetired);
            AddToChosen(cardsToRetire);
        }

        public void Retire(Player player1, Player player2, List<int> toRetire)
        {
            player1.Retire(toRetire);
            if (player1.EnemyRetired())
                AddAbilitiesToQueue(player1, player2, Activation.OnEnemyRetired);
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
            Console.WriteLine("----------\nEntered Final Rush!");
        }

        public void PlayOrder(Player player1, Player player2, int tempID, bool alchemagic)
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
            bool swapped = false;
            if (_inputManager._player1._playerID != player1._playerID)
            {
                _inputManager.SwapPlayers();
                swapped = true;
            }
            List<int> cardsToImprison = _inputManager.SelectFromList(player1, cardsToSelect, count, min, "Choose card(s) to imprison.");
            player1.Imprison(cardsToImprison);
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

        public List<int> SelectCards(Player player1, List<Card> cardsToSelect, int max, int min)
        {
            List<int> selectedCards = _inputManager.SelectFromList(player1, cardsToSelect, max, min, "Select card(s).");
            AddToChosen(selectedCards);
            return selectedCards;
        }

        public int SelectOption(Player player, string[] list)
        {
            return _inputManager.SelectOption(player, list);
        }

        public bool YesNo(Player player, string query)
        {
            return _inputManager.YesNo(player, query);
        }

        public void RearrangeOnTop(Player player1, List<Card> cardsToRearrange)
        {
            List<int> newOrder = _inputManager.ChooseOrder(player1, cardsToRearrange);
            _player1.RearrangeOnTop(newOrder);
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
            AddAbilitiesToQueue(_currentAbility.GetPlayer1(), _currentAbility.GetPlayer2(), Activation.OnChosen);
        }

        public bool AlchemagicableCardsAvailable(Player player, int tempID)
        {
            List<Ability> alchemagicable = _abilities.GetAlchemagicableCards(player, tempID);
            if (alchemagicable.Count > 0)
                return true;
            return false;
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
}
