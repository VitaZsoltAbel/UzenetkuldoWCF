using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UzenetkuldoConsole.Database
{
    public static class Database
    {
        private static readonly string ConnectionString = "Server=localhost;Database=uzenetkuldo;Uid=root;Pwd=;CharSet=utf8mb4;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}