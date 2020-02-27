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
    [SerializeField] private GameObject axe;
    [SerializeField] private float swingDistance, swingIntensity;
    private bool animatingSwing, swingingDown;
    private AudioSource oufer;
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
        oufer = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        animatingSwing = false;
        swingingDown = true;
    }

    void Update()
    {
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
        if (onGround && rb.velocity.y < 0.1 && rb.velocity.y > -0.1)
        {
            rb.velocity += new Vector3(0f, Input.GetAxisRaw("Jump") * jumpStrength, 0f);
        }

        //Axe Animation
        if (Input.GetMouseButton(0))
            animatingSwing = true;

        if (animatingSwing)
        {
            if (swingingDown)
            {
                if (axe.transform.localRotation.eulerAngles.x < swingDistance)
                    axe.transform.localRotation = Quaternion.Euler(axe.transform.localRotation.eulerAngles.x + swingIntensity, axe.transform.localRotation.eulerAngles.y, 0f);
                else
                    swingingDown = false;
            }
            else
            {
                if (axe.transform.localRotation.eulerAngles.x < 350f)
                    axe.transform.localRotation = Quaternion.Euler(axe.transform.localRotation.eulerAngles.x - 5f, axe.transform.localRotation.eulerAngles.y, 0f);
                else
                {
                    axe.transform.localRotation = Quaternion.Euler(0f, axe.transform.localRotation.eulerAngles.y, 0f);
                    swingingDown = true;
                    animatingSwing = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blade"))
            oufer.Play();
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

