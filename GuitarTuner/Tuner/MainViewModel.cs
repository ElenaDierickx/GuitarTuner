using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuner
{
    public class MainViewModel : ObservableObject
    {
        public IRelayCommand ToECommand { get; private set; }
        public IRelayCommand ToACommand { get; private set; }
        public IRelayCommand ToDCommand { get; private set; }
        public IRelayCommand ToGCommand { get; private set; }
        public IRelayCommand ToBCommand { get; private set; }
        public IRelayCommand ToeCommand { get; private set; }

        private string tuning;
        public string Tuning
        {
            get { return tuning; }
            set
            {
                if (tuning != value)
                {
                    tuning = value;
                    OnPropertyChanged("Tuning");
                }
            }
        }

        private double fft;
        public double FFT
        {
            get { return fft; }
            set
            {
                if (fft != value)
                {
                    fft = value;

                    CheckFrequency();
                    OnPropertyChanged("FFT");
                }
            }
        }

        private string higherLower;
        public string HigherLower
        {
            get { return higherLower; }
            set
            {
                if (higherLower != value)
                {
                    higherLower = value;
                    OnPropertyChanged("HigherLower");
                }
            }
        }

        private double achieveFrequency;

        private readonly IMicrophone microphone;
        public MainViewModel(IMicrophone microphone)
        {
            this.microphone = microphone;
            ToECommand = new RelayCommand(ToE);
            ToACommand = new RelayCommand(ToA);
            ToDCommand = new RelayCommand(ToD);
            ToGCommand = new RelayCommand(ToG);
            ToBCommand = new RelayCommand(ToB);
            ToeCommand = new RelayCommand(Toe);
            ToE();

            Task.Run(GetFFT);
        }
        private void ToE()
        {
            achieveFrequency = 82.41;
            Tuning = "E";
        }
        private void ToA()
        {
            achieveFrequency = 110.0;
            Tuning = "A";
        }
        private void ToD()
        {
            achieveFrequency = 146.83;
            Tuning = "D";
        }
        private void ToG()
        {
            achieveFrequency = 196.00;
            Tuning = "G";
        }
        private void ToB()
        {
            achieveFrequency = 246.94;
            Tuning = "B";
        }
        private void Toe()
        {
            achieveFrequency = 329.63;
            Tuning = "e";
        }

        private void CheckFrequency()
        {
            if(FFT < achieveFrequency - 1)
            {
                HigherLower = "Higher";
            }
            else if(FFT > achieveFrequency + 1)
            {
                HigherLower = "Lower";
            }
            else
            {
                HigherLower = "Good";
            }
        }

        private void GetFFT()
        {
            while (true)
            {
                double[] fftArray = microphone.getFrequency();
                fftArray = fftArray.Take(fftArray.Length / 2).ToArray();
                double max = fftArray.Max();
                if (max != 0)
                {
                    int index = Array.IndexOf(fftArray, max);
                    FFT = index * 0.9765625;
                }

            }
        }
    }
}
