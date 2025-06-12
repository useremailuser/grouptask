using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System;

public class ThirdPersonController : MonoBehaviour
{
    public float walkSpeed = 2, runSpeed = 4, speed = 6;

    public Transform modelMesh;

    private Rigidbody rb;


    //pD will chase mV
    public Vector3 movementVector, playerDirection;

    public float jumpForce = 4;
    public bool grounded = true;

    private Animator ani;
    public TextMeshProUGUI displayText;

    public float dashLength = 3.0f;
    public float dashForce = 4f;

    public float targetTime = 1f;
    public float charges = 3;
    public Transform bulletSpawn;
    public float dashtime = 0.2f;

    public bool dashing = false;

    public bool Stationary = true;
    public Vector3 vel;
    float xMovement = 0;
    public bool IsGrounded;
    public float distToGround;

    public SpriteRenderer sprite;

    Vector3 Dashdir;
    

    void Awake() {
        rb = GetComponent<Rigidbody>();
        ani = modelMesh.GetComponent<Animator>();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDirection = transform.forward;
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update()
    {

        if (movementVector.x > 0)
        {
            sprite.flipX = false;
        }
        else if (movementVector.x < 0)
        {
            sprite.flipX = true;   
        }

        //dashing = false;
        if (dashtime == 0.5f)
        {

        }


        //grounded check
        //grounded = Physics.BoxCast(transform.position + Vector3.up, Vector3.one * 0.5f, Vector3.down, modelMesh.rotation, 0.7f);

        //Flattened versions of the Camera's direction. Removing their y-axis from play
        Vector3 forwardFlat = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        Vector3 sideFlat = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

        // WASD calculated
        movementVector = (forwardFlat * Input.GetAxis("Vertical"))
                        + (sideFlat * Input.GetAxis("Horizontal"));
        movementVector.Normalize();

        //Rotating player direction towards the movement vector
        //Locking rotation forward if RMB is held
        //SLERP -- spherical interpolation. Use for rotation lerping
        //if (Input.GetMouseButton(1))
        //    playerDirection = Vector3.Slerp(playerDirection, forwardFlat, 5*Time.deltaTime);
        //else
        //    playerDirection = Vector3.Slerp(playerDirection, movementVector.magnitude > 0 ? movementVector:playerDirection, 5 * Time.deltaTime);
        //modelMesh.rotation = Quaternion.LookRotation(playerDirection);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (charges > 0)
            {
                if (dashing == false)
                {
                charges -= 1;
                displayText.text = charges.ToString();
                dashing = true;
                rb.linearVelocity = new Vector3(0, 0, 0);
                    //rb.AddForce(bulletSpawn.forward * 20, ForceMode.Impulse);
                    if (Stationary == false)
                    {
                        rb.AddForce(movementVector * 20, ForceMode.Impulse);
                    }
                    else
                    {
                        dashDir = new Vector3(movementVector.x, 0, 0);
                        rb.AddForce(dashDir * 20, ForceMode.Impulse); 

                    }

                }

                    
            }
            return;

        }
        if (dashing == true)
        {
            dashtime -= Time.deltaTime;
            Stationary = false;
            rb.useGravity = false;


        }
        if (dashtime <= 0f)
        {

            dashing = false;
            dashtime = 0.2f;
            rb.linearVelocity = new Vector3(0, 0, 0);
        }

        if (charges < 3)
        {
            targetTime -= Time.deltaTime;


        }
        if (targetTime <= 0.0f)
        {

            AddCD();
            Debug.Log("addCD");
            displayText.text = charges.ToString();
        }
        //Jumping if SPACE pressed AND we're grounded
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
        {
          
               Stationary = false;
            
            
        }
        else
        {
            if (dashing == false)
            {
               Stationary = true;
            }
            else
            {

                Stationary = false;
                
            }

        }
        if (Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f))
        {

            grounded = true;


        }
        else
        {
               grounded = false;

        }

        if (dashing == false)
        {
            rb.useGravity = true;
        }




        //Lerping of SPEED towards 0, walkspeed and runspeed, given condition.
        //MOVE TOWARDS -- lerping with a set step
        //if (movementVector.magnitude > 6)
        //    speed = Mathf.MoveTowards(speed, Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed, 2 * Time.deltaTime );
        //else
        //    speed = Mathf.MoveTowards(speed, 6, 5 * Time.deltaTime);

        Animation Updates;
        //ani.SetBool("walking?", !Stationary);
        //ani.SetBool("dashing?", dashing);
    }

    void AddCD()
    {
        charges += 1;
        Debug.Log("CDadded");
        targetTime = 1f;
        displayText.text = charges.ToString();

    }
    void FixedUpdate() {
        //use movementvector and speed to calculate my object's movement this FixedUpdate (0.02 sec)
        //reapply the object's y velocity to retain gravity.

        if (dashing == false)
        {
            rb.linearVelocity = (movementVector * speed) + (Vector3.up * rb.linearVelocity.y);
        }

        if (Stationary == true)
        {
            Vector3 vel = rb.linearVelocity;
            vel.x = xMovement;
            rb.linearVelocity = vel;

        }
    }

}
