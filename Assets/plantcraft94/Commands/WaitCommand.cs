using System.Collections;
using UnityEngine;

public class WaitCommand : MonoBehaviour, IInstruction
{
    public IEnumerator RunInstruction(ExecutionContext context)
    {
        yield return context.player.Wait();
    }
}