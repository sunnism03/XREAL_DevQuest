using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 5;
    private int currentHP;

    private Animator animator;

    void Start()
    {
        currentHP = maxHP;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        currentHP -= amount;

        // 🔥 stun 애니메이션 재생
        if (animator != null)
        {
            animator.SetTrigger("stun");
        }

        Debug.Log($"Enemy Hit! HP → {currentHP}");

        if (currentHP <= 0)
            Die();
    }

    private void Die()
    {
        GameManager.Instance.EnemyKilled();
        Destroy(gameObject);
    }
}