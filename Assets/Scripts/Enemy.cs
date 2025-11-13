using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Preset Fields")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject splashFx;

    [Header("Settings")]
    [SerializeField] private float attackRange = 2f;          // 근거리 공격 범위
    [SerializeField] private float detectRange = 20f;         // 추적 시작 거리
    [SerializeField] private float moveSpeed = 3.5f;

    private NavMeshAgent agent;
    private Transform player;
    private bool attackDone;

    public enum State
    {
        None,
        Idle,
        Chase,
        Attack,
        RangedAttack
    }

    [Header("Debug")]
    public State state = State.None;
    public State nextState = State.None;

    private void Start()
    {
        state = State.None;
        nextState = State.Idle;

        agent = GetComponent<NavMeshAgent>();
        if (agent != null) agent.speed = moveSpeed;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        // 1️⃣ 스테이트 전환 조건 판단
        if (nextState == State.None)
        {
            switch (state)
            {
                case State.Idle:
                    if (IsPlayerInRange(detectRange))
                        nextState = State.Chase;
                    break;

                case State.Chase:
                    if (IsPlayerInRange(attackRange))
                        nextState = State.Attack;
                    else if (!IsPlayerInRange(detectRange))
                        nextState = State.Idle;
                    break;
                case State.Attack:
                    if (attackDone)
                    {
                        nextState = State.Idle;
                        attackDone = false;
                    }
                    break;
            }
        }

        // 2️⃣ 스테이트 초기화
        if (nextState != State.None)
        {
            state = nextState;
            nextState = State.None;

            switch (state)
            {
                case State.Idle:
                    agent.isStopped = true;
                    animator.SetBool("isRunning", false);
                    break;

                case State.Chase:
                    agent.isStopped = false;
                    animator.SetBool("isRunning", true);
                    break;

                case State.Attack:
                    agent.isStopped = true;
                    animator.SetTrigger("attack");
                    break;
            }
        }

        // 3️⃣ 글로벌 & 스테이트 업데이트
        if (state == State.Chase)
        {
            if (agent.enabled && player != null)
                agent.SetDestination(player.position);
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.position) <= range;
    }

    // ===== 애니메이션 이벤트 =====
    public void InstantiateFx()
    {
        Instantiate(splashFx, transform.position, Quaternion.identity);
    }


    public void WhenAnimationDone()
    {
        attackDone = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}