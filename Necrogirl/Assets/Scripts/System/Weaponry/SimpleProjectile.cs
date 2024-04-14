using UnityEngine;

public sealed class SimpleProjectile : ProjectileBase
{
	private void OnCollisionEnter2D(Collision2D other)
	{
		flySpeed = 0f;

		ProcessCollision(other);

		Destroy(gameObject);
	}

	public override void ProcessCollision(Collision2D other)
	{
		EntityStats target = other.collider.GetComponentInParent<EntityStats>();

		if (target != null)
			target.TakeDamage(_wearerStats.GetDynamicStat(Stat.Damage), false, transform.position, _wearerStats.GetStaticStat(Stat.KnockBackStrength));
	}
}
