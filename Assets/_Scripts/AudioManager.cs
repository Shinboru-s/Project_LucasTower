using Criaath.Goldio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
    }
    public GoldioPlayer Complete;
    public GoldioPlayer Drop;
    public GoldioPlayer Pick;
}
