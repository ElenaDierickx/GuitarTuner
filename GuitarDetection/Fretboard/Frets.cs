using System;

namespace Fretboard
{
    public class Frets
    {
        public static string getFret(double frequency)
        {
            if(frequency > 80.41)
            {
                if (frequency > 85.31)
                {
                    if(frequency > 90.50)
                    {
                        if (frequency > 102.83)
                        {
                            return "E2";
                        }
                        return "E2";
                    }
                    return "E1";
                }
                return "E0";
            } else
            {
                return "NA";
            }
        }

        public static string getString(double frequency)
        {
            if(frequency >= 80.41 && frequency <= 84.41)
            {
                return "E";
            }else if (frequency >= 108.0 && frequency <= 112.0)
            {
                return "A";
            }
            else if (frequency >= 144.83 && frequency <= 148.83)
            {
                return "D";
            }
            else if (frequency >= 194.00 && frequency <= 198.00)
            {
                return "G";
            }
            else if (frequency >= 244.94 && frequency <= 248.94)
            {
                return "B";
            }
            else if (frequency >= 327.63 && frequency <= 331.63)
            {
                return "e";
            }
            else
            {
                return "NA";
            }
        }
    }

}
