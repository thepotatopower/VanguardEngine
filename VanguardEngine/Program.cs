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
            start = cardFight.Initialize("testDeck.txt", "testDeck.txt", inputManager);
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
