using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLevelMeterTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ale = new ouchi.AudioLevelMeter(new ouchi.PeakMeter(AudioSettings.outputSampleRate), channel);
        transform.localScale = new Vector3(1, 0.125f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        db_ = ale.GetLevel_dB();
        transform.localPosition = new Vector3(1, 96 + System.Math.Max(db_, -96.0f), 1f) / 16;
        Debug.Log("Update AudioLevelMeter");
    }
    ouchi.AudioLevelMeter ale;

    [SerializeField]
    float db_;
    [SerializeField]
    int channel;
}
