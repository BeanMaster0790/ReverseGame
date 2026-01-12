using Unity.VisualScripting;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float Fov = 90f;
    public float ViewDistance = 10;

    public float ListenDistance = 5;

    public LayerMask RaycastLayers;

    public Transform RayPoint;

    [SerializeField] private float _detectionTimeSeconds = 3;

    [SerializeField] private float _damagePerTick = 2;

    [SerializeField] private float _detectionTimeSecondsListen = 5;

    private float _damageTime;

    private float _damageTimeListen;

    private Timer _damageTimer;

    private Transform _player;

    void Start()
    {
        this._player = GameObject.FindGameObjectWithTag("Player").transform;

        this._damageTimer = new Timer();

        this._damageTime = this._detectionTimeSeconds / (this._player.GetComponent<PlayerHealth>().PlayerMaxHealth / this._damagePerTick);
        this._damageTimeListen = this._detectionTimeSecondsListen / (this._player.GetComponent<PlayerHealth>().PlayerMaxHealth / this._damagePerTick);

        Debug.Log(this._damageTime);

        this._damageTimer.StartTimer(this._damageTime); 
    }

    // Update is called once per frame
    void Update()
    {
        this._damageTimer.Update();

        Vector3 difference = this._player.position - this.transform.position;

        if(Vector3.Dot(this.transform.forward, difference.normalized) > Mathf.Cos(this.Fov * 0.5f * Mathf.Deg2Rad) && Vector3.Distance(this._player.position, this.transform.position) < this.ViewDistance)
        {
            Vector3 rayDirection = this._player.position - this.RayPoint.position;

            if(Physics.Raycast(this.RayPoint.position, rayDirection, out RaycastHit hitInfo, this.ViewDistance, this.RaycastLayers))
            {
                if(hitInfo.transform.tag == "Player" && this._damageTimer.IsFinished)
                {
                    this._player.GetComponent<PlayerHealth>().Damage(this._damagePerTick);

                    this._damageTimer.StartTimer(this._damageTime);
                }
            }
        }
        else if(Vector3.Distance(this._player.position, this.transform.position) < this.ListenDistance && this._damageTimer.IsFinished)
        {
            this._player.GetComponent<PlayerHealth>().Damage(this._damagePerTick);

            this._damageTimer.StartTimer(this._damageTimeListen);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 forward = transform.forward * ViewDistance;

        Gizmos.DrawLine(transform.position, transform.position + forward);

        float halfFOV = Fov * 0.5f;

        Vector3 rightDir = Quaternion.Euler(0, halfFOV, 0) * transform.forward * ViewDistance;
        Vector3 leftDir = Quaternion.Euler(0, -halfFOV, 0) * transform.forward * ViewDistance;

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + rightDir);
        Gizmos.DrawLine(transform.position, transform.position + leftDir);

        if (_player)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _player.position);
        }
    }
}
