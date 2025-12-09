using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UnitTaskManager : MonoBehaviour
{
    [SerializeField] UnityEvent<UnitTask> onUnitTaskChanged = new();

    private UnitTask WaitingTask
    {
        get => _selectedUnit.WaitingTask;
        set
        {
            _selectedUnit.WaitingTask = value;
            onUnitTaskChanged.Invoke(value);
        }
    }
    private Unit _selectedUnit;

    public void ToggleTask(string taskName)
    {
        if (_selectedUnit == null)
            return;

        UnitTask task = Enum.Parse<UnitTask>(taskName);

        if (WaitingTask == task)
        {
            WaitingTask = UnitTask.None;
        }
        else
        {
            WaitingTask = task;
        }
    }

    public void OnSelectionChanged(HashSet<Entity> entities)
    {
        List<Entity> playerEntitites = entities.Where(e => e.TeamID == 0).ToList();
        if (playerEntitites.Count == 1 && playerEntitites[0] is Unit unit)
        {
            _selectedUnit = unit;
        }
        else if (playerEntitites.Count == 0 && _selectedUnit != null)
        {
            WaitingTask = UnitTask.None;
        }
    }
}
