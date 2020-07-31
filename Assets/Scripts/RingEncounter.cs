using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingEncounter : MonoBehaviour
{
    public Text ringCounter;
    int ringsCollected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.transform.parent.transform.parent.gameObject.tag);
        if(other.transform.parent.transform.parent.gameObject.tag == "BasicRing")
        {
            ringsCollected++;
            ringCounter.text = ringsCollected.ToString();
            Destroy(other.transform.parent.transform.parent.gameObject, 1);
        }
        else 
        {
            Debug.Log("GAME OVER");
        }
    }
}
