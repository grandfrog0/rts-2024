using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class TargetMovement : MonoBehaviour
{
    private NavMeshAgent _agent;

    public void SetTarget(Vector3 position)
    {
        if (_agent.isOnNavMesh)
        {
            _agent.destination = position;
        }
        else
        {
            Debug.Log("Is not on NavMesh!");
        }
    }

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
}
