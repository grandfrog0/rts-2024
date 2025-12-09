using System.Collections;
using System.Collections.Generic;
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
    //private void OnTriggerStay(Collider coll)
    private void OnTriggerEnter(Collider coll)
    {
        if ((_target == null || Vector3.Distance(transform.position, _target.position) > Vector3.Distance(transform.position, coll.transform.position) || _target == coll.gameObject) &&
            coll.TryGetComponent(out Unit other) && other.TeamID != _unit.TeamID)
        {
            _target = coll.transform;
            _unit.SetDestination(other.transform.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (_target == other.transform)
            _target = null;
    }
}
