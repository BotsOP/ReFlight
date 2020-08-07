using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedRingSystem : MonoBehaviour
{
    [SerializeField] float countDownDuration;
    [SerializeField] private Material ringMaterial;
    [SerializeField] Color beginColor;
    [SerializeField] Color EndColor;
    CollisionManager collisionManager;
    public int ringsCollected;
    int amountRings;
    bool shouldCountDown;
    float startTime;
    bool firstTimeCountdown = true;
    private IEnumerator coroutine;

    void Start()
    {
        collisionManager = GameObject.Find("Player").GetComponent<CollisionManager>();
        amountRings = transform.childCount;
        for (int i = 1; i < amountRings; i++) 
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(shouldCountDown)
        {
            ringMaterial.color = Color.Lerp(beginColor, EndColor, (Time.time - startTime) / countDownDuration);
            if(Time.time - startTime > countDownDuration)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                firstTimeCountdown = true;
                ringsCollected = 0;
                for (int i = 1; i < amountRings; i++) 
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
            }
        }
    }

    public void RedRingCollected()
    {
        FirstEncounter();
        if (ringsCollected < amountRings - 1)
        {
            collisionManager.HitRing();
            coroutine = DeactivateRing(transform.GetChild(ringsCollected).gameObject);
            StartCoroutine(coroutine);
            //transform.GetChild(ringsCollected).gameObject.SetActive(false);
            ringsCollected++;
            transform.GetChild(ringsCollected).gameObject.SetActive(true);
        }
        else
            CompletedRedRing();
    }
    private IEnumerator DeactivateRing(GameObject gameObject)
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private IEnumerator DelayedSoundEffect()
    {
        yield return new WaitForSeconds(1f);
        FindObjectOfType<AudioManager>().Play("RedRingCompletion");
    }

    private void FirstEncounter()
    {
        if(firstTimeCountdown)
        {
            startTime = Time.time;
            shouldCountDown = true;

            for (int i = 0; i < amountRings; i++) 
                {
                    transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.transform.GetChild(6).gameObject.SetActive(false);
                    transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.transform.GetChild(7).gameObject.SetActive(true);
                }

            firstTimeCountdown = false;
        }
    }

    private void CompletedRedRing()
    {
        Debug.Log("completed ring");
        collisionManager.RingCollected(gameObject);
        collisionManager.HitRing();
        StartCoroutine("DelayedSoundEffect");
        
        coroutine = DeactivateRing(transform.GetChild(amountRings - 1).gameObject);
        StartCoroutine(coroutine);
        coroutine = DeactivateRing(gameObject);
        StartCoroutine(coroutine);
        coroutine = DeactivateRing(transform.GetChild(0).gameObject);
        StartCoroutine(coroutine);
        
    }
}
