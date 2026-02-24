using System.Collections;
using UnityEngine;

public class MoveCommand : MonoBehaviour, IInstruction
{
    public IEnumerator RunInstruction(ExecutionContext context)
    {
        yield return context.player.MoveForward();
    }
}