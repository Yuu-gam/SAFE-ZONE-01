using UnityEngine;

public class BugMovement : MonoBehaviour
{
    public float speed = 0.5f; //속도
    public float moveRadius = 0.5f; //이동 반경
    private Vector3 targetPosition; //목표 지점

    void Start()
    {
        ResetTarget();
    }

    void Update()
    {
        //목표 지점으로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        //목표 지점에 도달 시 새로운 목표 지정
        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            ResetTarget();
        }
    }

    //랜덤으로 목표 지점 지정
    void ResetTarget()
    {
        Vector2 randDir = Random.insideUnitCircle * moveRadius;
        targetPosition = new Vector3(transform.position.x + randDir.x, transform.position.y, transform.position.z + randDir.y);
    }
}
