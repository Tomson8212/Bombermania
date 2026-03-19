using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float lifetime = 0.5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzamy, czy to wróg
        if (other.CompareTag("Enemy"))
        {
            EnemyAI enemy = other.GetComponent<EnemyAI>();
            if (enemy != null) enemy.Die();
        }
        // Sprawdzamy, czy to gracz
        else if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null) player.Die();
        }
        // Sprawdzamy, czy ogień dotknął innej bomby!
        else if (other.CompareTag("Bomb"))
        {
            Bomb bomb = other.GetComponent<Bomb>();
            if (bomb != null)
            {
                bomb.ForceExplode(); // Przymusowa detonacja!
            }
        }
    }
}