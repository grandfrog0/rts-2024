using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    private int _teamID = -1;
    public int TeamID
    {
        get => _teamID;
        set
        {
            if (_enemyAI != null || TryGetComponent(out _enemyAI))
                _enemyAI.enabled = value != 0;

            _teamID = value; 
        }
    }

    private EnemyAI _enemyAI;
    private void Start()
    {
        _enemyAI = GetComponent<EnemyAI>();
    }
}
