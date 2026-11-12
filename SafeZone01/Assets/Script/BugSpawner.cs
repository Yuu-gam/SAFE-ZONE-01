using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BugSpawner : MonoBehaviour
{

    public GameObject PF_BugDefault;
    ARPlaneManager arPlaneManager;

    public float spawnProbability = 30f;    //벌레 생성 확률
    public int maxSpawn = 3;              //벌레 생성 최대 마리 수
    public int currentSpawned = 0;         //현재 생성된 벌레 마리 수

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
            Debug.Log("새로운 바닥 인식");

            if (currentSpawned >= maxSpawn)
            {
                break;
            }

            float rand = Random.Range(0f, 100f);
            if(rand <= spawnProbability)
            {
                SpawnBug(plane);
                break;
            }
        }
    }

    void SpawnBug(ARPlane plane)
    {
        if (PF_BugDefault == null) return;
        
        //인식된 바닥의 중심 좌표
        Vector3 spawnPosition = plane.transform.TransformPoint(plane.center);

        //벌레 생성
        GameObject spawnedBug = Instantiate(PF_BugDefault, spawnPosition, Quaternion.identity);

        BugMovement movement = spawnedBug.GetComponent<BugMovement>();
        movement.bugSpawner = this;
        
        currentSpawned++;
        Debug.Log("생성");
    }
}
