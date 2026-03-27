using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Singleton dla interfejsu
    public static UIManager Instance { get; private set; }

    [Header("Top Panel")]
    public TMP_Text levelText;
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public Image keyIcon;
    public Image powerUpLevelIcon; // Ikonka znaku zapytania
    public Sprite keyColorSprite;  // Kolorowy klucz (przeciągniesz w Inspektorze)
    public Sprite powerUpColorSprite; // Kolorowy ptaszek (przeciągniesz w Inspektorze)

    [Header("Bottom Panel - Stats")]
    public TMP_Text fireRadiusText;
    public TMP_Text maxBombsText;

    [Header("Bottom Panel - Inventory")]
    public Image[] inventoryIcons; // Przeciągniesz tu obiekty Icon_Item z pustych slotów
    public GameObject[] inventoryCircles; // Przeciągniesz tu obiekty Circle_BG (kółka z cyfrą)
    private int currentInventoryIndex = 0; // Śledzi, który slot jest następny do zapełnienia

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // --- FUNKCJE DLA TOP PANELU ---
    public void UpdateLevel(int level) { levelText.text = "LEVEL " + level.ToString(); }
    public void UpdateScore(int score) { scoreText.text = "SCORE: " + score.ToString("D6"); }
    public void UpdateLives(int lives) { livesText.text = "x " + lives.ToString(); }

    public void ActivateKey() { keyIcon.sprite = keyColorSprite; }
    public void ActivateLevelPowerUp() { powerUpLevelIcon.sprite = powerUpColorSprite; }

    // --- FUNKCJE DLA BOTTOM PANELU ---
    public void UpdateStats(int bombs, int fire)
    {
        maxBombsText.text = bombs.ToString();
        fireRadiusText.text = fire.ToString();
    }

    public void AddToInventory(Sprite powerUpSprite)
    {
        // Sprawdzamy, czy mamy jeszcze miejsce w ekwipunku
        if (currentInventoryIndex < inventoryIcons.Length)
        {
            // Włączamy obrazek i podmieniamy grafikę
            inventoryIcons[currentInventoryIndex].gameObject.SetActive(true);
            inventoryIcons[currentInventoryIndex].sprite = powerUpSprite;

            // Włączamy kółeczko z cyfrą na dole slota
            inventoryCircles[currentInventoryIndex].gameObject.SetActive(true);

            currentInventoryIndex++;
        }
    }
}