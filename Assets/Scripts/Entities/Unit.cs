using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TargetMovement))]
public class Unit : Entity
{
    protected TargetMovement _targetMovement;

    public UnitTask CurrentTask { get; protected set; } = UnitTask.None;
    public UnitTask WaitingTask { get; set; } = UnitTask.None;

    public void SetDestination(Vector3 position)
    {
        _targetMovement.SetTarget(position);
        CurrentTask = UnitTask.Command;
        Debug.Log("Command got! Current task: " + CurrentTask);
        WaitingTask = UnitTask.None;
    }

    private void OnTargetGoaled()
    {
        CurrentTask = UnitTask.None;
        Debug.Log("Target goaled! Current task: " + CurrentTask);
    }

    protected override void Start()
    {
        base.Start();
        _targetMovement = GetComponent<TargetMovement>();
        _targetMovement.onTargetCompleted = OnTargetGoaled;
    }
}
