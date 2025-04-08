using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using HulluKyla.Models;
using System.Data;

namespace HulluKyla.Services
{
    public static class PalveluService
    {
        public static List<Palvelu> HaeKaikki() 
        {
            var palvelut = new List<Palvelu>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM palvelu", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                palvelut.Add(new Palvelu(
                    (uint)reader.GetInt32("palvelu_id"),
                    (uint)reader.GetInt32("alue_id"),
                    reader.GetDouble("hinta"),
                    reader.GetDouble("alv"),
                    reader.GetString("nimi"),
                    reader.GetString("kuvaus")
                ));
            }

            return palvelut;
        }


        // HaeAlueenPalvelut -metodilla haetaan palvelut annetun alue_id:n mukaan
        public static List<Palvelu> HaeAlueenPalvelut(uint id) 
        {
            var palvelut = new List<Palvelu>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("SELECT * FROM palvelu WHERE alue_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                palvelut.Add(new Palvelu(
                    (uint)reader.GetInt32("palvelu_id"),
                    (uint)reader.GetInt32("alue_id"),
                    reader.GetDouble("hinta"),
                    reader.GetDouble("alv"),
                    reader.GetString("nimi"),
                    reader.GetString("kuvaus")
                ));
            }

            return palvelut;
        }

        public static void Lisaa(Palvelu p) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand(@"
                INSERT INTO palvelu (alue_id, nimi, kuvaus, hinta, alv)
                VALUES (@alue_id, @nimi, @kuvaus, @hinta, @alv)", conn);

            cmd.Parameters.AddWithValue("@alue_id", p.AlueId);
            cmd.Parameters.AddWithValue("@nimi", p.Nimi);
            cmd.Parameters.AddWithValue("@kuvaus", p.Kuvaus);
            cmd.Parameters.AddWithValue("@hinta", p.Hinta);
            cmd.Parameters.AddWithValue("@alv", p.Alv);

            cmd.ExecuteNonQuery();
        }


        public static void Paivita(Palvelu p) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand(@"
                UPDATE palvelu SET alue_id = @alue_id, nimi = @nimi, kuvaus = @kuvaus,
                    hinta = @hinta, alv = @alv
                WHERE palvelu_id = @id", conn);

            cmd.Parameters.AddWithValue("@alue_id", p.AlueId);
            cmd.Parameters.AddWithValue("@nimi", p.Nimi);
            cmd.Parameters.AddWithValue("@kuvaus", p.Kuvaus);
            cmd.Parameters.AddWithValue("@hinta", p.Hinta);
            cmd.Parameters.AddWithValue("@alv", p.Alv);
            cmd.Parameters.AddWithValue("@id", p.PalveluId);

            cmd.ExecuteNonQuery();
        }


        public static void Poista(uint id) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("DELETE FROM palvelu WHERE palvelu_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
