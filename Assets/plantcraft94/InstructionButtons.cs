using UnityEngine;
using UnityEngine.UI;

public class InstructionButtons : MonoBehaviour
{
    [SerializeField] CommandController commandPrefab;

    [SerializeField] CommandController endIfPrefab;
    [SerializeField] CommandController jumpPrefab;
    [SerializeField] CommandController jumpToPrefab;

    public Transform InstructionCanvas;

    public void OnClick()
    {
    CommandController current =
        Instantiate(commandPrefab, InstructionCanvas);

    current.parent = InstructionCanvas;

        CommandController current =
            Instantiate(commandPrefab, InstructionCanvas);

        current.parent = InstructionCanvas;


        IfCommand ifCmd = current.GetComponent<IfCommand>();

        if (ifCmd != null && endIfPrefab != null)
        {
            CommandController endObj =
                Instantiate(endIfPrefab, InstructionCanvas);

            endObj.parent = InstructionCanvas;

            EndIfCommand endCmd =
                endObj.GetComponent<EndIfCommand>();

            ifCmd.pair = endCmd;
            endCmd.pair = ifCmd;
        }

        JumpCommand jump = current.GetComponent<JumpCommand>();

        if (jump != null && jumpToPrefab != null)
        {
            CommandController jumpToObj =
                Instantiate(jumpToPrefab, InstructionCanvas);

            jumpToObj.parent = InstructionCanvas;

            JumpToCommand jumpTo =
                jumpToObj.GetComponent<JumpToCommand>();

            jump.pair = jumpTo;
            jumpTo.pair = jump;
        }
    }
}