using UnityEngine;
using System.Collections;

public class Command3 : MonoBehaviour, IInstruction
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator RunInstruction()
    {
        Debug.Log("Command 3");
        yield return new WaitForSeconds(1f);
    }
}
