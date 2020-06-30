using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform cameraPos;
    [SerializeField] Transform speedCameraPos;
    [SerializeField] Transform slowCameraPos;
    [SerializeField] private float transitionSpeed = 6;
    Vector3 localOffset;

    void Update()
    {
        FollowsPlayer();
        CameraSpeed();
        CameraSlow();
    }

    private void FollowsPlayer()
    {
        if(!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
            localOffset = cameraPos.position;

        transform.position = Vector3.Lerp(transform.position, localOffset, Time.deltaTime * transitionSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, playerController.transform.rotation, Time.deltaTime * transitionSpeed);
    }

    private void CameraSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && playerController.speedMeter >= 0)
            localOffset = speedCameraPos.position;

        else if (playerController.speedMeter < 0)
            localOffset = cameraPos.position;
    }

    private void CameraSlow()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            localOffset = slowCameraPos.position;
    }
}
