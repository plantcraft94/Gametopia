using System.Collections.Generic;
using UnityEngine;

public class CommandIndentController : MonoBehaviour
{
    [SerializeField] float indentWidth = 40f;

    public void RebuildIndent()
    {
        int indent = 0;

        List<CommandBase> commands = new List<CommandBase>();

        foreach (Transform child in transform)
        {
            var cmd = child.GetComponent<CommandBase>();
            if (cmd != null)
                commands.Add(cmd);
        }

        foreach (var cmd in commands)
        {
            bool isClosing = IsClosing(cmd);
            bool isOpening = IsOpening(cmd);

            if (isClosing)
                indent = Mathf.Max(0, indent - 1);

            ApplyIndent(cmd, indent);

            if (isOpening)
                indent++;
        }
    }

    bool IsOpening(CommandBase cmd)
    {
        return cmd is IfCommand ||
               cmd is JumpToCommand;
    }

    bool IsClosing(CommandBase cmd)
    {
        return cmd is EndIfCommand ||
               cmd is JumpCommand;
    }

    void ApplyIndent(CommandBase cmd, int indentLevel)
    {
        var indentHandler = cmd.GetComponent<CommandIndentHandler>();
        if (indentHandler != null)
        {
            indentHandler.SetIndent(indentLevel, indentWidth);
        }
    }
}