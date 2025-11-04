using UnityEngine;

public class Timer
{
    private float _duration;

    private float _timePassed;

    public bool IsFinished;

    public bool IsActive;

    public void StartTimer(float duration)
    {
        this.IsFinished = false;

        this._duration = duration;

        this._timePassed = 0;

        this.IsActive = true;
    }

    public void PauseTimer()
    {
        this.IsActive = false;
    }

    public void FinishTimer()
    {
        this.IsFinished = true;

        this.IsActive = false;
    }

    public void Update()
    {
        if (this.IsActive)
        {
            this._timePassed += Time.deltaTime;

            if (this._timePassed >= this._duration)
            {
                FinishTimer();
            }
        }
    }
}