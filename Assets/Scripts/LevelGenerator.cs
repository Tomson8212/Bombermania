using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 15;
    public int height = 11;

    [Header("Static Environment (Tilemap)")]
    public Tilemap floorTilemap;
    public TileBase floorTile;
    public Tilemap solidWallTilemap;
    public TileBase solidWallTile;

    [Header("Dynamic Objects (Prefabs)")]
    public GameObject cratePrefab;

    [Header("Hidden Items")]
    public GameObject gatePrefab;
    public GameObject powerUpFirePrefab;

    [Header("Enemies")]
    public GameObject enemyPrefab;          // Prefab fioletowego potwora
    public int enemiesToSpawn = 3;          // Ilu wrogów ma być na planszy

    [Header("Level Setup")]
    public int cratesToSpawn = 20;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        // 1. CZYSZCZENIE MAPY
        floorTilemap.ClearAllTiles();
        solidWallTilemap.ClearAllTiles();

        List<Vector2> availableSpaces = new List<Vector2>();

        // 2. BUDOWA MAPY
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);
                Vector2 worldSpawnPos = new Vector2(x + 0.5f, y + 0.5f);

                floorTilemap.SetTile(cellPosition, floorTile);

                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    solidWallTilemap.SetTile(cellPosition, solidWallTile);
                }
                else if (x % 2 == 0 && y % 2 == 0)
                {
                    solidWallTilemap.SetTile(cellPosition, solidWallTile);
                }
                else
                {
                    // L-Shape w lewym GÓRNYM rogu (Baza Gracza)
                    if ((x == 1 && y == height - 2) ||
                        (x == 1 && y == height - 3) ||
                        (x == 2 && y == height - 2))
                    {
                        continue;
                    }

                    availableSpaces.Add(worldSpawnPos);
                }
            }
        }

        // 3. ROZSTAWIANIE SKRZYNEK
        int spawned = 0;
        List<Crate> spawnedCrates = new List<Crate>();

        while (spawned < cratesToSpawn && availableSpaces.Count > 0)
        {
            int randomIndex = Random.Range(0, availableSpaces.Count);
            Vector2 cratePos = availableSpaces[randomIndex];

            GameObject newCrateObj = Instantiate(cratePrefab, cratePos, Quaternion.identity, transform);
            Crate newCrateScript = newCrateObj.GetComponent<Crate>();
            spawnedCrates.Add(newCrateScript);

            availableSpaces.RemoveAt(randomIndex);
            spawned++;
        }

        // 4. UKRYWANIE SKARBÓW (Brama i Power-up)
        if (spawnedCrates.Count >= 2)
        {
            int gateIndex = Random.Range(0, spawnedCrates.Count);
            spawnedCrates[gateIndex].hiddenItemPrefab = gatePrefab;
            spawnedCrates.RemoveAt(gateIndex);

            int powerUpIndex = Random.Range(0, spawnedCrates.Count);
            spawnedCrates[powerUpIndex].hiddenItemPrefab = powerUpFirePrefab;
        }

        // 5. GENEROWANIE PRZECIWNIKÓW
        // Filtrujemy dostępne miejsca, żeby potwory nie pojawiły się za blisko startu gracza
        List<Vector2> safeEnemySpaces = new List<Vector2>();
        Vector2 playerStartPos = new Vector2(1.5f, height - 1.5f); // Przybliżona pozycja startowa

        foreach (Vector2 space in availableSpaces)
        {
            // Jeśli odległość płytki od gracza jest większa niż 3 kratki, uznajemy ją za bezpieczną
            if (Vector2.Distance(playerStartPos, space) > 3f)
            {
                safeEnemySpaces.Add(space);
            }
        }

        int spawnedEnemies = 0;
        while (spawnedEnemies < enemiesToSpawn && safeEnemySpaces.Count > 0)
        {
            int randomIndex = Random.Range(0, safeEnemySpaces.Count);
            Vector2 enemyPos = safeEnemySpaces[randomIndex];

            // Tworzymy potwora na wylosowanej pozycji
            Instantiate(enemyPrefab, enemyPos, Quaternion.identity, transform);

            safeEnemySpaces.RemoveAt(randomIndex);
            spawnedEnemies++;
        }
    }
}