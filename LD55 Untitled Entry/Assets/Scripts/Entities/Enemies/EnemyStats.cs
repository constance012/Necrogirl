using UnityEngine;

public class EnemyStats : EntityStats
{
	[Header("Enemy Stats"), Space]
	[Space, SerializeField] protected Vector2 attackRange;
	[SerializeField] protected LayerMask hitLayer;

	[Header("Health Bar"), Space]
	[SerializeField] protected WorldHealthBar healthBar;

	// Protected fields.
	protected Collider2D[] _hitObjects = new Collider2D[2];
	protected ContactFilter2D _contactFilter;

	private void Awake()
	{
		_mat = this.GetComponentInChildren<SpriteRenderer>("Graphic").material;
	}

	protected override void Start()
	{
		base.Start();

		_contactFilter.layerMask = hitLayer;
		_contactFilter.useLayerMask = true;

		healthBar.SetMaxHealth(stats.GetDynamicStat(Stat.MaxHealth));
		healthBar.name = $"{gameObject.name} Health Bar";
	}

	public override void TakeDamage(float amount, bool weakpointHit, Vector3 attackerPos = default, float knockBackStrength = 0)
	{
		(brain as MeleeEnemyAI).Alert();

		base.TakeDamage(amount, weakpointHit, attackerPos, knockBackStrength);

		healthBar.SetCurrentHealth(_currentHealth);
	}

    public override void Heal(float amount)
    {
        base.Heal(amount);

		healthBar.SetCurrentHealth(_currentHealth);
    }

    public override void Die()
	{
		Destroy(healthBar.gameObject);

		base.Die();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(transform.position, attackRange);
	}
}
