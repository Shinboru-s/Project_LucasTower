using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
    }

    [SerializeField] private TextMeshProUGUI _levelIndexText;
    [SerializeField] private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private TextMeshProUGUI _stepIndexText;
    [SerializeField] private TapHoldWindow _tapHoldWindow;

    private int _stepCount;
    private bool _canCount = true;

    public void CountStep()
    {
        if (_canCount is not true) return;
        _stepCount++;
        SetStepUI();
    }
    public void ResetStep()
    {
        _stepCount = 0;
        SetStepUI();
    }

    public void SetLevelIndex(string level) => _levelIndexText.text = level;
    public void ToggleCountable(bool state) => _canCount = state;
    public void ToggleRaycaster(bool state) => _graphicRaycaster.enabled = state;
    private void SetStepUI() => _stepIndexText.text = _stepCount.ToString();
    public void ToggleDraggable(bool state) => _tapHoldWindow.ToggleInteract(state);
}
