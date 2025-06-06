using UnityEngine;

public class EnemyControllerJump : MonoBehaviour
{

    public float walkSpeed = 2, runSpeed = 4, speed = 1;

    public Transform modelMesh;

    private Rigidbody rb;

    public float distToGround;

    //pD will chase mV
    private Vector3 movementVector, playerDirection;

    public float jumpForce = 4;
    public bool grounded = true;
    public Transform Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;

    }

    // Update is called once per frame
    void Update()
    {
        movementVector.x =  Player.position.x - transform.position.x;
        movementVector.Normalize();
        if (Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f))
        {

            grounded = true;


        }
        else
        {
            grounded = false;

        }
        if (grounded == true)
        {
           
            
             rb.AddForce(0, jumpForce, 0, ForceMode.Force);
            rb.AddForce(movementVector);
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = (movementVector * speed) + (Vector3.up * rb.linearVelocity.y);

    }
}
