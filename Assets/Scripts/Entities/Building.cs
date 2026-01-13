using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Building : Entity
{
    public UnityEvent onBuilt = new();
    public bool IsBuilt { get; set; } = true;
    public override bool IsReady => IsAlive && IsBuilt;
    public void PrepareToBuild()
    {
        Health = 1;
        IsBuilt = false;

        OnHealthChanged.AddListener(CheckHealth);
    }
    private void CheckHealth()
    {
        if (HealthPercent >= 1)
        {
            IsBuilt = true;
            OnHealthChanged.RemoveListener(CheckHealth);
            onBuilt.Invoke();
        }
    }

}
