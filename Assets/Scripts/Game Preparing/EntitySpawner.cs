using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        e.Init(teamID);

        if (e is Unit unit)
        {
            if (e.ConfigName != "")
            {
                string json = ResourceManager.GetText(e.ConfigName);
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

        return e;
    }

    [SerializeField] Transform unitParent;
    [SerializeField] LayerMask floorMask;
    private Dictionary<int, List<Unit>> _units = new();
    public void Initialize()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }
}
