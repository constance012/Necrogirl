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

		SpotTarget();
    }

    private void SpotTarget()
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

			return;
		}

		animator.SetFloat("Speed", rb2D.velocity.sqrMagnitude);
		SelectTarget();
		FollowTarget();
	}

	public void Alert()
	{
		_spottedPlayer = true;
		_spotTimer = 0f;

		// Add this enemy to the hash set.
		_nearbyEntities.Add(rb2D);
	}
}
