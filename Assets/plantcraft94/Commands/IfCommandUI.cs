using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IfCommandUI : MonoBehaviour
{
    [SerializeField] TMP_Dropdown directionDropdown;
    [SerializeField] TMP_Dropdown tileDropdown;

    IfCommand command;

    void Awake()
    {
        command = GetComponent<IfCommand>();

        directionDropdown.onValueChanged.AddListener(OnDirectionChanged);
        tileDropdown.onValueChanged.AddListener(OnTileChanged);

        directionDropdown.value = (int)command.direction;
        tileDropdown.value = (int)command.checkType;
    }

    void OnDirectionChanged(int index)
    {
        command.direction = (RelativeDirection)index;
    }

    void OnTileChanged(int index)
    {
        command.checkType = (TileCheckType)index;
    }
}