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
    [SerializeField] GameObject unitButtons, buildingButtons;
    [SerializeField] GameObject builderButtons, archerButtons, healerButtons;

    public void OnEntityChanged(HashSet<Entity> entities)
    {
        if (entities.All(x => x.TeamID == 0))
        {
            SetEntities(entities);
            return;
        }

        // else
        ClearInterface();
    }

    public void SetEntities(HashSet<Entity> entities)
    {
        mainActionsParent.SetActive(true);

        bool isUnit = false;
        bool isBuilding = false;

        if (isUnit = entities.All(x => x is Unit))
        {
            bool isBuilder = entities.All(x => x is Builder);
            
            buildActionsParent.SetActive(isBuilder);
            builderButtons.SetActive(isBuilder);

            archerButtons.SetActive(entities.All(x => x is Archer));
            healerButtons.SetActive(entities.All(x => x is Healer));
        }
        else if (isBuilding = entities.All(x => x is Building))
        {

        }

        unitButtons.SetActive(isUnit);
        buildingButtons.SetActive(isBuilding);
    }
    public void ClearInterface()
    {
        buildActionsParent.SetActive(false);
        mainActionsParent.SetActive(false);
    }
}
