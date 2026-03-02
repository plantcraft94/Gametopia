using System;
using System.Collections;
using UnityEngine;

public class ToolCommand : CommandBase, IInstruction
{
    public ToolType toolType;
    public int range = 5;

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
                // build sau
                break;

            case ToolType.Mimic:
                // build sau
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

            // Tìm object tại cell này
            GameObject hitObject = FindObjectAtCell(checkCell);

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

            // Nếu gặp tường trước khi gặp HookAble thì dừng
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

    GameObject FindObjectAtCell(Vector2Int cell)
    {
        Vector3 worldPos = 
            GameObject.FindObjectOfType<GridManager>()
            .CellToWorld(cell);

        Collider2D col = Physics2D.OverlapPoint(worldPos);

        if (col != null)
            return col.gameObject;

        return null;
    }
}