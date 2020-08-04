using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float leftRightSpeed;
    [SerializeField] float upDownSpeed;
    public float speed;
    [SerializeField] float momentumDelay;
    [SerializeField] float secondsToWait = 1;
    [SerializeField] float timesToCheckPerSec;
    [SerializeField] Slider slider;
    [SerializeField] Animator anim;
    public bool isHuman = true;

    public GameObject[] planeUIs;

    float verticalInput;
    float horizontalInput;
    float sidewaysInput;
    public float updatedSpeed;
    float speedValue;
    float resetAxisTimer = 0;
    public float speedMeter = 200;
    float speedTransition = 2;
    public bool isAlive = true;
    float cachedTime;

    float yValue;
    float tempYValue;
    float yAcceleration;
    bool shouldContinueCheck = true;
    public bool letGo;

    float pitchWind;
    float pitchMotor;
    float volumeMotor;

    AudioManager audioManager;

    void Start()
    {
        GetAnim();
        speedValue = speed;
        audioManager = FindObjectOfType<AudioManager>();
        
    }

    void Update()
    {
        if(!isHuman)
        {
            updatedSpeed = speedValue - yAcceleration;
            speed = Mathf.Lerp(speed, updatedSpeed, Time.deltaTime / speedTransition);
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            pitchWind = Mathf.Lerp(1, 3, (speed - 40) / 80);
            audioManager.SetPitch(pitchWind, "Wind");

            CheckMovement();
            CheckIfResetRotation();

            SpeedUp();
            //SlowDown();
            StartCoroutine("CalculateAccelerationY");

            slider.value = speedMeter / 200;

            if(anim != null)
            {
                anim.SetFloat("VerMovement", verticalInput);
                anim.SetFloat("HorMovement", horizontalInput);
            }
        }
    }

    public void CheckMovement()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        sidewaysInput = Input.GetAxis("Sideways");

        if (verticalInput != 0 && isAlive)
        {
            MoveUpDown(verticalInput);
        }

        if (horizontalInput != 0 && isAlive)
        {
            MoveLeftRight(-horizontalInput);
        }

        if (sidewaysInput != 0 && isAlive)
        {
            MoveSideways(-sidewaysInput);
        }
    }

    public void BecomePlane()
    {
        isHuman = false;
        audioManager.Play("Wind");
        audioManager.Play("Motor");
        audioManager.StopSound("Nature");
        audioManager.StopSound("Footsteps");
        GameObject.Find("HumanCamera").GetComponent<AudioListener>().enabled = false;
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = true;

        foreach(GameObject planeUI in planeUIs)
        {
            Debug.Log(planeUI.name);
            planeUI.SetActive(true);
        }
    }

    public void BecomeHuman()
    {
        isHuman = true;
        audioManager.StopSound("Wind");
        audioManager.StopSound("Motor");
        GameObject.Find("Main Camera").GetComponent<AudioListener>().enabled = false;
        GameObject.Find("HumanCamera").GetComponent<AudioListener>().enabled = true;

        foreach(GameObject planeUI in planeUIs)
        {
            planeUI.SetActive(false);
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
        if (verticalInput == 0 && horizontalInput == 0 && sidewaysInput == 0)
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
        //forward.Normalize();
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
            if(Time.time - cachedTime > 2 && !letGo)
            {
                cachedTime = Time.time;
                letGo = true; 
            }
            
            pitchMotor = Mathf.Lerp(audioManager.ReturnPitch("Motor"), 1.2f, Time.time - cachedTime);
            volumeMotor = Mathf.Lerp(audioManager.ReturnVolume("Motor"), 0.3f, Time.time - cachedTime);
            audioManager.SetPitch(pitchMotor, "Motor");
            audioManager.SetVolume(volumeMotor, "Motor");

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
        else
        {
            if(Time.time - cachedTime > 1 && letGo)
            {
                cachedTime = Time.time;
                letGo = false; 
            }

            pitchMotor = Mathf.Lerp(audioManager.ReturnPitch("Motor"), 1, Time.time - cachedTime);
            volumeMotor = Mathf.Lerp(audioManager.ReturnVolume("Motor"), 0.2f, Time.time - cachedTime);
            audioManager.SetPitch(pitchMotor, "Motor");
            audioManager.SetVolume(volumeMotor, "Motor");
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

    public void GetAnim()
    {
        anim = transform.GetChild(5).gameObject.GetComponent<Animator>();
    }

}
