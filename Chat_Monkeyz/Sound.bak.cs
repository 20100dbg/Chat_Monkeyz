using System;
using NAudio.Wave;
using System.IO;


namespace Chat_Monkeyz
{

    public class Sound
    {

        WaveInEvent sourceStream;
        DirectSoundOut waveOut;
        
        public delegate void DataReceivedDelegate(byte[] buffer);
        public event DataReceivedDelegate DataReceived;

        public bool ready = false;

        public Sound()
        {

        }

        public void Init()
        {
            int deviceNumber = 0;

            sourceStream = new WaveInEvent();
            sourceStream.DeviceNumber = deviceNumber;

            int channels = WaveIn.GetCapabilities(deviceNumber).Channels;
            if (channels < 1) channels = 2;

            sourceStream.WaveFormat = new WaveFormat(44100, channels);

            //waveIn = new WaveInProvider(sourceStream);
            waveOut = new DirectSoundOut();
            //waveOut.Init(waveIn);

            sourceStream.DataAvailable += sourceStream_DataAvailable;

            ready = true;
        }

        void sourceStream_DataAvailable(object sender, WaveInEventArgs e)
        {
            DataReceived(e.Buffer);
        }


        public void Play(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            WaveStream ws = new WaveFileReader(ms);
            waveOut.Init(ws);
            waveOut.Play();
        }



        public void StartRecord()
        {
            sourceStream.StartRecording();
            //waveOut.Play();
        }

        public void StopRecord()
        {
            sourceStream.StopRecording();
            //waveOut.Stop();
        }

    }
}