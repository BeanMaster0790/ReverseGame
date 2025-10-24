using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectController : MonoBehaviour
{
    public Transform CameraTransform;

    public LayerMask objectLayer;

    [SerializeField] private RewindObject _selectedRewindObject = null;

    [SerializeField] private bool _isRewinding;

    [SerializeField] private bool _isPausing;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camForward = CameraTransform.forward;
        camForward.Normalize();

        Vector3 camRight = CameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        if (this._isRewinding)
        {
            this._isRewinding = this._selectedRewindObject.IsRewinding;

            return;
        }
        
        if(this._isPausing)
        {
            this._isPausing = this._selectedRewindObject.IsPasued;

            return;
        }

        RaycastHit hit;
        float maxDistance = 500f;

        if (Physics.Raycast(CameraTransform.position, camForward, out hit, maxDistance, objectLayer))
        {
            // hit now contains the hit info
            Debug.Log($"Hit {hit.collider.name} at {hit.point}");

            this._selectedRewindObject = hit.collider.GetComponent<RewindObject>();
        }
        else
        {
            this._selectedRewindObject = null;
        }
    }

    public void OnRewind(InputAction.CallbackContext context)
    {
        if (context.performed && this._selectedRewindObject != null)
        {
            if (this._isRewinding)
            {
                this._selectedRewindObject.SetRewind(false);
            }
            else
            {
                this._isRewinding = true;
                this._isPausing = false;
                this._selectedRewindObject.SetRewind(true);
            }
        }
    }
    
    public void OnPasue(InputAction.CallbackContext context)
    {
        if(context.performed && this._selectedRewindObject != null)
        {
            if (this._isPausing)
            {
                this._selectedRewindObject.SetPause(false);
            }
            else
            {
                this._isPausing = true;
                this._isRewinding = false;
                this._selectedRewindObject.SetPause(true);
            }
        }
    }
}
