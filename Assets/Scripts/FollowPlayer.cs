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
    Camera mainCamera;
    Vector3 localOffset;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if(!playerController.isHuman)
        {
            mainCamera.enabled = true;
            FollowsPlayer();
            CameraSpeed();
            //CameraSlow();
        }
        else
        {
            mainCamera.enabled = false;
        }
        
    }

    private void FollowsPlayer()
    {
        if(!Input.GetKey(KeyCode.LeftShift))
            localOffset = cameraPos.position;

        transform.position = Vector3.Lerp(transform.position, localOffset, Time.deltaTime * transitionSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, playerController.transform.rotation, Time.deltaTime * transitionSpeed);
    }

    private void CameraSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) && playerController.currentExtraSpeed >= 0)
            localOffset = speedCameraPos.position;

        else if (playerController.currentExtraSpeed < 0)
            localOffset = cameraPos.position;
    }

    private void CameraSlow()
    {
        if (Input.GetKey(KeyCode.LeftControl))
            localOffset = slowCameraPos.position;
    }
}
