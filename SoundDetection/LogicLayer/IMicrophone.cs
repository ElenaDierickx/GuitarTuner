using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IMicrophone
    {
        public string getDevcount();
        public double[] getFrequency();
    }
}
