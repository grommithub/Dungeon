using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float mouseInputFactor;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Camera cam;
    [SerializeField] private float pushOut;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float runningFactor;
    private bool running;
    private Rigidbody rb;
    private bool colliding, onGround;
    private Vector2 mouseInput;
    private Vector2 movementInput;
    void Start()
    {
        running = false;
        mouseInput.x = mouseInput.y = 0f;
        movementInput.x = movementInput.y = 0f;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        print("On Ground: " + onGround + ", Colliding: " + colliding);

        //Handling Mouse Input
        mouseInput.x += mouseInputFactor * Input.GetAxis("Mouse X");
        mouseInput.y += mouseInputFactor * Input.GetAxis("Mouse Y");

        if (mouseInput.y > 89f) mouseInput.y = 89f;
        if (mouseInput.y < -89f) mouseInput.y = -89f;
        if (mouseInput.x > 180f) mouseInput.x -= 360f;
        if (mouseInput.x < -180f) mouseInput.x += 360f;

        //Handling WASD and shift Input
        running = Input.GetKey(KeyCode.LeftShift);
        movementInput.y = moveSpeed * (running && Input.GetAxis("Vertical") > 0 ? runningFactor : 1) * Input.GetAxis("Vertical");
        movementInput.x = moveSpeed * Input.GetAxis("Horizontal");

        
        //Horizontal Movement
        transform.rotation = Quaternion.Euler(0f, mouseInput.x, 0f);
        cam.transform.rotation = Quaternion.Euler(-mouseInput.y, mouseInput.x, 0f);
        rb.velocity = new Vector3(transform.forward.x * movementInput.y, rb.velocity.y, transform.forward.z * movementInput.y);
        rb.velocity += new Vector3(transform.forward.z * movementInput.x, 0f, -transform.forward.x * movementInput.x);

        //Vertical Movement
        if (onGround)
        {
            rb.AddForce(new Vector3(0f, Input.GetAxisRaw("Jump") * jumpStrength, 0f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
            onGround = true;
        else
            colliding = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
            onGround = false;
        else
            colliding = false;
    }
}

