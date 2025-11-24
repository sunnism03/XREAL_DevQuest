using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    public int damage = 1;
    public float fireRate = 0.2f;
    public float maxDistance = 50f;

    [Header("Ammo")]
    public int maxAmmo = 30;
    private int currentAmmo;
    public TextMeshProUGUI ammoText; // 총 위에 World Space 텍스트

    [Header("Refs")]
    public Transform muzzle;
    public LayerMask hitMask;  // Monster 레이어만 포함하도록 설정

    private float lastFireTime;

    private void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    public void OnActivate(ActivateEventArgs args)
    {
        Shoot();
    }

    private void Shoot()
    {
        if (Time.time - lastFireTime < fireRate)
            return;

        if (currentAmmo <= 0)
        {
            // 나중에 "Reload" 같은 거 표시 가능
            return;
        }

        lastFireTime = Time.time;
        currentAmmo--;
        UpdateAmmoUI();

        Ray ray = new Ray(muzzle.position, muzzle.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, hitMask))
        {
            MonsterHealth mh = hit.collider.GetComponentInParent<MonsterHealth>();
            if (mh != null)
            {
                mh.TakeDamage(damage);
            }
        }

        // TODO: 총구 섬광 VFX, 사운드, 반동 등 추가 가능
    }

    private void UpdateAmmoUI()
    {
        if (ammoText)
        {
            ammoText.text = $"{currentAmmo}/{maxAmmo}";
        }
    }
}