using System;
using System.Collections;
using UnityEngine;

public class EndIfCommand : PairedCommand, IInstruction
{
    public IEnumerator Execute(
        ExecutionContext context,
        Action<int> jumpTo,
        int currentIP)
    {
        // Just go to next instruction
        jumpTo(currentIP + 1);
        yield break;
    }
}