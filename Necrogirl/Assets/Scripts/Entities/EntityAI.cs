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

	[Header("Spotting Settings")]
	[SerializeField] protected float aggroRange;
	[SerializeField] protected float spotTimer;
	[SerializeField] protected LayerMask spotLayer;

    // Protected fields.
    protected static HashSet<Rigidbody2D> _nearbyEntities;
	protected Collider2D[] _hitTargets = new Collider2D[5];
	protected ContactFilter2D _contactFilter;
	protected List<EntityStats> _inAggroTargets = new List<EntityStats>();
	protected Vector2 _targetPreviousPos;
    protected bool _facingRight = true;

    protected virtual void Start()
    {
        _nearbyEntities ??= new HashSet<Rigidbody2D>();

		_targetPreviousPos = transform.position;

		_contactFilter.layerMask = spotLayer;
		_contactFilter.useLayerMask = true;
    }

    private void OnDestroy()
	{
		_nearbyEntities.Remove(rb2D);
	}

    protected virtual void FixedUpdate()
	{
		if (PlayerStats.IsDeath)
		 	return;
		
		animator.SetFloat("Speed", rb2D.velocity.sqrMagnitude);
	}

	protected virtual void FollowTarget()
	{
		// Request a path if the target has moved a certain distance fron the last position.
		if (Vector3.Distance(target.position, _targetPreviousPos) >= maxMovementDelta)
		{
			PathRequester.Request(transform.position, target.position, this.gameObject, OnPathFound);
			_targetPreviousPos = target.position;
		}
	}

	protected bool TrySelectTarget()
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
				_inAggroTargets.Sort();
				target = _inAggroTargets[0].transform;
				Debug.Log(target.name);
			}

			return _inAggroTargets.Count > 0;
		} 

		return false;
	}

    protected override IEnumerator ExecuteFoundPath(int previousIndex = -1)
    {
        if (_path.Length == 0)
			yield break;

		Vector2 currentWaypoint = previousIndex == -1 ? _path[0] : _path[previousIndex];

		//Debug.Log($"{gameObject.name} following path...");

		while (true)
		{
			float distanceToCurrent = Vector2.Distance(rb2D.position, currentWaypoint);

			if (distanceToCurrent <= .15f)
			{
				_waypointIndex++;

				// If there's no more waypoints to move, then simply returns out of the coroutine.
				if (_waypointIndex >= _path.Length)
				{
					Debug.Log("No waypoint left.");
					_waypointIndex = 0;
					_path = new Vector3[0];
					rb2D.velocity = Vector2.zero;
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

    protected void CheckFlip()
	{
		float sign = target != null ? Mathf.Sign(target.position.x - rb2D.position.x) :
									Mathf.Sign(PlayerMovement.Position.x - rb2D.position.x);
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
	protected Vector2 CalculateVelocity(Vector2 direction)
	{
		// Enemies will try to avoid each other.
		Vector2 repelForce = Vector2.zero;

		foreach (Rigidbody2D entity in _nearbyEntities)
		{
			if (entity == rb2D)
				continue;

			if (Vector2.Distance(entity.position, rb2D.position) <= repelRange)
			{
				Vector2 repelDirection = (rb2D.position - entity.position).normalized;
				repelForce += repelDirection;
			}
		}

		Vector2 velocity = direction * stats.GetDynamicStat(Stat.MoveSpeed);
		velocity += repelForce.normalized * repelAmplitude;

		return velocity;
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, repelRange);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}