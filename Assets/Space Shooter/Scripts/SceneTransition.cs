using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition instance;
    
    [Header("Transition Settings")]
    public float transitionDelay = 0.5f;
    
    private void Awake()
    {
        // Singleton pattern to persist between scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Load scene with optional delay
    public void LoadScene(string sceneName, float delay = 0f)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, delay));
    }
    
    private IEnumerator LoadSceneCoroutine(string sceneName, float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
    
    // Quick access methods for common scene transitions
    public static void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public static void LoadGameLevel()
    {
        SceneManager.LoadScene("GameLevel");
    }
}
