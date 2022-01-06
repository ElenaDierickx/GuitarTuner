using Fretboard;
using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarBuddy
{
    public class NoteRecognizerViewModel : ObservableObject
    {
        private string fret;
        public string Fret
        {
            get { return fret; }
            set
            {
                if (fret != value)
                {
                    fret = value;
                    OnPropertyChanged("Fret");
                }
            }
        }

        private readonly IMicrophone microphone;
        public NoteRecognizerViewModel(IMicrophone microphone)
        {
            this.microphone = microphone;
            Task.Run(GetFFT);
        }

        private void GetFFT()
        {
            double freq;
            while (true)
            {

                double[] fftArray = microphone.getFrequency();
                double max = fftArray.Max();
                if (max != 0)
                {
                    int fund_freq = 0;
                    double[] sum = new double[fftArray.Length / 8];
                    double max_value2 = max;
                    for (int k = 0; k < fftArray.Length / 8; k++)
                    {
                        sum[k] = fftArray[k] * fftArray[2 * k] * fftArray[3 * k];
                        // find fundamental frequency (maximum value in plot)
                        if (sum[k] > max_value2 && k > 0)
                        {
                            max_value2 = sum[k];
                            fund_freq = k;
                        }
                    }
                    freq = fund_freq * 8000 / 16384.0;
                    Fret = Frets.getString(freq);
                }


            }
        }
    }
}
