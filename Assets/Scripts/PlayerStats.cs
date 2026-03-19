using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int fireRadius = 1;
    [SerializeField] private int maxBombs = 1;
    [SerializeField] private bool hasDetonator = false;

    // GETTERY
    public int FireRadius => fireRadius;
    public int MaxBombs => maxBombs;
    public bool HasDetonator => hasDetonator;

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
    public void EnableDetonator()
    {
        hasDetonator = true;
        Debug.Log("Zebrano Power-up! Detonator aktywny.");
    }
}