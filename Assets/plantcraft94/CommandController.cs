using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandController : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, ICommandVisual
{
    public Transform parent;
    GameObject HigherLayer;

    Canvas canvas;
    [SerializeField] GameObject placeholder;

    RectTransform rectTransform;
    Image image;

    GameObject currentPlaceholder;
    bool isDragging = false;

    void Awake()
    {
        if (canvas == null)
            canvas = GameObject.FindGameObjectWithTag("UICanvas").GetComponent<Canvas>();
            HigherLayer = GameObject.FindGameObjectWithTag("Dropable");

        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    [SerializeField] Image background;
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color highlightColor = new Color(1f, 0.9f, 0.3f);

    public void SetHighlight(bool value)
    {
        if (background == null)
            background = GetComponent<Image>();

        background.color = value ? highlightColor : normalColor;
    }

    // =============================
    // DRAG START
    // =============================

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentState != GameState.Idle)
            return;

        isDragging = true;

        currentPlaceholder = Instantiate(placeholder, parent);
        currentPlaceholder.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(HigherLayer.transform);
        transform.SetAsLastSibling();

        image.raycastTarget = false;
    }

    // =============================
    // DRAGGING
    // =============================

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        UpdatePlaceholderIndex();
    }

    // =============================
    // DRAG END
    // =============================

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging)
            return;

        isDragging = false;

        if (currentPlaceholder == null)
            return;

        if (IsValidDrop(eventData))
        {
            transform.SetParent(parent);
            transform.SetSiblingIndex(currentPlaceholder.transform.GetSiblingIndex());
        }
        else
        {
            PairedCommand paired = GetComponent<PairedCommand>();

            if (paired != null && paired.pair != null)
            {
                Destroy(paired.pair.gameObject);
            }

            Destroy(gameObject);
        }

        Destroy(currentPlaceholder);
        image.raycastTarget = true;
    }

    // =============================
    // PLACEHOLDER LOGIC
    // =============================

    void UpdatePlaceholderIndex()
    {
        int newIndex = parent.childCount;

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);

            if (child == currentPlaceholder.transform)
                continue;

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

    // =============================
    // DROP CHECK
    // =============================

    bool IsValidDrop(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("Dropable"))
                return true;
        }

        return false;
    }

}