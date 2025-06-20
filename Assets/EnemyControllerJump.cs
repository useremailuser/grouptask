using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyControllerJump : MonoBehaviour
{
    NavMeshAgent agent;
    private float timer;
    private Vector3 playerscent;
    public float walkSpeed = 2, runSpeed = 4, speed = 1;

    public Transform modelMesh;

    private Rigidbody rb;

    public float distToGround;
    public float groundTime = 0.6f;
    //pD will chase mV
    private Vector3 movementVector, playerDirection;

    public float jumpForce = 1;
    public bool grounded = true;
    public Transform Player;

    public enum State
    {
        Patrol,
        Investigating,
        Chasing,
        Attack,
    }
    public State state;

    ThirdPersonController player;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<ThirdPersonController>();
    }

   
    void Start()
    {
        state = State.Patrol;
        NextState();
                rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    void NextState()
    {
        switch (state)
        {
            case State.Patrol:
                StartCoroutine(PatrolState());
                break;
            case State.Investigating:
                StartCoroutine(InvestigatingState());
                break;
            case State.Chasing:
                StartCoroutine(ChasingState());
                break;
            case State.Attack:
                StartCoroutine(AttackState());
                break;
            default:
                Debug.LogError("State doesn't exist");
                break;
        }
    }

    IEnumerator PatrolState()
    {
        Debug.Log("Entering Patrol State");


        while (state == State.Patrol) // "Update() loop"
        {

           
            //Direction from A to B
            //B - A
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.Normalize();
            //Dot product parameters should be "normalised"
            float result = Vector3.Dot(transform.forward, directionToPlayer);

            if (result >= 0.95 && Vector3.Distance(transform.position, player.transform.position) < 10)
            {
                state = State.Chasing;
            }

            yield return null; // Wait for a frame
        }


        //tear down/ exit  point  / OnDestory()
        Debug.Log("Exiting Patrol State");
        NextState();
    }


    IEnumerator InvestigatingState()
    {
        Debug.Log("Entering Investigating State");
        float startTime = Time.time;
        float deltaSum = 0;

        while (state == State.Investigating) // "Update() loop"
        {
            //timer is set so after 6 seconds it updates the player position for the monster to track, 
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                playerscent = player.transform.position;
                timer = 3;
            }

            agent.destination = playerscent;

            //if player is more than 10 units away from monster and monster is within 1 unit of last known location, swithch back to patrolling
            if (Vector3.Distance(transform.position, player.transform.position) > 10 && Vector3.Distance(agent.destination, transform.position) < 1)
            {
                state = State.Patrol;
            }


            if (Vector3.Distance(transform.position, player.transform.position) < 7)
            {
                state = State.Chasing;
            }
            deltaSum += Time.deltaTime;
            yield return null; // Wait for a frame
        }

        float endTime = Time.time - startTime;
        Debug.Log("DeltaSum = " + deltaSum + " | End Time = " + endTime);

        //tear down/ exit  point  / OnDestory()
        Debug.Log("Exiting Investigating State");
        NextState();
    }


    IEnumerator ChasingState()
    {
        Debug.Log("Entering Chasing State");


        while (state == State.Chasing) // "Update() loop"
        {
            float wave = Mathf.Sin(Time.time * 20f) * 0.1f + 1f;
            float wave2 = Mathf.Cos(Time.time * 20f) * 0.1f + 1f;
            transform.localScale = new Vector3(wave, wave2, wave);


            float shimmy = Mathf.Cos(Time.time * 30f) * 10f + 15f;
            //choose transform movement or rigidbody movement
            //transform.position += transform.right * shimmy * Time.deltaTime;

            Vector3 directionToPlayer = player.transform.position - transform.position;
            //directionToPlayer.Normalize();
            float angle = Vector3.SignedAngle(transform.forward, directionToPlayer, Vector3.up);

            agent.destination = player.transform.position;

            if (angle > 0)
            {
                transform.rotation *= Quaternion.Euler(0f, 50f * Time.deltaTime, 0f);
            }
            else
            {
                transform.rotation *= Quaternion.Euler(0f, -50f * Time.deltaTime, 0f);
            }

            //if (rb.velocity.magnitude < 5f)
            //{
            //    rb.AddForce(transform.forward * shimmy * 0.01f, ForceMode.Acceleration);
            //}

            //if player is less then two units away change to attacking state
            if (directionToPlayer.magnitude < 2f)
            {
                state = State.Attack;
            }
            //if player is greater then ten units away change to investigating
            else if (directionToPlayer.magnitude > 6f)
            {
                playerscent = player.transform.position;
                state = State.Investigating;
            }

            yield return new WaitForFixedUpdate(); // Wait for the next fixed update
        }


        //tear down/ exit  point  / OnDestory()
        Debug.Log("Exiting Chasing State");
        NextState();
    }

    IEnumerator AttackState()
    {
        Debug.Log("Entering Attack State");

        while (state == State.Attack) // "Update() loop"
        {
            Vector3 scale = transform.localScale;
            scale.z = Mathf.Cos(Time.time * 20f) * 3f + 1f;
            transform.localScale = scale;

            Vector3 directionToPlayer = player.transform.position - transform.position;
            if (directionToPlayer.magnitude > 3f)
            {
                state = State.Investigating;
            }

            yield return null; // Wait for a frame
        }

        //tear down/ exit  point  / OnDestory()
        Debug.Log("Exiting Attack State");
        NextState();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        movementVector.x =  Player.position.x - transform.position.x;
        movementVector.Normalize();
        if (Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f))
        {  

        }
        else
        {
            grounded = false;

        }
        if (grounded == true)
        {
           
           
            
               rb.AddForce(0, jumpForce, 0, ForceMode.Force);
               
            
             
            
        }
    }
    void FixedUpdate()
    {
        rb.linearVelocity = (movementVector * speed) + (Vector3.up * rb.linearVelocity.y);

    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Player")
        {
            Debug.Log("HIII");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
