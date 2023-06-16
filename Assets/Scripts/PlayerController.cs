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
    //TODO show proper num
    public double initialMass;
    //public double pressure = "250 Milliarden bar"; 265 billion bar
    //public double density = "150 Gramm pro Kubikzentimeter, 150000000 bzw 150 Mio pro Kubikmeter";
    // neutron star: about 1015 grams / cubic cm
    private bool hasWon = false;
    public TMP_Text countText;
    public GameObject winTextObject;
    public AudioManager audioM;
    public AudioSource audioSource;
    public AudioSource collisionAudioSource;
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
    public float JUMP_MULT = 1.92f;
    public float DRAG_DIV = 1.25f;
    public float EMISSION_MULT = 1.2f;
    public float WIN_MASS = 15000000;

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
    //TODO Spielziel, möglichkeit alles zu sammeln(?), Kakteen umgehbar...., Loch in Mitte als Endziel machbar? You win als texture, leittextur an wege 
    // make goals and enemies more obvious
    // enemy - jump in rand dir

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
        hasWon = false;
        SetCountText();
        winTextObject.SetActive(false);
        audioM.Play("Ambient");

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
        //TODO set this in corners
        // density : 1 Kubikmeter/1000000 = 1 Kubikzentimeter
        // 100 kubikmeter - drone 10x10x10 meter -> 15 000 000kg 4-5 Meters, ca 2.2 für 10 kubikmeter
        countText.text = "Mass: " + rb.mass.ToString("n0") + "\nSpeed: " + Math.Round(rb.velocity.magnitude, 2) + "\nDensity: " + (rb.mass / 1000000).ToString("n5") + "g/cm³";
    }

    void win()
    {
        hasWon = true;
        audioM.Play("Win");
        playerMat.material.SetColor("_EmissionColor", playerMat.material.GetColor("_EmissionColor") * 30);
        winTextObject.SetActive(true);

        //audioM.Stop("Ambient");
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
        if (this.IsGrounded() && !hasWon)
        {
            // standard would be /100 but source is quiet
            audioSource.volume = rb.velocity.magnitude/800 * MathF.Log(rb.mass);
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


    //just before physics calculations, phys code here
    void FixedUpdate()
    {

        MoveRelToCamera();
        SetCountText();


        if (rb.position.y < -400)
        {
            Debug.Log("Dead");
            gameM.EndGame();
        }
        // 150000000 gramm pro kubikmeter
        // 13100000 aktuelle masse in gramm?
        // should be 150000000g or 150000kg bzw *10 wenn drone 10 kubikmeter hat
        // for some materials: *15Mio Pressure = *15Mio Temp

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            // TODO abflachende kurven für Feedback!
            // limited growth functions
            // N(t+1)=N(t)+k⋅(S−N(t))
            // is there some way without setting max? see emission

            audioM.Play("Collect");
            count += 1;
            other.gameObject.SetActive(false);
            //count += 1;
            rb.mass *= MASS_MULT;
            //rb.mass += initialMass;
            //speed += 1.8f * rb.mass/initialMass * speed;
            speed *= SPEED_MULT;
            // needs exp? because it starts lower than mass (prob)
            jumpForce *= JUMP_MULT;
            // lower drag more gravity to simulate weight
            rb.drag /= DRAG_DIV; 
            rb.angularDrag /= DRAG_DIV;
            rb.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            playerMat.material.SetColor("_EmissionColor", playerMat.material.GetColor("_EmissionColor") * (EMISSION_MULT + (EMISSION_MULT * 2 / (3*count))));

            if (rb.mass >= WIN_MASS)
            {
                Invoke("win", 0.2f);
            }
            else
            {
                winTextObject.SetActive(false);
            }

            //rb.gravityScale *= 2;
            //SetCountText();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player encountered");
            Debug.Log(other.gameObject.GetComponent<Rigidbody>().mass);
            if (other.gameObject.GetComponent<Rigidbody>().mass < 2 * rb.mass)
            {
                audioM.Play("Collect");
                count += 1;
                // TODO implement properly
                //rb.mass += other.gameObject.GetComponent<Rigidbody>().mass;
                other.gameObject.SetActive(false);
                audioM.Play("Collect");
                count += 1;
                other.gameObject.SetActive(false);
                //count += 1;
                rb.mass *= MASS_MULT;
                //rb.mass += initialMass;
                //speed += 1.8f * rb.mass/initialMass * speed;
                speed *= SPEED_MULT;
                // needs exp? because it starts lower than mass (prob)
                jumpForce *= JUMP_MULT;
                // lower drag more gravity to simulate weight
                rb.drag /= DRAG_DIV;
                rb.angularDrag /= DRAG_DIV;
                rb.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                playerMat.material.SetColor("_EmissionColor", playerMat.material.GetColor("_EmissionColor") * (EMISSION_MULT + (EMISSION_MULT * 2 / (3 * count))));

            }
        } else
        {
            collisionAudioSource.volume = other.impulse.magnitude / 1000000;
            collisionAudioSource.Play();
        }
    }

    public void becomeActive()
    {
        this.rb.isKinematic = false;
    }

}


