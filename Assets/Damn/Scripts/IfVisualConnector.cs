using UnityEngine;
using UnityEngine.UI;

public class IfVisualConnector : MonoBehaviour
{
    public RectTransform lineImage;
    public RectTransform endBlock;
    RectTransform rect;

    IfCommand ifCommand;
    JumpCommand jumpCommand;

    void Awake()
    {
        ifCommand = GetComponent<IfCommand>();
        jumpCommand = GetComponent<JumpCommand>();
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (ifCommand.pair == null && jumpCommand.pair == null) return;
        if (ifCommand != null)
        {
            var end = ifCommand.pair.gameObject.GetComponent<RectTransform>();

            Vector3 startPos = rect.anchoredPosition;
            Vector3 endPos = end.anchoredPosition;

            Debug.Log($"{endPos.y} , {end.sizeDelta.y}, {startPos.y}, {rect.sizeDelta.y}");

            float height = Mathf.Abs(endPos.y - end.sizeDelta.y/2 - (startPos.y + rect.sizeDelta.y/2));

            lineImage.sizeDelta = new Vector2(lineImage.sizeDelta.x, height);
            // lineImage.position = new Vector3(
            //     lineImage.rect.position.x,
            //     (startPos.y + endPos.y) / 2f,
            //     0);
        }
        if (jumpCommand != null)
        {
            var end = jumpCommand.pair.GetComponent<RectTransform>();

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
}