using System;
using System.Collections;
using UnityEngine;

public class PullableComponent : MonoBehaviour, IPullable
{
    public Vector2Int cellPos;
    Vector2Int startCell;
    public GridManager grid;
    void Awake()
    {
        grid = FindFirstObjectByType<GridManager>();

        cellPos = grid.WorldToCell(transform.position);
        startCell = cellPos;

        transform.position = grid.CellToWorld(cellPos);
    }
    public IEnumerator OnPull(PlayerController player, GridManager grid)
    {
        if (transform.gameObject.CompareTag("KeyItem"))
        {
            yield return PullToPlayer(player, grid);
        }
        else
        {
            Vector2Int dir = player.GetFacingVector();
            Vector2Int cell = grid.WorldToCell(transform.position);
            Vector2Int target = cell - dir;

            if (!grid.IsWalkable(target))
                yield break;

            yield return MoveTo(target, grid);
        }
    }

    IEnumerator MoveTo(Vector2Int target, GridManager grid)
    {
        Vector3 start = transform.position;
        Vector3 end = grid.CellToWorld(target);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.25f;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
    }
    IEnumerator PullToPlayer(PlayerController player, GridManager grid)
    {
        Vector2Int playerCell = grid.WorldToCell(player.transform.position);
        Vector2Int currentCell = grid.WorldToCell(transform.position);

        // Move step-by-step toward player
        while (currentCell != playerCell)
        {
            Vector2Int dir = new Vector2Int(
                Math.Sign(playerCell.x - currentCell.x),
                Math.Sign(playerCell.y - currentCell.y)
            );

            Vector2Int next = currentCell + dir;

            // Optional safety
            if (!grid.IsWalkable(next) && next != playerCell)
                yield break;

            yield return MoveTo(next, grid);

            currentCell = next;
        }
    }
    public void ResetObject()
    {

        StopAllCoroutines();

        cellPos = startCell;

        if (grid != null)
            transform.position = grid.CellToWorld(startCell);

        gameObject.SetActive(true);
    }
}