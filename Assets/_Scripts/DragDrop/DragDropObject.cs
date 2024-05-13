using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class DragDropObject : MonoBehaviour
{
    public int DragableIndex = 0;
    [SerializeField][ReadOnly] private bool _canDragable = true;

    private Vector2 _dragStartPosition;
    private bool _dragging;
    private ContainerObject _assignedContainer;

    [Foldout("Settings")]
    [SerializeField] private bool _returnStartOnFail = true;
    [Foldout("Settings")]
    [SerializeField] private bool _snapToContainer = true;

    [Foldout("Movement")]
    public float MoveSpeed = 5;
    [Foldout("Movement")]
    public Ease MoveEase;

    [Foldout("Events")]
    public UnityEvent OnDragBegin;
    [Foldout("Events")]
    public UnityEvent OnDrag;
    [Foldout("Events")]
    public UnityEvent OnDragEnd;
    [Foldout("Events")]
    public UnityEvent OnDropFail;
    [Foldout("Events")]
    public UnityEvent OnDropSuccess;
    [Foldout("Events")]
    public UnityEvent OnReturnedToStart;
    [Foldout("Events")]
    public UnityEvent OnContainerAssigned;

    #region UnityFunctions
    void OnEnable()
    {
        OnDrag.AddListener(FollowMouse);
    }
    void OnDisable()
    {
        OnDrag.RemoveListener(FollowMouse);
    }
    void Update()
    {
        Drag();
    }
    #endregion

    //! You have to add this function to TapHoldObject's OnHoldBegin UnityEvent
    public void BeginDrag()
    {
        if (_canDragable is not true) return;

        if (_assignedContainer != null)
        {
            _assignedContainer.OnDraggablePick?.Invoke(this);
        }

        _dragStartPosition = transform.position;
        _dragging = true;
        OnDragBegin?.Invoke();
    }
    private void Drag()
    {
        if (_dragging is not true) return;
        OnDrag?.Invoke();
    }

    //! You have to add this function to TapHoldObject's OnHoldCompleted UnityEvent
    public void EndDrag()
    {
        if (_dragging is not true) return;

        _dragging = false;
        ContainerObject container = DragDropManager.Instance.GetContainer();
        Drop(container);
        OnDragEnd?.Invoke();
    }

    public void Drop(ContainerObject container)
    {
        transform.DOKill();
        if (container != null && container.CheckIndex(DragableIndex))
        {
            if (_snapToContainer)
                MovePosition(container.transform.position, false);

            AssignToContainer(container);
            OnDropSuccess?.Invoke();
        }
        else
        {
            if (_returnStartOnFail)
            {
                ReturnToStart();
                AssignToContainer(_assignedContainer);
            }

            OnDropFail?.Invoke();
        }
    }

    private void FollowMouse()
    {
        Vector2 mousePosition = DragDropManager.Instance.GetMousePosition();
        transform.position = mousePosition;
    }

    private void ReturnToStart() => MovePosition(_dragStartPosition, true);

    private void MovePosition(Vector2 position, bool invokeEvent)
    {
        ToggleDraggable(false);
        float travelTime = Vector2.Distance(transform.position, position) / MoveSpeed;
        transform.DOMove(position, travelTime).SetEase(MoveEase).OnComplete(() =>
        {
            ToggleDraggable(true);
            if (invokeEvent)
                OnReturnedToStart?.Invoke();
        });
    }

    private void AssignToContainer(ContainerObject newContainer)
    {
        _assignedContainer = newContainer;
        OnContainerAssigned?.Invoke();
        _assignedContainer.OnDraggableDrop?.Invoke(this);
    }

    public ContainerObject GetAssignContainer() { return _assignedContainer; }
    public void ToggleDraggable(bool state) => _canDragable = state;
}
