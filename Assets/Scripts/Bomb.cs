using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private float fuseTime = 3f;
    [SerializeField] private GameObject explosionPrefab;

    [Header("Collision Settings")]
    [SerializeField] private LayerMask obstacleLayer;

    private int explosionRadius = 1;

    // NOWOŚĆ: Referencje do ignorowania kolizji
    private Collider2D bombCollider;
    private Collider2D playerCollider;

    private void Start()
    {
        Invoke(nameof(Explode), fuseTime);

        // Znajdujemy kolidery
        bombCollider = GetComponent<Collider2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerCollider = player.GetComponent<Collider2D>();

            // Mówimy fizyce Unity: "Zignoruj to, że te dwa obiekty się przenikają!"
            if (bombCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(bombCollider, playerCollider, true);
            }
        }
    }

    private void Update()
    {
        // Co klatkę sprawdzamy, czy gracz wciąż dotyka bomby
        if (playerCollider != null && bombCollider != null)
        {
            if (!bombCollider.bounds.Intersects(playerCollider.bounds))
            {
                // Gracz z niej zszedł! Włączamy twardą kolizję, żeby zablokować powrót.
                Physics2D.IgnoreCollision(bombCollider, playerCollider, false);
                playerCollider = null; // Przestajemy to sprawdzać, bo zrobiliśmy swoje
            }
        }
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            SpawnExplosionInDirection(Vector2.up);
            SpawnExplosionInDirection(Vector2.down);
            SpawnExplosionInDirection(Vector2.left);
            SpawnExplosionInDirection(Vector2.right);
        }

        Destroy(gameObject);
    }

    private void SpawnExplosionInDirection(Vector2 direction)
    {
        for (int i = 1; i <= explosionRadius; i++)
        {
            Vector2 spawnPosition = (Vector2)transform.position + (direction * i);
            Collider2D hit = Physics2D.OverlapBox(spawnPosition, new Vector2(0.5f, 0.5f), 0f, obstacleLayer);

            if (hit != null)
            {
                Crate crate = hit.GetComponent<Crate>();
                if (crate != null)
                {
                    crate.DestroyCrate();
                    Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
                }
                break;
            }

            Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void SetRadius(int newRadius)
    {
        explosionRadius = newRadius;
    }
}