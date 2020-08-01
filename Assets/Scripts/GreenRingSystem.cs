using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenRingSystem : MonoBehaviour
{
    [SerializeField] private Material ringMaterial;
    PlayerController player;
    [SerializeField] float minSpeed = 20f;
    public float thresholdSpeed = 80f;
    float playerSpeed;
    float LerpSpeed;
    Color test1;
    void Start()
    {
        LerpSpeed = thresholdSpeed - minSpeed;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        playerSpeed = player.speed;
        ringMaterial.color = Color.Lerp(Color.red, Color.green, (playerSpeed - minSpeed) / LerpSpeed);
    }
}
