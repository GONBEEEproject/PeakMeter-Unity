using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ouchi
{
    public class AudioLevelMeter
    {
        public AudioLevelMeter(ILevelMeter meter, int channel, int bufferSize = 1024)
        {
            meter_ = meter;
            meter.maxAcceptableSampleCount = bufferSize;
            buffer_ = new float[1024];
            channel_ = channel;
        }

        public float GetLevel_dBSPL()
        {
            Feed();
            return meter_.level_dBSPL;
        }
        public float GetLevel_dB()
        {
            Feed();
            return meter_.level_dB;
        }

        private void Feed()
        {
            AudioListener.GetOutputData(buffer_, channel_);
            meter_.Feed(buffer_, UnityEngine.Time.deltaTime);
        }


        public ILevelMeter meter_;
        private float[] buffer_;
        private int channel_;

    }
}
