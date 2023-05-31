using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    public float height;
    public float distance;
    public float turnSpeed = 10;

    private Vector3 offsetX;
    private Vector3 offsetY;

    void Start()
    {
        height = transform.position.y;
        distance = transform.position.z;
        offsetX = new Vector3(0, height, distance);
        offsetY = new Vector3(0, 0, distance);
    }

    void LateUpdate()
    {
        offsetX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offsetX;
        offsetY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * turnSpeed, Vector3.right) * offsetY;
        transform.position = player.transform.position + offsetX + offsetY;
        transform.LookAt(player.transform.position);
    }

    /*
    private Vector3 offset;
    public float sensitivity = 10f; // The camera movement sensitivity

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.transform.position.x;
        
    }

    // Update is called once per frame
    // LateUpdate runs last
    void LateUpdate()
    {


        // Get the input for camera movement
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector3.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = Vector3.down;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector3.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = Vector3.left;
        }

        //transform.RotateAround(player.transform.position, direction, sensitivity * Time.deltaTime);
        //transform.position = player.transform.position + offset;
    }
    */

}
