using System.Collections;
using UnityEngine;

public class Executor : MonoBehaviour
{
    [Header("Refs")]
    public PlayerController player;
    public GridManager grid;

    [Header("Timing")]
    public float commandDelay = 0.1f;

    Coroutine runRoutine;
    ExecutionContext context;

    bool hasStarted = false;
    bool isPaused = false;

    void Awake()
    {
        context = new ExecutionContext(player, grid);
    }

    public void StartProgram()
    {
        if (hasStarted) return;

        hasStarted = true;
        isPaused = false;

        runRoutine = StartCoroutine(RunInstruction());
    }

    public void PauseProgram()
    {
        isPaused = !isPaused;
    }

    public void RestartProgram()
    {
        StopAllCoroutines();
        runRoutine = null;

        hasStarted = false;
        isPaused = false;

        player.ResetPlayer();
    }


    IEnumerator RunInstruction()
    {
        foreach (Transform child in transform)
        {
            while (isPaused)
                yield return null;

            IInstruction instruction = child.GetComponent<IInstruction>();

            if (instruction != null)
            {
                yield return instruction.RunInstruction(context);
                yield return new WaitForSeconds(commandDelay);
            }
        }

        Debug.Log("PROGRAM END");
    }
}