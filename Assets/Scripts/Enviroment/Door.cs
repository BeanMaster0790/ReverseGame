using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private DoorButton[] _doorButtons;

    private int _buttonsPressed;

    [SerializeField] private Transform _openPositionTransform;

    private Vector3 _openPosition;
    private Vector3 _startPosition;

    private Vector3 _buttonChangePoint;

    [SerializeField] private float _timeToOpen;

    private float _currentTime;


    void Start()
    {
        this._startPosition = this.transform.position;
        this._openPosition = this._openPositionTransform.position;

        foreach (DoorButton button in this._doorButtons)
        {
            button.ButtonDoor = this;
        }

        BoundingBoxDrawer boxDrawer = this.gameObject.AddComponent<BoundingBoxDrawer>();
        boxDrawer._lineColour = Color.green;
    }

    void Update()
    {
        if (this._buttonsPressed == this._doorButtons.Length)
        {
            float t = this._currentTime / this._timeToOpen;

            this.transform.position = Vector3.Lerp(this._buttonChangePoint, this._openPosition, t);

            this._currentTime += Time.deltaTime;
        }
        else
        {
            float t = this._currentTime / this._timeToOpen;

            this.transform.position = Vector3.Lerp(this._buttonChangePoint, this._startPosition, t);

            this._currentTime += Time.deltaTime;
        }
    }

    public void ButtonStateChange(ButtonState state)
    {
        if (state == ButtonState.Pressed)
        {
            this._buttonsPressed += 1;

            this._buttonChangePoint = this.transform.position;

            float maxDistanceBetween = Vector3.Distance(this._startPosition, this._openPosition);
            float currentDistanceBetween = Vector3.Distance(this._buttonChangePoint, this._openPosition);

            float percentDifference = currentDistanceBetween / maxDistanceBetween;

            this._currentTime = this._timeToOpen - (this._timeToOpen * percentDifference)  - Time.deltaTime;

            Debug.Log($"{maxDistanceBetween} {currentDistanceBetween} {percentDifference} {_currentTime}");

        }

        else if (state == ButtonState.Relaesed)
        {
            this._buttonsPressed -= 1;

            this._buttonChangePoint = this.transform.position;

            float maxDistanceBetween = Vector3.Distance(this._startPosition, this._openPosition);
            float currentDistanceBetween = Vector3.Distance(this._buttonChangePoint, this._startPosition);

            float percentDifference = currentDistanceBetween / maxDistanceBetween;

            this._currentTime = this._timeToOpen - (this._timeToOpen * percentDifference) - Time.deltaTime;

            Debug.Log($"{maxDistanceBetween} {currentDistanceBetween} {percentDifference} {_currentTime}");

        }
    }
}
