using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GamePreparer : MonoBehaviour
{
    [SerializeField] GenerationManager generationManager;
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] EntitySpawner spawner;
    [SerializeField] MapCamera mapCamera;
    [SerializeField] Transform floorTransform;
    [SerializeField] Inventory inventory;
    [SerializeField] NavMeshSurface surface;

    public void Prepare(GameConfig gameConfig)
    {
        spawner.Initialize();

        EntitySpawner.OnUnitsCountChanged.AddListener((int teamID, List<Unit> units) =>
        {
            if (teamID == 0)
                inventory.Init("units", units.Count);
        });

        generationManager.WorldSize = gameConfig.WorldSize;
        GenerationData generationData = generationManager.StartGeneration(gameConfig);
        cameraMovement.WorldSize = gameConfig.WorldSize;
        cameraMovement.transform.position = new Vector3(generationManager.PlayerBasePosition.x, 0, generationManager.PlayerBasePosition.y);
    
        mapCamera.Init(generationData);

        floorTransform.localScale = new Vector3(gameConfig.WorldSize * 2, floorTransform.localScale.y, gameConfig.WorldSize * 2);
        surface.BuildNavMesh();
    }
}
