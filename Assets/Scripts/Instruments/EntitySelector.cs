using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EntitySelector : MonoBehaviour
{
    [SerializeField] UnityEvent<HashSet<Entity>> onSelectionChanged = new();

    [SerializeField] LayerMask objectLayerMask;
    [SerializeField] LayerMask floorLayerMask;
    private Camera _camera;
    private HashSet<Entity> _selectedEntities = new HashSet<Entity>();

    [SerializeField] RectTransform selectionField;
    private Vector3 _selectionStart;
    private Vector3 _mouseDelta;

    private void HandleObjectClick(Entity entity, bool revertSelected, bool multipleChoice)
    {
        if (_selectedEntities.Count == 1 && _selectedEntities.First() is Unit unit)
        {
            if (unit is Archer archer && unit.WaitingTask == UnitTask.Attack && entity.TeamID != unit.TeamID)
            {
                archer.SetAttackDestination(entity);
                return;
            }
            else if (unit is Healer healer && unit.WaitingTask == UnitTask.Heal && entity.TeamID == unit.TeamID)
            {
                healer.SetHealDestination(entity);
                return;
            }
        }

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
    }

    private void ClearSelected()
    {
        //Entity first = null;
        //if (_selectedEntities.Count == 1)
        //    first = _selectedEntities.First();

        _selectedEntities.Clear();

        //if (first != null && first == UnitTaskManager.SelectedUnit && UnitTaskManager.IsAppliedNow)
        //    _selectedEntities.Add(first);

        onSelectionChanged.Invoke(_selectedEntities);
    }

    public void MouseDown()
    {
        // Multiple selection
       
        _selectionStart = Input.mousePosition;
        selectionField.anchoredPosition = _selectionStart - new Vector3(Screen.width / 2, Screen.height / 2);

        // Solo selection
       
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
    }
    public void MouseHold()
    {
        _mouseDelta = Input.mousePosition - _selectionStart;

        selectionField.pivot = new Vector2(_mouseDelta.x >= 0 ? 0 : 1, _mouseDelta.y >= 0 ? 0 : 1);
        selectionField.sizeDelta = new Vector2(Mathf.Abs(_mouseDelta.x), Mathf.Abs(_mouseDelta.y));
    }
    public void MouseUp()
    {
        // lb, lu, ru, rb
        Vector3[] corners = new Vector3[4];
        selectionField.GetWorldCorners(corners);

        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = new Vector3(Mathf.Clamp(corners[i].x, 0, Screen.width), Mathf.Clamp(corners[i].y, 0, Screen.height));
            corners[i] = ScreenToWorldPoint(corners[i]).Value;
        }

        //Debug.Log(string.Join("; ", corners));

        Vector3 halfExtents = corners[2] - corners[0];

        if (halfExtents.magnitude > 0.25f)
        {
            Vector3 center = corners[0] + halfExtents / 2;
            halfExtents.y = 10;
            center.y = 0;

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