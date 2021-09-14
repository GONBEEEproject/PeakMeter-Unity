using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLevelMeterTest : MonoBehaviour
{
    ouchi.ILevelMeter[] meters;    // Start is called before the first frame update
    void Start()
    {
        meters = new ouchi.ILevelMeter[] {
            new ouchi.RMSMeter(AudioSettings.outputSampleRate, new ouchi.Time(300, ouchi.Time.units.milliseconds)),
            new ouchi.PeakMeter(AudioSettings.outputSampleRate)
        };

        //ale = new ouchi.AudioLevelMeter(new ouchi.PeakMeter(AudioSettings.outputSampleRate), channel);
        ale = new ouchi.AudioLevelMeter(meters[meter], channel);
        transform.localScale = new Vector3(1, 0.125f, 1);
        if(meter == 1)
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        db = ale.GetLevel_dB();
        transform.localPosition = new Vector3(1, (-mindb + System.Math.Max(db, mindb)) / -mindb * 10, 1f);
        Debug.Log("Update AudioLevelMeter");
    }
    ouchi.AudioLevelMeter ale;

    [SerializeField]
    float db;
    [SerializeField]
    int channel;
    [SerializeField]
    float mindb = -60;
    [SerializeField]
    int meter;
}
