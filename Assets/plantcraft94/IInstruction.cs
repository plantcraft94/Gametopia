using System.Collections;

public interface IInstruction
{
    IEnumerator RunInstruction(ExecutionContext context);
}