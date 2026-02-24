using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parent;
    [SerializeField] Canvas canvas;
    RectTransform rectTransform;
    Image canvasGroup;
    [SerializeField] GameObject placeholder;
    GameObject currentPlaceholder;
    bool isDragging;
    private void Awake()
    {
        canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }
    public void BeginDragManually(PointerEventData eventData)
    {
        OnBeginDrag(eventData);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        currentPlaceholder = Instantiate(placeholder, parent);
        currentPlaceholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        canvasGroup.raycastTarget = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            return;
        }
        rectTransform.anchoredPosition += eventData.delta;
        UpdatePlaceholderIndex();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging)
        {
            return;
        }
        transform.SetParent(parent);
        transform.SetSiblingIndex(currentPlaceholder.transform.GetSiblingIndex());
        Destroy(currentPlaceholder.gameObject);
        isDragging = false;
        canvasGroup.raycastTarget = true;
    }
    void UpdatePlaceholderIndex()
    {
        int newIndex = parent.childCount;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child == currentPlaceholder.transform) continue;

            if (rectTransform.position.y > child.position.y)
            {
                newIndex = i;
                if (currentPlaceholder.transform.GetSiblingIndex() < newIndex)
                    newIndex--;

                break;
            }
        }

        currentPlaceholder.transform.SetSiblingIndex(newIndex);
    }
}
