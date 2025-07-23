using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Advanced heart UI manager that handles multiple rows of hearts
/// </summary>
public class HeartUIManager : MonoBehaviour
{
    [Header("Heart UI Settings")]
    public GameObject heartPrefab;
    public Transform heartContainer;
    public int heartsPerRow = 5; // Maximum hearts per row
    public float rowSpacing = 10f; // Spacing between rows
    
    private List<GameObject> heartSprites = new List<GameObject>();
    private List<Transform> rows = new List<Transform>();
    
    public void InitializeHearts(int maxHealth)
    {
        ClearHearts();
        CreateHeartRows(maxHealth);
    }
    
    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < heartSprites.Count; i++)
        {
            if (i < currentHealth)
                heartSprites[i].SetActive(true);
            else
                heartSprites[i].SetActive(false);
        }
    }
    
    public void AddHearts(int newMaxHealth)
    {
        int heartsToAdd = newMaxHealth - heartSprites.Count;
        for (int i = 0; i < heartsToAdd; i++)
        {
            CreateHeart(heartSprites.Count);
        }
    }
    
    private void CreateHeartRows(int totalHearts)
    {
        int currentRow = 0;
        
        for (int i = 0; i < totalHearts; i++)
        {
            // Create new row if needed
            if (i % heartsPerRow == 0)
            {
                CreateNewRow(currentRow);
                currentRow++;
            }
            
            CreateHeart(i);
        }
    }
    
    private void CreateNewRow(int rowIndex)
    {
        GameObject rowObject = new GameObject($"HeartRow{rowIndex}");
        rowObject.transform.SetParent(heartContainer);
        
        // Add horizontal layout group to row
        HorizontalLayoutGroup layoutGroup = rowObject.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.spacing = 5f;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        
        // Position row
        RectTransform rectTransform = rowObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.anchoredPosition = new Vector2(0, -rowIndex * rowSpacing);
        
        rows.Add(rowObject.transform);
    }
    
    private void CreateHeart(int heartIndex)
    {
        int rowIndex = heartIndex / heartsPerRow;
        Transform targetRow = rows[rowIndex];
        
        GameObject heart = Instantiate(heartPrefab, targetRow);
        heartSprites.Add(heart);
    }
    
    private void ClearHearts()
    {
        foreach (GameObject heart in heartSprites)
        {
            if (heart != null) Destroy(heart);
        }
        heartSprites.Clear();
        
        foreach (Transform row in rows)
        {
            if (row != null) Destroy(row.gameObject);
        }
        rows.Clear();
    }
}
