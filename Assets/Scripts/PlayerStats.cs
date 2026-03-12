using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    public int fireRadius = 1;

    
    public void IncreaseFireRadius()
    {
        fireRadius++;
        Debug.Log("Zebrano Power-up! Nowy zasięg ognia: " + fireRadius);
    }
}