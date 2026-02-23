using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Executor : MonoBehaviour
{
    public PlayerController player;
    public GridManager grid;

    Coroutine runInstruction;
    ExecutionContext context;

    void Awake()
    {
        context = new ExecutionContext(player, grid);
    }

    private void Update()
    {
        if(Keyboard.current.spaceKey.wasPressedThisFrame && runInstruction == null)
        {
            runInstruction = StartCoroutine(RunInstruction());
        }
    }


    public IEnumerator RunInstruction()
    {
        foreach(Transform child in transform)
        {
            IInstruction currentInstruction = child.GetComponent<IInstruction>();
            if (currentInstruction != null)
            {
                yield return currentInstruction.RunInstruction(context);               
            }
        }
        runInstruction = null;
    }
}
