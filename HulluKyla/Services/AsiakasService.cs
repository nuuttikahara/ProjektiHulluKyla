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
        // ja tehdään riveistä Asiakas-olioita, jotka lisätään asiakkaat-listaan joka sitten palautetaan
        public static List<Asiakas> HaeKaikki() 
        {
            var asiakkaat = new List<Asiakas>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM asiakas", conn);
            using var reader = cmd.ExecuteReader();

            // Koska luodaan oliot tietokannan tiedoilla niin käytetään konstruktoria, jossa myös asiakasId mukana
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


        // Lisaa-metodi, jolla lisätään annetun asiakas-olion tiedot tietokantaan
        public static void Lisaa(Asiakas a) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(
                "INSERT INTO asiakas (etunimi, sukunimi, lahiosoite, postinro, email, puhelinnro) VALUES (@etunimi, @sukunimi, @lahiosoite, @postinro, @email, @puhelinnro)", conn);

            cmd.Parameters.AddWithValue("@etunimi", a.Etunimi);
            cmd.Parameters.AddWithValue("@sukunimi", a.Sukunimi);
            cmd.Parameters.AddWithValue("@lahiosoite", a.Lahiosoite);
            cmd.Parameters.AddWithValue("@postinro", a.Postinro);
            cmd.Parameters.AddWithValue("@email", a.Email);
            cmd.Parameters.AddWithValue("@puhelinnro", a.Puhelinnro);

            cmd.ExecuteNonQuery();
        }


        // Paivita-metodi, jolla päivitetään syötetyn asiakkaan tiedot (lukuunottamatta asiakasId:tä)
        public static void Paivita(Asiakas a) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(
                "UPDATE asiakas SET etunimi = @etunimi, sukunimi = @sukunimi, lahiosoite = @lahiosoite, postinro = @postinro, email = @email, puhelinnro = @puhelinnro WHERE asiakas_id = @id", conn);

            cmd.Parameters.AddWithValue("@etunimi", a.Etunimi);
            cmd.Parameters.AddWithValue("@sukunimi", a.Sukunimi);
            cmd.Parameters.AddWithValue("@lahiosoite", a.Lahiosoite);
            cmd.Parameters.AddWithValue("@postinro", a.Postinro);
            cmd.Parameters.AddWithValue("@email", a.Email);
            cmd.Parameters.AddWithValue("@puhelinnro", a.Puhelinnro);
            cmd.Parameters.AddWithValue("@id", a.AsiakasId);

            cmd.ExecuteNonQuery();
        }


        // Poista-metodi, jolla poistetaan asiakas tietokannasta asiakasId:n perusteella
        public static void Poista(int id) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM asiakas WHERE asiakas_id = @id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }



    }
}
