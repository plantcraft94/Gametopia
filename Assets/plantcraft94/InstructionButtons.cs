using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InstructionButtons : MonoBehaviour
{
    [SerializeField] CommandController commandPrefab;
    public Transform InstructionCanvas;
    [SerializeField] Canvas canvas;

    public void OnClick()
    {
        CommandController current = Instantiate(commandPrefab,InstructionCanvas);
        current.parent = InstructionCanvas;
        // random the color
        current.gameObject.GetComponent<Image>().color = Random.ColorHSV();
    }
}
