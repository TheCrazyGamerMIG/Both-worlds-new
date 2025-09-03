using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PowerupPickup : MonoBehaviour
{
    [Header("Assign the Powerup ScriptableObject")]
    public Powerup powerup;

    private bool collected = false; // prevents multiple collections

    private void Awake()
    {
        // Ensure the collider is a trigger
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            Debug.LogWarning("PowerupPickup collider should be set as Trigger!");
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Prevent multiple triggers
        if (collected) return;

        // Check if the collider has a component implementing IPlayerController
        IPlayerController player = other.GetComponent<IPlayerController>();
        if (player != null && powerup != null)
        {
            collected = true; // mark as collected immediately
            powerup.Apply(player); // apply the powerup effect
            Destroy(gameObject);   // remove the powerup from the scene
            Debug.Log($"{player.GetPlayerName()} collected {powerup.powerupName}!");
        }
    }
}
