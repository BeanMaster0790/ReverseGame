using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 1000;

        ObjectController.S_ControlModeToggle += (object sender, ControlModeEvent modeEvent) =>
        {
            if(modeEvent.State)
                this.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 1000;
            else
                this.gameObject.GetComponent<AudioLowPassFilter>().cutoffFrequency = 22000;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
