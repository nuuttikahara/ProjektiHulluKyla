using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HulluKyla.Models;
using MySql.Data.MySqlClient;

namespace HulluKyla.Services
{
    public static class LaskuService
    {
        // Laskun kokonaissumman ja alv:n laskeminen mökin ja palveluiden perusteella
        public static (double summa, double alv) LaskeSummaJaAlv(uint varausId) 
        {
            double summa = 0;
            double alv = 0;

            using var conn = SqlService.GetConnection();
            conn.Open();

            // Mökin hinta * päivien määrä
            var mokkiCmd = new MySqlCommand(@"
                SELECT m.hinta, DATEDIFF(v.varattu_loppupvm, v.varattu_alkupvm) AS paivat
                FROM varaus v
                JOIN mokki m
                ON v.mokki_id = m.mokki_id
                WHERE v.varaus_id = @id", conn);

            mokkiCmd.Parameters.AddWithValue("@id", varausId);

            using (var reader = mokkiCmd.ExecuteReader()) 
            {
                if (reader.Read()) 
                {
                    double hinta = reader.GetDouble("hinta");
                    int paivat = reader.GetInt32("paivat");
                    double kokonaishinta = hinta * paivat;
                    summa += kokonaishinta;
                    alv += kokonaishinta * 0.10;   // Oletetaan 10% ALV mökeille
                }
            }

            // Palveluiden hinta * määrä + ALV
            var palveluCmd = new MySqlCommand(@"
                SELECT p.hinta, p.alv, vp.lkm
                FROM varauksen_palvelut vp
                JOIN palvelu p
                ON vp.palvelu_id = p.palvelu_id
                WHERE vp.varaus_id = @id", conn);

            palveluCmd.Parameters.AddWithValue("@id", varausId);

            using (var reader = palveluCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    double hinta = reader.GetDouble("hinta");
                    double palveluAlv = reader.GetDouble("alv");
                    int lkm = reader.GetInt32("lkm");
                    double rivihinta = hinta * lkm;
                    summa += rivihinta;
                    alv += rivihinta * (palveluAlv / 100);
                }
            }

            return (summa, alv);
        }


        // Laskun luonti annetun varaus_id:n mukaan
        public static void LuoLasku(uint varausId) 
        {
            var (summa, alv) = LaskeSummaJaAlv(varausId);

            var lasku = new Lasku(varausId, summa, alv);

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO lasku (varaus_id, summa, alv, maksettu)
                VALUES (@varaus_id, @summa, @alv, @maksettu)", conn);

            cmd.Parameters.AddWithValue("@varaus_id", lasku.VarausId);
            cmd.Parameters.AddWithValue("@summa", lasku.Summa);
            cmd.Parameters.AddWithValue("@alv", lasku.Alv);
            cmd.Parameters.AddWithValue("@maksettu", lasku.Maksettu);

            cmd.ExecuteNonQuery();
        }


        // Palauttaa bool-arvon sen mukaan onko varaukselle luotu jo lasku
        public static bool OnkoLaskuJoLuotu(uint varausId) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT COUNT(*) FROM lasku WHERE varaus_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", varausId);

            long count = (long)cmd.ExecuteScalar();
            return count > 0;
        }


        // Luo laskut kaikille päättyneille varauksille joille sitä ei ole vielä luotu
        // Ideana siis, että lasku luodaan varaukselle vasta varauksen loputtua, eikä heti varaus luodessa
        // Hyödynnetään Laskujen hallinta/seuraus sivulla 
        public static void LuoLaskutPaattyneille() 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                SELECT v.varaus_id FROM varaus v
                WHERE v.varattu_loppupvm < CURRENT_TIMESTAMP()
                    AND NOT EXISTS (SELECT 1 FROM lasku l WHERE l.varaus_id = v.varaus_id)", conn);

            using var reader = cmd.ExecuteReader();

            var varausIdt = new List<uint>();
            while (reader.Read()) 
            {
                varausIdt.Add((uint)reader.GetInt32("varaus_id"));
            }

            foreach (var id in varausIdt) 
            {
                LuoLasku(id);
            }
        }

        // Haku metodi laskujen hakuun tietokannasta.
        public static List<Lasku> HaeKaikki() {
            var laskut = new List<Lasku>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM lasku", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read()) {
                laskut.Add(new Lasku(
                    (uint)reader.GetInt32("lasku_id"),
                    (uint)reader.GetInt32("varaus_id"),
                    reader.GetDouble("summa"),
                    reader.GetDouble("alv"),
                    reader.GetBoolean("maksettu")
                ));
            }

            return laskut;
        }


        // Maksamattomien laskujen haku
        public static List<Lasku> HaeMaksamattomatLaskut() 
        {
            var laskut = new List<Lasku>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM lasku WHERE maksettu = 0", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read()) 
            {
                laskut.Add(new Lasku(
                    (uint)reader.GetInt32("lasku_id"),
                    (uint)reader.GetInt32("varaus_id"),
                    reader.GetDouble("summa"),
                    reader.GetDouble("alv"),
                    reader.GetBoolean("maksettu")
                ));
            }

            return laskut;
        }


        // Laskun maksetuksi merkitseminen lasku_id:n mukaan
        public static void MerkitseMaksetuksi(int id) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("UPDATE lasku SET maksettu = 1 WHERE lasku_id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }


        // Pdf-tiedoston luonti laskusta tehdään todennäköisesti eri serviceen
    }
}
