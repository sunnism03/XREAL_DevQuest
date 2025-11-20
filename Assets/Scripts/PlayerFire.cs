using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerFire : MonoBehaviour
{
    [Header("Refs")]
    public PlayerAim playerAim;          // 같은 오브젝트나 Player에 붙은 PlayerAim drag
    public Transform firePoint;          // 총구 위치 (GunHolder/Gun/FirePoint)
    public GameObject bulletPrefab;      // BulletController 포함된 프리팹
    public float fallbackDistance = 100f;

    private PlayerInput playerInput;
    private InputAction fireAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.actions["Fire"];   // ⬅️ Input Action Map: Player/Fire
        fireAction.performed += OnFire;
    }

    void OnEnable() => fireAction.Enable();
    void OnDisable() => fireAction.Disable();

    private void OnFire(InputAction.CallbackContext ctx)
    {
        Debug.Log($"🟢 [FIRE INPUT] Fire action triggered! phase={ctx.phase}, time={Time.time:F2}");

        if (bulletPrefab == null || firePoint == null || playerAim == null)
        {
            Debug.LogWarning("❌ [PlayerFire] Reference missing (bulletPrefab/firePoint/playerAim). Fire aborted.");
            return;
        }

        // 🎯 조준 상태 확인
        Debug.Log($"[AIM STATUS] HasAimHit={playerAim.HasAimHit}, AimHitPoint={playerAim.AimHitPoint:F2}");

        // 목표 지점 계산: 에임이 유효하면 AimHitPoint, 아니면 카메라 정면 Fallback
        Vector3 targetPoint;
        if (playerAim.HasAimHit)
        {
            targetPoint = playerAim.AimHitPoint;
            Debug.Log($"🎯 [TARGET] Using aim hit point → {targetPoint:F2}");
        }
        else
        {
            var cam = playerAim.cam;
            targetPoint = cam.transform.position + cam.transform.forward * fallbackDistance;
            Debug.Log($"⚪ [TARGET] No aim hit → fallback forward {fallbackDistance}m");
        }

        Vector3 dir = (targetPoint - firePoint.position);
        float distance = dir.magnitude;

        if (distance < 0.001f)
        {
            Debug.LogWarning("⚠️ [DIRECTION] Target too close to firePoint, using forward instead.");
            dir = firePoint.forward;
            distance = fallbackDistance;
        }
        dir.Normalize();

        // 🔫 탄환 생성 & 발사
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(dir));
        Debug.Log($"🧩 [BULLET] Spawned '{bullet.name}' at {firePoint.position:F2}");

        var bc = bullet.GetComponent<BulletController>();
        if (bc != null)
        {
            bc.Fire(dir);
            Debug.Log($"🚀 [BULLET FIRE] Using BulletController.Fire(dir), direction={dir}, distance={distance:F2}");
        }
        else
        {
            var rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = dir * 25f;
                Debug.Log($"💨 [BULLET RB] No BulletController, applied velocity={rb.linearVelocity:F2}");
            }
            else
            {
                Debug.LogWarning("❌ [BULLET] No Rigidbody or BulletController found. Bullet won't move!");
            }
        }

        // 🧪 디버깅 비주얼
        Debug.DrawLine(firePoint.position, targetPoint, Color.yellow, 1.0f);
        Debug.Log($"🟡 [TRACE] Fired → dir={dir}, distance={distance:F2}, target={targetPoint:F2}");
    }

    // 씬에서 총구를 보기 쉽게 Gizmo(에디터 전용) 표시
    void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(firePoint.position, 0.03f);
        Gizmos.DrawLine(firePoint.position, firePoint.position + firePoint.forward * 0.5f);
    }
}
