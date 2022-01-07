using System;

namespace Fretboard
{
    public class Frets
    {

        public static string getNote(double frequency)
        {
            if (frequency > 80 && frequency < 84 || frequency > 163 && frequency < 167 || frequency > 327 && frequency < 331 || frequency > 657 && frequency < 661)
            {
                return "E";
            } else if(frequency > 85 && frequency < 89 || frequency > 173 && frequency < 177 || frequency > 347 && frequency < 351 || frequency > 696 && frequency < 700)
            {
                return "F";
            } else if(frequency > 90 && frequency < 94 || frequency > 183 && frequency < 187 || frequency > 368 && frequency < 372 || frequency > 738 && frequency < 742)
            {
                return "F#";
            }
            else if (frequency > 96 && frequency < 100 || frequency > 194 && frequency < 198 || frequency > 390 && frequency < 394 || frequency > 782 && frequency < 786)
            {
                return "G";
            }
            else if (frequency > 102 && frequency < 106 || frequency > 206 && frequency < 210 || frequency > 413 && frequency < 417 || frequency > 829 && frequency < 833)
            {
                return "G#";
            }
            else if (frequency > 108 && frequency < 112 || frequency > 218 && frequency < 222 || frequency > 438 && frequency < 442 || frequency > 878 && frequency < 882)
            {
                return "A";
            }
            else if (frequency > 115 && frequency < 119 || frequency > 231 && frequency < 235 || frequency > 464 && frequency < 468 || frequency > 930 && frequency < 934)
            {
                return "A#";
            }
            else if (frequency > 121 && frequency < 125 || frequency > 245 && frequency < 249 || frequency > 492 && frequency < 496 || frequency > 986 && frequency < 990)
            {
                return "B";
            }
            else if (frequency > 129 && frequency < 133 || frequency > 260 && frequency < 264 || frequency > 521 && frequency < 525 || frequency > 1045 && frequency < 1049)
            {
                return "C";
            }
            else if (frequency > 137 && frequency < 141 || frequency > 275 && frequency < 279 || frequency > 552 && frequency < 526)
            {
                return "C#";
            }
            else if (frequency > 145 && frequency < 149 || frequency > 292 && frequency < 296 || frequency > 585 && frequency < 589)
            {
                return "D";
            }
            else if (frequency > 154 && frequency < 158 || frequency > 309 && frequency < 313 || frequency > 620 && frequency < 624)
            {
                return "D#";
            } else
            {
                return "NA";
            }

        }

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
