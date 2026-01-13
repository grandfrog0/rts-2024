using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class TargetMovement : MonoBehaviour
{
    public event Action onTargetGoaled;

    private NavMeshAgent _agent;
    private Coroutine _targetUpdate;
    private IHurtable _target;

    private float _speed;
    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;

            if (_agent != null) 
                _agent.speed = value;
        }
    }

    private Animator _animator;

    public void SetTarget(Vector3 position)
    {
        _animator.SetBool("isMoving", true);

        if (_agent.isOnNavMesh)
        {   
            _agent.isStopped = false;
            _agent.destination = position;

            if (!_targetUpdate.IsUnityNull())
            {
                StopCoroutine(_targetUpdate);
            }
            _targetUpdate = StartCoroutine(TargetUpdate(position));
        }
        else
        {
            Debug.Log("Is not on NavMesh!");
        }
    }
    public void SetTarget(IHurtable target)
    {
        _target = target;
        SetTarget(target.Position);
    }

    private IEnumerator TargetUpdate(Vector3 targetPosition)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            if (Vector3.Distance(targetPosition, transform.position) <= _agent.stoppingDistance + 0.5f)
            {
                _agent.isStopped = true;

                onTargetGoaled?.Invoke();

                yield break;
            }
            else if (_target != null)
            {
                targetPosition = _target.Position;
                _agent.destination = targetPosition;
            }
        }
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = Speed;

        _animator = GetComponentInChildren<Animator>();
    }
}
