using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 3;
    private int currentHP;

    public GameObject gameOverUI; // Canvas에 넣어줄 것

    void Start()
    {
        currentHP = maxHP;
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"💔 Player damaged: -{amount} → HP {currentHP}");

        if (currentHP <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("💀 GAME OVER");

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Time.timeScale = 0f;  // 게임 정지
    }
}