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
                return output[0];
            }
        }
        private string LoadConnectionString(string id = "Default")
        {
            //return ConfigurationManager.ConnectionStrings[id].ConnectionString;
            return connectionString;

        }
    }
}
