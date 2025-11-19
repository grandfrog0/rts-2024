using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig
{
    public EnemyData PlayerData;
    public List<EnemyData> EnemiesData = new List<EnemyData>();
    public int Hardness = 0;
    public int WorldSize = 2500; // 2500 - 10_000
}
