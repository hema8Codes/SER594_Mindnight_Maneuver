using UnityEngine;
using UnityEngine.UI; // Use TMPro if using TextMeshPro

public class UIManager : MonoBehaviour
{
    public Text scoreText; // Reference to the UI Text element

    public void UpdateScoreUI(int newScore)
    {
        scoreText.text = "Score: " + newScore.ToString();
    }
}