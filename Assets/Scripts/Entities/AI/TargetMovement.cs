using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class TargetMovement : MonoBehaviour
{
    public Action onTargetCompleted;

    private NavMeshAgent _agent;
    private Coroutine _targetUpdate;

    public void SetTarget(Vector3 position)
    {
        if (_agent.isOnNavMesh)
        {   
            _agent.isStopped = false;
            _agent.destination = position;

            if (_targetUpdate != null)
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

    private IEnumerator TargetUpdate(Vector3 targetPosition)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            if (Vector3.Distance(targetPosition, transform.position) <= _agent.stoppingDistance + 0.5f)
            {
                _agent.isStopped = true;

                if (onTargetCompleted != null)
                    onTargetCompleted();

                yield break;
            }
        }
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
}
