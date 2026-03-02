using TMPro;
using UnityEngine;

public class ToolCommandUI : MonoBehaviour
{
    [SerializeField] TMP_Dropdown toolDropdown;

    ToolCommand command;

    void Awake()
    {
        command = GetComponent<ToolCommand>();

        toolDropdown.onValueChanged.AddListener(OnToolChanged);

        toolDropdown.value = (int)command.toolType;
    }

    void OnToolChanged(int index)
    {
        command.toolType = (ToolType)index;
    }
}