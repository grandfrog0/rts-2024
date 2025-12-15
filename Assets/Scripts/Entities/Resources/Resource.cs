using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour, IHurtable
{
    public UnityEvent OnHealthChanged { get; } = new();
    public UnityEvent OnDead { get; } = new();

    [Header("Characteristics")]
    private bool _isBroken = false;
    [SerializeField] float maxHealth = 10;
    private float _health;
    public float MaxHealth
    {
        get => maxHealth;
        protected set => maxHealth = value;
    }
    public float Health
    {
        get => _health;
        private set
        {
            _health = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChanged.Invoke();
        }
    }

    public bool IsAlive => Health > 0;
    public Vector3 Position => transform.position;

    public string ResourceName;
    [SerializeField] float _size = 1f;
    public float Size => _size;
    private void Start()
    {
        _health = maxHealth;
    }

    public void TakeDamage(float value, Entity enemy)
    {
        Health -= value;
        Debug.Log($"{this} was damaged. ({Health})");

        if (Health <= 0 && !_isBroken)
            BreakResource();
    }
    protected virtual void BreakResource()
    {
        _isBroken = true;

        Inventory.instance.AddCount(ResourceName, 1);
        gameObject.SetActive(false);

        OnDead.Invoke();
        OnDead.RemoveAllListeners();
    }
}
