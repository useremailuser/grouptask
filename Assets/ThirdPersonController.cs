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
    private Vector3 movementVector, playerDirection;

    public float jumpForce = 4;
    public bool grounded = true;

    private Animator ani;
    public TextMeshProUGUI displayText;

    public float dashLength = 3.0f;
    public float dashForce = 4f;

    public float targetTime = 3.0f;
    public float charges = 3;
    public Transform bulletSpawn;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        ani = modelMesh.GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerDirection = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {



    
        //grounded check
        grounded = Physics.BoxCast(transform.position + Vector3.up, Vector3.one * 0.5f, Vector3.down, modelMesh.rotation, 0.7f);

        //Flattened versions of the Camera's direction. Removing their y-axis from play
        Vector3 forwardFlat = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        Vector3 sideFlat = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

        {
            if (Input.GetKey(KeyCode.A))

            {
                //rb.AddForce(-1 * 100 * Time.deltaTime, 0, 0);
            }
        }

        {
            if (Input.GetKey(KeyCode.D))
            {
                //rb.AddForce(1 * 100 * Time.deltaTime, 0, 0);
            }
        }
        // WASD calculated
        movementVector = ( forwardFlat * Input.GetAxis("Vertical") ) 
                        + ( sideFlat * Input.GetAxis("Horizontal") );
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
                rb.AddForce(bulletSpawn.forward * 90, ForceMode.Force);
                charges -= 1;
                displayText.text = charges.ToString();

                Debug.Log("space");
            }

        }
        if (charges < 3)
        {
            targetTime -= Time.deltaTime;
           
            
        }
        if (targetTime <= 0.0f)
        {

            AddCD();
            Debug.Log("addCD");
        }
        //Jumping if SPACE pressed AND we're grounded
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb.AddForce(0,jumpForce,0,ForceMode.Impulse);
        }

        //Lerping of SPEED towards 0, walkspeed and runspeed, given condition.
        //MOVE TOWARDS -- lerping with a set step
        //if (movementVector.magnitude > 6)
        //    speed = Mathf.MoveTowards(speed, Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed, 2 * Time.deltaTime );
        //else
        //    speed = Mathf.MoveTowards(speed, 6, 5 * Time.deltaTime);

        //Animation Updates
        //ani.SetBool("walking?", movementVector.magnitude > 0);
        //ani.SetBool("running?", Input.GetKey(KeyCode.LeftShift));
        //ani.SetBool("locked?", Input.GetMouseButton(1));
        //ani.SetFloat("x", Input.GetAxis("Horizontal"));
        //ani.SetFloat("z", Input.GetAxis("Vertical"));
        //ani.SetBool("grounded?", grounded);
    }
    void AddCD()
    {
        charges += 1;
        Debug.Log("CDadded");
        targetTime = 3.0f;
        displayText.text = charges.ToString();

    }
    void FixedUpdate() {
        //use movementvector and speed to calculate my object's movement this FixedUpdate (0.02 sec)
        //reapply the object's y velocity to retain gravity.
        rb.linearVelocity = (movementVector * speed) + (Vector3.up * rb.linearVelocity.y);
    }
}
