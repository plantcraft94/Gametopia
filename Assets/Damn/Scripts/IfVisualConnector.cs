using UnityEngine;
using UnityEngine.UI;

public class IfVisualConnector : MonoBehaviour
{
    public RectTransform lineImage;
    public RectTransform endBlock;

    IfCommand ifCommand;
    JumpCommand jumpCommand;

    void Awake()
    {
        ifCommand = GetComponent<IfCommand>();
        jumpCommand = GetComponent<JumpCommand>();
    }

    void Update()
    {
        if (ifCommand.pair == null && jumpCommand.pair == null) return;
        if(ifCommand != null){
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
        if(jumpCommand != null){
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