using System.Collections;
using UnityEngine;

public class TurnCommand : MonoBehaviour, IInstruction
{
    public bool turnRight;

    public IEnumerator RunInstruction(ExecutionContext context)
    {
        if (turnRight)
            yield return context.player.TurnRight();
        else
            yield return context.player.TurnLeft();
    }
}