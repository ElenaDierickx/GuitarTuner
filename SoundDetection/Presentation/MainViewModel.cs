using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    public class MainViewModel : ObservableObject
    {
        private double fft;
        public double FFT
        {
            get { return fft; }
            set
            {
                if (fft != value)
                {
                    fft = value;
                    OnPropertyChanged("FFT");
                }
            }
        }

        private string devcount;
        public string Devcount
        {
            get { return devcount; }
            set
            {
                if (devcount != value)
                {
                    devcount = value;
                    OnPropertyChanged("Devcount");
                }
            }
        }

        public IRelayCommand GetFFTCommand { get; private set; }

        private readonly IMicrophone microphone;
        public MainViewModel(IMicrophone microphone)
        {
            this.microphone = microphone;

            Devcount = microphone.getDevcount();

            Task.Run(GetFFT);
            
        }

        private void GetFFT()
        {
            while (true)
            {
                double[] fftArray = microphone.getFrequency();
                FFT = fftArray[fftArray.Length - 1];
            }
        }
    }
}
