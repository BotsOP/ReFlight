using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float leftRightSpeed;
    [SerializeField] float upDownSpeed;
    [SerializeField] float speed;
    [SerializeField] float momentumDelay;
    [SerializeField] float secondsToWait = 1;
    [SerializeField] float timesToCheckPerSec;

    float verticalInput;
    float horizontalInput;
    float updatedSpeed;
    float speedValue = 40;
    float resetAxisTimer = 0;
    public float speedTransition = 2;

    public float yValue;
    public float tempYValue;
    public float yAcceleration;

    bool shouldContinueCheck = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updatedSpeed = speedValue - yAcceleration;
        speed = Mathf.Lerp(speed, updatedSpeed, Time.deltaTime / speedTransition);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        CheckMovement();
        CheckIfResetRotation();

        SpeedUp();
        SlowDown();
        StartCoroutine("CalculateAccelerationY");
    }

    void FixedUpdate()
    {

    }

    public void CheckMovement()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

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

    public void CheckIfResetRotation()
    {
        if (verticalInput == 0 && horizontalInput == 0)
        {
            if (resetAxisTimer > secondsToWait)
            {
                ResetOrientation();
            }
            resetAxisTimer += Time.deltaTime;
        }
        else
        {
            resetAxisTimer = 0;
        }
    }

    public void ResetOrientation()
    {
        Vector3 forward = transform.forward;
        forward.Normalize();
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward), Time.deltaTime);
    }

    private IEnumerator CalculateAccelerationY()
    {
        if(shouldContinueCheck)
        {
            shouldContinueCheck = false;
            yValue = transform.position.y;

            yield return new WaitForSeconds(1 / timesToCheckPerSec);
            tempYValue = transform.position.y;

            yAcceleration = (tempYValue - yValue) / 0.1f;
            shouldContinueCheck = true;
        }
        
    }

    private void SpeedUp()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedValue = 100;
            speedTransition = 0.5f;
            if (speed > 150)
            {
                speed = 150;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speedValue = 40;
            speedTransition = 2f;
        }
            
    }

    private void SlowDown()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            speedValue = 25;
            speedTransition = 0.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            speedValue = 40;
            speedTransition = 2f;
        }

    }

}
