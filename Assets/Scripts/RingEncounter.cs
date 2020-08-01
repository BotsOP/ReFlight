using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingEncounter : MonoBehaviour
{
    public Text ringCounter;
    int ringsCollected;
    PlayerController player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.parent.transform.parent.gameObject.tag);
        if(other.transform.parent.transform.parent.gameObject.tag == "BasicRing")
        {
            ringsCollected++;
            ringCounter.text = ringsCollected.ToString();
            Destroy(other.transform.parent.transform.parent.gameObject, 1);
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
                Destroy(other.transform.parent.transform.parent.gameObject, 1);
            }
        }
        else 
        {
            Debug.Log("GAME OVER");
        }
    }
}
