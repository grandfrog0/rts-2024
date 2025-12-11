using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inputs : MonoBehaviour
{
    [SerializeField] UnityEvent onMouse0Down;
    [SerializeField] UnityEvent onMouse0Up;
    [SerializeField] UnityEvent onMouse0;

    [SerializeField] CameraMovement cameraMovement;
    private void Update()
    {
        Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        cameraMovement.SetAxis(axis);

        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue != 0f)
        {
            cameraMovement.AddZoom(scrollValue);
        }

        if (Input.GetMouseButtonDown(0))
            onMouse0Down.Invoke();
        if (Input.GetMouseButtonUp(0))
            onMouse0Up.Invoke();
        if (Input.GetMouseButton(0))
            onMouse0.Invoke();
    }
}
