using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityInfoMini : MonoBehaviour
{
    public Image Icon;
    public Image HealthBar;
    public Image ColorViewer;
    private Entity _entity;

    public void Subscribe(Entity entity)
    {
        _entity = entity;
        _entity.OnHealthChanged.AddListener(UpdateHealth);
        _entity.OnDead.AddListener(OnDead);
    }
    public void Describe()
    {
        if (_entity != null)
        {
            _entity.OnHealthChanged.RemoveListener(UpdateHealth);
            _entity.OnDead.RemoveListener(OnDead);
        }
    }

    private void UpdateHealth()
    {
        HealthBar.fillAmount = _entity.Health / _entity.MaxHealth;
    }
    private void OnDead()
    {
        gameObject.SetActive(false);
    }
}