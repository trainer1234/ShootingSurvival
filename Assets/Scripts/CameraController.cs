using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 offsetPos, offsetRotation;
    //public float rotationSpeed = 10.0F;

    //Transform cameraTransform;

    // Use this for initialization
    void Start()
    {
        offsetPos = transform.position - player.transform.position;
        //cameraTransform = Camera.main.transform;
    }

    /*public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationY = 0F;

    void Update()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }

        else if (axes == RotationAxes.MouseX)
        {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }

        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
            transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        }
    }*/

    public float mouseSensitivity = 3.0f; //Mouse sensitivity.
     //public float viewRange = 20.0f;
     //float rotVertical = 0;
     
     // Update is called once per frame
     void Update()
     {
         //Camera
         float rotHorizontal = Input.GetAxisRaw("Mouse X") * mouseSensitivity;

         transform.Rotate(0, rotHorizontal, 0);


         /*rotVertical = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

         rotVertical = Mathf.Clamp(rotVertical, -viewRange, viewRange);
         cameraTransform.localEulerAngles = new Vector3(Mathf.Clamp(
             cameraTransform.localEulerAngles.x, -viewRange, viewRange), 0, 0);

         cameraTransform.localRotation *= Quaternion.Euler(-rotVertical, 0, 0);*/

     }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = offsetPos + player.transform.position;
    }
}
