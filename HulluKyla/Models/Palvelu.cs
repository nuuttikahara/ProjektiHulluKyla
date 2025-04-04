using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    public class Palvelu
    {
        // Properties
        private uint palveluId;
        private uint alueId;
        private string? nimi;
        private string? kuvaus;
        private double hinta;
        private double alv;

        // Static Properties
        private readonly int NIMI_MAX = 40;
        private readonly int KUVAUS_MAX = 255;
        private readonly double DMIN = 0;
        private readonly double DMAX = 100000000;

        // Constructors
        // Non-nullablet, paitsi ID.
        public Palvelu(double hinta, double alv)
        {
            this.Hinta = hinta;
            this.Alv = alv;
            // NULL
            this.nimi = null;
            this.kuvaus = null;
        }

        // Non-Nullablet.
        public Palvelu(uint palveluId, uint alueId, double hinta, double alv)
            : this(hinta, alv)
        {
            this.PalveluId = palveluId;
            this.AlueId = alueId;
        }

        // Tietokanta import.
        public Palvelu(
            uint palveluId,
            uint alueId,
            double hinta,
            double alv,
            string nimi,
            string kuvaus
        )
            : this(palveluId, alueId, hinta, alv)
        {
            this.Nimi = nimi;
            this.Kuvaus = kuvaus;
        }

        // Getters and Setters
        public uint PalveluId
        {
            get => this.palveluId;
            set { this.palveluId = value; }
        }

        public uint AlueId
        {
            get => this.alueId;
            set { this.alueId = value; }
        }

        public string Nimi
        {
            get
            {
                if (null != this.nimi)
                    return this.nimi;
                else
                    return "";
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    this.nimi = null;
                }
                else if (value.Trim().Length > NIMI_MAX)
                {
                    throw new ArgumentException(
                        "Nimen maksimipituus on {0} merkkiä.",
                        NIMI_MAX.ToString()
                    );
                }
                else
                {
                    this.nimi = value.Trim();
                }
            }
        }

        public string Kuvaus
        {
            get
            {
                if (null != this.kuvaus)
                    return this.kuvaus;
                else
                    return "";
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    this.kuvaus = null;
                }
                else if (value.Trim().Length > KUVAUS_MAX)
                {
                    throw new ArgumentException(
                        "Kuvauksen maksimipituus on {0} merkkiä.",
                        KUVAUS_MAX.ToString()
                    );
                }
                else
                {
                    this.kuvaus = value.Trim();
                }
            }
        }

        public double Hinta
        {
            get => this.hinta;
            set
            {
                if (Double.IsNaN(value))
                    throw new ArgumentException("Hinnan täytyy olla numero.");
                else if (value < DMIN)
                    throw new ArgumentException("Minimihinta on {0}€.", DMIN.ToString());
                else if (value >= DMAX)
                    throw new ArgumentException("Maksimihinta on {0}€.", DMAX.ToString());
                else
                    this.hinta = value;
            }
        }

        public double Alv
        {
            get => this.alv;
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

        // Methods
    }
}
