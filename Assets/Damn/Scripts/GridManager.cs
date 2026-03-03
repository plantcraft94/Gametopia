using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap wallTilemap;
    public Tilemap goalTilemap;

    public float cellSize = 1f;

    public Vector2Int WorldToCell(Vector3 worldPos)
    {
        Vector3Int cell = groundTilemap.WorldToCell(worldPos);
        return new Vector2Int(cell.x, cell.y);
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        Vector3Int c = new Vector3Int(cell.x, cell.y, 0);
        return groundTilemap.GetCellCenterWorld(c);
    }

    public bool IsWalkable(Vector2Int cell)
    {
        Vector3Int c = new Vector3Int(cell.x, cell.y, 0);

        if (wallTilemap.HasTile(c))
            return false;

        return groundTilemap.HasTile(c) || goalTilemap.HasTile(c);
    }

    public bool CanEnter(Vector2Int cell)
    {
        Vector3Int c = new Vector3Int(cell.x, cell.y, 0);

        if (wallTilemap.HasTile(c))
            return false;

        return true;
    }

    public bool IsGoal(Vector2Int cell)
    {
        Vector3Int c = new Vector3Int(cell.x, cell.y, 0);
        return goalTilemap.HasTile(c);
    }

    public bool IsHole(Vector2Int cell)
    {
        Vector3Int c = new Vector3Int(cell.x, cell.y, 0);

        bool hasGround = groundTilemap.HasTile(c);
        bool hasWall = wallTilemap.HasTile(c);
        bool hasGoal = goalTilemap.HasTile(c);

        return !hasGround && !hasWall && !hasGoal;
    }
}