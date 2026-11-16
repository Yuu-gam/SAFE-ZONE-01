using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static UnityEngine.Rendering.GPUSort;

public class BugSpawner : MonoBehaviour
{

    public GameObject PF_BugDefault;
    ARPlaneManager arPlaneManager;
    private Camera arCamera;

    public float spawnProbability = 30f;    //벌레 생성 확률
    public int maxSpawn = 3;              //벌레 생성 최대 마리 수
    public int currentSpawned = 0;         //현재 생성된 벌레 마리 수
    private float checkInterval = 2.0f;     //바닥 인식 텀

    private float minSpawnDistance = 0.5f;
    private float maxSpawnDistance = 3.0f;

    private Coroutine spawnRoutine;

    private void Awake()
    {
        arPlaneManager = FindFirstObjectByType<ARPlaneManager>();
    }

    private void OnEnable()
    {
        spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDisable()
    {
        if(spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }
    }

    IEnumerator SpawnRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(checkInterval);

        while(true)
        {
            yield return wait;

            if(arPlaneManager == null)
            {
                continue;
            }

            if(arPlaneManager.trackables.count == 0 )
            {
                continue;
            }

            float rand = Random.Range(0f, 100f);
            if (rand <= spawnProbability)
            {
                ARPlane randomPlane = GetRandomPlane();
                if(randomPlane != null)
                {
                    SpawnBug(randomPlane);
                }
            }
        }
    }

    ARPlane GetRandomPlane()
    {
        if (arCamera == null)
        {
            arCamera = Camera.main;
            if (arCamera == null) return null;
        }

        List<ARPlane> validPlanes = new List<ARPlane>();
        foreach(var plane in arPlaneManager.trackables)
        {
            //수평 바닥인지 확인
            if(plane.alignment == PlaneAlignment.HorizontalUp)
            {
                Vector3 planeCenter = plane.transform.TransformPoint(plane.center);

                // 2. 카메라와의 거리 계산
                float distance = Vector3.Distance(arCamera.transform.position, planeCenter);
                if (distance < minSpawnDistance || distance > maxSpawnDistance)
                {
                    continue; // 너무 가깝거나 멀면 패스
                }

                // 3. 카메라 시야(Viewport) 안에 있는지 확인 (Z > 0은 카메라 앞쪽을 의미)
                Vector3 viewportPos = arCamera.WorldToViewportPoint(planeCenter);
                if (viewportPos.z > 0 &&
                    viewportPos.x >= 0.0f && viewportPos.x <= 1.0f &&
                    viewportPos.y >= 0.0f && viewportPos.y <= 1.0f)
                {
                    validPlanes.Add(plane);
                }
            }
        }

        if(validPlanes.Count == 0)
        {
            return null;
        }

        int randInt = Random.Range(0, validPlanes.Count);
        return validPlanes[randInt];
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
