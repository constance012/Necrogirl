using UnityEngine;

public class MeleeUnitAI : EntityAI
{
	protected override void Start()
	{
		base.Start();
		_nearbyEntities.Add(this.rb2D);
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

		if (TrySelectTarget())
			FollowTarget();
    }
}