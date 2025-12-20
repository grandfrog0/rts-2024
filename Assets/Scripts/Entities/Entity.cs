using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour, IHurtable
{
    public UnityEvent OnHealthChanged { get; } = new();
    public UnityEvent OnDead { get; } = new();

    [Header("Characteristics")]
    private bool _isDead = false;
    [SerializeField] float maxHealth;
    private float _health;
    public float MaxHealth
    {
        get => maxHealth;
        protected set => maxHealth = value;
    }
    public float Health
    {
        get => _health;
        protected set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChanged.Invoke();
        }
    }
    public float HealthPercent => Health / MaxHealth;
    public bool IsAlive => Health > 0;
    public virtual bool IsReady => IsAlive;
    public Vector3 Position => transform.position;

    [SerializeField] float _attackStrength = 1;
    public float AttackStrength { get => _attackStrength; protected set => _attackStrength = value; }

    [Header("Main information")]
    public string ConfigName;
    public Sprite Icon;
    public string Name;
    public EntityModelInfo Model;
    private int _teamID = -1;
    public int TeamID
    {
        get => _teamID;
        private set
        {
            if (_enemyAI != null || TryGetComponent(out _enemyAI))
                _enemyAI.enabled = value != 0;

            _teamID = value; 
        }
    }
    [SerializeField] int _attackPriority;
    public int AttackPriority => _attackPriority;

    private ArcherAI _enemyAI;
    public virtual void Init(int teamID)
    {
        _enemyAI = GetComponent<ArcherAI>();
        _health = maxHealth;
        TeamID = teamID;
    }

    public void Attack(IHurtable other)
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
        Invoke("Disactivate", 3);

        OnDead.Invoke();
        OnDead.RemoveAllListeners();
    }

    private void Disactivate()
        => gameObject.SetActive(false);
}
