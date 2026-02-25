using System.Collections;
using UnityEngine;

public class MoveCommand : MonoBehaviour, IInstruction
{
    public IEnumerator RunInstruction(ExecutionContext context)
    {
        var player = context.player;

        if (!player.TryGetForwardCell(out var next))
        {
            Debug.Log("Blocked");
            yield break;
        }

        player.CommitMove(next);
        yield return player.AnimateMove(next);

        if (player.IsGoalCell(next))
            Debug.Log("GOAL!");
    }
}