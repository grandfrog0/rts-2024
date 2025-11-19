using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] float baseRange = 30;
    [SerializeField] ResourceGenerator resourceGenerator;
    [SerializeField] BaseGenerator baseGenerator;
    public int WorldSize { get; set; }
    public Vector2 PlayerBasePosition => baseGenerator.BasesPosition[0];
    public void StartGeneration(GameConfig gameConfig)
    {
        baseGenerator.Initialize(gameConfig, baseRange);
        resourceGenerator.Initialize(WorldSize, baseGenerator.BasesPosition, baseRange);
    }
}
