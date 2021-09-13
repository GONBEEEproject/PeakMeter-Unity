using System.Collections;
using System.Collections.Generic;

namespace ouchi
{
    public class PeakMeter : ILevelMeter
    {
        public PeakMeter(int samplerate, int releaserate = -24)
        {
            sampleRate = samplerate;
            peakTime_ = new Time(1, audioClockUnit);
            reference = 1f;
            releaseRate = releaserate;
            linearPeak_ = 0;
            maxAcceptableSampleCount = 1024;
        }
        /// <summary>
        /// メーターに波形データを入力し，メーターが指す値を更新する．
        /// </summary>
        /// <param name="samples">samplesの最後の要素が最新の波形標本になるような配列</param>
        /// <param name="deltaTime">前回の入力から経過した時間</param>
        public void Feed(float[] samples, Time deltaTime)
        {
            deltaTime = deltaTime.CastTime(audioClockUnit);
            if(deltaTime.value - samples.Length > 0)
                peakTime_.value += deltaTime.value - samples.Length;
            int size = System.Math.Min(System.Math.Min(deltaTime.value, maxAcceptableSampleCount), samples.Length);
            double peak = linearPeak_ * (float)System.Math.Pow(linearReleaseRatePerClock_, peakTime_.value);
            for(int i = 0; i < size ; ++i, peak *= linearReleaseRatePerClock_)
            {
                var sample = System.Math.Abs(samples[i]);
                if (sample > peak)
                {
                    peak = linearPeak_ = sample;
                    peakTime_.value = size - i;
                }
                else peakTime_.value++;
            }
        }
        /// <summary>
        /// メーターに波形データを入力し，メーターが指す値を更新する．
        /// </summary>
        /// <param name="samples">samplesの最後の要素が最新の波形標本になるような配列</param>
        /// <param name="deltaTime">前回の入力から経過した時間[秒]</param>
        public void Feed(float[] samples, float deltaTimeSec)
        {
            Feed(samples, new Time((int)(deltaTimeSec * sampleRate), audioClockUnit));
        }
        /// <summary>
        /// メーターに波形データを入力し，メーターが指す値を更新する．samplesの標本数に対応する分だけ時間が経ったものとする．
        /// </summary>
        /// <param name="samples">samplesの最後の要素が最新の波形標本になるような配列</param>
        public void Feed(float[] samples)
        {
            Feed(samples, new Time(samples.Length, audioClockUnit));
        }
        public float peakLevelLinear { get { return linearPeak_ * (float)System.Math.Pow(linearReleaseRatePerClock_, peakTime_.value); } }
        public float level_dB { get { return (float)(10 * System.Math.Log10(peakLevelLinear / reference)); } }
        public float level_dBSPL { get { return (float)(20 * System.Math.Log10(peakLevelLinear / reference)); } }
        /// <summary>
        /// 入力がなかった場合ピーク値が1秒に何dB下がるか規定する値．0未満の値でなければならない．
        /// </summary>
        public float releaseRate {
            set {
                linearReleaseRatePerClock_ = System.Math.Pow(10, value / 10.0 / sampleRate);
            }
            get
            {
                return (float)(10 * System.Math.Log10(linearReleaseRatePerClock_) * sampleRate);
            }
        }
        public Ratio audioClockUnit { get { return new Ratio(1, sampleRate); } }
        /// <summary>
        /// このサイズを超えて入力されたサンプルは無視される．
        /// </summary>
        public int maxAcceptableSampleCount { get; set; }
        /// <summary>
        /// デシベル計算の基準量
        /// </summary>
        public float reference;
        public int sampleRate { 
            private get { return samplerate_; }
            set { samplerate_ = value; } }

        private float linearPeak_;
        private Time peakTime_;    // 相対値
        private double linearReleaseRatePerClock_;
        private int samplerate_;
    }
}
