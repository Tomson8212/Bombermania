using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    // Jak długo ogień ma być widoczny na ekranie (w sekundach)
    [SerializeField] private float lifetime = 0.5f;

    private void Start()
    {
        // Funkcja Destroy z drugim parametrem niszczy obiekt dopiero po podanym czasie.
        Destroy(gameObject, lifetime);
    }
}