using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using System.IO;

namespace VanguardEngine
{
    public class LuaInterpreter
    {
        public string luaPath;
        public CardFight cardFight;
        int _abilityID = 0;
        DynValue l;
        DynValue a;
        DynValue q;
        DynValue t;
        DynValue o;
        DynValue FL;
        DynValue p;
        DynValue u;
        DynValue tt;
        DynValue s;
        DynValue ps;
        DynValue cs;


        public LuaInterpreter(string path, CardFight cf)
        {
            luaPath = path;
            cardFight = cf;
            UserData.RegisterType<Ability>();
            UserData.RegisterType<Location>();
            UserData.RegisterType<Activation>();
            UserData.RegisterType<Query>();
            UserData.RegisterType<AbilityType>();
            UserData.RegisterType<Other>();
            UserData.RegisterType<FL>();
            UserData.RegisterType<Property>();
            UserData.RegisterType<UnitType>();
            UserData.RegisterType<Trigger>();
            UserData.RegisterType<Skill>();
            UserData.RegisterType<PlayerState>();
            UserData.RegisterType<CardState>();
            l = UserData.Create(new Location());
            a = UserData.Create(new Activation());
            q = UserData.Create(new Query());
            t = UserData.Create(new AbilityType());
            o = UserData.Create(new Other());
            FL = UserData.Create(new FL());
            p = UserData.Create(new Property());
            u = UserData.Create(new UnitType());
            tt = UserData.Create(new Trigger());
            s = UserData.Create(new Skill());
            ps = UserData.Create(new PlayerState());
            cs = UserData.Create(new CardState());
        }

        public List<Ability> GetAbilities(Card card, Player player1, Player player2, bool player)
        {
            List<Ability> abilities = new List<Ability>();
            Ability ability;
            Script script;
            bool success;
            string filePath = card.id;
            filePath = luaPath + Path.DirectorySeparatorChar + filePath + ".lua";
            if (!File.Exists(filePath))
            {
                Log.WriteLine("no lua script for " + filePath);
                return abilities;
            }
            else
                Log.WriteLine("loading " + filePath);
            Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.FileSystemScriptLoader();
            Script tempScript = new Script();
            tempScript.DoFile(filePath);
            DynValue numberOfAbilities = tempScript.Call(tempScript.Globals["NumberOfAbilities"]);
            for (int i = 0; i < numberOfAbilities.Number; i++)
            {
                script = new Script();
                DynValue obj;
                script.Globals.Set("l", l);
                script.Globals.Set("a", a);
                script.Globals.Set("q", q);
                script.Globals.Set("t", t);
                script.Globals.Set("o", o);
                script.Globals.Set("FL", FL);
                script.Globals.Set("p", p);
                script.Globals.Set("u", u);
                script.Globals.Set("tt", tt);
                script.Globals.Set("s", s);
                script.Globals.Set("ps", ps);
                script.Globals.Set("cs", cs);
                script.DoFile(filePath);
                ability = new Ability(player1, player2, cardFight, card, _abilityID++);
                success = ability.StoreAbility(script, i + 1, player);
                if (success)
                {
                    obj = UserData.Create(ability);
                    script.Globals.Set("obj", obj);
                    abilities.Add(ability);
                }
            }
            return abilities;
        }
    }

    public class Abilities
    {
        List<Ability>[] _abilities;

        public Abilities()
        {
            _abilities = new List<Ability>[200];
            for (int i = 0; i < _abilities.Length; i++)
            {
                _abilities[i] = new List<Ability>();
            }
        }

        public void AddAbilities(int placement, List<Ability> ability)
        {
            _abilities[placement].AddRange(ability);
        }

        public Ability GetAbility(int tempID, int activationNumber)
        {
            return _abilities[tempID][activationNumber - 1];
        }

        public List<Ability> GetAbilities(int activation, List<Card> cards, int timingCount)
        {
            List<Ability> abilities = new List<Ability>();
            foreach (Card card in cards)
            {
                if (card == null)
                    continue;
                foreach (Ability ability in _abilities[card.tempID])
                {
                    ability.SetTimingCount(timingCount);
                    if (ability.GetActivation == activation && ability.CheckCondition())
                    {
                        abilities.Add(ability);
                    }
                }
            }
            return abilities;
        }

        public void EndTurn()
        {
            foreach (List<Ability> abilities in _abilities)
            {
                if (abilities != null)
                {
                    foreach (Ability ability in abilities)
                        ability.ResetActivation();
                }
            }
        }

        public void ResetActivation(int tempID)
        {
            if (_abilities[tempID] != null)
            {
                foreach (Ability ability in _abilities[tempID])
                {
                    if (!ability.isHardOncePerTurn)
                        ability.ResetActivation();
                }
            }
        }

        public bool CanOverDress(int tempID, int circle)
        {
            foreach (Ability ability in _abilities[tempID])
            {
                if (ability.GetActivation == Activation.OverDress)
                {
                    if (ability.CanOverDress(tempID, circle))
                        return true;
                }
            }
            return false;
        }

        public List<Ability> GetAlchemagicableCards(Player player1, int tempID)
        {
            Ability fromHand = GetAbility(tempID, 1);
            Ability fromDrop;
            int SB = 0;
            int CB = 0;
            int Retire = 0;
            List<Card> drop = player1.GetDrop();
            List<Card> damage = player1.GetDamageZone();
            List<Ability> alchemagicable = new List<Ability>();
            foreach (Card card in drop)
            {
                if (card.orderType == OrderType.Normal && ((player1.CanAlchemagicDiff() && card.name != player1.GetCard(fromHand.GetID()).name) || (player1.CanAlchemagicSame() && card.name == player1.GetCard(fromHand.GetID()).name)))
                {
                    fromDrop = GetAbility(card.tempID, 1);
                    SB = fromHand.GetSB() + fromDrop.GetSB();
                    CB = fromHand.GetCB() + fromDrop.GetCB();
                    Retire = fromHand.GetRetire() + fromDrop.GetRetire();
                    if (player1.AlchemagicFreeSBAvailable())
                        SB = 0;
                    if (player1.CanSB(SB) && player1.CanCB(CB) && player1.CanRetire(Retire))
                        alchemagicable.Add(fromDrop);
                }
            }
            return alchemagicable;
        }
    }

    public class Ability
    {
        Player _player1;
        Player _player2;
        CardFight _cardFight;
        Card _card;
        bool _isMandatory = false;
        bool _hasPrompt = false;
        bool _hardOncePerTurn = false;
        bool _oncePerTurn = false;
        Dictionary<int, int> _costs = new Dictionary<int, int>();
        string _description = "";
        bool _forEnemy = false;
        bool _isSuperiorCall = false;
        bool _activated = false;
        bool _withAlchemagic = true;
        bool _payingCost = false;
        int _activation;
        List<int> _location = new List<int>();
        int _abilityType;
        int _abilityNumber;
        int _abilityID;
        int _timingCount = 0;
        List<int> _overDressParams = new List<int>();
        Script _script;
        DynValue _abilityActivate;
        DynValue _abilityCost;
        DynValue _checkCondition;
        DynValue _canFullyResolve;
        List<Param> _params = new List<Param>();
        List<Card> _selected = new List<Card>();

        public Ability(Player player1, Player player2, CardFight cardFight, Card card, int abilityID)
        {
            _player1 = player1;
            _player2 = player2;
            _cardFight = cardFight;
            _card = card;
            _abilityID = abilityID;
        }

        public Card GetCard()
        {
            return _card;
        }

        public void SetTimingCount(int timingCount)
        {
            _timingCount = timingCount;
        }

