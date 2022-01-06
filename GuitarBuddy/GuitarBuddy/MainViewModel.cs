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

        private readonly IMicrophone microphone;
        public MainViewModel(IMicrophone microphone)
        {
            //this.tuner = tuner;
            this.microphone = microphone;
            ToTunerCommand = new RelayCommand(ToTuner);
            ToSpectrumCommand = new RelayCommand(ToSpectrum);
            ToNotesCommand = new RelayCommand(ToNotes);
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
