namespace ouchi
{
    public class RMSMeter :ILevelMeter
    {
        public RMSMeter(int samplerate, Time bufferLength) {
            sampleRate = samplerate;
            buffer_ = new RingBuffer<float>(bufferLength.CastTime(audioClockUnit).value);
            linearRMS = 0;
            reference = 1;
        }

        public RMSMeter(int samplerate)
            : this(samplerate, new Time(300, Time.units.milliseconds))
        { }

        public void Feed(float[] samples, Time deltaTime)
        {
            deltaTime = deltaTime.CastTime(audioClockUnit);
            deltaTime.value = System.Math.Min(maxAcceptableSampleCount, deltaTime.value);
            // samplesのうち破棄する数
            int discard = System.Math.Max(0, samples.Length - deltaTime.value);
            float sum = 0;
            for(int i = System.Math.Max(0, buffer_.size - (maxAcceptableSampleCount - deltaTime.value)); i < buffer_.size; ++i)
            {
                sum += buffer_[i];
            }
            for(int i = discard; i < samples.Length; ++i)
            {
                float sq = samples[i] * samples[i];
                buffer_.Add(sq);
                sum += sq;
            }
            linearRMS = (float)System.Math.Sqrt(sum / maxAcceptableSampleCount);
        }

        public void Feed(float[] samples, float deltaTimeSec)
        {
            Feed(samples, new Time((int)(deltaTimeSec * sampleRate), audioClockUnit));
        }

        public void Feed(float[] samples)
        {
            Feed(samples, new Time(samples.Length, audioClockUnit));
        }
        public float level_dB { get => 10 * (float)System.Math.Log10(linearRMS / reference); } 

        public float level_dBSPL { get => 20 * (float)System.Math.Log10(linearRMS / reference); }
        public float RMS_Linear { get => linearRMS; }
        public Ratio audioClockUnit { get => new Ratio(1, sampleRate); }
        /// <summary>
        /// このサイズを超えて入力されたサンプルは無視される．
        /// </summary>
        public int maxAcceptableSampleCount { get { return buffer_.capacity; } set { bufferLength = new Time(value, audioClockUnit); } }
        /// <summary>
        /// デシベル計算の基準量
        /// </summary>
        public float reference;
        public int sampleRate { 
            get { return samplerate_; }
            set { samplerate_ = value; }
        }
        public Time bufferLength
        {
            get { return new Time(buffer_.capacity, audioClockUnit); }
            set {
                value = value.CastTime(audioClockUnit);
                buffer_.capacity = value.value;
            }
        }
        private float linearRMS;
        private int samplerate_;
        private RingBuffer<float> buffer_;
    }
}
