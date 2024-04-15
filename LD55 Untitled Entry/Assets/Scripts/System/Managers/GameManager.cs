using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	[Header("UI References"), Space]
	[SerializeField] private HealthBar playerHealthBar;
	[SerializeField] private GameObject gameOverScreen;
	[SerializeField] private GameObject victoryScreen;
	[SerializeField] private Transform enemies;

	[Space, SerializeField] private TextMeshProUGUI hpPotionCountText;
	[SerializeField] private TextMeshProUGUI manaPotionCountText;
	[SerializeField] private TextMeshProUGUI coinText;

	// Properties.
	public bool GameFinished { get; private set; }

	// Private fields.
	private Item _healthPotion;
	private Item _manaPotion;
	private Item _coins;

	private void Start()
	{
		hpPotionCountText.text = "0";
		manaPotionCountText.text = "0";
		coinText.text = "0";
	}

	private void Update()
	{
		if (enemies.childCount == 0)
		{
			ShowVictoryScreen();
			return;
		}

		if (InputManager.Instance.GetKeyDown(KeybindingActions.Heal))
			UseItem("health");

		if (InputManager.Instance.GetKeyDown(KeybindingActions.RecoverMana))
			UseItem("mana");

		if (InputManager.Instance.GetKeyDown(KeybindingActions.Pause))
			ReturnToMenu();
	}

	public void AddItem(Item item)
	{
		switch(item.itemName)
		{
			case "Health Potion":
				if (_healthPotion == null)
					_healthPotion = item;
				else
					_healthPotion.quantity += item.quantity;
				
				hpPotionCountText.text = _healthPotion.quantity.ToString();
				break;
			
			case "Mana Potion":
				if (_manaPotion == null)
					_manaPotion = item;
				else
					_manaPotion.quantity += item.quantity;
				
				manaPotionCountText.text = _manaPotion.quantity.ToString();
				break;
			
			case "Coin":
				if (_coins == null)
					_coins = item;
				else
					_coins.quantity += item.quantity;

				coinText.text = _coins.quantity.ToString();
				break;
		}
	}

	public void UpdateCurrentHealth(float currentHP)
	{
		playerHealthBar.SetCurrentHealth(currentHP);
	}

	public void InitializeHealthBar(float initialHP)
	{
		playerHealthBar.SetMaxHealth(initialHP);
	}

	/// <summary>
	/// Callback method for the retry button.
	/// </summary>
	public void RestartGame()
	{
		GameFinished = false;

		SceneManager.LoadSceneAsync("Scenes/Main Game");
	}

	/// <summary>
	/// Callback method for the return to menu button.
	/// </summary>
	public void ReturnToMenu()
	{
		SceneManager.LoadSceneAsync("Scenes/Menu");
	}

	// Call back method for the item buttons.
	public void UseItem(string name)
	{
		switch(name.ToLower())
		{
			case "health":
				if (_healthPotion != null && _healthPotion.Use())
					hpPotionCountText.text = _healthPotion.quantity.ToString();
				break;

			case "mana":
				if (_manaPotion != null && _manaPotion.Use())
					manaPotionCountText.text = _manaPotion?.quantity.ToString();
				break;
		}
	}

	public void ShowGameOverScreen()
	{
		GameFinished = true;

		gameOverScreen.SetActive(true);
		victoryScreen.SetActive(false);
	}

	public void ShowVictoryScreen()
	{
		GameFinished = true;

		gameOverScreen.SetActive(false);
		victoryScreen.SetActive(true);
	}
}