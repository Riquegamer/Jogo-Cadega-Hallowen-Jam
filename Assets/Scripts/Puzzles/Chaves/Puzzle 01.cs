using UnityEngine;
using UnityEngine.EventSystems;

public class Puzzle01 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Procura o canvas automaticamente
        canvas = GameObject.FindWithTag("PRINCIPAL CANVAS")?.GetComponent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Torna o item semi-transparente e ignorável para raycast
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        // Ajusta a movimentação conforme o tamanho do Canvas
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out pos
        );

        rectTransform.anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restaura visibilidade e raycast
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
