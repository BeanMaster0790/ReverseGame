using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SelectionObject : MonoBehaviour
{
    public Vector3? FirstRecordedPosition 
    {
        get
        {
            if(this._recordedPositions.Count > 0)
            {
                return this._recordedPositions[0];
            }

            return null;
        }
    }

    public quaternion? FirstRecordedRotation 
    {
        get
        {
            if(this._recordedRotations.Count > 0)
            {
                return this._recordedRotations[0];
            }

            return null;
        }
    }

    [Header("Pannel Settings")]

    public string Name = "Name me please";
    public string Desc = "Descibe me please :(";

    public string PasueTime = "I will change on my own";
    public string RewindTime = "I will change on my own";

    [SerializeField] private Color _outlineColour = Color.blue;

    private List<Vector3> _recordedPositions = new List<Vector3>();
    private List<quaternion> _recordedRotations = new List<quaternion>();

    [Header("Object Settings")]
    public float RecordTimeSeconds = 10;

    public float PauseTimeSeconds = 5;

    public float PauseCooldown = 5;

    public bool IsRewinding;

    public bool IsPasued;

    private Timer _pauseTimer;
    private Timer _pauseCooldownTimer;

    private Rigidbody _objectRb;

    private bool _hasRb;

    private int _maxIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._pauseTimer = new Timer();

        this._pauseCooldownTimer = new Timer();

        this._objectRb = gameObject.GetComponent<Rigidbody>();

        if (this._objectRb != null)
            this._hasRb = true;

        float fixedTimeInterval = Time.fixedDeltaTime;

        this._maxIndex = (int)math.ceil(this.RecordTimeSeconds / fixedTimeInterval);

        BoundingBoxDrawer boxDrawer = this.gameObject.AddComponent<BoundingBoxDrawer>();
        boxDrawer._lineColour = this._outlineColour;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!this._hasRb)
            return;

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

        if(!this._hasRb)
        {
            this.RewindTime = "N/A";
            this.PasueTime = "N/A";
            return;
        }

        this._pauseTimer.Update();
        this._pauseCooldownTimer.Update();

        if (this._pauseTimer.IsActive)
        {
            this.PasueTime = this._pauseTimer.TimeRemaing.ToString("0") + "s";
        }
        else
        {
            this.PasueTime = this.PauseTimeSeconds.ToString("0") + "s";
        }

        if(_pauseCooldownTimer.IsActive)
        {
            float dotValue = this.PauseCooldown / 4;

            int numberOfDots = (int)Mathf.Floor(this._pauseCooldownTimer.TimePassed / dotValue);

            this.PasueTime = ".";

            for (int i = 0; i < numberOfDots; i++)
            {
                this.PasueTime += ".";
            }
        }

        this.RewindTime = ((float)this._recordedPositions.Count * Time.fixedDeltaTime).ToString("0") + "s";

        if(this.RecordTimeSeconds == 0)
        {
            this.RewindTime = "N/A";
        }

        if(this.PauseTimeSeconds == 0)
        {
            this.PasueTime = "N/A";
        }
    }

    public void SetRewind(bool state)
    {
        if (!this._hasRb || this.RecordTimeSeconds == 0)
            return;

        this.IsRewinding = state;

        PlayerSoundsManager.Current.PlaySound("Rewind");

        if (state)
            this.IsPasued = false;
    }

    public void SetPause(bool state)
    {

        if (!this._hasRb || this.PauseTimeSeconds == 0)
            return;

        if (state)
        {
            if (!_pauseCooldownTimer.IsActive)
            {
                this._pauseTimer.StartTimer(this.PauseTimeSeconds);

                this._objectRb.isKinematic = true;

                this.IsRewinding = false;

                IsPasued = state;
            }
        }
        else
        {
            this._objectRb.isKinematic = false;
            this._pauseTimer.FinishTimer();

            this._pauseCooldownTimer.StartTimer(this.PauseCooldown);

            IsPasued = state;
        }
        
    }
}
