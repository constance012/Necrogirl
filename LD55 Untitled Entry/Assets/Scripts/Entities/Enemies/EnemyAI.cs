using UnityEngine;

public class EnemyAI : EntityAI
{
	[Header("Spotting Settings")]
	[SerializeField] private float aggroRange;
	[SerializeField] private float spotTimer;

	// Private fields.
	private Vector2 _targetPreviousPos;
	private bool _spottedPlayer;
	private float _spotTimer;

	protected override void Start()
	{
		base.Start();

		_targetPreviousPos = PlayerMovement.Position;
		_spotTimer = spotTimer;
	}

    protected override void FixedUpdate()
    {
		SpotPlayer();
        base.FixedUpdate();
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
