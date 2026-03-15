using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    private Vector2 currentDirection;
    private Rigidbody2D rb;

    [Header("Sensors")]
    public LayerMask obstacleLayer;
    public float sensorLength = 0.55f;

    [Header("Combat (Strefa Śmierci)")]
    public LayerMask playerLayer; // Radar będzie szukał tylko tej warstwy
    public float killRadius = 0.3f; // Rozmiar śmiertelnego jądra (możesz dopasować!)

    private void Start()
    {
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterEnemy();
        }

        rb = GetComponent<Rigidbody2D>();
        ChooseRandomDirection();
    }

    private void FixedUpdate()
    {
        // 1. RUCH
        rb.linearVelocity = currentDirection * speed;

        // 2. SZUKANIE ŚCIAN
        if (IsDirectionBlocked(currentDirection, sensorLength))
        {
            ChooseRandomDirection();
        }

        // 3. NOWOŚĆ: SZUKANIE GRACZA (Matematyczny Hitbox)
        // Rysujemy niewidzialne kółko na środku potwora. Jeśli dotknie warstwy Gracza...
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, killRadius, playerLayer);

        if (playerCollider != null)
        {
            PlayerMovement player = playerCollider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.Die();
            }
        }
    }

    private void ChooseRandomDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 temp = directions[i];
            int randomIndex = Random.Range(i, directions.Length);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }

        foreach (Vector2 dir in directions)
        {
            if (!IsDirectionBlocked(dir, 0.7f))
            {
                currentDirection = dir;
                return;
            }
        }
    }

    private bool IsDirectionBlocked(Vector2 dir, float distance)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, distance, obstacleLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject != gameObject)
            {
                return true;
            }
        }
        return false;
    }

    public void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemyDefeated(transform.position);
        }

        Destroy(gameObject);
    }

}