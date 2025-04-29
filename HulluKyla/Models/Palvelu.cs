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

        // Constants
        // Max
        private const int NIMI_MAX = 40;
        private const int KUVAUS_MAX = 255;
        private const double DOUBLE_MAX_ALLOWED = 999999.99;
        private const double ALV_MAX = 100;

        // Min
        private const double DOUBLE_MIN_ALLOWED = 0;
        private const double ALV_MIN = 0;

        // Null String return value
        private const string STRING_NULL = "NULL";

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

        // Ilman palvelu_id:tä
        public Palvelu(
            uint alueId,
            double hinta,
            double alv,
            string nimi,
            string kuvaus
        )
            : this(hinta, alv) 
        {
            this.AlueId = alueId;
            this.Nimi = nimi;
            this.Kuvaus = kuvaus;
        }

        // Getters and Setters
        public uint PalveluId
        {
            get => this.palveluId;
            private set => this.palveluId = value;
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
                    return STRING_NULL;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    this.nimi = null;
                }
                else if (value.Trim().Length > NIMI_MAX)
                {
                    throw new ArgumentException($"Nimen maksimipituus on {NIMI_MAX} merkkiä.");
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
                    return STRING_NULL;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    this.kuvaus = null;
                }
                else if (value.Trim().Length > KUVAUS_MAX)
                {
                    throw new ArgumentException($"Kuvauksen maksimipituus on {KUVAUS_MAX} merkkiä.");
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
                else if (value < DOUBLE_MIN_ALLOWED)
                    throw new ArgumentException($"Minimihinta on {DOUBLE_MIN_ALLOWED:f2}€.");
                else if (value > DOUBLE_MAX_ALLOWED)
                    throw new ArgumentException($"Maksimihinta on {DOUBLE_MAX_ALLOWED:f2}€.");
                else
                    this.hinta = value;
            }
        }

        // ALV on prosenttimäärä, 0 - 100%.
        public double Alv
        {
            get => this.alv;
            set
            {
                if (!Double.IsNaN(value))
                {
                    if (value > ALV_MAX)
                        throw new ArgumentException($"ALV maksimiarvo on {ALV_MAX}%.");
                    else if (value < ALV_MIN)
                        throw new ArgumentException($"ALV minimiarvo on {ALV_MIN}%.");
                    else
                        this.alv = value;
                }
                else
                {
                    throw new ArgumentException("ALV täytyy olla numero.");
                }
            }
        }

        // Methods
    }
}
