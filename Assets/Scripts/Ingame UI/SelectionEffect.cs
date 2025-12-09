using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectionEffect : MonoBehaviour
{
    [SerializeField] GameObject selectionPrefab;
    private HashSet<GameObject> _selections = new HashSet<GameObject>();

    public void OnSelectionChanged(HashSet<Entity> entities)
    {
        ClearSelections();

        List<Entity> playerEntities = entities.Where(e => e.TeamID == 0).ToList();

        foreach (Entity entity in playerEntities)
        {
            GameObject selection = Instantiate(selectionPrefab, entity.transform.position, selectionPrefab.transform.rotation, entity.transform);
            selection.transform.position = new Vector3(selection.transform.position.x, 0.1f, selection.transform.position.z);
            selection.transform.localScale = new Vector3(entity.Size, entity.Size, 1);
            _selections.Add(selection);
        }
    }

    private void ClearSelections()
    {
        foreach (GameObject selection in _selections)
        {
            Destroy(selection);
        }
        _selections.Clear();
    }
}
