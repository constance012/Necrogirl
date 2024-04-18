using System;
using System.Collections;
using UnityEngine;

public abstract class EntityStats : MonoBehaviour, IComparable<EntityStats>
{
	[Header("Component References"), Space]
	[SerializeField] protected Rigidbody2D rb2D;
	[SerializeField] protected MonoBehaviour brain;

	[Header("Damage Text"), Space]
	[SerializeField] protected GameObject dmgTextPrefab;
	[SerializeField] protected Transform dmgTextLoc;

	[Header("Base Stats"), Space]
	[SerializeField] protected Stats stats;
	[SerializeField] protected float damageFlashTime;
	[SerializeField] protected int priority;

	[Header("Effects"), Space]
	public GameObject deathEffect;

	// Properties.
	protected float BaseAttackInterval => 1f / stats.GetDynamicStat(Stat.AttackSpeed);
	public float CurrentHealthNormalized => _currentHealth / stats.GetDynamicStat(Stat.MaxHealth);

	// Protected fields.
	protected Material _mat;
	protected Coroutine _attackCoroutine;
	protected float _currentHealth;
	protected float _attackInterval;

	protected virtual void Start()
	{
		_currentHealth = stats.GetDynamicStat(Stat.MaxHealth);
	}

	protected abstract void TryAttack();

	protected abstract IEnumerator DoAttack();

	public virtual void TakeDamage(float amount, bool weakpointHit, Vector3 attackerPos = default, float knockBackStrength = 0f)
	{
		AudioManager.Instance.PlayWithRandomPitch("Taking Damage", .7f, 1.2f);
		
		_currentHealth -= amount;
		_currentHealth = Mathf.Max(_currentHealth, 0f);

		DamageTextStyle style = weakpointHit ? DamageTextStyle.Critical : DamageTextStyle.Normal;
		DamageText.Generate(dmgTextPrefab, dmgTextLoc.position, style, amount.ToString("0"));

		StartCoroutine(TriggerDamageFlash());
		StartCoroutine(BeingKnockedBack(attackerPos, knockBackStrength));

		if (_currentHealth <= 0)
			Die();
	}

	public virtual void Heal(float amount)
	{
		_currentHealth += amount;
		_currentHealth = Mathf.Min(_currentHealth, stats.GetDynamicStat(Stat.MaxHealth));

		DamageText.Generate(dmgTextPrefab, dmgTextLoc.position, DamageText.HealingColor, DamageTextStyle.Normal, amount.ToString("0"));
	}

	public virtual void Die()
	{
		if (deathEffect != null)
		{
			GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
			effect.transform.localScale = transform.localScale;
			
			// Destroy effect here.
		}

		Destroy(gameObject);
	}

	protected IEnumerator TriggerDamageFlash()
	{
		float flashIntensity;
		float elapsedTime = 0f;

		while (elapsedTime < damageFlashTime)
		{
			elapsedTime += Time.deltaTime;

			flashIntensity = Mathf.Lerp(1f, 0f, elapsedTime / damageFlashTime);
			_mat.SetFloat("_FlashIntensity", flashIntensity);

			yield return null;
		}
	}

	protected IEnumerator BeingKnockedBack(Vector3 attackerPos, float strength)
	{
		if (attackerPos == default)
			yield break;

		rb2D.velocity = Vector3.zero;

		brain.enabled = false;
		brain.StopAllCoroutines();

		Vector2 direction = transform.position - attackerPos;
		float knockBackStrength = strength * (1f - stats.GetStaticStat(Stat.KnockBackRes));

		Vector2 force = direction.normalized * knockBackStrength;

		rb2D.AddForce(force, ForceMode2D.Impulse);

		yield return new WaitForSeconds(.3f);

		brain.enabled = true;
	}

    public int CompareTo(EntityStats other)
    {
		// Priortize the player if there's no other unit in sight.
		if (this.priority == 0 && other.priority != 0)
			return 1;
		else if (this.priority != 0 && other.priority == 0)
			return -1;

		// Else, priortize the unit with the highest priority value.
		if (this.priority != other.priority)
			return other.priority - this.priority;
		       
		// Lastly, priortize the unit with the lower health.
		return this.CurrentHealthNormalized.CompareTo(other.CurrentHealthNormalized);
    }
}
