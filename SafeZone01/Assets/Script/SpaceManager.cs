using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpaceManager : MonoBehaviour
{
    public GameObject targetObject;
    public Image targetImage;
    public Sprite[] changedSprite = new Sprite[4];

    public void visitSpace(int spaceIndex)
    {
        GameManager.Instance.globalbug[spaceIndex] = 1;
    }

    public void viewDiary(int diaryIndex)
    {
        if (GameManager.Instance.globalbug[diaryIndex] == 1 && GameManager.Instance.globalspace[diaryIndex] == 1)
        {
            targetObject.SetActive(true);
            targetImage.sprite = changedSprite[diaryIndex];
        }
        
    }
}
