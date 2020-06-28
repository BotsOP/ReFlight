using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float leftRightSpeed = 60;
    [SerializeField] float upDownSpeed = 80;
    [SerializeField] float speed = 30;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        CheckMovement();
    }

    public void CheckMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput != 0)
        {
            MoveUpDown(verticalInput);
        }

        if (horizontalInput != 0)
        {
            MoveLeftRight(-horizontalInput);
        }
    }

    public void MoveUpDown(float addValue)
    {
        addValue *= Time.deltaTime * upDownSpeed;

        Quaternion rotator = Quaternion.AngleAxis(addValue, transform.right);
        transform.rotation = rotator * transform.rotation;
    }

    public void MoveLeftRight(float addValue)
    {
        addValue *= Time.deltaTime * leftRightSpeed;

        Quaternion rotator = Quaternion.AngleAxis(addValue, transform.forward);
        transform.rotation = rotator * transform.rotation;
    }
}
