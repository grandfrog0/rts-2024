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
        List<Entity> playerEntities = entities.Where(e => e.TeamID == 0).ToList();

        if (playerEntities.Count == 1)
        {
            SetEntity(playerEntities[0]);
        }
        else
        {
            ClearInterface();
        }
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
