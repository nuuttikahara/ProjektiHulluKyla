using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HulluKyla.Models;
using MySql.Data.MySqlClient;

namespace HulluKyla.Services
{
    public static class MokkiService
    {

        public static List<Mokki> HaeKaikki() 
        {
            var mokit = new List<Mokki>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM mokki", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                mokit.Add(new Mokki(
                    (uint)reader.GetInt32("mokki_id"),
                    (uint)reader.GetInt32("alue_id"),
                    reader.GetString("postinro"),
                    reader.GetString("mokkinimi"),
                    reader.GetString("katuosoite"),
                    reader.GetDouble("hinta"),
                    reader.GetString("kuvaus"),
                    reader.GetInt32("henkilomaara"),
                    reader.GetString("varustelu")
                ));
            }

            return mokit;
        }

        // Vapaiden mökkien haku tietyllä alueella, tietyllä minimi henkilömäärällä, tiettynä ajanjaksona
        public static List<Mokki> HaeVapaatMokit(uint alueId, int minHenkilomaara, DateTime alkuPvm, DateTime loppuPvm) {

            var mokit = new List<Mokki>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT * FROM mokki
                WHERE alue_id = @alueId
                    AND henkilomaara >= @minHenkilomaara
                    AND mokki_id NOT IN (
                        SELECT mokki_id FROM varaus
                        WHERE NOT (
                            varattu_loppupvm <= @alkuPvm OR
                            varattu_alkupvm >= @loppuPvm
                        )
                    )", conn);

            cmd.Parameters.AddWithValue("@alueId", alueId);
            cmd.Parameters.AddWithValue("@minHenkilomaara", minHenkilomaara);
            cmd.Parameters.AddWithValue("@alkuPvm", alkuPvm);
            cmd.Parameters.AddWithValue("@loppuPvm", loppuPvm);

            using var reader = cmd.ExecuteReader();
            while (reader.Read()) {
                var mokki = new Mokki(
                    (uint)reader.GetInt32("mokki_id"),
                    (uint)reader.GetInt32("alue_id"),
                    reader.GetString("postinro"),
                    reader.GetString("mokkinimi"),
                    reader.GetString("katuosoite"),
                    reader.GetDouble("hinta"),
                    reader.GetString("kuvaus"),
                    reader.GetInt32("henkilomaara"),
                    reader.GetString("varustelu")
                );

                mokit.Add(mokki);
            }

            return mokit;
        }


        // HaeAlueenMokit-metodi hakee tietyn alueen mökit alue_id:n mukaan
        public static List<Mokki> HaeAlueenMokit(uint id) 
        {
            var mokit = new List<Mokki>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM mokki WHERE alue_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                mokit.Add(new Mokki(
                    (uint)reader.GetInt32("mokki_id"),
                    (uint)reader.GetInt32("alue_id"),
                    reader.GetString("postinro"),
                    reader.GetString("mokkinimi"),
                    reader.GetString("katuosoite"),
                    reader.GetDouble("hinta"),
                    reader.GetString("kuvaus"),
                    reader.GetInt32("henkilomaara"),
                    reader.GetString("varustelu")
                ));
            }

            return mokit;
        }


        public static void Lisaa(Mokki m) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO mokki (alue_id, postinro, mokkinimi, katuosoite, hinta, kuvaus, henkilomaara, varustelu)
                VALUES (@alue_id, @postinro, @mokkinimi, @katuosoite, @hinta, @kuvaus, @henkilomaara, @varustelu)", conn);

            cmd.Parameters.AddWithValue("@alue_id", m.AlueId);
            cmd.Parameters.AddWithValue("@postinro", m.Postinro);
            cmd.Parameters.AddWithValue("@mokkinimi", m.MokkiNimi);
            cmd.Parameters.AddWithValue("@katuosoite", m.Katuosoite);
            cmd.Parameters.AddWithValue("@hinta", m.Hinta);
            cmd.Parameters.AddWithValue("@kuvaus", m.Kuvaus);
            cmd.Parameters.AddWithValue("@henkilomaara", m.HenkiloMaara);
            cmd.Parameters.AddWithValue("@varustelu", m.Varustelu);

            cmd.ExecuteNonQuery();
        }


        public static void Paivita(Mokki m) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                UPDATE mokki SET alue_id = @alue_id, postinro = @postinro, mokkinimi = @mokkinimi,
                    katuosoite = @katuosoite, hinta = @hinta, kuvaus = @kuvaus,
                    henkilomaara = @henkilomaara, varustelu = @varustelu
                WHERE mokki_id = @id", conn);

            cmd.Parameters.AddWithValue("@alue_id", m.AlueId);
            cmd.Parameters.AddWithValue("@postinro", m.Postinro);
            cmd.Parameters.AddWithValue("@mokkinimi", m.MokkiNimi);
            cmd.Parameters.AddWithValue("@katuosoite", m.Katuosoite);
            cmd.Parameters.AddWithValue("@hinta", m.Hinta);
            cmd.Parameters.AddWithValue("@kuvaus", m.Kuvaus);
            cmd.Parameters.AddWithValue("@henkilomaara", m.HenkiloMaara);
            cmd.Parameters.AddWithValue("@varustelu", m.Varustelu);
            cmd.Parameters.AddWithValue("@id", m.MokkiId);

            cmd.ExecuteNonQuery();
        }


        public static void Poista(uint id) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM mokki WHERE mokki_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
