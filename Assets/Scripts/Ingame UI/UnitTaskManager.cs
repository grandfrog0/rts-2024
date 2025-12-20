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
    public static List<Builder> SelectedBuilders { get; private set; } = new List<Builder>();
    
    [SerializeField] UnityEvent<UnitTask> onUnitTaskChanged = new();
    [SerializeField] UnityEvent<Building> onBuildingTaskChanged = new();
    [SerializeField] BuildingBuilder buildingBuilder;
    private UnitTask WaitingTask
    {
        get => SelectedUnits[0].WaitingTask;
        set
        {
            foreach(var unit in SelectedUnits)
                unit.WaitingTask = value;

            if (value == UnitTask.None)
                EntitySelector.IgnoreNext = false;
            Debug.Log((value, EntitySelector.IgnoreNext));

            onUnitTaskChanged.Invoke(value);
        }
    }
    private Building WaitingBuildingTask
    {
        get => SelectedBuilders[0].WaitingBuildingTask;
        set
        {
            foreach (var builder in SelectedBuilders)
                builder.WaitingBuildingTask = value;

            onBuildingTaskChanged.Invoke(value);
        }
    }
    public void ToggleTask(string taskName)
    {
        if (SelectedUnits.Count == 0)
            return;

        UnitTask task = Enum.Parse<UnitTask>(taskName);

        if (WaitingTask == UnitTask.Build)
        {
            bool isBuildingEnd = false;

            if (task == UnitTask.Fix && SelectedBuilders.Count != 0)
            {
                Building b = buildingBuilder.TryPlace();
                if (b != null)
                {
                    b.PrepareToBuild();
                    foreach (var builder in SelectedBuilders)
                        builder.SetFixDestination(b);
                }

                isBuildingEnd = true;
                WaitingTask = UnitTask.None;
            }

            WaitingBuildingTask = null;
            buildingBuilder.CancelPreparing();

            if (isBuildingEnd)
                return;
        }

        if (WaitingTask == task)
        {

            WaitingTask = UnitTask.None;
        }
        else
        {
            WaitingTask = task;
        }
    }
    public void ToggleBuildingTask(string buildingName)
    {
        if (SelectedUnits.Count == 0)
            return;

        WaitingTask = UnitTask.Build;
        if (WaitingBuildingTask != null && WaitingBuildingTask.Name == buildingName)
        {
            WaitingTask = UnitTask.None;
            WaitingBuildingTask = null;
            buildingBuilder.CancelPreparing();
        }
        else
        {
            WaitingBuildingTask = buildingBuilder.PrepareSpawn(buildingName);
            EntitySelector.IgnoreNext = true;
        }
    }

    public void OnSelectionChanged(HashSet<Entity> entities)
    {
        if (entities.All(x => x is Unit && x.TeamID == 0))
        {
            SelectedUnits = entities.Select(x => (Unit)x).ToList();
            SelectedBuilders = entities.Where(x => x is Builder).Select(x => (Builder)x).ToList();
        }
        else
        {
            WaitingTask = UnitTask.None;
            WaitingBuildingTask = null;
            SelectedUnits.Clear();
            SelectedBuilders.Clear();
        }
    }
}
