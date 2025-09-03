using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Sprite[] playerSprites; // Assign 2+ sprites in Inspector

    private void OnEnable()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // Assign unique sprite
        SpriteRenderer sr = playerInput.GetComponent<SpriteRenderer>();
        int playerIndex = playerInput.playerIndex;

        if (playerIndex < playerSprites.Length)
        {
            sr.sprite = playerSprites[playerIndex];
        }

        // Optional: Set unique spawn positions
        playerInput.transform.position = new Vector3(playerIndex * 2, 0, 0);
    }
}
