using System.Collections;
using UnityEngine;

public class WaitCommand : MonoBehaviour, IInstruction
{
    public float waitTime = 0.25f;

    public IEnumerator RunInstruction(ExecutionContext context)
    {
        yield return context.player.AnimateWait(waitTime);
    }
}