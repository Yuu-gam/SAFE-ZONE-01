using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    public int del_index;

    private void Update()
    {
        if (GameManager.Instance.globalspace[del_index] == 1)
        {
            Destroy(gameObject);
        }
    }

}
