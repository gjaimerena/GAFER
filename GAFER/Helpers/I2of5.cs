using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GAFER.Helpers
{
    public class I2of5
    {
        public static string Interleaved25(string input)
        {
            if (input.Length <= 0) return "";

            if (input.Length % 2 != 0)
            {
                input = "0" + input;
            }

            string result = "";

            //Takes pairs of numbers and convert them to chars
            for (int i = 0; i <= input.Length - 1; i += 2)
            {
                int pair = Int32.Parse(input.Substring(i, 2));

                if (pair < 90)
                    pair = pair + 33;
                else
                {
                    pair = pair + 71;
                }

                result = result + Convert.ToChar(pair);
            }

            //Leading and trailing chars.
            return (char)203 + result + (char)204;
        }
    }
}