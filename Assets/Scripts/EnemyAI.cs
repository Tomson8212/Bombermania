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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ChooseRandomDirection();
    }

    private void FixedUpdate()
    {
        // 1. Zawsze idziemy w aktualnym kierunku
        rb.linearVelocity = currentDirection * speed;

        // 2. Patrzymy przed siebie. Jeśli zbliżamy się do ściany, skręcamy!
        if (IsDirectionBlocked(currentDirection, sensorLength))
        {
            ChooseRandomDirection();
        }
    }

    private void ChooseRandomDirection()
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        // Tasowanie kierunków
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 temp = directions[i];
            int randomIndex = Random.Range(i, directions.Length);
            directions[i] = directions[randomIndex];
            directions[randomIndex] = temp;
        }

        // Sprawdzamy wylosowane kierunki nieco dłuższym laserem
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
            // IGNORE SELF: Ignorujemy zderzenie z samym sobą!
            if (hit.collider.gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }

    public void Die()
    {
        Debug.Log("Potwór został usmażony!");
        Destroy(gameObject);
    }
}