using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumable : Item
{
	public enum HealingType
	{
		Health,
		Mana
	}

	[Header("Healing amount"), Space]
	public HealingType healingType;
	public int healingAmount;

	public override bool Use()
	{
		Debug.Log("Using " + itemName);

		if (quantity > 0)
		{
			PlayerStats player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

			if (healingType == HealingType.Health)
				player.Heal(healingAmount);
			else
				player.RecoverMana(healingAmount);

			quantity--;
			return true;
		}

		return false;
	}

	public override string ToString()
	{
		return base.ToString() + "\n" +
				$"<b> +{healingAmount} HP. </b>\n" +
				$"<b> Right Click to use. </b>";
	}
}
