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

        foreach (Entity entity in entities)
        {
            _selections.Add(SpawnSelection(entity.transform, entity.Model.Size));
        }
    }

    public GameObject SpawnSelection(Transform target, float size)
    {
        GameObject selection = Instantiate(selectionPrefab, target.position, selectionPrefab.transform.rotation, target);
        selection.transform.position = new Vector3(selection.transform.position.x, 0.1f, selection.transform.position.z);
        selection.transform.localScale = new Vector3(size, size, 1);
        return selection;
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
