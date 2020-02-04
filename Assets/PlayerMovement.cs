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
       inputVector = new Vector3 (Input.GetAxisRaw("Horizontal") * speed, rb.velocity.y, Input.GetAxisRaw("Vertical") * speed).normalized * 8;
    //    Vector3 direction = focus.position - transform.position;
       rb.velocity = inputVector;
    }

}
