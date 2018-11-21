using System;

namespace Entity
{
    public class BaseEntity
    {
        public static decimal RoundDecimal(decimal dec, int digits)
        {
            return Math.Round(dec, digits, MidpointRounding.AwayFromZero);
        }
    }
}
