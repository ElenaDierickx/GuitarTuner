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
        public IRelayCommand ToSpectrumCommand { get; private set; }
        public IRelayCommand ToNotesCommand { get; private set; }

        private string device;
        public string Device
        {
            get { return device; }
            set
            {
                if (device != value)
                {
                    device = value;
                    OnPropertyChanged("Device");
                    ChangeDevice();
                }
            }
        }

        private List<string> devices;
        public List<string> Devices
        {
            get { return devices; }
            set
            {
                if (devices != value)
                {
                    devices = value;

                    OnPropertyChanged("Devices");
                }
            }
        }

        private readonly IMicrophone microphone;
        public MainViewModel(IMicrophone microphone)
        {
            //this.tuner = tuner;
            this.microphone = microphone;
            ToTunerCommand = new RelayCommand(ToTuner);
            ToSpectrumCommand = new RelayCommand(ToSpectrum);
            ToNotesCommand = new RelayCommand(ToNotes);

            List<string> devices = microphone.GetDevcount();

            Devices = devices;
            Device = devices[0];


        }

        private void ChangeDevice()
        {
            if (Device != null)
            {
                int index = Devices.FindIndex(a => a.Contains(Device));
                microphone.ChangeDevice(index);
            }
            
        }

        private void ToTuner()
        {
            TunerViewModel vm = new(microphone);
            Tuner tuner = new(vm);
            tuner.ShowDialog();
        }

        private void ToSpectrum()
        {
            SpectrumViewModel vm = new(microphone);
            Spectrum spectrum = new(vm);
            spectrum.ShowDialog();
            
        }

        private void ToNotes()
        {
            NoteRecognizerViewModel vm = new(microphone);
            NoteRecognizer notes = new(vm);
            notes.ShowDialog();
        }
    }
}
