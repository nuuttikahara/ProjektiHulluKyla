using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    internal class Lasku
    {
        // Properties
        private int laskuId;
        private uint varausId;
        private double summa;
        private double alv;
        private byte maksettu;

        // Static properties
        private readonly double DMAX = 100000000;
        private readonly double DMIN = 0;

        // Constructors
        // Tietokannasta otetuille laskuille.
        public Lasku(int laskuId, uint varausId, double summa, double alv, byte maksettu)
        {
            this.LaskuId = laskuId;
            this.VarausId = varausId;
            this.Summa = summa;
            this.Alv = alv;
            this.Maksettu = maksettu;
        }

        // Uusille laskuille.
        public Lasku(int laskuId, uint varausId, double summa, double alv)
        {
            this.LaskuId = laskuId;
            this.VarausId = varausId;
            this.Summa = summa;
            this.Alv = alv;
            this.Maksettu = 0;
        }

        // Getters and Getters
        public int LaskuId
        {
            get => laskuId;
            set { this.laskuId = value; }
        }

        public uint VarausId
        {
            get => varausId;
            set { this.varausId = value; }
        }

        public double Summa
        {
            get => summa;
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("Arvon täytyy olla numero.");
                else if (value >= DMAX)
                    throw new ArgumentException("Liian suuri arvo.");
                else if (value < DMIN)
                    throw new ArgumentException("Minimiarvo on 0.");

                this.summa = value;
            }
        }

        public double Alv
        {
            get => alv;
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("Arvon täytyy olla numero.");
                else if (value >= DMAX)
                    throw new ArgumentException("Liian suuri arvo.");
                else if (value < DMIN)
                    throw new ArgumentException("Minimiarvo on 0.");

                this.alv = value;
            }
        }

        // Toimii kuin boolean.
        public byte Maksettu
        {
            get => maksettu;
            set
            {
                if (value > 1)
                    throw new ArgumentException("Maksimiarvo on 1.");
                else if (value < 0)
                    throw new ArgumentException("Minimiarvo on 0.");

                this.maksettu = value;
            }
        }

        // Methods
    }
}
