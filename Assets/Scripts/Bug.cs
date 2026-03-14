using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{
    public Transform lightOrb;
    public float moveSpeed = 5f;
    public float moveInterval = 0.3f;

    private Vector2Int gridPos;
    private bool isMoving;

    void Start()
    {
        // Snap to nearest grid cell on spawn
        gridPos = GridManager.Instance.WorldToGrid(transform.position);
        transform.position = GridManager.Instance.GridToWorld(gridPos.x, gridPos.y);

        BoardTile startTile = GridManager.Instance.GetTileAt(gridPos.x, gridPos.y);
        if (startTile != null)
        {
            startTile.SetOccupant(gameObject);
        }

        StartCoroutine(MoveLoop());
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);

            if (!isMoving)
            {
                TryMove();
            }
        }
    }

    void TryMove()
    {
        Vector2Int targetGridPos = GridManager.Instance.WorldToGrid(lightOrb.position);
        Vector2Int diff = targetGridPos - gridPos;

        // Stop if already one tile away or less from the light
        if (Mathf.Abs(diff.x) + Mathf.Abs(diff.y) <= 1)
            return;

        // Build list of candidate directions (up, down, left, right)
        List<Vector2Int> directions = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        // Sort directions by which one brings us closest to the light orb
        directions.Sort((a, b) =>
        {
            float distA = Vector2Int.Distance(gridPos + a, targetGridPos);
            float distB = Vector2Int.Distance(gridPos + b, targetGridPos);
            return distA.CompareTo(distB);
        });

        foreach (Vector2Int dir in directions)
        {
            Vector2Int nextPos = gridPos + dir;

            if (!GridManager.Instance.IsValidTile(nextPos.x, nextPos.y))
                continue;

            BoardTile nextTile = GridManager.Instance.GetTileAt(nextPos.x, nextPos.y);
            if (nextTile == null || !nextTile.isWalkable || nextTile.IsOccupied)
                continue;

            // Move to this tile
            BoardTile currentTile = GridManager.Instance.GetTileAt(gridPos.x, gridPos.y);
            if (currentTile != null)
            {
                currentTile.ClearOccupant();
            }

            gridPos = nextPos;
            nextTile.SetOccupant(gameObject);

            StartCoroutine(SmoothMove(GridManager.Instance.GridToWorld(nextPos.x, nextPos.y)));
            break;
        }
    }

    IEnumerator SmoothMove(Vector3 targetWorldPos)
    {
        isMoving = true;
        Vector3 startPos = transform.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPos, targetWorldPos, t);
            yield return null;
        }

        transform.position = targetWorldPos;
        isMoving = false;

        BoardTile currentTile = GridManager.Instance.GetTileAt(gridPos.x, gridPos.y);
        if (currentTile != null && currentTile.isHole)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        BoardTile tile = GridManager.Instance?.GetTileAt(gridPos.x, gridPos.y);
        if (tile != null)
        {
            tile.ClearOccupant();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bug"))
        {
            // Find a free adjacent tile for the new bug
            Vector2Int[] offsets = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int spawnPos = gridPos + offset;
                if (!GridManager.Instance.IsValidTile(spawnPos.x, spawnPos.y))
                    continue;

                BoardTile tile = GridManager.Instance.GetTileAt(spawnPos.x, spawnPos.y);
                if (tile != null && tile.isWalkable && !tile.IsOccupied)
                {
                    Vector3 worldPos = GridManager.Instance.GridToWorld(spawnPos.x, spawnPos.y);
                    Instantiate(gameObject, worldPos, Quaternion.identity);
                    break;
                }
            }
        }
    }
}
