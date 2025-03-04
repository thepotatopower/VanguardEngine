﻿using System;
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
        public int shield = -1;
        public int critical = -1;
        public int trigger = -1;
        public int triggerPower = 0;
        public int unitType = -1;
        public int orderType = -1;
        public int crestType = -1;
        public int skill = -1;
        public int regalisPiece = -1;
        public int format = -1;
        public int tempID = 0;
        public string id = "";
        public int no = -1;
        public int personaRide = -1;
        public string effect = "";
        public string text = "";
        public bool alchemagic = false;
        public int originalOwner = 0;
        public bool fromRideDeck = false;
        public string str1 = "";
        public string str2 = "";
        public string str3 = "";
        public string str4 = "";
        public string str5 = "";
        public string str6 = "";

        public Card Clone()
        {
            Card card = new Card();
            card.clan = clan;
            card.critical = critical;
            card.effect = effect;
            card.regalisPiece = regalisPiece;
            card.format = format;
            card.grade = grade;
            card.id = id;
            card.name = name;
            card.nation = nation;
            card.orderType = orderType;
            card.crestType = crestType;
            card.personaRide = personaRide;
            card.power = power;
            card.race = race;
            card.shield = shield;
            card.skill = skill;
            card.trigger = trigger;
            card.triggerPower = triggerPower;
            card.unitType = unitType;
            card.tempID = tempID;
            card.text = text;
            card.str1 = str1;
            card.str2 = str2;
            card.str3 = str3;
            card.str4 = str4;
            card.str5 = str5;
            card.str6 = str6;
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

        public int OriginalGrade()
        {
            return grade;
        }
    }

    public class Nation
    {
    	public const int NullNation = -1;
    	public const int CrayElemental = 0;
        public const int KeterSanctuary = 1;
        public const int DragonEmpire = 2;
        public const int BrandtGate = 3;
        public const int DarkStates = 4;
        public const int Stoicheia = 5;
        public const int LyricalMonasterio = 6;

        public static string NationName(int name)
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

    public class UnitType
    {
        public const int NotUnit = -1;
        public const int Normal = 0;
        public const int Trigger = 1;
        public const int Sentinel = 2;
        public const int overDress = 3;
        public const int Token = 4;

        public static bool IsNormal(int unitType)
        {
            if (unitType != Token && unitType != NotUnit && unitType != 1)
                return true;
            return false;
        }
    }

    public class OrderType
    {
        public const int NotOrder = -1;
        public const int Normal = 0;
        public const int Blitz = 1;
        public const int Set = 2;
        public const int Music = 3;
        public const int Prison = 4;
        public const int World = 5;
        public const int Song = 6;
        public const int Gem = 7;
        public const int Meteorite = 8;
        public const int LeftDeityArms = 9;
        public const int RightDeityArms = 10;
        public const int FoxArt = 11;
        public const int Research = 12;

        public static bool IsSetOrder(int type)
        {
            if (type == OrderType.Set || type == OrderType.Prison || type == OrderType.World || type == OrderType.Song || type == OrderType.Meteorite
                || type == OrderType.Research)
                return true;
            return false;
        }
        public static bool IsNormalOrder(int type)
        {
            return type == OrderType.Normal || type == OrderType.Gem || 
                type == OrderType.LeftDeityArms || type == OrderType.RightDeityArms;
        }

        public static bool IsArms(int type)
        {
            return type == OrderType.LeftDeityArms || type == OrderType.RightDeityArms;
        }

        public static bool IsBlitzOrder(int type)
        {
            return type == OrderType.Blitz || type == OrderType.FoxArt;
        }
    }

    public class Skill
    {
        public const int Boost = 0;
        public const int Intercept = 1;
        public const int TwinDrive = 2;
        public const int TripleDrive = 3;
    }

    public class Trigger
    {
        public const int NotTrigger = -1;
        public const int Critical = 0;
        public const int Draw = 1;
        public const int Front = 2;
        public const int Heal = 3;
        public const int Stand = 4;
        public const int Over = 5;
    }

    public class Race
    {
        public const int Ghost = 0;
    }

    public class Format
    {
        public const int P = 0;
        public const int V = 1;
        public const int D = 2;
    }

    public class Names
    {
        public const string Eugene = "Heavy Artillery of Dust Storm, Eugene";
    }

    public class Clan
    {
    	public const int RoyalPaladin = 1;
    	public const int ShadowPaladin = 2;
    	public const int Kagero = 3;
    }

    public class CrestType
    {
        public const int NotCrest = -1;
        public const int Crest = 1;
        public const int RideDeckCrest = 2;
    }
}
