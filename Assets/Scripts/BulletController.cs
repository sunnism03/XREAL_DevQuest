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
        // 권장 설정: RB에서 Use Gravity=OFF, Collision Detection=Continuous
    }

    void OnEnable()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        CancelInvoke();
        Invoke(nameof(Disable), lifetime);
    }

    public void Fire(Vector3 dir)
    {
        rb.velocity = dir.normalized * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.rigidbody != null)
            {
                Vector3 knockbackDir = -collision.contacts[0].normal;
                collision.rigidbody.AddForce(knockbackDir * 300f, ForceMode.Impulse);
            }
        }
        Disable();
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}