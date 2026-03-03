using UnityEngine;
using System.Collections;

public class MimicObject : MonoBehaviour, IMimicable, IResettable
{
    public Vector2Int cellPos;

    PlayerController boundPlayer;
    GridManager grid;

    bool isBound = false;
    Vector2Int startCell;

    void Awake()
    {
        grid = FindFirstObjectByType<GridManager>();

        cellPos = grid.WorldToCell(transform.position);
        startCell = cellPos;

        transform.position = grid.CellToWorld(cellPos);
    }

    public void Initialize(GridManager g)
    {
        cellPos = grid.WorldToCell(transform.position);
    }

    public void Bind(PlayerController player)
    {
        if (isBound) return;

        boundPlayer = player;
        boundPlayer.OnMoved += HandlePlayerMoved;
        isBound = true;
    }

    public void Unbind()
    {
        if (!isBound) return;

        if (boundPlayer != null)
            boundPlayer.OnMoved -= HandlePlayerMoved;

        boundPlayer = null;
        isBound = false;
    }

    void HandlePlayerMoved(Vector2Int oldCell, Vector2Int newCell)
    {
        Vector2Int delta = newCell - oldCell;
        Vector2Int target = cellPos + delta;

        if (!grid.CanEnter(target))
            return;

        GameObject other = FindObjectAtCell(target);
        if (other != null && other != gameObject)
            return;

        StartCoroutine(AnimateMove(target));
    }

    IEnumerator AnimateMove(Vector2Int targetCell)
    {
        Vector3 start = transform.position;
        Vector3 end = grid.CellToWorld(targetCell);

        float moveTime = 0.25f;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        transform.position = end;
        cellPos = targetCell;

        if (grid.IsHole(cellPos))
        {
            Destroy(gameObject);
        }
    }

    GameObject FindObjectAtCell(Vector2Int cell)
    {
        Vector3 worldPos = grid.CellToWorld(cell);
        Collider2D col = Physics2D.OverlapPoint(worldPos);

        if (col != null)
            return col.gameObject;

        return null;
    }

    public void ResetObject()
    {
        Unbind();

        StopAllCoroutines();

        cellPos = startCell;

        if (grid != null)
            transform.position = grid.CellToWorld(startCell);
    }
}