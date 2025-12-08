using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField] Camera cam;

    public void Init(GenerationData data)
    {
        cam.orthographicSize = data.WorldSize / 2;
        // TODO: spawn bases circles;
    }
}