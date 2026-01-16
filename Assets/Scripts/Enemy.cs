using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyData data;
    public float currentHealth;

    private BaseState currentState;
    private Transform playerTransform;
    private SpriteRenderer spriteRenderer;
    private float lastDamageTime;
    private float damageInterval = 1f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    private void OnEnable()
    {
        currentHealth = data.health;
        SwitchState(new ChaseState());
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public void SwitchState(BaseState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(data.damage);
                lastDamageTime = Time.time;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(data.damage);
                    lastDamageTime = Time.time;
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            GameManager.Instance.EnemyDied();
            gameObject.SetActive(false);
        }
    }

    public Transform GetPlayer() => playerTransform;
}