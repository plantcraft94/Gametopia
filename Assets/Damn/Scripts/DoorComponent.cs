using UnityEngine;

public class DoorComponent : MonoBehaviour, IResettable
{
    Vector2Int cellPos;
    Vector2Int startCell;
    GridManager grid;

    bool isOpen = false;

    void Awake()
    {
        grid = FindFirstObjectByType<GridManager>();

        cellPos = grid.WorldToCell(transform.position);
        startCell = cellPos;

        transform.position = grid.CellToWorld(cellPos);
    }

    public bool TryOpen(PlayerController player)
    {
        if (isOpen)
            return true;

        PlayerResource resource = player.GetComponent<PlayerResource>();

        if (resource.KeyAmount > 0)
        {
            resource.UseKey();
            Open();
            return true;
        }

        return false; // Block player
    }

    void Open()
    {
        isOpen = true;
        gameObject.SetActive(false);
    }

    public void ResetObject()
    {
        isOpen = false;
        gameObject.SetActive(true);

        if (grid != null)
            transform.position = grid.CellToWorld(startCell);
    }
}