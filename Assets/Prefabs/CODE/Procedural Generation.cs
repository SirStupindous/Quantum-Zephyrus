using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width = 1000;
    [SerializeField] int spawnerGap = 50;
    [SerializeField] int startPos = 10;
    [SerializeField] int endPos = 990;
    int height, minStoneHeight, maxStoneHeight;
    float heightValue, smoothness, seed;
    public GameObject[] background;
    [SerializeField] Tilemap dirtTilemap, grassTilemap, stoneTilemap, sandTilemap, snowTilemap;
    [SerializeField] Tile hillsMidTile, hillsTopTile, hillsBotTile, sandTopTile, sandMidTile, sandBotTile, snowTopTile, snowMidTile, snowBotTile, cityMidTile, cityTopTile, cityBotTile;

    public GameObject spawnPointPrefab; // Reference to the SpawnPoint prefab
    public GameObject endRoundPrefab;   // Reference to the EndRound prefab
    public GameObject enemySpawnerPrefab; // Reference to the Enemy Spawner prefab
    public GameObject bossEnemySpawnerPrefab; // Reference to the Boss Enemy Spawner prefab

    // Start is called before the first frame update
    void Start()
    {
        // Randomize the seed value
        seed = Random.Range(-100000, 100000);

        // Generate the terrain
        Generation();

        // Create SpawnPoint and EndRound GameObjects dynamically
        if (spawnPointPrefab != null && endRoundPrefab != null)
        {
            // Instantiate the SpawnPoint object at x = 10 with yOffset = 1
            InstantiateAtPosition(spawnPointPrefab, startPos, 1);

            // Instantiate the EndRound object at x = 99 with yOffset = 1
            InstantiateAtPosition(endRoundPrefab, endPos, 1);

            // Instantiate enemy spawners at multiples of 10 with yOffset = 1
            for (int i = 0; i < 9; i++)
            {
                int xPosition = (i + 1) * spawnerGap;
                InstantiateAtPosition(enemySpawnerPrefab, xPosition, 1);
            }

            // Instantiate the boss enemy spawner at x = 100 with yOffset = 2
            InstantiateAtPosition(bossEnemySpawnerPrefab, 100, 2);
        }
    }

    void InstantiateAtPosition(GameObject prefab, int x, int yOffset)
    {
        // Instantiate the object at the specified x position with yOffset
        GameObject obj = Instantiate(prefab);
        // float yPos = GetTerrainHeight(x) + yOffset;
        float yPos = 30 + yOffset;
        obj.transform.position = new Vector3(x, yPos, 0);
    }

    // Helper method to get the terrain height at a specific X position
    float GetTerrainHeight(float x)
    {
        int terrainX = Mathf.Clamp(Mathf.RoundToInt(x), 0, width - 1);
        Tilemap[] tilemaps = { stoneTilemap, dirtTilemap, grassTilemap, sandTilemap, snowTilemap };

        float maxHeight = float.MinValue;

        foreach (var tilemap in tilemaps)
        {
            Vector3Int cellPosition = new Vector3Int(terrainX, 0, 0);
            while (cellPosition.y < height)
            {
                if (tilemap.HasTile(cellPosition))
                {
                    Vector3 cellCenter = tilemap.GetCellCenterWorld(cellPosition);
                    maxHeight = Mathf.Max(maxHeight, cellCenter.y);
                }
                cellPosition.y++;
            }
        }

        return maxHeight;
    }

    // Update is called once per frame
    private void Update()
    {
        // Generates tiles based on the player's input
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            seed = Random.Range(-100000, 100000);
            Generation();
        }
        // Clears all tiles based on the player's input
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            for (int i = 0; i < background.Length; i++)
            {
                background[i].SetActive(false);
            }
            stoneTilemap.ClearAllTiles();
            dirtTilemap.ClearAllTiles();
            grassTilemap.ClearAllTiles();
            sandTilemap.ClearAllTiles();
            snowTilemap.ClearAllTiles();
        }
    }

    // Generates the certain tiles at different locations using Perlin noise
    void Generation()
    {
        // ========== Key to the theme ==========
        // theme = 1            ->          Hills
        // theme = 2            ->          Snow
        // theme = 3            ->          Sand
        // theme = 4            ->          City
        // ======================================
        int theme = Random.Range(1, 5);

        // Sets the values of the terrain based on the theme
        switch (theme)
        {
            case 1:     // Hills
                minStoneHeight = 6;
                maxStoneHeight = 10;
                heightValue = 35;
                smoothness = 25;
                break;
            case 2:     // Snow
                minStoneHeight = 2;
                maxStoneHeight = 15;
                heightValue = 45;
                smoothness = 15;
                break;
            case 3:     // Sand
                minStoneHeight = 4;
                maxStoneHeight = 8;
                heightValue = 25;
                smoothness = 35;
                break;
            case 4:     // City
                minStoneHeight = 7;
                maxStoneHeight = 8;
                heightValue = 30;
                smoothness = 30;
                break;
        }

        // Set the image of the background active to the corresponding theme
        if (background != null && background.Length >= 4)
        {
            background[theme - 1].SetActive(true);
        }

        for (int x = 0; x < width; x++)         //helps spawn tiles on the x axis
        {
            height = Mathf.RoundToInt(heightValue * Mathf.PerlinNoise(x / smoothness, seed));            //randomizes the height of the grass
            int minStoneSpawnDistance = height - minStoneHeight;                                         //sets the minimum distance between the grass and the stone
            int maxStoneSpawnDistance = height - maxStoneHeight;                                         //sets the maximum distance between the grass and the stone
            int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);    //randomizes the distance between the grass and the stone

            for (int y = 0; y < height; y++)    //helps spawn tiles on the y axis
            {
                //if the y value is greater than the total stone spawn distance, spawn stone
                if (y < totalStoneSpawnDistance)
                {
                    switch (theme)
                    {
                        case 1:
                            stoneTilemap.SetTile(new Vector3Int(x, y, 0), hillsBotTile);
                            break;
                        case 2:
                            stoneTilemap.SetTile(new Vector3Int(x, y, 0), snowBotTile);
                            break;
                        case 3:
                            stoneTilemap.SetTile(new Vector3Int(x, y, 0), sandBotTile);
                            break;
                        case 4:
                            stoneTilemap.SetTile(new Vector3Int(x, y, 0), cityBotTile);
                            break;
                    }
                }
                //if the y value is less than the total stone spawn distance, spawn dirt
                else
                {
                    switch (theme)
                    {
                        case 1:
                            dirtTilemap.SetTile(new Vector3Int(x, y, 0), hillsMidTile);
                            break;
                        case 2:
                            dirtTilemap.SetTile(new Vector3Int(x, y, 0), snowMidTile);
                            break;
                        case 3:
                            dirtTilemap.SetTile(new Vector3Int(x, y, 0), sandMidTile);
                            break;
                        case 4:
                            dirtTilemap.SetTile(new Vector3Int(x, y, 0), cityMidTile);
                            break;
                    }
                }
            }
            //if the height is less than the total stone spawn distance, spawn stone
            if (height == totalStoneSpawnDistance)
            {
                switch (theme)
                {
                    case 1:
                        stoneTilemap.SetTile(new Vector3Int(x, height, 0), hillsBotTile);
                        break;
                    case 2:
                        stoneTilemap.SetTile(new Vector3Int(x, height, 0), snowBotTile);
                        break;
                    case 3:
                        stoneTilemap.SetTile(new Vector3Int(x, height, 0), sandBotTile);
                        break;
                    case 4:
                        stoneTilemap.SetTile(new Vector3Int(x, height, 0), cityBotTile);
                        break;
                }
            }
            //if the height is greater than the total stone spawn distance, spawn the ground
            else
            {
                switch (theme)
                {
                    case 1:
                        grassTilemap.SetTile(new Vector3Int(x, height, 0), hillsTopTile);
                        break;
                    case 2:
                        grassTilemap.SetTile(new Vector3Int(x, height, 0), snowTopTile);
                        break;
                    case 3:
                        grassTilemap.SetTile(new Vector3Int(x, height, 0), sandTopTile);
                        break;
                    case 4:
                        grassTilemap.SetTile(new Vector3Int(x, height, 0), cityTopTile);
                        break;
                }
            }
        }
    }
}
