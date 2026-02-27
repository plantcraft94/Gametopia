using UnityEngine;
using UnityEngine.UI;

public class IfVisualConnector : MonoBehaviour
{
    public RectTransform lineImage;
    public RectTransform endBlock;

    IfCommand ifCommand;

    void Awake()
    {
        ifCommand = GetComponent<IfCommand>();
    }

    void Update()
    {
        if (ifCommand.pair == null) return;

        var end = ifCommand.pair.GetComponent<RectTransform>();

        Vector3 startPos = transform.position;
        Vector3 endPos = end.position;

        float height = Mathf.Abs(endPos.y - startPos.y);

        lineImage.sizeDelta = new Vector2(4, height);
        lineImage.position = new Vector3(
            startPos.x - 60f,
            (startPos.y + endPos.y) / 2f,
            0);
    }
}