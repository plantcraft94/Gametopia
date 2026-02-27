using System;
using System.Collections;
using UnityEngine;

public enum RelativeDirection
{
    Ahead,
    Behind,
    Left,
    Right
}

public enum TileCheckType
{
    Wall,
    Ground,
    Goal
}

public class IfCommand : PairedCommand, IInstruction
{
    [Header("Condition")]
    public RelativeDirection direction;
    public TileCheckType checkType;

    public IEnumerator Execute(
        ExecutionContext context,
        Action<int> jumpTo,
        int currentIP)
    {
        bool result = Evaluate(context);

        if (result)
        {
            jumpTo(currentIP + 1);
        }
        else
        {
            if (pair != null)
            {
                var endIf = pair as EndIfCommand;
                jumpTo(endIf.runtimeIndex + 1);
            }
            else
            {
                jumpTo(currentIP + 1);
            }
        }

        yield break;
    }

    bool Evaluate(ExecutionContext context)
    {
        var player = context.player;

        // Lấy cell ở direction tương đối
        Vector2Int targetCell = player.GetRelativeCell(direction);

        switch (checkType)
        {
            case TileCheckType.Wall:
                return player.IsWall(targetCell);

            case TileCheckType.Ground:
                return player.IsGround(targetCell);

            case TileCheckType.Goal:
                return player.IsGoal(targetCell);
        }

        return false;
    }
}