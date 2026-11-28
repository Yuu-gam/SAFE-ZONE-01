using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class BugController : MonoBehaviour
{

    public BugSpawner bugSpawner;
    private Camera arCamera;

    public float speed = 0.5f; //속도
    public float moveRadius = 0.5f; //이동 반경
    private Vector3 targetPosition; //목표 지점

    private bool isDestroyed = false;
    private int catchCount = 0; //포획 횟수 확인


    //글리치종
    public ARGlitchController glitchController;
    private Renderer[] renderers;
    private float hideDuration = 1.0f; //순간이동 주기
    private float hidePerSecond = 0.2f; //초당 20% 확률로 사라짐
    private float hideCooldown = 3.0f; //3초 쿨타임
    private float cooldownTimer = 0f;
    private bool isHiding = false;


    //형광안료종
    public GameObject PF_Ink;
    private float minInkDistance = 0.05f;
    private Vector3 lastInkPosition;


    void Start()
    {
        arCamera = Camera.main;
        if (arCamera != null && glitchController == null)
        {
            glitchController = arCamera.GetComponent<ARGlitchController>();
        }

        renderers = GetComponentsInChildren<Renderer>(true);
        lastInkPosition = transform.position;
        ResetTarget();
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


        //글리치종이라면 순간이동
        if(cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
        if (GPSManager.Instance.SpaceNum == 0 && !isHiding && cooldownTimer <= 0f)
        {
            if (Random.value < hidePerSecond * Time.deltaTime)
            {
                cooldownTimer = hideCooldown;
                StartCoroutine(HideRoutine(hideDuration));
            }
        }


        //형광안료종이라면 트레일 출력
        if (GPSManager.Instance.SpaceNum == 2) 
        {
            SpawnInkTrail();
        }

        CheckScreenOut();
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
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
        catchCount = 0;

        if (bugSpawner != null)
        {
            bugSpawner.currentSpawned--;
        }

        Debug.Log("삭제");
        Destroy(gameObject);
    }


    void CheckTouchInput()
    {
        Vector2 inputPos = Vector2.zero;
        bool isPressed = false;

        // PC 환경: 마우스 왼쪽 클릭 체크
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            inputPos = Mouse.current.position.ReadValue();
            isPressed = true;
        }
        // 모바일 환경: 화면 터치 체크
        else if (Touch.activeTouches.Count > 0)
        {
            inputPos = Touch.activeTouches[0].screenPosition;
            isPressed = true;
        }

        // 감지 시 레이캐스트 발사
        if (isPressed && arCamera != null)
        {
            Ray ray = arCamera.ScreenPointToRay(inputPos);
            RaycastHit hit;

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
        int[] bug = GameManager.Instance.globalbug;
        int space = GPSManager.Instance.SpaceNum;

        if (bugSpawner == null)
        {
            return;
        }

        if(space == 3)
        {
            catchCount++;
            Debug.Log($"포획 시도:{catchCount}");
            if (catchCount >= 5)
            {
                bug[3] = 1;
                RemoveBug();
            }
        }
        else if (0 <= space && space < bug.Length)
        {
            bug[space] = 1;
            RemoveBug();
        }
    }


    //글리치종 순간이동
    IEnumerator HideRoutine(float duration)
    {
        isHiding = true;
        SetRenderers(false);
        glitchController?.TriggerGlitch();
        Debug.Log("순간이동");

        yield return new WaitForSeconds(duration);

        isHiding = false;
        SetRenderers(true);
        glitchController?.TriggerGlitch();
    }
    void SetRenderers(bool enabled)
    {
        foreach(var renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.enabled = enabled;
            }
        }
    }


    //형광안료종 트레일 출력
    void SpawnInkTrail()
    {
        if(PF_Ink == null)
        {
            return;
        }

        if(Vector3.Distance(transform.position, lastInkPosition) >= minInkDistance)
        {
            Vector3 inkPos = new Vector3(transform.position.x, transform.position.y + 0.005f, transform.position.z);
            Instantiate(PF_Ink, inkPos, PF_Ink.transform.rotation);
            lastInkPosition = transform.position;
        }
    }
}
