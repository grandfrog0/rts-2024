using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePreparer : MonoBehaviour
{
    [SerializeField] GenerationManager generationManager;
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] EntitySpawner spawner;
    [SerializeField] MapCamera mapCamera;
    [SerializeField] Transform floorTransform;

    public void Prepare(GameConfig gameConfig)
    {
        spawner.Initialize();

        generationManager.WorldSize = gameConfig.WorldSize;
        GenerationData generationData = generationManager.StartGeneration(gameConfig);
        cameraMovement.WorldSize = gameConfig.WorldSize;
        cameraMovement.transform.position = new Vector3(generationManager.PlayerBasePosition.x, 0, generationManager.PlayerBasePosition.y);
    
        mapCamera.Init(generationData);

        floorTransform.localScale = new Vector3(gameConfig.WorldSize * 2, floorTransform.localScale.y, gameConfig.WorldSize * 2);
    }
}
