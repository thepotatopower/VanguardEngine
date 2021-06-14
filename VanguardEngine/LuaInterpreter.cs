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
        
        public LuaInterpreter(string path, CardFight cf)
        {
            luaPath = path;
            cardFight = cf;
        }

        public List<Ability> GetAbilities(Card card, Player player1, Player player2)
        {
            List<Ability> abilities = new List<Ability>();
            Ability ability;
            Script script;
            string filePath = card.id.Replace("/", "_");
            filePath = filePath.Replace("-", "");
            filePath = filePath.ToLower();
            filePath = luaPath + Path.DirectorySeparatorChar + filePath + ".lua";
            if (!File.Exists(filePath))
            {
                Console.WriteLine("no lua script for " + filePath);
                return abilities;
            }
            else
                Console.WriteLine("loading " + filePath);
            Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.FileSystemScriptLoader();
            script = new Script();
            UserData.RegisterType<Ability>();
            UserData.RegisterType<Location>();
            UserData.RegisterType<Activation>();
            UserData.RegisterType<Query>();
            UserData.RegisterType<UnitType>();
            UserData.RegisterType<Other>();
            DynValue l = UserData.Create(new Location());
            DynValue a = UserData.Create(new Activation());
            DynValue q = UserData.Create(new Query());
            DynValue t = UserData.Create(new UnitType());
            DynValue o = UserData.Create(new Other());
            script.Globals.Set("l", l);
            script.Globals.Set("a", a);
            script.Globals.Set("q", q);
            script.Globals.Set("t", t);
            script.Globals.Set("o", o);
            script.DoFile(filePath);
            DynValue numberOfAbilities = script.Call(script.Globals["NumberOfAbilities"]);
            for (int i = 0; i < numberOfAbilities.Number; i++)
            {
                ability = new Ability(player1, player2, cardFight, card);
                DynValue obj = UserData.Create(ability);
                script.Globals.Set("obj", obj);
                ability.StoreAbility(script, i + 1);
                abilities.Add(ability);
            }
            return abilities;
        }
    }

    public class Abilities
    {
        List<Ability>[] _abilities;

        public Abilities(int count)
        {
            _abilities = new List<Ability>[count + 10];
        }

        public void AddAbilities(int placement, List<Ability> ability)
        {
            _abilities[placement] = ability;
        }

        public Ability GetAbility(int tempID, int activationNumber)
        {
            return _abilities[tempID][activationNumber - 1];
        }

        public List<Ability> GetAbilities(int activation, List<Card> cards)
        {
            List<Ability> abilities = new List<Ability>();
            foreach (Card card in cards)
            {
                if (card == null)
                    continue;
                foreach (Ability ability in _abilities[card.tempID])
                {
                    if (ability.GetActivation == activation && ability.CheckCondition())
                    {
                        //foreach (int location in ability.Locations)
                        //{
                        //    if (location == card.location)
                        //    {
                        //        abilities.Add(ability);
                        //        break;
                        //    }
                        //}
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
    }

    public class Ability
    {
        Player _player1;
        Player _player2;
        CardFight _cardFight;
        Card _card; 
        bool _isMandatory = true;
        bool _isCont = true;
        bool _isSuperiorCall = false;
        bool _activated = false;
        bool _active = true;
        int _activation;
        List<int> _location = new List<int>();
        int _abilityType;
        int _abilityNumber;
        int _overDressParam = -1;
        Script _script;
        DynValue _abilityActivate;
        DynValue _abilityCost;
        DynValue _checkCondition;
        List<Param> _params = new List<Param>();

        public Ability(Player player1, Player player2, CardFight cardFight, Card card)
        {
            _player1 = player1;
            _player2 = player2;
            _cardFight = cardFight;
            _card = card;
        }

        public void StoreAbility(Script script, int num)
        {
            _script = script;
            _abilityNumber = num;
            Param param;
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
                    else if (returnedParam.Tuple[j].Number == Query.Type)
                    {
                        param.AddType((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                    else if (returnedParam.Tuple[j].Number == Query.Other)
                    {
                        param.AddOther((int)returnedParam.Tuple[j + 1].Number);
                        j++;
                    }
                }
                _params.Add(param);
            }
            DynValue activationRequirement = script.Call(script.Globals["ActivationRequirement"], _abilityNumber);
            _activation = (int)activationRequirement.Tuple[0].Number;
            if (_activation == Activation.OverDress)
            {
                _overDressParam = (int)activationRequirement.Tuple[1].Number;
                return;
            }
            _location.Add((int)activationRequirement.Tuple[1].Number);
            int tupleLocation = 1;
            _isCont = activationRequirement.Tuple[tupleLocation].Boolean;
            _isMandatory = activationRequirement.Tuple[tupleLocation + 1].Boolean;
            _checkCondition = script.Globals.Get("CheckCondition");
            _abilityActivate = script.Globals.Get("Activate");
            _abilityCost = script.Globals.Get("Cost");
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
            get => _isCont;
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

        public int GetID()
        {
            return _card.tempID;
        }

        public void ResetActivation()
        {
            _activated = false;
        }

        public void Test()
        {
            Console.WriteLine("test");
        }

        public int Activate()
        {
            _script.Call(_abilityCost, _abilityNumber);
            DynValue Then = _script.Call(_abilityActivate, _abilityNumber);
            _activated = true;
            return (int)Then.Number;
        }

        public bool CheckCondition()
        {
            DynValue check = _script.Call(_checkCondition, _abilityNumber);
            if (check.Boolean)
                return true;
            return false;
        }

        public bool IsRodeUponThisTurn()
        {
            foreach (Card card in _player1.GetRiddenOnThisTurn())
            {
                if (_card.tempID == card.tempID)
                    return true;
            }
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
            List<Card> cards;
            foreach (int location in param.Locations)
            {
                if (location == Location.Deck)
                    currentPool.AddRange(_player1.GetDeck());
                else if (location == Location.Drop)
                    currentPool.AddRange(_player1.GetDrop());
                else if (location == Location.PlayerHand)
                    currentPool.AddRange(_player1.GetHand());
                else if (location == Location.EnemyHand)
                    currentPool.AddRange(_player2.GetHand());
                else if (location == Location.Soul)
                    currentPool.AddRange(_player1.GetSoul());
                else if (location == Location.PlayerRC)
                {
                    cards = _player1.GetAllUnitsOnField();
                    foreach (Card card in cards)
                    {
                        if (card.location == Location.PlayerRC)
                            currentPool.Add(card);
                    }
                }
                else if (location == Location.EnemyRC)
                {
                    cards = _player1.GetAllUnitsOnField();
                    foreach (Card card in cards)
                    {
                        if (card.location == Location.EnemyRC)
                            currentPool.Add(card);
                    }
                }
                else if (location == Location.PlayerVC)
                    currentPool.Add(_player1.Vanguard());
                else if (location == Location.Damage)
                    currentPool.AddRange(_player1.GetDamageZone());
            }
            if (currentPool.Count == 0)
                return null;
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
                                return newPool;
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
                    if (param.Grades.Contains(currentPool[i].grade) && !newPool.Contains(currentPool[i]))
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
                    if (param.Types.Contains(currentPool[i].unitType) && !newPool.Contains(currentPool[i]))
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

        public bool CanOverDress(int tempID, int circle)
        {
            if (_overDressParam < 0)
                return false;
            Param param = _params[_overDressParam - 1];
            Card card = _player1.GetUnitAt(circle);
            if (card == null)
                return false;
            if (param.Names.Count != 0)
            {
                foreach (string name in param.Names)
                {
                    if (card.name == name)
                        return true;
                }
            }
            return false;
        }

        public bool LastPlacedOnGC()
        {
            return _player1.IsLastPlacedOnGC(_card.tempID);
        }

        public bool LastPlacedOnRC()
        {
            return _player1.IsLastPlacedOnRC(_card.tempID);
        }

        public bool OnTriggerZone()
        {
            Card trigger = _player1.GetTrigger(C.Player);
            if (trigger != null && trigger.tempID == _card.tempID)
                return true;
            return false;
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

        public bool CanSB(int paramNum)
        {
            List<Card> soul = _player1.GetSoul();
            int faceupCards = 0;
            foreach (Card card in soul)
            {
                    faceupCards++;
            }
            if (faceupCards >= _params[paramNum - 1].Counts[0])
                return true;
            return false;
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

        public bool CanStand(int paramNum)
        {
            List<Card> units = ValidCards(paramNum);
            int count = 0;
            foreach (Card card in units)
            {
                if (!card.upright)
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
            foreach (Card card in units)
            {
                if (!card.targetImmunity)
                    canRetire.Add(card);
            }
            if (canRetire.Count < _params[paramNum - 1].Counts[0])
                return false;
            return true;
        }

        public bool CanDiscard(int paramNum)
        {
            List<Card> hand = _player1.GetHand();
            if (hand.Count < _params[paramNum - 1].Counts[0])
                return false;
            return true;
        }

        public bool IsAttackingUnit()
        {
            if (_player1.AttackingUnit() != null && _card.tempID == _player1.AttackingUnit().tempID)
                return true;
            return false;
        }

        public bool IsVanguard()
        {
            if (_card.tempID == _player1.Vanguard().tempID)
                return true;
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

        public bool InOverDress()
        {
            List<Card> Units = _player1.GetActiveUnits();
            foreach (Card card in Units)
            {
                if (card.tempID == _card.tempID && card.overDress)
                    return true;
            }
            return false;
        }

        public bool VanguardIs(string name)
        {
            if (_player1.Vanguard().name == name)
                return true;
            return false;
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
            Console.WriteLine("turn: " + _player1.Turn);
            return _player1.Turn;
        }

        public void Draw(int count)
        {
            _player1.Draw(count);
            _player2.EnemyDraw(count);
        }

        public void SuperiorCall(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            _cardFight.SuperiorCall(_player1, _player2, cardsToSelect);
        }

        public void CounterBlast(int paramNum)
        {
            List<Card> canCB = new List<Card>();
            foreach (Card card in _player1.GetDamageZone())
            {
                if (card.faceup)
                    canCB.Add(card);
            }
            _cardFight.CounterBlast(_player1, _player2, canCB, _params[paramNum - 1].Counts[0]);
        }

        public void SoulBlast(int paramNum)
        {
            List<Card> canSB = new List<Card>();
            foreach (Card card in _player1.GetSoul())
            {
                canSB.Add(card);
            }
            _cardFight.SoulBlast(_player1, _player2, canSB, _params[paramNum - 1].Counts[0]);
        }

        public void Search(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            _cardFight.Search(_player1, _player2, cardsToSelect);
        }

        public void AddToHand(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            _cardFight.AddToHand(_player1, _player2, cardsToSelect, _params[paramNum - 1].Counts[0], false);
        }

        public void Discard(int paramNum)
        {
            _cardFight.Discard(_player1, _player2, _params[paramNum - 1].Counts[0]);
        }

        public void Retire(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<Card> canRetire = new List<Card>();
            foreach (Card card in cardsToSelect)
            {
                if (!card.targetImmunity)
                    canRetire.Add(card);
            }
            _cardFight.SelectCardToRetire(_player1, _player2, canRetire, _params[paramNum - 1].Counts[0], false); 
        }

        public void Stand(int paramNum)
        {
            List<Card> cardsToSelect = ValidCards(paramNum);
            List<Card> canStand = new List<Card>();
            foreach (Card card in cardsToSelect)
            {
                if (!card.upright)
                    canStand.Add(card);
            }
            _cardFight.Stand(_player1, _player2, canStand, _params[paramNum - 1].Counts[0], true);
        }

        public void PerfectGuard()
        {
            _cardFight.PerfectGuard(_player1, _player2);
        }

        public void TargetImmunity(int paramNum)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _cardFight.TargetImmunity(_player1, _player2, card.tempID);
            }
        }

        public void AddPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddPower(card.tempID, power);
                _player2.AddPower(card.tempID, power);
            }
        }

        public void ChooseAddTempPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            _cardFight.ChooseAddTempPower(_player1, _player2, cards, power, _params[paramNum - 1].Counts[0]);
        }

        public void AddAbilityPower(int paramNum, int power, int abilityNumber)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddAbilityPower(_card.tempID, abilityNumber, card.tempID, power);
                _player2.AddAbilityPower(_card.tempID, abilityNumber, card.tempID, power);
                card.tempPower += power;
            }
        }

        public void AddTempPower(int paramNum, int power)
        {
            List<Card> cards = ValidCards(paramNum);
            foreach (Card card in cards)
            {
                _player1.AddTempPower(card.tempID, power, C.Player);
                _player2.AddTempPower(card.tempID, power, C.Enemy);
                card.tempPower += power;
            }
        }

        public void OnRideAbilityResolved()
        {
            _player1.OnRideAbilityResolved(_card.tempID);
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
        List<int> _other = new List<int>();

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

        public void AddOther(int other)
        {
            _other.Add(other);
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

        public List<int> Others
        {
            get => _other;
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
        public const int OrderZone = 21;
    }

    class Query
    {
        public const int Name = 1;
        public const int Grade = 2;
        public const int Location = 3;
        public const int Count = 4;
        public const int Type = 5;
        public const int Other = 6;
    }

    class Other
    {
        public const int This = 1;
    }
}
