using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int[] globalspace = new int[4];

    private void Awake()
    {

        // Singleton 설정
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
