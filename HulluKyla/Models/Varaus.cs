using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    public class Varaus
    {
        // Properties
        private uint varausId;
        private Asiakas asiakas;
        private Mokki mokki;
        private DateTime varattuPvm;
        private DateTime vahvistusPvm;
        private DateTime varattuAlkuPvm;
        private DateTime varattuLoppuPvm;

        // Constants and Static Properties
        // Default
        private readonly Mokki MOKKI_DEFAULT = new Mokki(
            uint.MaxValue,
            PostiUtil.POSTINRO_DEFAULT,
            double.MaxValue
        );
        private readonly DateTime VARATTU_PVM_DEFAULT = DateTime.MinValue;
        private readonly DateTime VAHVISTUS_PVM_DEFAULT = DateTime.MinValue;
        private readonly DateTime VARATTU_ALKU_PVM_DEFAULT = DateTime.MinValue;
        private readonly DateTime VARATTU_LOPPU_PVM_DEFAULT = DateTime.MaxValue;
        private readonly Asiakas ASIAKAS_DEFAULT = new Asiakas(
            "Oletus",
            "Asiakas",
            "Oletuskatu 1",
            "00720",
            "oletus@email.com",
            "0123456789"
        );

        // Constructors
        // Non-nullables
        public Varaus(uint varausId, Asiakas asiakas, Mokki mokki)
        {
            // INIT
            this.varausId = uint.MinValue;
            this.asiakas = ASIAKAS_DEFAULT;
            this.mokki = MOKKI_DEFAULT;
            this.VarattuPvm = VARATTU_PVM_DEFAULT;
            this.VahvistusPvm = VAHVISTUS_PVM_DEFAULT;
            this.VarattuAlkuPvm = VARATTU_ALKU_PVM_DEFAULT;
            this.VarattuLoppuPvm = VARATTU_LOPPU_PVM_DEFAULT;
            // VALUES
            this.VarausId = varausId;
            this.Asiakas = asiakas;
            this.Mokki = mokki;
        }

        // Database import
        public Varaus(
            uint varausId,
            Asiakas asiakas,
            Mokki mokki,
            DateTime varattuPvm,
            DateTime vahvistusPvm,
            DateTime varattuAlkuPvm,
            DateTime varattuLoppuPvm
        )
            : this(varausId, asiakas, mokki)
        {
            this.VarattuPvm = varattuPvm;
            this.VahvistusPvm = vahvistusPvm;
            this.VarattuAlkuPvm = varattuAlkuPvm;
            this.VarattuLoppuPvm = varattuLoppuPvm;
        }

        // Getters and Setters
        public uint VarausId
        {
            get { return this.varausId; }
            private set { this.varausId = value; }
        }

        public Asiakas Asiakas
        {
            get { return this.asiakas; }
            set
            {
                if (null != value && Asiakas.GetType().Equals(value.GetType()))
                    this.asiakas = value;
                else
                    throw new ArgumentException("Virheellinen asiakastieto.");
            }
        }

        public Mokki Mokki
        {
            get { return this.mokki; }
            set
            {
                if (null != value && Mokki.GetType().Equals(value.GetType()))
                    this.mokki = value;
                else
                    throw new ArgumentException("Virheellinen mökkitieto.");
            }
        }

        public DateTime VarattuPvm
        {
            get { return this.varattuPvm; }
            set { this.varattuPvm = value; }
        }

        public DateTime VahvistusPvm
        {
            get { return this.vahvistusPvm; }
            set { this.vahvistusPvm = value; }
        }

        public DateTime VarattuAlkuPvm
        {
            get { return this.varattuAlkuPvm; }
            // Checks that value is before varattuLoppuPvm.
            set
            {
                if (DateTime.Compare(value, this.VarattuLoppuPvm) < 0)
                    this.varattuAlkuPvm = value;
                else
                    throw new ArgumentException(
                        "VarattuAlkuPvm täytyy olla ennen VarattuLoppuPvm."
                    );
            }
        }

        public DateTime VarattuLoppuPvm
        {
            get { return this.varattuLoppuPvm; }
            // Checks that value is after varattuAlkuPvm.
            set
            {
                if (DateTime.Compare(value, this.VarattuAlkuPvm) > 0)
                    this.varattuLoppuPvm = value;
                else
                    throw new ArgumentException(
                        "VarattuLoppuPvm täytyy olla VarattuAlkuPvm jälkeen."
                    );
            }
        }
        // Methods
    }
}
