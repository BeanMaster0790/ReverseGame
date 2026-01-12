using UnityEngine;

public class Lift : MonoBehaviour
{
    private Rigidbody _liftRb;

    [SerializeField] private Transform _goToPosition;

    [SerializeField] private float _timeToTravel;

    private bool _isActive;

    private Vector3 _startPos;

    [SerializeField] private Vector3 _currentTarget;

    private float _currentTime;

    [SerializeField] private bool _stickToTarget;


    void Start()
    {
        this._liftRb = this.GetComponent<Rigidbody>();

        this._startPos = this.transform.position;

        this._currentTarget = this._startPos;
    }

    // Update is called once per frame
    void Update()
    {

        if(this._liftRb.isKinematic)
            return;

        float t = this._currentTime / this._timeToTravel;

        if(this._isActive)
        {
            this._liftRb.MovePosition(Vector3.Lerp(this._startPos, this._goToPosition.position, t));
        }
        else
        {
            this._liftRb.MovePosition(Vector3.Lerp(this._goToPosition.position, this._startPos, t));
        }

        this._currentTime += Time.deltaTime;

        if(Vector3.Distance(this.transform.position, this._goToPosition.position) < 0.1f && this._isActive && !this._stickToTarget)
        {
            this._isActive = false;
            this._currentTime = 0f;
        }

    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Player" && !this._isActive && Vector3.Distance(this.transform.position, this._startPos) < 0.1f)
        {
            this._isActive = true;
            this._currentTime = 0f;
        }
    }
}
