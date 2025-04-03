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
        private string nimi;

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
            get => nimi;
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nimi ei voi olla tyhjä.");
                else if (value.Trim().Length > 40)
                    throw new ArgumentException("Nimen maksimipituus on 40 merkkiä.");

                nimi = value.Trim();
            }
        }

        // Methods
    }
}
