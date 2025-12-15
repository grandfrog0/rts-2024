using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Unit
{
    public float MineSpeed { get; private set; }
    public float RepairSpeed { get; private set; }
    public float RepairEffeciency { get; private set; }

    public void SetMineDestination(Resource resource)
    {
        _targetMovement.SetTarget(resource.transform.position);
        CurrentTask = UnitTask.Mine;
        WaitingTask = UnitTask.None;
        SetAttackTarget(resource);

        resource.OnDead.AddListener(ClearCurrentTask);
    }
    public void Load(SerializableBuilder unit)
    {
        Load(unit as SerializableUnit);

        MineSpeed = unit.MineSpeed;
        RepairSpeed = unit.RepairSpeed;
        RepairEffeciency = unit.RepairEffeciency;

        MaxAttackRange = 3;
    }
}
