using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterManager : MonoBehaviour
{
    [Header("Character Data")]
    [Tooltip("Reference to the character database ScriptableObject")]
    public CharacterDatabase characterDatabase;
    
    [Header("Player References")]
    [Tooltip("The Player GameObject whose sprite will be changed")]
    public GameObject playerObject;
    
    [Tooltip("SpriteRenderer component of the player (auto-found if not assigned)")]
    public SpriteRenderer playerSpriteRenderer;
    
    private void Start()
    {
        Debug.Log("PlayerCharacterManager: Starting character application process...");
        
        // Validate character database
        if (characterDatabase == null)
        {
            Debug.LogError("PlayerCharacterManager: Character Database is not assigned!");
            return;
        }
        
        if (!characterDatabase.IsValid())
        {
            Debug.LogError("PlayerCharacterManager: Character Database is not valid!");
            return;
        }
        
        Debug.Log($"PlayerCharacterManager: Character Database loaded with {characterDatabase.characters.Length} characters");
        
        // Auto-find player sprite renderer if not assigned
        if (playerSpriteRenderer == null && playerObject != null)
        {
            playerSpriteRenderer = playerObject.GetComponent<SpriteRenderer>();
            Debug.Log("PlayerCharacterManager: Found SpriteRenderer from assigned playerObject");
        }
        
        // If still null, try to find Player by tag or name
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Debug.Log("PlayerCharacterManager: Found Player by tag");
            }
            else
            {
                playerObject = GameObject.Find("Player");
                if (playerObject != null)
                {
                    Debug.Log("PlayerCharacterManager: Found Player by name");
                }
            }
        }
        
        // Get sprite renderer from found player
        if (playerObject != null && playerSpriteRenderer == null)
        {
            playerSpriteRenderer = playerObject.GetComponent<SpriteRenderer>();
            if (playerSpriteRenderer != null)
            {
                Debug.Log("PlayerCharacterManager: Found SpriteRenderer component");
            }
        }
        
        // Debug what we found
        Debug.Log($"PlayerCharacterManager: Player Object = {(playerObject != null ? playerObject.name : "NULL")}");
        Debug.Log($"PlayerCharacterManager: SpriteRenderer = {(playerSpriteRenderer != null ? "Found" : "NULL")}");
        Debug.Log($"PlayerCharacterManager: Available Characters Count = {characterDatabase.characters.Length}");
        
        // Apply the selected character
        ApplySelectedCharacter();
    }
    
    private void ApplySelectedCharacter()
    {
        // Get the selected character data
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        Debug.Log($"PlayerCharacterManager: Selected character index from PlayerPrefs = {selectedIndex}");
        
        CharacterData selectedCharacter = CharacterSelector.GetSelectedCharacter(characterDatabase);
        
        if (selectedCharacter != null)
        {
            Debug.Log($"PlayerCharacterManager: Selected character = {selectedCharacter.characterName}");
            Debug.Log($"PlayerCharacterManager: Character sprite = {(selectedCharacter.characterSprite != null ? selectedCharacter.characterSprite.name : "NULL")}");
        }
        else
        {
            Debug.LogError("PlayerCharacterManager: No character selected or character data not found!");
        }
        
        if (selectedCharacter != null && playerSpriteRenderer != null)
        {
            // Store original sprite for comparison
            Sprite originalSprite = playerSpriteRenderer.sprite;
            
            // Change the player sprite
            playerSpriteRenderer.sprite = selectedCharacter.characterSprite;
            
            Debug.Log($"PlayerCharacterManager: Changed sprite from {(originalSprite != null ? originalSprite.name : "NULL")} to {selectedCharacter.characterSprite.name}");
            Debug.Log($"PlayerCharacterManager: Successfully applied character sprite: {selectedCharacter.characterName}");
        }
        else
        {
            if (selectedCharacter == null)
                Debug.LogError("PlayerCharacterManager: selectedCharacter is NULL!");
            if (playerSpriteRenderer == null)
                Debug.LogError("PlayerCharacterManager: playerSpriteRenderer is NULL!");
        }
    }
    
    // Optional: Method to change character mid-game
    public void ChangeCharacter(int characterIndex)
    {
        if (characterDatabase != null && characterIndex >= 0 && characterIndex < characterDatabase.characters.Length)
        {
            CharacterData newCharacter = characterDatabase.characters[characterIndex];
            if (playerSpriteRenderer != null && newCharacter.characterSprite != null)
            {
                playerSpriteRenderer.sprite = newCharacter.characterSprite;
                
                // Save the selection
                PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
                PlayerPrefs.Save();
                
                Debug.Log($"Character changed to: {newCharacter.characterName}");
            }
        }
        else
        {
            if (characterDatabase == null)
                Debug.LogError("PlayerCharacterManager: Character Database is null!");
            else
                Debug.LogError($"PlayerCharacterManager: Character index {characterIndex} is out of bounds!");
        }
    }
}
