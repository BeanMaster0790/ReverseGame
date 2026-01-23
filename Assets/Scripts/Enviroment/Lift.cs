using UnityEngine;

public class Lift : MonoBehaviour
{
    private Rigidbody _liftRb;

    [SerializeField] private Transform _goToPosition;

    [SerializeField] private float _timeToTravel;

    private bool _isActive;

    private bool _atTarget;

    private Vector3 _startPos;

    private Vector3 _activatePoint;

    [SerializeField] private Vector3 _currentTarget;

    private float _currentTime;

    [SerializeField] private bool _needReactivationByObject;


    void Start()
    {
        this._liftRb = this.GetComponent<Rigidbody>();

        this._startPos = this.transform.position;
        this._activatePoint = this._startPos;

        this._currentTarget = this._goToPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(this._liftRb.isKinematic)
            return;

        float t = this._currentTime / this._timeToTravel;

        if(this._isActive)
        {
            this._liftRb.MovePosition(Vector3.Lerp(this._activatePoint, this._currentTarget, t));
        }
    
        this._currentTime += Time.deltaTime;

        if(Vector3.Distance(this.transform.position, this._currentTarget) < 0.1f && this._isActive)
        {

            if(this._currentTarget == this._startPos)
            {
                this._currentTarget = this._goToPosition.position;
                this._atTarget = false;
            }
            else
            {
                this._atTarget = true;
                this._currentTarget = this._startPos;
            }

            if(!this._needReactivationByObject)
            {
                if(!this._atTarget)
                    this._isActive = false;
                else
                    this._isActive = true;
            }
            else
                this._isActive = false;
        }

    }

    void OnTriggerEnter(Collider collision)
    {
        if((collision.tag == "Player" || collision.tag == "Enemy") && !this._isActive)
        {
            this._isActive = true;

            this._activatePoint = this.transform.position;

            this._currentTime = 0;
        }
    }
}
