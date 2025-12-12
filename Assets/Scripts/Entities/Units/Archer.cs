using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    public void Load(SerializableArcher unit)
    {
        Name = unit.Name;
        _targetMovement.Speed = unit.MovementSpeed;
        MaxHealth = unit.MaxHealth;
        TrainingCost = unit.TrainingCost;
        DetectionRange = unit.DetectionRange;

        Cooldown = unit.Cooldown;
        AttackStrength = unit.Damage;
        MaxAttackRange = unit.MaxAttackRange;
        MinAttackDistance = unit.MinAttackRange;
    }
}
