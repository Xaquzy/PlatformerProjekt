using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void LoadWinScene()
    {
        // Enable cursor before switching screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Load scene
        SceneManager.LoadScene("WinScene");
    }

    public void LoadMainLevel()
    {
        // No cursor while playing
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // Load scene
        SceneManager.LoadScene("Level1");
    }

    public void LoadGameOverScene()
    {
        // Enable cursor before switching screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Load scene
        SceneManager.LoadScene("GameOverScreen");
    }

    public void LoadMainMenu()
    {
        // Enable cursor before switching screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // Load scene
        SceneManager.LoadScene("StartScreen");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
