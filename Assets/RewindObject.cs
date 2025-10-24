using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RewindObject : MonoBehaviour
{
    private List<Vector3> _recordedPositions = new List<Vector3>();
    private List<quaternion> _recordedRotations = new List<quaternion>();

    [SerializeField] private float _recordTimeSeconds = 10;

    [SerializeField] private float _pauseTimeSeconds = 5;

    public bool IsRewinding;

    public bool IsPasued;

    private Timer _pauseTimer;

    private Rigidbody _objectRb;

    private int _maxIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this._pauseTimer = new Timer();

        this._objectRb = gameObject.GetComponent<Rigidbody>();

        float fixedTimeInterval = Time.fixedDeltaTime;

        this._maxIndex = (int)math.ceil(this._recordTimeSeconds / fixedTimeInterval);
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
    }

    public void SetRewind(bool state)
    {
        this.IsRewinding = state;

        if (state)
            this.IsPasued = false;
    }

    public void SetPause(bool state)
    {
        IsPasued = state;

        if (state)
        {
            this._pauseTimer.StartTimer(this._pauseTimeSeconds);

            this._objectRb.isKinematic = true;

            this.IsRewinding = false;
        }
        else
        {
            this._objectRb.isKinematic = false;
        }
    }
}
