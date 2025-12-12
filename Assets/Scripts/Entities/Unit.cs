using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(TargetMovement))]
public class Unit : Entity
{
    // Characteristics
    [SerializeField] float _attackRange = 1;
    public float MaxAttackRange { get => _attackRange; protected set => _attackRange = value; }
    public float MinAttackDistance { get; protected set; }
    [SerializeField] float _cooldown = 1;
    public float Cooldown { get => _cooldown; protected set => _cooldown = value; }

    // CD
    public bool InCooldown { get; private set; }
    private Coroutine _attackRoutine;
    private Entity _attackTarget;

    // Misc
    public List<TrainingCost> TrainingCost { get; set; }
    public float DetectionRange { get; set; }

    // Tasks
    public UnitTask CurrentTask { get; protected set; } = UnitTask.None;
    public UnitTask WaitingTask { get; set; } = UnitTask.None;

    // Movement logic
    protected TargetMovement _targetMovement;
    public void SetDestination(Vector3 position)
    {
        _targetMovement.SetTarget(position);
        CurrentTask = UnitTask.Command;
        WaitingTask = UnitTask.None;
    }
    private void OnTargetGoaled()
    {
        CurrentTask = UnitTask.None;
    }

    // Attack logic
    public void SetAttackTarget(Entity entity)
    {
        if (entity.TeamID == TeamID)
            throw new ArgumentException("Wrong argument: " + entity);

        _attackTarget = entity;

        if (_attackRoutine == null)
            _attackRoutine = StartCoroutine(AttackRoutine());

    }
    public void ClearAttackTarget()
    {
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
        }

        InCooldown = false;
    }
    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (_attackTarget == null || _attackTarget.Health <= 0)
            {
                _attackRoutine = null;
                yield break;
            }

            if (Vector3.Distance(transform.position, _attackTarget.transform.position) < MaxAttackRange)
            {
                Attack(_attackTarget);

                InCooldown = true;
                yield return new WaitForSeconds(Cooldown);
                InCooldown = false;
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    // Organization
    public override void Init(int teamID)
    {
        base.Init(teamID);
        _targetMovement = GetComponent<TargetMovement>();
        _targetMovement.onTargetGoaled = OnTargetGoaled;
        Debug.Log(gameObject.name + "; " + _targetMovement + ";");
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
    public void Load(SerializableUnit unit)
    {
        Debug.Log(gameObject.name + "; " + _targetMovement + ";" + unit);
        Name = unit.Name;
        _targetMovement.Speed = unit.MovementSpeed;
        MaxHealth = unit.MaxHealth;
        TrainingCost = unit.TrainingCost;
        DetectionRange = unit.DetectionRange;
    }
}
