using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EntitySpawner : MonoBehaviour
{
    public static UnityEvent<int, List<Unit>> OnUnitsCountChanged = new ();
    private static EntitySpawner _instance;

    public static Entity Spawn(GameObject entity, Vector3 position, Quaternion rotation, int teamID = -1)
        => Spawn(entity.GetComponent<Entity>(), position, rotation, teamID);
    public static Entity Spawn(Entity entity, Vector3 position, Quaternion rotation, int teamID = -1)
    {
        Entity e = Instantiate(entity, position, rotation, _instance.unitParent);
        e.TeamID = teamID;

        //if (!_instance._entities.ContainsKey(teamID))
        //    _instance._entities[teamID] = new HashSet<Entity>();
        //_instance._entities[teamID].Add(entity);
        //entity.OnDead.AddListener(() => _instance._entities[teamID].Remove(entity));

        if (entity is Unit unit)
        {
            if (!_instance._units.ContainsKey(teamID))
                _instance._units[teamID] = new List<Unit>();

            _instance._units[teamID].Add(unit);
            OnUnitsCountChanged.Invoke(teamID, _instance._units[teamID]);

            unit.OnDead.AddListener(() => {
                _instance._units[teamID].Remove(unit);
                OnUnitsCountChanged.Invoke(teamID, _instance._units[teamID]);
                });
        }

        return e;
    }

    [SerializeField] Transform unitParent;
    [SerializeField] LayerMask floorMask;
    //private Dictionary<int, HashSet<Entity>> _entities;
    private Dictionary<int, List<Unit>> _units = new();
    public void Initialize()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }
}
