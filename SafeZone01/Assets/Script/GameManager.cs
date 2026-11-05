using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int Space1;
    public int Space2;
    public int Space3;
    public int Space4;


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
