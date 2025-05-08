using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HulluKyla.Models;
using MySql.Data.MySqlClient;

namespace HulluKyla.Services
{
    public static class AlueService
    {

        public static List<Alue> HaeKaikki() 
        {
            var alueet = new List<Alue>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM alue", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                alueet.Add(new Alue(
                    (uint)reader.GetInt32("alue_id"),
                    reader.GetString("nimi")
                ));
            }

            return alueet;
        }

        public static List<Alue> HaeHakusanalla(string hakusana) {
            var alueet = new List<Alue>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM alue WHERE LOWER(nimi) LIKE @haku", conn);
            cmd.Parameters.AddWithValue("@haku", "%" + hakusana.ToLower() + "%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                alueet.Add(new Alue(
                    (uint)reader.GetInt32("alue_id"),
                    reader.GetString("nimi")
                ));
            }

            return alueet;
        }


        public static void Lisaa(Alue a) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("INSERT INTO alue (nimi) VALUES (@nimi)", conn);
            cmd.Parameters.AddWithValue("@nimi", a.Nimi);

            cmd.ExecuteNonQuery();
        }


        public static void Paivita(Alue a) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("UPDATE alue SET nimi = @nimi WHERE alue_id = @id", conn);
            cmd.Parameters.AddWithValue("@nimi", a.Nimi);
            cmd.Parameters.AddWithValue("@id", a.AlueId);

            cmd.ExecuteNonQuery();
        }


        public static void Poista(uint id) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM alue WHERE alue_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
