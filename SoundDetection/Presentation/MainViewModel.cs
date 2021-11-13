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
                double[] fftArray = microphone.getFrequency();
                double[] halfArray = fftArray.Take(fftArray.Length / 2).ToArray();
                double max = fftArray.Max();
                int previous = 0;
                if (max != 0)
                {
                    for(int i = 0; fftArray.Length / 2 > i; i++ )
                    {
                        int scaled = (int)(fftArray[i] / 500 * (maxRow / 2 - 50));
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
                    int index = Array.IndexOf(halfArray, max);
                    //FFT = index * 10.7666015625 * 2;
                    FFT = index;
                }

            }
        }
    }
}
