using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Entity
{
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
        }
    }

}
