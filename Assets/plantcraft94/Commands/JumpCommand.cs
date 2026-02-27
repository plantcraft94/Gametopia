using System.Collections;
using UnityEngine;

public class JumpCommand : PairedCommand, IInstruction
{
    public IEnumerator Execute(
        ExecutionContext context,
        System.Action<int> jumpTo,
        int currentIP)
    {
        if (pair != null)
        {
            JumpToCommand target = pair as JumpToCommand;

            if (target != null)
            {
                jumpTo(target.runtimeIndex);
                yield break;
            }
        }

        jumpTo(currentIP + 1);
    }
}