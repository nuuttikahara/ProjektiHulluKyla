using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HulluKyla.Models;
using MySql.Data.MySqlClient;

namespace HulluKyla.Services
{
    public static class VarauksenPalvelutService
    {

        // Annetun palvelun ja sen määrän lisääminen annettuun varaukseen, varaus_id:n, palvelu_id:n ja lukumäärän mukaan
        public static void LisaaPalveluVaraukseen(uint varausId, uint palveluId, int lkm) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                INSERT INTO varauksen_palvelut (varaus_id, palvelu_id, lkm)
                VALUES (@varaus_id, @palvelu_id, @lkm)", conn);

            cmd.Parameters.AddWithValue("@varaus_id", varausId);
            cmd.Parameters.AddWithValue("@palvelu_id", palveluId);
            cmd.Parameters.AddWithValue("@lkm", lkm);

            cmd.ExecuteNonQuery();
        }


        // Annetun palvelun poisto annetusta varauksesta, palvelu_id:n ja varaus_id:n mukaan
        public static void PoistaPalveluVarauksesta(uint varausId, uint palveluId) 
        {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
                DELETE FROM varauksen_palvelut
                WHERE varaus_id = @varaus_id AND palvelu_id = @palvelu_id", conn);

            cmd.Parameters.AddWithValue("@varaus_id", varausId);
            cmd.Parameters.AddWithValue("@palvelu_id", palveluId);

            cmd.ExecuteNonQuery();
        }

        // Kaikkien palveluiden poisto varaukselta
        public static void PoistaKaikkiPalvelutVaraukselta(uint varausId) {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM varauksen_palvelut WHERE varaus_id = @varausId", conn);
            cmd.Parameters.AddWithValue("@varausId", varausId);
            cmd.ExecuteNonQuery();
        }


        // Palveluiden ja niiden määrän haku annetun varaus_id:n mukaan
        public static List<VarauksenPalvelu> HaeVarauksenPalvelut(uint varausId) 
        {
            var tulokset = new List<VarauksenPalvelu>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var cmd = new MySqlCommand(@"
            SELECT p.palvelu_id, p.alue_id, p.nimi, p.kuvaus, p.hinta, p.alv, vp.lkm
            FROM varauksen_palvelut vp
            JOIN palvelu p
            ON vp.palvelu_id = p.palvelu_id
            WHERE vp.varaus_id = @varaus_id", conn);

            cmd.Parameters.AddWithValue("@varaus_id", varausId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read()) 
            {
                var palvelu = new Palvelu(
                    (uint)reader.GetInt32("palvelu_id"),
                    (uint)reader.GetInt32("alue_id"),
                    reader.GetDouble("hinta"),
                    reader.GetDouble("alv"),
                    reader.GetString("nimi"),
                    reader.GetString("kuvaus")
                );

                int lkm = reader.GetInt32("lkm");
                tulokset.Add(new VarauksenPalvelu(
                    varausId,
                    lkm,
                    palvelu
                ));
            }

            return tulokset;
        }
    }
}
