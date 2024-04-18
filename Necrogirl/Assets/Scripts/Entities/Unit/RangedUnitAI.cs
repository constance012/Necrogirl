using UnityEngine;

public class RangedUnitAI : UnitAI
{
	[Header("Keep Distance Settings"), Space]
	[SerializeField] private float retreatMinDistance;

	// Protected fields.
	protected UnitStats _unitStats;

    protected override void Start()
    {
        base.Start();
		_unitStats = heart as UnitStats;
    }

    protected override void FixedUpdate()
    {
		if (PlayerStats.IsDeath)
		 	return;

        base.FixedUpdate();

		if (TrySelectTarget() && target != null)
			ProcessTarget();
    }

    private void ProcessTarget()
    {
        if (target == _player)
		{
			_forcedStopMoving = false;
			RequestNewPath(PlayerMovement.Position);
		}
		else
		{
			float distance = Vector3.Distance(target.position, transform.position);
			_forcedStopMoving = distance <= _unitStats.RangedAttackRadius;
			
			if (distance <= retreatMinDistance)
			{
				Vector2 direction = (transform.position - target.position).normalized;
				Vector2 velocity = CalculateVelocity(direction);
				
				CheckFlip();

				rb2D.velocity = velocity;
			}
			else if (distance > _unitStats.RangedAttackRadius)
			{
				_forcedStopMoving = false;
				RequestNewPath(target.position);
			}
		}
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(transform.position, retreatMinDistance);
    }
}