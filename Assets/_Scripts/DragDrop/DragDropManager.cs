using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-5)]
public class DragDropManager : MonoBehaviour
{
    public static DragDropManager Instance;
    void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
    }

    private Camera _mainCamera;
    private List<ContainerObject> _containerObjects = new();

    public void AddContainerObject(ContainerObject container) => _containerObjects.Add(container);
    public void RemoveContainerObject(ContainerObject container) => _containerObjects.Remove(container);

    public ContainerObject GetContainer()
    {
        Vector2 mousePosition = GetMousePosition();
        foreach (var container in _containerObjects)
        {
            if (container.CheckContainer(mousePosition))
            {

                return container;
            }
        }
        Debug.Log("Container not found in position!");
        return null;
    }

    public Vector2 GetMousePosition()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;

        return _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
