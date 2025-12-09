using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityDirector : MonoBehaviour
{
    private List<Unit> units = new List<Unit>();

    public void OnSelectionChanged(HashSet<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            if (entity is Unit unit)
                units.Add(unit);
        }
    }

    public void UpdateDestination()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Floor"), QueryTriggerInteraction.Ignore))
        {
            foreach (Unit unit in units)
                if (unit.CurrentTask == UnitTask.None && unit.WaitingTask == UnitTask.Command)
                    unit.SetDestination(hit.point);
        }
    }
}
