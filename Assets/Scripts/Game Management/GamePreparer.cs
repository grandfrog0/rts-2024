using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePreparer : MonoBehaviour
{
    [SerializeField] GenerationManager generationManager;
    [SerializeField] CameraMovement cameraMovement;

    private void Start()
    {
        generationManager.StartGeneration();
        cameraMovement.WorldSize = generationManager.WorldSize;
        cameraMovement.transform.position = new Vector3(generationManager.PlayerBasePosition.x, 0, generationManager.PlayerBasePosition.y);
    }
}
