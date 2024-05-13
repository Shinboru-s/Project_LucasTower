using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-5)]
public class TapHoldManager : MonoBehaviour
{
    public static TapHoldManager Instance;
    void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
    }

    private List<TapHoldWorldObject> _tapHoldObjects = new();
    private TapHoldWorldObject _interactedObject;

    public void AddTapHoldObject(TapHoldWorldObject tapHoldWorldObject) => _tapHoldObjects.Add(tapHoldWorldObject);
    public void RemoveTapHoldObject(TapHoldWorldObject tapHoldWorldObject) => _tapHoldObjects.Remove(tapHoldWorldObject);

    public void TryToInteract(Vector2 mouseWorldPosition)
    {
        _interactedObject = null;
        foreach (var tapHoldObject in _tapHoldObjects)
        {
            if (tapHoldObject.CheckTouchPosition(mouseWorldPosition))
            {
                _interactedObject = tapHoldObject;
                _interactedObject.Interact();
                break;
            }
        }
    }

    public void EndInteraction(Vector2 mouseWorldPosition)
    {
        if (_interactedObject == null) return;
        _interactedObject.EndInteract(mouseWorldPosition);
    }
}
