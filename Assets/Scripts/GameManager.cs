using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Level Progress")]
    public int currentLevel = 1;
    public int score = 0;
    public int enemyCount = 0;

    [Header("Player State")]
    public bool hasKey = false;
    public GameObject keyPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        // Aktualizujemy UI na samym starcie gry
        UIManager.Instance.UpdateLevel(currentLevel);
        UIManager.Instance.UpdateScore(score);
    }

    public void RegisterEnemy() { enemyCount++; }

    public void EnemyDefeated(Vector3 deathPosition)
    {
        enemyCount--;

        // Dodajemy punkty i aktualizujemy ekran!
        score += 100; // Zmień tę wartość na taką, jaką chcesz
        UIManager.Instance.UpdateScore(score);

        Debug.Log("Zabito potwora! Pozostało: " + enemyCount);

        if (enemyCount <= 0) { SpawnKey(deathPosition); }
    }

    private void SpawnKey(Vector3 spawnPosition)
    {
        Debug.Log("Ostatni potwór pokonany! Pojawia się KLUCZ!");
        float snapX = Mathf.Floor(spawnPosition.x) + 0.5f;
        float snapY = Mathf.Floor(spawnPosition.y) + 0.5f;
        Vector3 centeredPosition = new Vector3(snapX, snapY, 0f);

        if (keyPrefab != null) { Instantiate(keyPrefab, centeredPosition, Quaternion.identity); }
    }

    public void PickUpKey()
    {
        hasKey = true;
        // Zmieniamy szarą ikonkę klucza na kolorową!
        UIManager.Instance.ActivateKey();

        Gate levelGate = FindAnyObjectByType<Gate>();
        if (levelGate != null) { levelGate.OpenGate(); }
    }
}