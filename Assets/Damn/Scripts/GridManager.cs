using UnityEngine;

public enum CellType
{
    Empty,
    Wall,
    Goal
}

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 6;
    public float cellSize = 1f;

    public CellType[,] grid;

    void Awake()
    {
        grid = new CellType[width, height];

        for (int x = 0; x < width; x++)
        {
            grid[x, 0] = CellType.Wall;
            grid[x, height - 1] = CellType.Wall;
        }

        for (int y = 0; y < height; y++)
        {
            grid[0, y] = CellType.Wall;
            grid[width - 1, y] = CellType.Wall;
        }

        grid[3, 2] = CellType.Wall;

        grid[6, 3] = CellType.Goal;
    }

    public Vector2Int WorldToCell(Vector3 pos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(pos.x / cellSize),
            Mathf.RoundToInt(pos.y / cellSize)
        );
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        return new Vector3(
            cell.x * cellSize,
            cell.y * cellSize,
            0
        );
    }

    public bool IsInside(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < width &&
               cell.y >= 0 && cell.y < height;
    }

    public bool IsWalkable(Vector2Int cell)
    {
        if (!IsInside(cell)) return false;
        return grid[cell.x, cell.y] != CellType.Wall;
    }

    public bool IsGoal(Vector2Int cell)
    {
        if (!IsInside(cell)) return false;
        return grid[cell.x, cell.y] == CellType.Goal;
    }
}