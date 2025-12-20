using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Unit
{
    public float MineSpeed { get; private set; }
    public float RepairSpeed { get; private set; }
    public float RepairEfficiency { get; private set; }

    public Building WaitingBuildingTask { get; set; }

    private Building _fixTarget;
    private Coroutine _fixRoutine;


    public void SetMineDestination(Resource resource)
    {
        _targetMovement.SetTarget(resource.transform.position);
        CurrentTask = UnitTask.Mine;
        //WaitingTask = UnitTask.None;
        SetAttackTarget(resource);

        resource.OnDead.AddListener(ClearCurrentTask);
    }
    public void SetFixDestination(Building building)
    {
        _targetMovement.SetTarget(building.transform.position);
        CurrentTask = UnitTask.Fix;
        //WaitingTask = UnitTask.None;
        SetFixTarget(building);

        building.OnHealthChanged.AddListener(ClearFixTask);
    }
    public void Load(SerializableBuilder unit)
    {
        Load(unit as SerializableUnit);

        MineSpeed = unit.MineSpeed;
        RepairSpeed = unit.RepairSpeed;
        RepairEfficiency = unit.RepairEffeciency;

        MaxAttackRange = 3;
    }

    private void SetFixTarget(Building target)
    {
        _fixTarget = target;

        if (_fixRoutine == null)
            _fixRoutine = StartCoroutine(FixRoutine());

    }
    public void ClearFixTask()
    {
        if (_fixTarget != null && _fixTarget.HealthPercent < 1)
        {
            return;
        }

        if (_fixRoutine != null)
        {
            StopCoroutine(_fixRoutine);
            _fixRoutine = null;
        }

        InCooldown = false;
    }
    private IEnumerator FixRoutine()
    {
        while (true)
        {
            if (_fixTarget == null || !_fixTarget.IsAlive || _fixTarget.HealthPercent >= 1)
            {
                _fixRoutine = null;
                yield break;
            }

            Debug.Log($"{Vector3.Distance(transform.position, _fixTarget.Position)} < {MaxAttackRange}");
            if (Vector3.Distance(transform.position, _fixTarget.Position) < MaxAttackRange)
            {
                _fixTarget.TakeDamage(-RepairEfficiency, this);
                _animator.SetTrigger(attackAnimationName);

                InCooldown = true;
                yield return new WaitForSeconds(RepairEfficiency / RepairSpeed);
                InCooldown = false;
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
