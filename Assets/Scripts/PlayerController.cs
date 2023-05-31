using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public float jumpForce = 10f;
    public float initialMass;
    public float winCount = 13;
    public TMP_Text countText;
    public GameObject winTextObject;
    public Vector3 jump;

    public Rigidbody rb;
    private Renderer playerMat;
    private int count;
    private float movementX;
    private float movementY;
    private float movementZ;
    private bool isGrounded;

    public float MASS_MULT = 2.0f;
    public float SPEED_MULT = 1.8f;
    public float JUMP_MULT = 1.9f;
    public float DRAG_DIV = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMat = GetComponent<Renderer>();
        count = 0;
        initialMass = rb.mass;
        SetCountText();
        winTextObject.SetActive(false);

        jump = new Vector3(0.0f, 2.0f, 0.0f);

        // drag not mass affects falling speed
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x; 
        movementY = movementVector.y;
        movementZ = 0.0f;
    }


void SetCountText()
    {
        countText.text = "Mass: " + rb.mass.ToString() + "  -  Accelleration: " + speed;
        if (count >= winCount)
        {
            winTextObject.SetActive(true);
        }
    }

    // Update is called once per frame
    // before rendering a frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }


    //just before physics calculations, phys code here
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, movementZ, movementY);
        rb.AddForce(movement * speed);



        /*
         * Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(Vector3.left);
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(Vector3.right);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(Vector3.up);
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(Vector3.down);
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
            playerMat.material.SetColor("_EmissionColor", playerMat.material.GetColor("_EmissionColor") * 1.1f);
            


            //rb.gravityScale *= 2;
            SetCountText();
        }
    }

}


