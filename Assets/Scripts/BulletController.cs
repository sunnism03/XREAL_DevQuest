using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    public float speed = 25f;
    public float lifetime = 3f;
    public Rigidbody rb;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        CancelInvoke();
        Invoke(nameof(Disable), lifetime);
    }

    public void Fire(Vector3 dir)
    {
        rb.linearVelocity = dir.normalized * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"🟡 Bullet collided with {collision.gameObject.name}");

        // Enemy 데미지 처리
        EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            Vector3 hitPoint = collision.contacts[0].point;

            enemy.TakeDamage(1, hitPoint);   // Bullet 당 데미지 = 1
            Debug.Log("🎯 Bullet HIT Enemy! Damage 1 applied.");
        }

        Disable();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}