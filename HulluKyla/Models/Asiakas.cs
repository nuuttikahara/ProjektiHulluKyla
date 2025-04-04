using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    public class Asiakas
    {
        private readonly uint asiakasId;
        private string? etunimi;
        private string? sukunimi;
        private string? lahiosoite;
        private string postinro;
        private string? email;
        private string? puhelinnro;

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
            this.postinro = "00720";
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
                    return "";
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    etunimi = null;
                else if (value.Trim().Length > 20)
                    throw new ArgumentException("Etunimen maksimipituus on 20 merkkiä.");
                else
                    etunimi = value.Trim();
            }
        }

        public string Sukunimi
        {
            get
            {
                if (null != this.sukunimi)
                    return this.sukunimi;
                else
                    return "";
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    sukunimi = null;
                else if (value.Trim().Length > 40)
                    throw new ArgumentException("Sukunimen maksimipituus on 40 merkkiä.");
                else
                    sukunimi = value.Trim();
            }
        }

        public string Lahiosoite
        {
            get
            {
                if (null != this.lahiosoite)
                    return this.lahiosoite;
                else
                    return "";
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    lahiosoite = null;
                else if (value.Trim().Length > 40)
                    throw new ArgumentException("Lähiosoitteen maksimipituus on 40 merkkiä.");
                else
                    lahiosoite = value.Trim();
            }
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
                else
                    postinro = value.Trim();
            }
        }

        public string Email
        {
            get
            {
                if (null != this.email)
                    return this.email;
                else
                    return "";
            }
            set
            {
                if (!value.Contains("@"))
                    throw new ArgumentException("Virheellinen sähköpostiosoite.");
                else if (String.IsNullOrWhiteSpace(value))
                    email = null;
                else if (value.Trim().Length > 50)
                    throw new ArgumentException("Sähköpostiosoitteen maksimipituus on 50 merkkiä.");
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
                    return "";
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Puhelinnumero ei voi olla tyhjä.");
                else if (value.Trim().Length > 15)
                    throw new ArgumentException("Puhelinnumeron maksimipituus on 15 merkkiä.");
                else
                    puhelinnro = value.Trim();
            }
        }
    }
}
