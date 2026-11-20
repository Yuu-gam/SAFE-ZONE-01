using UnityEngine;
using UnityEngine.UI;

public class SpaceDicManager : MonoBehaviour
{
    public Image TargetImage;
    public Sprite[] activeSpaceDic = new Sprite[4];
    public Sprite[] originalSpaceDic = new Sprite[4];

    private int page = 0;

    public void changePage()
    {
        page++;
        if (page > 3) page = 0;
        
        if (GameManager.Instance.globalspace[page] == 1)
        {
            TargetImage.sprite = activeSpaceDic[page];
        }
        else if (GameManager.Instance.globalspace[page] == 0)
        {
            TargetImage.sprite = originalSpaceDic[page];
        }
    }
}
