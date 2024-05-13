using Criaath;
using Criaath.Extensions;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Linq;
using UnityEngine;

public class LucasManager : MonoBehaviour
{
    public static LucasManager Instance;
    void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;

        Initialize();
    }
    [SerializeField] private LucasStick[] _lucasSticks;
    [MinValue(3), MaxValue(5)][SerializeField] private int _startLevel;

    [Foldout("Level Settings")]
    [SerializeField] private GameObject _lucasObjectPrefab;
    [Foldout("Level Settings")]
    [SerializeField] private LucasStick _spawnStick;
    [Foldout("Level Settings")]
    [SerializeField] private LucasStick _targetStick;


    [Foldout("Lucas Object Settings")]
    [SerializeField] private Sprite[] _mouthSprites;
    [Foldout("Lucas Object Settings")]
    [SerializeField] private Color[] _bodyColors;

    [Foldout("Level Complete")]
    [SerializeField] private ParticleSystem[] _particles;
    [Foldout("Level Complete")]
    [SerializeField] private RectTransform _completeText;

    private int _spawnCount;

    private ObjectPool<LucasObject> _lucasObjectPool;
    private int _minLevel = 3;
    private int _maxLevel = 5;
    private int _levelIndex;
    private Sequence _levelCompleteSequence;

    void Start()
    {
        _levelIndex = _startLevel;
        UIManager.Instance.SetLevelIndex(_levelIndex.ToString());
        Spawn(_levelIndex);

    }
    private void Initialize()
    {
        _lucasObjectPool = new ObjectPool<LucasObject>(_lucasObjectPrefab, transform, 5);
        _bodyColors = _bodyColors.Shuffle();
    }
    public void ChangeLevel(int change)
    {
        if (_minLevel > _levelIndex + change || _levelIndex + change > _maxLevel) return;

        ResetLucasSticks();

        _levelIndex += change;
        UIManager.Instance.SetLevelIndex(_levelIndex.ToString());
        Spawn(_levelIndex);

    }
    public Sprite GetRandomMouth() => _mouthSprites[Random.Range(0, _mouthSprites.Length)];
    public Color GetRandomColor()
    {
        _spawnCount++;
        _spawnCount %= _bodyColors.Length;
        return _bodyColors[_spawnCount];
    }

    public void Spawn(int objectCount)
    {
        _lucasObjectPool.PushAllItems();
        UIManager.Instance.ResetStep();
        StartCoroutine(SpawnRoutine(objectCount));
    }
    private IEnumerator SpawnRoutine(int objectCount)
    {
        UIManager.Instance.ToggleRaycaster(false);
        UIManager.Instance.ToggleCountable(false);
        UIManager.Instance.ToggleDraggable(false);
        for (int i = objectCount; i > 0; i--)
        {
            LucasObject lucasObject = _lucasObjectPool.Pull();
            lucasObject.SetDragableIndex(i);
            lucasObject.Initialize();
            lucasObject.gameObject.SetActive(true);
            lucasObject.gameObject.transform.position = _spawnStick.transform.position + new Vector3(0, 3.5f);
            lucasObject.DragDropScript.Drop(_spawnStick.GetContainer());
            float travelTime = 3.5f / lucasObject.DragDropScript.MoveSpeed;
            yield return new WaitForSeconds(travelTime + 0.2f);
        }
        UIManager.Instance.ToggleRaycaster(true);
        UIManager.Instance.ToggleCountable(true);
        UIManager.Instance.ToggleDraggable(true);
    }

    public void ResetLucasSticks()
    {
        foreach (var stick in _lucasSticks)
        {
            stick.ResetStick();
        }
    }
    public void CheckLevel()
    {
        if (_levelIndex == _targetStick.GetObjectCount())
            LevelCompleted();
    }

    public void LevelCompleted()
    {
        foreach (var particle in _particles)
        {
            particle.Play();
        }
        UIManager.Instance.ToggleDraggable(false);
        CompleteTextAnimation();
        AudioManager.Instance.Complete.Play();
    }
    private void CompleteTextAnimation()
    {
        _levelCompleteSequence = DOTween.Sequence();
        _levelCompleteSequence.Append(_completeText.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack));
        _levelCompleteSequence.Append(_completeText.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.InBack)
            .SetDelay(1f));
        _levelCompleteSequence.Play();

    }

}
