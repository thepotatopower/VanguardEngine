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
        public static List<string> GenerateList(string deckFilepath, int loadCode)
        {
            string[] f1 = File.ReadAllLines(deckFilepath);
            //if (loadCode == LoadCode.WithRideDeck && f1.Length != 52)
            //    return null;
            List<string> output = new List<string>();
            if (loadCode == LoadCode.WithRideDeck)
            {
                for (int i = 0; i < 52; i++)
                {
                    if (i == 0 || i == 5)
                        continue;
                    output.Add(f1[i]);
                }
                return output;
            }
            else
            {
                for (int i = 0; i < f1.Length; i++)
                {
                    output.Add(f1[i]);
                }
                return output;
            }
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

        public static List<Card> Search(string query, string connectionString)
        {
            SQLiteDataAccess sql = new SQLiteDataAccess();
            sql.connectionString = connectionString;
            return sql.Search(query);
        }

        public static void LoadNames(string nameString, NameKeys nameKeys)
        {
            SQLiteDataAccess sql = new SQLiteDataAccess();
            sql.connectionString = nameString;
            sql.LoadNames(nameKeys);
        }
    }

    public class LoadCode
    {
        public const int WithRideDeck = 0;
        public const int Tokens = 1;
    }
}
