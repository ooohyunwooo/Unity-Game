using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jellymove : MonoBehaviour
{
    public float detectionRange = 5f; // 플레이어 탐지 범위
    public float jumpForce = 5f; // 점프 힘
    public float moveSpeed = 2f; // 왼쪽으로 이동 속도
    public Vector3 destructionBounds = new Vector3(-10f, -10f, -10f); // 제거 위치 (화면 왼쪽, 아래)
    public Transform groundCheck; // Ground 체크 위치
    public float groundCheckRadius = 0.3f; // Ground 체크 반경

    private Transform target; // 타겟(플레이어) Transform
    private Rigidbody rb;
    private Animator animator; // 애니메이터 컴포넌트
    private bool isTriggered = false; // 플레이어 탐지 여부
    private bool isGrounded = false; // 바닥에 있는지 여부

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // 타겟 설정
        GameObject player = GameObject.Find("Character");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Character 오브젝트를 찾을 수 없습니다!");
        }
    }

    void Update()
    {
        // 타겟이 없으면 실행 중지
        if (target == null) return;

        // 바닥 체크
        CheckGround();

        // 플레이어 탐지 및 점프
        if (!isTriggered && PlayerInRange() && isGrounded)
        {
            TriggerJump();
        }

        // 왼쪽 이동
        if (isTriggered)
        {
            rb.velocity = new Vector3(-moveSpeed, rb.velocity.y, 0f);
        }

        // 적 제거 조건
        if (transform.position.x < destructionBounds.x ||
            transform.position.y < destructionBounds.y ||
            transform.position.z < destructionBounds.z)
        {
            Destroy(gameObject);
        }
    }

    private void CheckGround()
    {
        // OverlapSphere로 바닥 탐지
        Collider[] hits = Physics.OverlapSphere(groundCheck.position, groundCheckRadius);

        // "Ground" 태그를 가진 오브젝트가 있는지 확인
        isGrounded = false;
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Ground"))
            {
                isGrounded = true;
                break;
            }
        }
    }

    private bool PlayerInRange()
    {
        // 타겟이 탐지 범위에 있는지 확인
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        return distanceToTarget <= detectionRange;
    }

    private void TriggerJump()
    {
        isTriggered = true;


        // 점프 시작
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);

        // 점프 애니메이션 재생
        if (animator != null)
        {
            animator.SetTrigger("jelly_jump");
        }
    }

    void OnDrawGizmosSelected()
    {
        // 탐지 범위 시각화
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 바닥 체크 위치 시각화
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        // 제거 위치 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(destructionBounds.x, -100f, destructionBounds.z),
                        new Vector3(destructionBounds.x, 100f, destructionBounds.z)); // 왼쪽 경계
    }
}
