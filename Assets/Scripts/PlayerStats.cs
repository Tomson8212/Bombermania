using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private int lives = 3;
    [SerializeField] private int fireRadius = 1;
    [SerializeField] private int maxBombs = 1;
    [SerializeField] private bool hasDetonator = false;

    [Header("PowerUp UI Graphics")]
    public Sprite detonatorSprite; // Przeciągniesz tu obrazek detonatora z zieloną ramką

    public int FireRadius => fireRadius;
    public int MaxBombs => maxBombs;
    public bool HasDetonator => hasDetonator;

    private void Start()
    {
        // Odświeżenie UI na start poziomu
        UIManager.Instance.UpdateLives(lives);
        UIManager.Instance.UpdateStats(maxBombs, fireRadius);
    }

    public void LoseLife()
    {
        lives--;
        UIManager.Instance.UpdateLives(lives);
        if (lives <= 0) { Debug.Log("Koniec Gry!"); /* Tu dodamy Game Over w przyszłości */ }
    }

    public void IncreaseFireRadius()
    {
        fireRadius++;
        UIManager.Instance.UpdateStats(maxBombs, fireRadius);

        UIManager.Instance.ActivateLevelPowerUp();
    }

    public void IncreaseMaxBombs()
    {
        maxBombs++;
        UIManager.Instance.UpdateStats(maxBombs, fireRadius);

        UIManager.Instance.ActivateLevelPowerUp();
    }

    public void EnableDetonator()
    {
        hasDetonator = true;
        // Wrzuć detonator do pierwszego wolnego slota w ekwipunku!
        UIManager.Instance.AddToInventory(detonatorSprite);

        UIManager.Instance.ActivateLevelPowerUp();
    }
}