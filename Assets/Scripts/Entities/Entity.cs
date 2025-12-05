using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    public UnityEvent OnHealthChanged { get; } = new();
    [SerializeField] float maxHealth;
    private float _health;
    public float MaxHealth => maxHealth;
    public float Health => _health;

    public Sprite Icon;
    public string Name;
    [SerializeField] float _size = 7.5f;
    public float Size => _size;
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
    protected virtual void Start()
    {
        _enemyAI = GetComponent<EnemyAI>();
        _health = maxHealth;
    }
}
