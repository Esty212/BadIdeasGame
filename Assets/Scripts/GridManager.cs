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
                Vector3 spawnPos = new Vector3(x, y, 0);

                // Instantiate the tile at (x, y) world coordinates
                GameObject newTile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity);
                newTile.transform.parent = transform;
                newTile.name = $"Tile_{x}_{y}";

                // Store a reference in the array for easy access later
                BoardTile tileScript = newTile.GetComponent<BoardTile>();
                tileScript.x = x;
                tileScript.y = y;
                grid[x, y] = tileScript;
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
