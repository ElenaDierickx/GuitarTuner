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
        }
        private void ToE()
        {
            achieveFrequency = 82.41;
        }
        private void ToA()
        {
            achieveFrequency = 110.0;
        }
        private void ToD()
        {
            achieveFrequency = 146.83;
        }
        private void ToG()
        {
            achieveFrequency = 196.00;
        }
        private void ToB()
        {
            achieveFrequency = 246.94;
        }
        private void Toe()
        {
            achieveFrequency = 329.63;
        }
    }
}
