using System.Collections;
using System.Collections.Generic;
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
        runRoutine = StartCoroutine(RunProgram());
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

    IEnumerator RunProgram()
    {
        List<IInstruction> instructions = new List<IInstruction>();
        List<CommandBase> commandBases = new List<CommandBase>();

        foreach (Transform child in transform)
        {
            var ins = child.GetComponent<IInstruction>();
            var cmd = child.GetComponent<CommandBase>();

            if (ins != null && cmd != null)
            {
                instructions.Add(ins);
                commandBases.Add(cmd);
            }
        }

        for (int i = 0; i < commandBases.Count; i++)
        {
            commandBases[i].runtimeIndex = i;
        }

        int ip = 0;
        int safety = 0;

        while (ip >= 0 && ip < instructions.Count)
        {
            if (GameManager.Instance.CurrentState != GameState.Running)
                yield break;

            while (isPaused)
                yield return null;

            safety++;
            if (safety > 500)
            {
                Debug.LogWarning("Infinite loop detected");
                GameManager.Instance.SetState(GameState.Fail);
                yield break;
            }

            int currentIp = ip;

            currentIp = ip;
            SetHighlight(commandBases, currentIp, true);

            int nextIP = currentIp + 1;

            yield return instructions[currentIp]
                .Execute(context, (newIP) => nextIP = newIP, currentIp);

            yield return new WaitForSeconds(commandDelay);

            SetHighlight(commandBases, currentIp, false);

            ip = nextIP;

            yield return new WaitForSeconds(commandDelay);

            SetHighlight(commandBases, currentIp, false);

            if (grid.IsGoal(player.cellPos))
            {
                GameManager.Instance.SetState(GameState.Win);
                yield break;
            }
        }

        GameManager.Instance.SetState(GameState.Fail);
    }

    void SetHighlight(List<CommandBase> commands, int index, bool value)
    {
        if (index < 0 || index >= commands.Count)
            return;

        var visual = commands[index].GetComponent<ICommandVisual>();
        if (visual != null)
            visual.SetHighlight(value);
    }
}