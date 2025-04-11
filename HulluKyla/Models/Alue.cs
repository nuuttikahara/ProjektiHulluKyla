using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    public class Alue
    {
        // Properties
        private readonly uint alueId;
        private string? nimi;

        // Static Properties
        private readonly int NIMI_MAX_LENGTH = 40;

        // Constructors
        public Alue(string nimi)
        {
            this.Nimi = nimi;
        }

        public Alue(uint alueId, string nimi)
        {
            this.alueId = alueId;
            this.Nimi = nimi;
        }

        // Getters and Setters
        public uint AlueId => alueId;

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
                    nimi = null;
                }
                else if (value.Trim().Length > NIMI_MAX_LENGTH)
                {
                    throw new ArgumentException(
                        "Nimen maksimipituus on {0} merkkiä.",
                        NIMI_MAX_LENGTH.ToString()
                    );
                }
                else
                {
                    nimi = value.Trim();
                }
            }
        }

        // Methods
    }
}
