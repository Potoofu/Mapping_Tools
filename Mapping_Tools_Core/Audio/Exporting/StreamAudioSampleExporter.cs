﻿using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mapping_Tools_Core.Audio.Exporting {
    public abstract class StreamAudioSampleExporter : IStreamAudioSampleExporter {
        public Stream OutStream { get; set; }

        protected Stack<ISampleProvider> sampleProviders;

        protected StreamAudioSampleExporter() {
            sampleProviders = new Stack<ISampleProvider>();
        }

        protected StreamAudioSampleExporter(Stream outStream) : this() {
            OutStream = outStream;
        }

        public bool Flush() {
            var numTracks = sampleProviders.Count;

            if (numTracks == 0) return false;

            // If it has only one valid sample, we can skip the mixing part
            if (numTracks == 1) { 
                return ExportSampleProvider(sampleProviders.Pop(), numTracks);
            }

            // Synchronize the sample rate and channels for all samples and get the sample providers
            int maxSampleRate = sampleProviders.Max(o => o.WaveFormat.SampleRate);
            int maxChannels = sampleProviders.Max(o => o.WaveFormat.Channels);

            IEnumerable<ISampleProvider> sameFormatSamples = sampleProviders.Select(o =>
                (ISampleProvider)new WdlResamplingSampleProvider(Helpers.SetChannels(o, maxChannels), maxSampleRate));

            ISampleProvider sampleProvider = new MixingSampleProvider(sameFormatSamples);

            return ExportSampleProvider(sampleProvider, numTracks);
        }

        /// <summary>
        /// Exports the sample provider to <see cref="OutStream"/>.
        /// </summary>
        /// <param name="sampleProvider">The audio to export</param>
        /// <param name="numTracks">The number of tracks mixed into the sample provider</param>
        /// <returns>Whether the export was a success</returns>
        protected abstract bool ExportSampleProvider(ISampleProvider sampleProvider, int numTracks);

        public void AddAudio(ISampleProvider sample) {
            sampleProviders.Push(sample);
        }

        public ISampleProvider PopAudio() {
            return sampleProviders.Pop();
        }

        public abstract string GetDesiredExtension();

        public abstract WaveFormatEncoding? GetDesiredWaveEncoding();
    }
}