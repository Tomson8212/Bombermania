using UnityEngine;

public class PowerUp : MonoBehaviour
{
    
    public enum PowerUpType
    {
        FireRange,
        ExtraBomb,
        SpeedMove // na przyszłość
    }

    [Header("PowerUp Settings")]
    [SerializeField] private PowerUpType type; // Wybór typu w Inspektorze

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();

            if (stats != null)
            {
                // Sprawdzamy, jaki typ power-upa właśnie podnieśliśmy
                switch (type)
                {
                    case PowerUpType.FireRange:
                        stats.IncreaseFireRadius();
                        break;

                    case PowerUpType.ExtraBomb:
                        stats.IncreaseMaxBombs();
                        break;
                }

                Destroy(gameObject);
            }
        }
    }
}