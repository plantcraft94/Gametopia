using System;
using System.Collections;
using UnityEngine;

public class PullableComponent : MonoBehaviour, IPullable
{
    public IEnumerator OnPull(PlayerController player, GridManager grid)
    {
        Vector2Int dir = player.GetFacingVector();
        Vector2Int cell = grid.WorldToCell(transform.position);
        Vector2Int target = cell - dir;

        if (!grid.IsWalkable(target))
            yield break;

        yield return MoveTo(target, grid);
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
}