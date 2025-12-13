using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    public void Load(SerializableArcher unit)
    {
        Load(unit as SerializableUnit);

        Cooldown = unit.Cooldown;
        AttackStrength = unit.Damage;
        MaxAttackRange = unit.MaxAttackRange;
        MinAttackDistance = unit.MinAttackRange;
    }
}
