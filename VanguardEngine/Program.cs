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
            Card test;
            SQLiteDataAccess SQL = new SQLiteDataAccess();
            InputManager inputManager = new InputManager();
            CardFight cardFight = new CardFight();
            bool start = false;
            //LuaTest testing = new LuaTest();
            //testing.Testing();
            Console.WriteLine("Starting CardFight.");
            List<Card> deck1 = LoadCards.GenerateCardsFromList(LoadCards.GenerateList("testDeck.txt", LoadCode.WithRideDeck), "Data Source=./cards.db;Version=3;");
            List<Card> deck2 = LoadCards.GenerateCardsFromList(LoadCards.GenerateList("testDeck2.txt", LoadCode.WithRideDeck), "Data Source=./cards.db;Version=3;");
            List<Card> tokens = LoadCards.GenerateCardsFromList(LoadCards.GenerateList("tokens.txt", LoadCode.Tokens), "Data Source=./cards.db;Version=3;");
            Console.WriteLine(Directory.GetCurrentDirectory());
            start = cardFight.Initialize(deck1, deck2, tokens, inputManager, ".." + Path.DirectorySeparatorChar + "lua");
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
