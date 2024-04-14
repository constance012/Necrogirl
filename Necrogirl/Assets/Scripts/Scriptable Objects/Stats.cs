using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Unit Stats", fileName = "New Blank Stats")]
public class Stats : ScriptableObject
{
	public SerializedDictionary<Stat, float> staticStats = new SerializedDictionary<Stat, float>();
	public SerializedDictionary<Stat, float> dynamicStats = new SerializedDictionary<Stat, float>();

	// Private fields.
	private List<StatsUpgrade> appliedUpgrades = new List<StatsUpgrade>();

	public void AddUpgrade(StatsUpgrade upgrade)
	{
		if (!appliedUpgrades.Contains(upgrade))
			appliedUpgrades.Add(upgrade);
	}

	public void ClearUpgrades()
	{
		appliedUpgrades.Clear();
	}

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
		if (dynamicStats.TryGetValue(statName, out float baseValue))
			return GetUpgradedValue(statName, baseValue);
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

	private float GetUpgradedValue(Stat stat, float baseValue)
	{
		foreach (StatsUpgrade upgrade in appliedUpgrades)
		{
			if (!upgrade.affectedStats.TryGetValue(stat, out float upgradeValue))
				continue;
			
			if (upgrade.isPercentageUpgrade)
				baseValue *= 1f + (upgradeValue / 100f);
			else
				baseValue += upgradeValue;
		}

		return baseValue;
	}

    public override string ToString()
    {
        string result = "";
		foreach (KeyValuePair<Stat, float> stat in dynamicStats)
		{
			result += $"{stat.Key}: {stat.Value}\n";
		}
		result += "\n";
		foreach (KeyValuePair<Stat, float> stat in staticStats)
		{
			result += $"{stat.Key}: {stat.Value}\n";
		}
		return result.TrimEnd('\r', '\n');
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
	LifeStealRatio,
	ProjectileSpeed,
	ProjectileTrackingRigidity,
	ProjectileLifeTime
}