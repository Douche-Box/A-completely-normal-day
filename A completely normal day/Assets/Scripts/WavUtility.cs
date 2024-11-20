using UnityEngine;
using System.IO;
using System;

public static class WavUtility
{
    // Convert an AudioClip to a WAV byte array
    public static byte[] FromAudioClip(AudioClip clip)
    {
        // Get the audio samples from the AudioClip
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        // Convert the float samples to 16-bit PCM samples
        short[] pcmSamples = new short[samples.Length];
        for (int i = 0; i < samples.Length; i++)
        {
            pcmSamples[i] = (short)(samples[i] * short.MaxValue);  // Convert float to 16-bit PCM
        }

        // Write the WAV header and PCM data to a memory stream
        using (MemoryStream memoryStream = new MemoryStream())
        {
            WriteWavHeader(memoryStream, clip.channels, clip.frequency, pcmSamples.Length);
            WritePcmData(memoryStream, pcmSamples);

            return memoryStream.ToArray();  // Return the byte array containing the WAV data
        }
    }

    // Write the WAV header to the stream
    private static void WriteWavHeader(MemoryStream stream, int channels, int sampleRate, int dataSize)
    {
        int headerSize = 44;
        int byteRate = sampleRate * channels * 2;
        int blockAlign = channels * 2;

        // RIFF header
        stream.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"), 0, 4);
        stream.Write(BitConverter.GetBytes(36 + dataSize), 0, 4); // Total file size (header size + data)
        stream.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"), 0, 4);

        // fmt header
        stream.Write(System.Text.Encoding.ASCII.GetBytes("fmt "), 0, 4);
        stream.Write(BitConverter.GetBytes(16), 0, 4); // Size of fmt chunk
        stream.Write(BitConverter.GetBytes((short)1), 0, 2); // Audio format (1 = PCM)
        stream.Write(BitConverter.GetBytes((short)channels), 0, 2); // Number of channels
        stream.Write(BitConverter.GetBytes(sampleRate), 0, 4); // Sample rate
        stream.Write(BitConverter.GetBytes(byteRate), 0, 4); // Byte rate
        stream.Write(BitConverter.GetBytes((short)blockAlign), 0, 2); // Block align
        stream.Write(BitConverter.GetBytes((short)16), 0, 2); // Bits per sample (16-bit PCM)

        // data header
        stream.Write(System.Text.Encoding.ASCII.GetBytes("data"), 0, 4);
        stream.Write(BitConverter.GetBytes(dataSize), 0, 4); // Data size
    }

    // Write PCM data to the stream
    private static void WritePcmData(MemoryStream stream, short[] pcmSamples)
    {
        byte[] byteArray = new byte[pcmSamples.Length * 2];
        Buffer.BlockCopy(pcmSamples, 0, byteArray, 0, byteArray.Length);
        stream.Write(byteArray, 0, byteArray.Length); // Write PCM data to stream
    }
}
