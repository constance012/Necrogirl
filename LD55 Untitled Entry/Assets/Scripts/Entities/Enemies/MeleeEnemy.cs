using UnityEngine;

public class MeleeEnemy : EnemyStats
{
    private void LateUpdate()
	{
		_attackInterval -= Time.deltaTime;

		if (_attackInterval <= 0f)
		{
			int hitColliders = Physics2D.OverlapBox(transform.position, attackRange, 0f, _contactFilter, _hitObjects);

			for (int i = 0; i < hitColliders; i++)
			{
				EntityStats entity = _hitObjects[i].GetComponentInParent<EntityStats>();

				if (entity != null && entity.GetType() != typeof(EnemyStats))
					entity.TakeDamage(stats.GetDynamicStat(Stat.Damage), false, transform.position, stats.GetStaticStat(Stat.KnockBackStrength));
			}

			_attackInterval = 1f / stats.GetDynamicStat(Stat.AttackSpeed);
		}
	}
}