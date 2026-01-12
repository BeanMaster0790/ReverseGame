using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectController : MonoBehaviour
{
    public Transform CameraTransform;

    public LayerMask objectLayer;

    [SerializeField] private SelectionObject _selectedRewindObject = null;

    [SerializeField] private bool _isRewinding;

    [SerializeField] private bool _isPausing;

    public bool InControlMode;

    public static EventHandler<ControlModeEvent> S_ControlModeToggle;
    public static EventHandler<ObjectSelectEvent> S_ObjectSelect;

    // Update is called once per frame
    void Update()
    {
        if (!this.InControlMode)
        {
            return;
        }

        Vector3 camForward = CameraTransform.forward;
        camForward.Normalize();

        Vector3 camRight = CameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        if (this._isRewinding && this._selectedRewindObject != null)
        {
            this._isRewinding = this._selectedRewindObject.IsRewinding;

            return;
        }
        else if(this._isPausing && this._selectedRewindObject != null)
        {
            this._isPausing = this._selectedRewindObject.IsPasued;

            return;
        }
        else if(this._selectedRewindObject == null)
        {
            this._isPausing = false;
            this._isRewinding = false;
        }

        RaycastHit hit;
        float maxDistance = 500f;

        if (Physics.Raycast(CameraTransform.position, camForward, out hit, maxDistance, objectLayer))
        {
            this._selectedRewindObject = hit.collider.GetComponent<SelectionObject>();

            if(this._selectedRewindObject == null)
            {
                this._selectedRewindObject = null;
                S_ObjectSelect?.Invoke(this, new ObjectSelectEvent(null));     
            }
            else
            {
                S_ObjectSelect?.Invoke(this, new ObjectSelectEvent(this._selectedRewindObject));
            }

        }
    }

    public void OnRewind(InputAction.CallbackContext context)
    {
        if (context.performed && this._selectedRewindObject != null && this.InControlMode)
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
        if (context.performed && this._selectedRewindObject != null && this.InControlMode)
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
        this.InControlMode = !this.InControlMode;

        S_ControlModeToggle?.Invoke(this, new ControlModeEvent(this.InControlMode));
    }
    
    public void OnChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject.FindGameObjectWithTag("CameraTransition").GetComponent<Animator>().Play("CameraTransitionAnimation");
            PlayerSoundsManager.Current.PlaySound("Tab");
        }
    }
}
