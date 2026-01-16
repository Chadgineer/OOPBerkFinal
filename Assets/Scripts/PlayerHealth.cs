using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float maxHealth = 100f;
    [SerializeField] private float currentHealth;
    public TextMeshPro healthText;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("Hasar aldim" + amount);
        currentHealth -= amount;
        healthText.text = currentHealth.ToString() + " / 100";
        if (currentHealth <= 0)
        {
            Debug.Log("Game Over");
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}