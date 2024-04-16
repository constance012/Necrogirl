using System.Collections.Generic;
using UnityEngine;

public class SummonManager : Singleton<SummonManager>
{
	[Header("References"), Space]
	[SerializeField] private PlayerStats player;
	[SerializeField] private HealthBar playerManaBar;
	[SerializeField] private List<UnitButton> unitButtons;
	[SerializeField] private List<GameObject> unitPrefabs;
	[SerializeField] private List<Stats> unitStats;
	[SerializeField] private Transform container;

	[Header("Summon Settings"), Space]
	[SerializeField, Min(0f)] private float cooldown;
	[SerializeField] private Vector2 summonRange;

	// Private fields.
	private float _cooldown = 0f;
	private readonly KeyCode[] summonKeys = new KeyCode[]
	{
		KeyCode.Alpha1,
		KeyCode.Alpha2,
		KeyCode.Alpha3,
		KeyCode.Alpha4,
	};

	private void Update()
	{
		if (GameManager.Instance.GameFinished)
			return;

		_cooldown -= Time.deltaTime;

		if (Input.anyKeyDown && _cooldown <= 0f)
		{
			for (int i = 0; i < summonKeys.Length; i++)
			{
				if (Input.GetKeyDown(summonKeys[i]))
				{
					SummonUnit(i);
					break;
				}
			}
		}
	}

	public void SummonUnit(int index)
	{
		float manaCost = unitStats[index].GetStaticStat(Stat.ManaCost);
		Debug.Log(manaCost);

		if (player.CurrentMana >= manaCost)
		{
			float x = Random.Range(PlayerMovement.Position.x - summonRange.x, PlayerMovement.Position.x + summonRange.x);
			float y = Random.Range(PlayerMovement.Position.y - summonRange.y, PlayerMovement.Position.y + summonRange.y);

			GameObject unit = Instantiate(unitPrefabs[index], new Vector2(x, y), Quaternion.identity);
			unit.name = unitPrefabs[index].name;
			unit.transform.SetParent(container);

			player.ConsumeMana(manaCost);

			_cooldown = cooldown;
		}
	}

	// Callback method for summoning UI buttons.
	public void SummonUnit(UnitButton button)
	{
		int index = button.transform.GetSiblingIndex();
		float manaCost = unitStats[index].GetStaticStat(Stat.ManaCost);

		if (player.CurrentMana >= manaCost)
		{
			float x = Random.Range(PlayerMovement.Position.x - summonRange.x, PlayerMovement.Position.x + summonRange.x);
			float y = Random.Range(PlayerMovement.Position.y - summonRange.y, PlayerMovement.Position.y + summonRange.y);

			GameObject unit = Instantiate(unitPrefabs[index], new Vector2(x, y), Quaternion.identity);
			unit.name = unitPrefabs[index].name;
			unit.transform.SetParent(container);

			player.ConsumeMana(manaCost);
			
			_cooldown = cooldown;
		}
	}

	public void UpdateCurrentMana(float currentMana)
	{
		playerManaBar.SetCurrentHealth(currentMana);
		unitButtons.ForEach(button => button.ValidateManaPoint(currentMana));
	}

	public void InitializeManaBar(float initialMana)
	{
		playerManaBar.SetMaxHealth(initialMana);
		unitButtons.ForEach(button => button.ValidateManaPoint(initialMana));
	}
}