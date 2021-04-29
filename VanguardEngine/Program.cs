using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;

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
            LoadCards loadCards = new LoadCards();
            List<Card> deck1 = loadCards.GenerateCards("testDeck.txt", "Data Source=./cards.db;Version=3;");
            List<Card> deck2 = loadCards.GenerateCards("testDeck.txt", "Data Source=./cards.db;Version=3;");
            start = cardFight.Initialize(deck1, deck2, inputManager);
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
