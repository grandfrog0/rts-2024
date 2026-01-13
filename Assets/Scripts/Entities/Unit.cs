using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using Unity.VisualScripting;
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
    public bool InCooldown { get; protected set; }
    private Coroutine _attackRoutine;
    private IHurtable _attackTarget;

    // Misc
    public List<TrainingCost> TrainingCost { get; set; }
    public float DetectionRange { get; set; }

    // Tasks
    private UnitTask _currentTask = UnitTask.None;
    public UnitTask CurrentTask
    {
        get => _currentTask;
        protected set
        {
            BreakTask();
            _currentTask = value;
        }
    }
    public UnitTask WaitingTask { get; set; } = UnitTask.None;

    // Animator
    public string attackAnimationName = "OnAttack";
    protected Animator _animator;

    public void BreakTask()
    {
        _currentTask = UnitTask.None;
    }

    // Movement logic
    protected TargetMovement _targetMovement;
    public void SetDestination(Vector3 position)
    {
        _targetMovement.SetTarget(position);
        CurrentTask = UnitTask.Command;
        //WaitingTask = UnitTask.None;
    }
    private void OnTargetGoaled()
    {
        ClearCurrentTask();
        _animator.SetBool("isMoving", false);
    }

    public void ClearCurrentTask()
    {
        CurrentTask = UnitTask.None;
        //WaitingTask = UnitTask.None;
    }

    // Attack logic
    public void SetAttackTarget(IHurtable target)
    {
        _attackTarget = target;

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
            if (_attackTarget == null || !_attackTarget.IsReady)
            {
                _attackRoutine = null;
                yield break;
            }

            Debug.Log($"{Vector3.Distance(transform.position, _attackTarget.Position)} < {MaxAttackRange}");
            if (Vector3.Distance(transform.position, _attackTarget.Position) < MaxAttackRange)
            {
                Attack(_attackTarget);
                _animator.SetTrigger(attackAnimationName);

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
        _targetMovement.onTargetGoaled += OnTargetGoaled;

        _animator = GetComponentInChildren<Animator>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
    public void Load(SerializableUnit unit)
    {
        Name = unit.Name;
        _targetMovement.Speed = unit.MovementSpeed;
        MaxHealth = unit.MaxHealth;
        TrainingCost = unit.TrainingCost;
        DetectionRange = unit.DetectionRange;

        Health = MaxHealth;
    }

    protected override void Die(Entity enemy)
    {
        _animator.SetBool("IsDied", true);

        base.Die(enemy);
    }
}
