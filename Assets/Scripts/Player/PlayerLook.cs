using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private float xSensitivity = 30f;
    [SerializeField] private float ySensitivity = 30f;


    [SerializeField] private Camera cam;
    private float xRotation = 0f;
    private void Awake()
    {
    }

    public void ProcessLook(Vector2 input)
    {

        float mouseX = input.x;
        float mouseY = input.y;

        xRotation = mouseY * Time.deltaTime * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        //Debug.Log(xRotation);
        //Debug.Log(cam.transform.localRotation.eulerAngles.x);
        float camLocalRotationX = cam.transform.localRotation.eulerAngles.x;
        //Debug.Log(camLocalRotationX);
        if (cam.transform.localRotation.eulerAngles.x >= 180)
        {
            camLocalRotationX -= 360;
        }

        if (camLocalRotationX - xRotation > 85)
        {
            cam.transform.localRotation = Quaternion.Euler(85, 0, 0);
        }
        else if (camLocalRotationX - xRotation < -85)
        {
            cam.transform.localRotation = Quaternion.Euler(-85, 0, 0);
        }
        else
        {
            cam.transform.Rotate(Vector3.left * xRotation);
        }
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);


    }
}
