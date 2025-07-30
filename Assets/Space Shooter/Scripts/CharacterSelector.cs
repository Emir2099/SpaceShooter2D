using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterData
{
    [Tooltip("Name of the character/spaceship")]
    public string characterName;
    
    [Tooltip("Sprite to use for this character in game")]
    public Sprite characterSprite;
    
    [Tooltip("Preview image for character selection UI")]
    public Sprite previewSprite;
    
    [Tooltip("Optional description")]
    public string description;
}

public class CharacterSelector : MonoBehaviour
{
    [Header("Character Data")]
    [Tooltip("Reference to the character database ScriptableObject")]
    public CharacterDatabase characterDatabase;
    
    [Header("UI Elements")]
    [Tooltip("Image that shows the current character preview")]
    public Image characterPreviewImage;
    
    [Tooltip("Text that shows the character name")]
    public Text characterNameText;
    
    [Tooltip("Text that shows the character description")]
    public Text characterDescriptionText;
    
    [Tooltip("Button to go to previous character")]
    public Button previousButton;
    
    [Tooltip("Button to go to next character")]
    public Button nextButton;
    
    [Tooltip("Button to select current character and start game")]
    public Button selectButton;
    
    [Tooltip("Button to go back to main menu")]
    public Button backButton;
    
    [Header("Settings")]
    private int currentCharacterIndex = 0;
    
    private void Start()
    {
        // Validate character database
        if (characterDatabase == null)
        {
            Debug.LogError("CharacterSelector: Character Database is not assigned!");
            return;
        }
        
        if (!characterDatabase.IsValid())
        {
            Debug.LogError("CharacterSelector: Character Database is not valid!");
            return;
        }
        
        // Set up button listeners
        if (previousButton != null)
            previousButton.onClick.AddListener(PreviousCharacter);
            
        if (nextButton != null)
            nextButton.onClick.AddListener(NextCharacter);
            
        if (selectButton != null)
            selectButton.onClick.AddListener(SelectCharacter);
            
        if (backButton != null)
            backButton.onClick.AddListener(GoBackToMainMenu);
        
        // Load saved character selection or default to first
        currentCharacterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        
        // Make sure index is valid
        if (currentCharacterIndex >= characterDatabase.characters.Length)
            currentCharacterIndex = 0;
            
        // Update UI
        UpdateCharacterDisplay();
    }
    
    public void NextCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex + 1) % characterDatabase.characters.Length;
        UpdateCharacterDisplay();
    }
    
    public void PreviousCharacter()
    {
        currentCharacterIndex = (currentCharacterIndex - 1 + characterDatabase.characters.Length) % characterDatabase.characters.Length;
        UpdateCharacterDisplay();
    }
    
    private void UpdateCharacterDisplay()
    {
        if (characterDatabase.characters.Length == 0) return;
        
        CharacterData currentCharacter = characterDatabase.characters[currentCharacterIndex];
        
        // Update preview image
        if (characterPreviewImage != null && currentCharacter.previewSprite != null)
        {
            characterPreviewImage.sprite = currentCharacter.previewSprite;
        }
        
        // Update character name
        if (characterNameText != null)
        {
            characterNameText.text = currentCharacter.characterName;
        }
        
        // Update description
        if (characterDescriptionText != null)
        {
            characterDescriptionText.text = currentCharacter.description;
        }
        
        Debug.Log($"Character selected: {currentCharacter.characterName}");
    }
    
    public void SelectCharacter()
    {
        // Save the selected character
        PlayerPrefs.SetInt("SelectedCharacter", currentCharacterIndex);
        PlayerPrefs.Save();
        
        Debug.Log($"Character {characterDatabase.characters[currentCharacterIndex].characterName} selected and saved!");
        
        // Trigger start game from MenuManager
        FindObjectOfType<MenuManager>()?.StartGame();
    }
    
    public void GoBackToMainMenu()
    {
        Debug.Log("Going back to main menu from character selection");
        
        // Tell MenuManager to show main menu
        FindObjectOfType<MenuManager>()?.ShowMainMenu();
    }
    
    // Static method to get the selected character data (used by game scene)
    public static CharacterData GetSelectedCharacter(CharacterDatabase database)
    {
        if (database == null)
        {
            Debug.LogError("CharacterSelector.GetSelectedCharacter: Character database is NULL!");
            return null;
        }
        
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        Debug.Log($"CharacterSelector.GetSelectedCharacter: Selected index = {selectedIndex}");
        
        CharacterData selectedCharacter = database.GetCharacter(selectedIndex);
        
        if (selectedCharacter != null)
        {
            Debug.Log($"CharacterSelector.GetSelectedCharacter: Returning character '{selectedCharacter.characterName}'");
            Debug.Log($"CharacterSelector.GetSelectedCharacter: Character sprite = {(selectedCharacter.characterSprite != null ? selectedCharacter.characterSprite.name : "NULL")}");
        }
        else
        {
            Debug.LogError($"CharacterSelector.GetSelectedCharacter: Failed to get character at index {selectedIndex}");
        }
        
        return selectedCharacter;
    }
}
