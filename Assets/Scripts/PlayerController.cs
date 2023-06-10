using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public float jumpForce = 10f;
    public float initialMass;
    public float winCount = 13;
    public TMP_Text countText;
    public GameObject winTextObject;
    public AudioManager audioM;
    public AudioSource audioSource;
    public MyGameManager gameM;
    public Vector3 jump;

    public Rigidbody rb;
    public Collider collider;
    private Renderer playerMat;
    private int count;
    private float movementX;
    private float movementY;
    private float movementZ;
    private bool isGrounded;
    private float distGround;


    public float MASS_MULT = 2.0f;
    public float SPEED_MULT = 1.8f;
    public float JUMP_MULT = 1.9f;
    public float DRAG_DIV = 1.5f;
    public float EMISSION_MULT = 1.2f;

    // springen: so ausprobieren
    /*
    function Start()
    {
        // get the distance to ground
        distToGround = collider.bounds.extents.y;
    }

    function IsGrounded(): boolean {
   return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1);
 }
    */

// Start is called before the first frame update
void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMat = GetComponent<Renderer>();
        GameObject gameManagerObject = GameObject.Find("GameManager");
        gameM = gameManagerObject.GetComponent<MyGameManager>();
        audioM = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Debug.Log(audioM.sounds);
        count = 0;
        initialMass = rb.mass;
        SetCountText();
        winTextObject.SetActive(false);

        jump = new Vector3(0.0f, 2.0f, 0.0f);

        collider = GetComponent<Collider>();

        distGround = collider.bounds.extents.y;

        //audioM.Play("Rolling");

        // drag not mass affects falling speed
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x; 
        movementY = movementVector.y;
        movementZ = 0.0f;
        //MoveRelToCamera();
    }

    void MoveRelToCamera()
    {
        Vector3 movement = new Vector3(movementX, movementZ, movementY);

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        right = right.normalized;

        //right = new Vector3(1, 0, 0);
        //forward = new Vector3(0, 0, 1);

        Vector3 rightRelInput = movement.x * right;
        Vector3 forwardRelInput = movement.z * forward;
        movement = rightRelInput + forwardRelInput;
        rb.AddForce(movement * speed);
    }


    void SetCountText()
    {
        countText.text = "Mass: " + rb.mass.ToString() + "  -  Speed: " + rb.velocity.magnitude;
        if (count >= winCount)
        {
            winTextObject.SetActive(true);
        }
    }

    // Update is called once per frame
    // before rendering a frame
    void Update()
    {
        // jumps should stay here, raycast works well
        if (Input.GetKeyDown(KeyCode.Space) && this.IsGrounded())
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        if (isGrounded)
        {
            audioSource.volume = rb.velocity.magnitude/100;
        }
        else
        {
            audioSource.volume = 0;
        }

    }

    // alt grounded using raycast
    bool IsGrounded() {
      return Physics.Raycast(transform.position, -Vector3.up, this.distGround + 0.2f);
    }

void OnCollisionStay(Collision hit)
    {
        if (hit.gameObject.CompareTag("Wall"))
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.CompareTag("Wall"))
        {
            //isGrounded = false;
        }
        else
        {
            //isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision hit)
    {
        if (!hit.gameObject.CompareTag("Wall"))
        {
        isGrounded = false;
        }

    }

    // restart wehen under y = -400


    //just before physics calculations, phys code here
    void FixedUpdate()
    {

        MoveRelToCamera();

        /*
        float playerVerticalInput = Input.GetAxis("Vertical");
        float playerHorizontalInput = Input.GetAxis("Horizontal");

        // Camera normalized directional vectors
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;
        forward = forward.normalized;
        forward = right.normalized;

        //Direction-relative-input vectors
        Vector3 forwardRelInput = playerVerticalInput * forward;
        Vector3 rightRelInput = playerHorizontalInput * right;

        // Camera relative movement
        Vector3 cameraRelMovement = forwardRelInput + rightRelInput;
        //this.transform.Translate(cameraRelMovement, Space.World);
        rb.AddForce(cameraRelMovement);
        */




        //better 
        /*
                 if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        { 
            transform.Translate(-1 * Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A)) { 
            transform.Rotate(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 1, 0);
        }
        */

        if (rb.position.y < -350)
        {
            Debug.Log("Dead");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            //count += 1;
            rb.mass *= MASS_MULT;
            //rb.mass += initialMass;
            //speed += 1.8f * rb.mass/initialMass * speed;
            speed *= SPEED_MULT;
            jumpForce *= JUMP_MULT;
            // lower drag more gravity to simulate weight
            rb.drag /= DRAG_DIV; 
            rb.angularDrag /= DRAG_DIV;
            rb.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            playerMat.material.SetColor("_EmissionColor", playerMat.material.GetColor("_EmissionColor") * EMISSION_MULT);
            


            //rb.gravityScale *= 2;
            SetCountText();
        }
    }

    public void becomeActive()
    {
        this.rb.isKinematic = false;
    }

}


