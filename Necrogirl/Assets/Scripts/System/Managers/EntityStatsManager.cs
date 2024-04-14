using System.Collections.Generic;
using UnityEngine;

public class EntityStatsManager : MonoBehaviour
{
    [Header("Stats to Monitor"), Space]
    [SerializeField] private List<Stats> monitoredStats = new List<Stats>();

    private void Start()
    {
        monitoredStats.ForEach(stats => stats.ClearUpgrades());
    }
}