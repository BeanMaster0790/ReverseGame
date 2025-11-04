using UnityEngine;

public class CameraTransitionHandler : MonoBehaviour
{
    [SerializeField] private GameObject _controlUi;

    private bool _isControlMode;

    public void ChangeControlMode()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<ObjectController>().ToggleChanges();

        this._isControlMode = !this._isControlMode;

        this._controlUi.SetActive(this._isControlMode);
    }
}
