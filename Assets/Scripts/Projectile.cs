using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    public float damage = 10f;
    public float lifetime = 2f;
    private float timer;

    private void OnEnable() => timer = lifetime;

    private void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }
}