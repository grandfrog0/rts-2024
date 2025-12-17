using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[RequireComponent(typeof(Archer))]
public class ArcherAI : MonoBehaviour
{
    private Archer _unit;
    private Entity _target;

    private void Awake()
    {
        _unit = GetComponent<Archer>();
    }
    private void OnTriggerStay(Collider coll)
    {
        if (_unit.TeamID == 0)
            return;

        if (coll.TryGetComponent(out Entity other) && other.IsAlive && other.TeamID != _unit.TeamID && 
            (_target == null || !_target.IsAlive || other.AttackPriority > _target.AttackPriority 
            || (other.AttackPriority == _target.AttackPriority && Vector3.Distance(transform.position, _target.Position) > Vector3.Distance(transform.position, coll.transform.position))))
        {
            _target = other;
            Debug.Log(_target);
            _unit.SetAttackDestination(other);
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
