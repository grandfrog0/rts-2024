using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    private static EntitySpawner _instance;

    public static Entity Spawn(GameObject entity, Vector3 position, Quaternion rotation, int teamID = -1)
        => Spawn(entity.GetComponent<Entity>(), position, rotation, teamID);
    public static Entity Spawn(Entity entity, Vector3 position, Quaternion rotation, int teamID = -1)
    {
        Entity e = Instantiate(entity, position, rotation, _instance.unitParent);
        e.TeamID = teamID;
        return e;
    }

    [SerializeField] Transform unitParent;
    public void Initialize()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }
}
