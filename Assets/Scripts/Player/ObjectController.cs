using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectController : MonoBehaviour
{
    public Transform CameraTransform;

    public LayerMask objectLayer;

    [SerializeField] private SelectionObject _selectedRewindObject = null;

    [SerializeField] private bool _isRewinding;

    [SerializeField] private bool _isPausing;

    [SerializeField] private bool _controlMode;

    public static EventHandler<ControlModeEvent> S_ControlModeToggle;
    public static EventHandler<ObjectSelectEvent> S_ObjectSelect;

    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!this._controlMode)
            return;

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
            this._selectedRewindObject = hit.collider.GetComponent<SelectionObject>();

            S_ObjectSelect?.Invoke(this, new ObjectSelectEvent(this._selectedRewindObject));
        }
        else
        {
            this._selectedRewindObject = null;
            S_ObjectSelect?.Invoke(this, new ObjectSelectEvent(null));
        }
    }

    public void OnRewind(InputAction.CallbackContext context)
    {
        if (context.performed && this._selectedRewindObject != null && this._controlMode)
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
        if (context.performed && this._selectedRewindObject != null && this._controlMode)
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

    public void ToggleChanges()
    {
        this._controlMode = !this._controlMode;

        S_ControlModeToggle?.Invoke(this, new ControlModeEvent(this._controlMode));
    }
    
    public void OnChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject.FindGameObjectWithTag("CameraTransition").GetComponent<Animator>().Play("CameraTransitionAnimation");
        }
    }
}
