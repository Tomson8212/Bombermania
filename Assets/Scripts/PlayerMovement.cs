using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private Vector2 movementInput;

    private Vector2 primaryDirection;
    private Vector2 secondaryDirection;
    private Vector2 lastRawInput;

    // (Gap Seeking)
    private bool wasBlockedX;
    private bool wasBlockedY;

    private PlayerControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
    }

    private void OnEnable() => controls.Enable();
    private void OnDisable() => controls.Disable();

    private void Update()
    {
        Vector2 rawInput = controls.Player.Move.ReadValue<Vector2>();

        Vector2 input = new Vector2(
            Mathf.Abs(rawInput.x) > 0.1f ? Mathf.Sign(rawInput.x) : 0,
            Mathf.Abs(rawInput.y) > 0.1f ? Mathf.Sign(rawInput.y) : 0
        );

        Vector2 dirX = new Vector2(input.x, 0);
        Vector2 dirY = new Vector2(0, input.y);

        // Sprawdzamy radarem oba wciśnięte kierunki
        bool isBlockedX = input.x != 0 && IsDirectionBlocked(dirX);
        bool isBlockedY = input.y != 0 && IsDirectionBlocked(dirY);

        // GAP SEEKING - Priorytet dla luki, która właśnie się pojawiła
        if (input.x != 0 && wasBlockedX && !isBlockedX)
        {
            primaryDirection = dirX;
            secondaryDirection = dirY;
        }
        
        else if (input.y != 0 && wasBlockedY && !isBlockedY)
        {
            primaryDirection = dirY;
            secondaryDirection = dirX;
        }
        // STANDARDOWY PRIORYTET (Ostatnio wciśnięty klawisz)
        else if (input != lastRawInput)
        {
            if (input.x != 0 && lastRawInput.x == 0)
            {
                primaryDirection = dirX;
                secondaryDirection = dirY;
            }
            else if (input.y != 0 && lastRawInput.y == 0)
            {
                primaryDirection = dirY;
                secondaryDirection = dirX;
            }
            else if (input == Vector2.zero)
            {
                primaryDirection = Vector2.zero;
                secondaryDirection = Vector2.zero;
            }
            else
            {
                if (input.x != 0) { primaryDirection = dirX; secondaryDirection = Vector2.zero; }
                else if (input.y != 0) { primaryDirection = dirY; secondaryDirection = Vector2.zero; }
            }
        }

        // Zapisujemy stany na następną klatkę
        lastRawInput = input;
        wasBlockedX = isBlockedX;
        wasBlockedY = isBlockedY;

        // LOGIKA RUCHU I ŚLIZGANIA
        movementInput = primaryDirection;

        if (primaryDirection != Vector2.zero && secondaryDirection != Vector2.zero)
        {
            if (IsDirectionBlocked(primaryDirection))
            {
                if (!IsDirectionBlocked(secondaryDirection))
                {
                    movementInput = secondaryDirection;
                }
                else
                {
                    movementInput = Vector2.zero; // Oba zablokowane
                }
            }
        }
    }

    private bool IsDirectionBlocked(Vector2 direction)
    {
        // CircleCastAll zwraca listę wszystkiego, co uderzył nasz radar
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 0.15f, direction, 0.4f, obstacleLayer);

        foreach (RaycastHit2D hit in hits)
        {
            // 1. Ignorujemy samego gracza (zabezpieczenie)
            if (hit.collider.gameObject == gameObject) continue;

            // 2. Jeśli uderzyliśmy w bombę, sprawdzamy, czy gracz wciąż na niej stoi
            if (hit.collider.CompareTag("Bomb"))
            {
                Collider2D playerCollider = GetComponent<Collider2D>();

                // Jeśli nasze granice wciąż się przecinają, ignorujemy tę bombę
                if (playerCollider != null && hit.collider.bounds.Intersects(playerCollider.bounds))
                {
                    continue;
                }
            }

            // Jeśli doszliśmy tutaj, trafiliśmy na prawdziwą, twardą przeszkodę (ścianę, skrzynkę lub zamkniętą bombę)
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movementInput * moveSpeed;
    }
    public void Die()
    {
        Debug.Log("Gracz nie żyje! GAME OVER");
        
        Destroy(gameObject);
    }
}