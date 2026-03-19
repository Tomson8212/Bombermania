using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BombSpawner : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private LayerMask bombLayer;

    private PlayerControls controls;
    private PlayerStats playerStats;

    private int currentBombs = 0;

    // NOWOŚĆ: Lista przechowująca aktualnie leżące bomby
    private List<Bomb> activeBombs = new List<Bomb>();

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.PlaceBomb.performed += context => SpawnBomb();
        controls.Player.Detonate.performed += context => DetonateOldestBomb();

        playerStats = GetComponent<PlayerStats>();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void SpawnBomb()
    {
        if (bombPrefab == null || playerStats == null) return;
        if (currentBombs >= playerStats.MaxBombs) return;

        Vector2 playerPosition = transform.position;
        float snappedX = Mathf.Floor(playerPosition.x) + 0.5f;
        float snappedY = Mathf.Floor(playerPosition.y) + 0.5f;
        Vector2 snapPosition = new Vector2(snappedX, snappedY);

        Collider2D overlappingBomb = Physics2D.OverlapCircle(snapPosition, 0.1f, bombLayer);
        if (overlappingBomb != null) return;

        GameObject spawnedBomb = Instantiate(bombPrefab, snapPosition, Quaternion.identity);
        currentBombs++;

        Bomb bombScript = spawnedBomb.GetComponent<Bomb>();
        if (bombScript != null)
        {
            // Przekazujemy bombie wszystkie informacje za jednym zamachem
            bombScript.InitializeBomb(this, playerStats.FireRadius, playerStats.HasDetonator);

            // Dodajemy bombę na koniec naszej listy
            activeBombs.Add(bombScript);
        }
    }

    // Funkcja aktywowana przyciskiem Detonate
    private void DetonateOldestBomb()
    {
        if (!playerStats.HasDetonator) return;

        // Czyścimy listę z bomb, które mogły już wybuchnąć w reakcji łańcuchowej (są nullami)
        activeBombs.RemoveAll(b => b == null);

        if (activeBombs.Count > 0)
        {
            Bomb oldestBomb = activeBombs[0]; // Pobieramy najstarszą
            activeBombs.RemoveAt(0); // Usuwamy ją z kolejki

            if (oldestBomb != null)
            {
                oldestBomb.ForceExplode();
            }
        }
    }

    public void OnBombExploded()
    {
        currentBombs--;
        if (currentBombs < 0) currentBombs = 0;
    }
}