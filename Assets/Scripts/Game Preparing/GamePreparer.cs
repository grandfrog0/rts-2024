using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePreparer : MonoBehaviour
{
    [SerializeField] GenerationManager generationManager;
    [SerializeField] CameraMovement cameraMovement;

    public void Prepare(GameConfig gameConfig)
    {
        generationManager.WorldSize = gameConfig.WorldSize;
        generationManager.StartGeneration(gameConfig);
        cameraMovement.WorldSize = gameConfig.WorldSize;
        cameraMovement.transform.position = new Vector3(generationManager.PlayerBasePosition.x, 0, generationManager.PlayerBasePosition.y);
    }
}
