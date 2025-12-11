using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TargetMovement))]
public class Unit : Entity
{
    [SerializeField] float _attackRange = 1;
    public float AttackRange => _attackRange;
    [SerializeField] float _cooldown = 1;
    public float Cooldown => _cooldown;
    public bool InCooldown { get; private set; }
    private Coroutine _attackRoutine;
    private Entity _attackTarget;

    protected TargetMovement _targetMovement;

    public UnitTask CurrentTask { get; protected set; } = UnitTask.None;
    public UnitTask WaitingTask { get; set; } = UnitTask.None;

    // Movement logic
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

            if (Vector3.Distance(transform.position, _attackTarget.transform.position) < AttackRange)
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

    protected override void Start()
    {
        base.Start();
        _targetMovement = GetComponent<TargetMovement>();
        _targetMovement.onTargetCompleted = OnTargetGoaled;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
