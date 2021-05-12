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
        public int _turn;
        public int _phase;
        public bool _gameOver = false;
        public LuaInterpreter luaInterpreter;
        public InputManager _inputManager;
        public List<Card> _deck1;
        public List<Card> _deck2;
        public EventHandler<CardEventArgs> OnDrawPhase;
        public EventHandler<CardEventArgs> OnRidePhase;
        public EventHandler<CardEventArgs> OnMainPhase;
        public EventHandler<CardEventArgs> OnBattlePhase;
        public EventHandler<CardEventArgs> OnEndPhase;

        public bool Initialize(List<Card> Deck1, List<Card> Deck2, InputManager inputManager, string luaPath)
        {
            List<Card> deck1;
            List<Card> deck2;
            deck1 = Deck1;
            deck2 = Deck2;
            foreach (Card card in deck2)
                card.tempID += 50;
            _player1 = new Player();
            _player2 = new Player();
            _player1.Initialize(deck1, deck2, _player2, 1);
            _player2.Initialize(deck2, deck1, _player1, 2);
            inputManager.Initialize(_player1, _player2);
            _inputManager = inputManager;
            luaInterpreter = new LuaInterpreter(luaPath);
            ShuffleDeck(_player1, _player2);
            ShuffleDeck(_player2, _player1);
            return true;
        }

        public void StartFight()
        {
            Player player1;
            Player player2;
            CardEventArgs args;
            int RPS1 = 0, RPS2 = 0;
            int first = 0;
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
            while(true)
            {
                player1.StandAll();
                player2.EnemyStandAll();
                if (_turn > 1)
                {
                    Console.WriteLine("----------\nSTAND AND DRAW");
                    Draw(player1, player2, 1);
                }
                Console.WriteLine("----------\nRIDE PHASE");
                RidePhaseMenu(player1, player2);
                Console.WriteLine("----------\nMAIN PHASE");
                MainPhaseMenu(player1, player2);
                if (_turn > 1)
                {
                    Console.WriteLine("----------\nBATTLE PHASE");
                    BattlePhaseMenu(player1, player2);
                }
                if (_gameOver)
                {
                    if (player1 == _player1)
                        Console.WriteLine("----------\nPLAYER 1 WINS.");
                    else
                        Console.WriteLine("PLAYER 2 WINS.");
                    return;
                }
                Console.WriteLine("----------\nEND PHASE");
                if (OnEndPhase != null)
                {
                    args = new CardEventArgs();
                    OnEndPhase(this, args);
                }
                player1.ResetPower();
                player2.ResetPower();
                _turn++;
                player1.IncrementTurn();
                player2.IncrementTurn();
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
            player2.EnemyDraw(count);
        }

        public void Mulligan(Player player1, Player player2)
        {
            List<int> selection;
            for (int i = 0; i < 2; i++)
            {
                selection = _inputManager.SelectCardsToMulligan();
                player1.MulliganCards(selection, true);
                player2.MulliganCards(selection, false);
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
                else
                    player1.DisplayRearguard(selection);
            }
        }

        public void ShuffleDeck(Player player1, Player player2)
        {
            player1.ShuffleDeck();
            player2.EnemyShuffleDeck();
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
            while (true)
            {
                if (CanRideFromRideDeck || CanRideFromHand)
                {
                    Console.WriteLine("----------\nDo you wish to Ride?");
                    if (_inputManager.YesNo(PromptType.Ride))
                    {
                        if (CanRideFromRideDeck)
                        {
                            Console.WriteLine("----------\nRide from Ride Deck?");
                            if (_inputManager.YesNo(PromptType.RideFromRideDeck))
                            {
                                Discard(player1, player2, 1);
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
            }
        }

        public void Ride(Player player1, Player player2, int location, int selection)
        {
            List<Effect> effects;
            player1.Ride(location, selection, C.Player);
            player2.Ride(location, selection, C.Enemy);
            effects = luaInterpreter.CheckEffects(EffectType.OnRide, player1, player2);
            ActivateEffects(effects);
        }

        public void Discard(Player player1, Player player2, int count)
        {
            List<int> list;
            list = _inputManager.SelectCardsFromHand(count);
            player1.Discard(list, C.Player);
            player2.Discard(list, C.Enemy);
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
                        location = _inputManager.SelectCallLocation();
                        Call(player1, player2, location, input);
                    }
                    else
                        Console.WriteLine("No Rearguards can be called.");
                }
                else if (selection == 4)
                {
                    if (player1.CanMoveRearguard())
                    {
                        selection = _inputManager.SelectRearguardColumn();
                        MoveRearguard(player1, player2, selection);
                    }
                    else
                        Console.WriteLine("No Rearguards can be moved.");
                }
                else if (selection == 5)
                {
                    if (player1.CanACT())
                    {
                        max = player1.PrintACTableCards();
                        selection = SelectPrompt(max);
                        ActivateACT(player1, selection);
                    }
                    else
                        Console.WriteLine("No abilites can be activated.");
                }
                else if (selection == 6)
                {
                    if (player1.CanActivateOrder())
                    {
                        max = player1.PrintAvailableOrders();
                        selection = SelectPrompt(max);
                        ActivateOrder(player1, player2, selection);
                    }
                    else
                        Console.WriteLine("No Orders can be activated.");
                }
                else
                    break;
            }
        }

        public void Call(Player player1, Player player2, int location, int selection)
        {
            //List<Effect> Effects;            
            player1.Call(location, selection, C.Player);
            player2.Call(location, selection, C.Enemy);
            //Effects = player1.CheckForCallEffects();
        }

        public void MoveRearguard(Player player1, Player player2, int selection)
        {
            player1.MoveRearguard(selection);
            player2.EnemyMoveRearguard(selection);
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
                        selection = _inputManager.SelectAttackingUnit();
                        target = _inputManager.SelectUnitToAttack();
                        Attack(player1, player2, selection, target);
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

        public void Attack(Player player1, Player player2, int attacker, int target)
        {
            int selection;
            int max;
            int drive;
            int critical;
            List<Effect> effects = null;
            player1.InitiateAttack(attacker, target, C.Player);
            player2.InitiateAttack(attacker, target, C.Enemy);
            effects = luaInterpreter.CheckEffects(EffectType.OnAttack, player1, player2);
            ActivateEffects(effects);

            if (player1.CanBeBoosted())
            {
                Console.WriteLine("----------\nBoost?\n" +
                    "1. Yes\n" +
                    "2. No");
                if (_inputManager.YesNo(PromptType.Boost))
                {
                    player1.Boost();
                    player2.EnemyBoost();
                }
            }
            if (player1 == _player1)
                Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
            else
                Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
            _inputManager.SwapPlayers();
            while (true)
            {
                player2.PrintEnemyAttack();
                if (player2.CanGuard())
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
                        selection = _inputManager.SelectCardToGuardWith();
                        player2.Guard(selection, C.Player);
                        player1.Guard(selection, C.Enemy);
                    }
                    else
                        break;
                }
             }
            if (player1.AttackerIsVanguard())
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
            if (player2.AttackHits() && player2.TargetIsVanguard())
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
                player2.RetireGC();
                player1.EnemyRetireGC();
            }
            if (player1 == _player1)
                Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 1.");
            else
                Console.WriteLine("----------\nSWITCHING CONTROL TO PLAYER 2.");
            _inputManager.SwapPlayers();
            player1.ResetBattleOnlyPower();
            player2.ResetBattleOnlyPower();
            player1.EndAttack();
            player2.EndAttack();
        }

        public void TriggerCheck(Player player1, Player player2, bool drivecheck)
        {
            int check;
            int selection;
            int max;
            if (drivecheck)
                Console.WriteLine("----------\nPerforming Drive Check.");
            else
                Console.WriteLine("----------\nPerforming Damage Check.");
            check = player1.TriggerCheck();
            player2.EnemyTriggerCheck();
            if (check != Trigger.NotTrigger && check != Trigger.Over) 
            {
                Console.WriteLine("----------\nChoose unit to give +10000 power to.");
                selection = _inputManager.SelectActiveUnit(PromptType.AddPower, 10000);
                player1.AddPower(selection, 10000);
                player2.AddPower(selection, 10000);
            }
            if (check == Trigger.Critical) 
            {
                Console.WriteLine("----------\nChoose unit to give Critical to.");
                selection = _inputManager.SelectActiveUnit(PromptType.AddCritical, 1);
                player1.AddCritical(selection, 1);
                player2.AddCritical(selection, 1);
            }
            else if (check == Trigger.Stand) //STAND TRIGGER (no stand triggers rn, will fix later if needed)
            {
                Console.WriteLine("----------\nChoose unit to Stand.");
                selection = _inputManager.SelectActiveUnit(PromptType.Stand, 0);
                player1.Stand(selection);
                player2.Stand(selection);
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
                    player1.Heal(selection, C.Player);
                    player2.Heal(selection, C.Enemy);
                }
            }
            else if (check == Trigger.Front) //FRONT TRIGGER
            {
                max = player1.PrintActiveUnits();
                for (int i = 0; i < max; i++)
                {
                    player1.AddPower(i, 10000);
                    player2.AddPower(i, 10000);
                }
            }
            else if (check == Trigger.Over) //OVER TRIGGER
            {
                Draw(player1, player2, 1);
                selection = _inputManager.SelectActiveUnit(PromptType.AddPower, 100000000);
                player1.AddPower(selection, 100000000);
                player2.AddPower(selection, 100000000);
                if (drivecheck)
                {
                    
                }
                player1.RemoveTrigger();
                player2.RemoveEnemyTrigger();
                return;
            }
            if (drivecheck)
            {
                player1.AddTriggerToHand();
                player2.EnemyAddTriggerToHand();
            }
            else
            {
                player1.TakeDamage();
                player2.EnemyTakeDamage();
            }
        }
        public void ActivateEffects(List<Effect> effects)
        {
            int i = 0;
            int selection = 0;
            int location = 0;
            bool mandatory = true;

            while (effects.Count > 0)
            {
                if (effects[0].needsPrompt == false)
                {
                    effects[0].Activate(0);
                    effects.RemoveAt(0);
                    continue;
                }
                selection = _inputManager.SelectEffect(effects);
                if (!mandatory && selection == effects.Count)
                    return;
                else
                {
                    Console.WriteLine("----------\n" + effects[selection].Name + "'s effect activates!");
                    if (effects[selection].isSuperiorCall)
                    {
                        location = _inputManager.SelectCallLocation();
                        effects[selection].Activate(location);
                    }
                    else
                        effects[selection].Activate(0);
                    effects.RemoveAt(selection);
                }
            }
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

        public bool CheckForPrompts(List<Effect> effects)
        {
            foreach (Effect effect in effects)
            {
                if (effect.needsPrompt)
                    return true;
            }
            return false;
        }
    }
    public class C
    {
        public const bool Player = true;
        public const bool Enemy = false;
    }
}
