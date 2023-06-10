using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class enemyMovementOwnTracking : MonoBehaviour { 

    public float speed = 10;
    public float jumpForce = 500f;
    public float mass = 500;
    public float winCount = 13;
    public TMP_Text countText;
    public GameObject winTextObject;
    public GameObject player;
    public Vector3 jump;

    public Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private float movementZ;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        jump = new Vector3(0.0f, 2.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, movementZ, movementY);
        rb.AddForce(movement * speed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            rb.mass *= 2;
            speed *= 1.8f;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Enemy collided");
        
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player encountered");
            Debug.Log(other.gameObject.GetComponent<Rigidbody>().mass);
            if (other.gameObject.GetComponent<Rigidbody>().mass > 2 * rb.mass)
            {
                Debug.Log(other.gameObject.GetComponent<Rigidbody>().mass);
                gameObject.SetActive(false);
            }

            //rb.gravityScale *= 2;

        }
    }


}
