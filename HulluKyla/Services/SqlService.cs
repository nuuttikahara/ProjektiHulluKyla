using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Services
{
    public static class SqlService
    {
        // Tietokannan "pääsytiedot"
        private static readonly string connectionString =
            "server=localhost;port=3307;user=root;password=Ruutti;database=vn;";

        // Tietokantayhteyden luonti
        public static MySqlConnection GetConnection() 
        {
            return new MySqlConnection(connectionString);
        }

    }
}
