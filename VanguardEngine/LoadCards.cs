using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VanguardEngine
{
    public class LoadCards
    {
        public static List<string> GenerateList(string deckFilepath)
        {
            string[] f1 = File.ReadAllLines(deckFilepath);
            List<string> output = new List<string>();
            for (int i = 0; i < 52; i++)
            {
                if (i == 0 || i == 5)
                    continue;
                output.Add(f1[i]);
            }
            return output;
        }
        public static List<Card> GenerateCardsFromList(List<string> list, string connectionString)
        {
            List<Card> deck = new List<Card>();
            SQLiteDataAccess sql = new SQLiteDataAccess();
            sql.connectionString = connectionString;
            int tempID = 0;
            Card card = null;
            foreach (string item in list)
            {
                card = sql.Load(item);
                card.tempID = tempID++;
                deck.Add(card);
            }
            return deck;
        }

        public static List<Card> GenerateCards(string deckFilepath, string connectionString)
        {
            List<Card> deck = new List<Card>();
            SQLiteDataAccess sql = new SQLiteDataAccess();
            sql.connectionString = connectionString;
            string[] f1 = File.ReadAllLines(deckFilepath);
            int tempID = 0;
            Card card = null;
            if (f1.Length != 52)
            {
                return null;
            }
            for (int i = 0; i < 52; i++)
            {
                if (i == 0 || i == 5)
                    continue;
                card = sql.Load(f1[i]);
                card.tempID = tempID++;
                deck.Add(card);
            }
            return deck;
        }
    }
}
