using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCamera : MonoBehaviour
{

    private Vector2 _moveVector;

    private float _xRotation;
    private float _yRotation;

    [SerializeField] private float _sensitivity = 0.25f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this._xRotation += this._moveVector.y * this._sensitivity * Time.fixedDeltaTime;
        this._yRotation += this._moveVector.x * this._sensitivity * Time.fixedDeltaTime;

        this._xRotation = Mathf.Clamp(this._xRotation, -85 * Mathf.Deg2Rad, 85 * Mathf.Deg2Rad);

        this.transform.rotation = quaternion.Euler(-this._xRotation, this._yRotation, 0);
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        this._moveVector = context.ReadValue<Vector2>();

    }
}
