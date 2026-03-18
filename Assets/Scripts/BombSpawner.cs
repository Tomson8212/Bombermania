using UnityEngine;
using UnityEngine.InputSystem;

public class BombSpawner : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private LayerMask bombLayer; // Warstwa bomby, do blokowania duplikatów

    private PlayerControls controls;
    private PlayerStats playerStats;

    private int currentBombs = 0; // Aktualna liczba bomb na planszy

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
        if (bombPrefab == null || playerStats == null) return;

        // 1. Sprawdzamy, czy gracz nie przekroczył limitu
        if (currentBombs >= playerStats.MaxBombs)
        {
            Debug.Log("Nie możesz postawić więcej bomb!");
            return;
        }

        Vector2 playerPosition = transform.position;
        float snappedX = Mathf.Floor(playerPosition.x) + 0.5f;
        float snappedY = Mathf.Floor(playerPosition.y) + 0.5f;
        Vector2 snapPosition = new Vector2(snappedX, snappedY);

        // 2. Sprawdzamy, czy na tej kratce nie leży już jakaś bomba
        Collider2D overlappingBomb = Physics2D.OverlapCircle(snapPosition, 0.1f, bombLayer);
        if (overlappingBomb != null)
        {
            return; // Przerywamy, bomba już tu jest!
        }

        // 3. Stawiamy bombę i zwiększamy licznik
        GameObject spawnedBomb = Instantiate(bombPrefab, snapPosition, Quaternion.identity);
        currentBombs++;

        Bomb bombScript = spawnedBomb.GetComponent<Bomb>();
        if (bombScript != null)
        {
            bombScript.SetRadius(playerStats.FireRadius);

            // 4. Mówimy bombie, kto ją postawił
            bombScript.SetSpawner(this);
        }
    }

    // Funkcja wywoływana przez bombę z ułamku sekundy przed jej usunięciem
    public void OnBombExploded()
    {
        currentBombs--;
        if (currentBombs < 0) currentBombs = 0; // Fail-safe
    }
}