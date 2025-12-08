using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectedEntityInfo : MonoBehaviour
{
    [SerializeField] GameObject entityInfoWindow, entitiesInfoWindow;
    // entity
    [SerializeField] Image entityImage, entityHealthBar;
    [SerializeField] TMP_Text entityTitle, entityHealthBarText;
    private Entity _curEntity;
    // entities
    [SerializeField] EntityInfoMini entityInfoMini;
    private List<EntityInfoMini> _entities = new();

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
        foreach (EntityInfoMini entity in _entities)
        {
            Destroy(entity.gameObject);
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
        UpdateEntityHealth();
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
