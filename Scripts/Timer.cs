using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Counts down from a set time (default 5 minutes),
/// updates a TextMeshPro UI element, and
/// determines the winner based on player kills when time runs out.
/// </summary>
public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeMinutes = 5f;      // Duration in minutes
    private float remainingTime;

    [Header("UI")]
    public TMP_Text timerText;           // Assign in inspector

    [Header("Scene Names")]
    public string player1WinScene = "Player1Win";
    public string player2WinScene = "Player2Win";
    public string drawScene = "DrawScene"; 

    private bool timerEnded = false;

    private void Start()
    {
        remainingTime = timeMinutes * 60f;
        UpdateTimerUI();
    }

    private void Update()
    {
        if (timerEnded) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            timerEnded = true;
            DetermineWinner();
        }

        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void DetermineWinner()
    {
        int p1Kills = GameManager.Instance.GetPlayer1Kills();
        int p2Kills = GameManager.Instance.GetPlayer2Kills();

        if (p1Kills > p2Kills)
            SceneManager.LoadScene(player1WinScene);
        else if (p2Kills > p1Kills)
            SceneManager.LoadScene(player2WinScene);
        else
            SceneManager.LoadScene(drawScene); 
    }
}
