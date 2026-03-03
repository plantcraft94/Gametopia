using System;
using System.Collections;
using UnityEngine;

public class HookableComponent : MonoBehaviour, IHookable
{
    public IEnumerator OnHook(PlayerController player, GridManager grid)
    {
        Vector2Int dir = player.GetFacingVector();
        Vector2Int cell = grid.WorldToCell(transform.position);
        Vector2Int target = cell - dir;

        if (!grid.IsWalkable(target))
            yield break;

        player.CommitMove(target);
        yield return player.AnimateMove(target);
    }
}