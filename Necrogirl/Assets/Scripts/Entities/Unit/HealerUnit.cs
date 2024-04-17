using UnityEngine;

public class HealerUnit : UnitStats
{
    protected override void Start()
    {
        base.Start();

		_hitObjects = new Collider2D[7];
    }

    private void Update()
	{
		_attackInterval -= Time.deltaTime;

		if (_attackInterval <= 0f)
		{
			int hitColliders = Physics2D.OverlapCircle(transform.position, attackRange.x, _contactFilter, _hitObjects);

			if (hitColliders > 0)
			{
				for (int i = 0; i < hitColliders; i++)
				{
					EntityStats entity = _hitObjects[i].GetComponentInParent<EntityStats>();

					if (entity != null)
						entity.Heal(stats.GetDynamicStat(Stat.Damage));
				}

				_attackInterval = AttackInterval;
			}
		}
	}
}