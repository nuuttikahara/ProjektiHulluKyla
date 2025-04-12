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

        // Constants
        // Default
        private const string TOIMIPAIKKA_DEFAULT = "HELSINKI";

        // Null String return value
        private const string STRING_NULL = "NULL";

        // Constructors
        public Posti(string numero, string toimipaikka)
        {
            // INIT
            this.numero = PostiUtil.POSTINRO_DEFAULT;
            this.toimipaikka = TOIMIPAIKKA_DEFAULT;
            // VALUES
            this.Numero = numero;
            this.Toimipaikka = toimipaikka;
        }

        // Getters and Setters
        public string Numero
        {
            get { return this.numero; }
            set { this.numero = PostiUtil.PostinroHandler(value); }
        }

        public string Toimipaikka
        {
            get
            {
                if (null != this.toimipaikka)
                    return this.toimipaikka;
                else
                    return STRING_NULL;
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                    this.toimipaikka = value.Trim().ToUpper();
                else
                    throw new ArgumentException("Toimipaikka ei voi olla tyhjä.");
            }
        }
    }
}
