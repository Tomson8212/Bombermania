using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzamy, czy to gracz dotknął klucza
        if (other.CompareTag("Player"))
        {
            // Informujemy GameManagera
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PickUpKey();
            }

            // Niszczymy obiekt klucza (znika z planszy)
            Destroy(gameObject);
        }
    }
}