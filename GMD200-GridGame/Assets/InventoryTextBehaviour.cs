using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections;

public class InventoryTextBehaviour : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField]
    private Image bg;

    [SerializeField]
    private float lifetime = 1.0f;

    [SerializeField]
    private float fadeTime = 1.0f;

    public ItemSO MyItem { get; private set; }
    public int Count { get; private set; }

    Timer timer;

    InventoryTextIndicator myIndicator;

    private void Start()
    {   
        myIndicator = GetComponentInParent<InventoryTextIndicator>();
    }

    private void Update() => timer.Tick(Time.deltaTime);

    /// <summary>
    /// Handles death
    /// </summary>
    /// <returns></returns>
    private IEnumerator Death()
    {
        float elapsed = 0.0f;
        Color color = textMesh.color;

        while (elapsed < fadeTime) {
            textMesh.color = new Color(color.r, color.g, color.b, Mathf.Lerp(1, 0, elapsed / fadeTime));
            bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, Mathf.Lerp(0.6f, 0, elapsed / fadeTime));

            elapsed += Time.deltaTime;

            yield return null;
        }

        textMesh.color = new Color(color.r, color.g, color.b, 0);
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0);

        myIndicator.Remove(this);
        yield return null;

        Destroy(gameObject);
    }

    public void SetItem(ItemSO myItem, int count)
    {
        MyItem = myItem;
        Count = count;

        textMesh.text = $"{Count} {myItem.name}";

        // set timer
        timer = new Timer(lifetime);
        timer.OnTimerEnd += () => StartCoroutine(Death());

        // stop and reset color
        StopAllCoroutines();

        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1);
        bg.color = new Color(bg.color.r, bg.color.g, bg.color.b, 0.6f);
    }
}
