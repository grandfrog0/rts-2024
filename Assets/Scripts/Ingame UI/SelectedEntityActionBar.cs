using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ObjectChangeEventStream;

public class SelectedEntityActionBar : MonoBehaviour
{
    [SerializeField] GameObject buildActionsParent, mainActionsParent;
    [SerializeField] GameObject unitButtons, buildingButtons;
    [SerializeField] GameObject builderButtons, archerButtons, healerButtons;

    public void OnEntityChanged(HashSet<Entity> entities)
    {
        if (entities.Count != 0 && entities.All(x => x.TeamID == 0))
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

        // unit || building
        unitButtons.SetActive(false);
        buildingButtons.SetActive(false);

        // builder
        buildActionsParent.SetActive(false);
        builderButtons.SetActive(false);

        // archer || healer
        archerButtons.SetActive(false);
        healerButtons.SetActive(false); 
        
        if (entities.All(x => x is Unit))
        {
            unitButtons.SetActive(true);

            if (entities.All(x => x is Builder))
            {
                buildActionsParent.SetActive(true);
                builderButtons.SetActive(true);
            }
            else if (entities.All(x => x is Archer))
            {
                archerButtons.SetActive(true);
            }
            else if (entities.All(x => x is Healer))
            {
                healerButtons.SetActive(true);
            }
        }
        else if (entities.All(x => x is Building))
        {
            buildingButtons.SetActive(true);
        }
    }
    public void ClearInterface()
    {
        buildActionsParent.SetActive(false);
        mainActionsParent.SetActive(false);
    }
}
