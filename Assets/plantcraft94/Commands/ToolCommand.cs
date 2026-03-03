using System;
using System.Collections;
using UnityEngine;

public class ToolCommand : CommandBase, IInstruction
{
    public ToolType toolType;
    public int range = 5;

    static IMimicable activeMimic;

    public IEnumerator Execute(
        ExecutionContext context,
        Action<int> jumpTo,
        int currentIP)
    {
        switch (toolType)
        {
            case ToolType.HookPlayer:
                yield return ExecuteHook(context);
                break;

            case ToolType.Pull:
                yield return ExecutePull(context);
                break;

            case ToolType.Mimic:
                yield return ExecuteMimic(context);
                break;
        }
    }

    IEnumerator ExecuteHook(ExecutionContext context)
    {
        PlayerController player = context.player;
        GridManager grid = context.grid;
        Vector2Int dir = FacingToVector(player.facing);

        for (int i = 1; i <= range; i++)
        {
            Vector2Int checkCell = player.cellPos + dir * i;

            GameObject hit = FindObjectAtCell(checkCell, grid);

            if (hit != null)
            {
                var hookable = hit.GetComponent<IHookable>();
                if (hookable != null)
                {
                    yield return hookable.OnHook(player, grid);
                    yield break;
                }
            }

            if (!grid.CanEnter(checkCell))
                yield break;
        }
    }

    IEnumerator ExecutePull(ExecutionContext context)
    {
        PlayerController player = context.player;
        GridManager grid = context.grid;
        Vector2Int dir = FacingToVector(player.facing);

        for (int i = 1; i <= range; i++)
        {
            Vector2Int checkCell = player.cellPos + dir * i;

            GameObject hit = FindObjectAtCell(checkCell, grid);

            if (hit != null)
            {
                var pullable = hit.GetComponent<IPullable>();
                if (pullable != null)
                {
                    yield return pullable.OnPull(player, grid);
                    yield break;
                }
            }

            if (!grid.CanEnter(checkCell))
                yield break;
        }
    }

    IEnumerator ExecuteMimic(ExecutionContext context)
    {
        PlayerController player = context.player;
        GridManager grid = context.grid;
        Vector2Int dir = FacingToVector(player.facing);

        if (activeMimic != null)
        {
            activeMimic.Unbind();
            activeMimic = null;
            yield break;
        }

        for (int i = 1; i <= range; i++)
        {
            Vector2Int checkCell = player.cellPos + dir * i;

            GameObject hit = FindObjectAtCell(checkCell, grid);

            if (hit != null)
            {
                var mimicable = hit.GetComponent<IMimicable>();
                if (mimicable != null)
                {
                    mimicable.Bind(player);
                    activeMimic = mimicable;
                    yield break;
                }
            }

            if (!grid.CanEnter(checkCell))
                yield break;
        }
    }

    Vector2Int FacingToVector(Facing f)
    {
        switch (f)
        {
            case Facing.Up: return Vector2Int.up;
            case Facing.Right: return Vector2Int.right;
            case Facing.Down: return Vector2Int.down;
            case Facing.Left: return Vector2Int.left;
        }
        return Vector2Int.up;
    }

    GameObject FindObjectAtCell(Vector2Int cell, GridManager grid)
    {
        Vector3 worldPos = grid.CellToWorld(cell);
        Collider2D col = Physics2D.OverlapPoint(worldPos);

        if (col != null)
            return col.gameObject;

        return null;
    }

    public static void ResetActiveMimic()
    {
        if (activeMimic != null)
        {
            activeMimic.Unbind();
            activeMimic = null;
        }
    }
}