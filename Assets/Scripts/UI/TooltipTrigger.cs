using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string tooltipText = "Feature description";
    public float delay = 0.5f;

    private GameObject tooltipInstance;
    private bool isPointerOver = false;
    private float timer = 0f;

    public static Canvas mainCanvas;

    void Start()
    {
        if (mainCanvas == null)
            mainCanvas = FindObjectOfType<Canvas>();
    }

    void Update()
    {
        if (isPointerOver)
        {
            timer += Time.deltaTime;
            if (timer >= delay && tooltipInstance == null)
            {
                ShowTooltip();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        timer = 0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        timer = 0f;
        HideTooltip();
    }

    void ShowTooltip()
    {
        if (TooltipManager.Instance != null)
        {
            TooltipManager.Instance.ShowTooltip(tooltipText, transform.position);
        }
    }

    void HideTooltip()
    {
        if (TooltipManager.Instance != null)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}