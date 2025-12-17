using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectedEntityActionBar : MonoBehaviour
{
    [SerializeField] GameObject buildActionsParent, mainActionsParent;
    [SerializeField] GameObject builderButtons, archerButtons, healerButtons;

    public void OnEntityChanged(HashSet<Entity> entities)
    {
        if (entities.Count == 1)
        {
            Entity entity = entities.First();
            if (entity.TeamID == 0)
            {
                SetEntity(entity);
                return;
            }
        }

        // else
        ClearInterface();
    }

    public void SetEntity(Entity entity)
    {
        mainActionsParent.SetActive(true);
        buildActionsParent.SetActive(entity is Builder);

        builderButtons.SetActive(entity is Builder);
        archerButtons.SetActive(entity is Archer);
        healerButtons.SetActive(entity is Healer);
    }
    public void ClearInterface()
    {
        buildActionsParent.SetActive(false);
        mainActionsParent.SetActive(false);
    }
}
