using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField] private int _damage;

    private Timer _cooldownTimer = new Timer();

    [SerializeField] float _damageHitCooldown = 0.5f;

    void Start()
    {
        this._cooldownTimer.StartTimer(this._damageHitCooldown);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            DamageCollider(collision);
        }
    }

    void Update()
    {
        this._cooldownTimer.Update();
    }

    private void DamageCollider(Collision collision)
    {
        if (!this._cooldownTimer.IsFinished)
            return;

        PlayerHealth health = collision.collider.gameObject.GetComponent<PlayerHealth>();

        if (health == null)
        {
            Debug.LogError("The player is missing its health component");
        }

        this._cooldownTimer.StartTimer(this._damageHitCooldown);

        health.Damage(this._damage);
    }
}
