using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public float directionChangeInterval = 3f; // NOWOŚĆ: Co ile sekund potwór sam z siebie skręca
    private float timer; // NOWOŚĆ: Licznik czasu
    private Vector2 currentDirection;
    private Rigidbody2D rb;

    [Header("Sensors")]
    public LayerMask obstacleLayer;
    public float sensorLength = 0.55f;

    [Header("Combat (Strefa Śmierci)")]
    public LayerMask playerLayer;
    public float killRadius = 0.3f;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterEnemy();
        }

        rb = GetComponent<Rigidbody2D>();
        timer = directionChangeInterval; // Inicjalizacja licznika
        ChooseRandomDirection();
    }

    private void Update()
    {
        // NOWOŚĆ: Odliczanie czasu do spontanicznej zmiany kierunku
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ChooseRandomDirection();
            timer = directionChangeInterval; // Reset licznika
        }
    }

    private void FixedUpdate()
    {
        // 1. RUCH (Zwróć uwagę: w Unity 6 używamy linearVelocity)
        rb.linearVelocity = currentDirection * speed;

        // 2. SZUKANIE PRZESZKÓD
        if (IsDirectionBlocked(currentDirection, sensorLength))
        {
            ChooseRandomDirection();
        }

        // 3. SZUKANIE GRACZA (Zabijanie)
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

        // Tasowanie tablicy
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

        // Jeśli potwór jest otoczony z każdej strony (np. przez bomby) - zatrzymuje się
        currentDirection = Vector2.zero;
    }

    private bool IsDirectionBlocked(Vector2 dir, float distance)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dir, distance, obstacleLayer);

        foreach (RaycastHit2D hit in hits)
        {
            // Ignorujemy samych siebie oraz ignorujemy "triggery" (np. zasięg wybuchu)
            if (hit.collider.gameObject != gameObject && !hit.collider.isTrigger)
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