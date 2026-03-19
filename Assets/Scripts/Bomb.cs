using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private float fuseTime = 3f;
    [SerializeField] private GameObject explosionPrefab;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask obstacleLayer;

    private int explosionRadius = 1;

    // Referencje do ignorowania kolizji
    private Collider2D bombCollider;
    private Collider2D playerCollider;
    private BombSpawner mySpawner;

    // Flaga zabezpieczająca przed nieskończoną pętlą wybuchów
    private bool isExploding = false;
    
    private bool isDetonatorControlled = false;
    public void InitializeBomb(BombSpawner spawner, int radius, bool detonatorActive)
    {
        mySpawner = spawner;
        explosionRadius = radius;
        isDetonatorControlled = detonatorActive;

        // Jeśli gracz NIE ma detonatora, włączamy stary zapalnik czasowy
        if (!isDetonatorControlled)
        {
            Invoke(nameof(Explode), fuseTime);
        }
    }

    private void Start()
    {
        bombCollider = GetComponent<Collider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();

            if (bombCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(bombCollider, playerCollider, true);
            }
        }
    }

    private void Update()
    {
        if (playerCollider != null && bombCollider != null)
        {
            if (!bombCollider.bounds.Intersects(playerCollider.bounds))
            {
                Physics2D.IgnoreCollision(bombCollider, playerCollider, false);
                playerCollider = null;
            }
        }
    }

    // Funkcja wywoływana, gdy innna bomba "dotknie" nas swoim ogniem
    public void ForceExplode()
    {
        if (!isExploding)
        {
            CancelInvoke(nameof(Explode)); // Anulujemy standardowe odliczanie
            Explode(); // Odpalamy bombę natychmiast
        }
    }

    private void Explode()
    {
        // Jeśli bomba już zaczęła wybuchać w tej klatce, przerywamy
        if (isExploding) return;

        isExploding = true; // Blokujemy możliwość ponownego odpalenia

        if (explosionPrefab != null)
        {
            // Ogień na środku (w miejscu samej bomby)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Ogień rozchodzący się na boki
            SpawnExplosionInDirection(Vector2.up);
            SpawnExplosionInDirection(Vector2.down);
            SpawnExplosionInDirection(Vector2.left);
            SpawnExplosionInDirection(Vector2.right);
        }

        if (mySpawner != null)
        {
            mySpawner.OnBombExploded();
        }

        Destroy(gameObject);
    }

    private void SpawnExplosionInDirection(Vector2 direction)
    {
        for (int i = 1; i <= explosionRadius; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + (direction * i);

            // Skanujemy kratkę przed nami
            Collider2D hit = Physics2D.OverlapBox(spawnPosition, new Vector2(0.5f, 0.5f), 0f, obstacleLayer);

            if (hit != null)
            {
                // 1. Sprawdzamy, czy uderzyliśmy w skrzynkę
                Crate crate = hit.GetComponent<Crate>();
                if (crate != null)
                {
                    crate.DestroyCrate();
                    Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
                }

                // 2. Sprawdzamy, czy na naszej drodze stoi INNA BOMBA!
                Bomb otherBomb = hit.GetComponent<Bomb>();
                if (otherBomb != null)
                {
                    // Odpalamy sąsiednią bombę natychmiast!
                    otherBomb.ForceExplode();
                }

                // Przerywamy pętlę - wybuch nie idzie dalej za przeszkodę (ścianę/skrzynkę/bombę)
                break;
            }

            // Jeśli droga jest wolna, stawiamy zwykły płomień
            Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void SetRadius(int newRadius)
    {
        explosionRadius = newRadius;
    }
}