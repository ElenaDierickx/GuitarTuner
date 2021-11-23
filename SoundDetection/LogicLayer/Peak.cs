using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public class Peak
    {
        public double Frequency { get; set; }
        public double Amplitude { get; set; }

        public Peak(double frequency, double amplitude)
        {
            Frequency = frequency;
            Amplitude = amplitude;
        }
    }
}
