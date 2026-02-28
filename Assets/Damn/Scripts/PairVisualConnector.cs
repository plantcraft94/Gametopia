using UnityEngine;

public class IfVisualConnector : MonoBehaviour
{
    [SerializeField] RectTransform lineImage;

    RectTransform rect;
    PairedCommand paired;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        paired = GetComponent<PairedCommand>();
    }

    void LateUpdate()
    {
        if (paired == null || paired.pair == null)
        {
            if (lineImage != null)
                lineImage.gameObject.SetActive(false);

            return;
        }

        if (lineImage == null)
            return;

        lineImage.gameObject.SetActive(true);

        RectTransform end =
            paired.pair.GetComponent<RectTransform>();

        if (end == null)
            return;

        float startY = rect.position.y;
        float endY = end.position.y;

        float height = Mathf.Abs(endY - startY);

        lineImage.sizeDelta =
            new Vector2(lineImage.sizeDelta.x, height);

        lineImage.position =
            new Vector3(
                rect.position.x - 60f,
                (startY + endY) / 2f,
                0);
    }
}