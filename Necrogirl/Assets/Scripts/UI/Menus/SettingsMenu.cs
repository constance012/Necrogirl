using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
	[Header("Audio Mixer"), Space]
	[SerializeField] private AudioMixer mixer;
	[SerializeField] private bool closeOnStart;

	[Header("UI References"), Space]
	[SerializeField] private Slider _masterSlider;
	[SerializeField] private Slider _musicSlider;
	[SerializeField] private Slider _soundsSlider;

	// Private fields
	private TextMeshProUGUI _musicText;
	private TextMeshProUGUI _soundsText;
	private TextMeshProUGUI _masterText;

	private void Awake()
	{
		_musicText = _musicSlider.GetComponentInChildren<TextMeshProUGUI>();
		_soundsText = _soundsSlider.GetComponentInChildren<TextMeshProUGUI>();
		_masterText = _masterSlider.GetComponentInChildren<TextMeshProUGUI>();
	}

	private void Start()
	{
		if (closeOnStart)
		{
			ReloadUI();
			gameObject.SetActive(false);
		}
	}

	#region Callback Method for UI.
	public void SetMasterVolume(float amount)
	{
		float volume = Mathf.Log10(amount) * 20f;
		mixer.SetFloat("masterVol", volume);

		_masterText.text = $"Master: {ConvertDecibelToText(amount)}";
		UserSettings.MasterVolume = amount;
	}

	public void SetMusicVolume(float amount)
	{
		float volume = Mathf.Log10(amount) * 20f;
		mixer.SetFloat("musicVol", volume);

		_musicText.text = $"Music: {ConvertDecibelToText(amount)}";
		UserSettings.MusicVolume = amount;
	}

	public void SetSoundsVolume(float amount)
	{
		float volume = Mathf.Log10(amount) * 20f;
		mixer.SetFloat("soundsVol", volume);

		_soundsText.text = $"Sound: {ConvertDecibelToText(amount)}";
		UserSettings.SoundsVolume = amount;
	}

	public void SetQualityLevel(int index)
	{
		QualitySettings.SetQualityLevel(index);
		UserSettings.QualityLevel = index;
	}

	public void ResetToDefault()
	{
		UserSettings.ResetToDefault(UserSettings.SettingSection.All);
		ReloadUI();
	}
	#endregion

	private string ConvertDecibelToText(float amount)
	{
		return (amount * 100f).ToString("0");
	}

	public void ReloadUI()
	{
		float masterVol = UserSettings.MasterVolume;
		float musicVol = UserSettings.MusicVolume;
		float soundsVol = UserSettings.SoundsVolume;

		_masterSlider.value = masterVol;
		_musicSlider.value = musicVol;
		_soundsSlider.value = soundsVol;

		_masterText.text = $"Master: {ConvertDecibelToText(masterVol)}";
		_musicText.text = $"Music: {ConvertDecibelToText(musicVol)}";
		_soundsText.text = $"Sound: {ConvertDecibelToText(soundsVol)}";
	}
}
