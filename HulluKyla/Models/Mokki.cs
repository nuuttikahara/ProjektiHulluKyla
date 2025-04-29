using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    public class Mokki
    {
        // Properties
        private readonly uint mokkiId;
        private uint alueId;
        private string postinro;
        private string? mokkiNimi;
        private string? katuosoite;
        private double hinta;
        private string? kuvaus;
        private long? henkiloMaara;
        private string? varustelu;

        // Constants
        // Max
        private const int KUVAUS_MAX_LENGTH = 150;
        private const int VARUSTELU_MAX_LENGTH = 100;
        private const int MOKKINIMI_MAX_LENGTH = 45;
        private const int KATUOSOITE_MAX_LENGTH = 45;
        private const double DOUBLE_MAX_ALLOWED = 999999.99;

        // Min
        private const long HENKILOMAARA_MIN_ALLOWED = 0;
        private const double DOUBLE_MIN_ALLOWED = 0;

        // Default
        private const long HENKILOMAARA_DEFAULT = 0;

        // Null String return value
        private const string STRING_NULL = "NULL";

        // Constructors
        // Tietokanta import.
        public Mokki(
            uint mokkiId,
            uint alueId,
            string postinro,
            string mokkiNimi,
            string katuosoite,
            double hinta,
            string kuvaus,
            long henkiloMaara,
            string varustelu
        )
            : this(alueId, postinro, mokkiNimi, katuosoite, hinta, kuvaus, henkiloMaara, varustelu)
        {
            this.mokkiId = mokkiId;
        }

        // Uusille.
        public Mokki(
            uint alueId,
            string postinro,
            string mokkiNimi,
            string katuosoite,
            double hinta,
            string kuvaus,
            long henkiloMaara,
            string varustelu
        )
            : this(alueId, postinro, hinta)
        {
            this.MokkiNimi = mokkiNimi;
            this.Katuosoite = katuosoite;
            this.Kuvaus = kuvaus;
            this.HenkiloMaara = henkiloMaara;
            this.Varustelu = varustelu;
        }

        // Vain non-nullablet, paitsi mokkiId.
        public Mokki(uint alueId, string postinro, double hinta)
        {
            //INIT
            this.postinro = PostiUtil.POSTINRO_DEFAULT;
            this.mokkiNimi = null;
            this.katuosoite = null;
            this.kuvaus = null;
            this.henkiloMaara = HENKILOMAARA_DEFAULT;
            this.varustelu = null;
            // VALUES
            this.AlueId = alueId;
            this.Postinro = postinro;
            this.Hinta = hinta;
        }

        // Getters and Setters
        public uint MokkiId
        {
            get => this.mokkiId;
        }

        public uint AlueId
        {
            get => this.alueId;
            set { this.alueId = value; }
        }

        public string Postinro
        {
            get => this.postinro;
            set { this.postinro = PostiUtil.PostinroHandler(value); }
        }

        public string MokkiNimi
        {
            get
            {
                if (null != this.mokkiNimi)
                    return this.mokkiNimi;
                else
                    return STRING_NULL;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    this.mokkiNimi = null;
                else if (value.Trim().Length > MOKKINIMI_MAX_LENGTH)
                    throw new ArgumentException($"Mökin nimen maksimipituus on {MOKKINIMI_MAX_LENGTH} merkkiä.");
                else
                    this.mokkiNimi = value.Trim();
            }
        }

        public string Katuosoite
        {
            get
            {
                if (null != this.katuosoite)
                    return this.katuosoite;
                else
                    return STRING_NULL;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    this.katuosoite = null;
                else if (value.Trim().Length > KATUOSOITE_MAX_LENGTH)
                    throw new ArgumentException($"Katuosoitteen maksimipituus on {KATUOSOITE_MAX_LENGTH} merkkiä.");
                else
                    this.katuosoite = value.Trim();
            }
        }

        public double Hinta
        {
            get => hinta;
            set
            {
                if (value > DOUBLE_MAX_ALLOWED)
                {
                    throw new ArgumentOutOfRangeException(null, $"Maksimihinta on {DOUBLE_MAX_ALLOWED:f2}€.");
                }
                else if (value < DOUBLE_MIN_ALLOWED)
                {
                    throw new ArgumentOutOfRangeException(null, $"Minimihinta on {DOUBLE_MIN_ALLOWED:f2}€.");
                }
                else
                {
                    this.hinta = value;
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
                else if (value.Trim().Length > KUVAUS_MAX_LENGTH)
                {
                    throw new ArgumentException($"Kuvauksen maksimipituus on {KUVAUS_MAX_LENGTH} merkkiä.");
                }
                else
                {
                    this.kuvaus = value.Trim();
                }
            }
        }

        public long HenkiloMaara
        {
            get
            {
                if (null != this.henkiloMaara)
                    return (long)this.henkiloMaara;
                else
                    return 0;
            }
            set
            {
                if (value > long.MaxValue)
                {
                    throw new ArgumentException($"Henkilömäärän maksimiarvo on {long.MaxValue}.");
                }
                else if (value < HENKILOMAARA_MIN_ALLOWED)
                {
                    throw new ArgumentException($"Henkilömäärä ei voi olla alle {HENKILOMAARA_MIN_ALLOWED}.");
                }
                else
                {
                    this.henkiloMaara = value;
                }
            }
        }

        public string Varustelu
        {
            get
            {
                if (null != this.varustelu)
                    return this.varustelu;
                else
                    return STRING_NULL;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    this.varustelu = null;
                }
                else if (value.Trim().Length > VARUSTELU_MAX_LENGTH)
                {
                    throw new ArgumentException($"Varustelun maksimipituus on {VARUSTELU_MAX_LENGTH} merkkiä.");
                }
                else
                {
                    this.varustelu = value.Trim();
                }
            }
        }

        // Methods
    }
}
