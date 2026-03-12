using UnityEngine;
using UnityEngine.InputSystem;

public class BombSpawner : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private GameObject bombPrefab;

    private PlayerControls controls;

    private PlayerStats playerStats;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.PlaceBomb.performed += context => SpawnBomb();

        playerStats = GetComponent<PlayerStats>();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void SpawnBomb()
    {
        if (bombPrefab == null) return;

        Vector2 playerPosition = transform.position;

        // Snapping do siatki
        float snappedX = Mathf.Floor(playerPosition.x) + 0.5f;
        float snappedY = Mathf.Floor(playerPosition.y) + 0.5f;
        Vector2 snapPosition = new Vector2(snappedX, snappedY);

        GameObject spawnedBomb = Instantiate(bombPrefab, snapPosition, Quaternion.identity);

        
        if (playerStats != null)
        {
            Bomb bombScript = spawnedBomb.GetComponent<Bomb>();
            if (bombScript != null)
            {
                bombScript.SetRadius(playerStats.fireRadius);
            }
        }
    }
}