
namespace ouchi
{
    public interface ILevelMeter
    {
        public float level_dB { get; }
        public float level_dBSPL { get; }
        public int sampleRate { set; }
        public void Feed(float[] samples, Time deltaTime);
        public void Feed(float[] samples, float deltaTimeSec);
        public void Feed(float[] samples);
    }
}
