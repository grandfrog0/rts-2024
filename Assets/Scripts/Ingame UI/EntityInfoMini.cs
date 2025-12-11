using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityInfoMini : MonoBehaviour
{
    public Image Icon;
    public Image HealthBar;
    private Entity _entity;

    public void Subscribe(Entity entity)
    {
        _entity = entity;
        _entity.OnHealthChanged.AddListener(UpdateHealth);
    }
    public void Describe()
    {
        if (_entity != null)
            _entity.OnHealthChanged.RemoveListener(UpdateHealth);
    }

    private void UpdateHealth()
    {
        HealthBar.fillAmount = _entity.Health / _entity.MaxHealth;
    }
}