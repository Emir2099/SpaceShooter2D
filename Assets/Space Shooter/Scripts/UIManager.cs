using UnityEngine;
using TMPro;

/// <summary>
/// Manages all UI elements in the game
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Score UI")]
    public TextMeshProUGUI scoreText;
    
    public static UIManager instance;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    
    private void Start()
    {
        UpdateScore(Player.globalScore);
    }
    
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
