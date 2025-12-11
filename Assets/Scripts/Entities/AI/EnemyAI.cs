using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class EnemyAI : MonoBehaviour
{
    private Unit _unit;
    private Transform _target;

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }
    private void OnTriggerStay(Collider coll)
    {
        if (_unit.TeamID == 0)
            return;

        float colliderDistance = Vector3.Distance(transform.position, coll.transform.position);
        if ((_target == null || Vector3.Distance(transform.position, _target.position) > colliderDistance || _target == coll.transform) &&
            coll.TryGetComponent(out Entity other) && other.TeamID != _unit.TeamID)
        {
            _target = coll.transform;
            _unit.SetDestination(other.transform.position);
            
            if (colliderDistance <= _unit.AttackRange)
            {
                _unit.SetAttackTarget(other);
            }
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (_unit.TeamID == 0)
            return;

        if (_target == other.transform)
        {
            _target = null;
        }
    }
}
