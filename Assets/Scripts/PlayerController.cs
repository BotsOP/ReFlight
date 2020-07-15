using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float leftRightSpeed;
    [SerializeField] float upDownSpeed;
    [SerializeField] float speed;
    [SerializeField] float momentumDelay;
    [SerializeField] float secondsToWait = 1;
    [SerializeField] float timesToCheckPerSec;
    [SerializeField] Slider slider;

    float verticalInput;
    float horizontalInput;
    float sidewaysInput;
    float updatedSpeed;
    float speedValue = 40;
    float resetAxisTimer = 0;
    public float speedMeter = 200;
    float speedTransition = 2;

    float yValue;
    float tempYValue;
    float yAcceleration;

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

        slider.value = speedMeter / 200;
    }

    void FixedUpdate()
    {

    }

    public void CheckMovement()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        sidewaysInput = Input.GetAxis("Sideways");

        if (verticalInput != 0)
        {
            MoveUpDown(verticalInput);
        }

        if (horizontalInput != 0)
        {
            MoveLeftRight(-horizontalInput);
        }

        if (sidewaysInput != 0)
        {
            MoveSideways(-sidewaysInput);
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

        Quaternion rotator = Quaternion.AngleAxis(addValue, transform.up * -1);
        Quaternion rotator2 = Quaternion.AngleAxis(addValue * 0.5f, transform.forward);
        transform.rotation = rotator * transform.rotation;
        transform.rotation = rotator2 * transform.rotation;

    }

    public void MoveSideways(float addValue)
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
        StartCoroutine(CheckSpeedMeter());
        if (speedMeter > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            speedValue = 100;
            speedTransition = 0.5f;
            if (speed > 150)
            {
                speed = 150;
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || speedMeter < 0)
        {
            speedValue = 40;
            speedTransition = 2f;
        }
            
    }

    private IEnumerator CheckSpeedMeter()
    {
        if (Input.GetKey(KeyCode.LeftShift) && speedMeter >= -1)
        {
            speedMeter = Mathf.Clamp(speedMeter, 0, 200);
            speedMeter --;
        }
        else if (speedMeter < 200)
        {
            yield return new WaitForSeconds(1);
            if(speedMeter < 200)
                speedMeter += 2;
            if (speedMeter == 201)
                speedMeter = 200;
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
