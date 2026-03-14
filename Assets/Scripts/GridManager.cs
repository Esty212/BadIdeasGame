using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public GameObject tilePrefab;
    [SerializeField] private GameObject holePrefab;
    [SerializeField] private int holeX;
    [SerializeField] private int holeY;
    public int width = 8;
    public int height = 8;

    private BoardTile[,] grid;
    private Vector2 gridOffset;

    void Awake()
    {
        Instance = this;
        GenerateGrid();
    }

    [ContextMenu("Generate Board")]
    void GenerateGrid()
    {
        grid = new BoardTile[width, height];
        gridOffset = new Vector2(-(width - 1) / 2f, -(height - 1) / 2f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 spawnPos = new Vector2(x + gridOffset.x, y + gridOffset.y);

                GameObject newTile = Instantiate(tilePrefab, spawnPos, Quaternion.identity);
                newTile.transform.parent = transform;
                newTile.name = $"Tile_{x}_{y}";

                BoardTile tileScript = newTile.GetComponent<BoardTile>();
                tileScript.x = x;
                tileScript.y = y;
                grid[x, y] = tileScript;
            }
        }

        SpawnHole();
    }

    void SpawnHole()
    {
        if (holePrefab == null || !IsValidTile(holeX, holeY))
            return;

        Vector3 holeWorldPos = GridToWorld(holeX, holeY);
        GameObject hole = Instantiate(holePrefab, holeWorldPos, Quaternion.identity);
        hole.transform.parent = transform;
        hole.name = $"Hole_{holeX}_{holeY}";

        BoardTile holeTile = grid[holeX, holeY];
        holeTile.isHole = true;
    }

    public BoardTile GetTileAt(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return grid[x, y];
        }
        return null;
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x - gridOffset.x);
        int y = Mathf.RoundToInt(worldPos.y - gridOffset.y);
        return new Vector2Int(x, y);
    }

    public Vector3 GridToWorld(int x, int y)
    {
        return new Vector3(x + gridOffset.x, y + gridOffset.y, 0f);
    }

    public bool IsValidTile(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
