using UnityEngine;
using UnityEngine.SceneManagement;

// Manager for non-singleton managers.
public class Managers : MonoBehaviour
{
    public static Managers Instance;
    
    public UIManager UI;
    public BattleManager Battle;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UI = FindObjectOfType<UIManager>();
        Battle = FindObjectOfType<BattleManager>();
    }
}
