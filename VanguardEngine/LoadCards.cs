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
        public List<Card> GenerateCards(string deckFilepath, string connectionString)
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
