using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NAudio.Wave; // installed with nuget
using NAudio.CoreAudioApi;
using System.Numerics;


namespace LogicLayer
{
    
    public class Microphone : IMicrophone
    {
        public WaveIn wi;
        public BufferedWaveProvider bwp;
        public Int32 envelopeMax;

        private int RATE = 8000;
        //private int RATE = 1200;
        //private int BUFFERSIZE = (int)Math.Pow(2, 13);
        private int BUFFERSIZE = (int)Math.Pow(2, 15);
        private int devcount;

        public Microphone()
        {
            devcount = WaveIn.DeviceCount;

            wi = new WaveIn();
            wi.DeviceNumber = 0;
            wi.WaveFormat = new WaveFormat(RATE, 1);

            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.BufferLength = BUFFERSIZE * 2;

            bwp.DiscardOnBufferOverflow = true;
            wi.StartRecording();
        }

        void wi_DataAvailable(object sender, WaveInEventArgs e)
        {
            bwp.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }

        public void ChangeDevice(int deviceIndex)
        {
            wi.Dispose();
            wi = new WaveIn();
            wi.DeviceNumber = deviceIndex;
            wi.WaveFormat = new WaveFormat(RATE, 1);

            wi.DataAvailable += new EventHandler<WaveInEventArgs>(wi_DataAvailable);
            bwp = new BufferedWaveProvider(wi.WaveFormat);
            bwp.BufferLength = BUFFERSIZE * 2;

            bwp.DiscardOnBufferOverflow = true;
            wi.StartRecording();
        }

        public List<string> GetDevcount()
        {
            int waveInDevices = WaveIn.DeviceCount;
            List <string> devices = new List<string>();
            for (int waveInDevice = 0; waveInDevice < waveInDevices; waveInDevice++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(waveInDevice);
                devices.Add(deviceInfo.ProductName);
            }
            return devices;

        }

        public double[] GetFrequency()
        {
            // read the bytes from the stream
            int frameSize = BUFFERSIZE;
            var frames = new byte[frameSize];
            bwp.Read(frames, 0, frameSize);

            // convert it to int32 manually
            int SAMPLE_RESOLUTION = 16;
            int BYTES_PER_POINT = SAMPLE_RESOLUTION / 8;
            Int32[] vals = new Int32[frames.Length / BYTES_PER_POINT];
            double[] Ys = new double[frames.Length / BYTES_PER_POINT];
            double[] Ys2 = new double[frames.Length / BYTES_PER_POINT];
            for (int i = 0; i < vals.Length; i++)
            {
                // bit shift the byte buffer into the right variable format
                byte hByte = frames[i * 2 + 1];
                byte lByte = frames[i * 2 + 0];
                vals[i] = (int)(short)((hByte << 8) | lByte);
                Ys[i] = vals[i];
            }

            Ys2 = FFT(Ys);
            return Ys2;

        }

        public double[] FFT(double[] data)
        {
            double[] fft = new double[data.Length]; // this is where we will store the output (fft)
            Complex[] fftComplex = new Complex[data.Length]; // the FFT function requires complex format
            for (int i = 0; i < data.Length; i++)
            {
                fftComplex[i] = new Complex(data[i], 0.0); // make it complex format (imaginary = 0)
            }
            Accord.Math.FourierTransform.FFT(fftComplex, Accord.Math.FourierTransform.Direction.Forward);
            for (int i = 0; i < data.Length; i++)
            {
                if(i < 30)
                {
                    fft[i] = 0.0;
                } else
                {
                    fft[i] = fftComplex[i].Magnitude; // back to double
                }
                
            }
            return fft;
        }

    }
}
