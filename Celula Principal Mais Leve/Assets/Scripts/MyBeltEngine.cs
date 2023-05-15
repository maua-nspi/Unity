using UnityEngine;

public class MyBeltEngine : MonoBehaviour
{
    public float speed = 10f;
    public float force = 10f;
    private Rigidbody beltRigidbody;

    void Start()
    {
        beltRigidbody = transform.parent.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * speed * Time.deltaTime);
        beltRigidbody.AddForce(transform.right * force);
    }
}