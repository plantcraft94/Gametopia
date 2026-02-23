using System.Collections;
using UnityEngine;

public class Command1 : MonoBehaviour, IInstruction
{
    public IEnumerator RunInstruction()
    {
        Debug.Log("Command 1");
        yield return new WaitForSeconds(1f);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
