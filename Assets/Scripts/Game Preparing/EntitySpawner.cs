using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EntitySpawner : MonoBehaviour
{
    public static UnityEvent<int, List<Unit>> OnUnitsCountChanged = new ();
    public static List<Entity> Entities => _instance._entities;
    private static EntitySpawner _instance;

    public static Entity Spawn(GameObject entity, Vector3 position, Quaternion rotation, int teamID = -1)
        => Spawn(entity.GetComponent<Entity>(), position, rotation, teamID);
    public static Entity Spawn(Entity prefab, Vector3 position, Quaternion rotation, int teamID = -1)
    {
        Entity entity = Instantiate(prefab, position, rotation, _instance.unitParent);
        entity.Init(teamID);

        _instance._entities.Add(entity);
        if (entity is Unit unit)
        {
            if (entity.ConfigName != "")
            {
                string json = ResourceManager.GetText(entity.ConfigName);
                switch (unit)
                {
                    case Builder builder:
                        builder.Load(JsonUtility.FromJson<SerializableBuilder>(json));
                        break;

                    case Archer archer:
                        archer.Load(JsonUtility.FromJson<SerializableArcher>(json));
                        break;

                    default:
                        unit.Load(JsonUtility.FromJson<SerializableUnit>(json));
                        break;
                }
            }


            if (!_instance._units.ContainsKey(teamID))
                _instance._units[teamID] = new List<Unit>();

            _instance._units[teamID].Add(unit);
            OnUnitsCountChanged.Invoke(teamID, _instance._units[teamID]);

            unit.OnDead.AddListener(() => {
                _instance._units[teamID].Remove(unit);
                OnUnitsCountChanged.Invoke(teamID, _instance._units[teamID]);
                });
        }

        return entity;
    }

    [SerializeField] Transform unitParent;
    [SerializeField] LayerMask floorMask;
    private Dictionary<int, List<Unit>> _units = new();
    private List<Entity> _entities = new();
    public void Initialize()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }
}
