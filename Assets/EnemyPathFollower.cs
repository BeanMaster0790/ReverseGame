using UnityEngine;

public class EnemyPathFollower : MonoBehaviour
{
    public CharacterController Controller;

    public EnemyPath Path;

    [SerializeField] private float _speed;
    [SerializeField] private float _gravity;

    [SerializeField] private Vector3 _velocity;

    private EnemyPathNode _node;

    private Timer _waitTimer;

    void Start()
    {
        this._node = Path.GetNextNode(null);

        this._waitTimer = new Timer();

        Debug.Log(this._node.Position);

        BoundingBoxDrawer boxDrawer = this.gameObject.AddComponent<BoundingBoxDrawer>();
        boxDrawer._lineColour = Color.red;
    }

    void Update()
    {
        this._waitTimer.Update();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = this._node.Position - this.transform.position;

        direction.Normalize();

        float yVel = this._velocity.y;

        this._velocity = new Vector3(direction.x * this._speed * Time.fixedDeltaTime, yVel, direction.z * this._speed * Time.fixedDeltaTime);

        if (Controller.isGrounded)
            this._velocity.y = 0;
        else
            this._velocity.y += this._gravity * Time.fixedDeltaTime;

        if(this._waitTimer.IsFinished)
            this.Controller.Move(this._velocity);

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if(this.Path.IsColliderMyNode(other, this._node))
        {
            this._waitTimer.StartTimer(this._node.WaitTime);
            this._node = this.Path.GetNextNode(this._node);
        }
    }
}
