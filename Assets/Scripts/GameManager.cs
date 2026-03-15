using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Wzorzec Singleton - pozwala innym skryptom łatwo "rozmawiać" z menedżerem
    public static GameManager Instance { get; private set; }

    [Header("Level Progress")]
    public int enemyCount = 0;

    // NOWOŚĆ: Czy gracz ma już klucz?
    [Header("Player State")]
    public bool hasKey = false;

    // Tu w przyszłości przypiszemy prefab Klucza
    public GameObject keyPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Funkcja wywoływana, gdy potwór się rodzi
    public void RegisterEnemy()
    {
        enemyCount++;
    }

    // Funkcja wywoływana, gdy potwór ginie
    public void EnemyDefeated(Vector3 deathPosition)
    {
        enemyCount--;
        Debug.Log("Zabito potwora! Pozostało: " + enemyCount);

        // Jeśli to był ostatni potwór...
        if (enemyCount <= 0)
        {
            SpawnKey(deathPosition);
        }
    }

    private void SpawnKey(Vector3 spawnPosition)
    {
        Debug.Log("Ostatni potwór pokonany! Pojawia się KLUCZ!");

        // Zaokrąglamy w dół do pełnej liczby (np. 2.8 -> 2.0) i dodajemy 0.5, 
        // żeby trafić idealnie w środek kafelka (wynik: 2.5)
        float snapX = Mathf.Floor(spawnPosition.x) + 0.5f;
        float snapY = Mathf.Floor(spawnPosition.y) + 0.5f;

        Vector3 centeredPosition = new Vector3(snapX, snapY, 0f);

        if (keyPrefab != null)
        {
            Instantiate(keyPrefab, centeredPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Brakuje prefabu klucza w GameManagerze!");
        }
    }

    public void PickUpKey()
    {
        hasKey = true;
        Debug.Log("Gracz podniósł klucz! Czas znaleźć bramę!");

        // Szukamy na scenie obiektu typu Gate (jeśli gracz już wysadził z niej skrzynkę)
        Gate levelGate = FindAnyObjectByType<Gate>();

        // Jeśli znaleźliśmy bramę (czyli leży już gdzieś na planszy), każemy jej się otworzyć
        if (levelGate != null)
        {
            levelGate.OpenGate();
        }
    }
}