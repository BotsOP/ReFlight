using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private int horizontalRotationSpeed;
    [SerializeField] private int verticalRotationSpeed;
    [SerializeField] private int sidewaysRotationSpeed;

    private float horizontalMove;
    [SerializeField] private float verticalMove;
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        MoveLeftRight();
        MoveUpDown();
        CheckZAxis();

        Debug.Log(transform.eulerAngles.z);
    }

    private void MoveLeftRight()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if(horizontalMove > 0 && y < 1.5f)
            y += 0.01f;
        else if(horizontalMove < 0 && y > -1.5f)
            y -= 0.01f;

        else
        {
            if (y < 2 && y > 0)
                y -= 0.001f;
            if (y > -2 && y < 0)
                y += 0.001f;
        }
            
        transform.Rotate(Vector3.up * y * horizontalRotationSpeed * Time.deltaTime);
    }

    private void MoveUpDown()
    {
        verticalMove = Input.GetAxisRaw("Vertical");
        if (verticalMove > 0 && x < 1)
            x += 0.01f;
        else if (verticalMove < 0 && x > -1)
            x -= 0.01f;
        else
        {
            if (x < 2 && x > 0)
                x -= 0.001f;
            if (x > -2 && x < 0)
                x += 0.001f;
        }
            
        transform.Rotate(Vector3.right * x * verticalRotationSpeed * Time.deltaTime);
    }

    private void CheckZAxis()
    {
        //Als de speler de linker of rechter toets heeft ingedrukt gaat de z rotatie zich daaran aanpassen
        if (y > 0.1 && z > -9)
            z -= 0.5f;
        else if (y < -0.1 && z < 9)
            z += 0.5f;

        else
        {
            //Als de speler geen toetsen indrukt gaat de z variable terug naar 0
            if (z < 10 && z > 0)
                z -= 0.1f;
            else if (z > -10 && z < 0)
                z += 0.1f;

            //Als de speler niet de linker of rechter toets heeft ingedrukt gaat de z rotatie terug naar 0
            if (transform.eulerAngles.z > 0.1 && transform.eulerAngles.z < 180 && z < 0.1)
                transform.Rotate(Vector3.forward * -10 * sidewaysRotationSpeed * Time.deltaTime);
            else if (360 - transform.eulerAngles.z > 0.1 && 360 - transform.eulerAngles.z < 180 && z > -0.1)
                transform.Rotate(Vector3.forward * 10 * sidewaysRotationSpeed * Time.deltaTime);
        }

        transform.Rotate(Vector3.forward * z * sidewaysRotationSpeed * Time.deltaTime);
    }
}
