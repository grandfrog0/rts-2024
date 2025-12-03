using UnityEngine;
using UnityEngine.EventSystems;

public class EntityOutline : MonoBehaviour
{
    [SerializeField] LayerMask objectLayerMask;
    private Camera _camera;

    public static Entity SelectedEntity { get; private set; }

    private void HandleObjectClick(Entity entity)
    {
        SelectedEntity = entity == SelectedEntity ? null : entity;
        Debug.Log("Обрабатываем клик по: " + entity.name);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ui click ignore
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, objectLayerMask))
            {
                Debug.Log("Попал по объекту: " + hit.collider.gameObject.name);
                if (hit.collider.TryGetComponent(out Entity entity))
                {
                    HandleObjectClick(entity);
                }
            }
        }
    }

    private void Start()
    {
        _camera = Camera.main;
    }
}