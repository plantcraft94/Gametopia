using System;
using System.Collections;
using UnityEngine;

public class MoveCommand : CommandBase, IInstruction
{
    public IEnumerator Execute(
        ExecutionContext context,
        Action<int> jumpTo,
        int currentIP)
    {
        var player = context.player;

        if (player.TryGetForwardCell(out Vector2Int nextCell))
        {
            player.CommitMove(nextCell);
            yield return player.AnimateMove(nextCell);
        }

        // default next
        jumpTo(currentIP + 1);
    }
}