using System.Collections.Generic;
using UnityEngine;

public class GenerationData
{
    public int WorldSize;

    public Vector2 PlayerBase;
    public List<Vector2> EnemyBases;

    public List<Vector2> GetBases()
    {
        List<Vector2> bases = new List<Vector2>(EnemyBases);
        bases.Insert(0, PlayerBase);
        return bases;
    }
}