using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.UI.CanvasScaler;

public class EntitySelector : MonoBehaviour
{
    public static bool IgnoreNext { get; set; }

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
        if (revertSelected && _selectedEntities.Count != 0 && _selectedEntities.All(x => x is Unit))
        {
            if (_selectedEntities.All(x => x is Archer archer && archer.WaitingTask == UnitTask.Attack && entity.TeamID != archer.TeamID))
            {
                foreach(var e in _selectedEntities)
                    ((Archer)e).SetAttackDestination(entity);

                IgnoreNext = true;
                return;
            }
            else if (_selectedEntities.All(x => x is Healer healer && healer.WaitingTask == UnitTask.Heal && entity.TeamID == healer.TeamID))
            {
                foreach (var e in _selectedEntities)
                    ((Healer)e).SetHealDestination(entity);

                IgnoreNext = true;
                return;
            }
            else if (entity is Building building && entity.HealthPercent < 1 && _selectedEntities.All(x => x is Builder builder && (builder.WaitingTask is UnitTask.Fix or UnitTask.Build) && entity.TeamID == builder.TeamID))
            {
                foreach (var e in _selectedEntities)
                    ((Builder)e).SetFixDestination(building);

                IgnoreNext = true;
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
        _selectedEntities.Clear();
        onSelectionChanged.Invoke(_selectedEntities);
    }

    public void MouseDown()
    {
        // Multiple selection
        _selectionStart = Input.mousePosition;
        selectionField.anchoredPosition = _selectionStart - new Vector3(Screen.width / 2, Screen.height / 2);
    }
    public void MouseHold()
    {
        _mouseDelta = Input.mousePosition - _selectionStart;

        selectionField.pivot = new Vector2(_mouseDelta.x >= 0 ? 0 : 1, _mouseDelta.y >= 0 ? 0 : 1);
        selectionField.sizeDelta = new Vector2(Mathf.Abs(_mouseDelta.x), Mathf.Abs(_mouseDelta.y));
    }
    public void MouseUp()
    {
        if (IgnoreNext)
        {
            selectionField.sizeDelta = Vector2.zero;
            return;
        }

        Vector3 start = _selectionStart;
        Vector3 end = start + _mouseDelta;

        Rect rect = GetScreenRect(start, end);

        if (rect.width < 5 && rect.height < 5)
        {
            selectionField.sizeDelta = Vector2.zero;
            HandleSingleClick();
            return;
        }

        _selectedEntities.Clear();
        foreach (Entity entity in EntitySpawner.Entities)
        {
            if (!entity.IsAlive)
                continue;

            Vector3 screenPos = _camera.WorldToScreenPoint(entity.transform.position);

            if (rect.Contains(screenPos))
            {
                HandleObjectClick(entity, revertSelected: false, multipleChoice: true);
            }
        }

        if (!IgnoreNext)
            onSelectionChanged.Invoke(_selectedEntities);

        // clear
        selectionField.sizeDelta = Vector2.zero;
    }
    public void HandleSingleClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        bool multipleChoice = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, objectLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.TryGetComponent(out Entity entity))
            {
                HandleObjectClick(entity, revertSelected: true, multipleChoice: multipleChoice);

                if (!IgnoreNext)
                    onSelectionChanged.Invoke(_selectedEntities);

                return;
            }
        }

        if (!multipleChoice)
        {
            ClearSelected();
        }
    }
    private Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        Vector3 topLeft = Vector3.Min(screenPosition1, screenPosition2);
        Vector3 bottomRight = Vector3.Max(screenPosition1, screenPosition2);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    private void Start()
    {
        _camera = Camera.main;
    }
}