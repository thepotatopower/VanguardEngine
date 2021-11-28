using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using System.IO;
using MoonSharp.Interpreter.Interop.LuaStateInterop;
using System.Configuration;
using System.Collections.Specialized;

namespace VanguardEngine
{
    public class LuaInterpreter
    {
        public string luaPath;
        public string currentFilePath;
        public CardFight cardFight;
        int _abilityID = 0;
        Card currentCard = null;
        Player currentP1 = null;
        Player currentP2 = null;
        List<Ability> abilities;
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
        DynValue r;
        DynValue li;
        DynValue n;


        public LuaInterpreter(string path, CardFight cf)
        {
            luaPath = path;
            cardFight = cf;
            UserData.RegisterType<Ability>();
            UserData.RegisterType<Location>();
            UserData.RegisterType<Activation>();
            UserData.RegisterType<Query>();
            UserData.RegisterType<Other>();
            UserData.RegisterType<FL>();
            UserData.RegisterType<Property>();
            UserData.RegisterType<UnitType>();
            UserData.RegisterType<Trigger>();
            UserData.RegisterType<Skill>();
            UserData.RegisterType<PlayerState>();
            UserData.RegisterType<CardState>();
            UserData.RegisterType<PlayerState>();
            UserData.RegisterType<Race>();
            UserData.RegisterType<LuaInterpreter>();
            UserData.RegisterType<Card>();
            UserData.RegisterType<Names>();
            l = UserData.Create(new Location());
            a = UserData.Create(new Activation());
            q = UserData.Create(new Query());
            o = UserData.Create(new Other());
            FL = UserData.Create(new FL());
            p = UserData.Create(new Property());
            u = UserData.Create(new UnitType());
            tt = UserData.Create(new Trigger());
            s = UserData.Create(new Skill());
            ps = UserData.Create(new PlayerState());
            cs = UserData.Create(new CardState());
            r = UserData.Create(new Race());
            li = UserData.Create(this);
            n = UserData.Create(new Names());
        }

