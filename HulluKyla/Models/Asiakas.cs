using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    public class Asiakas
    {
        // Properties
        private readonly uint asiakasId;
        private string? etunimi;
        private string? sukunimi;
        private string? lahiosoite;
        private string postinro;
        private string? email;
        private string? puhelinnro;

        // Constants
        // Max
        private const int ETUNIMI_MAX_LENGTH = 20;
        private const int SUKUNIMI_MAX_LENGTH = 40;
        private const int LAHIOSOITE_MAX_LENGTH = 40;
        private const int EMAIL_MAX_LENGTH = 50;
        private const int PUHELINNRO_MAX_LENGTH = 15;

        // Null String return value
        private const string STRING_NULL = "NULL";

        // Konstruktori ilman asiakasId:tä (esim. luodessa uusi asiakas)
        public Asiakas(
            string etunimi,
            string sukunimi,
            string lahiosoite,
            string postinro,
            string email,
            string puhelinnro
        )
        {
            // INIT
            this.postinro = PostiUtil.POSTINRO_DEFAULT;
            // VALUES
            Etunimi = etunimi;
            Sukunimi = sukunimi;
            Lahiosoite = lahiosoite;
            Postinro = postinro;
            Email = email;
            Puhelinnro = puhelinnro;
        }

        // Konstruktori ID:llä (esim. haettaessa tietokannasta)
        public Asiakas(
            uint id,
            string etunimi,
            string sukunimi,
            string lahiosoite,
            string postinro,
            string email,
            string puhelinnro
        )
            : this(etunimi, sukunimi, lahiosoite, postinro, email, puhelinnro)
        {
            this.asiakasId = id;
        }

        // Getterit ja setterit
        // id:llä pelkkä get koska se on readonly
        public uint AsiakasId => asiakasId;

        public string Etunimi
        {
            get
            {
                if (null != this.etunimi)
                    return this.etunimi;
                else
                    return STRING_NULL;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    etunimi = null;
                }
                else if (value.Trim().Length > ETUNIMI_MAX_LENGTH)
                {
                    throw new ArgumentException(
                        "Etunimen maksimipituus on {0} merkkiä.",
                        ETUNIMI_MAX_LENGTH.ToString()
                    );
                }
                else
                {
                    etunimi = value.Trim();
                }
            }
        }

        public string Sukunimi
        {
            get
            {
                if (null != this.sukunimi)
                    return this.sukunimi;
                else
                    return STRING_NULL;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    sukunimi = null;
                }
                else if (value.Trim().Length > SUKUNIMI_MAX_LENGTH)
                {
                    throw new ArgumentException(
                        "Sukunimen maksimipituus on {0} merkkiä.",
                        SUKUNIMI_MAX_LENGTH.ToString()
                    );
                }
                else
                {
                    sukunimi = value.Trim();
                }
            }
        }

        public string Lahiosoite
        {
            get
            {
                if (null != this.lahiosoite)
                    return this.lahiosoite;
                else
                    return STRING_NULL;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    lahiosoite = null;
                }
                else if (value.Trim().Length > LAHIOSOITE_MAX_LENGTH)
                {
                    throw new ArgumentException(
                        "Lähiosoitteen maksimipituus on {0} merkkiä.",
                        LAHIOSOITE_MAX_LENGTH.ToString()
                    );
                }
                else
                {
                    lahiosoite = value.Trim();
                }
            }
        }

        public string Postinro
        {
            get => postinro;
            set { this.postinro = PostiUtil.PostinroHandler(value); }
        }

        public string Email
        {
            get
            {
                if (null != this.email)
                    return this.email;
                else
                    return STRING_NULL;
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    email = null;
                }
                else if (!value.Contains('@'))
                {
                    throw new ArgumentException("Virheellinen sähköpostiosoite.");
                }
                else if (value.Trim().Length > EMAIL_MAX_LENGTH)
                    throw new ArgumentException(
                        "Sähköpostiosoitteen maksimipituus on {0} merkkiä.",
                        EMAIL_MAX_LENGTH.ToString()
                    );
                else
                    email = value.Trim();
            }
        }

        public string Puhelinnro
        {
            get
            {
                if (null != this.puhelinnro)
                    return this.puhelinnro;
                else
                    return STRING_NULL;
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    string parsed = "";
                    foreach (char c in value)
                    {
                        if (Char.IsNumber(c))
                            parsed += c;
                    }

                    if (
                        !String.IsNullOrWhiteSpace(parsed)
                        && parsed.Length <= PUHELINNRO_MAX_LENGTH
                    )
                        this.puhelinnro = parsed.TrimStart();
                }
                else if (value.Trim().Length > PUHELINNRO_MAX_LENGTH)
                {
                    throw new ArgumentException(
                        "Puhelinnumeron maksimipituus on {0} merkkiä.",
                        PUHELINNRO_MAX_LENGTH.ToString()
                    );
                }
                else
                {
                    throw new ArgumentException("Puhelinnumero ei voi olla tyhjä.");
                }
            }
        }
    }
}
