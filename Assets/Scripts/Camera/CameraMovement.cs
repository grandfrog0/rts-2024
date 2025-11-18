using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float worldSize;
    [SerializeField] float speed;
    [SerializeField] float scrollStrength;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float minView, maxView;
    private Camera _camera;
    public int WorldSize { get; set; }

    public void SetAxis(Vector2 axis)
    { 
        cameraTransform.position = new Vector3(
            Mathf.Clamp(cameraTransform.position.x + axis.x * speed * Time.deltaTime, -WorldSize / 2 + _camera.fieldOfView, WorldSize / 2 - _camera.fieldOfView), 
            cameraTransform.position.y, 
            Mathf.Clamp(cameraTransform.position.z + axis.y * speed * Time.deltaTime, -WorldSize / 2 + _camera.fieldOfView, WorldSize / 2 - _camera.fieldOfView)
            );
    }
    public void AddFieldOfView(float value)
    {
        _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView + value, minView, maxView);
    }
    private void Start()
    {
        _camera = Camera.main;
    }
}
