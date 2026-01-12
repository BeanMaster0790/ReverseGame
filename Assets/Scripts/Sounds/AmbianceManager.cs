using UnityEngine;

public class AmbianceManager : MonoBehaviour
{
    private float _startVoloume;

    private AudioSource _audioSource;


    void Start()
    {
        this._audioSource = this.gameObject.GetComponent<AudioSource>();

        this._startVoloume = this._audioSource.volume;

        this._audioSource.volume = 0;

        ObjectController.S_ControlModeToggle += (object sender, ControlModeEvent modeEvent) =>
        {
            if(modeEvent.State)
                this._audioSource.volume = this._startVoloume;
            else
                this._audioSource.volume = 0;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
