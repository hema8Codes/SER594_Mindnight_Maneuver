using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Reference to the entire Canvas
    public GameObject mainMenu;
    public GameObject helpPanel; // Assign in Inspector

    // Function to start the game
    public void StartGame()
    {
        // Hide the main menu when the game starts
        mainMenu.SetActive(false);

        // Optionally, if the game is in a different scene, you can load it:
        // SceneManager.LoadScene("GameSceneName");
        Debug.Log("Play Game");
        // Optionally disable the menu (or load a different scene)
        gameObject.SetActive(false); // 
        // Or if it's the same scene, you can just disable the menu and continue gameplay
        // Example: Enable game objects or start the gameplay logic
    }

    // Function to show the help menu
    
    // Function to quit the game
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the editor
        #else
            Application.Quit(); // Quit the game in a build
        #endif
    }

    public void ShowHelp()
    {
        // Toggle the help panel visibility
        if (helpPanel != null)
        {
            helpPanel.SetActive(!helpPanel.activeSelf);
        }
    }

     public void HideHelp()
    {
        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }
    }

}
