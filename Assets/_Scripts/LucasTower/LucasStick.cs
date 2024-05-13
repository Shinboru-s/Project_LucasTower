using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LucasStick : MonoBehaviour
{
    [SerializeField] private ContainerObject _container;
    [SerializeField] private float _moveValue;
    [SerializeField] private List<LucasObject> _draggableObjectVisiable = new(); // For Inspector view

    private Stack<LucasObject> _dragableObjects = new();

    #region Built-in
    void Awake()
    {
        _container.OnDraggablePick += ObjectPick;
        _container.OnDraggableDrop += ObjectDrop;
    }
    void Start()
    {
        SetContainableIndex();


    }
    #endregion

    public ContainerObject GetContainer() { return _container; }
    private void MoveContainer(float moveValue) => _container.transform.position += new Vector3(0, moveValue);
    public int GetObjectCount() { return _dragableObjects.Count; }

    private void ObjectPick(DragDropObject draggableObject)
    {
        _dragableObjects.Pop();
        _draggableObjectVisiable.Remove(draggableObject.GetComponent<LucasObject>());
        MoveContainer(-_moveValue);
        if (_dragableObjects.TryPeek(out LucasObject bottomObject))
            bottomObject.DragDropScript.ToggleDraggable(true);
        Debug.Log(draggableObject.name + " Picked!");

        SetContainableIndex();
    }

    public void ObjectDrop(DragDropObject draggableObject)
    {
        if (_dragableObjects.TryPeek(out LucasObject bottomObject))
            bottomObject.DragDropScript.ToggleDraggable(false);

        LucasObject droppedObject = draggableObject.GetComponent<LucasObject>();
        _dragableObjects.Push(droppedObject);
        _draggableObjectVisiable.Add(droppedObject);
        Debug.Log(droppedObject.name + " Dropped!");
        MoveContainer(_moveValue);
        SetContainableIndex();
    }


    private void SetContainableIndex()
    {
        int bottomIndex;

        if (_dragableObjects.TryPeek(out LucasObject bottomObject))
            bottomIndex = bottomObject.GetDragableIndex() - 1;
        else
            bottomIndex = 10;

        int[] containableIndexes = Enumerable.Range(1, bottomIndex).Reverse().ToArray();

        _container.SetContainableIndexes(containableIndexes);
    }

    public void ResetStick()
    {
        for (int i = 0; i <= 10; i++)
        {
            if (_dragableObjects.TryPeek(out LucasObject bottomObject))
                ObjectPick(bottomObject.DragDropScript);
        }
    }
}
