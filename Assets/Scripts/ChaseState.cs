using UnityEngine;

public class ChaseState : BaseState
{
    public override void EnterState(Enemy enemy) { }

    public override void UpdateState(Enemy enemy)
    {
        Transform player = enemy.GetPlayer();
        if (player == null) return;

        Vector2 direction = (player.position - enemy.transform.position).normalized;
        enemy.transform.Translate(direction * enemy.data.speed * Time.deltaTime);
    }
}