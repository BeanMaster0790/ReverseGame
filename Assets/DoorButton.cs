using System.Net.Cache;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    [SerializeField] private string _doorID;

    [SerializeField] private float _lowerBy;
    [SerializeField] private float _timeToLower;


    [HideInInspector] public Door ButtonDoor;


    private bool _isPressed;

    private Vector3 _startPosition;
    private Vector3 _endPosition;

    private float _currentTime = 0;

    private int _objectsOnButton;



    void Start()
    {
        this._startPosition = this.transform.position;

        this._endPosition = this.transform.position;

        this._endPosition.y -= this._lowerBy;

        BoundingBoxDrawer boxDrawer = this.transform.parent.gameObject.AddComponent<BoundingBoxDrawer>();
        boxDrawer._lineColour = Color.yellow;
    }

    void Update()
    {
        if (this._isPressed)
        {
            float t = this._currentTime / this._timeToLower;

            this.transform.parent.position = Vector3.Lerp(this._startPosition, this._endPosition, t);

            this._currentTime += Time.deltaTime;
        }
        else
        {
            float t = this._currentTime / this._timeToLower;

            this.transform.parent.position = Vector3.Lerp(this._endPosition, this._startPosition, t);

            this._currentTime += Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider collision)
    {

        this._objectsOnButton += 1;

        if(!this._isPressed && this._objectsOnButton == 1)
        {
            this._isPressed = true;

            this._currentTime = 0;

            this.ButtonDoor.ButtonStateChange(ButtonState.Pressed);
        }

    }

    void OnTriggerExit(Collider collision)
    {

        this._objectsOnButton -= 1;

        if (this._isPressed && this._objectsOnButton == 0)
        {
            this._isPressed = false;

            this._currentTime = 0;

            this.ButtonDoor.ButtonStateChange(ButtonState.Relaesed);
        }
    }
}
