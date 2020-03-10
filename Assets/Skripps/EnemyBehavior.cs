using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource dink;
    [SerializeField] private GameObject axe;
    [SerializeField] private float swingDistance, swingIntensity;
    private bool animatingSwing, swingingDown;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animatingSwing = false;
        swingingDown = true;
        dink = GetComponent<AudioSource>();
    }

    void Update()
    {
        rb.velocity = new Vector3(transform.forward.x * walkSpeed, rb.velocity.y, transform.forward.z * walkSpeed);
        Turn();
        if ((player.transform.position - transform.position).magnitude < 1.5f)
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

    private void Turn()
    {
        transform.forward = Vector3.Lerp(transform.forward, (player.transform.position - transform.position).normalized, turnSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blade"))
        {
            dink.loop = true;
            dink.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Blade"))
            dink.loop = false;
    }
}
