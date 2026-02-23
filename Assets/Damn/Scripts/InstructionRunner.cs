using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionRunner : MonoBehaviour
{
    public PlayerController player;

    public IEnumerator RunProgram(List<Instruction> program)
    {
        for (int pc = 0; pc < program.Count; pc++)
        {
            Instruction inst = program[pc];

            switch (inst.command)
            {
                case CommandType.Move:
                    yield return player.MoveForward();
                    break;

                case CommandType.Turn:
                    if (inst.parameter == 0)
                        yield return player.TurnLeft();
                    else
                        yield return player.TurnRight();
                    break;

                case CommandType.Wait:
                    yield return player.Wait();
                    break;
            }
        }

        Debug.Log("Program finished");
    }
}