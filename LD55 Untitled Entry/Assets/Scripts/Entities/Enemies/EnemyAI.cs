using UnityEngine;

public class EnemyAI : EntityAI
{
	[Header("Spotting Settings")]
	[SerializeField] private float aggroRange;
	[SerializeField] private float spotTimer;

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

		SpotPlayer();
        
    }

    private void SpotPlayer()
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

		// Request a path if the player has moved a certain distance fron the last position.
		if ((PlayerMovement.Position - _targetPreviousPos).sqrMagnitude >= maxMovementDeltaSqr)
		{
			PathRequester.Request(transform.position, PlayerMovement.Position, OnPathFound);
			_targetPreviousPos = PlayerMovement.Position;
		}
    }

	public void Alert()
	{
		_spottedPlayer = true;
		_spotTimer = 0f;

		// Add this enemy to the hash set.
		_nearbyEntity.Add(rb2D);
	}

	protected override void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, aggroRange);
	}
}
