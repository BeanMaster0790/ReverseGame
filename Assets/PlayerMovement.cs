using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController CharacterController;
    public Transform CameraTransform;

    private Vector2 playerMoveVector;
    
    [Header("Player Movement Variables")]
    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private float _playerSpeed = 0.2f;
    [SerializeField] private float _playerMaxSpeed = 12f;
    [SerializeField] private float _playerSpeedIncrement = 2f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _jumpHeight = 1.5f;


    private float _playerCurrentSpeed = 0.2f;
    private bool _jumpPressed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        this._playerCurrentSpeed = this._playerSpeed;
    }

    void FixedUpdate()
    {
        // Get camera directions (ignore Y)
        Vector3 camForward = CameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = CameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        if (playerMoveVector.magnitude == 0)
            this._playerCurrentSpeed = this._playerSpeed;

        float ySpeed = playerVelocity.y;

        playerVelocity = camForward * playerMoveVector.y + camRight * playerMoveVector.x;

        Vector2 groundVel = new Vector2(playerVelocity.x, playerVelocity.z);

        groundVel.Normalize();

        groundVel *= this._playerCurrentSpeed * Time.fixedDeltaTime;

        playerVelocity.x = groundVel.x;
        playerVelocity.z = groundVel.y;

        playerVelocity.y = ySpeed;

        if (CharacterController.isGrounded)
            playerVelocity.y = 0;

        if (this._jumpPressed && CharacterController.isGrounded)
        {
            playerVelocity.y += this._jumpHeight * Time.fixedDeltaTime;
            this._jumpPressed = false; 
        }

        if (!CharacterController.isGrounded)
            playerVelocity.y += this._gravity * Time.fixedDeltaTime;

        CharacterController.Move(playerVelocity);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        playerMoveVector = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            this._jumpPressed = true;

            if(this._playerCurrentSpeed < this._playerMaxSpeed)
                this._playerCurrentSpeed += this._playerSpeedIncrement;
        }
    }
}
