using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform container;
    public List<GameObject> enemies = new List<GameObject>();
    public Vector2Int spawnCount;
    public float range;

    private void Awake()
    {
        int count = Random.Range(spawnCount.x, spawnCount.y);
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * range;

            GameObject prefab = enemies[Random.Range(0, enemies.Count)];
            GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);
            enemy.name = prefab.name;
            enemy.transform.SetParent(container);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}