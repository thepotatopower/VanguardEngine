using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using MoonSharp.RemoteDebugger;
using System.IO;
using MoonSharp.VsCodeDebugger;

namespace VanguardEngine
{
    public class LuaInterpreter
    {
        public List<Effect> CheckEffects(int effectType, Player player1, Player player2)
        {
            List<Card> cards = new List<Card>();
            List<Effect> effects = new List<Effect>();
            Effect effect = null;
            if (effectType == EffectType.OnRide)
                cards = player1.CardsForOnRide();
            if (effectType == EffectType.OnAttack)
                cards = player1.CardsForOnAttack();
            foreach (Card card in cards)
            {
                if (card != null)
                {
                    effect = LookUpEffect(card, effectType, player1, player2);
                    if (effect != null)
                        effects.Add(effect);
                }
            }
            return effects;
        }

        public Effect LookUpEffect(Card card, int effectType, Player player1, Player player2)
        {
            bool valid = false;
            Script script;
            DynValue conditionMet;
            Effect effect = new Effect(player1, player2, card);
            string filePath = card.id.Replace("/", "");
            filePath = "../../lua/" + filePath + ".lua";
            if (!File.Exists(filePath))
                return null;
            Script.DefaultOptions.ScriptLoader = new MoonSharp.Interpreter.Loaders.FileSystemScriptLoader();
            script = new Script();
            UserData.RegisterType<Effect>();
            UserData.RegisterType<EffectType>();
            DynValue obj = UserData.Create(effect);
            DynValue e = UserData.Create(new EffectType());
            script.Globals.Set("obj", obj);
            script.Globals.Set("e", e);
            script.DoFile(filePath);
            DynValue conditionType = script.Call(script.Globals.Get("ConditionType"));
            for (int i = 0; i < conditionType.Tuple.Length; i++)
            {
                if (conditionType.Tuple[i].Number == effectType)
                {
                    valid = true;
                    break;
                }
            }
            if (!valid)
                return null;
            if (effectType == EffectType.OnRide)
            {
                conditionMet = script.Call(script.Globals.Get("CheckConditionOnRide"));
                if (!conditionMet.Boolean)
                    return null;
                effect.setScript(script);
                effect.setEffect(script.Globals.Get("OnRideActivate"));
                return effect;
            }
            if (effectType == EffectType.OnAttack)
            {
                conditionMet = script.Call(script.Globals.Get("CheckConditionOnAttack"));
                if (!conditionMet.Boolean)
                    return null;
                effect.setScript(script);
                effect.setEffect(script.Globals.Get("OnAttackActivate"));
                return effect;
            }
            return null;
        }
    }

    public class Effect
    {
        Player _player1;
        Player _player2;
        Card _card; 
        bool _isMandatory = true;
        bool _needsPrompt = true;
        bool _isSuperiorCall = false;
        bool _active = true;
        int _effectType;
        Script _script;
        DynValue _effectActivate;

        public Effect(Player player1, Player player2, Card card)
        {
            _player1 = player1;
            _player2 = player2;
            _card = card;
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

        public void Activate(int parameter)
        {
            _script.Call(_effectActivate, parameter);
        }

        public bool isTopSoul()
        {
            if (_player1.isTopSoul(_card))
                return true;
            return false;
        }

        public bool HasCardInDeck(string name)
        {
            if (_player1.HasCardInDeck(name))
                return true;
            return false;
        }

        public bool isAttackingUnit()
        {
            if (_card == _player1.AttackingUnit())
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

        public void SuperiorCall(int location, string name)
        {
            _player1.SuperiorCall(location, name);
            _player2.EnemySuperiorCall(location, name);
        }

        public void AddBattleOnlyPower(int selection, int power)
        {
            _player1.AddBattleOnlyPower(selection, power);
            _player2.AddBattleOnlyPower(selection, power);
        }
    }

    class EffectType
    {
        public const int OnRide = 1;
        public const int OnAttack = 2;
    }
}
