using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public InstructionRunner runner;

    List<Instruction> program;

    void Start()
    {
        program = new List<Instruction>()
{
            new Instruction(CommandType.Move),
            new Instruction(CommandType.Move),
            new Instruction(CommandType.Turn, 1),
            new Instruction(CommandType.Move),
            new Instruction(CommandType.Turn, 0),
            new Instruction(CommandType.Move),
};
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(runner.RunProgram(program));
        }
    }
}