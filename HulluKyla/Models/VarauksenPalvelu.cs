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

        // Static Properties
        private readonly Palvelu DEFAULT_PALVELU = new Palvelu(
            uint.MaxValue,
            uint.MaxValue,
            double.MaxValue,
            double.MaxValue
        );

        // Constructors
        public VarauksenPalvelu(uint varausId, int lkm, Palvelu palvelu)
        {
            this.palvelu = DEFAULT_PALVELU;
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
