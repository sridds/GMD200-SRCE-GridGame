using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class Tooltip : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TextMeshProUGUI headerText;

    [SerializeField]
    private TextMeshProUGUI subheaderText;

    [SerializeField]
    private Image image;

    [SerializeField]
    private RectTransform background;

    [SerializeField]
    private RectTransform holder;

    [Header("Modifiers")]
    [SerializeField]
    private float inTime = 0.5f;

    [SerializeField]
    private float holdTime = 2.0f;

    [SerializeField]
    private float outTime = 0.5f;

    [SerializeField]
    private Ease easeIn = Ease.OutBounce;

    [SerializeField]
    private Ease easeOut = Ease.OutSine;

    public bool IsReady { get; private set; }

    private void Start()
    {
        IsReady = true;
    }
    /// <summary>
    /// Initializes a tooltip with the data provided
    /// </summary>
    /// <param name="data"></param>
    public void Initialize(TooltipData data) {
        // initialize to the data inside tooltip data 
        headerText.text = data.headerText;
        subheaderText.text = data.subheaderText;
        image.sprite = data.icon;

        IsReady = false;

        StartCoroutine(StartTooltip());
    }

    private IEnumerator StartTooltip()
    {
        Vector2 initial = new Vector2(holder.anchoredPosition.x, holder.anchoredPosition.y);
        Vector2 target = new Vector2(holder.anchoredPosition.x, holder.anchoredPosition.y + (background.rect.y * 2));

        holder.anchoredPosition = initial;

        // do Y bounce
        yield return holder.DOAnchorPosY(target.y, inTime).SetEase(easeIn);
        yield return new WaitForSeconds(holdTime);
        yield return holder.DOAnchorPosY(initial.y, outTime).SetEase(easeOut);

        IsReady = true;
    }
}
