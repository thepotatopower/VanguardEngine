using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace VanguardEngine
{
    public class SQLiteDataAccess
    {
        public string connectionString;
        public string nameString;
        public Card Load(string id)
        {
            using (SqliteConnection cnn = new SqliteConnection(LoadConnectionString()))
            {
                cnn.Open();
                Card card = new Card();
                using (SqliteCommand cmd = new SqliteCommand("select * from data WHERE id='" + id + "'", cnn))
                {
                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            card.id = reader.GetString(reader.GetOrdinal("id"));
                            card.nation = reader.IsDBNull(reader.GetOrdinal("nation")) ? Nation.NullNation : reader.GetInt32(reader.GetOrdinal("nation"));
                            card.clan = reader.IsDBNull(reader.GetOrdinal("clan")) ? -1 : reader.GetInt32(reader.GetOrdinal("clan"));
                            card.race = reader.IsDBNull(reader.GetOrdinal("race")) ? -1 : reader.GetInt32(reader.GetOrdinal("race"));
                            card.grade = reader.IsDBNull(reader.GetOrdinal("grade")) ? -1 : reader.GetInt32(reader.GetOrdinal("grade"));
                            card.power = reader.IsDBNull(reader.GetOrdinal("power")) ? 0 : reader.GetInt32(reader.GetOrdinal("power"));
                            card.shield = reader.IsDBNull(reader.GetOrdinal("shield")) ? 0 : reader.GetInt32(reader.GetOrdinal("shield"));
                            card.critical = reader.IsDBNull(reader.GetOrdinal("critical")) ? 0 : reader.GetInt32(reader.GetOrdinal("critical"));
                            card.trigger = reader.IsDBNull(reader.GetOrdinal("trigger")) ? Trigger.NotTrigger : reader.GetInt32(reader.GetOrdinal("trigger"));
                            card.triggerPower = reader.IsDBNull(reader.GetOrdinal("triggerpower")) ? 0 : reader.GetInt32(reader.GetOrdinal("triggerpower"));
                            card.unitType = reader.IsDBNull(reader.GetOrdinal("unitType")) ? UnitType.NotUnit : reader.GetInt32(reader.GetOrdinal("unitType"));
                            card.orderType = reader.IsDBNull(reader.GetOrdinal("orderType")) ? OrderType.NotOrder : reader.GetInt32(reader.GetOrdinal("orderType"));
                            card.crestType = reader.IsDBNull(reader.GetOrdinal("crestType")) ? CrestType.NotCrest : reader.GetInt32(reader.GetOrdinal("crestType"));
                            card.skill = reader.IsDBNull(reader.GetOrdinal("skill")) ? -1 : reader.GetInt32(reader.GetOrdinal("skill"));
                            card.regalisPiece = reader.IsDBNull(reader.GetOrdinal("regalisPiece")) ? 0 : reader.GetInt32(reader.GetOrdinal("regalisPiece"));
                            card.format = reader.IsDBNull(reader.GetOrdinal("format")) ? 2 : reader.GetInt32(reader.GetOrdinal("format"));
                            card.personaRide = reader.IsDBNull(reader.GetOrdinal("personaRide")) ? 0 : reader.GetInt32(reader.GetOrdinal("personaRide"));
                        }
                    }
                }
                if (card.id == "")
                    throw new Exception("Card ID not found: " + id);
                using (SqliteCommand cmd = new SqliteCommand("select * from text WHERE id='" + id + "'", cnn))
                {
                    using (SqliteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            card.name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader.GetString(reader.GetOrdinal("name"));
                            card.effect = reader.IsDBNull(reader.GetOrdinal("text")) ? "" : reader.GetString(reader.GetOrdinal("text"));
                            card.str1 = reader.IsDBNull(reader.GetOrdinal("str1")) ? "" : reader.GetString(reader.GetOrdinal("str1"));
                            card.str2 = reader.IsDBNull(reader.GetOrdinal("str2")) ? "" : reader.GetString(reader.GetOrdinal("str2"));
                            card.str3 = reader.IsDBNull(reader.GetOrdinal("str3")) ? "" : reader.GetString(reader.GetOrdinal("str3"));
                            card.str4 = reader.IsDBNull(reader.GetOrdinal("str4")) ? "" : reader.GetString(reader.GetOrdinal("str4"));
                            card.str5 = reader.IsDBNull(reader.GetOrdinal("str5")) ? "" : reader.GetString(reader.GetOrdinal("str5"));
                            card.str6 = reader.IsDBNull(reader.GetOrdinal("str6")) ? "" : reader.GetString(reader.GetOrdinal("str6"));
                        }
                    }
                }
                if (card.name == "")
                    throw new Exception("Card name not found for Card ID: " + id);
                return card;
            }
        }

        public List<Card> Search(string query)
        {
            using (var cnn = new SqliteConnection(LoadConnectionString()))
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

        public void LoadNames(NameKeys nameKeys)
        {
            using (var cnn = new SqliteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<NameKey>("select * from names", new DynamicParameters()).ToList();
                foreach (NameKey nameKey in output)
                    nameKeys.InsertKey(nameKey.key, nameKey.name);
            }
        }

        private string LoadConnectionString(string id = "Default")
        {
            //return ConfigurationManager.ConnectionStrings[id].ConnectionString;
            return connectionString;

        }
    }
}
