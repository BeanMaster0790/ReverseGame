using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController CharacterController;

    public Transform MoveCameraTransform;
    public Transform ControlCameraTransform;

    private Vector2 playerMoveVector;

    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private Vector3 playerVelocity;
    
    [Header("Move Mode Variables")]
    [SerializeField] private float _movePlayerSpeed = 0.2f;
    [SerializeField] private float _movePlayerMaxSpeed = 12f;
    [SerializeField] private float _movePlayerSpeedIncrement = 2f;
    [SerializeField] private float _moveJumpHeight = 1.5f;


    [Header("Control Mode Variables")]
    [SerializeField] private float _controlPlayerSpeed = 0.2f;
    [SerializeField] private float _controlJumpHeight = 1.5f;


    private float _playerCurrentSpeed = 0.2f;
    private bool _jumpPressed;

    private bool _controlMode;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        this._playerCurrentSpeed = this._movePlayerSpeed;
    }

    void FixedUpdate()
    {
        // Get camera directions (ignore Y)
        Vector3 camForward = (this._controlMode) ? this.ControlCameraTransform.forward : MoveCameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = (this._controlMode) ? this.ControlCameraTransform.right : MoveCameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        if (playerMoveVector.magnitude == 0)
            this._playerCurrentSpeed = (this._controlMode) ? this._controlPlayerSpeed : this._movePlayerSpeed;

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
            playerVelocity.y += ((this._controlMode) ? this._controlJumpHeight : this._moveJumpHeight) * Time.fixedDeltaTime;
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

            if (this._playerCurrentSpeed < this._movePlayerMaxSpeed && !this._controlMode)
                this._playerCurrentSpeed += this._movePlayerSpeedIncrement;
        }
    }
    
    public void OnChange(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            this._controlMode = !this._controlMode;

            this._playerCurrentSpeed = (this._controlMode) ? this._controlPlayerSpeed : this._movePlayerSpeed;

            if (this._controlMode)
            {
                this.MoveCameraTransform.gameObject.SetActive(false);
                this.ControlCameraTransform.gameObject.SetActive(true);
            }
            else
            {
                this.MoveCameraTransform.gameObject.SetActive(true);
                this.ControlCameraTransform.gameObject.SetActive(false);
            }

        }
    }
}
