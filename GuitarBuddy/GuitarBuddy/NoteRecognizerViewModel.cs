using Fretboard;
using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GuitarBuddy
{
    public class NoteRecognizerViewModel : ObservableObject
    {
        private string note;
        public string Note
        {
            get { return note; }
            set
            {
                if (note != value)
                {
                    note = value;
                    OnPropertyChanged("Note");
                }
            }
        }

        public IRelayCommand StopCommand { get; private set; }

        CancellationTokenSource tokenSource;

        private readonly IMicrophone microphone;
        public NoteRecognizerViewModel(IMicrophone microphone)
        {
            this.microphone = microphone;

            StopCommand = new RelayCommand(StopFFT);

            tokenSource = new CancellationTokenSource();
            Task.Run(GetFFT, tokenSource.Token);
        }

        private void StopFFT()
        {
            tokenSource.Cancel();
        }

        private void GetFFT()
        {
            CancellationToken ct = tokenSource.Token;
            double freq;
            while (!ct.IsCancellationRequested)
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
                    Note = Frets.getNote(freq);
                }


            }
        }
    }
}
