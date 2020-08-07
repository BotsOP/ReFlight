using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHumanCamera : MonoBehaviour
{
    public Camera humanCamera;
    public float lookSensitivity;
    public float smoothing;
    private GameObject Player;
    public Vector2 smoothedVelocity;
    public Vector2 currentLookingPos;
    public Vector2 inputValues;
    public float maxLookRotation = 85;
    PlayerController playerController;

    void Start()
    {
        humanCamera = GetComponent<Camera>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        Player = transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(playerController.isHuman)
        {
            humanCamera.enabled = true;
            RotateCamera();
        }
        else
        {
            humanCamera.enabled = false;
        }
            
        
    }

    private void RotateCamera()
    {
        
        inputValues = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        inputValues = Vector2.Scale(inputValues, new Vector2(lookSensitivity * smoothing, lookSensitivity * smoothing));
        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1f / smoothing);

        currentLookingPos += smoothedVelocity;

        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, -maxLookRotation, maxLookRotation);
        transform.localRotation = Quaternion.AngleAxis(-currentLookingPos.y, Vector3.right);
        Player.transform.localRotation = Quaternion.AngleAxis(currentLookingPos.x, Player.transform.up);
    }

    
}
