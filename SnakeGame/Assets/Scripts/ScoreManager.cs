using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    
    public static int score = 0;
    public TextMeshProUGUI scoreText;

    public static void AddScore(int amount)
    {
        score += amount;
    }

    void Start()
    {
        scoreText.text = $"Score: {ScoreManager.score}";
    }

}