        public bool StoreAbility(Script script, int num, bool player)
        {
            _script = script;
            _abilityNumber = num;
            Param param;
            DynValue activationRequirement = script.Call(script.Globals["ActivationRequirement"], _abilityNumber);
            DynValue numOfParams = script.Call(script.Globals["NumberOfParams"]);
            DynValue returnedParam;
            for (int i = 0; i < numOfParams.Number; i++)
            {
                returnedParam = script.Call(script.Globals["GetParam"], i + 1);
                param = new Param();
                for (int j = 0; j < returnedParam.Tuple.Length; j++)
                {
                    if (returnedParam.Tuple[j].Number == Query.Count)
                    {
                        param.AddCount((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.Min)
                    {
                        param.AddMin((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.Location)
                    {
                        param.AddLocation((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.Grade)
                    {
                        param.AddGrade((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.Name)
                    {
                        param.AddName(returnedParam.Tuple[j + 1].String);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.UnitType)
                    {
                        param.AddUnitType((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.Trigger)
                    {
                        param.AddTriggerType((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.Other)
                    {
                        param.AddOther((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.FL)
                    {
                        param.AddFL((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.Column)
                    {
                        param.AddColumn((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.NameContains)
                    {
                        param.AddNameContains((string)returnedParam.Tuple[j + 1].String);
                        j++;
                    }
                }
                _params.Add(param);
            }
            _activation = (int)activationRequirement.Tuple[0].Number;
            if (_activation == Activation.OverDress)
            {
                for (int i = 1; i < activationRequirement.Tuple.Length; i++)
                    _overDressParams.Add((int)activationRequirement.Tuple[i].Number);
                return true;
            }
            _abilityType = (int)activationRequirement.Tuple[1].Number;
            for (int i = 2; i < activationRequirement.Tuple.Length; i++)
            {
                if ((int)activationRequirement.Tuple[i].Number == Property.HasPrompt)
                {
                    _hasPrompt = true;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.IsMandatory)
                {
                    _isMandatory = true;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.Description)
                {
                    _description = activationRequirement.Tuple[i + 1].String;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.ForEnemy)
                    _forEnemy = true;
                else if ((int)activationRequirement.Tuple[i].Number == Property.CB)
                {
                    _costs[Property.CB] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.SB)
                {
                    _costs[Property.SB] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.Retire)
                {
                    _costs[Property.Retire] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.Discard)
                {
                    _costs[Property.Discard] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.HardOncePerTurn)
                {
                    _hardOncePerTurn = true;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.OncePerTurn)
                {
                    _oncePerTurn = true;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.AddToSoul)
                {
                    _costs[Property.AddToSoul] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.Reveal)
                {
                    _costs[Property.Reveal] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
            }
            if (_abilityType == VanguardEngine.AbilityType.ACT || _abilityType == VanguardEngine.AbilityType.Order)
            {
                _isMandatory = false;
                _hasPrompt = true;
            }
            else if (_abilityType == VanguardEngine.AbilityType.Cont)
            {
                _isMandatory = true;
                _hasPrompt = false;
            }
            if (!_forEnemy && !player)
                return false;
            _checkCondition = script.Globals.Get("CheckCondition");
            _canFullyResolve = script.Globals.Get("CanFullyResolve");
            _abilityActivate = script.Globals.Get("Activate");
            return true;
        }

        public void setScript(Script script)
        {
            _script = script;
        }

        public void setAbility(DynValue abilityActivate)
        {
            _abilityActivate = abilityActivate;
        }

        public bool isMandatory
        {
            get => _isMandatory;
        }

        public bool isContinuous
        {
            get => !_hasPrompt;
        }

        public bool isHardOncePerTurn
        {
            get => _hardOncePerTurn;
        }

        public bool isSuperiorCall
        {
            get => _isSuperiorCall;
            set => _isSuperiorCall = value;
        }

        public int GetActivation
        {
            get => _activation;
        }

        public List<int> Locations
        {
            get => _location;
        }

        public string Name
        {
            get => _card.name;
        }

        public int AbilityType
        {
            get => _abilityType;
        }

        public bool IsPayingCost()
        {
            return _payingCost;
        }

        public int GetID()
        {
            return _card.tempID;
        }

        public Player GetPlayer1()
        {
            return _player1;
        }

        public Player GetPlayer2()
        {
            return _player2;
        }

        public int GetCount(int paramNum)
        {
            int i = paramNum - 1;
            if (_params.Count >= i)
            {
                if (_params[i].Counts.Count >= 1)
                    return _params[i].Counts[0];
            }
            if (_params[i].Mins.Count >= 1)
                return GetMin(paramNum);
            return -1;
        }

        public int GetMin(int paramNum)
        {
            int i = paramNum - 1;
            if (_params.Count >= i)
            {
                if (_params[i].Mins.Count >= 1)
                    return _params[i].Mins[0];
                else
                    return GetCount(paramNum);
            }
            return -1;
        }

        public bool GetOrLess(int paramNum)
        {
            int i = paramNum - 1;
            if (_params[i].Others.Contains(Other.OrLess))
                return true;
            return false;
        }

        public List<int> GetTempIDs(List<Card> cards)
        {
            List<int> tempIDs = new List<int>();
            foreach (Card card in cards)
            {
                tempIDs.Add(card.tempID);
            }
            return tempIDs;
        }

        public void ResetActivation()
        {
            _activated = false;
        }

        public void Test()
        {
            Log.WriteLine("test");
        }

        public int Activate()
        {
            _payingCost = false;
            DynValue Then = _script.Call(_abilityActivate, _abilityNumber);
            _activated = true;
            return (int)Then.Number;
        }

        public int ActivateAsGiven(Card card)
        {
            _payingCost = false;
            Card originalCard = _card;
            bool swapped = false;
            Player temp;
            if (card.originalOwner != _player1._playerID)
            {
                temp = _player1;
                _player1 = _player2;
                _player2 = temp;
                swapped = true;
            }
            _card = card;
            DynValue Then = _script.Call(_abilityActivate, _abilityNumber);
            _activated = true;
            _card = originalCard;
            if (swapped)
            {
                temp = _player1;
                _player1 = _player2;
                _player2 = temp;
            }
            _player1.UpdateRecordedValues();
            _player2.UpdateRecordedValues();
            return (int)Then.Number;
        }

        public void PayCost()
        {
            _payingCost = true;
            //_script.Call(_abilityCost, _abilityNumber);
            if (_costs.ContainsKey(Property.CB))
                CounterBlast(_costs[Property.CB]);
            if (_costs.ContainsKey(Property.SB))
                SoulBlast(_costs[Property.SB]);
            if (_costs.ContainsKey(Property.Discard))
                Discard(_costs[Property.Discard], _costs[Property.Discard]);
            if (_costs.ContainsKey(Property.Reveal))
                ChooseReveal(_costs[Property.Reveal]);
            if (_costs.ContainsKey(Property.AddToSoul))
                ChooseAddToSoul(_costs[Property.AddToSoul]);
        }

        public bool CanPayCost()
        {
            foreach (int key in _costs.Keys)
            {
                if (key == Property.CB && !CanCB(_costs[key]))
                    return false;
                else if (key == Property.SB && !CanSB(_costs[key]))
                    return false;
                else if (key == Property.Discard && !CanDiscard(_costs[key]))
                    return false;
                else if (key == Property.AddToSoul && !CanAddToSoul(_costs[key]))
                    return false;
                else if (key == Property.Reveal && !CanReveal(_costs[key]))
                    return false;
            }
            return true;
        }
        
        public bool CheckCondition()
        {
            if (!CanPayCost())
                return false;
            if (_oncePerTurn && _activated)
                return false;
            DynValue check = _script.Call(_checkCondition, _abilityNumber);
            if (check.Boolean)
                return true;
            return false;
        }

        public bool CheckConditionAsGiven(Card card)
        {
            if (!CanPayCost())
                return false;
            if (_oncePerTurn && _activated)
                return false;
            Card originalCard = _card;
            bool swapped = false;
            Player temp;
            if (card.originalOwner != _player1._playerID)
            {
                temp = _player1;
                _player1 = _player2;
                _player2 = temp;
                swapped = true;
            }
            _card = card;
            DynValue check = _script.Call(_checkCondition, _abilityNumber);
            _card = originalCard;
            if (swapped)
            {
                temp = _player1;
                _player1 = _player2;
                _player2 = temp;
            }
            if (check.Boolean)
                return true;
            return false;
        }

        public bool CheckConditionWithoutAlchemagic()
        {
            _withAlchemagic = false;
            bool condition = CheckCondition();
            _withAlchemagic = true;
            return condition;
        }

        public bool CanFullyResolve()
        {
            DynValue check = _script.Call(_canFullyResolve, _abilityNumber);
            if (check.Boolean)
                return true;
            return false;
        }

        public int GetCB()
        {
            if (_costs.ContainsKey(Property.CB))
                return _costs[Property.CB];
            else
                return 0;
        }

        public int GetSB()
        {
            if (_costs.ContainsKey(Property.SB))
                return _costs[Property.SB];
            else
                return 0;
        }

        public int GetRetire()
        {
            if (_costs.ContainsKey(Property.Retire))
                return _costs[Property.Retire];
            else
                return 0;
        }

        public bool NotActivatedYet()
        {
            return !_activated;
        }
        public bool IsRodeUponThisTurn()
        {
            foreach (Card card in _player1.GetRiddenOnThisTurn(_timingCount))
            {
                if (_card.tempID == card.tempID)
                    return true;
            }
            return false;
        }

        public bool WasRodeUponBy(string name)
        {
            return _player1.RodeUponBy(_timingCount, _card.tempID, name, false);
        }

        public bool WasRodeUponByNameContains(string name)
        {
            return _player1.RodeUponBy(_timingCount, _card.tempID, name, true);
        }

        public bool MyVanguardWasPlaced()
        {
            if (_player1.GetLastPlacedOnVC(_timingCount).Count > 0)
                return true;
            return false;
        }

        public bool HasCardInDeck(string name)
        {
            if (_player1.HasCardInDeck(name))
                return true;
            return false;
        }

        public int HandCount()
        {
            return _player1.GetHand().Count;
        }

        public bool DropHasGrade(int grade)
        {
            List<Card> drop = _player1.GetDrop();
            foreach (Card card in drop)
            {
                if (card.grade == grade)
                    return true;
            }
            return false;
        }

        public bool PersonaRode()
        {
            return _player1.PersonaRode();
        }

        public bool CanSuperiorCall(int paramNum)
        {
            if (Exists(paramNum))
                return true;
            return false;
        }

        public bool CanSuperiorCall(int paramNum, int circle)
        {
            List<Card> cards = ValidCards(paramNum);
            if (cards.Count >= _params[paramNum - 1].Counts[0])
                return true;
            return false;
        }

        public List<Card> ValidCards(int paramNum)
        {
            Param param = _params[paramNum - 1];
            List<Card> currentPool = new List<Card>();
            List<Card> newPool = new List<Card>();
            List<Card> cards;
            foreach (int location in param.Locations)
            {
                if (location == Location.Deck)
                    currentPool.AddRange(_player1.GetDeck());
                else if (location == Location.Drop || location == Location.PlayerDrop)
                    currentPool.AddRange(_player1.GetDrop());
                else if (location == Location.EnemyDrop)
                    currentPool.AddRange(_player2.GetDrop());
                else if (location == Location.PlayerHand)
                    currentPool.AddRange(_player1.GetHand());
                else if (location == Location.EnemyHand)
                    currentPool.AddRange(_player2.GetHand());
                else if (location == Location.Soul)
                    currentPool.AddRange(_player1.GetSoul());
                else if (location == Location.PlayerRC)
                {
                    currentPool.AddRange(_player1.GetRearguards(C.Player));
                }
                else if (location == Location.EnemyRC)
                {
                    currentPool.AddRange(_player1.GetRearguards(C.Enemy));
                }
                else if (location == Location.PlayerVC)
                    currentPool.Add(_player1.Vanguard());
                else if (location == Location.EnemyVC)
                    currentPool.Add(_player2.Vanguard());
                else if (location == Location.Damage)
                    currentPool.AddRange(_player1.GetDamageZone());
                else if (location == Location.GC)
                    currentPool.AddRange(_player1.GetGC());
                else if (location == Location.Revealed)
                    currentPool.AddRange(_player1.GetRevealed());
                else if (location == Location.RevealedTriggers)
                    currentPool.AddRange(_player1.GetRevealedTriggers());
                else if (location == Location.RevealedTrigger)
                {
                    if (_player1.GetRevealedTrigger() != null)
                        currentPool.Add(_player1.GetRevealedTrigger());
                }
                else if (location == Location.Selected)
                    currentPool.AddRange(_selected);
                else if (location == Location.BackRow)
                    currentPool.AddRange(_player1.GetBackRow());
                else if (location == Location.FrontRow)
                    currentPool.AddRange(_player1.GetPlayerFrontRow());
                else if (location == Location.PlayerPrisoners)
                    currentPool.AddRange(_player1.GetPlayerPrisoners());
                else if (location == Location.EnemyPrisoners)
                    currentPool.AddRange(_player1.GetEnemyPrisoners());
                else if (location == Location.Looking)
                    currentPool.AddRange(_player1.GetLooking());
                else if (location == Location.FrontRowEnemyRC)
                    currentPool.AddRange(_player1.GetEnemyFrontRowRearguards());
                else if (location == Location.Trigger)
                    currentPool.Add(_player1.GetTrigger(true));
                else if (location == Location.LastCalled)
                    currentPool.AddRange(_player1.GetLastPlacedOnRC(_timingCount));
                else if (location == Location.PlayerOrder)
                    currentPool.AddRange(_player1.GetPlayerOrder());
                else if (location == Location.PlayerUnits)
                {
                    currentPool.AddRange(_player1.GetRearguards(C.Player));
                    if (_player1.Vanguard() != null)
                        currentPool.Add(_player1.Vanguard());
                    currentPool.AddRange(_player1.GetGC());
                }
                else if (location == Location.Bind)
                    currentPool.AddRange(_player1.GetBind());
                else if (location == Location.PlayedOrder)
                {
                    if (_player1.GetPlayedOrder() != null)
                        currentPool.Add(_player1.GetPlayedOrder());
                }
                else if (location == Location.LastCalledFromPrison)
                    currentPool.AddRange(_player2.GetLastCalledFromPrison());
                else if (location == Location.LastStood)
                    currentPool.AddRange(_player1.GetLastStood(_timingCount));
                else if (location == Location.InFront)
                    currentPool.AddRange(_player1.GetInFront(_card.tempID));
                else if (location == Location.UnitsCalledThisTurn)
                    currentPool.AddRange(_player1.GetUnitsCalledThisTurn());
                else if (location == Location.UnitsCalledFromHandThisTurn)
                    currentPool.AddRange(_player1.GetUnitsCalledFromHandThisTurn());
            }
            if (currentPool.Count == 0)
            {
                if (param.Others.Contains(Other.This))
                    currentPool.Add(_player1.GetCard(_card.tempID));
                return currentPool;
            }
            if (param.FLs.Count > 0)
            {
                foreach (Card card in currentPool)
                {
                    foreach (int FL in param.FLs)
                    {
                        if (_player1.GetUnitAt(FL, true) != null && _player1.GetUnitAt(FL, true).tempID == card.tempID)
                        {
                            newPool.Add(card);
                            break;
                        }
                    }
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.Columns.Count > 0)
            {
                foreach (int column in param.Columns)
                {
                    cards = _player1.GetUnitsAtColumn(column);
                    foreach (Card card in currentPool)
                    {
                        if (cards.Contains(card))
                            newPool.Add(card);
                    }
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.Others.Count > 0)
            {
                foreach (int other in param.Others)
                {
                    if (other == Other.This)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.tempID == _card.tempID)
                            {
                                newPool.Add(card);
                                break;
                            }
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Unit)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.unitType >= UnitType.Normal)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    if (other == Other.Prison)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.orderType == OrderType.Prison)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.World)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.orderType == OrderType.World)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Standing)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (_player1.IsUpRight(card))
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Resting)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (!_player1.IsUpRight(card))
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.OverDress)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.overDress)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.CanChoose)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.originalOwner == _player1._playerID || !_player1.CardStates.HasState(card.tempID, CardState.Resist))
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Order)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.orderType >= OrderType.Normal)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.NormalOrder)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.orderType == OrderType.Normal)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.NotThis)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.tempID != _card.tempID)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.FaceUp)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (_player1.IsFaceUp(card))
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.FaceDown)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (!_player1.IsFaceUp(card))
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.NotAttackTarget)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (!_player1.AttackedUnits().Contains(card))
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Attacking)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (_player1.GetAttacker().tempID == card.tempID)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Token)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.unitType == UnitType.Token)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.SetOrder)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.orderType == OrderType.Set || card.orderType == OrderType.Prison || card.orderType == OrderType.World)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.InFront)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (_player1.IsInFront(card.tempID, _card.tempID))
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Boosting)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.tempID == _player1.Booster().tempID)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.SameColumn)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (_player1.GetUnitsAtColumn(_player1.GetColumn(_card.tempID)).Contains(card))
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                }
            }
            if (param.Names.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    if (param.Names.Contains(currentPool[i].name) && !newPool.Contains(currentPool[i]))
                        newPool.Add(currentPool[i]);
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.NameContains.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    foreach (string substring in param.NameContains)
                    {
                        if (currentPool[i].name.Contains(substring))
                            newPool.Add(currentPool[i]);
                    }
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.Grades.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    if ((param.Grades.Contains(currentPool[i].grade) || 
                        (param.Others.Contains(Other.GradeOrLess) && currentPool[i].grade <= param.Grades.Max()) && 
                        !newPool.Contains(currentPool[i])))
                        newPool.Add(currentPool[i]);
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.UnitTypes.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    if (param.UnitTypes.Contains(currentPool[i].unitType) && !newPool.Contains(currentPool[i]))
                        newPool.Add(currentPool[i]);
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.TriggerTypes.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    if (param.TriggerTypes.Contains(currentPool[i].trigger) && !newPool.Contains(currentPool[i]))
                        newPool.Add(currentPool[i]);
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            return currentPool;
        }

        public string GetSelectedName(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            return cards[0].name;
        }

        public int GetSelectedGrade(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            return cards[0].grade;
        }

        public int GetSelectedUnitType(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            return cards[0].unitType;
        }

        public bool CanOverDress(int tempID, int circle)
        {
            Card card = _player1.GetUnitAt(circle, false);
            if (card == null)
                return false;
            List<Card> cards;
            if (_overDressParams.Count == 0)
                return false;
            foreach (int overDressParam in _overDressParams)
            {
                cards = ValidCards(overDressParam);
                if (cards.Contains(card))
                    return true;
            }
            return false;
        }

        public bool LastPlacedOnGC()
        {
            return _player1.IsLastPlacedOnGC(_card.tempID, _timingCount);
        }

        public bool LastPutOnGC()
        {
            return _player1.IsLastPutOnGC(_card.tempID, _timingCount);
        }

        public bool IsIntercepting()
        {
            return _player1.IsIntercepting(_card.tempID);
        }

        public bool LastPlacedOnRC()
        {
            return _player1.IsLastPlacedOnRC(_card.tempID, _timingCount);
        }

        public bool LastPlacedOnRCFromHand()
        {
            return _player1.IsLastPlacedOnRCFromHand(_card.tempID, _timingCount);
        }

        public bool LastPlacedOnVC()
        {
            return _player1.IsLastPlacedOnVC(_card.tempID, _timingCount);
        }

        public bool LastDiscarded()
        {
            return _player1.IsLastDiscarded(_card.tempID, _timingCount);
        }

        public bool OnTriggerZone()
        {
            Card trigger = _player1.GetTrigger(C.Player);
            if (trigger != null && trigger.tempID == _card.tempID)
                return true;
            return false;
        }

        public bool TriggerRevealed()
        {
            Card trigger = _player1.GetTrigger(true);
            if (trigger.trigger != Trigger.NotTrigger)
                return true;
            return false;
        }

        public bool Exists(int paramNum)
        {
            int min = GetMin(paramNum);
            List<Card> cards = ValidCards(paramNum);
            bool orLess = GetOrLess(paramNum);
            if (cards.Count >= 0 && cards.Count >= min)
            {
                if (orLess && cards.Count > min)
                    return false;
                return true;
            }
            if (orLess && cards.Count == 0)
                return true;
            return false;
        }

        public bool ExistsAmong(int forEachNum, int customParamNum, int query)
        {
            List<Card> forEach = ValidCards(forEachNum);
            Param customParam = _params[customParamNum - 1];
            foreach (Card card in forEach)
            {
                if (query == Query.Name)
                    customParam.InjectName(card.name);
                if (Exists(customParamNum))
                    return true;
            }
            return false;
        }

        public bool OpenCirclesExist(int paramNum)
        {
            if (OpenCircles(paramNum).Count > 0)
                return true;
            return false;
        }

        public List<int> OpenCircles(int paramNum)
        {
            Param param = _params[paramNum - 1];
            List<int> openCircles;
            List<int> availableCircles = new List<int>();
            if (param.Locations.Contains(Location.FrontRowEnemyRC))
            {
                openCircles = _player1.GetOpenCircles(C.Enemy);
                if (openCircles.Contains(_player1.Convert(FL.EnemyFrontRight)))
                    availableCircles.Add(_player1.Convert(FL.EnemyFrontRight));
                if (openCircles.Contains(_player1.Convert(FL.EnemyFrontLeft)))
                    availableCircles.Add(_player1.Convert(FL.EnemyFrontLeft));
            }
            if (param.Locations.Contains(Location.PlayerRC))
            {
                openCircles = _player1.GetOpenCircles(C.Player);
                for (int i = _player1.Convert(FL.PlayerFrontLeft); i < _player1.Convert(FL.PlayerVanguard); i++)
                {
                    if (openCircles.Contains(i))
                        availableCircles.Add(i);
                }
            }
            if (param.Locations.Contains(Location.BackRow))
            {
                openCircles = _player1.GetOpenCircles(C.Player);
                if (openCircles.Contains(_player1.Convert(FL.PlayerBackCenter)))
                    availableCircles.Add(_player1.Convert(FL.PlayerBackCenter));
                if (openCircles.Contains(_player1.Convert(FL.PlayerBackLeft)))
                    availableCircles.Add(_player1.Convert(FL.PlayerBackLeft));
                if (openCircles.Contains(_player1.Convert(FL.PlayerBackRight)))
                    availableCircles.Add(_player1.Convert(FL.PlayerBackRight));
            }
            return availableCircles;
        }

        public void ChooseMoveEnemyRearguard(int paramNum, List<int> circles)
        {
            if (circles.Count == 0)
                return;
            List<Card> cards = ValidCards(paramNum);
            _cardFight.ChooseMoveEnemyRearguard(_player1, cards, circles);
        }

        public void ChooseGiveAbility(int paramNum, int activationNumber)
        {
            List<Card> cards = ValidCards(paramNum);
            List<int> tempIDs = _cardFight.SelectCards(_player1, cards, GetCount(paramNum), GetMin(paramNum), "Select card to give ability.");
            _player1.GiveAbility(tempIDs, _card.tempID, activationNumber);
        }

        public void GiveAbility(int paramNum, int activationNumber)
        {
            List<Card> cards = ValidCards(paramNum);
            List<int> tempIDs = new List<int>();
            foreach (Card card in cards)
                tempIDs.Add(card.tempID);
            _player1.GiveAbility(tempIDs, _card.tempID, activationNumber);
        }

        public void Inject(int paramNum, int query, string name)
        {
            Param customParam = _params[paramNum - 1];
            if (query == Query.Name)
                customParam.InjectName(name);
        }

        public void Inject(int paramNum, int query, int num)
        {
            Param customParam = _params[paramNum - 1];
            if (query == Query.Count)
                customParam.InjectCount(num);
            else if (query == Query.Column)
                customParam.InjectColumn(num);
            else if (query == Query.Grade)
                customParam.InjectGrade(num);
        }

        public bool OnGC()
        {
            List<Card> GC = _player1.GetGC();
            foreach (Card card in GC)
            {
                if (card.tempID == _card.tempID)
                    return true;
            }
            return false;
        }

        public bool CanAddPower(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            if (cards != null && cards.Count >= _params[paramNum - 1].Counts[0])
                return true;
            return false;
        }

        public bool CanCB(int count)
        {
            List<Card> damage = _player1.GetDamageZone();
            int faceupCards = 0;
            foreach (Card card in damage)
            {
                if (_player1.IsFaceUp(card))
                    faceupCards++;
            }
            if (_withAlchemagic && _card.orderType == OrderType.Normal && (_player1.CanAlchemagicDiff() || _player1.CanAlchemagicSame()) && _cardFight.AlchemagicableCardsAvailable(_player1, _card.tempID))
                faceupCards += _player1.AlchemagicFreeCBAvailable();
            if (faceupCards >= count)
                return true;
            return false;
        }

        public bool CanSB(int count)
        {
            if (_player1.IsAlchemagic() && _player1.AlchemagicFreeSBAvailable())
                return true;
            if (_player1.GetSoul().Count < count)
                return false;
            return true;
        }

        public bool CanSearch(int paramNum)
        {
            List<Card> canSearch = ValidCards(paramNum);
            if (canSearch != null)
                return true;
            return false;
        }

        public bool CanAddToHand(int paramNum)
        {
            List<Card> canAddToHand = ValidCards(paramNum);
            if (canAddToHand != null && canAddToHand.Count >= _params[paramNum - 1].Counts[0])
                return true;
            return false;
        }

        public bool CanAddToSoul(int paramNum)
        {
            List<Card> canAddToSoul = ValidCards(paramNum);
            if (canAddToSoul != null && canAddToSoul.Count >= GetCount(paramNum))
                return true;
            return false;
        }

        public bool CanStand(int paramNum)
        {
            List<Card> units = ValidCards(paramNum);
            int count = 0;
            foreach (Card card in units)
            {
                if (!_player1.IsUpRight(card))
                    count++;
            }
            if (count < _params[paramNum - 1].Counts[0])
                return false;
            return true;
        }

        public bool CanRetire(int paramNum)
        {
            List<Card> units = ValidCards(paramNum);
            if (units == null)
                return false;
            List<Card> canRetire = new List<Card>();
            int count = 0;
            foreach (Card card in units)
            {
                if (!(_player1.CardStates.HasState(card.tempID, CardState.Resist) && _player1.IsEnemy(card.tempID)))
                {
                    count++;
                    if (_player1.CanCountAsTwoRetires(card.tempID))
                        count++;
                }
            }
            if (count >= GetCount(paramNum) || count >= GetMin(paramNum))
                return true;
            return false;
        }

        public bool CanDiscard(int count)
        {
            List<Card> hand = _player1.GetHand();
            if (hand.Count < count)
                return false;
            return true;
        }

        public bool CanReveal(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            if (cards != null && cards.Count >= _params[paramNum - 1].Counts[0])
                return true;
            return false;
        }

        public bool IsAttackingUnit()
        {
            if (_player1.AttackingUnit() != null && _card.tempID == _player1.AttackingUnit().tempID)
                return true;
            return false;
        }

        public bool VanguardIsAttackingUnit()
        {
            if (_player1.AttackingUnit() != null && _player1.AttackingUnit().tempID == _player1.Vanguard().tempID)
                return true;
            return false;
        }

        public bool IsBooster()
        {
            if (_player1.Booster() != null && _card.tempID == _player1.Booster().tempID)
                return true;
            return false;
        }

        public bool IsVanguard()
        {
            if (_card.tempID == _player1.Vanguard().tempID)
                return true;
            return false;
        }

        public bool IsRearguard()
        {
            if (_player1.IsRearguard(_card.tempID))
                return true;
            return false;
        }

        public bool IsGuardian()
        {
            List<Card> units = _player1.GetGC();
            foreach (Card card in units)
            {
                if (card.tempID == _card.tempID)
                    return true;
            }
            return false;
        }

        public bool TargetIsPlayerVanguard()
        {
            if (_player1.TargetIsVanguard(C.Player))
                return true;
            return false;
        }

        public bool TargetIsEnemyVanguard()
        {
            if (_player1.TargetIsVanguard(C.Enemy))
                return true;
            return false;
        }

        public bool TargetIsEnemyRearguard()
        {
            if (_player1.TargetIsRearguard(C.Enemy))
                return true;
            return false;
        }

        public bool EnemyVanguardHit()
        {
            return _player2.VanguardHit(true);
        }

        public bool InOverDress()
        {
            Card card = _player1.GetCard(_card.tempID);
            if (card.overDress)
                return true;
            return false;
        }

        public bool InFinalRush()
        {
            return _player1.InFinalRush();
        }

        public bool IsDarkNight()
        {
            return _player1.IsDarkNight();
        }

        public bool IsAbyssalDarkNight()
        {
            return _player1.IsAbyssalDarkNight();
        }

        public bool VanguardIs(string name)
        {
            if (_player1.Vanguard().name == name)
                return true;
            return false;
        }

        public int VanguardGrade()
        {
            return _player1.Vanguard().grade;
        }

        public int EnemyVanguardGrade()
        {
            return _player2.Vanguard().grade;
        }

        public bool OpponentHasRearguards()
        {
            List<Card> Units = _player1.GetAllUnitsOnField();
            for (int i = FL.EnemyFrontLeft; i <= FL.EnemyVanguard - 1; i++)
            {
                if (Units[i] != null)
                    return true;
            }
            return false;
        }

        public bool IsPlayerTurn()
        {
            return _player1.IsPlayerTurn();
        }

        public bool StoodByCardEffect()
        {
            return _player1.StoodByCardEffect(_card.tempID, _timingCount);
        }

        public int AttackingUnitLocation()
        {
            return _player1.AttackingUnitLocation();
        }

        public int AttackingUnitID()
        {
            return _player1.AttackingUnitID();
        }

        public int Turn()
        {
            Log.WriteLine("turn: " + _player1.Turn);
            return _player1.Turn;
        }

        public void Draw(int count)
        {
            _player1.Draw(count);
        }

        public void Mill(int count)
        {
            _player1.Mill(count);
        }

        public void SuperiorCall(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            int count = GetCount(paramNum);
            int min = GetMin(paramNum);
            if (count == -1)
                count = cardsToSelect.Count;
            if (min == -1)
                min = 0;
            _cardFight.SuperiorCall(_player1, _player2, cardsToSelect, count, min, null, false, true);
        }

        public void SuperiorCall(int paramNum, int circle)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<int> circles = new List<int>();
            if (circle == FL.OpenCircle)
            {
                circles.AddRange(_player1.GetOpenCircles(C.Player));
                if (circles.Count == 0)
                    return;
            }
            if (circle == FL.BackRow)
            {
                circles.Add(_player1.Convert(FL.PlayerBackRight));
                circles.Add(_player1.Convert(FL.PlayerBackLeft));
                circles.Add(_player1.Convert(FL.PlayerBackCenter));
            }
            else
                circles.Add(_player1.Convert(circle));
            int count = GetCount(paramNum);
            int min = GetMin(paramNum);
            if (count == -1)
                count = cardsToSelect.Count;
            if (min == -1)
                min = 0;
            _cardFight.SuperiorCall(_player1, _player2, cardsToSelect, count, min, circles, false, true);
        }

        public void SuperiorCallAsRest(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            int count = GetCount(paramNum);
            int min = GetMin(paramNum);
            if (count == -1)
                count = cardsToSelect.Count;
            if (min == -1)
                min = 0;
            _cardFight.SuperiorCall(_player1, _player2, cardsToSelect, count, min, null, false, false);
        }

        public void SuperiorOverDress(int paramNum, int paramNum2)
        {
            List<Card> fromHand = ValidCards(paramNum);
            int circle = _player1.GetCircle(ValidCards(paramNum2)[0]);
            List<int> circles = new List<int>();
            circles[0] = circle;
            _cardFight.SuperiorCall(_player1, _player2, fromHand, 1, 1, circles, true, true);
        }

        public void CounterBlast(int count)
        {
            CounterBlast(count, count);
        }

        public void CounterBlast(int count, int min)
        {
            List<Card> canCB = new List<Card>();
            foreach (Card card in _player1.GetDamageZone())
            {
                if (_player1.IsFaceUp(card))
                    canCB.Add(card);
            }
            _cardFight.CounterBlast(_player1, _player2, canCB, count, min);
        }

        public int CBUsed()
        {
            return _player1.CBUsed();
        }

        public void SoulBlast(int count)
        {
            SoulBlast(count, count);
        }

        public void SoulBlast(int count, int min)
        {
            List<Card> canSB = _player1.GetSoul();
            _cardFight.SoulBlast(_player1, _player2, canSB, count);
        }

        public void CounterCharge(int count)
        {
            List<Card> damage = _player1.GetDamageZone();
            List<Card> canCC = new List<Card>();
            foreach (Card card in damage)
            {
                if (!_player1.IsFaceUp(card))
                    canCC.Add(card);
            }
            if (canCC.Count > 0)
                _cardFight.CounterCharge(_player1, canCC, count);
        }

        public void SoulCharge(int count)
        {
            _cardFight.SoulCharge(_player1, _player2, count);
        }

        public bool SoulChargedThisTurn()
        {
            return _player1.SoulChargedThisTurn();
        }

        public void Search(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            _cardFight.Search(_player1, _player2, cardsToSelect);
        }

        public void ChooseAddToHand(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            _cardFight.AddToHand(_player1, _player2, cardsToSelect, _params[paramNum - 1].Counts[0], _params[paramNum - 1].Counts[0]);
        }

        public void AddToHand(int paramNum)
        {
            List<Card> cardsToAdd = ValidCards(paramNum);
            List<int> IDs = new List<int>();
            foreach (Card card in cardsToAdd)
                IDs.Add(card.tempID);
            _player1.AddToHand(IDs);
        }

        public void ChooseAddToSoul(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            if (GetMin(paramNum) < 0)
                AddToSoul(paramNum);
            else
                _cardFight.AddToSoul(_player1, _player2, cardsToSelect, _params[paramNum - 1].Counts[0], _params[paramNum - 1].Counts[0]);
        }

        public void AddToSoul(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<int> cardsToSoul = new List<int>();
            foreach (Card card in cardsToSelect)
                cardsToSoul.Add(card.tempID);
            _player1.AddToSoul(cardsToSoul);
        }

        public void AddToEnemySoul(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<int> cardsToSoul = new List<int>();
            foreach (Card card in cardsToSelect)
                cardsToSoul.Add(card.tempID);
            _player2.AddToSoul(cardsToSoul);
        }

        public void AddToDrop(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<int> cardsToDrop = new List<int>();
            foreach (Card card in cardsToSelect)
                cardsToDrop.Add(card.tempID);
            _player1.AddToDrop(cardsToDrop);
        }

        public void ChooseAddToDrop(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<int> cardsToDrop = _cardFight.SelectCards(_player1, cardsToSelect, GetCount(paramNum), GetMin(paramNum), "Choose card(s) to send to drop.");
            _player1.AddToDrop(cardsToDrop);
        }

        public void AddToDamageZone(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<int> tempIDs = new List<int>();
            foreach (Card card in cardsToSelect)
            {
                tempIDs.Add(card.tempID);
            }
            _player1.AddToDamageZone(tempIDs);
        }

        public void Discard(int count)
        {
            Discard(count, count);
        }

        public void Discard(int count, int min)
        {
            List<Card> cardsToSelect = _player1.GetHand();
            _cardFight.Discard(_player1, _player2, cardsToSelect, count, min);
        }

        public bool WasRetiredForPlayerCost()
        {
            return _player1.WasRetiredForPlayerCost(_card.tempID, _timingCount);
        }

        public void CountsAsTwoRetires(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.CountsAsTwoRetires(card.tempID);
        }

        public void DoesNotCountAsTwoRetires(int paramNum)
        {

        }

        public void ChooseRetire(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<Card> canRetire = new List<Card>();
            foreach (Card card in cardsToSelect)
            {
                if (!_player1.CardStates.HasState(card.tempID, CardState.Resist))
                    canRetire.Add(card);
            }
            _cardFight.SelectCardToRetire(_player1, _player2, canRetire, _params[paramNum - 1].Counts[0], false); 
        }

        public void Bind(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            List<int> tempIDs = new List<int>();
            foreach (Card card in cards)
                tempIDs.Add(card.tempID);
            _player1.Bind(tempIDs);
        }

        public void ChooseBind(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.ChooseBind(_player1, cards, GetCount(paramNum), GetMin(paramNum));
        }

        public void Retire(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<int> canRetire = new List<int>();
            foreach (Card card in cardsToSelect)
            {
                canRetire.Add(card.tempID);
            }
            _cardFight.Retire(_player1, _player2, canRetire);
        }

        public void ChooseSendToBottom(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            int max = GetCount(paramNum);
            int min = GetMin(paramNum);
            _cardFight.ChooseSendToBottom(_player1, _player2, cardsToSelect, max, min);
        }
        
        public void SendToBottom(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            List<int> cardsToSend = new List<int>();
            foreach (Card card in cards)
            {
                cardsToSend.Add(card.tempID);
            }
            _player1.SendToDeck(cardsToSend, true);
        }

        public void ChooseSendToTop(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            int max = GetCount(paramNum);
            int min = GetMin(paramNum);
            _cardFight.ChooseSendToTop(_player1, _player2, cardsToSelect, max, min);
        }

        public void SendToTop(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            List<int> cardsToSend = new List<int>();
            foreach (Card card in cards)
            {
                cardsToSend.Add(card.tempID);
            }
            _player1.SendToDeck(cardsToSend, false);
        }

        public void ChooseStand(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<Card> canStand = new List<Card>();
            foreach (Card card in cardsToSelect)
            {
                if (!_player1.IsUpRight(card))
                    canStand.Add(card);
            }
            _cardFight.Stand(_player1, _player2, canStand, _params[paramNum - 1].Counts[0], true);
        }

        public void Stand(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<int> canStand = new List<int>();
            foreach (Card card in cardsToSelect)
            {
                if (!_player1.IsUpRight(card))
                    canStand.Add(card.tempID);
            }
            _cardFight.Stand(_player1, _player2, canStand);
        }

        public void ChooseRest(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<Card> canRest = new List<Card>();
            foreach (Card card in cardsToSelect)
            {
                if (_player1.IsUpRight(card))
                    canRest.Add(card);
            }
            _cardFight.Rest(_player1, _player2, canRest, GetCount(paramNum), true);
        }

        public void Rest(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            List<int> toRest = new List<int>();
            foreach (Card card in cards)
                toRest.Add(card.tempID);
            _player1.Rest(toRest);
        }

        public void Free(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            int count = GetCount(paramNum);
            int min = GetMin(paramNum);
            if (count == -1)
                count = cardsToSelect.Count;
            if (min == -1)
                min = 0;
            _cardFight.SuperiorCall(_player1, _player2, cardsToSelect, count, min, null, false, true);
        }

        public void ChooseReveal(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            _cardFight.ChooseReveal(_player1, _player2, cardsToSelect, _params[paramNum - 1].Counts[0], _params[paramNum - 1].Counts[0]);
        }

        public void RevealFromDeck(int count)
        {
            _cardFight.RevealFromDeck(_player1, _player2, count);
        }

        public void EndReveal()
        {
            _cardFight.EndReveal(_player1, _player2);
        }

        public void LookAtTopOfDeck(int count)
        {
            _player1.LookAtTopOfDeck(count);
        }

        public void RearrangeOnTop(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
;            _cardFight.RearrangeOnTop(_player1, cards);
        }

        public void RearrangeOnBottom(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
             _cardFight.RearrangeOnBottom(_player1, cards);
        }

        public void EnemyRearrangeOnBottom(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.RearrangeOnBottom(_player2, cards);
        }

        public void DisplayCards(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.DisplayCards(_player1, cards);
        }

        public int NumEnemyOpenCircles()
        {
            return _player1.NumEnemyOpenCircles();
        }

        public void RearguardDriveCheck()
        {
            _player1.RearguardDriveCheck();
        }

        public bool LastRevealedTrigger()
        {
            if (_player1.GetRevealedTrigger() != null && _card.tempID == _player1.GetRevealedTrigger().tempID)
                return true;
            return false;
        }

        public void FinalRush()
        {
            _cardFight.FinalRush(_player1, _player2);
        }

        public void PerfectGuard()
        {
            _cardFight.PerfectGuard(_player1, _player2);
        }

        public void HitImmunity(int paramNum, params int[] grades)
        {
            List<Card> cards = ValidCards(paramNum);
            List<int> tempIDs = new List<int>();
            foreach (Card card in cards)
                tempIDs.Add(card.tempID);
            _player1.HitImmunity(tempIDs, grades.ToList());
        }

        public void TargetImmunity(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _cardFight.Resist(_player1, _player2, card.tempID);
            }
        }

        public void Resist(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _cardFight.Resist(_player1, _player2, card.tempID);
            }
        }

        public void AddBonusDriveCheckPower(int power)
        {
            _player1.AddBonusDriveCheckPower(power);
        }

        public int PowerOfThisUnit()
        {
            return _player1.CalculatePowerOfUnit(_player1.GetCircle(_player1.GetCard(_card.tempID)));
        }

        public void DoublePower(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.DoublePower(card.tempID);
        }

        public void DoubleCritical(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.DoubleCritical(card.tempID);
        }

        public void ChooseAddTempPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.ChooseAddTempPower(_player1, _player2, cards, power, _params[paramNum - 1].Counts[0]);
        }

        public void ChooseAddBattleOnlyPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.ChooseAddBattleOnlyPower(_player1, _player2, cards, power, _params[paramNum - 1].Counts[0]);
        }

        public void SetAbilityPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.SetAbilityPower(card.tempID, power);
            }
        }

        public void SetAbilityShield(int paramNum, int shield)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.SetAbilityShield(card.tempID, shield);
            }
        }

        public void SetAbilityDrive(int paramNum, int drive)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.SetAbilityDrive(card.tempID, drive);
            }
        }

        public void SetAbilityCritical(int paramNum, int critical)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.SetAbilityCritical(card.tempID, critical);
            }
        }

        public void AddTempPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddTempPower(card.tempID, power, false);
            }
        }

        public void AddCirclePower(int location, int power)
        {
            _player1.AddCirclePower(location, power);
        }

        public void ChooseAddCritical(int paramNum, int critical)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.ChooseAddCritical(_player1, _player2, cards, critical, GetCount(paramNum));
        }

        public void AddCritical(int paramNum, int critical)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddCritical(card.tempID, critical);
            }
        }

        public void AddBattleOnlyCritical(int paramNum, int critical)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddBattleOnlyCritical(card.tempID, critical);
            }
        }

        public void ChooseAddBattleOnlyCritical(int paramNum, int critical)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.ChooseAddBattleOnlyCritical(_player1, cards, critical, GetCount(paramNum), GetMin(paramNum));
        }

        public void AddCircleCritical(int location, int critical)
        {
            _player1.AddCircleCritical(location, critical);
        }

        public void AddDrive(int paramNum, int drive)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddDrive(card.tempID, drive);
            }
        }

        public void ChooseAddDrive(int paramNum, int drive)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.ChooseAddDrive(_player1, _player2, cards, drive, GetCount(paramNum));
        }

        public void AddBattleOnlyPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddTempPower(card.tempID, power, true);
            }
        }

        public void AddTempShield(int paramNum, int shield)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddTempShield(card.tempID, shield);
            }
        }

        public void AddSkill(int paramNum, int skill)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _cardFight.AddSkill(_player1, _player2, card, skill);
            }
        }

        public void RemoveSkill(int paramNum, int skill)
        {

        }

        public void AllowFreeSwap()
        {
            _player1.AllowFreeSwap();
        }

        public void AllowAttack(int paramNum)
        {

        }

        public void DisableAttack(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.DisableAttack(card.tempID);
        }

        public void DisableMove(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.DisableMove(card.tempID);
        }

        public void AllowBackRowAttack(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.AllowBackRowAttack(card.tempID);
        }

        public void AllowAttackAllFrontRow(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.AllowAttackAllFrontRow(card.tempID);
        }

        public void DisableAttackAllFrontRow(int paramNum)
        {

        }

        public void AllowAttackingBackRow(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.AllowAttackingBackRow(card.tempID);
        }

        public void AllowColumnAttack(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.AllowColumnAttack(card.tempID);
        }

        public void DisableColumnAttack(int paramNum)
        {

        }

        public void AllowInterceptFromBackRow(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.AllowInterceptFromBackRow(card.tempID);
        }

        public void EnemyDisableIntercept()
        {
            _player2.DisableIntercept();
        }

        public void Heal()
        {
            _cardFight.Heal(_player1);
        }

        public bool PlayerMainPhase()
        {
            return _cardFight.PlayerMainPhase(_player1._playerID);
        }

        public bool EnemyRetiredThisTurn()
        {
            return _player1.EnemyRetiredThisTurn();
        }

        public bool PlayerRetiredThisTurn()
        {
            return _player1.PlayerRetiredThisTurn();
        }

        public bool ChosenByVanguard()
        {
            int vanguard = _player1.Vanguard().tempID;
            if (_cardFight._chosen.ContainsKey(vanguard) && _cardFight._chosen[vanguard].Contains(_card.tempID))
                return true;
            return false;
        }

        public void Select(int paramNum)
        {
            _selected.Clear();
            List<Card> cardsToSelect = ValidCards(paramNum);
            int min = GetMin(paramNum);
            if (min < 0)
                min = GetCount(paramNum);
            List<int> selectedCards = _cardFight.SelectCards(_player1, cardsToSelect, GetCount(paramNum), min, "Select card(s).");
            foreach (int tempID in selectedCards)
                _selected.Add(_player1.GetCard(tempID));
        }

        public void EndSelect()
        {
            _selected.Clear();
        }

        public int SelectColumn()
        {
            return _cardFight.SelectColumn(_player1);
        }

        public void SetPrison()
        {
            _player1.SetPrison();
        }

        public bool HasPrison()
        {
            return _player1.HasPrison();
        }

        public void ChooseImprison(int paramNum)
        {
            if (!_player1.HasPrison())
                return;
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<Card> cardsToImprison = new List<Card>();
            foreach (Card card in cardsToSelect)
            {
                if (!_player1.CardStates.HasState(card.tempID, CardState.Resist))
                    cardsToImprison.Add(card);
            }
            _cardFight.ChooseImprison(_player1, _player2, cardsToImprison, GetCount(paramNum), GetMin(paramNum));
        }

        public void EnemyChooseImprison(int paramNum)
        {
            if (!_player1.HasPrison())
                return;
            List<Card> cardsToSelect = ValidCards(paramNum);
            _cardFight.EnemyChooseImprison(_player1, _player2, cardsToSelect, GetCount(paramNum), GetMin(paramNum));
        }

        public void EnemyChooseRetire(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            _cardFight.SelectCardToRetire(_player2, _player1, cardsToSelect, GetCount(paramNum), false);
        }

        public void EndOrder()
        {
            _player1.EndOrder();
        }

        public void SetWorld()
        {
            _player1.SetWorld();
        }

        public bool IsWorld()
        {
            return _player1.IsWorld(_card.tempID);
        }

        public bool OnlyWorlds()
        {
            return _player1.OnlyWorlds();
        }

        public int NumWorlds()
        {
            return _player1.NumWorlds();
        }

        public void DarkNight()
        {
            _player1.DarkNight();
        }

        public void AbyssalDarkNight()
        {
            _player1.AbyssalDarkNight();
        }

        public void NoWorld()
        {
            
        }

        public bool WorldPlayed()
        {
            return _player1.WorldPlayed();
        }

        public void CallToken(string tokenID)
        {
            int token = _player1.CreateToken(tokenID);
            List<Card> toBeCalled = new List<Card>();
            toBeCalled.Add(_player1.GetCard(token));
            toBeCalled[0].originalOwner = _player1._playerID;
            _cardFight.SuperiorCall(_player1, _player2, toBeCalled, 1, 1, null, false, true);
        }

        public int NumOriginalDress()
        {
            return _player1.NumOriginalDress(_card.tempID);
        }

        public bool OrderPlayed()
        {
            return _player1.OrderPlayed();
        }

        public void SetAlchemagicDiff()
        {
            _player1.SetAlchemagicDiff();
        }

        public void SetAlchemagicSame()
        {
            _player1.SetAlchemagicSame();
        }

        public void AlchemagicFreeSB()
        {
            _player1.AlchemagicFreeSB();
        }

        public void AddAlchemagicFreeCB(int count)
        {
            _player1.AddAlchemagicFreeCB(count);
        }

        public bool IsAlchemagic()
        {
            return _player1.IsAlchemagic();
        }

        public bool AlchemagicUsedThisTurn()
        {
            return _player1.AlchemagicUsedThisTurn();
        }

        public int GetNumberOf(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            return cards.Count;
        }

        public int SoulCount()
        {
            return _player1.GetSoul().Count;
        }

        public int GetColumn()
        {
            return _player1.GetColumn(_card.tempID);
        }

        public int GetColumn(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            return _player1.GetColumn(cards[0].tempID);
        }

        public void EnemyGuardWithTwo()
        {
            _player2.GuardWithTwo();
        }

        public void EnemyCannotGuardFromHand()
        {
            _player2.CannotGuardFromHand();
        }

        public void EnemyMinGradeForGuard(int min)
        {
            _player2.SetMinGradeForGuard(min);
        }

        public void CannotMove()
        {
            _player1.CannotMove();
        }

        public void CannotBoost()
        {
            _player1.CannotBoost();
        }

        public int SelectOption(params string[] list)
        {
            return _cardFight.SelectOption(_player1, list);
        }

        public bool YesNo(string query)
        {
            return _cardFight.YesNo(_player1, query);
        }

        public void OnRideAbilityResolved()
        {
            _player1.OnRideAbilityResolved(_card.tempID);
        }

        public bool Activated()
        {
            return _activated;
        }

        public bool Activated(int activationNumber)
        {
            return _cardFight.Activated(_card.tempID, activationNumber);
        }

        public void Shuffle()
        {
            _player1.Shuffle();
        }

        public void AddContinuousState(int paramNum, int state)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
                _player1.CardStates.AddContinuousState(card.tempID, state);
        }
    }

    class Param
    {
        List<int> _location = new List<int>();
        List<int> _count = new List<int>();
        List<string> _name = new List<string>();
        List<int> _grade = new List<int>();
        List<int> _unitType = new List<int>();
        List<int> _triggerType = new List<int>();
        List<int> _other = new List<int>();
        List<int> _FL = new List<int>();
        List<int> _column = new List<int>();
        List<int> _min = new List<int>();
        List<string> _nameContains = new List<string>();

        public void AddLocation(int location)
        {
            _location.Add(location);
        }

        public void AddCount(int count)
        {
            _count.Add(count);
        }

        public void AddName(string name)
        {
            _name.Add(name);
        }

        public void InjectName(string name)
        {
            _name.Clear();
            _name.Add(name);
        }

        public void InjectCount(int num)
        {
            _count.Clear();
            _count.Add(num);
        }

        public void InjectColumn(int num)
        {
            _column.Clear();
            _column.Add(num);
        }

        public void InjectGrade(int num)
        {
            _grade.Clear();
            _grade.Add(num);
        }

        public void AddGrade(int grade)
        {
            _grade.Add(grade);
        }

        public void AddUnitType(int type)
        {
            _unitType.Add(type);
        }

        public void AddTriggerType(int type)
        {
            _triggerType.Add(type);
        }

        public void AddOther(int other)
        {
            _other.Add(other);
        }

        public void AddFL(int fl)
        {
            _FL.Add(fl);
        }

        public void AddColumn(int column)
        {
            _column.Add(column);
        }

        public void AddMin(int min)
        {
            _min.Add(min);
        }

        public void AddNameContains(string name)
        {
            _nameContains.Add(name);
        }

        public List<int> Locations
        {
            get => _location;
        }

        public List<int> Counts
        {
            get => _count;
        }

        public List<string> Names
        {
            get => _name;
        }

        public List<int> Grades
        {
            get => _grade;
        }

        public List<int> UnitTypes
        {
            get => _unitType;
        }

        public List<int> TriggerTypes
        {
            get => _triggerType;
        }

        public List<int> Others
        {
            get => _other;
        }

        public List<int> FLs
        {
            get => _FL;
        }

        public List<int> Columns
        {
            get => _column;
        }

        public List<int> Mins
        {
            get => _min;
        }

        public List<string> NameContains
        {
            get => _nameContains;
        }
    }

    class Activation
    {
        public const int OnRide = 1;
        public const int OnAttack = 2;
        public const int OnOverDress = 3;
        public const int OnACT = 4;
        public const int OnBattleEnds = 5;
        public const int OverDress = 6;
        public const int Then = 7;
        public const int PlacedOnGC = 8;
        public const int PlacedOnRC = 9;
        public const int OnDriveCheck = 10;
        public const int OnOrder = 11;
        public const int Cont = 12;
        public const int PlacedOnVC = 13;
        public const int OnRidePhase = 14;
        public const int OnBlitzOrder = 15;
        public const int OnCallFromPrison = 16;
        public const int OnAttackHits = 17;
        public const int OnAttackHitsVanguard = 18;
        public const int OnEnemyRetired = 19;
        public const int OnOrderPlayed = 20;
        public const int OnOverTrigger = 21;
        public const int PlacedOnRCFromHand = 22;
        public const int OnDiscard = 23;
        public const int OnStand = 24;
        public const int OnChosen = 25;
        public const int PutOnGC = 26;
        public const int OnBattlePhase = 27;
        public const int OnEndPhase = 28;
        public const int OnRetiredForPlayerCost = 29;
    }

    public class Location
    {
        public const int TopSoul = 1;
        public const int VG = 2;
        public const int FaceupUnit = 3;
        public const int Drop = 4;
        public const int PlayerHand = 5;
        public const int Deck = 6;
        public const int Field = 7;
        public const int Soul = 8;
        public const int RideDeck = 9;
        public const int GC = 10;
        public const int Trigger = 11;
        public const int Damage = 12;
        public const int PlayerField = 13;
        public const int EnemyField = 14;
        public const int PlayerRC = 15;
        public const int PlayerVC = 16;
        public const int EnemyRC = 17;
        public const int EnemyVC = 18;
        public const int originalDress = 19;
        public const int EnemyHand = 20;
        public const int Revealed = 22;
        public const int RevealedTriggers = 23;
        public const int Selected = 24;
        public const int BackRow = 25;
        public const int Hand = 26;
        public const int RC = 27;
        public const int VC = 28;
        public const int PlayerPrisoners = 29;
        public const int EnemyPrisoners = 30;
        public const int Order = 31;
        public const int Prison = 32;
        public const int Looking = 33;
        public const int FrontRowEnemyRC = 34;
        public const int LastCalled = 35;
        public const int Bind = 36;
        public const int PlayerOrder = 37;
        public const int FrontRow = 38;
        public const int RevealedTrigger = 39;
        public const int PlayerUnits = 40;
        public const int PlayedOrder = 41;
        public const int LastCalledFromPrison = 42;
        public const int LastStood = 43;
        public const int PlayerDrop = 44;
        public const int EnemyDrop = 45;
        public const int InFront = 46;
        public const int PlayerBind = 47;
        public const int UnitsCalledThisTurn = 48;
        public const int UnitsCalledFromHandThisTurn = 49;
        public const int Anywhere = 50;
        public const int OrderArea = 51;
    }

    class Query
    {
        public const int Name = 1;
        public const int Grade = 2;
        public const int Location = 3;
        public const int Count = 4;
        public const int UnitType = 5;
        public const int Other = 6;
        public const int FL = 7;
        public const int Min = 8;
        public const int Trigger = 9;
        public const int Column = 10;
        public const int NameContains = 11;
    }

    class Other
    {
        public const int This = 1;
        public const int Unit = 2;
        public const int Prison = 3;
        public const int Standing = 4;
        public const int OverDress = 5;
        public const int OrLess = 6;
        public const int Resting = 7;
        public const int World = 8;
        public const int CanChoose = 9;
        public const int Order = 10;
        public const int NormalOrder = 11;
        public const int NotThis = 12;
        public const int FaceUp = 13;
        public const int FaceDown = 14;
        public const int NotAttackTarget = 15;
        public const int Attacking = 16;
        public const int Token = 17;
        public const int SetOrder = 18;
        public const int InFront = 19;
        public const int GradeOrLess = 20;
        public const int Boosting = 21;
        public const int Locked = 22;
        public const int SameColumn = 23;
    }

    class AbilityType
    {
        public const int Cont = 0;
        public const int Auto = 1;
        public const int ACT = 2;
        public const int OverTrigger = 3;
        public const int Order = 4;
    }

    class Property
    {
        public const int Description = 0;
        public const int ForEnemy = 1;
        public const int HasPrompt = 2;
        public const int IsMandatory = 3;
        public const int SB = 4;
        public const int CB = 5;
        public const int Retire = 6;
        public const int Discard = 7;
        public const int HardOncePerTurn = 8;
        public const int OncePerTurn = 9;
        public const int AddToSoul = 10;
        public const int Reveal = 11;
    }
}
