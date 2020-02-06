using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 10f;
    public Vector3 inputVector;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
       inputVector = new Vector3 (Input.GetAxisRaw("Horizontal"), rb.velocity.y, Input.GetAxisRaw("Vertical")).normalized * speed;
       transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));
       rb.velocity = inputVector;
    }

}
