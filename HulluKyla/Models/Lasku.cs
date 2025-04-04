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
        private int laskuId;
        private uint varausId;
        private double summa;
        private double alv;
        private bool maksettu;

        // Static properties
        private readonly double DMAX = 100000000;
        private readonly double DMIN = 0;

        // Constructors
        // Tietokannasta otetuille laskuille.
        public Lasku(int laskuId, uint varausId, double summa, double alv, bool maksettu)
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
            this.Maksettu = false;
        }

        // Getters and Setters
        public int LaskuId
        {
            get => this.laskuId;
            set { this.laskuId = value; }
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
                if (Double.IsNaN(value))
                    throw new ArgumentException("Summan arvo täytyy olla numero.");
                else if (value >= DMAX)
                    throw new ArgumentException("Liian suuri arvo summalle.");
                else if (value < DMIN)
                    throw new ArgumentException("Summan minimiarvo on 0.");
                else
                    this.summa = value;
            }
        }

        public double Alv
        {
            get => alv;
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("ALV täytyy olla numero.");
                else if (value < DMIN)
                    throw new ArgumentException("ALV minimiarvo on {0}.", DMIN.ToString());
                else if (value >= DMAX)
                    throw new ArgumentException("Liian suuri ALV arvo.");
                else
                    this.alv = value;
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
