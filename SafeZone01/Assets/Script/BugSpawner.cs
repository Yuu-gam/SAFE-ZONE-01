using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BugSpawner : MonoBehaviour
{
    public GameObject PF_BugDefault;
    ARPlaneManager arPlaneManager;

    public float spawnProbability = 50f;    //벌레 생성 확률
    private bool hasSpawned = false;        //벌레 중복 생성 방지

    private void Awake()
    {
        arPlaneManager = FindFirstObjectByType<ARPlaneManager>();
    }

    private void OnEnable()
    {
        if (arPlaneManager != null)
        {
            arPlaneManager.trackablesChanged.AddListener(OnPlanesChanged);
        }
    }

    private void OnDisable()
    {
        if(arPlaneManager != null)
        {
            arPlaneManager.trackablesChanged.RemoveListener(OnPlanesChanged);
        }
    }

    void OnPlanesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
    {
        foreach(var plane in args.added)
        {
            if (hasSpawned) return;

            float rand = Random.Range(0f, 100f);
            if(rand <= spawnProbability)
            {
                SpawnBug(plane);
                hasSpawned = true;
                break;
            }
        }
    }

    void SpawnBug(ARPlane plane)
    {
        if (PF_BugDefault == null) return;
        
        //인식된 바닥의 중심 좌표
        Vector3 spawnPosition = plane.center;

        //벌레 생성
        GameObject spawnedBug = Instantiate(PF_BugDefault, spawnPosition, Quaternion.identity);
        Debug.Log("생성");
    }
}
