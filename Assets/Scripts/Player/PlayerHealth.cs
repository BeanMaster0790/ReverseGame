using Unity.Mathematics;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _playerHealth = 100;

    [SerializeField] private int _playerMaxHealth = 100;


    public void Damage(int amount)
    {
        this._playerHealth -= math.abs(amount);

        if(this._playerHealth <= 0)
        {
            Respwan();
        }
    }

    public void Respwan()
    {
        
    }
}
