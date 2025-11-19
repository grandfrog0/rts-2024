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
            Mathf.Clamp(cameraTransform.position.x + axis.x * speed * Time.deltaTime, -WorldSize / 2 + _camera.transform.localPosition.magnitude, WorldSize / 2 - _camera.transform.localPosition.magnitude), 
            cameraTransform.position.y, 
            Mathf.Clamp(cameraTransform.position.z + axis.y * speed * Time.deltaTime, -WorldSize / 2 + _camera.transform.localPosition.magnitude, WorldSize / 2 - _camera.transform.localPosition.magnitude)
            );
    }
    public void AddZoom(float value)
    {
        _camera.transform.localPosition = _camera.transform.localPosition.normalized * Mathf.Clamp(_camera.transform.localPosition.magnitude + value * scrollStrength * Time.deltaTime, minView, maxView);
    }
    private void Start()
    {
        _camera = Camera.main;
    }
}
