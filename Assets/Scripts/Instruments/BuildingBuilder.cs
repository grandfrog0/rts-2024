using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingBuilder : MonoBehaviour
{
    [SerializeField] List<Building> buildings;
    [SerializeField] LayerMask floorMask, obstaclesMask;
    [SerializeField] Transform buildingParent;

    private Building _preparingBuilding;
    private GameObject _preparingBuildingModel;
    private Vector3 _offset;
    private bool _inGoodPlace;

    [SerializeField] SelectionEffect selectionEffect;
    private Renderer _selectionRenderer;

    /// <returns>Building prefab | null</returns>
    public Building PrepareSpawn(string buildingName)
    {
        if (_preparingBuilding != null)
        {
            OnClick();
            return _preparingBuilding;
        }

        Building prefab = buildings.First(x => x.Name == buildingName);
        _preparingBuilding = prefab;

        Vector3 screenPosition = new Vector3(Screen.width / 2, Screen.height / 2);
        Vector3 position = GetClickPosition(screenPosition);
        _offset = prefab.Model.ModelPrefab.transform.localPosition + Vector3.up;
        _preparingBuildingModel = Instantiate(prefab.Model.ModelPrefab, position + _offset, prefab.Model.ModelPrefab.transform.rotation, buildingParent);

        _selectionRenderer = selectionEffect.SpawnSelection(_preparingBuildingModel.transform, prefab.Model.Size / _preparingBuildingModel.transform.localScale.x).GetComponent<Renderer>();
        _selectionRenderer.material.color = HasObstacles(screenPosition) ? Color.red : Color.green;

        return prefab;
    }
    public void CancelPreparing()
    {
        if (_preparingBuilding == null) return;

        _preparingBuilding = null;

        Destroy(_preparingBuildingModel.gameObject);
        _preparingBuildingModel = null;

        Destroy(_selectionRenderer.gameObject);
        _selectionRenderer = null;
    }
    public Building TryPlace()
    {
        if (!_inGoodPlace)
            return null;

        Entity result = EntitySpawner.Spawn(_preparingBuilding, _preparingBuildingModel.transform.position, _preparingBuilding.transform.rotation, 0);
        CancelPreparing();

        return (Building)result;
    }

    private Vector3 GetClickPosition(Vector3 pos)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(pos), out RaycastHit hit, 300, floorMask, QueryTriggerInteraction.Ignore))
            return hit.point;
        return Vector3.zero;
    }
    private bool HasObstacles(Vector3 pos)
        => Physics.Raycast(Camera.main.ScreenPointToRay(pos), 300, obstaclesMask, QueryTriggerInteraction.Ignore);

    public void OnClick()
    {
        if (!_preparingBuilding || EventSystem.current.IsPointerOverGameObject())
            return;

        Vector3 mousePosition = Input.mousePosition;
        _preparingBuildingModel.transform.position = GetClickPosition(mousePosition) + _offset;

        _inGoodPlace = !HasObstacles(mousePosition);
        _selectionRenderer.material.color = _inGoodPlace ? Color.green : Color.red;
    }
}
