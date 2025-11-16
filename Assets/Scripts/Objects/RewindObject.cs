using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RewindObject : MonoBehaviour
{

    [Header("Pannel Settings")]

    public string Name = "Name me please";
    public string Desc = "Descibe me please :(";

    public string PasueTime = "I will change on my own";
    public string RewindTime = "I will change on my own";

    private List<Vector3> _recordedPositions = new List<Vector3>();
    private List<quaternion> _recordedRotations = new List<quaternion>();

    [Header("Object Settings")]
    [SerializeField] private float _recordTimeSeconds = 10;

    [SerializeField] private float _pauseTimeSeconds = 5;

    [SerializeField] private float _pauseCooldown = 5;

    public bool IsRewinding;

    public bool IsPasued;

    private Timer _pauseTimer;
    private Timer _pauseCooldownTimer;


    private Rigidbody _objectRb;

    private int _maxIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._pauseTimer = new Timer();

        this._pauseCooldownTimer = new Timer();

        this._objectRb = gameObject.GetComponent<Rigidbody>();

        float fixedTimeInterval = Time.fixedDeltaTime;

        this._maxIndex = (int)math.ceil(this._recordTimeSeconds / fixedTimeInterval);

        BoundingBoxDrawer boxDrawer = this.gameObject.AddComponent<BoundingBoxDrawer>();
        boxDrawer._lineColour = Color.blue;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int lastIndex = this._recordedPositions.Count - 1;

        if(IsPasued)
        {
            if (this._pauseTimer.IsFinished)
            {
                SetPause(false);
                return;
            }

            return;
        }

        if (!IsRewinding)
        {

            if (this._recordedPositions.Count == 0 || transform.position != this._recordedPositions[lastIndex] || transform.rotation != this._recordedRotations[lastIndex])
            {

                this._objectRb.isKinematic = false;

                this._recordedPositions.Add(transform.position);
                this._recordedRotations.Add(transform.rotation);

                if (this._recordedPositions.Count >= this._maxIndex)
                {
                    this._recordedPositions.RemoveAt(0);
                    this._recordedRotations.RemoveAt(0);
                }

            }

        }

        else
        {
            this._objectRb.isKinematic = true;

            if (this._recordedPositions.Count == 0)
            {
                IsRewinding = false;
            }
            else
            {

                transform.position = this._recordedPositions[lastIndex];
                transform.rotation = this._recordedRotations[lastIndex];

                this._recordedPositions.RemoveAt(lastIndex);
                this._recordedRotations.RemoveAt(lastIndex);

            }

        }
    }

    void Update()
    {
        this._pauseTimer.Update();
        this._pauseCooldownTimer.Update();

        if (this._pauseTimer.IsActive)
        {
            this.PasueTime = this._pauseTimer.TimeRemaing.ToString("0") + "s";
        }
        else
        {
            this.PasueTime = this._pauseTimeSeconds.ToString("0") + "s";
        }

        if(_pauseCooldownTimer.IsActive)
        {
            float dotValue = this._pauseCooldown / 4;

            int numberOfDots = (int)Mathf.Floor(this._pauseCooldownTimer.TimePassed / dotValue);

            this.PasueTime = ".";

            for (int i = 0; i < numberOfDots; i++)
            {
                this.PasueTime += ".";
            }
        }


        float indexTime = this._maxIndex / this._recordTimeSeconds;

        this.RewindTime = ((float)this._recordedPositions.Count * Time.fixedDeltaTime).ToString("0") + "s";
    }

    public void SetRewind(bool state)
    {
        this.IsRewinding = state;

        if (state)
            this.IsPasued = false;
    }

    public void SetPause(bool state)
    {

        if (state)
        {
            if (!_pauseCooldownTimer.IsActive)
            {
                this._pauseTimer.StartTimer(this._pauseTimeSeconds);

                this._objectRb.isKinematic = true;

                this.IsRewinding = false;

                IsPasued = state;
            }
        }
        else
        {
            this._objectRb.isKinematic = false;
            this._pauseTimer.FinishTimer();

            this._pauseCooldownTimer.StartTimer(this._pauseCooldown);

            IsPasued = state;
        }
        
    }
}
