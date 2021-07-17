using System;
using System.Collections.Generic;
using System.Text;

namespace VanguardEngine
{

    public class Card
    {
        public string name = "";
        public int nation = -1;
        public int clan = -1;
        public int race = -1;
        public int grade = -1;
        public int power = -1;
        public int bonusPower = 0;
        public int tempPower = 0;
        public int battleOnlyPower = 0;
        public int tempCritical = 0;
        public int battleOnlyCritical = 0;
        public int tempDrive = 0;
        public Dictionary<Tuple<int, int>, int> abilityPower = new Dictionary<Tuple<int, int>, int>();
        public Dictionary<Tuple<int, int>, int> abilityShield = new Dictionary<Tuple<int, int>, int>();
        public Dictionary<Tuple<int, int>, int> abilityDrive = new Dictionary<Tuple<int, int>, int>();
        public Dictionary<Tuple<int, int>, int> abilityCritical = new Dictionary<Tuple<int, int>, int>();
        public int tempShield = 0;
        public int shield = -1;
        public int critical = -1;
        public int trigger = -1;
        public int triggerPower = 0;
        public int unitType = -1;
        public int orderType = -1;
        public int skill = -1;
        public int drive = -1;
        public int format = -1;
        public int tempID = 0;
        public string id = "";
        public int no = -1;
        public int personaRide = -1;
        public string effect = "";
        public bool faceup = false;
        public bool upright = true;
        public bool overDress = false;
        public bool targetImmunity = false;
        public List<int> hitImmunity = new List<int>();
        public bool alchemagic = false;
        public int location = Location.Deck;
        public int originalOwner = 0;
        public List<Card> soul = new List<Card>();

        public Card Clone()
        {
            Card card = new Card();
            card.critical = critical;
            card.drive = drive;
            card.effect = effect;
            card.format = format;
            card.grade = grade;
            card.id = id;
            card.name = name;
            card.nation = nation;
            card.orderType = orderType;
            card.personaRide = personaRide;
            card.power = power;
            card.race = race;
            card.shield = shield;
            card.skill = skill;
            card.trigger = trigger;
            card.triggerPower = triggerPower;
            card.unitType = unitType;
            card.tempID = tempID;
            return card;
        }
        public void PrintCardInfo()
        {
            Console.WriteLine(name);
            Console.WriteLine(nation);
            Console.WriteLine(clan);
            Console.WriteLine(race);
            Console.WriteLine(grade);
            Console.WriteLine(power);
            Console.WriteLine(shield);
            Console.WriteLine(trigger);
            Console.WriteLine(skill);
            Console.WriteLine(effect);
            Console.WriteLine(id);
        }
    }

    class Nation
    {
        public const int KeterSanctuary = 0;
        public const int DragonEmpire = 1;
        public const int BrandtGate = 2;
        public const int DarkStates = 3;
        public const int Stoicheia = 4;
        public const int LyricalMonasterio = 5;

        public string NationName(int name)
        {
            switch(name)
            {
                case 0:
                    return "Keter Sanctuary";
                case 1:
                    return "Dragon Empire";
                case 2:
                    return "Brandt Gate";
                case 3:
                    return "Dark States";
                case 4:
                    return "Stoicheia";
                case 5:
                    return "Lyrical Monasterio";
            }
            return "N/A";
        }
    }

    class UnitType
    {
        public const int NotUnit = -1;
        public const int Normal = 0;
        public const int Trigger = 1;
        public const int Sentinel = 2;
        public const int overDress = 3;
        public const int Token = 4;
    }

    class OrderType
    {
        public const int NotOrder = -1;
        public const int Normal = 0;
        public const int Blitz = 1;
        public const int Set = 2;
        public const int Music = 3;
        public const int Prison = 4;
        public const int World = 5;
    }

    class Skill
    {
        public const int Boost = 0;
        public const int Intercept = 1;
        public const int TwinDrive = 2;
        public const int TripleDrive = 3;
    }

    class Trigger
    {
        public const int NotTrigger = -1;
        public const int Critical = 0;
        public const int Draw = 1;
        public const int Front = 2;
        public const int Heal = 3;
        public const int Stand = 4;
        public const int Over = 5;
    }

    class Format
    {
        public const int Original = 0;
        public const int V = 1;
        public const int D = 2;
    }
}
