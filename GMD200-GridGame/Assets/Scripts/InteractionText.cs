using UnityEngine;
using TMPro;

public class InteractionText : MonoBehaviour
{
    [SerializeField]
    private GameObject holder;

    [SerializeField]
    private TextMeshProUGUI textMesh;

    public void SetInteractionText(string text)
    {
        textMesh.text = text;
        holder.SetActive(true);
    }

    public void HideInteractionText()
    {
        textMesh.text = "";
        holder.SetActive(false);
    }
}
