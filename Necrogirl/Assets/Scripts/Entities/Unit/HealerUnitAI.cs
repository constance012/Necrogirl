using UnityEngine;

public class HealerUnitAI : UnitAI
{
    protected override void FixedUpdate()
    {
        if (PlayerStats.IsDeath)
		 	return;
        
        base.FixedUpdate();

		RequestNewPath(PlayerMovement.Position);
	}
}