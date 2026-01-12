using UnityEngine;

public class RandomLaunch : MonoBehaviour
{
    [SerializeField] private float _radius = 10;
    [SerializeField] private float _power = 10f;
    [SerializeField] private float _upwardsModifier = 0.3f;


    void Start()
    {
        Launch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Launch()
    {
        Vector3 launchPosition = this.transform.position;

        Collider[] colliders = Physics.OverlapSphere(launchPosition, this._radius);

        foreach(Collider collider in colliders)
        {
            Rigidbody rb = collider.gameObject.GetComponent<Rigidbody>();

            if(!rb)
                continue;

            rb.AddExplosionForce(this._power, launchPosition, this._radius, this._upwardsModifier, ForceMode.Force);
        }

    }
}
