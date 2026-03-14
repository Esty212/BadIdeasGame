using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    private int width = 8;
    private int height = 8;

    private BoardTile[,] grid;

    void Start()
    {
        GenerateGrid();
        
    }

    [ContextMenu("Generate Board")]
    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 spawnPos = new Vector2(x, y);

                // Instantiate the tile at (x, y) world coordinates
                GameObject newTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                newTile.transform.parent = transform;
                newTile.name = $"Tile_{x}_{y}";

                // Store a reference in the array for easy access later
                BoardTile tileScript = newTile.GetComponent<BoardTile>();
                tileScript.x = x;
                tileScript.y = y;
            }
        }
    }

    // Access any tile instantly by its coordinates
    public BoardTile GetTileAt(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return grid[x, y];
        }
        return null;
    }


}
