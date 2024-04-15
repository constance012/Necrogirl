using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAI : Seeker
{
    [Header("References"), Space]
    [SerializeField] protected Rigidbody2D rb2D;
	[SerializeField] protected Animator animator;
	[SerializeField] protected Stats stats;

    [Header("Mobility Settings"), Space]
	[SerializeField] private float repelRange;
	[SerializeField] private float repelAmplitude;

    // Protected fields.
    protected static HashSet<Rigidbody2D> _nearbyEntity;
	protected Vector2 _targetPreviousPos;
    protected bool _facingRight = true;

    protected virtual void Start()
    {
        _nearbyEntity ??= new HashSet<Rigidbody2D>();
		_targetPreviousPos = PlayerMovement.Position;
    }

    private void OnDestroy()
	{
		_nearbyEntity.Remove(rb2D);
	}

    protected virtual void FixedUpdate()
	{
		if (PlayerStats.IsDeath)
		 	return;
		
		animator.SetFloat("Speed", rb2D.velocity.sqrMagnitude);

		// Request a path if the player has moved a certain distance fron the last position.
		if ((PlayerMovement.Position - _targetPreviousPos).sqrMagnitude >= maxMovementDeltaSqr)
		{
			PathRequester.Request(transform.position, PlayerMovement.Position, OnPathFound);
			_targetPreviousPos = PlayerMovement.Position;
		}
	}

    protected override IEnumerator FollowPath(int previousIndex = -1)
    {
        if (_path.Length == 0)
			yield break;

		Vector2 currentWaypoint = previousIndex == -1 ? _path[0] : _path[previousIndex];

		Debug.Log($"{gameObject.name} following path...");

		while (true)
		{
			float distanceToCurrent = Vector2.Distance(rb2D.position, currentWaypoint);

			if (distanceToCurrent < .05f)
			{
				_waypointIndex++;

				// If there's no more waypoints to move, then simply returns out of the coroutine.
				if (_waypointIndex >= _path.Length)
				{
					_waypointIndex = 0;
					_path = new Vector3[0];
					yield break;
				}

				currentWaypoint = _path[_waypointIndex];
			}

			Vector2 direction = (currentWaypoint - rb2D.position).normalized;
			Vector2 velocity = CalculateVelocity(direction);

			CheckFlip();

			rb2D.velocity = velocity;

			yield return new WaitForFixedUpdate();
		}
    }

    private void CheckFlip()
	{
		float sign = Mathf.Sign(PlayerMovement.Position.x - rb2D.position.x);
		bool mustFlip = (_facingRight && sign < 0f) || (!_facingRight && sign > 0f);

		if (mustFlip)
		{
			transform.Rotate(Vector3.up * 180f);
			_facingRight = !_facingRight;
		}
	}

	/// <summary>
	/// Calculate the final velocity after applying repel force against other enemies nearby.
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	private Vector2 CalculateVelocity(Vector2 direction)
	{
		// Enemies will try to avoid each other.
		Vector2 repelForce = Vector2.zero;

		foreach (Rigidbody2D enemy in _nearbyEntity)
		{
			if (enemy == rb2D)
				continue;

			if (Vector2.Distance(enemy.position, rb2D.position) <= repelRange)
			{
				Vector2 repelDirection = (rb2D.position - enemy.position).normalized;
				repelForce += repelDirection;
			}
		}

		Vector2 velocity = direction * stats.GetDynamicStat(Stat.MoveSpeed);
		velocity += repelForce.normalized * repelAmplitude;

		return velocity;
	}

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, repelRange);
    }
}