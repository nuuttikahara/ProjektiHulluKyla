using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    public class Asiakas
    {
        private readonly int asiakasId;
        private string etunimi;
        private string sukunimi;
        private string lahiosoite;
        private string postinro;
        private string email;
        private string puhelinnro;


        // Konstruktori ilman asiakasId:tä (esim. luodessa uusi asiakas)
        public Asiakas(string etunimi, string sukunimi, string lahiosoite,
            string postinro, string email, string puhelinnro) 
        {
            Etunimi = etunimi;
            Sukunimi = sukunimi;
            Lahiosoite = lahiosoite;
            Postinro = postinro;
            Email = email;
            Puhelinnro = puhelinnro;
        }


        // Konstruktori ID:llä (esim. haettaessa tietokannasta)
        public Asiakas(int id, string etunimi, string sukunimi, string lahiosoite, string postinro, string email, string puhelinnro)
            : this(etunimi, sukunimi, lahiosoite, postinro, email, puhelinnro)
        {
            this.asiakasId = id;
        }
        


        // Getterit ja setterit

        public int AsiakasId => asiakasId;


        public string Etunimi 
        {
            get => etunimi;
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Etunimi ei voi olla tyhjä.");

                etunimi = value;
            }
        }

        public string Sukunimi 
        {
            get => sukunimi;
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Sukunimi ei voi olla tyhjä.");

                sukunimi = value;
            }
        }

        public string Lahiosoite 
        {
            get => lahiosoite;
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Lähiosoite ei voi olla tyhjä");

                lahiosoite = value;
            }
        }

        public string Postinro
        {
            get => postinro;
            set 
            {
                if (!string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Postinumero ei voi olla tyhjä tai null.");

                postinro = value;
            }
        }

        public string Email 
        {
            get => email;
            set 
            {
                if (string.IsNullOrEmpty(value) || !value.Contains("@"))
                    throw new ArgumentException("Virheellinen sähköpostiosoite.");

                email = value;
            }
        }

        public string Puhelinnro 
        {
            get => puhelinnro;
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Puhelinnumero ei voi olla tyhjä.");

                puhelinnro = value;
            }
        }
    }
}
