using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    public class Posti
    {
        // Properties
        private string numero;
        private string toimipaikka;

        // Static Properties
        private readonly int NUMERO_LENGTH = 5;

        // Constructors
        public Posti(string numero, string toimipaikka)
        {
            // INIT
            this.numero = "00720";
            this.toimipaikka = "HELSINKI";
            // VALUES
            this.Numero = numero;
            this.Toimipaikka = toimipaikka;
        }

        // Getters and Setters
        public string Numero
        {
            get { return this.numero; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    string parsed = "";
                    foreach (char c in value)
                    {
                        parsed += c;
                    }
                    if (null != parsed && parsed.Length == NUMERO_LENGTH)
                    {
                        this.numero = parsed.TrimStart();
                    }
                    else
                    {
                        throw new ArgumentException(
                            "Postinumeron täytyy olla {0} merkkiä pitkä.",
                            NUMERO_LENGTH.ToString()
                        );
                    }
                }
                else
                {
                    throw new ArgumentException("Postinumero ei voi olla tyhjä.");
                }
            }
        }

        public string Toimipaikka
        {
            get
            {
                if (null != this.toimipaikka)
                    return this.toimipaikka;
                else
                    return "";
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    this.toimipaikka = value.Trim().ToUpper();
                else
                    throw new ArgumentException("Toimipaikka ei voi olla tyhjä.");
            }
        }
    }
}
