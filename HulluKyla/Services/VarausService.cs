using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using HulluKyla.Models;

namespace HulluKyla.Services
{
    public static class VarausService
    {
        public static List<Varaus> HaeKaikki() 
        {
            var varaukset = new List<Varaus>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM varaus", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                uint asiakasId = (uint)reader.GetInt32("asiakas_id");
                uint mokkiId = (uint)reader.GetUInt32("mokki_id");

                var asiakas = AsiakasService.HaeKaikki().Find(a  => a.AsiakasId == asiakasId)
                     ?? throw new Exception($"Asiakasta ID:llä {asiakasId} ei löytynyt.");

                var mokki = MokkiService.HaeKaikki().Find(m => m.MokkiId == mokkiId)
                    ?? throw new Exception($"Mökkiä ID:llä {mokkiId} ei löytynyt.");

                varaukset.Add(new Varaus(
                    (uint)reader.GetInt32("varaus_id"),
                    asiakas,
                    mokki,
                    reader.GetDateTime("varattu_pvm"),
                    reader.GetDateTime("vahvistus_pvm"),
                    reader.GetDateTime("varattu_alkupvm"),
                    reader.GetDateTime("varattu_loppupvm")
                ));
            }

            return varaukset;
        }


        // Varauksien haku annetun asiakas_id:n mukaan
        public static List<Varaus> HaeAsiakkaanVaraukset(uint id) 
        {
            var varaukset = new List<Varaus>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM varaus WHERE asiakas_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            while (reader.Read()) 
            {
                uint mokkiId = (uint)reader.GetUInt32("mokki_id");

                var asiakas = AsiakasService.HaeKaikki().Find(a => a.AsiakasId == id)
                    ?? throw new Exception($"Asiakasta ID:llä {id} ei löytynyt.");

                var mokki = MokkiService.HaeKaikki().Find(m => m.MokkiId == mokkiId)
                    ?? throw new Exception($"Mökkiä ID:llä {mokkiId} ei löytynyt.");

                varaukset.Add(new Varaus(
                    (uint)reader.GetInt32("varaus_id"),
                    asiakas,
                    mokki,
                    reader.GetDateTime("varattu_pvm"),
                    reader.GetDateTime("vahvistus_pvm"),
                    reader.GetDateTime("varattu_alkupvm"),
                    reader.GetDateTime("varattu_loppupvm")
                ));
            }

            return varaukset;
        }


        // Varausten haku annetun alue_id:n mukaan
        public static List<Varaus> HaeAlueenVaraukset(uint id)
        {
            var varaukset = new List<Varaus>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT v.* FROM varaus v
                JOIN mokki m
                ON v.mokki_id = m.mokki_id
                WHERE m.alue_id = @id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                uint asiakasId = (uint)reader.GetInt32("asiakas_id");
                uint mokkiId = (uint)reader.GetUInt32("mokki_id");

                var asiakas = AsiakasService.HaeKaikki().Find(a => a.AsiakasId == asiakasId)
                    ?? throw new Exception($"Asiakasta ID:llä {asiakasId} ei löytynyt.");

                var mokki = MokkiService.HaeKaikki().Find(m => m.MokkiId == mokkiId)
                    ?? throw new Exception($"Mökkiä ID:llä {mokkiId} ei löytynyt.");

                varaukset.Add(new Varaus(
                    (uint)reader.GetInt32("varaus_id"),
                    asiakas,
                    mokki,
                    reader.GetDateTime("varattu_pvm"),
                    reader.GetDateTime("vahvistus_pvm"),
                    reader.GetDateTime("varattu_alkupvm"),
                    reader.GetDateTime("varattu_loppupvm")
                ));
            }

            return varaukset;
        }


        // Varausten haku tietyltä alueelta tietyllä ajanjaksolla
        public static List<Varaus> HaeAlueenVarauksetAjanjaksolla(uint id, DateTime alku, DateTime loppu)
        {
            var varaukset = new List<Varaus>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT v.* FROM varaus v 
                JOIN mokki m 
                ON v.mokki_id = m.mokki_id 
                WHERE m.alue_id = @id 
                    AND v.varattu_alkupvm <= @loppu 
                    AND v.varattu_loppupvm >= @alku", conn);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@alku", alku);
            cmd.Parameters.AddWithValue("@loppu", loppu);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                uint asiakasId = (uint)reader.GetInt32("asiakas_id");
                uint mokkiId = (uint)reader.GetUInt32("mokki_id");

                var asiakas = AsiakasService.HaeKaikki().Find(a => a.AsiakasId == asiakasId)
                    ?? throw new Exception($"Asiakasta ID:llä {asiakasId} ei löytynyt.");

                var mokki = MokkiService.HaeKaikki().Find(m => m.MokkiId == mokkiId)
                    ?? throw new Exception($"Mökkiä ID:llä {mokkiId} ei löytynyt.");

                varaukset.Add(new Varaus(
                    (uint)reader.GetInt32("varaus_id"),
                    asiakas,
                    mokki,
                    reader.GetDateTime("varattu_pvm"),
                    reader.GetDateTime("vahvistus_pvm"),
                    reader.GetDateTime("varattu_alkupvm"),
                    reader.GetDateTime("varattu_loppupvm")
                ));
            }

            return varaukset;
        }


        public static void Lisaa(Varaus v) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO varaus (asiakas_id, mokki_id, varattu_pvm, vahvistus_pvm, varattu_alkupvm, varattu_loppupvm)
                VALUES (@asiakas_id, @mokki_id, @varattu_pvm, @vahvistus_pvm, @varattu_alkupvm, @varattu_loppupvm)", conn);

            cmd.Parameters.AddWithValue("@asiakas_id", v.Asiakas.AsiakasId);
            cmd.Parameters.AddWithValue("@mokki_id", v.Mokki.MokkiId);
            cmd.Parameters.AddWithValue("@varattu_pvm", v.VarattuPvm);
            cmd.Parameters.AddWithValue("@vahvistus_pvm", v.VahvistusPvm);
            cmd.Parameters.AddWithValue("@varattu_alkupvm", v.VarattuAlkuPvm);
            cmd.Parameters.AddWithValue("@varattu_loppupvm", v.VarattuLoppuPvm);

            cmd.ExecuteNonQuery();
        }


        public static void Paivita(Varaus v) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                UPDATE varaus SET asiakas_id = @asiakas_id, mokki_id = @mokki_id, varattu_pvm = @varattu_pvm,
                    vahvistus_pvm = @vahvistus_pvm, varattu_alkupvm = @varattu_alkupvm, varattu_loppupvm = @varattu_loppupvm
                WHERE varaus_id = @id", conn);

            cmd.Parameters.AddWithValue("@asiakas_id", v.Asiakas.AsiakasId);
            cmd.Parameters.AddWithValue("@mokki_id", v.Mokki.MokkiId);
            cmd.Parameters.AddWithValue("@varattu_pvm", v.VarattuPvm);
            cmd.Parameters.AddWithValue("@vahvistus_pvm", v.VahvistusPvm);
            cmd.Parameters.AddWithValue("@varattu_alkupvm", v.VarattuAlkuPvm);
            cmd.Parameters.AddWithValue("@varattu_loppupvm", v.VarattuLoppuPvm);
            cmd.Parameters.AddWithValue("@id", v.VarausId);

            cmd.ExecuteNonQuery();
        }


        public static void Poista(uint id) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM varaus WHERE varaus_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
