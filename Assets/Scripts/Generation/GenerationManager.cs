using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [SerializeField] int worldSize = 256;
    [SerializeField] float baseRange = 30;
    [SerializeField] ResourceGenerator resourceGenerator;
    [SerializeField] BaseGenerator baseGenerator;
    public int WorldSize => worldSize;
    public Vector2 PlayerBasePosition => baseGenerator.BasesPosition[0];
    public void StartGeneration()
    {
        baseGenerator.Initialize(worldSize, baseRange);
        resourceGenerator.Initialize(worldSize, baseGenerator.BasesPosition, baseRange);
    }
}
