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
        public List<string> GetDevcount();
        public double[] GetFrequency();
        public void ChangeDevice(int deviceIndex);
    }
}
