using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerAim : MonoBehaviour
{
    public Camera cam;
    public GameObject gun;
    public Transform crosshairPoint;
    

    [Header("Ray Settings")]
    public float maxDistance = 100f;
    public float surfaceOffset = 0.02f;

    private PlayerInput playerInput;
    private InputAction aimAction;
    private bool isAiming = false;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        aimAction = playerInput.actions["Aim"];
        aimAction.performed += OnAimToggle;
    }

    void OnEnable() => aimAction.Enable();
    void OnDisable() => aimAction.Disable();

    private void OnAimToggle(InputAction.CallbackContext ctx)
    {
        isAiming = !isAiming;
        crosshairPoint.gameObject.SetActive(isAiming); // 에임 ON/OFF 시 전체 표시 전환
        gun.SetActive(isAiming);
        Debug.Log(isAiming ? "🔍 Aiming ON" : "❌ Aiming OFF");
    }

    void Update()
    {
        if (!isAiming) return;

        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        bool hasHit = Physics.Raycast(ray, out RaycastHit hit, maxDistance);

        if (hasHit)
        {
            crosshairPoint.position = hit.point + hit.normal * surfaceOffset;
            crosshairPoint.rotation = Quaternion.LookRotation(hit.normal);

            if (!crosshairPoint.gameObject.activeSelf)
                crosshairPoint.gameObject.SetActive(true);

            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }
        else
        {
            // 이전 프레임에만 활성화되어 있었다면 한 번만 비활성화
            if (crosshairPoint.gameObject.activeSelf)
                crosshairPoint.gameObject.SetActive(false);

            Debug.DrawLine(ray.origin, ray.origin + ray.direction * maxDistance, Color.red);
        }
    }
}