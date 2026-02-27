using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class TurnCommand : CommandBase, IInstruction
{
    [SerializeField] TMP_Dropdown dropdown;

    bool turnRight;

    void Awake()
    {
        dropdown.onValueChanged.AddListener(OnDirectionChanged);
        OnDirectionChanged(dropdown.value);
    }

    void OnDirectionChanged(int index)
    {
        turnRight = index == 1;
    }

    public IEnumerator Execute(
        ExecutionContext context,
        Action<int> jumpTo,
        int currentIP)
    {
        var player = context.player;

        player.CommitTurn(turnRight);
        yield return player.AnimateRotate();

        jumpTo(currentIP + 1);
    }
}