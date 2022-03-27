using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;
using System.Linq;

namespace VanguardEngine
{
    public class SQLiteDataAccess
    {
        public string connectionString;
        public Card Load(string id)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Card>("select * from data WHERE id='" + id + "'", new DynamicParameters()).ToList();
                Card card = null;
                if (output.Count > 0)
                    card = output[0];
                output = cnn.Query<Card>("select * from text WHERE id='" + id + "'", new DynamicParameters()).ToList();
                if (output.Count > 0 && card != null)
                {
                    card.name = output[0].name;
                    card.effect = output[0].text;
                    card.str1 = output[0].str1;
                    card.str2 = output[0].str2;
                    card.str3 = output[0].str3;
                    card.str4 = output[0].str4;
                    card.str5 = output[0].str5;
                    card.str6 = output[0].str6;
                }
                return card;
            }
        }

        public List<Card> Search(string query)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<Card>(query, new DynamicParameters()).ToList();
                foreach (Card card in output)
                {
                    if (card.effect == "")
                        card.effect = card.text;
                }
                return output;
            }
        }

        private string LoadConnectionString(string id = "Default")
        {
            //return ConfigurationManager.ConnectionStrings[id].ConnectionString;
            return connectionString;

        }
    }
}
