using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    
    [SerializeField] private float fuseTime = 3f;
    [SerializeField] private int explosionRadius = 2;
    [SerializeField] private GameObject explosionPrefab;

    private void Start()
    {
        
        Invoke(nameof(Explode), fuseTime);
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

            Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
        }
    }
}