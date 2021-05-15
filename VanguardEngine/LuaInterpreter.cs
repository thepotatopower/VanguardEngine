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
        public InputManager inputManager;
        
        public LuaInterpreter(string path, InputManager input)
        {
            luaPath = path;
            inputManager = input;
        }

        public List<Effect> GetEffects(Card card, Player player1, Player player2)
        {
            List<Effect> effects = new List<Effect>();
            Effect effect;
            Script script;
            string filePath = card.id.Replace("/", "");
            filePath = luaPath + "/" + filePath + ".lua";
            Console.WriteLine(filePath);
            if (!File.Exists(filePath))
            {
                Console.WriteLine("no lua script for " + filePath);
                return effects;
            }
            Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.FileSystemScriptLoader();
            script = new Script();
            UserData.RegisterType<Effect>();
            UserData.RegisterType<Location>();
            UserData.RegisterType<Activation>();
            UserData.RegisterType<ContEnd>();
            UserData.RegisterType<PromptType>();
            DynValue l = UserData.Create(new Location());
            DynValue a = UserData.Create(new Activation());
            DynValue c = UserData.Create(new ContEnd());
            DynValue p = UserData.Create(new Prompt());
            DynValue q = UserData.Create(new Query());
            DynValue t = UserData.Create(new UnitType());
            script.Globals.Set("l", l);
            script.Globals.Set("a", a);
            script.Globals.Set("c", c);
            script.Globals.Set("p", p);
            script.Globals.Set("q", q);
            script.Globals.Set("t", t);
            script.DoFile(filePath);
            DynValue numberOfEffects = script.Call(script.Globals["NumberOfEffects"]);
            for (int i = 0; i < numberOfEffects.Number; i++)
            {
                effect = new Effect(player1, player2, inputManager, card);
                effect.StoreEffect(script, i + 1);
                effects.Add(effect);
            }
            return effects;
        }
    }

    public class Effect
    {
        Player _player1;
        Player _player2;
        InputManager _inputManager;
        Card _card; 
        bool _isMandatory = true;
        bool _needsPrompt = true;
        bool _isSuperiorCall = false;
        bool _activated = false;
        bool _active = true;
        int _activation;
        List<int> _location = new List<int>();
        bool _cont;
        int _effectType;
        int _effectNumber;
        Script _script;
        DynValue _effectActivate;
        DynValue _checkCondition;
        List<Param> _params = new List<Param>();

        public Effect(Player player1, Player player2, InputManager inputManager, Card card)
        {
            _player1 = player1;
            _player2 = player2;
            _inputManager = inputManager;
            _card = card;
        }

        public void StoreEffect(Script script, int num)
        {
            _script = script;
            _effectNumber = num;
            Param param;
            DynValue numOfParams = script.Call(script.Globals["NumberOfParams"], _effectNumber);
            DynValue returnedParam;
            for (int i = 0; i < numOfParams.Number; i++)
            {
                returnedParam = script.Call(script.Globals["GetParam"], _effectNumber, i + 1);
                param = new Param();
                for (int j = 0; j < returnedParam.Tuple.Length; j++)
                {
                    if (returnedParam.Tuple[j].Number == Query.Count)
                    {
                        param.AddCount((int)returnedParam.Tuple[j + 1].Number);
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
                }
                _params.Add(param);
            }
            DynValue activationRequirement = script.Call(script.Globals["ActivationRequirement"], _effectNumber);
            _activation = (int)activationRequirement.Tuple[0].Number;
            _location.Add((int)activationRequirement.Tuple[1].Number);
            int tupleLocation = 2;
            while (true)
            {
                if (activationRequirement.Tuple[tupleLocation].Type == DataType.Number)
                    _location.Add((int)activationRequirement.Tuple[tupleLocation].Number);
                else
                    break;
                tupleLocation++;
            }
            _cont = activationRequirement.Tuple[tupleLocation].Boolean;
            _isMandatory = activationRequirement.Tuple[tupleLocation + 1].Boolean;
            _checkCondition = script.Globals.Get("CheckCondition");
            _effectActivate = script.Globals.Get("Activate");
        }

        public void setScript(Script script)
        {
            _script = script;
        }

        public void setEffect(DynValue effectActivate)
        {
            _effectActivate = effectActivate;
        }

        public bool isMandatory
        {
            get => _isMandatory;
            set => _isMandatory = value;
        }

        public bool needsPrompt
        {
            get => _needsPrompt;
            set => _needsPrompt = value;
        }

        public bool isSuperiorCall
        {
            get => _isSuperiorCall;
            set => _isSuperiorCall = value;
        }

        public string Name
        {
            get => _card.name;
        }

        public void Activate()
        {
            _script.Call(_effectActivate, _effectNumber);
        }

        public bool CheckCondition()
        {
            DynValue check = _script.Call(_checkCondition, _effectNumber);
            if (check.Boolean)
                return true;
            return false;
        }

        public bool IsTopSoul()
        {
            if (_player1.IsTopSoul(_card))
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

        public bool CanDiscard(int paramNum)
        {
            Param param = _params[paramNum - 1];
            if (param.Counts.Count > 0)
            {
                if (_player1.GetHand().Count < param.Counts[0])
                    return false;
            }
            return true;
        }

        public bool CanSuperiorCall(int paramNum)
        {
            if (ValidCards(paramNum) != null)
                return true;
            return false;
        }

        public List<Card> ValidCards(int paramNum)
        {
            Param param = _params[paramNum - 1];
            List<Card> currentPool = new List<Card>();
            List<Card> newPool = new List<Card>();
            foreach (int location in param.Locations)
            {
                if (location == Location.Deck)
                    currentPool.AddRange(_player1.GetDeck());
                else if (location == Location.Drop)
                    currentPool.AddRange(_player1.GetDrop());
                else if (location == Location.Hand)
                    currentPool.AddRange(_player1.GetHand());
                else if (location == Location.PlayerRC)
                {
                    foreach (Card card in _player1.GetActiveUnits())
                        currentPool.Add(card);
                }
                else if (location == Location.PlayerVC)
                    currentPool.Add(_player1.Vanguard());
            }
            if (currentPool.Count == 0)
                return null;
            if (param.Names.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    if (param.Names.Contains(currentPool[i].name))
                        newPool.Add(currentPool[i]);
                }
                if (newPool.Count == 0)
                    return null;
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.Grades.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    if (param.Grades.Contains(currentPool[i].grade))
                        newPool.Add(currentPool[i]);
                }
                if (newPool.Count == 0)
                    return null;
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            if (param.Types.Count > 0)
            {
                for (int i = 0; i < currentPool.Count; i++)
                {
                    if (param.Types.Contains(currentPool[i].unitType))
                        newPool.Add(currentPool[i]);
                }
                if (newPool.Count == 0)
                    return null;
                currentPool.Clear();
                currentPool.AddRange(newPool);
                newPool.Clear();
            }
            return currentPool;
        }

        public bool CanCB(int paramNum)
        {
            List<Card> damage = _player1.GetDamageZone();
            int faceupCards = 0;
            foreach (Card card in damage)
            {
                if (card.faceup)
                    faceupCards++;
            }
            if (faceupCards >= _params[paramNum - 1].Counts[0])
                return true;
            return false;
        }

        public bool IsAttackingUnit()
        {
            if (_card == _player1.AttackingUnit())
                return true;
            return false;
        }

        public bool IsVanguard()
        {
            if (_card == _player1.Vanguard())
                return true;
            return false;
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
            _player2.EnemyDraw(count);
        }

        public void SuperiorCall(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum - 1);
            int selection = _inputManager.SelectFromList(cardsToSelect, "Choose card to Call.");
            int location = 0;
            foreach (Card card in cardsToSelect)
            {
                if (card.tempID == selection)
                {
                    location = card.location;
                    break;
                }
            }
            int circle = _inputManager.SelectCallLocation("Choose RC.");
            _player1.SuperiorCall(circle, selection, location, C.Player);
            _player1.SuperiorCall(circle, selection, location, C.Enemy);
        }

        public void Discard(int paramNum)
        {
            List<int> cardsToDiscard = new List<int>();
            for (int i = 0; i < _params[paramNum - 1].Counts[0]; i++)
            {
                cardsToDiscard.Add(_inputManager.SelectCardFromHand());
            }
            _player1.Discard(cardsToDiscard, C.Player);
            _player2.Discard(cardsToDiscard, C.Enemy);
        }

        public void AddPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddPower(card.tempID, power);
                _player2.AddPower(card.tempID, power);
                card.tempPower += power;
            }
        }

        public void AddTempPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddTempPower(card.tempID, power);
                _player2.AddTempPower(card.tempID, power);
                card.tempPower += power;
            }
        }

        public bool Activated()
        {
            return _activated;
        }
    }

    class Param
    {
        List<int> _location = new List<int>();
        List<int> _count = new List<int>();
        List<string> _name = new List<string>();
        List<int> _grade = new List<int>();
        List<int> _type = new List<int>();

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

        public void AddGrade(int grade)
        {
            _grade.Add(grade);
        }

        public void AddType(int type)
        {
            _type.Add(type);
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

        public List<int> Types
        {
            get => _type;
        }
    }

    class Activation
    {
        public const int OnRide = 1;
        public const int OnAttack = 2;
        public const int OnOverDress = 3;
    }

    class Prompt
    {
        public const int SuperiorCall = 1;
    }

    class ContEnd
    {
        public const int BattleEnds = 1;
    }

    public class Location
    {
        public const int TopSoul = 1;
        public const int VG = 2;
        public const int FaceupUnit = 3;
        public const int Drop = 4;
        public const int Hand = 5;
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
    }

    class Query
    {
        public const int Name = 1;
        public const int Grade = 2;
        public const int Location = 3;
        public const int Count = 4;
        public const int Type = 5;
    }
}
