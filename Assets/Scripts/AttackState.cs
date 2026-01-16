using UnityEngine;

public class AttackState : BaseState
{
    private float attackCooldown = 1.0f;
    private float lastAttackTime;

    public override void EnterState(Enemy enemy)
    {
        lastAttackTime = Time.time;
    }

    public override void UpdateState(Enemy enemy)
    {
        Transform player = enemy.GetPlayer();
        if (player == null)
        {
            enemy.SwitchState(new ChaseState());
            return;
        }

        float distance = Vector2.Distance(enemy.transform.position, player.position);

        if (distance > 1.5f)
        {
            enemy.SwitchState(new ChaseState());
            return;
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            IDamageable damageable = player.GetComponent<IDamageable>();
            if (damageable == null)
            {
                damageable = player.GetComponentInChildren<IDamageable>();
            }

            if (damageable != null)
            {
                damageable.TakeDamage(enemy.data.damage);
            }
            lastAttackTime = Time.time;
        }
    }
}