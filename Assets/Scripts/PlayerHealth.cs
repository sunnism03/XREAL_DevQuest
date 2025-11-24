using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 5;
    private int currentHP;

    [Header("UI")]
    public Slider hpSlider;   // 손목이나 머리 위 World Space Slider

    private void Start()
    {
        currentHP = maxHP;
        if (hpSlider)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = currentHP;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (hpSlider)
            hpSlider.value = currentHP;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.OnPlayerDead();
    }
}