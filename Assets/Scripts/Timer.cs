using UnityEngine;

public class Timer
{
    private float _duration;

    public float TimePassed;

    public float TimeRemaing;

    public bool IsFinished = true;

    public bool IsActive;

    public void StartTimer(float duration)
    {
        this.IsFinished = false;

        this._duration = duration;

        this.TimePassed = 0;

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
            this.TimePassed += Time.deltaTime;

            if (this.TimePassed >= this._duration)
            {
                FinishTimer();
            }
        }

        this.TimeRemaing = this._duration - this.TimePassed;
    }
}