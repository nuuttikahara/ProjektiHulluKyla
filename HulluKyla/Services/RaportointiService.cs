using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HulluKyla.Models;
using MySql.Data.MySqlClient;

namespace HulluKyla.Services
{
    public static class RaportointiService
    {
        // Mökkien tuotto tietyllä ajanjaksolla tietyllä alueella
        public static double LaskeMokkienTuotto(DateTime alku, DateTime loppu, uint? alueId = null) {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var sql = @"
                SELECT SUM(m.hinta * DATEDIFF(v.varattu_loppupvm, v.varattu_alkupvm))
                FROM varaus v
                JOIN mokki m
                    ON v.mokki_id = m.mokki_id
                WHERE v.varattu_alkupvm <= @loppu AND v.varattu_loppupvm >= @alku";

            if (alueId.HasValue) {
                sql += " AND m.alue_id = @alue_id";
            }

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@alku", alku);
            cmd.Parameters.AddWithValue("@loppu", loppu);
            if (alueId.HasValue) {
                cmd.Parameters.AddWithValue("@alue_id", alueId);
            }

            var results = cmd.ExecuteScalar();
            return results != DBNull.Value ? Convert.ToDouble(results) : 0;
        }

        // Palveluiden tuotto tietyllä ajanjaksolla tietyllä alueella
        public static double LaskePalveluidenTuotto(DateTime alku, DateTime loppu, uint? alueId = null) {
            using var conn = SqlService.GetConnection();
            conn.Open();

            var sql = @"
                SELECT SUM(p.hinta * vp.lkm)
                FROM varaus v
                JOIN varauksen_palvelut vp
                    ON v.varaus_id = vp.varaus_id
                    JOIN mokki m
                        ON v.mokki_id = m.mokki_id
                WHERE v.varattu_alkupvm <= @loppu AND v.varattu_loppupvm >= @alku";

            if (alueId.HasValue) {
                sql += " AND m.alue_id = @alue_id";
            }

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@alku", alku);
            cmd.Parameters.AddWithValue("@loppu", loppu);
            if (alueId.HasValue) {
                cmd.Parameters.AddWithValue("@alue_id", alueId);
            }

            var results = cmd.ExecuteScalar();
            return results != DBNull.Value ? Convert.ToDouble(results) : 0;
        }

        // Annetun vuoden kokonaistuoton laskenta ja palautus listana kuukausittain eriteltynä
        public static List<(int Vuosi, int Kuukausi, double Tuotto)> LaskeVuodenKokonaistuotto(int vuosi) {
            var tulokset = new List<(int, int, double)>();

            using var conn = SqlService.GetConnection();
            conn.Open();

            var sql = @"
                SELECT
                    MONTH(v.varattu_alkupvm) AS kuukausi,
                    SUM(m.hinta * DATEDIFF(v.varattu_loppupvm, v.varattu_alkupvm)) +
                    IFNULL( (
                        SELECT SUM(p.hinta * vp.lkm)
                        FROM varauksen_palvelut vp
                        JOIN palvelu p
                            ON vp.palvelu_id = p.palvelu_id
                        WHERE vp.varaus_id = v.varausid
                    ), 0) AS kokonaistuotto
                FROM varaus v
                JOIN mokki m
                    ON v.mokki_id = m.mokki_id
                WHERE YEAR(v.varattu_alkupvm) = @vuosi
                    AND v.varattu_loppupvm < DATE_FORMAT(CURRENT_DATE, '%Y-%m-01')
                GROUP BY kuukausi
                ORDER BY kuukausi;";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@vuosi", vuosi);

            using var reader = cmd.ExecuteReader();

            while (reader.Read()) {
                int kuukausi = reader.GetInt32("kuukausi");
                double tuotto = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
                tulokset.Add((vuosi,  kuukausi, tuotto));
            }

            return tulokset;
        }
    }
}
