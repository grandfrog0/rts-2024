using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : Unit
{
    public void SetHealDestination(IHurtable hurtable)
    {
        _targetMovement.SetTarget(hurtable.Position);
        SetAttackTarget(hurtable);
        CurrentTask = UnitTask.Heal;
        //WaitingTask = UnitTask.None;

        hurtable.OnDead.AddListener(ClearCurrentTask);
    }
    public void Load(SerializableArcher unit)
    {
        Load(unit as SerializableUnit);

        Cooldown = unit.Cooldown;
        AttackStrength = -unit.Damage;
        MaxAttackRange = unit.MaxAttackRange;
        MinAttackDistance = unit.MinAttackRange;
    }
}
