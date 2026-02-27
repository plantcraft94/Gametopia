using System;
using System.Collections;

public interface IInstruction
{
    IEnumerator Execute(
        ExecutionContext context,
        Action<int> jumpTo,
        int currentIP
    );
}