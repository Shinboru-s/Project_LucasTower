using System;
using System.Linq;
using UnityEngine;

public class ContainerObject : MonoBehaviour
{
    public int[] ContainableIndexes = new int[] { 0 };
    [SerializeField] private Collider2D _collider;

    public Action<DragDropObject> OnDraggableDrop;
    public Action<DragDropObject> OnDraggablePick;

    void OnEnable() => DragDropManager.Instance.AddContainerObject(this);
    void OnDisable() => DragDropManager.Instance.RemoveContainerObject(this);
    public bool CheckContainer(Vector2 mousePosition) => _collider.bounds.Contains(mousePosition);
    public bool CheckIndex(int dragableIndex) => ContainableIndexes.Contains(dragableIndex);
    public void SetContainableIndexes(int[] containableIndexes) => ContainableIndexes = containableIndexes;

}
