using UnityEngine;
using UnityEngine.UI;

public class QuitGameButton : MonoBehaviour
{
    // Reference to the Button component
    public Button quitButton;

    void Start()
    {
        // Ensure the button is assigned
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
        else
        {
            Debug.LogError("Quit button is not assigned!");
        }
    }

    void OnQuitButtonClicked()
    {
        // Check if we are in the editor
        #if UNITY_EDITOR
        // Stop playing in the editor
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Quit the application
        Application.Quit();
        #endif
    }
}

