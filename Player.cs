using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float X;
    private float Y;
    private float speed;
    private bool grounded, sliding = false;


    public float Sensitivity = 180, walkSpeed = 10, runSpeed = 14, crouchSpeed = 5, jumpForce = 500, slideSpeed = 1300;
    public bool canMove = true;

    public Transform cam;

    void Awake()
    {
        Vector3 euler = transform.rotation.eulerAngles;
        X = euler.x;
        Y = euler.y;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        speed = walkSpeed;
    }

    void Update()
    {
        const float MIN_X = 0.0f;
        const float MAX_X = 360.0f;
        const float MIN_Y = -90.0f;
        const float MAX_Y = 90.0f;
        float xMov = Input.GetAxisRaw("Vertical");
        float yMov = Input.GetAxisRaw("Horizontal");

        X += Input.GetAxis("Mouse X") * (Sensitivity * Time.deltaTime);
        if (X < MIN_X) X += MAX_X;
        else if (X > MAX_X) X -= MAX_X;
        Y -= Input.GetAxis("Mouse Y") * (Sensitivity * Time.deltaTime);
        if (Y < MIN_Y) Y = MIN_Y;
        else if (Y > MAX_Y) Y = MAX_Y;

        transform.rotation = Quaternion.Euler(0, X, 0.0f);
        cam.rotation = Quaternion.Euler(Y, X, 0.0f);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else if(speed != crouchSpeed)
        {
            speed = walkSpeed;
        }
        
        if(xMov != 0 && canMove)
        {
            transform.position += transform.forward * xMov * speed * Time.deltaTime;
        }
        if (yMov != 0 && canMove)
        {
            transform.position += transform.right * yMov * speed * Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            grounded = false;
            GetComponent<Rigidbody>().AddForce(0, jumpForce, 0);
        }
        if(Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            if(speed > walkSpeed)
            {
                GetComponent<Rigidbody>().AddForce(transform.forward * slideSpeed);
                canMove = false;
                sliding = true;
            }
            transform.localScale = new Vector3(1, 0.5f, 1);
            speed = crouchSpeed;
        }
        if(Input.GetKeyUp(KeyCode.LeftControl))
        {
            if(sliding)
            {
                canMove = true;
                sliding = false;
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
            transform.localScale = new Vector3(1, 1, 1);
            speed = walkSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("NonClimbable"))
            grounded = true;
    }
}
