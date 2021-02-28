using System;
using NAudio.Wave;
using System.IO;


namespace Chat_Monkeyz
{

    public class Sound
    {
        WaveIn waveIn;
        WaveOut waveOut;

        BufferedWaveProvider buffwave;

        public delegate void DataReceivedDelegate(byte[] buffer);
        public event DataReceivedDelegate DataReceived;

        public bool ready = false;

        public Sound()
        {

        }

        public void Init()
        {
            int deviceNumber = 0;
            
            waveIn = new WaveIn();
            waveIn.BufferMilliseconds = 25;
            waveIn.DeviceNumber = deviceNumber;
            waveIn.DataAvailable += waveIn_DataAvailable;
            waveIn.WaveFormat = new WaveFormat(44100, 1);

            buffwave = new BufferedWaveProvider(waveIn.WaveFormat);
            buffwave.DiscardOnBufferOverflow = true;

            waveOut = new WaveOut();
            waveOut.DesiredLatency = 100;
            waveOut.Init(buffwave);

            ready = true;
        }

        void waveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            DataReceived(e.Buffer);
            //looping
            Play(e.Buffer);
        }

        
        public void Play(byte[] buffer)
        {
            buffwave.AddSamples(buffer, 0, buffer.Length);
        }



        public void StartRecord()
        {
            waveIn.StartRecording();
            waveOut.Play();
        }

        public void StopRecord()
        {
            waveIn.StopRecording();
            waveOut.Stop();
        }

    }
}