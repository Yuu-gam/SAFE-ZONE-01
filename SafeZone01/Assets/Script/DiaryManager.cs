using UnityEngine;
using UnityEngine.UI;

public class DiaryManager : MonoBehaviour
{
    public int bug_index;
    public Image targetImage;
    public Sprite changedSprite;

    private void Update()
    {
        if (GameManager.Instance.globalbug[bug_index] == 1)
        {
            targetImage.sprite = changedSprite;
        }
    }

}
