using UnityEngine;

public class LucasObject : MonoBehaviour
{
    public DragDropObject DragDropScript;
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer _body;
    [SerializeField] private SpriteRenderer _rightEye;
    [SerializeField] private SpriteRenderer _rightEyeLid;
    [SerializeField] private SpriteRenderer _rightIris;
    [SerializeField] private SpriteRenderer _leftEye;
    [SerializeField] private SpriteRenderer _leftEyeLid;
    [SerializeField] private SpriteRenderer _leftIris;
    [SerializeField] private SpriteRenderer _mouth;

    private ContainerObject _lastContainer;

    void Awake()
    {
        DragDropScript.OnDropSuccess.AddListener(() => { SuccessEvents(); });
        DragDropScript.OnDragBegin.AddListener(() =>
        {
            SetHoldingOrderInLayer();
            AudioManager.Instance.Pick.Play();
        });
        DragDropScript.OnDragEnd.AddListener(() =>
        {
            SetDefaultOrderInLayer();
            AudioManager.Instance.Drop.Play();
        });
    }
    public void SuccessEvents()
    {
        LucasManager.Instance.CheckLevel();
        CheckLastContainer();
        _lastContainer = DragDropScript.GetAssignContainer();
    }
    private void CheckLastContainer()
    {
        if (_lastContainer != DragDropScript.GetAssignContainer())
        {
            UIManager.Instance.CountStep();
        }
    }
    public int GetDragableIndex() => DragDropScript.DragableIndex;
    public void SetDragableIndex(int index) => DragDropScript.DragableIndex = index;
    public void Initialize()
    {
        int draggableIndex = GetDragableIndex();

        // Object size
        _body.size = new Vector2(draggableIndex * LucasObjectInfo.GrowthValue, _body.size.y);

        // Collider size
        _collider.size = new Vector2(_body.size.x * 0.4f, _body.size.y * 0.4f);

        // Eye position 
        float halfBodySize = 0.4f * draggableIndex * LucasObjectInfo.GrowthValue / 2f;
        float randomEyePosition = Random.Range(0.4f, halfBodySize - LucasObjectInfo.EyeDistanceFromEdge);
        _rightEye.transform.localPosition = new Vector2(randomEyePosition, 0.2f);
        _leftEye.transform.localPosition = new Vector2(-randomEyePosition, 0.2f);

        // Mouth
        _mouth.sprite = LucasManager.Instance.GetRandomMouth();

        // Color
        Color color = LucasManager.Instance.GetRandomColor();
        _body.color = color;
        _leftEyeLid.color = color;
        _rightEyeLid.color = color;
        _mouth.color = color;

        // Iris
        float randomIrisPositionX = Random.Range(-LucasObjectInfo.MaxIrisDistance.x, LucasObjectInfo.MaxIrisDistance.x);
        float randomIrisPositionY = Random.Range(0, LucasObjectInfo.MaxIrisDistance.y);
        _rightIris.transform.localPosition = new Vector3(randomIrisPositionX, -randomIrisPositionY, _rightIris.transform.localPosition.z);
        _leftIris.transform.localPosition = new Vector3(randomIrisPositionX, -randomIrisPositionY, _leftIris.transform.localPosition.z);

        SetOrderInLayer(draggableIndex);
    }
    public void SetHoldingOrderInLayer() => SetOrderInLayer(10);
    public void SetDefaultOrderInLayer()
    {
        int draggableIndex = GetDragableIndex();
        SetOrderInLayer(draggableIndex);
    }
    private void SetOrderInLayer(int index)
    {
        _body.sortingOrder = index;
        _rightEye.sortingOrder = index;
        _rightEyeLid.sortingOrder = index;
        _rightIris.sortingOrder = index;
        _leftEye.sortingOrder = index;
        _leftEyeLid.sortingOrder = index;
        _leftIris.sortingOrder = index;
        _mouth.sortingOrder = index;
    }
}
