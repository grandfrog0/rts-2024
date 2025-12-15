using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ResourceSelector: MonoBehaviour
{
    [SerializeField] LayerMask resourceLayerMask;
    //////
    private List<Builder> _builders = new List<Builder>();

    public void OnSelectionChanged(HashSet<Entity> entities)
    {
        foreach (Entity entity in entities)
        {
            if (entity is Builder builder)
                _builders.Add(builder);
        }
    }

    public void UpdateDestination()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, resourceLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent(out Resource resource))
            {
                foreach (Builder builder in _builders)
                {
                    if (builder.WaitingTask == UnitTask.Mine)
                        builder.SetMineDestination(resource);
                }
            }
        }
    }
}