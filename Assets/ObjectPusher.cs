using UnityEngine;

public class ObjectPusher : MonoBehaviour
{
    [SerializeField] private float _pushForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //I wish i knew this was a thing 2 months ago.
        Rigidbody rb = hit.collider.attachedRigidbody;

        if (!rb || rb.isKinematic)
         return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
        rb.AddForce(pushDir * _pushForce, ForceMode.Impulse);
    }

}
