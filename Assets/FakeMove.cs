using UnityEngine;

public class FakeMove : MonoBehaviour
{
    [SerializeField] private float _speed;

    [SerializeField] private Vector3 _velocity;

    [SerializeField] private  Transform _goToPos;

    void Start()
    {
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = this._goToPos.position - this.transform.position;

        direction.Normalize();
        
        this._velocity = direction * this._speed * Time.fixedDeltaTime;

        this.transform.Translate(this._velocity);

        if(Vector3.Distance(this.transform.position, this._goToPos.position) < 0.25f)
        {
            this.gameObject.GetComponent<Rigidbody>().useGravity = true;
            this.gameObject.GetComponent<BoxCollider>().enabled = true;

            GameObject.Destroy(this);
        }
        
    }
}
