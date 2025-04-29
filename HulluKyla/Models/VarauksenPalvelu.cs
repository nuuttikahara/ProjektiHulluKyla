using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HulluKyla.Models;

namespace HulluKyla.Models
{
    public class VarauksenPalvelu
    {
        // Properties
        private uint varausId;
        private Palvelu palvelu;
        private int lkm;

        // Constants and Static Properties
        private readonly Palvelu DEFAULT_PALVELU = new Palvelu(
            1000,
            1000,
            0.0,
            0.0
        );

        // Constructors
        public VarauksenPalvelu(uint varausId, int lkm, Palvelu palvelu)
        {
            // INIT
            this.palvelu = DEFAULT_PALVELU;
            // VALUES
            this.VarausId = varausId;
            this.Lkm = lkm;
            this.Palvelu = palvelu;
        }

        // Getters and Setters
        public uint VarausId
        {
            get { return varausId; }
            set { varausId = value; }
        }

        public Palvelu Palvelu
        {
            get { return this.palvelu; }
            set
            {
                if (null != value && Palvelu.GetType().Equals(value.GetType()))
                    this.palvelu = value;
                else
                    throw new ArgumentException("Palvelu ei voi olla tyhjä.");
            }
        }

        public int Lkm
        {
            get { return lkm; }
            set { lkm = value; }
        }
    }
}
