using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HulluKyla.Models
{
    // Vain postinumeroiden käsittelyyn. Ei luotavaksi.
    public static class PostiUtil
    {
        // Constants
        public const string POSTINRO_DEFAULT = "00720";
        public const int POSTINRO_LENGTH = 5;

        // Methods
        public static String PostinroHandler(String postinro)
        {
            if (!String.IsNullOrWhiteSpace(postinro))
            {
                String parsed = "";
                foreach (Char c in postinro)
                {
                    if (Char.IsNumber(c))
                        parsed += c;
                }

                if (null != parsed && parsed.Trim().Length == POSTINRO_LENGTH)
                    return parsed.TrimStart();
                else
                    throw new ArgumentException($"Postinumeron täytyy olla {POSTINRO_LENGTH} merkkiä pitkä.");
            }
            else
                throw new ArgumentException("Postinumeron täytyy olla numero.");
        }
    }
}
