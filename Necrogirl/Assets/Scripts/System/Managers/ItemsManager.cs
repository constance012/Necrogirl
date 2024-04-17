using UnityEngine;
using TMPro;

public class ItemsManager : Singleton<ItemsManager>
{
    [Header("References"), Space]
    [Space, SerializeField] private TextMeshProUGUI hpPotionCountText;
	[SerializeField] private TextMeshProUGUI coinText;

	// Properties.
	public int Coins
	{
		get
		{
			if (_coins != null)
				return _coins.quantity;
			return 0;
		}
	}

    // Private fields.
	private Item _healthPotion;
	private Item _coins;

	private void Start()
	{
		hpPotionCountText.text = "0";
		coinText.text = "0";
	}

    private void Update()
    {
        if (InputManager.Instance.GetKeyDown(KeybindingActions.Heal))
			UseItem("health");
    }

    public bool AddItem(Item item, bool isForcedPickup = false)
	{
		if (item.autoUse)
			return item.Use(isForcedPickup);
		
		switch(item.itemName)
		{
			case "Health Potion":
				bool success = false;
				if (_healthPotion == null)
				{
					_healthPotion = item;
					success = true;
				}
				else if (_healthPotion.UpdateQuantity(item.quantity))
					success = true;
				
				hpPotionCountText.text = _healthPotion.quantity.ToString();
				return success;			
			
			case "Coin":
				if (_coins == null)
					_coins = item;
				else
					_coins.UpdateQuantity(item.quantity);

				coinText.text = _coins.quantity.ToString();
				return true;

			default:
				return false;
		}
	}

    // Call back method for the item buttons.
	public void UseItem(string name)
	{
		Debug.Log($"Using {name} potion...");

		switch(name.ToLower())
		{
			case "health":
				if (_healthPotion != null && _healthPotion.Use())
					hpPotionCountText.text = _healthPotion.quantity.ToString();
				break;
		}
	}
}