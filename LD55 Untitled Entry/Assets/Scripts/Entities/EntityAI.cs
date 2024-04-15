using System.Collections.Generic;
using UnityEngine;

public class EntityAI : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] protected Rigidbody2D rb2D;
	[SerializeField] protected Animator animator;

    [Header("Mobility Settings"), Space]
    [SerializeField] protected float moveSpeed;
	[SerializeField] private float repelRange;
	[SerializeField] private float repelAmplitude;

    // Protected fields.
    protected static HashSet<Rigidbody2D> _nearbyEntity;
    protected bool _facingRight = true;

    protected virtual void Start()
    {
        _nearbyEntity ??= new HashSet<Rigidbody2D>();
    }

    private void OnDestroy()
	{
		_nearbyEntity.Remove(rb2D);
	}

    protected virtual void FixedUpdate()
	{
		if (PlayerStats.IsDeath)
		 	return;
		
		Vector2 direction = PlayerMovement.Position - rb2D.position;
		animator.SetFloat("Speed", rb2D.velocity.sqrMagnitude);

		if (direction.sqrMagnitude >= 1f)
		{
			Vector2 velocity = CalculateVelocity(direction);

			CheckFlip();

			rb2D.velocity = velocity;
		}
		else
			rb2D.velocity = Vector3.zero;
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

		Vector2 velocity = direction.normalized * moveSpeed;
		velocity += repelForce.normalized * repelAmplitude;

		return velocity;
	}

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, repelRange);
    }
}