using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    [SerializeField] CameraMovement cameraMovement;
    private void Update()
    {
        Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        cameraMovement.SetAxis(axis);

        float scrollValue = Input.GetAxis("Mouse Scroll Y");
        if (scrollValue != 0f)
        {
            cameraMovement.AddFieldOfView(scrollValue);
        }
    }
}
