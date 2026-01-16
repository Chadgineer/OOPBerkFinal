using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public string bulletTag = "Bullet";
    public float fireRate = 0.5f;
    public float spawnOffset = 1f;

    private UnityEngine.Camera cam;
    private float nextFireTime;

    private void Awake()
    {
        cam = UnityEngine.Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector2 direction = (mousePosition - transform.position).normalized;
        Vector2 spawnPosition = (Vector2)transform.position + (direction * spawnOffset);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        ObjectPoolManager.Instance.GetPooledObject(bulletTag, spawnPosition, rotation);
    }
}