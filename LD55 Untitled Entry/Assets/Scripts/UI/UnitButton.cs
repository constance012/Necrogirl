using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour
{
	[Header("References"), Space]
	[SerializeField] private Stats unitStats;
	[SerializeField] private TextMeshProUGUI manaCostText;
	[SerializeField] private CanvasGroup canvasGroup;
	[SerializeField] private Button button;

	private void Start()
	{
		manaCostText.text = unitStats.GetStaticStat(Stat.ManaCost).ToString();
	}

	public void ValidateManaPoint(float currentMana)
	{
		canvasGroup.alpha = currentMana < unitStats.GetStaticStat(Stat.ManaCost) ? .5f : 1f;
		button.interactable = currentMana >= unitStats.GetStaticStat(Stat.ManaCost);
	}
}