using UnityEngine;

public class BigGuyController1 : MonoBehaviour
{

    public float walkSpeed = 2, runSpeed = 4, speed = 1;

    public Transform modelMesh;

    private Rigidbody rb;

    public float distToGround;
    public float groundTime = 1f;

    //pD will chase mV
    private Vector3 movementVector, playerDirection;

    public float jumpForce = 7f;
    public bool grounded = true;
    public Transform Player;
    public bool attacking;
    public float attackTime = 1f;
    public bool jumping;
    public Transform spawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;

    }

    // Update is called once per frame
    void Update()
    {
        //movementVector = Player.position - transform.position;

        if (Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f))
        {

            grounded = true;


        }
        else
        {
            grounded = false;

        }
        if (attacking == false)
        {
            groundTime -= Time.deltaTime;
            if (groundTime <= 0)
            {
                
                groundTime = 1f;
                attacking = true;

                if (grounded)
                {

                    
                    jumping = true;
                    
                }
                
            }
                
            
        }
        if ((attacking == true) && (transform.position.x - Player.position.x < 15))
            {
           
            attackTime -= Time.deltaTime;
           
            if (attackTime <= 0)
            {
                
                attackTime = 1f;

                
               
               attacking = false;
                 
            }
        }
        movementVector.Normalize();
        if ((attacking == true) && (attackTime > 0))
        {
            rb.AddForce(movementVector * 1, ForceMode.Force);
        }
        if (transform.position.x - Player.position.x > 20)
        {
            transform.position = spawner.position;
            rb.linearVelocity = new Vector3(0, 0, 0);

        }
    }


    void FixedUpdate()
    {
        if (jumping == true)
        {
            rb.AddForce(0, 1 * 8, 0, ForceMode.Impulse);
            
            jumping = false;
        }
       
        if (groundTime > 0)
        {
            movementVector = (Player.position - transform.position);
            
        }
        if (grounded)
        {
            rb.linearVelocity = (movementVector * speed) + (Vector3.up * rb.linearVelocity.y);
        }

            

        


    }
}
