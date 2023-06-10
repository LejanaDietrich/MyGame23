using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tilter : MonoBehaviour
{
    public float turnSpeed = 5.0f;
    public float tiltSpeed = 50.0f;
    public float maxAngle = 0.25f;
    public GameObject player;
    public PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Player").GetComponent<PlayerController>().rb.mass > 1000000)
        {

            if (Input.GetAxis("Horizontal") > 0.0)
            { //if the right arrow is pressed (negative z)
                transform.Rotate(0.0f, turnSpeed * Time.deltaTime, -tiltSpeed * Time.deltaTime, Space.World); //and then turn the plane
            }
            if (Input.GetAxis("Horizontal") < 0.0)
            { //if the left arrow is pressed
                if (gameObject.transform.localRotation.z >= -maxAngle)
                {
                    transform.Rotate(0.0f, -turnSpeed * Time.deltaTime, tiltSpeed * Time.deltaTime, Space.World);

                }
            }
            if (Input.GetAxis("Vertical") > 0.0)
            { //if the up arrow is pressed
                if (gameObject.transform.localRotation.x <= maxAngle)
                {
                    transform.Rotate(tiltSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
            }
            if (Input.GetAxis("Vertical") < 0.0)
            { //if the down arrow is pressed
                if (gameObject.transform.localRotation.x >= -maxAngle)
                {
                    transform.Rotate(-tiltSpeed * Time.deltaTime, 0.0f, 0.0f);
                }
            }
        }
    }
}
