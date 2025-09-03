using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlComplete : MonoBehaviour
{ 
    public int MenuLevelIndex;

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuLevelIndex);
    }
}
