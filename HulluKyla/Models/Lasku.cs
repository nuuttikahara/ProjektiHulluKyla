using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    public class Lasku
    {
        // Properties
        // TODO: LASKUID AUTOINCREMENT
        private uint laskuId;
        private uint varausId;
        private double summa;
        private double alv;
        private bool maksettu;

        // Constants
        //Max
        private const double DOUBLE_MAX_ALLOWED = 999999.99;

        // Min
        private const double DOUBLE_MIN_ALLOWED = 0;

        // Default
        private const bool MAKSETTU_DEFAULT = false;

        // Constructors
        // Tietokannasta otetuille laskuille.
        public Lasku(uint laskuId, uint varausId, double summa, double alv, bool maksettu)
        {
            this.LaskuId = laskuId;
            this.VarausId = varausId;
            this.Summa = summa;
            this.Alv = alv;
            this.Maksettu = maksettu;
        }

        // Uusille laskuille.
        public Lasku(uint varausId, double summa, double alv)
        {
            this.VarausId = varausId;
            this.Summa = summa;
            this.Alv = alv;
            this.Maksettu = MAKSETTU_DEFAULT;
        }

        // Getters and Setters
        public uint LaskuId
        {
            get => this.laskuId;
            private set => this.laskuId = value;
        }

        public uint VarausId
        {
            get => this.varausId;
            set { this.varausId = value; }
        }

        public double Summa
        {
            get => summa;
            set
            {
                if (!Double.IsNaN(value))
                {
                    if (value > DOUBLE_MAX_ALLOWED)
                        throw new ArgumentException($"Summan maksimiarvo on {DOUBLE_MAX_ALLOWED:f2}€.");
                    else if (value < DOUBLE_MIN_ALLOWED)
                        throw new ArgumentException($"Summan minimiarvo on {DOUBLE_MIN_ALLOWED:f2}€.");
                    else
                        this.summa = value;
                }
                else
                {
                    throw new ArgumentException("Summan täytyy olla numero.");
                }
            }
        }

        // ALV on rahamäärä, 0 - Double.MaxValue €.
        public double Alv
        {
            get => alv;
            set
            {
                if (!Double.IsNaN(value))
                {
                    if (value > DOUBLE_MAX_ALLOWED)
                        throw new ArgumentException($"ALV maksimiarvo on {DOUBLE_MAX_ALLOWED:f2}€.");
                    else if (value < DOUBLE_MIN_ALLOWED)
                        throw new ArgumentException($"ALV minimiarvo on {DOUBLE_MIN_ALLOWED:f2}€.");
                    else
                        this.alv = value;
                }
                else
                {
                    throw new ArgumentException("ALV täytyy olla numero.");
                }
            }
        }

        public bool Maksettu
        {
            get => this.maksettu;
            set { this.maksettu = value; }
        }

        // Methods
    }
}
