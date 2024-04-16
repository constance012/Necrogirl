using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private AudioMixer mixer;

	private static bool _hasStartup;

	private void Start()
	{
		#if UNITY_EDITOR
		_hasStartup = false;
		#endif

		if (!_hasStartup)
		{
			InternalInitialization();
			_hasStartup = true;
		}
	}

	public void StartGame()
	{
		SceneManager.LoadSceneAsync("Scenes/Main Game");
		AudioManager.Instance.Play("Ambience Noise");
		//GameDataManager.Instance.LoadGame(false);
	}

	public void QuitGame()
	{
		Debug.Log("Quiting player...");
		Application.Quit();
	}

	private void InternalInitialization()
	{
		Debug.Log("Initializing settings internally...");

		mixer.SetFloat("masterVol", Mathf.Log10(UserSettings.MasterVolume) * 20f);
		mixer.SetFloat("musicVol", Mathf.Log10(UserSettings.MusicVolume) * 20f);
		mixer.SetFloat("soundsVol", Mathf.Log10(UserSettings.SoundsVolume) * 20f);
	}
}
