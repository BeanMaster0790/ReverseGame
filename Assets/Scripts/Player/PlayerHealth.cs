using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _playerHealth = 100;
    [SerializeField] private float _regenTickAmount = 0.25f;

    private RawImage _detectionEffect;

    public float PlayerMaxHealth = 100;

    public CheckpointZone CurrentCheckPoint;

    public bool Invincable;

    private float _secondsSinceLastDamage;

    void Start()
    {
        this._playerHealth = this.PlayerMaxHealth;

        this._detectionEffect = GameObject.FindGameObjectWithTag("DetectionEffect").GetComponent<RawImage>();
    }

    void Update()
    {
        if(!this.Invincable)
            this._detectionEffect.color = new Color(this._detectionEffect.color.r, this._detectionEffect.color.g, this._detectionEffect.color.b, (this.PlayerMaxHealth - this._playerHealth) / this.PlayerMaxHealth);

        this._secondsSinceLastDamage += Time.deltaTime;

        if(this._secondsSinceLastDamage > 1.5f)
        {
            this.Damage(-this._regenTickAmount);
        }  
    }

    public void Damage(float amount)
    {
        if(this.Invincable)
            return;

        if(amount > 0)
            this._secondsSinceLastDamage = 0;

        this._playerHealth -= amount;

        this._playerHealth = Mathf.Clamp(this._playerHealth, 0, this.PlayerMaxHealth);

        if(this._playerHealth <= 0)
        {
            Respwan();
        }
    }

    public void Respwan()
    {
        this.GetComponent<CharacterController>().enabled = false; 
        this.transform.position = this.CurrentCheckPoint.RespawnPoint.position;
        this.GetComponent<CharacterController>().enabled = true; 

        this._playerHealth = this.PlayerMaxHealth;

        if(this.gameObject.GetComponent<ObjectController>().InControlMode)
        {
            GameObject.FindGameObjectWithTag("CameraTransition").GetComponent<CameraTransitionHandler>().ChangeControlMode();
        }

        this.CurrentCheckPoint.Respawn();
    }
}
