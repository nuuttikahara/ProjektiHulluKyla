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
        private uint varausId;
        private Palvelu palvelu;
        private int lkm;


        public VarauksenPalvelu(uint varausId, Palvelu palvelu, int lkm) 
        {
            this.VarausId = varausId;
            this.Palvelu = palvelu;
            this.Lkm = lkm;
        }

        public uint VarausId 
        {
            get { return varausId; }
            set { varausId = value; }
        }

        public Palvelu Palvelu 
        {
            get { return palvelu; }
            set { palvelu = value; }
        }

        public int Lkm 
        {
            get { return lkm; }
            set { lkm = value; }
        }
    }
}
