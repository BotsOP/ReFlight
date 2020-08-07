using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerHuman : MonoBehaviour
{
    public float speed;
    public float sprintSpeed;
    float walkSpeed;
    public float jumpForce;
    public float raycastDistance;
    public Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    public Vector3 standScale = new Vector3(1, 1, 1);
    private Rigidbody rb;
    PlayerController playerController;
    public GameObject pressEText;
    [SerializeField] GameObject respawnPoint;
    bool soundPlaying = true;
    float hAxis;
    float vAxis;

    private void Start()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Nature");
        pressEText.SetActive(false);
        walkSpeed = speed;
        rb = gameObject.GetComponent<Rigidbody>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        if(playerController.isHuman)
        {
            Jump();
            Sprint();
        }
    }

    private void FixedUpdate()
    {
        if(playerController.isHuman)
            Move();
    }

    private void Move()
    {
        if (hAxis != 0 && soundPlaying && IsGrounded() || vAxis != 0 && soundPlaying && IsGrounded())
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("Footsteps");
            soundPlaying = false;
        }
        else if (hAxis == 0 && vAxis == 0 || !IsGrounded())
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().StopSound("Footsteps");
            soundPlaying = true;
        }
            
        Vector3 movement = new Vector3(hAxis, 0, vAxis) * speed * Time.fixedDeltaTime;
        Vector3 newPos = rb.position + rb.transform.TransformDirection(movement);
        rb.MovePosition(newPos);
        

        
        
    }

    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (IsGrounded())
            {
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            }
        }
    }

    private void Sprint()
    {
        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            speed = sprintSpeed;
            if(hAxis != 0 && IsGrounded() || vAxis != 0 && IsGrounded())
            {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().SetPitch(1, "Footsteps");
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.W))
        {
            speed = walkSpeed;
            if(hAxis != 0 && IsGrounded() || vAxis != 0 && IsGrounded() || !Input.GetKeyDown(KeyCode.LeftShift) || !Input.GetKeyDown(KeyCode.W))
            {
                GameObject.Find("AudioManager").GetComponent<AudioManager>().SetPitch(0.8f, "Footsteps");
            }
        }
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector3.down * raycastDistance, Color.blue);
        return Physics.Raycast(transform.position, Vector3.down, raycastDistance); ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Sea")
        {
            transform.position = respawnPoint.transform.position;
        }
        if(other.gameObject.name == "GetInPlane")
        {
            pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "GetInPlane")
        {
            pressEText.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.name == "GetInPlane" && Input.GetKeyDown(KeyCode.E))
        {
            pressEText.SetActive(false);
            playerController.BecomePlane();
        }
    }

    
}
