using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CellDoorAnimation : MonoBehaviour
{
    [SerializeField] private Rigidbody _boxRigidBody;

    [SerializeField] private Vector3 _launchVector;

    private Animator _animator;

    private GameObject _player;

    private GameObject _followCamera;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = this.GetComponent<Animator>();

        this._player = GameObject.FindGameObjectWithTag("Player");

        this._player.GetComponent<PlayerMovement>().enabled = false;
        this._player.GetComponent<ObjectController>().enabled = false;
        this._player.GetComponent<PlayerInput>().enabled = false;

        this._followCamera = GameObject.FindGameObjectWithTag("FollowCamera");

        this._followCamera.SetActive(false);

        this.gameObject.GetComponentInChildren<Camera>().gameObject.SetActive(true);
    }

    public void StartAnimation()
    {
        this._animator.Play("DoorOpen", -1, 0f);

        this.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LaunchBox()
    {
        this._boxRigidBody.AddForce(this._launchVector, ForceMode.Impulse);
    }

    public void DestroyAnimator()
    {
        Debug.Log("Destory");

        this._animator.enabled = false;

        this._player.GetComponent<PlayerMovement>().enabled = true;
        this._player.GetComponent<ObjectController>().enabled = true;
        this._player.GetComponent<PlayerInput>().enabled = true;

        this._followCamera.SetActive(true);
        this.gameObject.GetComponentInChildren<Camera>().gameObject.SetActive(false);

        GameManager.Instance.GameStarted = true;
    }
}
