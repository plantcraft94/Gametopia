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

    bool isPaused = false;

    void Awake()
    {
        context = new ExecutionContext(player, grid);
    }

    public void StartProgram()
    {
        if (GameManager.Instance.CurrentState != GameState.Idle)
            return;

        GameManager.Instance.SetState(GameState.Running);

        runRoutine = StartCoroutine(RunInstruction());
    }

    public void PauseProgram()
    {
        isPaused = !isPaused;
    }

    public void RestartProgram()
    {
        StopAllCoroutines();
        player.ResetPlayer();
        GameManager.Instance.SetState(GameState.Idle);
    }


    IEnumerator RunInstruction()
    {
        foreach (Transform child in transform)
        {
            if (GameManager.Instance.CurrentState != GameState.Running)
                yield break;

            IInstruction instruction = child.GetComponent<IInstruction>();
            ICommandVisual visual = child.GetComponent<ICommandVisual>();

            if (instruction != null)
            {
                if (visual != null)
                    visual.SetHighlight(true);

                yield return instruction.RunInstruction(context);
                yield return new WaitForSeconds(commandDelay);

                if (visual != null)
                    visual.SetHighlight(false);

                if (grid.IsGoal(player.cellPos))
                {
                    GameManager.Instance.SetState(GameState.Win);
                    yield break;
                }
            }
        }

        GameManager.Instance.SetState(GameState.Fail);
    }
}