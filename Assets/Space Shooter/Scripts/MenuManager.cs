using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    
    [Header("Scene Settings")]
    [Tooltip("Name of the game scene to load when Start is pressed")]
    public string gameSceneName = "GameLevel";
    
    private void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogError("Start Button is not assigned in MenuManager!");
        }
    }
    
    // Called when Start button is clicked
    public void StartGame()
    {
        Debug.Log("Start Game button clicked!");

        // Load the game scene
        SceneManager.LoadScene(gameSceneName);
    }
    
    public void QuitGame()
    {
        Debug.Log("Quit Game button clicked!");
        
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
