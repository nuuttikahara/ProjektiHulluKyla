using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HulluKyla.Models;
using MySql.Data.MySqlClient;

namespace HulluKyla.Services
{
    public static class AsiakasService
    {
        // HaeKaikki-metodi, jossa haetaan tietokannasta asiakas-taulun sisältö
        // ja tehdään riveistä Asiakas-oliot jotka lisätään asiakkaat-listaan joka palautetaan
        public static List<Asiakas> HaeKaikki() 
        {
            var asiakkaat = new List<Asiakas>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM asiakas", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                asiakkaat.Add(new Asiakas(
                    reader.GetInt32("asiakas_id"),
                    reader.GetString("etunimi"),
                    reader.GetString("sukunimi"),
                    reader.GetString("lahiosoite"),
                    reader.GetString("postinro"),
                    reader.GetString("email"),
                    reader.GetString("puhelinnro")
                ));
            }

            return asiakkaat;
        }



    }
}