        public List<Ability> GetAbilities(Card card, Player player1, Player player2, bool player)
        {
            Ability ability;
            abilities = new List<Ability>();
            Script script;
            bool success;
            string filePath = card.id;
            filePath = luaPath + Path.DirectorySeparatorChar + filePath + ".lua";
            currentFilePath = filePath;
            if (!File.Exists(filePath))
            {
                Log.WriteLine("no lua script for " + filePath);
                return abilities;
            }
            else
                Log.WriteLine("loading " + filePath + " " + card.tempID);
            Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.FileSystemScriptLoader();
            Script tempScript = new Script();
            tempScript.Globals["GetID"] = (Func<int>)GetID;
            tempScript.Globals["NewAbility"] = (Func<int, Ability>)NewAbility;
            if (card.str1 == "")
            {
                Log.WriteLine("old ability format");
                tempScript.DoFile(filePath);
                DynValue numberOfAbilities = tempScript.Call(tempScript.Globals["NumberOfAbilities"]);
                for (int i = 0; i < numberOfAbilities.Number; i++)
                {
                    script = new Script();
                    DynValue obj;
                    script.Globals.Set("l", l);
                    script.Globals.Set("a", a);
                    script.Globals.Set("q", q);
                    script.Globals.Set("o", o);
                    script.Globals.Set("FL", FL);
                    script.Globals.Set("p", p);
                    script.Globals.Set("u", u);
                    script.Globals.Set("tt", tt);
                    script.Globals.Set("s", s);
                    script.Globals.Set("ps", ps);
                    script.Globals.Set("cs", cs);
                    script.Globals.Set("r", r);
                    script.Globals.Set("n", n);
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
            }
            else
            {
                Log.WriteLine("new ability format");
                currentCard = card;
                currentP1 = player1;
                currentP2 = player2;
                abilities = new List<Ability>();
                tempScript.Globals.Set("l", l);
                tempScript.Globals.Set("a", a);
                tempScript.Globals.Set("q", q);
                tempScript.Globals.Set("o", o);
                tempScript.Globals.Set("FL", FL);
                tempScript.Globals.Set("p", p);
                tempScript.Globals.Set("u", u);
                tempScript.Globals.Set("tt", tt);
                tempScript.Globals.Set("s", s);
                tempScript.Globals.Set("ps", ps);
                tempScript.Globals.Set("cs", cs);
                tempScript.Globals.Set("r", r);
                tempScript.Globals.Set("n", n);
                tempScript.DoFile(filePath);
                tempScript.Call(tempScript.Globals["RegisterAbilities"]);
                if (OrderType.IsSetOrder(card.orderType) && !abilities.Exists(a => a.GetActivations.Exists(activation => activation == Activation.OnOrder)))
                {
                    currentFilePath = luaPath + Path.DirectorySeparatorChar + "set_order.lua";
                    tempScript.DoFile(currentFilePath);
                    tempScript.Call(tempScript.Globals["RegisterAbilities"]);
                }
            }
            currentFilePath = "";
            return abilities;
        }

        public Ability NewAbility(int tempID)
        {
            Script script = new Script();
            DynValue obj;
            script.Globals.Set("l", l);
            script.Globals.Set("a", a);
            script.Globals.Set("q", q);
            script.Globals.Set("o", o);
            script.Globals.Set("FL", FL);
            script.Globals.Set("p", p);
            script.Globals.Set("u", u);
            script.Globals.Set("tt", tt);
            script.Globals.Set("s", s);
            script.Globals.Set("ps", ps);
            script.Globals.Set("cs", cs);
            script.Globals.Set("r", r);
            script.Globals.Set("n", n);
            if (currentFilePath == "")
                script.DoFile(luaPath + Path.DirectorySeparatorChar + cardFight._player1.GetCard(tempID).id + ".lua");
            else
                script.DoFile(currentFilePath);
            script.Globals["NewAbility"] = (Func<int, Ability>)NewAbility;
            Card card = cardFight._player1.GetCard(tempID);
            Ability ability;
            if (card.originalOwner == 1)
                ability = new Ability(cardFight._player1, cardFight._player2, cardFight, card, _abilityID++);
            else
                ability = new Ability(cardFight._player2, cardFight._player1, cardFight, card, _abilityID++);
            ability.StoreAbility(script);
            obj = UserData.Create(ability);
            script.Globals.Set("obj", obj);
            cardFight.AddAbility(ability);
            return ability;
        }

        public int GetID()
        {
            if (currentCard != null)
                return currentCard.tempID;
            return -1;
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

        public List<Ability> GetAbilities(int activation, List<Card> cards, Tuple<int, int> timingCount)
        {
            List<Ability> abilities = new List<Ability>();
            foreach (Card card in cards)
            {
                if (card == null)
                    continue;
                foreach (Ability ability in _abilities[card.tempID])
                {
                    if (!ability.isGiven && ability.IsActivation(activation) && ability.IsTriggered(activation, timingCount))
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
                if (ability.CanOverDress(tempID, circle))
                    return true;
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

        public void OnZoneChange(int tempID)
        {
            List<Ability> toRemove = new List<Ability>();
            foreach (List<Ability> abilities in _abilities)
            {
                foreach (Ability ability in abilities)
                {
                    ability.Tracking.Remove(tempID);
                    if (ability.ResetTarget != -1 && ability.ResetTarget == tempID)
                        toRemove.Add(ability);
                }
                foreach (Ability ability in toRemove)
                    abilities.Remove(ability);
            }
        }

        public void EndOfBattle()
        {
            foreach (List<Ability> abilities in _abilities)
            {
                List<Ability> toRemove = new List<Ability>();
                foreach (Ability ability in abilities)
                {
                    if (ability.ResetTiming == Property.UntilEndOfBattle)
                        toRemove.Add(ability);
                }
                foreach (Ability ability in toRemove)
                    abilities.Remove(ability);
            }
        }
    }

    public class Ability
    {
        Player _player1;
        Player _player2;
        CardFight _cardFight;
        Card _card;
        bool _isMandatory = true;
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
        bool _given = false;
        bool _costRequired = true;
        List<int> _activations = new List<int>();
        List<int> _locations = new List<int>();
        List<int> _calledForCost = new List<int>();
        List<Card> _lastCalled = new List<Card>();
        int _abilityType;
        int _abilityNumber;
        public int _abilityID;
        Tuple<int, int> _timingCount;
        int _currentActivation = 0;
        int _resultOf = 0;
        List<int> _overDressParams = new List<int>();
        Script _script;
        DynValue _abilityActivate;
        DynValue _abilityCost;
        DynValue _checkCondition;
        DynValue _canFullyResolve;
        Dictionary<int, Param> _params = new Dictionary<int, Param>();
        List<Card> _selected = new List<Card>();
        List<int> _stored = new List<int>();
        List<int> _additionalProperties = new List<int>();
        string _activationFunction = "";
        string _triggerCondition = "";
        string _activationCondition = "";
        string _canFullyResolveFunction = "";
        string _cost = "";
        string _overDress = "";
        public bool newFormat = false;
        List<int> _tracking = new List<int>();
        int _resetTarget = -1;
        int _resetTiming = -1;
        bool _sourceIsRelevant = false;
        bool _sourceIsPlayer = false;
        int _sourceLocation = -1;
        int _movedTo = -1;
        int _movedFrom = -1;
        bool _repeatable = false;
        AbilityTimingData data = null;
        string _prompt = "";

        public Ability(Player player1, Player player2, CardFight cardFight, Card card, int abilityID)
        {
            _player1 = player1;
            _player2 = player2;
            _cardFight = cardFight;
            _card = card;
            _abilityID = abilityID;
        }

        void SetAbilityData(int activation, Tuple<int, int> timingCount)
        {
            if (timingCount == null)
                timingCount = new Tuple<int, int>(0, 0);
            _timingCount = timingCount;
            _currentActivation = activation;
            data = _cardFight.GetAbilityTimingData(activation, timingCount, _player1._playerID);
        }

        public void SetDescription(int number)
        {
            if (number == 1)
                _description = _card.str1;
            else if (number == 2)
                _description = _card.str2;
            else if (number == 3)
                _description = _card.str3;
            else if (number == 4)
                _description = _card.str4;
            else if (number == 5)
                _description = _card.str5;
            else if (number == 6)
                _description = _card.str6;
        }

        public void SetPrompt(int number)
        {
            if (number == 1)
                _prompt = _card.str1;
            else if (number == 2)
                _prompt = _card.str2;
            else if (number == 3)
                _prompt = _card.str3;
            else if (number == 4)
                _prompt = _card.str4;
            else if (number == 5)
                _prompt = _card.str5;
            else if (number == 6)
                _prompt = _card.str6;
        }

        public string GetPrompt()
        {
            return _prompt;
        }

        public string GetDescription(int number)
        {
            string description = "";
            if (number == 1)
                description = _card.str1;
            else if (number == 2)
                description = _card.str2;
            else if (number == 3)
                description = _card.str3;
            else if (number == 4)
                description = _card.str4;
            else if (number == 5)
                description = _card.str5;
            else if (number == 6)
                description = _card.str6;
            return description;
        }

        public void SetProperty(int property)
        {
            if (property == Property.OncePerTurn)
                _oncePerTurn = true;
            else if (property == Property.Repeatable)
                _repeatable = true;
            else if (property == Property.NotMandatory)
                _isMandatory = false;
        }

        public void SetTiming(int activation)
        {
            if (!_activations.Contains(activation))
                _activations.Add(activation);
        }

        public void SetLocation(int location)
        {
            if (!_locations.Contains(location))
                _locations.Add(location);
        }

        public void SetTrigger(string function)
        {
            SetTriggerCondition(function);
        }

        public void SetTriggerCondition(string function)
        {
            _triggerCondition = function;
        }

        public void SetCanFullyResolve(string function)
        {
            _canFullyResolveFunction = function;
        }

        public void SetCost(string function)
        {
            _cost = function;
            _isMandatory = false;
        }

        public void SetCondition(string function)
        {
            SetActivationCondition(function);
        }

        public void SetActivationCondition(string function)
        {
            _activationCondition = function;
        }

        public void SetOverDress(string function)
        {
            _overDress = function;
        }

        public void SetResetTarget(int tempID)
        {
            _resetTarget = tempID;
        }

        public void SetResetTiming(int timing)
        {
            _resetTiming = timing;
        }

        public void SetSourceIsPlayer(bool player)
        {
            _sourceIsRelevant = true;
            _sourceIsPlayer = player;
        }

        public void SetSourceLocation(int location)
        {
            _sourceIsRelevant = true;
            _sourceLocation = location;
        }

        public void SetMovedFrom(int location)
        {
            _movedFrom = location;
        }

        public void SetMovedTo(int location)
        {
            _movedTo = location;
        }

        public void SetParam(List<object> param, int position)
        {
            _params[position] = CreateParam(param);
        }

        public Card GetCard()
        {
            return _card;
        }

        //public void SetTimingCount(Tuple<int, int> timingCount)
        //{
        //    _timingCount.SetTimingCount(timingCount);
        //}

        public void SetActivation(string activation)
        {
            _activationFunction = activation;
        }

        public bool CostRequired()
        {
            return _costRequired;
        }

        public bool IsRepeatable()
        {
            return _repeatable;
        }

        public void StoreAbility(Script script)
        {
            _script = script;
            script.Globals["GetID"] = (Func<int>)GetID;
            newFormat = true;
            _hasPrompt = true;
        }

        public bool StoreAbility(Script script, int num, bool player)
        {
            _script = script;
            _abilityNumber = num;
            Param param;
            DynValue activationRequirement = script.Call(script.Globals["ActivationRequirement"], _abilityNumber);
            DynValue numOfParams = script.Call(script.Globals["NumberOfParams"]);
            DynValue returnedParam;
            _params[-1] = new Param();
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
                    else if (returnedParam.Tuple[j].Number == Query.Race)
                    {
                        param.AddRace((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.MaxPower)
                    {
                        param.AddMaxPower((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.Power)
                    {
                        param.AddPower((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                }
                _params[i + 1] = param;
            }
            int index = 0;
            while ((int)activationRequirement.Tuple[index].Number < 0)
            {
                _activations.Add((int)activationRequirement.Tuple[index].Number);
                if ((int)activationRequirement.Tuple[index].Number == Activation.OverDress)
                {
                    for (int j = index + 1; j < activationRequirement.Tuple.Length; j++)
                        _overDressParams.Add((int)activationRequirement.Tuple[j].Number);
                    return true;
                }
                index++;
            }
            for (int i = index; i < activationRequirement.Tuple.Length; i++)
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
                else if ((int)activationRequirement.Tuple[i].Number == Property.Rest)
                {
                    _costs[Property.Rest] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.SpecificCB)
                {
                    _costs[Property.SpecificCB] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.SpecificSB)
                {
                    _costs[Property.SpecificSB] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.AddToDrop)
                {
                    _costs[Property.AddToDrop] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.SpecificDiscard)
                {
                    _costs[Property.SpecificDiscard] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.ResultOf)
                {
                    _resultOf = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.Bind)
                {
                    _costs[Property.Bind] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.AddToDamageZone)
                {
                    _costs[Property.AddToDamageZone] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.Call)
                {
                    _costs[Property.Call] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.Given)
                {
                    _given = true;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.BlackWings)
                {
                    _additionalProperties.Add(Property.BlackWings);
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.WhiteWings)
                {
                    _additionalProperties.Add(Property.WhiteWings);
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.SendToTop)
                {
                    _costs[Property.SendToTop] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.SendToBottom)
                {
                    _costs[Property.SendToBottom] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.RevealAndSendToBottom)
                {
                    _costs[Property.RevealAndSendToBottom] = (int)activationRequirement.Tuple[i + 1].Number;
                    i++;
                }
                else if ((int)activationRequirement.Tuple[i].Number == Property.CostNotRequired)
                {
                    _costRequired = false;
                }
            }
            if (_activations.Contains(Activation.OnACT) || _activations.Contains(Activation.OnOrder)
                || _activations.Contains(Activation.OnBlitzOrder))
            {
                _isMandatory = false;
                _hasPrompt = true;
            }
            else if (_activations.Contains(Activation.Cont))
            {
                _isMandatory = true;
                _hasPrompt = false;
            }
            if (_costs.Count > 0)
            {
                _hasPrompt = true;
                _isMandatory = false;
            }
            if (!_forEnemy && !player)
                return false;
            if (_forEnemy && player)
                return false;
            _checkCondition = script.Globals.Get("CheckCondition");
            _canFullyResolve = script.Globals.Get("CanFullyResolve");
            _abilityActivate = script.Globals.Get("Activate");
            return true;
        }

        public Param CreateParam(List<object> newParam)
        {
            Param param = new Param();
            for (int j = 0; j < newParam.Count; j++)
            {
                if (newParam[j] is System.Double && (double)newParam[j] == Query.Count)
                {
                    param.AddCount((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.Min)
                {
                    param.AddMin((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.Location)
                {
                    param.AddLocation((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.Grade)
                {
                    param.AddGrade((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.Name)
                {
                    param.AddName((string)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.UnitType)
                {
                    param.AddUnitType((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.Trigger)
                {
                    param.AddTriggerType((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.Other)
                {
                    param.AddOther((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.FL)
                {
                    param.AddFL((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.Column)
                {
                    param.AddColumn((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.NameContains)
                {
                    param.AddNameContains((string)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.Race)
                {
                    param.AddRace((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.MaxPower)
                {
                    param.AddMaxPower((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.Power)
                {
                    param.AddPower((int)(double)newParam[j + 1]);
                    j++;
                }
                else if (newParam[j] is System.Double && (double)newParam[j] == Query.SnapshotIndex)
                {
                    param.AddSnapshotIndex((int)(double)newParam[j + 1]);
                    j++;
                }
            }
            return param;
        }

        //public Param CreateParam(params int[] parameters)
        //{
        //    Param param = new Param();
        //    for (int j = 0; j < parameters.Length; j++)
        //    {
        //        if (parameters[j] == Query.Count)
        //        {
        //            param.AddCount((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.Min)
        //        {
        //            param.AddMin((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.Location)
        //        {
        //            param.AddLocation((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.Grade)
        //        {
        //            param.AddGrade((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.UnitType)
        //        {
        //            param.AddUnitType((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.Trigger)
        //        {
        //            param.AddTriggerType((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.Other)
        //        {
        //            param.AddOther((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.FL)
        //        {
        //            param.AddFL((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.Column)
        //        {
        //            param.AddColumn((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.Race)
        //        {
        //            param.AddRace((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.MaxPower)
        //        {
        //            param.AddMaxPower((int)parameters[j + 1]);
        //            j++;
        //        }
        //        else if (parameters[j] == Query.Power)
        //        {
        //            param.AddPower((int)parameters[j + 1]);
        //            j++;
        //        }
        //    }
        //    return param;
        //}

        public void setScript(Script script)
        {
            _script = script;
        }

        public void setAbility(DynValue abilityActivate)
        {
            _abilityActivate = abilityActivate;
        }

        public List<int> Tracking
        {
            get => _tracking;
        }

        public bool isMandatory
        {
            get => _isMandatory;
        }

        public bool hasPrompt
        {
            get => _hasPrompt;
        }

        public bool isHardOncePerTurn
        {
            get => _hardOncePerTurn;
        }

        public bool isGiven
        {
            get => _given;
        }

        public List<int> GetActivations
        {
            get => _activations;
        }

        public bool IsActivation(int activation)
        {
            if (_activations.Contains(activation))
                return true;
            return false;
        }

        public string Description
        {
            get => _description;
        }

        public List<int> Locations
        {
            get => _locations;
        }

        public string Name
        {
            get => _card.name;
        }

        public int AbilityType
        {
            get => _abilityType;
        }

        public int ResetTarget
        {
            get => _resetTarget;
        }

        public int ResetTiming
        {
            get => _resetTiming;
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
            Param param = _params[paramNum];
            if (param.Counts.Count >= 1)
                return param.Counts[0];
            else
                return ValidCards(paramNum).Count;
            //if (_params[i].Mins.Count >= 1)
            //    return GetMin(paramNum);
            //return -1;
        }

        public int GetMin(int paramNum)
        {
            Param param = _params[paramNum];
            if (param.Mins.Count >= 1)
                return param.Mins[0];
            else
                return GetCount(paramNum);
        }

        public bool HasCount(int paramNum)
        {
            Param param = _params[paramNum];
            if (param.Counts.Count >= 1)
                return true;
            return false;
        }

        public bool HasMin(int paramNum)
        {
            Param param = _params[paramNum];
            if (param.Mins.Count >= 1)
                return true;
            return false;
        }

        public bool GetOrLess(int paramNum)
        {
            Param param = _params[paramNum];
            if (param.Others.Contains(Other.OrLess))
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
            _currentActivation = 0;
            _stored.Clear();
        }

        public void Test()
        {
            Log.WriteLine("test");
        }

        public int Activate(int activation, Tuple<int, int> timingCount)
        {
            SetAbilityData(activation, timingCount);
            _lastCalled.Clear();
            _payingCost = false;
            if (_activationFunction != "")
            {
                _script.Call(_script.Globals[_activationFunction]);
                _activated = true;
                return 0;
            }
            //DynValue Then = _script.Call(_abilityActivate, _abilityNumber);
            if (_currentActivation == Activation.OnOverTrigger)
            {
                int times = 1;
                if (_player1.MyStates.HasState(PlayerState.DoubleTriggerEffects))
                    times++;
                for (int i = 0; i < times; i++)
                    _script.Call(_abilityActivate, _abilityNumber);
            }
            else
                _script.Call(_abilityActivate, _abilityNumber);
            _activated = true;
            //return (int)Then.Number;
            return 0;
        }

        public int ActivateAsGiven(Card card)
        {
            if (!_player1.IsAlchemagic())
                _lastCalled.Clear();
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

        public bool PayCost()
        {
            return PayCost(true);
        }

        public bool PayCost(bool required)
        {
            if (!_costRequired && required)
                return true;
            _payingCost = true;
            if (_cost != "")
            {
                _script.Call(_script.Globals[_cost], false);
                return true;
            }
            if (_costs.ContainsKey(Property.CB))
                CounterBlast(_costs[Property.CB]);
            if (_costs.ContainsKey(Property.SpecificCB))
                SpecificCounterBlast(_costs[Property.SpecificCB]);
            if (_costs.ContainsKey(Property.SpecificSB))
                SpecificSoulBlast(_costs[Property.SpecificSB]);
            if (_costs.ContainsKey(Property.SB))
                SoulBlast(_costs[Property.SB]);
            if (_costs.ContainsKey(Property.Discard))
                Discard(_costs[Property.Discard], _costs[Property.Discard]);
            if (_costs.ContainsKey(Property.Reveal))
                ChooseReveal(_costs[Property.Reveal]);
            if (_costs.ContainsKey(Property.AddToSoul))
                ChooseAddToSoul(_costs[Property.AddToSoul]);
            if (_costs.ContainsKey(Property.Rest))
                ChooseRest(_costs[Property.Rest]);
            if (_costs.ContainsKey(Property.Retire))
                ChooseRetire(_costs[Property.Retire]);
            if (_costs.ContainsKey(Property.AddToDrop))
                ChooseAddToDrop(_costs[Property.AddToDrop]);
            if (_costs.ContainsKey(Property.SpecificDiscard))
                SpecificDiscard(_costs[Property.SpecificDiscard]);
            if (_costs.ContainsKey(Property.Bind))
                ChooseBind(_costs[Property.Bind]);
            if (_costs.ContainsKey(Property.AddToDamageZone))
                AddToDamageZone(Property.AddToDamageZone);
            if (_costs.ContainsKey(Property.SendToTop))
                ChooseSendToTop(_costs[Property.SendToTop], true);
            if (_costs.ContainsKey(Property.SendToBottom))
                ChooseSendToBottom(_costs[Property.SendToBottom], true);
            if (_costs.ContainsKey(Property.RevealAndSendToBottom))
                RevealAndSendToBottom(_costs[Property.RevealAndSendToBottom]);
            return true;
        }

        public bool CanPayCost()
        {
            if (_cost != "")
                return _script.Call(_script.Globals[_cost], true).Boolean;
            foreach (int key in _costs.Keys)
            {
                if (key == Property.CB && !CanCB(_costs[key]))
                    return false;
                else if (key == Property.SpecificCB && !CanSpecificCB(_costs[key]))
                    return false;
                else if (key == Property.SB && !CanSB(_costs[key]))
                    return false;
                else if (key == Property.SpecificSB && !CanSpecificSB(_costs[key]))
                    return false;
                else if (key == Property.Discard && !CanDiscard(_costs[key]))
                    return false;
                else if (key == Property.AddToSoul && !CanAddToSoul(_costs[key]))
                    return false;
                else if (key == Property.Reveal && !CanReveal(_costs[key]))
                    return false;
                else if (key == Property.Rest && !CanRest(_costs[key]))
                    return false;
                else if (key == Property.Retire && !CanRetire(_costs[key]))
                    return false;
                else if (key == Property.AddToDrop && !CanAddToDrop(_costs[key]))
                    return false;
                else if (key == Property.SpecificDiscard && !CanSpecificDiscard(_costs[key]))
                    return false;
                else if (key == Property.Bind && !CanBind(_costs[key]))
                    return false;
                else if (key == Property.AddToDamageZone && !Exists(_costs[key]))
                    return false;
                else if (key == Property.Call && !CanSuperiorCall(_costs[key]))
                    return false;
                else if (key == Property.SendToTop && !Exists(_costs[key]))
                    return false;
                else if (key == Property.SendToBottom && !Exists(_costs[key]))
                    return false;
                else if (key == Property.RevealAndSendToBottom && !Exists(_costs[key]))
                    return false;
            }
            return true;
        }

        public bool ChoosesToPayCost()
        {
            if (CanPayCost() && _cardFight.YesNo(_player1, "Pay cost?"))
                return PayCost(false);
            return false;
        }

        public bool IsTriggered(int activation, Tuple<int, int> timingCount)
        {
            SetAbilityData(activation, timingCount);
            if (_locations.Count > 0)
            {
                if (_card.originalOwner != _player1._playerID)
                    return false;
                bool validLocation = false;
                foreach (int location in _locations)
                {
                    if (location == Location.PlayerRC || location == Location.RC)
                    {
                        if ((data != null && data.allSnapshots[_card.tempID].location == Location.RC) || (data == null && IsRearguard()))
                            validLocation = true;
                    }
                    else if (location == Location.PlayerVC || location == Location.VC)
                    {
                        if ((data != null && data.allSnapshots[_card.tempID].location == Location.VC) || (data == null && IsVanguard()))
                            validLocation = true;
                    }
                    else if (location == Location.GC)
                    {
                        if ((data != null && data.allSnapshots[_card.tempID].location == Location.GC) || (data == null && IsGuardian()))
                                validLocation = true;
                    }
                    else if (location == Location.Soul)
                    {
                        if ((data != null && data.allSnapshots[_card.tempID].location == Location.Soul) || (data == null && _player1.GetSoul().Exists(card => card.tempID == _card.tempID)))
                            validLocation = true;
                    }
                    else if (location == Location.PlayerUnits)
                    {
                        int snapshottedLocation = -1;
                        if (data != null)
                            snapshottedLocation = data.allSnapshots[_card.tempID].location;
                        if (snapshottedLocation == -1)
                            validLocation = IsRearguard() || IsVanguard() || IsGuardian();
                        else
                            validLocation = snapshottedLocation == Location.RC || snapshottedLocation == Location.VC || snapshottedLocation == Location.GC;
                    }
                    else if (location == Location.BackRowRC)
                    {
                        if ((data != null && _player1.GetRowNum(data.allSnapshots[_card.tempID].circle) == 1) || IsBackRowRearguard())
                            validLocation = true;
                    }
                }
                if (!validLocation)
                    return false;
            }
            if (_triggerCondition != "")
                return _script.Call(_script.Globals[_triggerCondition]).Boolean;
            return CheckCondition(activation, timingCount);
        }
        
        public bool CheckCondition(int activation, Tuple<int, int> timingCount)
        {
            SetAbilityData(activation, timingCount);
            if (!CanActivate())
                return false;
            if (_additionalProperties.Contains(Property.BlackWings) && !IsBlackWings())
                return false;
            if (_additionalProperties.Contains(Property.WhiteWings) && !IsWhiteWings())
                return false;
            if (newFormat)
                return true;
            DynValue check = _script.Call(_checkCondition, _abilityNumber);
            if (check.Boolean)
                return true;
            return false;
        }

        public bool CheckConditionAsGiven(Card card, int activation, Tuple<int, int> timingCount)
        {
            SetAbilityData(activation, timingCount);
            if (!CanActivate())
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
            bool condition = CheckCondition(Activation.OnOrder, null);
            _withAlchemagic = true;
            return condition;
        }

        public bool CanActivate()
        {
            if (_costRequired && !CanPayCost())
                return false;
            if (_oncePerTurn && _activated)
                return false;
            if (_activationCondition != "" && !_script.Call(_script.Globals[_activationCondition]).Boolean)
                return false;
            return true;
        }

        public bool CanFullyResolve()
        {
            if (newFormat)
            {
                if (_canFullyResolveFunction != "")
                    return _script.Call(_script.Globals[_canFullyResolveFunction]).Boolean;
                else
                    return true;
            }
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
                return GetMin(_costs[Property.Retire]);
            else
                return 0;
        }

        public bool NotActivatedYet()
        {
            return !_activated;
        }
        public bool IsRodeUponThisTurn()
        {
            return IsApplicable();
            //return _abilityTimingData.Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
        }

        public bool WasRodeUponBy(string name)
        {
            AbilityTimingData lastRiddenOn = _cardFight.GetAbilityTimingData(Activation.OnRide, _timingCount.Item1, _player1._playerID)[_timingCount.Item2 - 1];
            AbilityTimingData lastPlacedOnVC = _cardFight.GetAbilityTimingData(Activation.PlacedOnVC, _timingCount.Item1, _player1._playerID)[_timingCount.Item2 - 1];
            //return lastRiddenOn.Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID)) &&
            //    lastPlacedOnVC.Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.name == name));
            return lastRiddenOn.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID) &&
                lastPlacedOnVC.GetRelevantSnapshots(0).Exists(snapshot => snapshot.name == name);
        }

        public bool WasRodeUponByNameContains(string name)
        {
            AbilityTimingData lastRiddenOn = _cardFight.GetAbilityTimingData(Activation.OnRide, _timingCount.Item1, _player1._playerID)[_timingCount.Item2 - 1];
            AbilityTimingData lastPlacedOnVC = _cardFight.GetAbilityTimingData(Activation.PlacedOnVC, _timingCount.Item1, _player1._playerID)[_timingCount.Item2 - 1];
            //return lastRiddenOn.Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID)) &&
            //    lastPlacedOnVC.Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.name == name));
            return lastRiddenOn.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID) &&
                lastPlacedOnVC.GetRelevantSnapshots(0).Exists(snapshot => snapshot.name.Contains(name));
        }

        public bool PlacedByRidingOn(string name)
        {
            AbilityTimingData lastRiddenOn = _cardFight.GetAbilityTimingData(Activation.OnRide, _timingCount.Item1, _player1._playerID)[_timingCount.Item2 - 1];
            AbilityTimingData lastPlacedOnVC = _cardFight.GetAbilityTimingData(Activation.PlacedOnVC, _timingCount.Item1, _player1._playerID)[_timingCount.Item2 - 1];
            //return lastRiddenOn.Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID)) &&
            //    lastPlacedOnVC.Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.name == name));
            return lastRiddenOn.GetRelevantSnapshots(0).Exists(snapshot => snapshot.name == _card.name) &&
                lastPlacedOnVC.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID);
        }

        public bool MyVanguardWasPlaced()
        {
            return _cardFight.GetAbilityTimingData(Activation.PlacedOnVC, _timingCount.Item1, _player1._playerID)[_timingCount.Item2 - 1].GetRelevantSnapshots(0).Count > 0;
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
                if (_player1.Grade(card.tempID) == grade)
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
            return CanSuperiorCallToSpecificCircle(paramNum, -1);
        }

        public bool CanSuperiorCallToSpecificCircle(List<object> param, params int[] circles)
        {
            SetParam(param, 1);
            return CanSuperiorCallToSpecificCircle(1, circles);
        }

        public bool CanSuperiorCallToSpecificCircle(int paramNum, params int[] circles)
        {
            int count = 0;
            if (Exists(paramNum))
            {
                foreach (Card card in ValidCards(paramNum))
                {
                    List<int> availableCircles = _player1.GetTotalAvailableCircles(card, circles);
                    if (availableCircles.Count > 0)
                        count++;
                }
                if (count >= GetCount(paramNum))
                    return true;
            }
            return false;
        }

        public List<int> ValidIDs(List<object> param)
        {
            SetParam(param, 1);
            return ConvertToTempIDs(ValidCards(1));
        }

        public List<Card> ValidCards(int paramNum)
        {
            Param param = _params[paramNum];
            List<Card> currentPool = new List<Card>();
            List<Card> newPool = new List<Card>();
            List<Card> cards;
            foreach (int location in param.Locations)
            {
                if (location == Location.Deck)
                    currentPool.AddRange(_player1.GetDeck());
                else if (location == Location.RideDeck)
                    currentPool.AddRange(_player1.GetRideDeck());
                else if (location == Location.Drop || location == Location.PlayerDrop)
                    currentPool.AddRange(_player1.GetDrop());
                else if (location == Location.EnemyDrop)
                    currentPool.AddRange(_player2.GetDrop());
                else if (location == Location.PlayerHand || location == Location.Hand)
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
                else if (location == Location.RevealedDamageChecks)
                    currentPool.AddRange(_player1.GetRevealedDamageChecks());
                else if (location == Location.RevealedDriveChecks)
                    currentPool.AddRange(_player1.GetRevealedDriveChecks());
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
                else if (location == Location.BackRowEnemyRC)
                    currentPool.AddRange(_player1.GetEnemyBackRowRearguards());
                else if (location == Location.Trigger)
                    currentPool.Add(_player1.GetTrigger(true));
                else if (location == Location.LastCalled)
                    currentPool.AddRange(_lastCalled);
                else if (location == Location.PlayerOrder)
                    currentPool.AddRange(_player1.GetPlayerOrder());
                else if (location == Location.OrderArea)
                    currentPool.AddRange(_player1.GetOrderArea());
                else if (location == Location.Order)
                    currentPool.AddRange(_player1.GetOrderZone());
                else if (location == Location.PlayerUnits)
                {
                    currentPool.AddRange(_player1.GetRearguards(C.Player));
                    if (_player1.Vanguard() != null)
                        currentPool.Add(_player1.Vanguard());
                    currentPool.AddRange(_player1.GetGC());
                }
                else if (location == Location.EnemyUnits)
                {
                    currentPool.AddRange(_player2.GetRearguards(C.Player));
                    if (_player1.Vanguard() != null)
                        currentPool.Add(_player2.Vanguard());
                    currentPool.AddRange(_player2.GetGC());
                }
                else if (location == Location.Bind)
                    currentPool.AddRange(_player1.GetBind());
                else if (location == Location.PlayedOrder)
                {
                    if (_player1.GetPlayedOrder() != null)
                        currentPool.Add(_player1.GetPlayedOrder());
                }
                else if (location == Location.LastCalledFromPrison)
                {
                    //foreach (AbilityTimingData data in _cardFight.GetAbilityTimingData(Activation.PlacedOnRCFromPrison, _timingCount, _player2._playerID))
                    //{
                    //    foreach (Snapshot snapShot in data.GetRelevantSnapshots(0))
                    //        currentPool.Add(_player1.GetCard(snapShot.tempID));
                    //}
                }
                else if (location == Location.LastStood)
                {
                    //foreach (AbilityTimingData data in _cardFight.GetAbilityTimingData(Activation.OnStand, _timingCount, _player1._playerID))
                    //{
                    //    foreach (Snapshot snapShot in data.GetRelevantSnapshots(0))
                    //        currentPool.Add(_player1.GetCard(snapShot.tempID));
                    //}
                }
                else if (location == Location.InFront)
                    currentPool.AddRange(_player1.GetInFront(_card.tempID));
                else if (location == Location.UnitsCalledThisTurn)
                    currentPool.AddRange(_player1.GetUnitsCalledThisTurn());
                else if (location == Location.UnitsCalledFromHandThisTurn)
                    currentPool.AddRange(_player1.GetUnitsCalledFromHandThisTurn());
                else if (location == Location.CalledForCost)
                {
                    List<Card> calledForCost = new List<Card>();
                    foreach (int tempID in _calledForCost)
                        calledForCost.Add(_player1.GetCard(tempID));
                    currentPool.AddRange(calledForCost);
                }
                else if (location == Location.PlayerRCRetired)
                {
                    currentPool.AddRange(GetSnapshottedCards(0));
                    //foreach (AbilityTimingData data in _cardFight.GetAbilityTimingData(Activation.OnPlayerRCRetired, _timingCount, _player1._playerID))
                    //{
                    //    foreach (Snapshot snapShot in data.GetRelevantSnapshots(0))
                    //        currentPool.Add(_player1.GetCard(snapShot.tempID));
                    //}
                }
                else if (location == Location.LastOrderPlayed)
                {
                    currentPool.AddRange(GetSnapshottedCards(0));
                    //foreach (AbilityTimingData data in _cardFight.GetAbilityTimingData(Activation.OnOrderPlayed, _timingCount, _player1._playerID))
                    //{
                    //    foreach (Snapshot snapShot in data.GetRelevantSnapshots(0))
                    //        currentPool.Add(_player1.GetCard(snapShot.tempID));
                    //}
                }
                else if (location == Location.Applicable)
                {
                    foreach (Card card in GetSnapshottedCards(0))
                    {
                        if (card.tempID == _card.tempID)
                            currentPool.Add(card);
                    }
                }
                else if (location == Location.PlayedOrdersThisTurn)
                    currentPool.AddRange(_player1.GetPlayedOrdersThisTurn());
                else if (location == Location.MyOriginalDress)
                    currentPool.AddRange(_player1.GetOriginalDress(_card.tempID));
                else if (location == Location.Stored)
                {
                    foreach (int tempID in _stored)
                        currentPool.Add(_player1.GetCard(tempID));
                }
            }
            if (param.SnapshotIndexes.Count > 0)
            {
                if (currentPool.Count > 0)
                {
                    foreach (Card card in currentPool)
                    {
                        foreach (int index in param.SnapshotIndexes)
                        {
                            if (GetSnapshottedCards(index).Contains(card))
                                newPool.Add(card);
                        }
                    }
                    currentPool.Clear();
                    currentPool.AddRange(newPool);
                    newPool.Clear();
                }
                else
                {
                    foreach (int index in param.SnapshotIndexes)
                        currentPool.AddRange(GetSnapshottedCards(index));
                }
            }
            if (param.Locations.Count == 0 && param.SnapshotIndexes.Count == 0)
            {
                if (param.Others.Contains(Other.This))
                    currentPool.Add(_player1.GetCard(_card.tempID));
                else if (param.Others.Contains(Other.ThisFieldID))
                {
                    if (data != null && data.allSnapshots[_card.tempID].fieldID == _player1.GetFieldID(_card.tempID))
                        currentPool.Add(_player1.GetCard(_card.tempID));
                }
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
                            if (card.unitType >= 0)
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
                            if (_player1.IsOverDress(card.tempID))
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
                            if (card.orderType != OrderType.NotOrder)
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
                    else if (other == Other.Attacked)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (_player1.AttackedUnits().Contains(card))
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
                            if (OrderType.IsSetOrder(card.orderType))
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
                            if (_player1.Booster() != null && card.tempID == _player1.Booster().tempID)
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
                    else if (other == Other.Song)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.orderType == OrderType.Song)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Revealed)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (_player1.GetRevealed().Contains(card))
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Gem)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.orderType == OrderType.Gem)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Player)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.originalOwner == _player1._playerID)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.EvenGrade)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (_player1.Grade(card.tempID) % 2 == 0)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.OddGrade)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (_player1.Grade(card.tempID) % 2 != 0)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Selected)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (_selected.Contains(card))
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.SameColumn)
                    {
                        int column = _player1.GetColumn(_card.tempID);
                        if (column == -1)
                            column = _player1.GetColumn(_card.tempID);
                        if (column != -1)
                        {
                            foreach (Card card in currentPool)
                            {
                                if (_player1.GetUnitsAtColumn(column).Exists(c => c.tempID == card.tempID))
                                    newPool.Add(card);
                            }
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.Meteorite)
                    {
                        foreach (Card card in currentPool)
                        {
                            if (card.orderType == OrderType.Meteorite)
                                newPool.Add(card);
                        }
                        currentPool.Clear();
                        currentPool.AddRange(newPool);
                        newPool.Clear();
                    }
                    else if (other == Other.ThisFieldID || other == Other.NotThisFieldID)
                    {
                        if (data != null)
                        {
                            if (other == Other.ThisFieldID && currentPool.Exists(card => card.tempID == _card.tempID))
                            {
                                currentPool.Clear();
                                if (data.allSnapshots[_card.tempID] != null && data.allSnapshots[_card.tempID].fieldID == _player1.GetFieldID(_card.tempID))
                                    currentPool.Add(_player1.GetCard(_card.tempID));
                            }
                            else if (other == Other.NotThisFieldID)
                            {
                                foreach (Card card in currentPool)
                                {
                                    if (data.allSnapshots[card.tempID] != data.allSnapshots[_card.tempID])
                                        newPool.Add(card);
                                }
                                currentPool.Clear();
                                currentPool.AddRange(newPool);
                                newPool.Clear();
                            }
                        }
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
                    if ((param.Grades.Contains(_player1.Grade(currentPool[i].tempID)) || 
                        (param.Others.Contains(Other.GradeOrLess) && _player1.Grade(currentPool[i].tempID) <= param.Grades.Max()) ||
                        (param.Others.Contains(Other.GradeOrHigher) && _player1.Grade(currentPool[i].tempID) >= param.Grades.Min())) && 
                        !newPool.Contains(currentPool[i]))
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
                    if ((param.UnitTypes.Contains(UnitType.Normal) && UnitType.IsNormal(currentPool[i].unitType)) ||
                       (param.UnitTypes.Contains(currentPool[i].unitType) && !newPool.Contains(currentPool[i])))
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
            if (param.Races.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    if (param.Races.Contains(currentPool[i].race) && !newPool.Contains(currentPool[i]))
                        newPool.Add(currentPool[i]);
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.MaxPowers.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    if (param.MaxPowers.Exists(power => power >= _player1.GetPower(currentPool[i].tempID)))
                        newPool.Add(currentPool[i]);
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.Powers.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    if (param.Powers.Exists(power => power == currentPool[i].power))
                        newPool.Add(currentPool[i]);
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.Others.Contains(Other.DifferentNames))
            {
                foreach (Card card in currentPool)
                {
                    if (!newPool.Exists(c => c.name == card.name))
                        newPool.Add(card);
                }
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.Others.Contains(Other.Shuffled))
            {
                Random r = new Random();
                List<Card> shuffled = new List<Card>();
                while (currentPool.Count > 0)
                {
                    Card randomCard = currentPool[r.Next(currentPool.Count)];
                    shuffled.Add(randomCard);
                    currentPool.Remove(randomCard);
                }
                return shuffled;
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
            return _player1.Grade(cards[0].tempID);
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
            if (_overDress != "")
                return _script.Call(_script.Globals[_overDress], card.tempID).Boolean;
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
            return IsApplicable();
            //return _cardFight.GetAbilityTimingData(Activation.PlacedOnGC, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
        }

        public bool LastPutOnGC()
        {
            return IsApplicable();
            //return _cardFight.GetAbilityTimingData(Activation.PutOnGC, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
        }

        public bool IsIntercepting()
        {
            return _player1.IsIntercepting(_card.tempID);
        }

        public bool LastPlacedOnRC()
        {
            return IsApplicable();
            //return _cardFight.GetAbilityTimingData(Activation.PlacedOnRC, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
        }

        public bool LastPlacedOnRCFromHand()
        {
            return IsApplicable();
            //return _cardFight.GetAbilityTimingData(Activation.PlacedOnRCFromHand, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
        }

        public bool LastPlacedOnVC()
        {
            return IsApplicable();
            //return _cardFight.GetAbilityTimingData(Activation.PlacedOnVC, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
        }

        public bool LastDiscarded()
        {
            return IsApplicable();
            //return _cardFight.GetAbilityTimingData(Activation.OnDiscard, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
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

        public bool Exists(List<object> param)
        {
            SetParam(param, 1);
            return Exists(1);
        }

        public bool Exists(int paramNum)
        {
            int min = GetMin(paramNum);
            List<Card> cards = ValidCards(paramNum);
            bool orLess = GetOrLess(paramNum);
            if (cards.Count >= 0 && cards.Count >= min)
            {
                if (!HasCount(paramNum) && !HasMin(paramNum) && cards.Count < 1)
                    return false;
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
            Param customParam = _params[customParamNum];
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
            Param param = _params[paramNum];
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
            List<int> tempIDs = _cardFight.SelectCards(_player1, cards, GetCount(paramNum), GetMin(paramNum), " to give ability.");
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
            Param customParam = _params[paramNum];
            if (query == Query.Name)
                customParam.InjectName(name);
        }

        public void Inject(int paramNum, int query, int num)
        {
            Param customParam = _params[paramNum];
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
            if (cards != null && cards.Count >= _params[paramNum].Counts[0])
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

        public bool CanSpecificCB(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            int faceupCards = 0;
            foreach (Card card in cards)
            {
                if (_player1.IsFaceUp(card) && _player1.GetDamageZone().Contains(card))
                    faceupCards++;
            }
            if (_withAlchemagic && _card.orderType == OrderType.Normal && (_player1.CanAlchemagicDiff() || _player1.CanAlchemagicSame()) && _cardFight.AlchemagicableCardsAvailable(_player1, _card.tempID))
                faceupCards += _player1.AlchemagicFreeCBAvailable();
            if (faceupCards >= GetMin(paramNum))
                return true;
            return false;
        }

        public bool CanSpecificSB(List<object> param)
        {
            SetParam(param, 1);
            return CanSpecificSB(1);
        }

        public bool CanSpecificSB(int paramNum)
        {
            if (_player1.IsAlchemagic() && _player1.AlchemagicFreeSBAvailable())
                return true;
            List<Card> cards = ValidCards(paramNum);
            int soulCount = 0;
            foreach (Card card in cards)
            {
                if (_player1.GetSoul().Contains(card))
                    soulCount++;
            }
            if (soulCount >= GetMin(paramNum))
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
            if (canAddToHand != null && canAddToHand.Count >= _params[paramNum].Counts[0])
                return true;
            return false;
        }

        public bool CanAddToSoul(List<object> param)
        {
            SetParam(param, 1);
            return CanAddToSoul(1);
        }

        public bool CanAddToSoul(int paramNum)
        {
            List<Card> canAddToSoul = ValidCards(paramNum);
            if (canAddToSoul != null && canAddToSoul.Count >= GetCount(paramNum))
                return true;
            return false;
        }

        public bool CanAddToDrop(List<object> param)
        {
            SetParam(param, 1);
            return CanAddToDrop(1);
        }

        public bool CanAddToDrop(int paramNum)
        {
            List<Card> canAddToDrop = ValidCards(paramNum);
            if (canAddToDrop != null && canAddToDrop.Count >= GetCount(paramNum))
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
            if (count < _params[paramNum].Counts[0])
                return false;
            return true;
        }

        public bool CanRetire(List<object> param)
        {
            SetParam(param, 1);
            return CanRetire(1);
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

        public bool CanSpecificDiscard(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            int inHand = 0;
            foreach (Card card in cards)
            {
                if (_player1.GetHand().Contains(card))
                    inHand++;
            }
            if (inHand > 0 && inHand >= GetMin(paramNum))
                return true;
            return false;
        }

        public bool CanReveal(List<object> param)
        {
            SetParam(param, 1);
            return CanReveal(1);
        }

        public bool CanReveal(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            if (cards != null && cards.Count > 0 && cards.Count >= _params[paramNum].Counts[0])
                return true;
            return false;
        }

        public bool CanRest(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            if (cards.Count > 0 && cards.Count >= GetMin(paramNum))
            {
                foreach (Card card in cards)
                {
                    if (!_player1.IsUpRight(card))
                        return false;
                }
                return true;
            }
            return false;
        }

        public bool IsAttackingUnit()
        {
            if (_player1.AttackingUnit() != null && _card.tempID == _player1.AttackingUnit().tempID)
                return true;
            return false;
        }

        public bool IsAttackingUnit(int tempID)
        {
            return _player1.AttackingUnit() != null && tempID == _player1.AttackingUnit().tempID;
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

        public bool IsBoosted()
        {
            if (_player1.Booster() != null && _player1.GetAttacker() != null && _player1.GetAttacker().tempID == _card.tempID)
                return true;
            return false;
        }

        public bool IsVanguard()
        {
            if (_card.tempID == _player1.Vanguard().tempID)
                return true;
            return false;
        }

        public bool IsRearguard(int tempID)
        {
            return _player1.IsRearguard(tempID);
        }

        public bool IsRearguard()
        {
            if (_player1.IsRearguard(_card.tempID))
                return true;
            return false;
        }

        public bool IsBackRowRearguard()
        {
            foreach (Card card in _player1.GetBackRow())
            {
                if (card.tempID == _card.tempID)
                    return true;
            }
            foreach (Card card in _player1.GetOverloadedBackRow())
            {
                if (card.tempID == _card.tempID)
                    return true;
            }
            return false;
        }

        public bool IsFrontRowRearguard()
        {
            foreach (Card card in _player1.GetPlayerFrontRow())
            {
                if (card.tempID == _card.tempID)
                    return true;
            }
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
            if (_player1.IsOverDress(_card.tempID))
                return true;
            return false;
        }

        public bool IsOverDress(int tempID)
        {
            return _player1.IsOverDress(tempID);
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
            return _player1.Grade(_player1.Vanguard().tempID);
        }

        public int EnemyVanguardGrade()
        {
            return _player2.Grade(_player2.Vanguard().tempID);
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
            return IsApplicable();
            //return _cardFight.GetAbilityTimingData(Activation.OnStand, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
        }

        public bool StoodByCardEffectThisTurn()
        {
            return _player1.StoodByCardEffectThisTurn(_card.tempID);
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

        public List<int> SuperiorCall(List<object> param)
        {
            SetParam(param, 1);
            return SuperiorCall(1);
        }

        public List<int> SuperiorCall(int paramNum)
        {
            return SuperiorCallToSpecificCircle(paramNum, -1);
        }

        public List<int> SuperiorCallToSpecificCircle(List<object> param, params int[] circles)
        {
            SetParam(param, 1);
            return SuperiorCallToSpecificCircle(1, circles);
        }

        public List<int> SuperiorCallToSpecificCircle(int paramNum, params int[] circles)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            if (circles.Length > 0 && circles[0] == -1)
                circles = null;
            int count = GetCount(paramNum);
            int min = GetMin(paramNum);
            if (count == -1)
                count = cardsToSelect.Count;
            if (min == -1)
                min = 0;
            if (IsPayingCost())
            {
                foreach (Card card in cardsToSelect)
                    _calledForCost.Add(card.tempID);
            }
            _lastCalled.AddRange(_cardFight.SuperiorCall(_player1, _player2, cardsToSelect, count, min, circles, false, true, false, false));
            return ConvertToTempIDs(_lastCalled);
        }

        public List<int> SuperiorCallAsRest(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            int count = GetCount(paramNum);
            int min = GetMin(paramNum);
            if (count == -1)
                count = cardsToSelect.Count;
            if (min == -1)
                min = 0;
            List<Card> cards = _cardFight.SuperiorCall(_player1, _player2, cardsToSelect, count, min, null, false, false, false, false);
            return ConvertToTempIDs(cards);
        }

        public List<int> SuperiorOverDress(int paramNum, int paramNum2)
        {
            List<Card> fromHand = ValidCards(paramNum);
            int circle = _player1.GetCircle(ValidCards(paramNum2)[0]);
            List<int> circles = new List<int>();
            circles.Add(circle);
            List<Card> cards = _cardFight.SuperiorCall(_player1, _player2, fromHand, 1, 1, circles.ToArray(), true, true, false, false);
            return ConvertToTempIDs(cards);
        }

        public List<int> SuperiorCallToDifferentRows(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            int count = GetCount(paramNum);
            int min = GetMin(paramNum);
            if (count == -1)
                count = cardsToSelect.Count;
            if (min == -1)
                min = 0;
            List<Card> cards = _cardFight.SuperiorCall(_player1, _player2, cardsToSelect, count, min, null, false, true, false, true);
            return ConvertToTempIDs(cards);
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

        public void SpecificCounterBlast(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            List<Card> canCB = new List<Card>();
            foreach (Card card in cards)
            {
                if (_player1.GetDamageZone().Contains(card) && _player1.IsFaceUp(card))
                    canCB.Add(card);
            }
            _cardFight.CounterBlast(_player1, _player2, canCB, GetCount(paramNum), GetMin(paramNum));
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

        public void SpecificSoulBlast(List<object> param)
        {
            SetParam(param, 1);
            SpecificSoulBlast(1);
        }

        public void SpecificSoulBlast(int paramNum)
        {
            List<Card> canSB = new List<Card>();
            foreach (Card card in ValidCards(paramNum))
            {
                if (_player1.GetSoul().Contains(card))
                    canSB.Add(card);
            }
            _cardFight.SoulBlast(_player1, _player2, canSB, GetMin(paramNum));
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

        public List<int> Search(List<object> param)
        {
            SetParam(param, 1);
            return Search(1);
        }

        public List<int> Search(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            return _cardFight.Search(_player1, _player2, cardsToSelect);
        }

        public List<int> ChooseAddToHand(List<object> param)
        {
            SetParam(param, 1);
            return ChooseAddToHand(1);
        }

        public List<int> ChooseAddToHand(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            return _cardFight.AddToHand(_player1, _player2, cardsToSelect, GetCount(1), GetMin(1));
        }

        public void AddToHand(List<object> param)
        {
            SetParam(param, 1);
            AddToHand(1);
        }

        public void AddToHand(int paramNum)
        {
            List<Card> cardsToAdd = ValidCards(paramNum);
            List<int> IDs = new List<int>();
            foreach (Card card in cardsToAdd)
                IDs.Add(card.tempID);
            _player1.AddToHand(IDs);
        }

        public void ChooseAddToSoul(List<object> param)
        {
            SetParam(param, 1);
            ChooseAddToSoul(1);
        }

        public void ChooseAddToSoul(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            if (!HasCount(paramNum) && !HasMin(paramNum))
                AddToSoul(paramNum);
            else
                _cardFight.AddToSoul(_player1, _player2, cardsToSelect, _params[paramNum].Counts[0], _params[paramNum].Counts[0]);
        }

        public void AddToSoul(List<object> param)
        {
            SetParam(param, 1);
            AddToSoul(1);
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

        public void AddToDrop(List<object> param)
        {
            SetParam(param, 1);
            AddToDrop(1);
        }

        public void AddToDrop(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<int> cardsToDrop = new List<int>();
            foreach (Card card in cardsToSelect)
                cardsToDrop.Add(card.tempID);
            _player1.AddToDrop(cardsToDrop);
        }

        public List<int> ChooseAddToDrop(List<object> param)
        {
            SetParam(param, 1);
            return ChooseAddToDrop(1);
        }

        public List<int> ChooseAddToDrop(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            if (!HasCount(paramNum) && !HasMin(paramNum))
                AddToSoul(paramNum);
            List<int> cardsToDrop = _cardFight.SelectCards(_player1, cardsToSelect, GetCount(paramNum), GetMin(paramNum), " to send to drop.");
            _player1.AddToDrop(cardsToDrop);
            return cardsToDrop;
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

        public void SpecificDiscard(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.Discard(_player1, _player2, cards, GetCount(paramNum), GetMin(paramNum));
        }

        public bool WasRetiredForPlayerCost()
        {
            return IsApplicable();
            //return _cardFight.GetAbilityTimingData(Activation.OnRetiredForPlayerCost, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
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

        public void ChooseRetire(List<object> param)
        {
            SetParam(param, 1);
            ChooseRetire(1);
        }

        public void ChooseRetire(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<Card> canRetire = new List<Card>();
            foreach (Card card in cardsToSelect)
            {
                if (!(card.originalOwner != _player1._playerID && _player1.CardStates.HasState(card.tempID, CardState.Resist)))
                    canRetire.Add(card);
            }
            if (!HasCount(paramNum) && !HasMin(paramNum))
                _cardFight.Retire(_player1, _player2, _player1.ConvertToTempIDs(canRetire));
            else
                _cardFight.SelectCardToRetire(_player1, _player2, canRetire, _params[paramNum].Counts[0], false); 
        }

        public bool CanBind(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            if (cards.Count >= GetMin(paramNum))
                return true;
            return false;
        }

        public void Bind(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            List<int> tempIDs = new List<int>();
            foreach (Card card in cards)
                tempIDs.Add(card.tempID);
            _cardFight.Bind(_player1, tempIDs);
        }

        public void ChooseBind(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            if (!HasCount(paramNum) && !HasMin(paramNum))
                _cardFight.Bind(_player1, _player1.ConvertToTempIDs(cards));
            else
                _cardFight.ChooseBind(_player1, cards, GetCount(paramNum), GetMin(paramNum));
        }

        public void Retire(List<object> param)
        {
            SetParam(param, 1);
            Retire(1);
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

        public bool ChooseSendToBottom(int paramNum)
        {
            return ChooseSendToBottom(paramNum, false);
        }

        public bool ChooseSendToBottom(int paramNum, bool cost)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            int max = GetCount(paramNum);
            int min = GetMin(paramNum);
            if (!HasCount(paramNum) && !HasMin(paramNum))
                return SendToBottom(paramNum);
            else
            {
                return _cardFight.ChooseSendToBottom(_player1, _player2, cardsToSelect, max, min, cost);
            }
        }

        public void RevealAndSendToBottom(int paramNum)
        {
            Select(paramNum);
            _player1.Reveal(_player1.ConvertToTempIDs(_selected));
            _player1.SendToDeck(_player1.ConvertToTempIDs(_selected), true);
        }
        
        public bool SendToBottom(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            List<int> cardsToSend = new List<int>();
            foreach (Card card in cards)
            {
                cardsToSend.Add(card.tempID);
            }
            _player1.SendToDeck(cardsToSend, true);
            return true;
        }

        public void ChooseSendToTop(int paramNum)
        {
            ChooseSendToTop(paramNum, false);
        }

        public void ChooseSendToTop(int paramNum, bool cost)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            int max = GetCount(paramNum);
            int min = GetMin(paramNum);
            if (!HasCount(paramNum) && !HasMin(paramNum))
                SendToTop(paramNum);
            else
                _cardFight.ChooseSendToTop(_player1, _player2, cardsToSelect, max, min, cost);
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

        public List<int> ChooseStand(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<Card> canStand = new List<Card>();
            foreach (Card card in cardsToSelect)
            {
                if (!_player1.IsUpRight(card))
                    canStand.Add(card);
            }
            return _player1.ConvertToTempIDs(_cardFight.Stand(_player1, _player2, canStand, _params[paramNum].Counts[0], true));
        }

        public void Stand(List<object> param)
        {
            SetParam(param, 1);
            Stand(1);
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

        public List<int> ChooseRest(List<object> param)
        {
            SetParam(param, 1);
            return ChooseRest(1);
        }

        public List<int> ChooseRest(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<Card> canRest = new List<Card>();
            foreach (Card card in cardsToSelect)
            {
                if (_player1.IsUpRight(card))
                    canRest.Add(card);
            }
            if (!HasCount(paramNum) && !HasMin(paramNum))
                return _player1.Rest(_player1.ConvertToTempIDs(canRest));
            else
                return _cardFight.Rest(_player1, _player2, canRest, GetCount(paramNum), true);
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
            _cardFight.SuperiorCall(_player1, _player2, cardsToSelect, count, min, null, false, true, true, false);
        }

        public void ChooseReveal(List<object> param)
        {
            SetParam(param, 1);
            ChooseReveal(1);
        }

        public void ChooseReveal(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            _cardFight.ChooseReveal(_player1, _player2, cardsToSelect, GetCount(1), GetMin(1));
        }

        public void Reveal(List<object> param)
        {
            SetParam(param, 1);
            Reveal(1);
        }

        public void Reveal(int paramNum)
        {
            _player1.Reveal(_player1.ConvertToTempIDs(ValidCards(paramNum)));
        }

        public void RevealFromDeck(int count)
        {
            _cardFight.RevealFromDeck(_player1, _player2, count);
        }

        public void EndReveal()
        {
            _cardFight.EndReveal(_player1, _player2);
        }

        public void BindTopOfDeck(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (_player1.GetDeck().Count > 0)
                {
                    List<int> list = new List<int>();
                    list.Add(_player1.GetDeck()[0].tempID);
                    _player1.Bind(list);
                }
            }
        }

        public void LookAtTopOfDeck(int count)
        {
            _player1.LookAtTopOfDeck(count);
        }

        public void RearrangeOnTop(List<object> param)
        {
            SetParam(param, 1);
            RearrangeOnTop(1);
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

        public void DisplayCards(List<object> param)
        {
            SetParam(param, 1);
            DisplayCards(1);
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

        public int NumPlayerOpenCircles()
        {
            return _player2.NumEnemyOpenCircles();
        }

        public void RearguardDriveCheck()
        {
            _player1.RearguardDriveCheck();
        }

        public bool LastRevealedTrigger()
        {
            return IsApplicable();
            //return _cardFight.GetAbilityTimingData(_currentActivation, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
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
            _cardFight.ChooseAddTempPower(_player1, _player2, cards, power, _params[paramNum].Counts[0]);
        }

        public void ChooseAddBattleOnlyPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.ChooseAddBattleOnlyPower(_player1, _player2, cards, power, _params[paramNum].Counts[0]);
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

        public void AddSkillUntilEndOfTurn(int paramNum, int skill)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddSkillUntilEndOfTurn(card.tempID, skill);
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

        public bool IsBattlePhase()
        {
            if (_cardFight.GetPhase() == Phase.Battle)
                return true;
            return false;
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

        public List<int> Select(List<object> param)
        {
            SetParam(param, 1);
            return Select(1);
        }


        public List<int> Select(int paramNum, string query, bool auto)
        {
            _selected.Clear();
            List<Card> cardsToSelect = ValidCards(paramNum);
            int min = GetMin(paramNum);
            if (min < 0)
                min = GetCount(paramNum);
            bool sameName = false;
            if (_params[paramNum].Others.Contains(Other.SameName))
                sameName = true;
            List<int> selectedCards;
            if (!auto)
                selectedCards = _cardFight.SelectCards(_player1, cardsToSelect, GetCount(paramNum), min, query, sameName);
            else
                selectedCards = ConvertToTempIDs(cardsToSelect);
            foreach (int tempID in selectedCards)
                _selected.Add(_player1.GetCard(tempID));
            return selectedCards;
        }

        public List<int> AutoSelect(List<object> param)
        {
            SetParam(param, 1);
            return Select(1, "", true);
        }

        public List<int> Select(int paramNum)
        {
            return Select(paramNum, "", false);
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

        public void ChooseImprison(List<object> param)
        {
            SetParam(param, 1);
            ChooseImprison(1);
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

        //public void DarkNight()
        //{
        //    _player1.DarkNight();
        //}

        //public void AbyssalDarkNight()
        //{
        //    _player1.AbyssalDarkNight();
        //}

        public void NoWorld()
        {
            
        }

        public bool WorldPlayed()
        {
            return _player1.WorldPlayed();
        }

        public bool LastPutOnOrderZoneIsWorld()
        {
            return false;
            //return _cardFight.GetAbilityTimingData(Activation.PutOnOrderZone, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => _player1.GetCard(snapshot.tempID).orderType == OrderType.World));
        }

        public bool LastPutOnOrderZone()
        {
            return IsApplicable();
            //return _cardFight.GetAbilityTimingData(Activation.PutOnOrderZone, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID));
        }

        public void CallToken(string tokenID)
        {
            int token = _player1.CreateToken(tokenID);
            List<Card> toBeCalled = new List<Card>();
            toBeCalled.Add(_player1.GetCard(token));
            toBeCalled[0].originalOwner = _player1._playerID;
            _cardFight.SuperiorCall(_player1, _player2, toBeCalled, 1, 1, null, false, true, false, false);
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

        public bool WasAlchemagic()
        {
            if (_currentActivation == Activation.OnOrderPlayed)
            {
                AbilityTimingData data = _cardFight.GetAbilityTimingData(_currentActivation, _timingCount, _player1._playerID);
                return data != null && data.additionalInfo;
            }
            return false;
        }

        public bool AlchemagicUsedThisTurn()
        {
            return _player1.AlchemagicUsedThisTurn();
        }

        public List<int> GetGrades(List<object> param)
        {
            SetParam(param, 1);
            List<int> grades = new List<int>();
            foreach (Card card in ValidCards(1))
                grades.Add(_player1.Grade(card.tempID));
            return grades;
        }

        public int GetNumberOf(List<object> param)
        {
            SetParam(param, 1);
            return GetNumberOf(1);
        }

        public int GetNumberOf(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            return cards.Count;
        }

        public string GetName(List<object> param)
        {
            SetParam(param, 1);
            return GetName(1);
        }

        public string GetName(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            if (cards.Count > 0)
                return cards[0].name;
            else
                return "";
        }

        public string GetNameFromCardID(string cardID)
        {
            Card card = _cardFight.LoadCard(cardID);
            if (card != null)
                return card.name;
            return "";
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

        public void RetireAtEndOfTurn(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            _player1.RetireAtEndOfTurn(_player1.ConvertToTempIDs(cards));
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

        public void AddContinuousPlayerState(int state)
        {
            _player1.MyStates.AddContinuousState(state);
        }

        public void AddPermanentPlayerState(int state)
        {
            _player1.MyStates.AddPermanentState(state);
        }

        public void AddContinuousEnemyValue(int state, int value)
        {
            _player2.MyStates.AddContinuousValue(state, value);
        }

        public void AddUntilEndOfBattleEnemyValue(int state, int value)
        {
            _player2.MyStates.AddUntilEndOfBattleValue(state, value);
        }

        public void AddUntilEndOfTurnEnemyState(int state)
        {
            _player2.MyStates.AddUntilEndOfTurnState(state);
        }

        public void AddUntilEndOfTurnPlayerState(int state)
        {
            _player1.MyStates.AddUntilEndOfTurnState(state);
        }

        public void IncrementUntilEndOfTurnPlayerValue(int state, int value)
        {
            _player1.MyStates.IncrementUntilEndOfTurnValue(state, value);
        }

        public void AddUntilEndOfTurnState(int paramNum, int state)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.CardStates.AddUntilEndOfTurnState(card.tempID, state);
            }
        }

        public void AddUntilEndOfNextTurnState(int paramNum, int state)
        {
            foreach (Card card in ValidCards(paramNum))
                _player1.CardStates.AddUntilEndOfNextTurnState(card.tempID, state);
        }

        public void AddUntilEndOfBattleValue(int paramNum, int state, int value)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.CardStates.AddUntilEndOfBattleValue(card.tempID, state, value);
            }
        }

        public void AddUntilEndOfBattleState(int paramNum, int state)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.CardStates.AddUntilEndOfBattleState(card.tempID, state);
            }
        }

        public void AddUntilEndOfTurnValue(int paramNum, int state, int value)
        {
            foreach (Card card in ValidCards(paramNum))
            {
                _player1.CardStates.AddUntilEndOfTurnValue(card.tempID, state, value);
            }
        }

        public bool IsWhiteWings()
        {
            //return true;
            if (_player1.MyStates.HasState(PlayerState.BlackAndWhiteWingsActive))
                return true;
            if (_player1.GetBind().Count > 0)
            {
                foreach (Card card in _player1.GetBind())
                {
                    if (_player1.Grade(card.tempID) % 2 == 0)
                        return false;
                }
                return true;
            }
            return false;
        }

        public bool IsBlackWings()
        {
            if (_player1.MyStates.HasState(PlayerState.BlackAndWhiteWingsActive))
                return true;
            if (_player1.GetBind().Count > 0)
            {
                foreach (Card card in _player1.GetBind())
                {
                    if (_player1.Grade(card.tempID) % 2 != 0)
                        return false;
                }
                return true;
            }
            return false;
        }

        public void AddContinuousValue(int targetParamNum, int state, int valueParamNum)
        {
            List<Card> targets = ValidCards(targetParamNum);
            List<Card> values = ValidCards(valueParamNum);
            foreach (Card target in targets)
            {
                foreach (Card value in values)
                {
                    _player1.CardStates.AddContinuousValue(target.tempID, state, value.tempID);
                }
            }
        }

        public List<int> ChoosePutIntoOrderZone(List<object> param)
        {
            SetParam(param, 1);
            List<int> selected = _cardFight.SelectCards(_player1, ValidCards(1), GetCount(1), GetMin(1), "to put into Order Zone");
            _player1.PutIntoOrderZone(selected);
            return selected;
        }

        public bool PutIntoOrderZone(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            _player1.PutIntoOrderZone(_player1.ConvertToTempIDs(cards));
            _cardFight.PutIntoOrderZone(_player1);
            return true;
        }

        public void Sing(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.Sing(_player1, cards, GetCount(paramNum));
        }

        public void Store(List<int> tempIDs)
        {
            _stored.Clear();
            _stored.AddRange(tempIDs);
        }

        public int Stored()
        {
            return -1;
        }

        public bool IsInOrderZone()
        {
            return _player1.GetOrderZone().Exists(card => card.tempID == _card.tempID);
        }

        public void ChooseFlipFaceUp(int paramNum)
        {
            ChooseFlip(paramNum, true);
        }

        public void ChooseFlip(int paramNum, bool faceup)
        {
            List<Card> cards = ValidCards(paramNum);
            List<Card> cardsToFlip = new List<Card>();
            foreach (Card card in cards)
            {
                if (faceup && !_player1.IsFaceUp(card))
                    cardsToFlip.Add(card);
            }
            _cardFight.ChooseFlip(_player1, cards, GetCount(paramNum), GetMin(paramNum), faceup);
        }

        public bool PlayerPlayedOrder()
        {
            return GetSnapshottedCards(0).Count > 0;
            //return _cardFight.GetAbilityTimingData(Activation.OnOrderPlayed, _timingCount, _player1._playerID).Exists(data => data.GetRelevantSnapshots(0).Count > 0);
        }

        public bool IsApplicable()
        {
            return GetSnapshottedCards(0).Exists(card => card.tempID == _card.tempID);
        }

        public int GetCurrentActivation()
        {
            return _currentActivation;
        }

        public bool CurrentActivationIs(int activation)
        {
            if (activation == _currentActivation)
                return true;
            return false;
        }

        public bool PlayerHasState(int state)
        {
            return _player1.MyStates.HasState(state);
        }

        public int GetOriginalShieldOf(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            if (cards.Count > 0)
                return cards[0].shield;
            return 0;
        }

        public int GetTotalOriginalPower(List<object> param)
        {
            SetParam(param, 1);
            return GetTotalOriginalPower(1);
        }

        public int GetTotalOriginalPower(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            int power = 0;
            foreach (Card card in cards)
                power += card.power;
            return power;
        }

        public void AddCardState(List<object> param, int state, int duration)
        {
            SetParam(param, 1);
            foreach (Card card in ValidCards(1))
            {
                int tempID = card.tempID;
                if (duration == Property.Continuous)
                    _player1.CardStates.AddContinuousState(tempID, state);
                else if (duration == Property.UntilEndOfBattle)
                    _player1.CardStates.AddUntilEndOfBattleState(tempID, state);
                else if (duration == Property.UntilEndOfTurn)
                    _player1.CardStates.AddUntilEndOfTurnState(tempID, state);
            }
        }

        public void AddCardValue(List<object> param, int state, int value, int duration)
        {
            SetParam(param, 1);
            foreach (Card card in ValidCards(1))
            {
                int tempID = card.tempID;
                if (duration == Property.Continuous)
                    _player1.CardStates.AddContinuousValue(tempID, state, value);
                else if (duration == Property.UntilEndOfBattle)
                    _player1.CardStates.AddUntilEndOfBattleValue(tempID, state, value);
                else if (duration == Property.UntilEndOfTurn)
                    _player1.CardStates.AddUntilEndOfTurnValue(tempID, state, value);
            }
        }

        public void AddEnemyState(int state, int duration)
        {
            AddPlayerState(state, duration, false);
        }

        public void AddPlayerState(int state, int duration)
        {
            AddPlayerState(state, duration, true);
        }

        public void AddPlayerState(int state, int duration, bool player)
        {
            Player targetPlayer = null;
            if (player)
                targetPlayer = _player1;
            else
                targetPlayer = _player2;
            if (duration == Property.UntilEndOfBattle)
                targetPlayer.MyStates.AddUntilEndOfBattleState(state);
        }

        public void AddPlayerValue(int state, int value, int duration)
        {
            AddPlayerValue(state, value, duration, true);
        }

        public void AddPlayerValue(int state, int value, int duration, bool player)
        {
            Player targetPlayer;
            if (player)
                targetPlayer = _player1;
            else
                targetPlayer = _player2;
            if (duration == Property.Continuous)
                targetPlayer.MyStates.AddContinuousValue(state, value);
            else if (duration == Property.UntilEndOfBattle)
                targetPlayer.MyStates.AddUntilEndOfBattleValue(state, value);
            else if (duration == Property.UntilEndOfTurn)
                targetPlayer.MyStates.AddUntilEndOfTurnValue(state, value);
        }

        public int NumOfAttacks()
        {
            return _player1.NumOfAttacks();
        }

        public int GradeOf(int tempID)
        {
            return _player1.Grade(tempID);
        }

        public void Track(int tempID)
        {
            if (!_tracking.Contains(tempID))
                _tracking.Add(tempID);
        }

        public bool IsSameZone()
        {
            return IsSameZone(_card.tempID);
        }

        public bool IsSameZone(int tempID)
        {
            //if (_tracking.Contains(tempID))
            //{
            //    List<AbilityTimingData> abilityTimingData = _cardFight.GetAbilityTimingData(_currentActivation, _timingCount, _player1._playerID);
            //    return abilityTimingData.Exists(data => data.allSnapshots[tempID].fieldID == _player1.GetFieldID(tempID));
            //}
            //return false;
            List<AbilityTimingData> list = _cardFight.GetAbilityTimingData(_currentActivation, _timingCount.Item1, _player1._playerID);
            return list.Exists(data => data.allSnapshots[tempID].fieldID == _player1.GetFieldID(tempID));
        }

        public string GetName(string name)
        {
            return ConfigurationManager.AppSettings.Get(name);
        }

        List<Card> GetSnapshottedCards(int index)
        {
            List<Card> cards = new List<Card>();
            AbilityTimingData data = null;
            List<AbilityTimingData> list = _cardFight.GetAbilityTimingData(_currentActivation, _timingCount.Item1, _player1._playerID);
            if (list.Count > 0 && list.Count >= _timingCount.Item2)
                data = list[_timingCount.Item2 - 1];
            else
                return cards;
            //foreach (AbilityTimingData data in _cardFight.GetAbilityTimingData(_currentActivation, _timingCount.Item1, _player1._playerID))
            //{
            //    if (_sourceIsRelevant && data.abilitySource != null)
            //    {
            //        if ((_sourceIsPlayer && _player1.GetCard(data.abilitySource.tempID).originalOwner != _player1._playerID) ||
            //                (!_sourceIsPlayer && _player1.GetCard(data.abilitySource.tempID).originalOwner == _player1._playerID))
            //            continue;
            //        if (_sourceLocation != -1 && _sourceLocation != data.abilitySource.location)
            //            continue;
            //    }
            //    foreach (Snapshot snapshot in data.GetRelevantSnapshots(index))
            //    {
            //        if (_movedTo == -1 && _movedFrom == -1)
            //            cards.Add(_player1.GetCard(snapshot.tempID));
            //        else if (_movedTo != -1 && _movedFrom != -1)
            //        {
            //            if (_movedTo == snapshot.location && _movedFrom == snapshot.previousLocation)
            //                cards.Add(_player1.GetCard(snapshot.tempID));
            //            else
            //                continue;
            //        }
            //        else if (_movedTo != -1 && _movedTo == snapshot.location)
            //            cards.Add(_player1.GetCard(snapshot.tempID));
            //        else if (_movedFrom != -1 && _movedFrom == snapshot.previousLocation)
            //            cards.Add(_player1.GetCard(snapshot.tempID));
            //    }
            //}
            if (_sourceIsRelevant && data.abilitySource != null)
            {
                if ((_sourceIsPlayer && _player1.GetCard(data.abilitySource.tempID).originalOwner != _player1._playerID) ||
                        (!_sourceIsPlayer && _player1.GetCard(data.abilitySource.tempID).originalOwner == _player1._playerID))
                    return cards;
                if (_sourceLocation != -1 && _sourceLocation != data.abilitySource.location)
                    return cards;
            }
            foreach (Snapshot snapshot in data.GetRelevantSnapshots(index))
            {
                if (_movedTo == -1 && _movedFrom == -1)
                    cards.Add(_player1.GetCard(snapshot.tempID));
                else if (_movedTo != -1 && _movedFrom != -1)
                {
                    if (_movedTo == snapshot.location && _movedFrom == snapshot.previousLocation)
                        cards.Add(_player1.GetCard(snapshot.tempID));
                    else
                        continue;
                }
                else if (_movedTo != -1 && _movedTo == snapshot.location)
                    cards.Add(_player1.GetCard(snapshot.tempID));
                else if (_movedFrom != -1 && _movedFrom == snapshot.previousLocation)
                    cards.Add(_player1.GetCard(snapshot.tempID));
            }
            return cards;
        }

        public int GetPlayerDamage()
        {
            return _player1.GetDamageZone().Count;
        }

        public bool SourceIsVanguardAbility()
        {
            AbilityTimingData data = null;
            List<AbilityTimingData> list = _cardFight.GetAbilityTimingData(_currentActivation, _timingCount.Item1, _player1._playerID);
            if (list.Count > 0 && list.Count >= _timingCount.Item2)
                data = list[_timingCount.Item2 - 1];
            else
                return false;
            if (data.abilitySource != null && data.abilitySource.location == Location.VC &&
                    _player1.GetCard(data.abilitySource.tempID).originalOwner == _player1._playerID &&
                    data.GetRelevantSnapshots(0).Exists(snapshot => snapshot.tempID == _card.tempID))
                return true;
            else
                return false;
        }

        public List<int> ConvertToTempIDs(List<Card> cards)
        {
            List<int> tempIDs = new List<int>();
            foreach (Card card in cards)
                tempIDs.Add(card.tempID);
            return tempIDs;
        }
    }

    public class TimingCount
    {
        Tuple<int, int> _timingCount = new Tuple<int, int>(0, 0);
        Tuple<int, int> _conditionalTimingCount = new Tuple<int, int>(0, 0);
        bool _conditionalMode = false;

        public void SetTimingCount(Tuple<int, int> timingCount)
        {
            if (timingCount == null)
                timingCount = new Tuple<int, int>(0, 0);
            if (_conditionalMode)
                _conditionalTimingCount = timingCount;
            else
                _timingCount = timingCount;
        }

        public void SetConditionalMode()
        {
            _conditionalMode = true;
        }

        public void EndConditionalMode()
        {
            _conditionalMode = false;
        }

        public Tuple<int, int> GetTimingCount()
        {
            if (_conditionalMode)
                return _conditionalTimingCount;
            else
                return _timingCount;
        }
    }

    public class Param
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
        List<int> _race = new List<int>();
        List<int> _maxPower = new List<int>();
        List<int> _power = new List<int>();
        List<int> _snapshotIndex = new List<int>();

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

        public void AddRace(int race)
        {
            _race.Add(race);
        }

        public void AddMaxPower(int maxPower)
        {
            _maxPower.Add(maxPower);
        }

        public void AddPower(int power)
        {
            _power.Add(power);
        }

        public void AddSnapshotIndex(int index)
        {
            _snapshotIndex.Add(index);
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

        public List<int> Races
        {
            get => _race;
        }

        public List<int> MaxPowers
        {
            get => _maxPower;
        }

        public List<int> Powers
        {
            get => _power;  
        }

        public List<int> SnapshotIndexes
        {
            get => _snapshotIndex;
        }
    }

    class Activation
    {
        public const int OnRide = -1;
        public const int OnAttack = -2;
        public const int OnOverDress = -3;
        public const int OnACT = -4;
        public const int OnBattleEnds = -5;
        public const int OverDress = -6;
        public const int Then = -7;
        public const int PlacedOnGC = -8;
        public const int PlacedOnRC = -9;
        public const int OnDriveCheck = -10;
        public const int OnOrder = -11;
        public const int Cont = -12;
        public const int PlacedOnVC = -13;
        public const int OnRidePhase = -14;
        public const int OnBlitzOrder = -15;
        public const int OnCallFromPrison = -16;
        public const int OnAttackHits = -17;
        public const int OnAttackHitsVanguard = -18;
        public const int OnEnemyRetired = -19;
        public const int OnOrderPlayed = -20;
        public const int OnOverTrigger = -21;
        public const int PlacedOnRCFromHand = -22;
        public const int OnDiscard = -23;
        public const int OnStand = -24;
        public const int OnChosen = -25;
        public const int PutOnGC = -26;
        public const int OnBattlePhase = -27;
        public const int OnEndPhase = -28;
        public const int OnRetiredForPlayerCost = -29;
        public const int PutOnOrderZone = -30;
        public const int PlacedOnRCFromPrison = -31;
        public const int OnPlayerRCRetired = -32;
        public const int OnMainPhase = -33;
        public const int OnSing = -34;
        public const int OnBind = -35;
        public const int PlacedOnRCOtherThanFromHand = -36;
        public const int OnRearguardReturnedToHand = -37;
        public const int OnPutIntoSoulFromRC = -38;
        public const int OnPut = -39;
        public const int RideDeckDiscardReplace = -40;
        public const int OnPlayerRetired = -41;
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
        public const int RevealedDamageChecks = 52;
        public const int RevealedDriveChecks = 53;
        public const int CalledForCost = 54;
        public const int BackRowEnemyRC = 55;
        public const int PlayerRCRetired = 56;
        public const int EnemyUnits = 57;
        public const int LastOrderPlayed = 58;
        public const int Applicable = 59;
        public const int SameColumn = 60;
        public const int PlayedOrdersThisTurn = 61;
        public const int MyOriginalDress = 62;
        public const int Stored = 63;
        public const int BackRowRC = 64;
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
        public const int Race = 12;
        public const int MaxPower = 13;
        public const int Power = 14;
        public const int SnapshotIndex = 15;
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
        public const int GradeOrHigher = 24;
        public const int Boosting = 21;
        public const int Locked = 22;
        public const int SameColumn = 23;
        public const int Shuffled = 24;
        public const int DifferentNames = 25;
        public const int Song = 26;
        public const int Revealed = 27;
        public const int Gem = 28;
        public const int Player = 29;
        public const int EvenGrade = 30;
        public const int OddGrade = 31;
        public const int Selected = 32;
        public const int SameName = 33;
        public const int Attacked = 34;
        public const int Meteorite = 35;
        public const int ThisFieldID = 36;
        public const int NotThisFieldID = 37;
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
        public const int Rest = 12;
        public const int SpecificCB = 13;
        public const int SpecificSB = 14;
        public const int AddToDrop = 15;
        public const int SpecificDiscard = 16;
        public const int ResultOf = 17;
        public const int Bind = 18;
        public const int AddToDamageZone = 19;
        public const int Given = 20;
        public const int Call = 21;
        public const int WhiteWings = 22;
        public const int BlackWings = 23;
        public const int SendToTop = 24;
        public const int CostNotRequired = 25;
        public const int SendToBottom = 26;
        public const int RevealAndSendToBottom = 27;
        public const int Continuous = 28;
        public const int UntilEndOfBattle = 29;
        public const int UntilEndOfTurn = 30;
        public const int Repeatable = 31;
        public const int NotMandatory = 32;
    }
}
