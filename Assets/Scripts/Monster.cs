using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    public Transform target;

    NavMeshAgent nmAgent;

    [Header("Chase Settings")]
    public float chaseDistance = 30f;     // 추적 시작 거리
    public float viewAngle = 120f;         // 시야각 (ex: 90도)

    private void Start()
    {
        nmAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (target == null) return;

        // 거리 체크
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > chaseDistance) 
        {
            nmAgent.ResetPath();
            return;
        }

        // 시야각 체크
        Vector3 dirToPlayer = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle > viewAngle * 0.5f)
        {
            // 플레이어가 정면 시야 밖이면 추적 중단
            nmAgent.ResetPath();
            return;
        }

        // ✅ 두 조건 모두 만족 → 추적 시작
        nmAgent.SetDestination(target.position);
    }
}