using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    // Czas w sekundach do wybuchu
    [SerializeField] private float fuseTime = 3f;
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
        }
        Destroy(gameObject);
    }
}