using UnityEngine;

public class MeleeUnitAI : UnitAI
{
    protected override void FixedUpdate()
    {
        if (PlayerStats.IsDeath)
		 	return;

        base.FixedUpdate();

		if (TrySelectTarget() && target != null)
			RequestNewPath(target.position);
    }
}