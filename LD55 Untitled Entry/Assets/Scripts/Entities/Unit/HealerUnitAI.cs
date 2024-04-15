using UnityEngine;

public class HealerUnitAI : EntityAI
{
    protected override void Start()
	{
		base.Start();
		_nearbyEntities.Add(this.rb2D);
	}

    protected override void FollowTarget()
	{
		// Request a path if the target has moved a certain distance fron the last position.
		if (Vector3.Distance(PlayerMovement.Position, _targetPreviousPos) >= maxMovementDelta)
		{
			PathRequester.Request(transform.position, PlayerMovement.Position, OnPathFound);
			_targetPreviousPos = PlayerMovement.Position;
		}
	}
}