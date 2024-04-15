using UnityEngine;

public class MeleeUnit : UnitStats
{
    private void LateUpdate()
	{
		_attackInterval -= Time.deltaTime;

		if (_attackInterval <= 0f)
		{
			int hitColliders = Physics2D.OverlapBox(transform.position, attackRange, 0f, _contactFilter, _hitObjects);

			for (int i = 0; i < hitColliders; i++)
			{
				EnemyStats enemy = _hitObjects[i].GetComponentInParent<EnemyStats>();

				if (enemy != null)
					enemy.TakeDamage(stats.GetDynamicStat(Stat.Damage), false, transform.position, stats.GetStaticStat(Stat.KnockBackStrength));
			}

			_attackInterval = 1f / stats.GetDynamicStat(Stat.AttackSpeed);
		}
	}
}