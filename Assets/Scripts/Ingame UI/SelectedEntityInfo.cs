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
    // enity
    [SerializeField] Image entityImage, entityHealthBar;
    [SerializeField] TMP_Text entityTitle, entityHealthBarText;
    private Entity _curEntity;

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
        entityInfoWindow.SetActive(false);
        entitiesInfoWindow.SetActive(true);
    }

    public void SetEntity(Entity entity)
    {
        entitiesInfoWindow.SetActive(false);
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
