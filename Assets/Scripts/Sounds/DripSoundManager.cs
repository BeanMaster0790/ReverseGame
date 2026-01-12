using UnityEngine;

public class DripSoundManager : MonoBehaviour
{
    private Timer _timer;

    private float _playIn;

    private AudioSource _source;

    void Start()
    {
        this._timer = new Timer();

        this._playIn = 1 / this.gameObject.GetComponent<ParticleSystem>().emission.rateOverTime.constant;

        this._timer.StartTimer(this._playIn);

        this._source = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        this._timer.Update();

        if(this._timer.IsFinished)
        {
            this._source.Play();
            this._timer.StartTimer(this._playIn);
        }
    }
}
