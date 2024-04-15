using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	[Header("References"), Space]
	[SerializeField] private HealthBar playerHealthBar;

	public bool GameFinished { get; private set; }

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

	public void ShowGameOverScreen()
	{

	}

	public void ShowVictoryScreen()
	{
		
	}
}