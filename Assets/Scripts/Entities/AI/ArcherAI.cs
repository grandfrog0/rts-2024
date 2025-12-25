using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

//[RequireComponent(typeof(Archer))]
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
        if (!_unit || _unit.TeamID == 0)
            return;
        /*
        Debug.Log("CAN BE TARGET: " + coll.gameObject);
        Debug.Log("TARGET IS " + _target);
        if (_target)
            Debug.Log("TARGET IS ALIVE: " + _target.IsReady + "; " + _target.IsAlive);

        Debug.Log(coll.TryGetComponent(out Entity o1) && o1.IsReady && o1.TeamID != _unit.TeamID &&
            (_target == null || !_target.IsReady || o1.AttackPriority > _target.AttackPriority
            || (o1.AttackPriority == _target.AttackPriority && Vector3.Distance(transform.position, _target.Position) > Vector3.Distance(transform.position, coll.transform.position))));
        Debug.Log(coll.TryGetComponent(out Entity o2) && o2.IsReady && o2.TeamID != _unit.TeamID &&
                    (_target == null || !_target.IsReady || o2.AttackPriority > _target.AttackPriority
                    || (o1.AttackPriority == _target.AttackPriority)));
        Debug.Log(coll.TryGetComponent(out Entity o3) && o3.IsReady && o3.TeamID != _unit.TeamID &&
                    (_target == null || !_target.IsReady || o3.AttackPriority > _target.AttackPriority));
        Debug.Log(coll.TryGetComponent(out Entity o4) && o4.IsReady && o4.TeamID != _unit.TeamID &&
                    (_target == null || !_target.IsReady));
        Debug.Log(coll.TryGetComponent(out Entity o5) && o5.IsReady && o5.TeamID != _unit.TeamID && _target == null);
        Debug.Log(coll.TryGetComponent(out Entity o6) && o6.IsReady && o6.TeamID != _unit.TeamID);
        Debug.Log(coll.TryGetComponent(out Entity o7) && o7.IsReady);
        Debug.Log(coll.TryGetComponent(out Entity _));
        */


        if (coll.TryGetComponent(out Entity other) && other.IsReady && other.TeamID != _unit.TeamID &&
            (_target == null || !_target.IsReady || other.AttackPriority > _target.AttackPriority
            || (other.AttackPriority == _target.AttackPriority && Vector3.Distance(transform.position, _target.Position) > Vector3.Distance(transform.position, coll.transform.position))))
        {
            _target = other;
            _unit.SetAttackDestination(other);
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (!_unit || _unit.TeamID == 0)
            return;

        if (_target != null && _target.transform == other.transform)
        {
            _target = null;
        }
    }
}
