using UnityEngine;

public class CommandIndentHandler : MonoBehaviour
{
    [SerializeField] RectTransform content;
    [SerializeField] float indentWidth = 40f;

    public void SetIndent(int level)
    {
        if (content == null)
            return;

        content.offsetMin = new Vector2(
            level * indentWidth,
            content.offsetMin.y);
    }
}