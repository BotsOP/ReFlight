using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Vector3 playerOffset = new Vector3(0, 8, -15);
    [SerializeField] private float transitionSpeed = 6;

    void Update()
    {
        Vector3 localOffset = playerController.transform.right * playerOffset.x + playerController.transform.up * playerOffset.y + playerController.transform.forward * playerOffset.z;

        Vector3 desiredPosition = playerController.transform.position + localOffset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * transitionSpeed);

        transform.rotation = Quaternion.Lerp(transform.rotation, playerController.transform.rotation, Time.deltaTime * transitionSpeed);
    }
}
