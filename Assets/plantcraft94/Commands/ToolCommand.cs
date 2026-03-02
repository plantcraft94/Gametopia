using System;
using System.Collections;
using UnityEngine;

public class ToolCommand : CommandBase, IInstruction
{
    public ToolType toolType;
    public int range = 5;
    static MimicObject activeMimic;

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

            GameObject hitObject = FindObjectAtCell(checkCell, grid);

            if (hitObject != null && hitObject.CompareTag("HookAble"))
            {
                Vector2Int targetCell = checkCell - dir;

                if (grid.IsWalkable(targetCell))
                {
                    player.CommitMove(targetCell);
                    yield return player.AnimateMove(targetCell);
                }

                yield break;
            }

            if (!grid.IsWalkable(checkCell))
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
    IEnumerator ExecutePull(ExecutionContext context)
    {
        PlayerController player = context.player;
        GridManager grid = context.grid;

        Vector2Int dir = FacingToVector(player.facing);

        for (int i = 1; i <= range; i++)
        {
            Vector2Int checkCell = player.cellPos + dir * i;

            GameObject hitObject = FindObjectAtCell(checkCell, grid);

            if (hitObject == null)
            {
                if (!grid.IsWalkable(checkCell))
                    yield break;

                continue;
            }

            if (hitObject.CompareTag("KeyItem"))
            {
                // chỉnh bool ở đây nhá
                Destroy(hitObject);
                yield break;
            }

            // ========================
            // PullAble
            // ========================
            if (hitObject.CompareTag("PullAble"))
            {
                Vector2Int targetCell = checkCell - dir;

                if (!grid.IsWalkable(targetCell))
                    yield break;

                if (FindObjectAtCell(targetCell, grid) != null)
                    yield break;

                yield return MoveObjectToCell(hitObject, targetCell, grid);

                yield break;
            }

            if (!grid.IsWalkable(checkCell))
                yield break;
        }
    }
    IEnumerator MoveObjectToCell(GameObject obj, Vector2Int targetCell, GridManager grid)
    {
        Vector3 start = obj.transform.position;
        Vector3 end = grid.CellToWorld(targetCell);

        float moveTime = 0.25f;
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            obj.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        obj.transform.position = end;
    }
    IEnumerator ExecuteMimic(ExecutionContext context)
    {
        PlayerController player = context.player;
        GridManager grid = context.grid;

        Vector2Int dir = FacingToVector(player.facing);

        // Nếu đã có mimic → toggle off
        if (activeMimic != null)
        {
            activeMimic.Unbind();
            activeMimic = null;
            yield break;
        }

        for (int i = 1; i <= range; i++)
        {
            Vector2Int checkCell = player.cellPos + dir * i;

            GameObject hitObject = FindObjectAtCell(checkCell, grid);

            if (hitObject == null)
            {
                if (!grid.IsWalkable(checkCell))
                    yield break;

                continue;
            }

            if (hitObject.CompareTag("MimicAble"))
            {
                MimicObject mimic =
                    hitObject.GetComponent<MimicObject>();

                if (mimic == null)
                    yield break;

                mimic.Initialize(grid);
                mimic.Bind(player);

                activeMimic = mimic;

                yield break;
            }

            if (!grid.IsWalkable(checkCell))
                yield break;
        }
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