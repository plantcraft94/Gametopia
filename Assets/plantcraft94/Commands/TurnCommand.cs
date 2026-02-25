using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurnCommand : MonoBehaviour, IInstruction
{
    public bool turnRight;
    public void Choose(int index)
    {
        switch (index)
        {
            case 0:
               turnRight = false;
               break;
            case 1:
               turnRight = true;
               break;
            default:
                turnRight = false;
                break;
        }
    }

    public IEnumerator RunInstruction(ExecutionContext context)
    {
        if (turnRight)
            yield return context.player.TurnRight();
        else
            yield return context.player.TurnLeft();
    }
}