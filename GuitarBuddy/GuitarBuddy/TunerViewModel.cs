using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GuitarBuddy
{
    public class TunerViewModel : ObservableObject
    {
        private int maxRow = 350;
        private int maxColumn = 630;
        public IRelayCommand ToFirstCommand { get; private set; }
        public IRelayCommand ToSecondCommand { get; private set; }
        public IRelayCommand ToThirdCommand { get; private set; }
        public IRelayCommand ToFourthCommand { get; private set; }
        public IRelayCommand ToFifthCommand { get; private set; }
        public IRelayCommand ToSixthCommand { get; private set; }

        public IRelayCommand StopCommand { get; private set; }

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

        private Tuning tuning;
        public Tuning Tuning
        {
            get { return tuning; }
            set
            {
                if (tuning != value)
                {
                    tuning = value;
                    ToFirst();
                    OnPropertyChanged("Tuning");
                }
            }
        }

        private List<Tuning> tunings;
        public List<Tuning> Tunings
        {
            get { return tunings; }
            set
            {
                if (tunings != value)
                {
                    tunings = value;
                    OnPropertyChanged("Tunings");
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

        public WriteableBitmap BitmapDisplay { get; private set; }
        CancellationTokenSource tokenSource;

        public TunerViewModel(IMicrophone microphone)
        {
            this.microphone = microphone;
            CreateBitmap();
            ToFirstCommand = new RelayCommand(ToFirst);
            ToSecondCommand = new RelayCommand(ToSecond);
            ToThirdCommand = new RelayCommand(ToThird);
            ToFourthCommand = new RelayCommand(ToFourth);
            ToFifthCommand = new RelayCommand(ToFifth);
            ToSixthCommand = new RelayCommand(ToSixth);
            StopCommand = new RelayCommand(StopFFT);

            Tuning RegularTuning = new Tuning("Regular Tuning", new List<string> { "E", "A", "D", "G", "B", "E" }, new List<double> { 82.41, 110.0, 146.83, 196.00, 246.0, 329.63 });
            Tuning EFlatTuning = new Tuning("E Flat Tuning", new List<string> { "D#", "G#", "C#", "F#", "A#", "D#" }, new List<double> { 77.78, 103.83, 138.59, 185.0, 233.08, 311.13 });
            Tuning DTuning = new Tuning("Open D Tuning", new List<string> { "D", "A", "D", "F#", "A", "D" }, new List<double> { 73.42, 110.0, 146.83, 185.0, 220.00, 293.66 });
            Tuning DropDTuning = new Tuning("Drop D Tuning", new List<string> { "D", "A", "D", "G", "B", "E" }, new List<double> { 73.42, 110.0, 146.83, 196.00, 246.0, 329.63 });
            Tuning OpenGTuning = new Tuning("Open G Tuning", new List<string> { "D", "G", "D", "G", "B", "D" }, new List<double> { 73.42, 98.00, 146.83, 196.00, 246.0, 293.66 });
            Tunings = new List<Tuning> { RegularTuning, EFlatTuning, DTuning, DropDTuning, OpenGTuning };
            Tuning = RegularTuning;

            ToFirst();

            tokenSource = new CancellationTokenSource();
            Task.Run(GetFFT, tokenSource.Token);
        }

        private void StopFFT()
        {
            tokenSource.Cancel();
        }

        private void CreateBitmap()
        {
            double dpiX = 96d;
            double dpiY = 96d;
            var pixelFormat = PixelFormats.Pbgra32;
            BitmapDisplay = new WriteableBitmap(maxColumn, maxRow, dpiX, dpiY, pixelFormat, null);
            OnPropertyChanged(nameof(BitmapDisplay));
            CompositionTarget.Rendering += DrawTuner;
        }

        private void DrawTuner(object sender, EventArgs e)
        {
            Int32Rect rectangle = new Int32Rect(0, 0, maxColumn, maxRow);
            int[,] colorInts = new int[maxRow, maxColumn];
            int halfline = maxColumn / 2;
            int scaled = (int)(FFT / achieveFrequency * 315);
            for (int x = 0; x < maxColumn; x++)
            {
                for (int y = 0; y < maxRow; y++)
                {
                    if (halfline == x)
                    {
                        colorInts[y, x] = BitConverter.ToInt32(new byte[] { 0, 0, 0, 255 });
                    }
                    if (scaled == x && scaled > 1 && scaled < maxColumn - 1)
                    {
                        colorInts[y, x] = BitConverter.ToInt32(new byte[] { 0, 0, 255, 255 });
                        colorInts[y, x + 1] = BitConverter.ToInt32(new byte[] { 0, 0, 255, 255 });
                        colorInts[y, x - 1] = BitConverter.ToInt32(new byte[] { 0, 0, 255, 255 });
                    }
                }
            }

            BitmapDisplay.WritePixels(rectangle, colorInts, BitmapDisplay.BackBufferStride, 0, 0);
        }

        private void ToFirst()
        {
            if (Tuning != null)
            {
                Note = Tuning.Notes[0];
                achieveFrequency = Tuning.Frequencies[0];
            }
        }
        private void ToSecond()
        {
            Note = Tuning.Notes[1];
            achieveFrequency = Tuning.Frequencies[1];
        }
        private void ToThird()
        {
            Note = Tuning.Notes[2];
            achieveFrequency = Tuning.Frequencies[2];
        }
        private void ToFourth()
        {
            Note = Tuning.Notes[3];
            achieveFrequency = Tuning.Frequencies[3];
        }
        private void ToFifth()
        {
            Note = Tuning.Notes[4];
            achieveFrequency = Tuning.Frequencies[4];
        }
        private void ToSixth()
        {
            Note = Tuning.Notes[5];
            achieveFrequency = Tuning.Frequencies[5];
        }

        private void CheckFrequency()
        {
            if (FFT < achieveFrequency - 1)
            {
                HigherLower = "Higher";
            }
            else if (FFT > achieveFrequency + 1)
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
            CancellationToken ct = tokenSource.Token;
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
                    FFT = fund_freq * 8000 / 16384.0;
                }


            }
        }
    }
}
