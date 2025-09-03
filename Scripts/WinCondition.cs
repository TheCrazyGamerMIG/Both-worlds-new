using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour
{
    private bool gameOver = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!gameOver)
        {
            if (other.CompareTag("Player1"))
            {
                Debug.Log("Player 1 Wins!");
                gameOver = true;
                SceneManager.LoadScene(2);
            }
            else if (other.CompareTag("Player2"))
            {
                Debug.Log("Player 2 Wins!");
                gameOver = true;
                SceneManager.LoadScene(3);
            }
        }
    }
}
