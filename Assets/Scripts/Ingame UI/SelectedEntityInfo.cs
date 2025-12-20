using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectedEntityInfo : MonoBehaviour
{
    [SerializeField] GameObject entityInfoWindow, entitiesInfoWindow, resourcesViewWindow;
    // entity
    [SerializeField] Image entityImage, entityHealthBar, entityColorViewer;
    [SerializeField] TMP_Text entityTitle, entityHealthBarText;
    private Entity _curEntity;
    // entities
    [SerializeField] EntityInfoMini entityInfoMini;
    private List<EntityInfoMini> _entities = new();

    public List<Color> TeamColors { get; set; }

    public void OnEntityChanged(HashSet<Entity> entities)
    {
        if (entities.Count == 0)
        {
            ClearInterface();
        }
        else if (entities.Count == 1)
        {
            SetEntity(entities.First());
        }
        else
        {
            SetEntities(entities);
        }
    }

    public void SetEntities(HashSet<Entity> entities)
    {
        UnloadEntity();
        ClearEntities();
        entitiesInfoWindow.SetActive(true);

        foreach(var entity in entities)
        {
            EntityInfoMini info = Instantiate(entityInfoMini, entitiesInfoWindow.transform);
            info.Icon.sprite = entity.Icon;
            info.HealthBar.fillAmount = entity.Health / entity.MaxHealth;

            info.ColorViewer.color = entity.TeamID != -1 ? TeamColors[entity.TeamID] : Color.clear;
            info.Subscribe(entity);
            _entities.Add(info);
        }
    }
    private void UnloadEntities()
    {
        entitiesInfoWindow.SetActive(false);
        ClearEntities();
    }
    private void ClearEntities()
    {
        foreach (EntityInfoMini info in _entities)
        {
            info.Describe();
            Destroy(info.gameObject);
        }
        _entities.Clear();
    }
    private void UnloadEntity()
    {
        entityInfoWindow.SetActive(false);
    }

    public void SetEntity(Entity entity)
    {
        UnloadEntities();
        entityInfoWindow.SetActive(true);

        if (_curEntity && _curEntity != entity)
            _curEntity.OnHealthChanged.RemoveListener(UpdateEntityHealth);

        _curEntity = entity;
        _curEntity.OnHealthChanged.AddListener(UpdateEntityHealth);

        entityImage.sprite = entity.Icon;
        entityTitle.text = entity.Name;
        entityColorViewer.color = entity.TeamID != -1 ? TeamColors[entity.TeamID] : Color.clear;
        UpdateEntityHealth();

        resourcesViewWindow.SetActive(entity is Builder);
    }

    public void UpdateEntityHealth()
    {
        entityHealthBar.fillAmount = _curEntity.Health / _curEntity.MaxHealth;
        entityHealthBarText.text = $"{_curEntity.Health}/{_curEntity.MaxHealth}";
    }
    public void ClearInterface()
    {
        entityInfoWindow.SetActive(false);
        entitiesInfoWindow.SetActive(false);
    }
}
