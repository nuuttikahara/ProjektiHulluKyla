using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    internal class Mokki
    {
        // Properties
        private uint mokkiId;
        private uint alueId;
        private string postinro;
        private string mokkiNimi;
        private string katuosoite;
        private double hinta;
        private string kuvaus;
        private int henkiloMaara;
        private string varustelu;

        // Static Properties
        private readonly double DMAX = 100000000;

        // Constructors
        // Getters and Setters
        public uint MokkiId
        {
            get => mokkiId;
            private set { this.mokkiId = value; }
        }

        public uint AlueId
        {
            get => alueId;
            set { this.alueId = value; }
        }

        public string Postinro
        {
            get => postinro;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Postinumero ei voi olla tyhjä tai null.");
                else if (value.Trim().Length != 5)
                    throw new ArgumentException("Postinumeron täytyy olla 5 merkkiä pitkä.");

                postinro = value.Trim();
            }
        }

        public string MokkiNimi
        {
            get => mokkiNimi;
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    this.mokkiNimi = null;
                else if (value.Trim().Length > 45)
                    throw new ArgumentException("Mökin nimen maksimipituus on 45 merkkiä.");
                else
                    this.mokkiNimi = value.Trim();
            }
        }

        public string Katuosoite
        {
            get => katuosoite;
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    this.katuosoite = null;
                else if (value.Trim().Length > 45)
                    throw new ArgumentException("Katuosoitteen maksimipituus on 45 merkkiä.");
                else
                    this.katuosoite = value.Trim();
            }
        }

        public double Hinta
        {
            get => hinta;
            set
            {
                if (value >= DMAX)
                    throw new ArgumentException(
                        "Hinta ei voi olla yhtä suuri tai suurempi kuin " + DMAX + "."
                    );
                else if (value < 0)
                    throw new ArgumentException("Hinta ei voi olla alle 0.");
                else
                    this.hinta = value;
            }
        }

        // Methods
    }
}
