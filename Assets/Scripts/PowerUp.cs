using UnityEngine;

public class PowerUp : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();

            if (stats != null)
            {
                
                stats.IncreaseFireRadius();
                Destroy(gameObject);

            }
        }
    }
}