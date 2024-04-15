using UnityEngine;

public class PlayerStats : EntityStats
{
	[Header("Player Stats"), Space]
	[SerializeField, Min(0f)] private float invincibilityTime;

	[Header("Projectile Prefab"), Space]
	[SerializeField] private GameObject projectilePrefab;

	public static bool IsDeath { get; set; }
	public float CurrentMana => _currentMana;
	public bool IsAlive => _currentHealth > 0f;

	// Private fields.
	private float _invincibilityTime;
	private float _currentMana;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetStatic()
	{
		IsDeath = false;
	}

	private void Awake()
	{
		_mat = this.GetComponentInChildren<SpriteRenderer>("Graphic").material;
	}

	protected override void Start()
	{
		base.Start();
		_currentMana = stats.GetDynamicStat(Stat.MaxMana);

		GameManager.Instance.InitializeHealthBar(stats.GetDynamicStat(Stat.MaxHealth));
		SummonManager.Instance.InitializeManaBar(stats.GetDynamicStat(Stat.MaxMana));

		_invincibilityTime = invincibilityTime;
		IsDeath = false;
	}

	private void Update()
	{
		if (_invincibilityTime > 0f)
			_invincibilityTime -= Time.deltaTime;

		Attack();
	}

	private void Attack()
	{
		_attackInterval -= Time.deltaTime;

		if (_attackInterval <= 0f)
		{
			if (InputManager.Instance.GetKeyDown(KeybindingActions.PrimaryAttack))
			{
				Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2 direction = (mousePos - PlayerMovement.Position).normalized;
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

				GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
				projectile.name = projectilePrefab.name;
				projectile.transform.eulerAngles = Vector3.forward * angle;
				projectile.GetComponent<SimpleProjectile>().Initialize(this.stats, null);

				_attackInterval = AttackInterval;
			}
		}
	}

	public override void TakeDamage(float amount, bool weakpointHit, Vector3 attackerPos = default, float knockBackStrength = 0f)
	{
		if (IsAlive && _invincibilityTime <= 0f)
		{
			base.TakeDamage(amount, weakpointHit, attackerPos, knockBackStrength);

			CameraShaker.Instance.ShakeCamera(2.5f, .1f);
			GameManager.Instance.UpdateCurrentHealth(_currentHealth);

			_invincibilityTime = invincibilityTime;
		}
	}

	public override void Heal(float amount)
	{
		if (IsAlive)
		{
			base.Heal(amount);

			GameManager.Instance.UpdateCurrentHealth(_currentHealth);
		}
	}

	public void ConsumeMana(float manaCost)
	{
		if (IsAlive)
		{
			_currentMana = Mathf.Max(_currentMana - manaCost, 0f);

			DamageText.Generate(dmgTextPrefab, dmgTextLoc.position, DamageText.ManaColor, DamageTextStyle.Normal, $"-{manaCost}");
			SummonManager.Instance.UpdateCurrentMana(_currentMana);
		}
	}

	public void RecoverMana(float amount)
	{
		if (IsAlive)
		{
			_currentMana = Mathf.Min(_currentMana + amount, stats.GetDynamicStat(Stat.MaxMana));

			DamageText.Generate(dmgTextPrefab, dmgTextLoc.position, DamageText.ManaColor, DamageTextStyle.Normal, $"+{amount}");
			SummonManager.Instance.UpdateCurrentMana(_currentMana);
		}
	}

	public override void Die()
	{
		if (deathEffect != null)
		{
			IsDeath = true;

			GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
			effect.transform.localScale = transform.localScale;
		}

		GameManager.Instance.ShowGameOverScreen();

		gameObject.SetActive(false);
	}
}
