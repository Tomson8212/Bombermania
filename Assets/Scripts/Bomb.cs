using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    [SerializeField] private float fuseTime = 3f;
    [SerializeField] private int explosionRadius = 2;
    [SerializeField] private GameObject explosionPrefab;

    [Header("Collision Settings")]
    // Warstwy, które zatrzymują ogień (Ściany i Skrzynki)
    [SerializeField] private LayerMask obstacleLayer;

    private void Start()
    {
        Invoke(nameof(Explode), fuseTime);
    }

    private void Explode()
    {
        if (explosionPrefab != null)
        {
            // Środek wybuchu
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Wypuszczamy ogień w 4 kierunkach
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

            // Puste pole
            Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
        }
    }
}