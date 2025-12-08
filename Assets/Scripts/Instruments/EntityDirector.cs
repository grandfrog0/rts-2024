using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDirector : MonoBehaviour
{
    private HashSet<Unit> units = new HashSet<Unit>();

    public void OnSelectionChanged(HashSet<Entity> entities)
    {
        units.Clear();
        foreach (Entity entity in entities)
        {
            if (entity is Unit unit)
                units.Add(unit);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Floor"), QueryTriggerInteraction.Ignore))
            {
                Debug.Log(hit.point);
                foreach (Unit unit in units)
                    unit.SetTarget(hit.point);
            }
        }
    }
}
