using UnityEngine;

public class InkLifetime : MonoBehaviour
{
    public float lifetime = 3f;
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
