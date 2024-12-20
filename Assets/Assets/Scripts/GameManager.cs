using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI cashText; // Assign the MoneyDisplay TextMeshPro element in the Inspector
    private int totalCash = 0; // Tracks total cash earned

    public GameObject gameOverUI; // Assign the Game Over UI panel in the Inspecto
    public GameObject mainMenu;  // Assign the Main Menu Panel in the Inspector

public void AddCash(int amount)
{
    totalCash += amount; // Add the earned cash to the total
    Debug.Log("Cash Added: $" + amount + ", Total Cash: $" + totalCash);
    UpdateCashUI(); // Update the UI
}

private void UpdateCashUI()
{
    if (cashText != null)
    {
        cashText.text = "$" + totalCash; // Update the text
        Debug.Log("Cash UI Updated: " + cashText.text);
    }
}

public void ShowGameOverScreen()
    {
    if (gameOverUI != null)
    {
        gameOverUI.SetActive(true); // Display the Game Over UI
        Debug.Log("Game Over UI displayed.");
    }
    else
    {
        Debug.LogError("Game Over UI is not assigned in the Inspector.");
    }

    Time.timeScale = 0f; // Freeze the game
    }


}
