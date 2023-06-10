using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendFlying : MonoBehaviour
{

    public float stability;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Enemy collided");

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player encountered");
            Debug.Log(other.gameObject.GetComponent<Rigidbody>().mass);
            if (other.impulse.magnitude > stability)
            {
                rb.isKinematic = false;
                //Object.Destroy(gameObject);
            }

            //rb.gravityScale *= 2;

        }
    }
}
