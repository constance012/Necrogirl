using UnityEngine;

public class RangedEnemyAI : MeleeEnemyAI
{
	[Header("Keep Distance Settings"), Space]
	[SerializeField] private float retreatMinDistance;

	// Protected fields.
	protected EnemyStats _enemyStats;

    protected override void Start()
    {
        base.Start();
		_enemyStats = heart as EnemyStats;
    }

    protected override void ChaseTarget()
	{
		if (TrySelectTarget() && target != null)
			ProcessTarget();
	}

	private void ProcessTarget()
	{
		float distance = Vector3.Distance(target.position, transform.position);
		_forcedStopMoving = distance <= _enemyStats.RangedAttackRadius;

		if (distance <= retreatMinDistance)
		{
			Vector2 direction = (transform.position - target.position).normalized;
			Vector2 velocity = CalculateVelocity(direction);
			
			CheckFlip();

			rb2D.velocity = velocity;
		}
		else if (distance >= _enemyStats.RangedAttackRadius)
		{
			_forcedStopMoving = false;
			RequestNewPath(target.position);
		}
	}

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, retreatMinDistance);
    }
}