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
        public IRelayCommand ToECommand { get; private set; }
        public IRelayCommand ToACommand { get; private set; }
        public IRelayCommand ToDCommand { get; private set; }
        public IRelayCommand ToGCommand { get; private set; }
        public IRelayCommand ToBCommand { get; private set; }
        public IRelayCommand ToeCommand { get; private set; }

        public IRelayCommand StopCommand { get; private set; }

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

        public WriteableBitmap BitmapDisplay { get; private set; }
        CancellationTokenSource tokenSource;

        public TunerViewModel(IMicrophone microphone)
        {
            this.microphone = microphone;
            CreateBitmap();
            ToECommand = new RelayCommand(ToE);
            ToACommand = new RelayCommand(ToA);
            ToDCommand = new RelayCommand(ToD);
            ToGCommand = new RelayCommand(ToG);
            ToBCommand = new RelayCommand(ToB);
            ToeCommand = new RelayCommand(Toe);
            StopCommand = new RelayCommand(StopFFT);
            ToE();

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
