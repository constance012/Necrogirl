using UnityEngine;

public class MeleeUnit : UnitStats
{
	[Header("Animator"), Space]
	[SerializeField] private Animator animator;

    private void Update()
	{
		_attackInterval -= Time.deltaTime;

		if (_attackInterval <= 0f)
		{
			int hitColliders = Physics2D.OverlapBox(transform.position, attackRange, 0f, _contactFilter, _hitObjects);

			if (hitColliders > 0)
			{
				animator.Play("Slash");
				for (int i = 0; i < hitColliders; i++)
				{
					EnemyStats enemy = _hitObjects[i].GetComponentInParent<EnemyStats>();

					if (enemy != null)
						enemy.TakeDamage(stats.GetDynamicStat(Stat.Damage), false, transform.position, stats.GetStaticStat(Stat.KnockBackStrength));
				}

				_attackInterval = AttackInterval;
			}
		}
	}
}