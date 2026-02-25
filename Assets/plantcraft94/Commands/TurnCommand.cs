using System.Collections;
using UnityEngine;
using TMPro;

public class TurnCommand : MonoBehaviour, IInstruction
{
    [SerializeField] TMP_Dropdown dropdown;

    bool turnRight;

    void Awake()
    {
        dropdown.onValueChanged.AddListener(OnDirectionChanged);
        OnDirectionChanged(dropdown.value);
    }

    public void OnDirectionChanged(int index)
    {
        turnRight = index == 1;
    }

    public IEnumerator RunInstruction(ExecutionContext context)
    {
        var player = context.player;

        player.CommitTurn(turnRight);
        yield return player.AnimateRotate();
    }
}