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

        // Static Properties
        private readonly int KUVAUS_MAX = 150;
        private readonly int VARUSTELU_MAX = 100;
        private readonly long HENKILOMAARA_DEFAULT = 0;
        private readonly double DOUBLE_MIN_ALLOWED = 0;
        private readonly string POSTINRO_DEFAULT = "00720";

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
            this.postinro = POSTINRO_DEFAULT;
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
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Postinumero ei voi olla tyhjä tai null.");
                else if (value.Trim().Length != 5)
                    throw new ArgumentException("Postinumeron täytyy olla 5 merkkiä pitkä.");
                else
                    this.postinro = value.Trim();
            }
        }

        public string MokkiNimi
        {
            get
            {
                if (null != this.mokkiNimi)
                    return this.mokkiNimi;
                else
                    return "";
            }
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
            get
            {
                if (null != this.katuosoite)
                    return this.katuosoite;
                else
                    return "";
            }
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
                if (value > Double.MaxValue)
                {
                    throw new ArgumentException(
                        "Maksimihinta on {0:f2}€.",
                        Double.MaxValue.ToString()
                    );
                }
                else if (value < 0)
                {
                    throw new ArgumentException("Hinta ei voi olla alle 0.");
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
                if (value >= LMAX)
                {
                    throw new ArgumentException(
                        "Henkilömäärä ei voi olla yhtä suuri tai suurempi kuin {0}.",
                        LMAX.ToString()
                    );
                }
                else if (value < 0)
                {
                    throw new ArgumentException("Henkilömäärä ei voi olla alle 0.");
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
                    return "";
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    this.varustelu = null;
                }
                else if (value.Trim().Length > VARUSTELU_MAX)
                {
                    throw new ArgumentException(
                        "Varustelun maksimipituus on {0} merkkiä.",
                        VARUSTELU_MAX.ToString()
                    );
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
