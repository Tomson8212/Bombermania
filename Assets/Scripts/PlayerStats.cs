using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int fireRadius = 1;
    [SerializeField] private int maxBombs = 1;

    // GETTERY
    public int FireRadius => fireRadius;
    public int MaxBombs => maxBombs;

    public void IncreaseFireRadius()
    {
        fireRadius++;
        Debug.Log("Zebrano Power-up! Nowy zasięg ognia: " + fireRadius);
    }

    public void IncreaseMaxBombs()
    {
        maxBombs++;
        Debug.Log("Zebrano Power-up! Maksymalna ilość bomb: " + maxBombs);
    }
}