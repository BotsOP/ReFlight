using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionManager : MonoBehaviour
{
    public Text basicRingCounter;
    public Text greenRingCounter;
    public Text redRingCounter;
    public GameObject lastCollectedRing;
    public GameObject gameObjectChild;
    public GameObject plane;
    public GameObject spawnPoint;
    Vector3 spawnPos;
    int basicRingsCollected;
    int greenRingsCollected;
    int redRingsCollected;
    bool hitRing;
    float cachedTime;
    float ringPitch = 1;
    int amountOfChildren = 7;
    Rigidbody rb;
    AudioManager audioManager;
    PlayerController player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private IEnumerator coroutine;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent != null && other.transform.parent.transform.parent != null)
        {
            if(other.transform.parent.transform.parent.gameObject.tag == "BasicRing")
                {
                    RingCollected(other.transform.parent.transform.parent.gameObject);
                    coroutine = DeactivateRing(other.transform.parent.transform.parent.gameObject);
                    StartCoroutine(coroutine);
                    HitRing();
                }
            else if(other.transform.parent.transform.parent.gameObject.tag == "RedRing")
                {
                    RedRingSystem redRing = other.transform.parent.transform.parent.gameObject.transform.parent.gameObject.GetComponent<RedRingSystem>();
                    redRing.RedRingCollected();
                }
            else if(other.transform.parent.transform.parent.gameObject.tag == "GreenRing")
            {
                float greenRingThresholdSpeed = other.transform.parent.transform.parent.gameObject.GetComponent<GreenRingSystem>().thresholdSpeed;
                if(player.speed > greenRingThresholdSpeed)
                {
                    RingCollected(other.transform.parent.transform.parent.gameObject);
                    coroutine = DeactivateRing(other.transform.parent.transform.parent.gameObject);
                    StartCoroutine(coroutine);
                    HitRing();
                }
            }
            else if(other.gameObject.name != "GetInPlane")
                {
                    DeathFunction();
                }
        }
        else if(other.gameObject.name != "GetInPlane")
            {
                DeathFunction();
            }
        
    }

    public void RingCollected(GameObject ringHit)
    {
        lastCollectedRing = ringHit;
        if(ringHit.tag == "BasicRing")
            basicRingsCollected++;
        else if(ringHit.tag == "GreenRing")
            greenRingsCollected++;
        else if(ringHit.tag == "RedRing")
            redRingsCollected++;
        basicRingCounter.text = basicRingsCollected.ToString();
        greenRingCounter.text = greenRingsCollected.ToString();
        redRingCounter.text = redRingsCollected.ToString();
    }

    private IEnumerator DeactivateRing(GameObject gameObject)
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private void DeathFunction()
    {
        Debug.Log("GAME OVER");
        audioManager.Play("Explosion");
        audioManager.StopSound("Motor");
        audioManager.StopSound("Wind");

        player.isAlive = false;

        for (int i = 0; i < amountOfChildren; i++) 
        {
            gameObjectChild = transform.GetChild(5).gameObject.transform.GetChild(i).gameObject;
            rb = transform.GetChild(5).gameObject.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
            rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f)) * 100, ForceMode.VelocityChange);
        }

        transform.GetChild(3).gameObject.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);

        if(lastCollectedRing != null)
        {
            if(lastCollectedRing.tag == "BasicRing")
                basicRingsCollected--;
            else if(lastCollectedRing.tag == "GreenRing")
                greenRingsCollected--;
            else if(lastCollectedRing.tag == "RedRing")
                redRingsCollected--;

            basicRingCounter.text = basicRingsCollected.ToString();
            greenRingCounter.text = greenRingsCollected.ToString();
            redRingCounter.text = redRingsCollected.ToString();

            //fix bug where ring doesnt get reactivated if you die within one second
            lastCollectedRing.SetActive(true);
            
        }
        coroutine = AliveFunction();
        StartCoroutine(coroutine);
            
    }

    private IEnumerator AliveFunction()
    {
        Debug.Log("alive");
        yield return new WaitForSeconds(3f);
        audioManager.Play("Motor");
        audioManager.Play("Wind");

        transform.position = spawnPoint.transform.position;
        transform.rotation = spawnPoint.transform.rotation;
        Destroy(transform.GetChild(5).gameObject);
        spawnPos = transform.position;
        Instantiate(plane, spawnPoint.transform.position, spawnPoint.transform.rotation, transform);
        transform.GetChild(5).gameObject.transform.localRotation = Quaternion.identity;
        yield return new WaitForSeconds(2f);
        player.isAlive = true;
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);
        player.GetAnim();
    }

    public void HitRing()
    {
        cachedTime = Time.time;
        hitRing = true;
        ringPitch += 0.1f;
        audioManager.SetPitch(ringPitch, "RingCompletion");
        audioManager.Play("RingCompletion");
    }

    void Update()
    {
        if(hitRing && Time.time - cachedTime > 5)
        {
            ringPitch = 1;
        }
        if(!player.isAlive)
        {
            player.speed = 0;
        }
    }
}
