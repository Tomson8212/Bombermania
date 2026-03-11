using UnityEngine;
using UnityEngine.InputSystem;

public class BombSpawner : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private GameObject bombPrefab;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Player.PlaceBomb.performed += context => SpawnBomb();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void SpawnBomb()
    {
        if (bombPrefab == null) return;

        Vector2 playerPosition = transform.position;

        // Ucina ułamek (np. 1.8 -> 1.0) i dodaje 0.5, celując w sam środek kafelka (1.5)
        float snappedX = Mathf.Floor(playerPosition.x) + 0.5f;
        float snappedY = Mathf.Floor(playerPosition.y) + 0.5f;

        Vector2 snapPosition = new Vector2(snappedX, snappedY);

        Instantiate(bombPrefab, snapPosition, Quaternion.identity);
    }
}