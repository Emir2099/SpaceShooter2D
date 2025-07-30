using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Space Shooter/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    [Header("Available Characters")]
    [Tooltip("List of all available characters in the game")]
    public CharacterData[] characters;
    
    [Header("Default Character")]
    [Tooltip("Index of the default character (used when no selection is made)")]
    public int defaultCharacterIndex = 0;
    
    /// <summary>
    /// Get character by index with bounds checking
    /// </summary>
    public CharacterData GetCharacter(int index)
    {
        if (characters != null && index >= 0 && index < characters.Length)
        {
            return characters[index];
        }
        
        Debug.LogWarning($"CharacterDatabase: Invalid character index {index}. Available characters: {(characters != null ? characters.Length : 0)}");
        return null;
    }
    
    /// <summary>
    /// Get the currently selected character from PlayerPrefs
    /// </summary>
    public CharacterData GetSelectedCharacter()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", defaultCharacterIndex);
        return GetCharacter(selectedIndex);
    }
    
    /// <summary>
    /// Get the default character
    /// </summary>
    public CharacterData GetDefaultCharacter()
    {
        return GetCharacter(defaultCharacterIndex);
    }
    
    /// <summary>
    /// Validate the database setup
    /// </summary>
    public bool IsValid()
    {
        if (characters == null || characters.Length == 0)
        {
            Debug.LogError("CharacterDatabase: No characters defined!");
            return false;
        }
        
        if (defaultCharacterIndex < 0 || defaultCharacterIndex >= characters.Length)
        {
            Debug.LogError($"CharacterDatabase: Default character index {defaultCharacterIndex} is out of bounds!");
            return false;
        }
        
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == null)
            {
                Debug.LogError($"CharacterDatabase: Character at index {i} is null!");
                return false;
            }
            
            if (characters[i].characterSprite == null)
            {
                Debug.LogError($"CharacterDatabase: Character '{characters[i].characterName}' has no sprite assigned!");
                return false;
            }
        }
        
        return true;
    }
}
