using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlCamera : MonoBehaviour
{

    private Vector2 _lookVector;
    private Vector2 _moveVector;

    private float _xRotation;
    private float _yRotation;

    public static float ControlModeSens = 0.25f;

    public bool IsNoclip;

    [SerializeField] private float _clipSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this._xRotation += this._lookVector.y * ControlModeSens * Time.fixedDeltaTime;
        this._yRotation += this._lookVector.x * ControlModeSens * Time.fixedDeltaTime;

        this._xRotation = Mathf.Clamp(this._xRotation, -85 * Mathf.Deg2Rad, 85 * Mathf.Deg2Rad);

        this.transform.rotation = quaternion.Euler(-this._xRotation, this._yRotation, 0);

        if (!IsNoclip)
            return;

        Vector3 move = this.transform.forward * this._moveVector.y + this.transform.right * this._moveVector.x;

        this.transform.position += move * this._clipSpeed * Time.fixedDeltaTime;
    }
    
    public void OnLook(InputAction.CallbackContext context)
    {
        this._lookVector = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        this._moveVector = context.ReadValue<Vector2>();
    }

    public void ActivateNoClip()
    {
        this.IsNoclip = !this.IsNoclip;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        player.GetComponent<CharacterController>().enabled = !this.IsNoclip;
        player.GetComponent<PlayerMovement>().enabled = !this.IsNoclip;

        if (!this.IsNoclip) 
        {
            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = this.transform.position;
            player.GetComponent<CharacterController>().enabled = true;

            this.transform.position = player.transform.position + new Vector3(0,1,0);

        }
    }


}
