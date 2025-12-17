using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UnitTaskManager : MonoBehaviour
{
    //private static bool _isAppliedNow = false;
    //public static bool IsAppliedNow { get => _isAppliedNow; set { _isAppliedNow = value; Debug.Log(_isAppliedNow); } }
    public static Unit SelectedUnit { get; private set; }
    
    [SerializeField] UnityEvent<UnitTask> onUnitTaskChanged = new();

    private UnitTask WaitingTask
    {
        get => SelectedUnit.WaitingTask;
        set
        {
            SelectedUnit.WaitingTask = value;

            if (value == UnitTask.None)
                EntitySelector.IgnoreNext = false;

            onUnitTaskChanged.Invoke(value);
        }
    }
    public void ToggleTask(string taskName)
    {
        if (SelectedUnit == null)
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
            SelectedUnit = unit;
        }
        else if (playerEntitites.Count == 0 && SelectedUnit != null)
        {
            WaitingTask = UnitTask.None;
        }
    }
}
