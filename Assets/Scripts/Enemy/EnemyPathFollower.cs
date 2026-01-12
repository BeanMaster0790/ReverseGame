using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    public CharacterController Controller;

    public EnemyPath Path;

    [SerializeField] private float _speed;
    [SerializeField] private float _gravity;

    [SerializeField] private Vector3 _velocity;

    [HideInInspector] private float _rotationSpeed;
    private EnemyPathNode _node;

    private Timer _waitTimer;

    private AudioSource _walkingSound;

    private float _walkingSoundStartVol;

    void Start()
    {
        this._node = Path.GetNextNode(null);

        this._waitTimer = new Timer();

        this._walkingSound = this.gameObject.GetComponent<AudioSource>();
        this._walkingSoundStartVol = this._walkingSound.volume;
    }

    void Update()
    {
        this._waitTimer.Update();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this._node == null)
        {
            this._node = Path.GetNextNode(null);
            return;
        }

        Vector3 direction = this._node.Position - this.transform.position;

        if(this._waitTimer.IsFinished)
        {
            this._walkingSound.volume = this._walkingSoundStartVol;

            direction.y = 0;

            direction.Normalize();

            this.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z), Vector3.up);
        }
        else
        {
            this._walkingSound.volume = 0;

            direction = Vector3.zero;

            this.transform.RotateAround(this.transform.position, Vector3.up, this._rotationSpeed * Time.fixedDeltaTime);
        }

        float yVel = this._velocity.y;

        
        this._velocity = new Vector3(direction.x * this._speed * Time.fixedDeltaTime, yVel, direction.z * this._speed * Time.fixedDeltaTime);

        if (Controller.isGrounded)
            this._velocity.y = 0;
        else
            this._velocity.y += this._gravity * Time.fixedDeltaTime;

        this.Controller.Move(this._velocity);
        

    }

    void OnTriggerEnter(Collider other)
    {
        if(this.Path.IsColliderMyNode(other, this._node))
        {
            this._waitTimer.StartTimer(this._node.WaitTime);
            this._rotationSpeed = this._node.RotationSpeed;
            this._node = this.Path.GetNextNode(this._node);
        }
    }
}
