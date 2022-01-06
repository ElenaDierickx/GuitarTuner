using LogicLayer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarBuddy
{
    public class MainViewModel : ObservableObject
    {
        public IRelayCommand ToTunerCommand { get; private set; }

        private readonly IMicrophone microphone;
        public MainViewModel(IMicrophone microphone)
        {
            //this.tuner = tuner;
            this.microphone = microphone;
            ToTunerCommand = new RelayCommand(ToTuner);
        }

        private void ToTuner()
        {
            TunerViewModel vm = new(microphone);
            Tuner tuner = new(vm);
            tuner.ShowDialog();
        }
    }
}
