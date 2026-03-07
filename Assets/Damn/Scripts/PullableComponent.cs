using System;
using System.Collections;
using UnityEngine;

public class PullableComponent : MonoBehaviour, IPullable, IResettable
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
        if (gameObject.CompareTag("KeyItem"))
        {
            yield return PullToPlayer(player, grid);
            yield break;
        }

        Vector2Int dir = player.GetFacingVector();
        Vector2Int cell = grid.WorldToCell(transform.position);
        Vector2Int target = cell - dir;

        if (!grid.CanEnter(target))
            yield break;

        if (FindObjectAtCell(target, grid) != null)
            yield break;

        yield return MoveTo(target, grid);
    }

    IEnumerator MoveTo(Vector2Int target, GridManager grid)
    {
        Vector3 start = transform.position;
        Vector3 end = grid.CellToWorld(target);

        float t = 0;
        float moveTime = 0.25f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
        cellPos = target;

        if (grid.IsHole(target))
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator PullToPlayer(PlayerController player, GridManager grid)
    {
        Vector2Int playerCell = grid.WorldToCell(player.transform.position);
        Vector2Int currentCell = grid.WorldToCell(transform.position);

        while (currentCell != playerCell)
        {
            Vector2Int dir = new Vector2Int(
                Math.Sign(playerCell.x - currentCell.x),
                Math.Sign(playerCell.y - currentCell.y)
            );

            Vector2Int next = currentCell + dir;

            if (!grid.CanEnter(next) && next != playerCell)
                yield break;

            if (next != playerCell && FindObjectAtCell(next, grid) != null)
                yield break;

            yield return MoveTo(next, grid);

            if (grid.IsHole(next))
                yield break;

            currentCell = next;
        }
    }

    GameObject FindObjectAtCell(Vector2Int cell, GridManager grid)
    {
        Vector3 worldPos = grid.CellToWorld(cell);
        Collider2D col = Physics2D.OverlapPoint(worldPos);

        if (col != null)
            return col.gameObject;

        return null;
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