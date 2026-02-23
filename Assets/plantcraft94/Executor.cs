using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Executor : MonoBehaviour
{
    Coroutine runInstruction;
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
                yield return currentInstruction.RunInstruction();               
            }
        }
        runInstruction = null;
    }
}
