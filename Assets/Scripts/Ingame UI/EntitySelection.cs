using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EntitySelection : MonoBehaviour
{
    [SerializeField] UnityEvent<HashSet<Entity>> onSelectionChanged = new();

    [SerializeField] LayerMask objectLayerMask;
    [SerializeField] LayerMask floorLayerMask;
    [SerializeField] GameObject selectionPrefab;
    private Camera _camera;
    private HashSet<GameObject> _selections = new HashSet<GameObject>();
    private HashSet<Entity> _selectedEntities = new HashSet<Entity>();

    [SerializeField] RectTransform selectionField;
    private Vector3 _selectionStart;
    private Vector3 _mouseDelta;

    private void HandleObjectClick(Entity entity, bool revertSelected, bool multipleChoice)
    {
        bool contains = _selectedEntities.Contains(entity);

        if (!multipleChoice)
        {
            _selectedEntities.Clear();
        }

        if (revertSelected && contains)
        {
            _selectedEntities.Remove(entity);
        }
        else
        {
            _selectedEntities.Add(entity);
        }

        HandleSelected();
    }

    private void HandleSelected()
    {
        ClearSelections();

        foreach(Entity entity in _selectedEntities)
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

    private void ClearSelected()
    {
        ClearSelections();
        _selectedEntities.Clear();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _selectionStart = Input.mousePosition;
            selectionField.anchoredPosition = _selectionStart - new Vector3(Screen.width / 2, Screen.height / 2);
        }
        if (Input.GetMouseButton(0))
        {
            _mouseDelta = Input.mousePosition - _selectionStart;

            selectionField.pivot = new Vector2(_mouseDelta.x >= 0 ? 0 : 1, _mouseDelta.y >= 0 ? 0 : 1);
            selectionField.sizeDelta = new Vector2(Mathf.Abs(_mouseDelta.x), Mathf.Abs(_mouseDelta.y));
        }
        if (Input.GetMouseButtonUp(0))
        {
            // lb, lu, ru, rb
            Vector3[] corners = new Vector3[4];
            selectionField.GetWorldCorners(corners);

            for (int i = 0; i < corners.Length; i++)
                corners[i] = ScreenToWorldPoint(corners[i]).Value;

            //Debug.Log(string.Join("; ", corners));

            Vector3 halfExtents = corners[2] - corners[0];

            if (halfExtents.magnitude > 0.25f)
            {
                Vector3 center = corners[0] + halfExtents / 2;
                halfExtents.y = 10;
                center.y = 0;
                Debug.Log(center + "; " + halfExtents);

                Collider[] colliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, objectLayerMask, QueryTriggerInteraction.Ignore);
                foreach (Collider collider in new HashSet<Collider>(colliders))
                {
                    //Debug.Log(collider);
                    if (collider.TryGetComponent(out Entity entity))
                    {
                        HandleObjectClick(entity, false, true);
                    }
                }

                onSelectionChanged.Invoke(_selectedEntities);
            }

            // clear
            selectionField.sizeDelta = Vector2.zero;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // ui click ignore
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            bool multipleChoice = Input.GetKey(KeyCode.LeftControl);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, objectLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider.TryGetComponent(out Entity entity))
                {
                    HandleObjectClick(entity, revertSelected: true, multipleChoice: multipleChoice);
                    onSelectionChanged.Invoke(_selectedEntities);
                    return;
                }
            }

            if (!multipleChoice)
            {
                ClearSelected();
            }
            onSelectionChanged.Invoke(_selectedEntities);
        }
    }
    
    private Vector3? ScreenToWorldPoint(Vector3 screenPoint)
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(screenPoint), out RaycastHit hit, Mathf.Infinity, floorLayerMask, QueryTriggerInteraction.Ignore))
            return hit.point;

        return null;
    }

    private void Start()
    {
        _camera = Camera.main;
    }
}