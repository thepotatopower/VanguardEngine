using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using System.IO;

namespace VanguardEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            SQLiteDataAccess SQL = new SQLiteDataAccess();
            InputManager inputManager = new InputManager();
            //inputManager.ReadFromInputLog("testReplay.txt");
            CardFight cardFight = new CardFight();
            bool start = false;
            //LuaTest testing = new LuaTest();
            //testing.Testing();
            //int seed = 542367287;
            //Random r = new Random(seed);
            //Console.WriteLine("seed: " + seed);
            //Console.WriteLine(r.Next(100000));
            //Console.WriteLine(r.Next(100000));
            //Console.WriteLine(r.Next(100000));
            //Console.ReadLine();
            Random r = new Random();
            Console.WriteLine("Starting CardFight.");
            List<Card> deck1 = LoadCards.GenerateCardsFromList(LoadCards.GenerateList("rorowa.txt", LoadCode.WithRideDeck, 1), "Data Source=./cards.db;Version=3;");
            List<Card> deck2 = LoadCards.GenerateCardsFromList(LoadCards.GenerateList("rorowa.txt", LoadCode.WithRideDeck, 1), "Data Source=./cards.db;Version=3;");
            List<Card> tokens = LoadCards.GenerateCardsFromList(LoadCards.GenerateList("tokens.txt", LoadCode.Tokens, -1), "Data Source=./cards.db;Version=3;");
            Console.WriteLine(Directory.GetCurrentDirectory());
            start = cardFight.Initialize(deck1, deck2, tokens, inputManager, ".." + Path.DirectorySeparatorChar + "lua", "Data Source=./cards.db;Version=3;", "Data Source=./names.db;Version=3;", r.Next(), 0);
            if (!start)
            {
                Console.WriteLine("Initialization error.");
                return;
            }
            else
            {
                cardFight.StartFight();
            }
        }
    }
}
