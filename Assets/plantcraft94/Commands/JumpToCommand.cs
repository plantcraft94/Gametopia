using System.Collections;
using UnityEngine;

public class JumpToCommand : PairedCommand, IInstruction
{
    public IEnumerator Execute(
        ExecutionContext context,
        System.Action<int> jumpTo,
        int currentIP)
    {
        jumpTo(currentIP + 1);
        yield break;
    }
}