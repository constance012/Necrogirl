using UnityEngine;

public class MeleeEnemyAI : EntityAI
{
	// Private fields.
	private bool _spottedPlayer;
	private float _spotTimer;

	protected override void Start()
	{
		base.Start();
		_spotTimer = spotTimer;
	}

    protected override void FixedUpdate()
    {
		if (PlayerStats.IsDeath)
			return;

		base.FixedUpdate();

		if (SpotTarget())
			ChaseTarget();
    }

	public void Alert()
	{
		_spottedPlayer = true;
		_spotTimer = 0f;

		_nearbyEntities.Add(rb2D);
	}

	protected virtual void ChaseTarget()
	{	
		if (TrySelectTarget() && target != null)
			RequestNewPath(target.position);
	}

    protected override bool TrySelectTarget()
    {
		int hitColliders = Physics2D.OverlapCircle(transform.position, aggroRange, _contactFilter, _hitTargets);

		if (hitColliders > 0)
		{
			_inAggroTargets.Clear();
			for (int i = 0; i < hitColliders; i++)
			{
				EntityStats entity = _hitTargets[i].GetComponentInParent<EntityStats>();

				if (entity != null)
					_inAggroTargets.Add(entity);
			}
			
			// Sort by priority if the list is not empty.
			if (_inAggroTargets.Count > 0)
			{
				// Only assign new target if it's different from the previous one or the previous is null.
				if (target == null || target != _inAggroTargets[0].transform)
				{
					_inAggroTargets.Sort();
					target = _inAggroTargets[0].transform;
					Debug.Log(target.name);
				}
			}

			return _inAggroTargets.Count > 0;
		} 

		return false;
    }

    private bool SpotTarget()
    {
        if (!_spottedPlayer)
		{
			float distanceToPlayer = Vector2.Distance(transform.position, PlayerMovement.Position);
			
			if (distanceToPlayer <= aggroRange)
			{
				_spotTimer -= Time.deltaTime;

				if (_spotTimer <= 0)
					Alert();
			}
			else
				_spotTimer = spotTimer;
		}
		
		return _spottedPlayer;
	}
}
