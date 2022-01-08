using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public class Tuning
    {
        public string Name { get; private set; }
        public List<string> Notes { get; private set; }
        public List<double> Frequencies { get; private set; }

        public Tuning(string name, List<string> notes, List<double> frequencies)
        {
            Name = name;
            Notes = notes;
            Frequencies = frequencies;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
