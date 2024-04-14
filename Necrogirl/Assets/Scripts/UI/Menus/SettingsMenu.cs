using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
	[Header("Audio Mixer"), Space]
	[SerializeField] private AudioMixer mixer;

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
		ReloadUI();
	}

	#region Callback Method for UI.
	public void SetMasterVolume(float amount)
	{
		mixer.SetFloat("masterVol", amount);

		_masterText.text = $"Master: {ConvertDecibelToText(amount)}";
		UserSettings.MasterVolume = amount;
	}

	public void SetMusicVolume(float amount)
	{
		mixer.SetFloat("musicVol", amount);

		_musicText.text = $"Music: {ConvertDecibelToText(amount)}";
		UserSettings.MusicVolume = amount;
	}

	public void SetSoundsVolume(float amount)
	{
		mixer.SetFloat("soundsVol", amount);

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
		float normalized = 1f - (Mathf.Abs(amount) / 80f);
		return (normalized * 100f).ToString("0");
	}

	private void ReloadUI()
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
