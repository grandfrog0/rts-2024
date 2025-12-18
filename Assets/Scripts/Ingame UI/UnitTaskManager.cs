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
    public static List<Unit> SelectedUnits { get; private set; } = new List<Unit>();
    
    [SerializeField] UnityEvent<UnitTask> onUnitTaskChanged = new();

    private UnitTask WaitingTask
    {
        get => SelectedUnits[0].WaitingTask;
        set
        {
            foreach(var unit in SelectedUnits)
                unit.WaitingTask = value;

            if (value == UnitTask.None)
                EntitySelector.IgnoreNext = false;

            onUnitTaskChanged.Invoke(value);
        }
    }
    public void ToggleTask(string taskName)
    {
        if (SelectedUnits.Count == 0)
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
        if (entities.All(x => x is Unit && x.TeamID == 0))
        {
            SelectedUnits = entities.Select(x => (Unit)x).ToList();
        }
        else
        {
            WaitingTask = UnitTask.None;
            SelectedUnits.Clear();
        }
    }
}
