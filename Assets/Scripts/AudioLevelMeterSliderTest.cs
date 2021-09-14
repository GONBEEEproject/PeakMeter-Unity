using ouchi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioLevelMeterSliderTest : MonoBehaviour
{

    [SerializeField]
    private Slider meter;

    private AudioLevelMeter ale;

    [SerializeField]
    private float db_;
    [SerializeField]
    private int channel;

    // Start is called before the first frame update
    void Start()
    {
        ale = new AudioLevelMeter(new PeakMeter(AudioSettings.outputSampleRate, -74), channel);
        //ale = new AudioLevelMeter(new ouchi.RMSMeter(AudioSettings.outputSampleRate), 0);
    }

    // Update is called once per frame
    void Update()
    {
        db_ = ale.GetLevel_dB();
        meter.value = db_;

        Debug.Log(db_);
    }
}
