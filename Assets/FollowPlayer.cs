using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform player;

    [SerializeField] private int rotationSpeed;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = cameraTarget.position;

        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
    }
}
