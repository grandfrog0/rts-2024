using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    public UnityEvent OnHealthChanged { get; } = new();
    public UnityEvent OnDead { get; } = new();

    [Header("Characteristics")]
    private bool _isDead = false;
    [SerializeField] float maxHealth;
    private float _health;
    public float MaxHealth => maxHealth;
    public float Health 
    { 
        get => _health;
        private set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChanged.Invoke();
        }
    }

    [SerializeField] float _attackStrength = 1;
    public float AttackStrength => _attackStrength;

    [Header("Main information")]
    public Sprite Icon;
    public string Name;
    [SerializeField] float _size = 1f;
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

    public void Attack(Entity other)
    {
        other.TakeDamage(_attackStrength, this);
    }
    public void TakeDamage(float value, Entity enemy)
    {
        Debug.Log($"{this} ({TeamID}) was damaged by {enemy} ({enemy.TeamID}). (Damage: {value}, start health: {Health})");
        Health -= value;

        if (Health <= 0 && !_isDead)
            Die(enemy);
    }
    protected virtual void Die(Entity enemy)
    {
        _isDead = true;

        //Debug.Log($"{this} ({TeamID}) was defeated by {enemy} ({enemy.TeamID}).");
        gameObject.SetActive(false);

        OnDead.Invoke();
        OnDead.RemoveAllListeners();
    }
}
