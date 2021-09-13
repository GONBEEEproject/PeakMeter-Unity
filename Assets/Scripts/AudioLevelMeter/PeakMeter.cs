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
        /// ���[�^�[�ɔg�`�f�[�^����͂��C���[�^�[���w���l���X�V����D
        /// </summary>
        /// <param name="samples">samples�̍Ō�̗v�f���ŐV�̔g�`�W�{�ɂȂ�悤�Ȕz��</param>
        /// <param name="deltaTime">�O��̓��͂���o�߂�������</param>
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
        /// ���[�^�[�ɔg�`�f�[�^����͂��C���[�^�[���w���l���X�V����D
        /// </summary>
        /// <param name="samples">samples�̍Ō�̗v�f���ŐV�̔g�`�W�{�ɂȂ�悤�Ȕz��</param>
        /// <param name="deltaTime">�O��̓��͂���o�߂�������[�b]</param>
        public void Feed(float[] samples, float deltaTimeSec)
        {
            Feed(samples, new Time((int)(deltaTimeSec * sampleRate), audioClockUnit));
        }
        /// <summary>
        /// ���[�^�[�ɔg�`�f�[�^����͂��C���[�^�[���w���l���X�V����Dsamples�̕W�{���ɑΉ����镪�������Ԃ��o�������̂Ƃ���D
        /// </summary>
        /// <param name="samples">samples�̍Ō�̗v�f���ŐV�̔g�`�W�{�ɂȂ�悤�Ȕz��</param>
        public void Feed(float[] samples)
        {
            Feed(samples, new Time(samples.Length, audioClockUnit));
        }
        public float peakLevelLinear { get { return linearPeak_ * (float)System.Math.Pow(linearReleaseRatePerClock_, peakTime_.value); } }
        public float level_dB { get { return (float)(10 * System.Math.Log10(peakLevelLinear / reference)); } }
        public float level_dBSPL { get { return (float)(20 * System.Math.Log10(peakLevelLinear / reference)); } }
        /// <summary>
        /// ���͂��Ȃ������ꍇ�s�[�N�l��1�b�ɉ�dB�����邩�K�肷��l�D0�����̒l�łȂ���΂Ȃ�Ȃ��D
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
        /// ���̃T�C�Y�𒴂��ē��͂��ꂽ�T���v���͖��������D
        /// </summary>
        public int maxAcceptableSampleCount { get; set; }
        /// <summary>
        /// �f�V�x���v�Z�̊��
        /// </summary>
        public float reference;
        public int sampleRate { 
            private get { return samplerate_; }
            set { samplerate_ = value; } }

        private float linearPeak_;
        private Time peakTime_;    // ���Βl
        private double linearReleaseRatePerClock_;
        private int samplerate_;
    }
}
