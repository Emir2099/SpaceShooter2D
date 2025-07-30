using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button startButton;
    public Button selectCharacterButton;
    
    [Header("UI Panels")]
    [Tooltip("Main menu panel with start and select character buttons")]
    public GameObject mainMenuPanel;
    
    [Tooltip("Character selection panel (will be shown when Select Character is clicked)")]
    public GameObject characterSelectionPanel;
    
    [Header("Character Selection")]
    [Tooltip("Character Selector component")]
    public CharacterSelector characterSelector;
    
    [Header("Scene Settings")]
    [Tooltip("Name of the game scene to load when Start is pressed")]
    public string gameSceneName = "GameLevel";
    
    private void Start()
    {
        // Set up start button
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
        else
        {
            Debug.LogError("Start Button is not assigned in MenuManager!");
        }
        
        // Set up select character button
        if (selectCharacterButton != null)
        {
            selectCharacterButton.onClick.AddListener(OpenCharacterSelection);
        }
        else
        {
            Debug.LogError("Select Character Button is not assigned in MenuManager!");
        }
        
        // Make sure we start with main menu visible
        ShowMainMenu();
    }
    
    // Called when Start button is clicked
    public void StartGame()
    {
        Debug.Log("Start Game button clicked!");

        // Load the game scene
        SceneManager.LoadScene(gameSceneName);
    }
    
    // Called when Select Character button is clicked
    public void OpenCharacterSelection()
    {
        Debug.Log("Opening Character Selection!");
        
        // Hide main menu and show character selection
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
            
        if (characterSelectionPanel != null)
            characterSelectionPanel.SetActive(true);
    }
    
    // Called to return to main menu (from character selection)
    public void ShowMainMenu()
    {
        Debug.Log("Returning to Main Menu!");
        
        // Show main menu and hide character selection
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
            
        if (characterSelectionPanel != null)
            characterSelectionPanel.SetActive(false);
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
