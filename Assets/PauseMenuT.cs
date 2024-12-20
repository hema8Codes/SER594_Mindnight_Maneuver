using UnityEngine;
using UnityEngine.SceneManagement;  // For loading the scene

public class PauseMenuT : MonoBehaviour
{
    public GameObject pauseMenu;  // Reference to the pause menu panel
    public GameObject mainMenu;   // Reference to the game UI (the main gameplay UI)
    private bool isPaused = false; // Boolean to track if the game is paused

    void Update()
    {
        // Toggle pause menu when the player presses the "Pause" button (e.g., the "Esc" key)
        if (Input.GetKeyDown(KeyCode.Escape)) // Or any other key/button to trigger pause
        {
            TogglePause();
        }
    }

    // Function to toggle the pause state (show/hide the pause menu)
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Function to pause the game
    public void PauseGame()
    {
        pauseMenu.SetActive(true); // Show the pause menu
        mainMenu.SetActive(false);   // Hide the game UI
        Time.timeScale = 0f;       // Stop the game time (pause the game)
        isPaused = true;
    }

    // Function to resume the game
    public void ResumeGame()
    {
        pauseMenu.SetActive(false); // Hide the pause menu
        //mainMenu.SetActive(true);   // Show the game UI again
        Time.timeScale = 1f;        // Resume the game time
        isPaused = false;
    }

    // Function to go back to the main menu and restart the game
    public void BackToMainMenu()
    {
        // Reload the current scene to restart the game
        // This will reset all objects, variables, and gameplay logic to their initial state
        //  SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
        Time.timeScale = 1f;        // Resume the game time
        

        // If you have additional reset logic or state variables (e.g., score, health), reset them here
    }

    // Function to quit the game
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the editor
        #else
            Application.Quit(); // Quit the game in a built application
        #endif
    }
}