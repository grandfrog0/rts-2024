using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class EntityDirector : MonoBehaviour
{
    private List<Unit> _units = new List<Unit>();

    public void OnSelectionChanged(HashSet<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            if (entity is Unit unit)
                _units.Add(unit);
        }
    }

    public void UpdateDestination()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Floor"), QueryTriggerInteraction.Ignore))
        {
            //UnitTaskManager.IsAppliedNow = false;
            foreach (Unit unit in _units)
            {
                if (unit.CurrentTask == UnitTask.None && unit.WaitingTask == UnitTask.Command)
                    unit.SetDestination(hit.point);
            }
        }
    }
}
