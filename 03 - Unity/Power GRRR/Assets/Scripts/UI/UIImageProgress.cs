using UnityEngine;
using UnityEngine.UI;

public class UIImageProgress : MonoBehaviour
{
    public Image image;
    public Sprite[] progressImages;

    public void Progess(float progress)
    {
        int index = Mathf.RoundToInt((progressImages.Length-1) * Mathf.Clamp01(progress));
        image.sprite = progressImages[index];
    }
}
