using UnityEngine;
using UnityEngine.SceneManagement;

public class SpaceManager : MonoBehaviour
{
    public void visitSpace(int spaceIndex)
    {
        GameManager.Instance.globalbug[spaceIndex] = 1;
    }
}
