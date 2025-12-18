using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    [SerializeField] Camera cam;

    [SerializeField] Transform basesParent;
    [SerializeField] Vector3 basesOffset;
    [SerializeField] Renderer teamBase;

    public void Init(GenerationData data, GameConfig config)
    {
        cam.orthographicSize = data.WorldSize / 2;

        var groups = FindObjectsOfType<LODGroup>();
        foreach (LODGroup group in groups)
            group.ForceLOD(0);

        List<Vector3> basesPositions = data.GetBases().Select(v => new Vector3(v.x, 0, v.y)).ToList();
        List<EnemyData> teamDatas = config.GetEntitiesData();
        float scaleMultiplier = 1 + 10 * (data.WorldSize - 2000) / 8000;
        for (int i = 0; i < basesPositions.Count; i++)
        {
            Renderer r = Instantiate(teamBase, basesPositions[i] + basesOffset, teamBase.transform.rotation, basesParent);
            r.material.color = teamDatas[i].Color;
        }

        cam.Render();

        foreach (LODGroup group in groups)
            group.ForceLOD(-1);

        cam.targetTexture = null; 
        gameObject.SetActive(false);
    }
}