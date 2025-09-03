using System.Collections;
using UnityEngine;

public class PlayerPowerups : MonoBehaviour
{
    private IPlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<IPlayerController>();
        if (playerController == null)
            Debug.LogError("PlayerPowerups requires a component implementing IPlayerController");
    }

    /// <summary>
    /// Collects the double jump powerup and enables it for a limited duration.
    /// </summary>
    /// <param name="duration">How long double jump remains active (seconds)</param>
    public void CollectDoubleJump(float duration)
    {
        StartCoroutine(DoubleJumpRoutine(duration));
    }

    private IEnumerator DoubleJumpRoutine(float duration)
    {
        playerController.EnableDoubleJump(true);
        Debug.Log("Double jump enabled!");
        yield return new WaitForSeconds(duration);
        playerController.EnableDoubleJump(false);
        Debug.Log("Double jump expired.");
    }
}
