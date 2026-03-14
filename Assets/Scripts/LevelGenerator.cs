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

            // Przypisujemy postawioną skrzynkę do zmiennej
            GameObject newCrateObj = Instantiate(cratePrefab, cratePos, Quaternion.identity, transform);

            // Pobieramy jej skrypt i dodajemy do naszej nowej listy
            Crate newCrateScript = newCrateObj.GetComponent<Crate>();
            spawnedCrates.Add(newCrateScript);

            availableSpaces.RemoveAt(randomIndex);
            spawned++;
        }

        // 4. UKRYWANIE SKARBÓW (Brama i Power-up)
        // Upewniamy się, że mamy chociaż 2 skrzynki na mapie
        if (spawnedCrates.Count >= 2)
        {
            // Losujemy pierwszą skrzynkę na Bramę
            int gateIndex = Random.Range(0, spawnedCrates.Count);
            spawnedCrates[gateIndex].hiddenItemPrefab = gatePrefab;

            // Usuwamy tę skrzynkę z listy losowania, żeby Power-up nie trafił w to samo miejsce!
            spawnedCrates.RemoveAt(gateIndex);

            // Losujemy drugą skrzynkę na Power-up
            int powerUpIndex = Random.Range(0, spawnedCrates.Count);
            spawnedCrates[powerUpIndex].hiddenItemPrefab = powerUpFirePrefab;
        }
    }
}