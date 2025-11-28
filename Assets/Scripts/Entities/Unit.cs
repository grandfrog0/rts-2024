using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TargetMovement))]
public class Unit : Entity
{
    protected TargetMovement _targetMovement;

    public UnitState State { get; protected set; } = UnitState.Waiting;
    protected Entity _targetEnemy;

    public void SetTarget(Entity targetEnemy)
    {
        _targetEnemy = targetEnemy;
        _targetMovement.SetTarget(targetEnemy.transform.position);
    }

    private void Start()
    {
        _targetMovement = GetComponent<TargetMovement>();
    }
}
