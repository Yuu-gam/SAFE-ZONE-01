using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class BugMovement : MonoBehaviour
{
    public BugSpawner bugSpawner;
    private Camera arCamera;

    public float speed = 0.5f; //속도
    public float moveRadius = 0.5f; //이동 반경
    public float lifeTime = 5f;
    private Vector3 targetPosition; //목표 지점

    private bool isDestroyed = false;

    void Start()
    {
        arCamera = Camera.main;
        ResetTarget();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        CheckTouchInput();

        //목표 지점으로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        //목표 지점에 도달 시 새로운 목표 지정
        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            ResetTarget();
        }

        CheckScreenOut();
    }

    //랜덤으로 목표 지점 지정
    void ResetTarget()
    {
        Vector2 randDir = Random.insideUnitCircle * moveRadius;
        targetPosition = new Vector3(transform.position.x + randDir.x, transform.position.y, transform.position.z + randDir.y);
    }

    void CheckScreenOut()
    {
        if (arCamera == null)
        {
            return;
        }

        Vector3 screenPos = arCamera.WorldToViewportPoint(transform.position);

        if(screenPos.x < 0 || screenPos.x > 1 ||
            screenPos.y < 0 || screenPos.y > 1 ||
            screenPos.z < 0)
        {
            RemoveBug();
        }

    }

    void RemoveBug()
    {
        if(isDestroyed)
        {
            return;
        }

        isDestroyed = true;

        if(bugSpawner != null)
        {
            bugSpawner.currentSpawned--;
        }

        Debug.Log("삭제");
        Destroy(gameObject);
    }


    void CheckTouchInput()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            Vector2 inputPos = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;

            Ray ray = arCamera.ScreenPointToRay(inputPos);
            RaycastHit hit;

            // 레이저를 쏴서 내 몸(벌레)에 맞았는지 확인
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    CatchBug();
                }
            }
        }
    }

    public void CatchBug()
    {
        if(bugSpawner != null)
        {
            RemoveBug();
            Debug.Log("포획");
        }
    }
}
