using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // N.Nkosi
    public int PlayLevelIndex;
    public int TutorialLevelIndex;

    /// <summary>
    /// Loads the main play level by index.
    /// </summary>
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(PlayLevelIndex);
    }

    /// <summary>
    /// Loads the tutorial level by index.
    /// </summary>
    public void Tutorial()
    {
        SceneManager.LoadScene(TutorialLevelIndex);
    }

    /// <summary>
    /// Loads the Narrative scene by name.
    /// Make sure the Narrative scene is added to Build Settings.
    /// </summary>
    public void LoadNarrative()
    {
        SceneManager.LoadScene("Narrative");
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    /*  
       Brackeys
       Start Menu in Unity
       Nov 29, 2017
       https://www.youtube.com/watch?v=zc8ac_qUXQY
   */
}
