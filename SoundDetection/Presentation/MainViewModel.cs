using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Presentation
{
    public class MainViewModel : ObservableObject
    {
        private int maxRow = 600;
        private int maxColumn = 1024;

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

        private double firstHarmonic;
        public double FirstHarmonic
        {
            get { return firstHarmonic; }
            set
            {
                if (firstHarmonic != value)
                {
                    firstHarmonic = value;
                    OnPropertyChanged("FirstHarmonic");
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

        public WriteableBitmap BitmapDisplay { get; private set; }

        public IRelayCommand GetFFTCommand { get; private set; }

        private readonly IMicrophone microphone;

        private Int32Rect rectangle;
        private int[,] colorInts;
        public MainViewModel(IMicrophone microphone)
        {
            this.microphone = microphone;
            Devcount = microphone.getDevcount();
            CreateBitmap();

            rectangle = new Int32Rect(0, 0, maxColumn, maxRow);
            colorInts = new int[maxRow, maxColumn];
            Task.Run(GetFFT);
            CompositionTarget.Rendering += DrawFFT;

        }

        private void CreateBitmap()
        {
            double dpiX = 96d;
            double dpiY = 96d;
            var pixelFormat = PixelFormats.Pbgra32;
            BitmapDisplay = new WriteableBitmap(maxColumn, maxRow, dpiX, dpiY, pixelFormat, null);
            OnPropertyChanged(nameof(BitmapDisplay));
        }

        private void DrawFFT(object sender, EventArgs e)
        {
            BitmapDisplay.WritePixels(rectangle, colorInts, BitmapDisplay.BackBufferStride, 0, 0);
        }

        private void GetFFT()
        {
            while (true)
            {
                List<Peak> peaks = new List<Peak>();
                double[] fftArray = microphone.getFrequency();
                fftArray = fftArray.Take(fftArray.Length / 2).ToArray();
                double max = fftArray.Max();
                int previous = 0;
                bool rising = false;
                if (max != 0)
                {
                    for(int i = 0; 1024 > i; i++ )
                    {
                        int scaled = (int)(fftArray[i] / 15 * (maxRow / 2 - 50));
                        if(i > 0 && fftArray[i] > fftArray[i - 1])
                        {
                            rising = true;
                        } else if(i > 0 && fftArray[i] < fftArray[i - 1] && rising)
                        {
                            rising = false;
                            if(previous > 120)
                            {
                                Peak peak = new Peak((i - 1) * 0.5859375, previous);
                                peaks.Add(peak);
                            }
                        }
                        for (int j = 0; maxRow > j; j++)
                        {
                            if (j == scaled)
                            {
                                colorInts[maxRow - 1 - j, i] = BitConverter.ToInt32(new byte[] { 0, 0, 0, 255 });
                                if (scaled > previous)
                                {
                                    for (int x = previous; x < scaled; x++)
                                    {
                                        colorInts[maxRow - 1 - x, i] = BitConverter.ToInt32(new byte[] { 0, 0, 0, 255 });
                                    }
                                }
                                if (scaled < previous)
                                {
                                    for (int x = scaled; x < previous; x++)
                                    {
                                        colorInts[maxRow - 1 - x, i] = BitConverter.ToInt32(new byte[] { 0, 0, 0, 255 });
                                    }
                                }

                                previous = scaled;

                            } else
                            {
                                colorInts[maxRow - 1 - j, i] = BitConverter.ToInt32(new byte[] { 255, 255, 255, 255 });
                            }
                        }
                    }
                    if(max > 10)
                    {

                        int index = Array.IndexOf(fftArray, max);
                        FFT = index * 0.5859375;

                        //Implement Harmonic Product Spectrum
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
                        FirstHarmonic = fund_freq * 8000 / 16384.0;

                    }
                    else
                    {
                        FFT = 0;
                    }
                }

            }
        }
    }
}
