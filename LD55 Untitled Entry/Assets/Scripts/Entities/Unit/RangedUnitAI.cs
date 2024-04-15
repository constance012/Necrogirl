using UnityEngine;

public class RangedUnitAI : EntityAI
{
	[Header("Keep Distance Settings"), Space]
	[SerializeField] private float minDistance;

	protected override void Start()
	{
		base.Start();
		_nearbyEntities.Add(this.rb2D);
	}

    protected override void FollowTarget()
    {
        if (target.gameObject.layer == LayerMask.NameToLayer("Player"))
			base.FollowTarget();
		else
		{
			float distance = Vector3.Distance(target.position, transform.position);
			Vector2 direction = (target.position - transform.position).normalized;
			Vector2 velocity = distance < minDistance ? CalculateVelocity(-direction) : CalculateVelocity(direction);
			
			CheckFlip();

			rb2D.velocity = velocity;
		}
    }
}