using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(menuName = "Unit Stats", fileName = "New Blank Stats")]
public class Stats : ScriptableObject
{
	public SerializedDictionary<Stat, float> staticStats = new SerializedDictionary<Stat, float>();
	public SerializedDictionary<Stat, float> dynamicStats = new SerializedDictionary<Stat, float>();

	public float GetStaticStat(Stat statName)
	{
		if (staticStats.TryGetValue(statName, out float value))
			return value;
		else
		{
			Debug.LogError($"No STATIC stat value found for {statName} on {this.name}");
			return 0f;
		}
	}
	
	public float GetDynamicStat(Stat statName)
	{
		if (dynamicStats.TryGetValue(statName, out float value))
			return value;
		else
		{
			Debug.LogError($"No DYNAMIC stat value found for {statName} on {this.name}");
			return 0f;
		}
	}

	public void ModifyStat(Stat statName, float delta)
	{
		if (dynamicStats.TryGetValue(statName, out float _))
		{
			dynamicStats[statName] += delta;
		}
		else
		{
			Debug.LogError($"No DYNAMIC stat value found for {statName} on {this.name}");
		}
	}
}

public enum Stat
{
	// Dynamic.
	MaxHealth,
	MaxMana,
	Damage,
	AttackSpeed,
	MoveSpeed,

	// Static.
	KnockBackStrength,
	KnockBackRes,
	ManaCost,
	LifeStealRatio
}