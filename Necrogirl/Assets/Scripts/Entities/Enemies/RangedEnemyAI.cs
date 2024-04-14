using UnityEngine;

public class RangedEnemyAI : MeleeEnemyAI
{
	[Header("Keep Distance Settings"), Space]
	[SerializeField] private float minDistance;

	protected override void FollowTarget()
	{
		float distance = Vector3.Distance(target.position, transform.position);
		Vector2 direction = (target.position - transform.position).normalized;
		Vector2 velocity = distance < minDistance ? CalculateVelocity(-direction) : CalculateVelocity(direction);
			
		CheckFlip();

		rb2D.velocity = velocity;
	}
}